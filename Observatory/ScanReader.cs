﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Observatory
{
    class ScanReader
    {
        private readonly ScanEvent scanEvent;
        private readonly Dictionary<(string System, long Body), ScanEvent> scanHistory;
        private readonly string currentSystem;
        private readonly bool isRing;
        private readonly UserInterest userInterest;
        public List<(string BodyName, string Description, string Detail)> Interest { get; private set; }
        private readonly Properties.Observatory settings;


        public ScanReader(LogMonitor logMonitor)
        {
            userInterest = logMonitor.UserInterest;
            scanEvent = logMonitor.LastScan;
            scanHistory = logMonitor.SystemBody;
            currentSystem = logMonitor.CurrentSystem;
            isRing = scanEvent.BodyName.Contains(" Ring");
            Interest = new List<(string BodyName, string Description, string Detail)>();
            settings = Properties.Observatory.Default;
        }

        public bool IsInteresting()
        {
            bool interesting = DefaultInterest() | CustomInterest();
            
            // Moved these outside the "DefaultInterest" method so the multiple criteria check would include user criteria, and so the "all jumponium" check would not be counted

            // Add note if multiple checks triggered
            if (settings.VeryInteresting && Interest.Count() > 1)
            {
                Interest.Add((scanEvent.BodyName, "Multiple criteria met", $"{Interest.Count()} Criteria Satisfied"));
            }

            // Check history to determine if all jumponium materials available in system
            if (settings.AllJumpSystem && scanEvent.Landable.GetValueOrDefault(false))
            {
                string matsNotFound = "carbongermaniumarsenicniobiumyttriumpolonium";
                foreach (var scan in scanHistory.Where(scan => scan.Key.System == currentSystem && scan.Value.Landable.GetValueOrDefault(false)))
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
                            interesting = true;
                            Interest.Add((currentSystem, $"All {settings.FSDBoostSynthName} materials in system", string.Empty));
                            break;
                        }
                    }
                }
            }

            if (Interest.Count() == 0)
            {
                Interest.Add((scanEvent.BodyName, "Uninteresting", string.Empty));
            }
            return interesting;
        }

        private bool DefaultInterest()
        {
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

            // Rings wider than 1 light-second
            if (settings.WideRing && scanEvent.Rings?.Count<Ring>() > 0)
            {
                foreach(Ring ring in scanEvent.Rings.Where<Ring>(ring => !ring.Name.Contains("Belt")))
                {
                    long ringWidth = (ring.OuterRad.GetValueOrDefault(0) - ring.InnerRad.GetValueOrDefault(0));
                    if (ringWidth > 299792458)
                    {
                        Interest.Add((ring.Name, "Wide Ring", $"Width: {(double)ringWidth / 299792458:N2}Ls"));
                    }
                }
            }

            //Parent relative checks
            if ((settings.CloseOrbit || settings.ShepherdMoon) && (scanEvent.Parent?[0].ParentType == "Planet" || scanEvent.Parent?[0].ParentType == "Star") &&
                !isRing && scanHistory.ContainsKey((currentSystem, scanEvent.Parent[0].Body)))
            {
                ScanEvent parent = scanHistory[(currentSystem, scanEvent.Parent[0].Body)];

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
            }

            // Close binary pair
            if ((settings.CloseBinary || settings.CollidingBinary) && scanEvent.Parent?[0].ParentType == "Null" && scanEvent.Radius / scanEvent.SemiMajorAxis > 0.5)
            {
                var binaryPartner = scanHistory.Where(system => system.Key.System == currentSystem && scanEvent.Parent?[0].Body == system.Value.Parent?[0].Body && scanEvent.BodyId != system.Value.BodyId);
                if (binaryPartner.Count() == 1)
                {
                    if (binaryPartner.First().Value.Radius / binaryPartner.First().Value.SemiMajorAxis > 0.5)
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

            // Good jumponium material availability
            if ((settings.AllJumpBody || settings.GoodJump) && scanEvent.Landable.GetValueOrDefault(false))
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
            return userInterest.CheckCriteria(scanEvent, Interest);
        }
    }
}