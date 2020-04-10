using System;
using System.Collections.Generic;
using System.Linq;

namespace Observatory
{
    class ScanReader
    {

        private readonly bool isRing;
        public List<(string BodyName, string Description, string Detail)> Interest { get; private set; }
        private readonly Properties.Observatory settings;
        private LogMonitor logMonitor;

        private readonly Materials PremiumBoostMaterials = 
            Materials.Carbon | Materials.Germanium | Materials.Arsenic | 
            Materials.Niobium | Materials.Yttrium | Materials.Polonium;

        private readonly Materials GoldSystemMaterials = 
            Materials.Antimony | Materials.Arsenic |  Materials.Cadmium | Materials.Carbon |
            Materials.Chromium | Materials.Germanium | Materials.Iron | Materials.Manganese |
            Materials.Mercury | Materials.Molybdenum | Materials.Nickel | Materials.Niobium |
            Materials.Phosphorus | Materials.Polonium | Materials.Ruthenium | Materials.Selenium | 
            Materials.Sulphur | Materials.Technetium | Materials.Tellurium | Materials.Tin | 
            Materials.Tungsten | Materials.Vanadium | Materials.Yttrium | Materials.Zinc | 
            Materials.Zirconium;

        public ScanReader(LogMonitor logMonitor)
        {
            this.logMonitor = logMonitor;
            settings = Properties.Observatory.Default;
            Interest = new List<(string BodyName, string Description, string Detail)>();
            isRing = logMonitor.LastScan.BodyName.Contains(" Ring");
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
            if (!logMonitor.GoldSystemReported && (settings.AllJumpSystem || settings.AllMaterialSystem) && logMonitor.LastScan.Landable.GetValueOrDefault(false))
            {
                Materials matsFound = Materials.None;

                foreach (var scan in logMonitor.SystemBody.Where(scan => scan.Key.System == logMonitor.CurrentSystem && scan.Value.Landable.GetValueOrDefault(false)))
                {
                    foreach (MaterialComposition material in scan.Value.Materials)
                    {
                        matsFound |= (Materials)MaterialLookup.ByName(material.Name.ToLower()); //(Materials)Enum.Parse(typeof(Materials), material.Name, true);
                    }

                    if (settings.AllMaterialSystem && !logMonitor.GoldSystemReported && (matsFound & GoldSystemMaterials) == GoldSystemMaterials)
                    {
                        interesting = true;
                        logMonitor.GoldSystemReported = true;
                        Interest.Add((logMonitor.CurrentSystem, $"All surface materials in system", string.Empty));
                    }
                    else if (settings.AllJumpSystem && !logMonitor.JumponiumReported && (matsFound & PremiumBoostMaterials) == PremiumBoostMaterials)
                    {
                        interesting = true;
                        logMonitor.JumponiumReported = true;
                        Interest.Add((logMonitor.CurrentSystem, $"All {settings.FSDBoostSynthName} materials in system", string.Empty));
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
                Interest.Add((scanEvent.BodyName, "Highly eccentric orbit", $"Eccentricity: {Math.Round((decimal)scanEvent.Eccentricity, 4)}"));
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
                    Materials matFound = (Materials)MaterialLookup.ByName(material.Name.ToLower()); //(Materials)Enum.Parse(typeof(Materials), material.Name, true);

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

            // Interesting secondary stars
            if ((settings.SecondaryStars && scanEvent.BodyId > 0 && !string.IsNullOrEmpty(scanEvent.StarType) && scanEvent.DistanceFromArrivalLs > 10))
            {
                var excludeTypes = new List<string>() { "O", "B", "A", "F", "G", "K", "M", "L", "T", "Y", "TTS" };
                if (!excludeTypes.Contains(scanEvent.StarType.ToUpper()))
                {
                    Interest.Add((scanEvent.BodyName, "Uncommon Secondary Star Type", $"{StarName(scanEvent.StarType)}, Distance: {scanEvent.DistanceFromArrivalLs:N0}Ls"));
                }
            }

            return Interest.Count > 0;
        }

        private string StarName(string starType)
        {
            string name;

            switch (starType.ToLower())
            {
                case "b_bluewhitesupergiant":
                    name = "B Blue-White Supergiant";
                    break;
                case "a_bluewhitesupergiant":
                    name = "A Blue-White Supergiant";
                    break;
                case "f_whitesupergiant":
                    name = "F White Supergiant";
                    break;
                case "g_whitesupergiant":
                    name = "G White Supergiant";
                    break;
                case "k_orangegiant":
                    name = "K Orange Giant";
                    break;
                case "m_redgiant":
                    name = "M Red Giant";
                    break;
                case "m_redsupergiant":
                    name = "M Red Supergiant";
                    break;
                case "aebe":
                    name = "Herbig Ae/Be";
                    break;
                case "w":
                case "wn":
                case "wnc":
                case "wc":
                case "wo":
                    name = "Wolf-Rayet";
                    break;
                case "c":
                case "cs":
                case "cn":
                case "cj":
                case "ch":
                case "chd":
                    name = "Carbon Star";
                    break;
                case "s":
                    name = "S-Type Star";
                    break;
                case "ms":
                    name = "MS-Type Star";
                    break;
                case "d":
                case "da":
                case "dab":
                case "dao":
                case "daz":
                case "dav":
                case "db":
                case "dbz":
                case "dbv":
                case "do":
                case "dov":
                case "dq":
                case "dc":
                case "dcv":
                case "dx":
                    name = "White Dwarf";
                    break;
                case "n":
                    name = "Neutron Star";
                    break;
                case "h":
                    name = "Black Hole";
                    break;
                case "supermassiveblackhole":
                    name = "Supermassive Black Hole";
                    break;
                case "x":
                    name = "Exotic Star";
                    break;
                case "rogueplanet":
                    name = "Rogue Planet";
                    break;
                default:
                    name = "Unknown Star Type";
                    break;
            }

            return name;
        }

        private bool CustomInterest()
        {
            return logMonitor.UserInterest.ProcessCriteria(logMonitor.LastScan, Interest, logMonitor.SystemBody, logMonitor.CurrentSystem);
        }
    }
}
