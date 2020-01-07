using System;
using System.Collections.Generic;
using System.Linq;

namespace Observatory
{
    class ScanReader
    {

        [Flags]
        private enum Materials
        {
            None        = 0,
            Antimony    = 0b0000000000000000000000000001,
            Arsenic     = 0b0000000000000000000000000010,
            Boron       = 0b0000000000000000000000000100,
            Cadmium     = 0b0000000000000000000000001000,
            Carbon      = 0b0000000000000000000000010000,
            Chromium    = 0b0000000000000000000000100000,
            Germanium   = 0b0000000000000000000001000000,
            Iron        = 0b0000000000000000000010000000,
            Lead        = 0b0000000000000000000100000000,
            Manganese   = 0b0000000000000000001000000000,
            Mercury     = 0b0000000000000000010000000000,
            Molybdenum  = 0b0000000000000000100000000000,
            Nickel      = 0b0000000000000001000000000000,
            Niobium     = 0b0000000000000010000000000000,
            Phosphorus  = 0b0000000000000100000000000000,
            Polonium    = 0b0000000000001000000000000000,
            Rhenium     = 0b0000000000010000000000000000,
            Ruthenium   = 0b0000000000100000000000000000,
            Selenium    = 0b0000000001000000000000000000,
            Sulphur     = 0b0000000010000000000000000000,
            Technetium  = 0b0000000100000000000000000000,
            Tellurium   = 0b0000001000000000000000000000,
            Tin         = 0b0000010000000000000000000000,
            Tungsten    = 0b0000100000000000000000000000,
            Vanadium    = 0b0001000000000000000000000000,
            Yttrium     = 0b0010000000000000000000000000,
            Zinc        = 0b0100000000000000000000000000,
            Zirconium   = 0b1000000000000000000000000000,
        }

        private readonly bool isRing;
        public List<(string BodyName, string Description, string Detail)> Interest { get; private set; }
        private readonly Properties.Observatory settings;
        private LogMonitor logMonitor;
        private readonly Materials PremiumBoostMaterials 
            = Materials.Carbon | Materials.Germanium | Materials.Arsenic | Materials.Niobium | Materials.Yttrium | Materials.Polonium;
        private readonly Dictionary<string, int> MaterialLookup = Enum.GetValues(typeof(Materials)).Cast<Materials>().ToDictionary(mat => mat.ToString().ToLower(), mat => (int)mat);


        public ScanReader(LogMonitor logMonitor)
        {
            this.logMonitor = logMonitor;
            isRing = logMonitor.LastScan.BodyName.Contains(" Ring");
            Interest = new List<(string BodyName, string Description, string Detail)>();
            settings = Properties.Observatory.Default;
        }

        public bool IsInteresting()
        {
            bool interesting = !isRing && (DefaultInterest() | CustomInterest());
            // Moved these outside the "DefaultInterest" method so the multiple criteria check would include user criteria, and so the "all jumponium" check would not be counted

            // Add note if multiple checks triggered
            if (settings.VeryInteresting && Interest.Count() > 1)
            {
                Interest.Add((logMonitor.LastScan.BodyName, "Multiple criteria met", $"{Interest.Count()} Criteria Satisfied"));
            }

            // Check history to determine if all jumponium materials available in system
            if (!logMonitor.JumponiumReported && settings.AllJumpSystem && logMonitor.LastScan.Landable.GetValueOrDefault(false))
            {
                Materials matsFound = Materials.None;
                
                foreach (var scan in logMonitor.SystemBody.Where(scan => scan.Key.System == logMonitor.CurrentSystem && scan.Value.Landable.GetValueOrDefault(false)))
                {
                    foreach (MaterialComposition material in scan.Value.Materials)
                    {
                        matsFound |= (Materials)MaterialLookup[material.Name.ToLower()]; //(Materials)Enum.Parse(typeof(Materials), material.Name, true);

                        if ((matsFound & PremiumBoostMaterials) == PremiumBoostMaterials)
                        {
                            interesting = true;
                            logMonitor.JumponiumReported = true;
                            Interest.Add((logMonitor.CurrentSystem, $"All {settings.FSDBoostSynthName} materials in system", string.Empty));
                            break;
                        }
                    }
                }
            }

            if (Interest.Count() == 0)
            {
                Interest.Add((logMonitor.LastScan.BodyName, "Uninteresting", string.Empty));
            }
            return interesting;
        }

