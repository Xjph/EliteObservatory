using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDisco
{
    class ScanReader
    {
        private readonly ScanEvent scanEvent;
        private readonly Dictionary<(string, long), ScanEvent> scanHistory;
        private readonly string currentSystem;
        private readonly bool isRing;
        public List<(string,string,string)> Interest { get; private set; }
        
        public ScanReader (ScanEvent scanEvent, Dictionary<(string,long),ScanEvent> scanHistory, string currentSystem)
        {
            this.scanEvent = scanEvent;
            this.scanHistory = scanHistory;
            this.currentSystem = currentSystem;
            isRing = scanEvent.BodyName.Contains(" Ring");
            Interest = new List<(string,string,string)>();
        }

        public bool IsInteresting()
        {
            bool interesting = DefaultInterest() || CustomInterest();
            if (Interest.Count() == 0)
            {
                Interest.Add((scanEvent.BodyName, "Uninteresting", string.Empty));
            }
            return interesting;
        }

        private bool DefaultInterest()
        {
            // Landable and has a terraform state
            if (scanEvent.Landable.GetValueOrDefault(false) && scanEvent.TerraformState.Length > 0)
            {
                Interest.Add((scanEvent.BodyName, $"Landable and {scanEvent.TerraformState}", string.Empty));
            }

            // Landable with atmosphere. Futureproofing!
            if (scanEvent.Landable.GetValueOrDefault(false) && scanEvent.Atmosphere.Length > 0)
            {
                Interest.Add((scanEvent.BodyName, "Landable with Atmosphere?!", string.Empty));
            }

            // Landable high-g
            if (scanEvent.Landable.GetValueOrDefault(false) && scanEvent.SurfaceGravity > 29.4)
            {
                Interest.Add((scanEvent.BodyName, "Landable with High Gravity", $"Surface gravity: {((double)scanEvent.SurfaceGravity / 9.81).ToString("0.00")}g"));
            }

            //Parent relative checks
            if ((scanEvent.Parent?[0].Item1 == "Planet" || scanEvent.Parent?[0].Item1 == "Star") &&
                !isRing && scanHistory.ContainsKey((currentSystem, scanEvent.Parent[0].Item2)))
            {
                ScanEvent parent = scanHistory[(currentSystem, scanEvent.Parent[0].Item2)];

                //Close orbit
                if (parent.Radius * 3 > scanEvent.SemiMajorAxis)
                {
                    Interest.Add((scanEvent.BodyName, "Close orbit relative to parent body size", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Parent radius: {Math.Truncate((double)parent.Radius / 1000):N0}km"));
                }

                //Body inside ring
                if (parent.Rings?.Last().OuterRad > scanEvent.SemiMajorAxis && !parent.Rings.Last().Name.Contains(" Belt"))
                {
                    Interest.Add((scanEvent.BodyName, "Orbit closer than outermost ring edge", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Ring radius: {Math.Truncate((double)parent.Rings.Last().OuterRad / 1000):N0}km"));
                }

            }

            // Close binary pair
            if (scanEvent.Parent?[0].Item1 == "Null" && scanEvent.Radius / scanEvent.SemiMajorAxis > 0.5)
            {
                var binaryPartner = scanHistory.Where(system => system.Key.Item1 == currentSystem && scanEvent.Parent?[0].Item2 == system.Value.Parent?[0].Item2 && scanEvent.BodyId != system.Value.BodyId);
                if (binaryPartner.Count() == 1)
                {
                    if (binaryPartner.First().Value.Radius / binaryPartner.First().Value.SemiMajorAxis > 0.5)
                    {
                        if (binaryPartner.First().Value.Radius + scanEvent.Radius >= binaryPartner.First().Value.SemiMajorAxis * (1 - binaryPartner.First().Value.Eccentricity) + scanEvent.SemiMajorAxis * (1 - scanEvent.Eccentricity))
                        {
                            Interest.Add((scanEvent.BodyName, "COLLIDING binary", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Radius: {Math.Truncate((double)scanEvent.Radius / 1000):N0}km, Partner: {binaryPartner.First().Value.BodyName}"));
                        }
                        else
                        {
                            Interest.Add((scanEvent.BodyName, "Close binary relative to body size", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Radius: {Math.Truncate((double)scanEvent.Radius / 1000):N0}km, Partner: {binaryPartner.First().Value.BodyName}"));
                        }
                    }
                }
            }

            // Moon of a moon
            if (scanEvent.Parent?.Count() > 1 && scanEvent.Parent[0].Item1 == "Planet" && scanEvent.Parent[1].Item1 == "Planet")
            {
                Interest.Add((scanEvent.BodyName, "Nested Moon", string.Empty));
            }

            // Tiny object
            if (scanEvent.StarType == null && scanEvent.Radius < 300000 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Small Body", $"Radius: {Math.Truncate((double)scanEvent.Radius / 1000)}km"));
            }

            // Fast rotation
            if (scanEvent.RotationPeriod != null && !scanEvent.TidalLock.GetValueOrDefault(true) && Math.Abs((double)scanEvent.RotationPeriod) < 28800 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Non-locked body with fast rotation", $"Rotational period: {Math.Abs(Math.Round((decimal)scanEvent.RotationPeriod / 3600, 1))} hours"));
            }

            // Fast orbit
            if (scanEvent.OrbitalPeriod != null && Math.Abs((double)scanEvent.OrbitalPeriod) < 28800 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Fast orbit", $"Orbital Period: {Math.Abs(Math.Round((decimal)scanEvent.OrbitalPeriod / 3600, 1))} hours"));
            }

            // High eccentricity
            if (scanEvent.Eccentricity > 0.9)
            {
                Interest.Add((scanEvent.BodyName, "Highly eccentric orbit", $"Eccentricity: {Math.Round((decimal)scanEvent.Eccentricity, 2)}"));
            }

            // Good jumponium material availability
            if (scanEvent.Landable.GetValueOrDefault(false))
            {
                int jumpMats = 0;
                string matsNotFound = "carbongermaniumarsenicniobiumyttriumpolonium";
                foreach (MaterialComposition material in scanEvent.Materials)
                {
                    switch (material.Name.ToLower())
                    {
                        case "carbon":
                        case "germanium":
                        case "arsenic":
                        case "niobium":
                        case "yttrium":
                        case "polonium":
                            jumpMats++;
                            matsNotFound = matsNotFound.Replace(material.Name.ToLower(), string.Empty);
                            break;
                    }
                }
                if (jumpMats == 6)
                {
                    Interest.Add((scanEvent.BodyName, "One stop jumponium shop", string.Empty));
                }
                else if (jumpMats == 5)
                {
                    Interest.Add((scanEvent.BodyName, "5 out of 6 jumponium materials", $"Missing material: {matsNotFound}"));
                }
            }

            // Add note if multiple checks triggered
            if (Interest.Count() > 1)
            {
                Interest.Add((scanEvent.BodyName, $"{(Interest.Count() > 2 ? "Very":"More")} Interesting Object", string.Empty));
            }

            // Check history to determine if all jumponium materials available in system
            if (scanEvent.Landable.GetValueOrDefault(false))
            {
                string matsNotFound = "carbongermaniumarsenicniobiumyttriumpolonium";
                foreach (var scan in scanHistory)
                {
                    if (scan.Key.Item1 == currentSystem && scan.Value.Landable.GetValueOrDefault(false))
                    {
                        foreach (MaterialComposition material in scan.Value.Materials)
                        {
                            switch (material.Name.ToLower())
                            {
                                case "carbon":
                                case "germanium":
                                case "arsenic":
                                case "niobium":
                                case "yttrium":
                                case "polonium":
                                    matsNotFound = matsNotFound.Replace(material.Name.ToLower(), string.Empty);
                                    break;
                            }
                            if (matsNotFound.Length == 0)
                            {
                                Interest.Add((currentSystem, "All jumponium materials in system", string.Empty));
                                break;
                            }
                        }
                    }
                }
            }

            return Interest.Count > 0;
        }

        private bool CustomInterest()
        {
            return false;
        }
    }
}