        private bool DefaultInterest()
        {
            ScanEvent scanEvent = logMonitor.LastScan;

            // Landable and has a terraform state
            if (settings.LandWithTerra && scanEvent.Landable.GetValueOrDefault(false) && scanEvent.TerraformState.Length > 0)
            {
                Interest.Add((scanEvent.BodyName, $"Landable and {scanEvent.TerraformState}", string.Empty));
            }

            // Landable with atmosphere. Futureproofing!
            if (settings.LandWithAtmo && scanEvent.Landable.GetValueOrDefault(false) && scanEvent.Atmosphere.Length > 0)
            {
                Interest.Add((scanEvent.BodyName, "Landable with Atmosphere?!", string.Empty));
            }

            // Landable high-g
            if (settings.LandHighG && scanEvent.Landable.GetValueOrDefault(false) && scanEvent.SurfaceGravity > 29.4)
            {
                Interest.Add((scanEvent.BodyName, "Landable with High Gravity", $"Surface gravity: {((double)scanEvent.SurfaceGravity / 9.81).ToString("0.00")}g"));
            }

            // Landable large planet
            if (settings.LandLarge && scanEvent.Landable.GetValueOrDefault(false) && scanEvent.Radius > 18000000)
            {
                Interest.Add((scanEvent.BodyName, "Landable Large Planet", $"Radius: {((double)scanEvent.Radius / 1000).ToString("0")}km"));
            }

            // Rings wider than 5x body radius
            if (settings.WideRing && scanEvent.Rings?.Count() > 0)
            {
                foreach(Ring ring in scanEvent.Rings.Where(ring => !ring.Name.Contains("Belt")))
                {
                    long ringWidth = (ring.OuterRad.GetValueOrDefault(0) - ring.InnerRad.GetValueOrDefault(0));
                    if (ringWidth > scanEvent.Radius * 5)
                    {
                        Interest.Add((ring.Name, "Wide Ring", $"Width: {(double)ringWidth / 299792458:N2}Ls / {(double)ringWidth / 1000:N0}km, Parent Radius: {Math.Truncate((double)scanEvent.Radius / 1000):N0}km"));
                    }
                }
            }

            //Parent relative checks
            if ((settings.CloseOrbit || settings.ShepherdMoon || settings.RingHugger) && (scanEvent.Parent?[0].ParentType == "Planet" || scanEvent.Parent?[0].ParentType == "Star") &&
                logMonitor.SystemBody.ContainsKey((logMonitor.CurrentSystem, scanEvent.Parent[0].Body)))
            {
                ScanEvent parent = logMonitor.SystemBody[(logMonitor.CurrentSystem, scanEvent.Parent[0].Body)];

                //Close orbit
                if (settings.CloseOrbit && parent.Radius * 3 > scanEvent.SemiMajorAxis)
                {
                    Interest.Add((scanEvent.BodyName, "Close orbit relative to parent body size", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Parent radius: {Math.Truncate((double)parent.Radius / 1000):N0}km"));
                }

                //Body inside ring
                if (settings.ShepherdMoon && parent.Rings?.Last().OuterRad > scanEvent.SemiMajorAxis && !parent.Rings.Last().Name.Contains(" Belt"))
                {
                    Interest.Add((scanEvent.BodyName, "Shepherd moon", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Ring radius: {Math.Truncate((double)parent.Rings.Last().OuterRad / 1000):N0}km"));
                }

                // Moon close to ring
                if (settings.RingHugger && parent.Rings?.Count() > 0)
                {
                    foreach (var ring in parent.Rings)
                    {
                        double separation = Math.Min(Math.Abs(scanEvent.SemiMajorAxis.GetValueOrDefault(0) - ring.OuterRad.GetValueOrDefault(0)), Math.Abs(ring.InnerRad.GetValueOrDefault(0) - scanEvent.SemiMajorAxis.GetValueOrDefault(0)));
                        if (separation < scanEvent.Radius * 10)
                        {
                            Interest.Add((scanEvent.BodyName, "Close ring proximity", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Radius: {((double)scanEvent.Radius / 1000).ToString("0")}km, Distance from ring: {separation / 1000:N0}km"));
                        }
                    }
                }
            }

            // Close binary pair
            if ((settings.CloseBinary || settings.CollidingBinary) && scanEvent.Parent?[0].ParentType == "Null" && scanEvent.Radius / scanEvent.SemiMajorAxis > 0.4)
            {
                var binaryPartner = logMonitor.SystemBody.Where(system => system.Key.System == logMonitor.CurrentSystem && scanEvent.Parent?[0].Body == system.Value.Parent?[0].Body && scanEvent.BodyId != system.Value.BodyId);
                if (binaryPartner.Count() == 1)
                {
                    if (binaryPartner.First().Value.Radius / binaryPartner.First().Value.SemiMajorAxis > 0.4)
                    {
                        if (settings.CollidingBinary && binaryPartner.First().Value.Radius + scanEvent.Radius >= binaryPartner.First().Value.SemiMajorAxis * (1 - binaryPartner.First().Value.Eccentricity) + scanEvent.SemiMajorAxis * (1 - scanEvent.Eccentricity))
                        {
                            Interest.Add((scanEvent.BodyName, "COLLIDING binary", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Radius: {Math.Truncate((double)scanEvent.Radius / 1000):N0}km, Partner: {binaryPartner.First().Value.BodyName}"));
                        }
                        else if (settings.CloseBinary)
                        {
                            Interest.Add((scanEvent.BodyName, "Close binary relative to body size", $"Orbit: {Math.Truncate((double)scanEvent.SemiMajorAxis / 1000):N0}km, Radius: {Math.Truncate((double)scanEvent.Radius / 1000):N0}km, Partner: {binaryPartner.First().Value.BodyName}"));
                        }
                    }
                }
            }

            // Moon of a moon
            if (settings.NestedMoon && scanEvent.Parent?.Count() > 1 && scanEvent.Parent[0].ParentType == "Planet" && scanEvent.Parent[1].ParentType == "Planet")
            {
                Interest.Add((scanEvent.BodyName, "Nested Moon", string.Empty));
            }

            // Tiny object
            if (settings.TinyObject && scanEvent.StarType == null && scanEvent.Radius < 300000 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Small Body", $"Radius: {Math.Truncate((double)scanEvent.Radius / 1000)}km"));
            }

            // Fast rotation
            if (settings.FastRotate && scanEvent.RotationPeriod != null && !scanEvent.TidalLock.GetValueOrDefault(true) && Math.Abs((double)scanEvent.RotationPeriod) < 28800 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Non-locked body with fast rotation", $"Rotational period: {Math.Abs(Math.Round((decimal)scanEvent.RotationPeriod / 3600, 1))} hours"));
            }

            // Fast orbit
            if (settings.FastOrbit && scanEvent.OrbitalPeriod != null && Math.Abs((double)scanEvent.OrbitalPeriod) < 28800 && !isRing)
            {
                Interest.Add((scanEvent.BodyName, "Fast orbit", $"Orbital Period: {Math.Abs(Math.Round((decimal)scanEvent.OrbitalPeriod / 3600, 1))} hours"));
            }

            // High eccentricity
            if (settings.HighEccentric && scanEvent.Eccentricity > 0.9)
            {
                Interest.Add((scanEvent.BodyName, "Highly eccentric orbit", $"Eccentricity: {Math.Round((decimal)scanEvent.Eccentricity, 2)}"));
            }

            // Ringed Landable
            if (settings.RingLandable && scanEvent.Landable.GetValueOrDefault(false) && scanEvent.Rings?.Count() > 0)
            {
                Interest.Add((scanEvent.BodyName, "Ringed landable body", string.Empty));
            }

            // Good jumponium material availability
            if ((settings.AllJumpBody || settings.GoodJump) && scanEvent.Landable.GetValueOrDefault(false))
            {
                int jumpMats = 0;
                Materials matsNotFound = PremiumBoostMaterials;
                foreach (MaterialComposition material in scanEvent.Materials)
                {
                    Materials matFound = (Materials)MaterialLookup[material.Name.ToLower()]; //(Materials)Enum.Parse(typeof(Materials), material.Name, true);

                    if ((matFound & PremiumBoostMaterials) == matFound)
                    {
                        jumpMats++;
                        matsNotFound ^= matFound;
                    }

                }
                if (settings.AllJumpBody && jumpMats == 6)
                {
                    Interest.Add((scanEvent.BodyName, $"One stop {settings.FSDBoostSynthName} shop", string.Empty));
                }
                else if (settings.GoodJump && jumpMats == 5)
                {
                    Interest.Add((scanEvent.BodyName, $"5 out of 6 {settings.FSDBoostSynthName} materials", $"Missing material: {matsNotFound}"));
                }
            }

            return Interest.Count > 0;
        }

        private bool CustomInterest()
        {
            return logMonitor.UserInterest.ProcessCriteria(logMonitor.LastScan, Interest, logMonitor.SystemBody, logMonitor.CurrentSystem);
        }
    }
}
