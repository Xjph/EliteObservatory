using System;
using Newtonsoft.Json;

namespace Observatory
{

    public partial class ScanEvent
    {
        private ParentObject[] ParentObjects;

        public string JournalEntry;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("ScanType")]
        public string ScanType { get; set; }

        [JsonProperty("BodyName")]
        public string BodyName { get; set; }

        [JsonProperty("BodyID")]
        public long BodyId { get; set; }

        [JsonProperty("Parents", NullValueHandling = NullValueHandling.Ignore)]
        public ParentObject[] Parents {
            get
            {
                return ParentObjects;
            }
            set
            {
                ParentObjects = value;
                Parent = new (string ParentType, long Body)[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Null != null)
                    {
                        Parent[i] = ("Null", (long)value[i].Null);
                    }
                    else if (value[i].Planet != null)
                    {
                        Parent[i] = ("Planet", (long)value[i].Planet);
                    }
                    else if (value[i].Star != null)
                    {
                        Parent[i] = ("Star", (long)value[i].Star);
                    }
                } 
            }
        }

        public (string ParentType,long Body)[] Parent { get; private set; }

        [JsonProperty("DistanceFromArrivalLS")]
        public double DistanceFromArrivalLs { get; set; }

        [JsonProperty("TidalLock", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TidalLock { get; set; }

        [JsonProperty("TerraformState", NullValueHandling = NullValueHandling.Ignore)]
        public string TerraformState { get; set; }

        [JsonProperty("PlanetClass", NullValueHandling = NullValueHandling.Ignore)]
        public string PlanetClass { get; set; }

        [JsonProperty("Atmosphere", NullValueHandling = NullValueHandling.Ignore)]
        public string Atmosphere { get; set; }

        [JsonProperty("AtmosphereType", NullValueHandling = NullValueHandling.Ignore)]
        public string AtmosphereType { get; set; }

        [JsonProperty("AtmosphereComposition", NullValueHandling = NullValueHandling.Ignore)]
        public MaterialComposition[] AtmosphereComposition { get; set; }

        [JsonProperty("Volcanism", NullValueHandling = NullValueHandling.Ignore)]
        public string Volcanism { get; set; }

        [JsonProperty("MassEM", NullValueHandling = NullValueHandling.Ignore)]
        public double? MassEm { get; set; }

        [JsonProperty("Radius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Radius { get; set; }

        [JsonProperty("SurfaceGravity", NullValueHandling = NullValueHandling.Ignore)]
        public double? SurfaceGravity { get; set; }

        [JsonProperty("SurfaceTemperature", NullValueHandling = NullValueHandling.Ignore)]
        public double? SurfaceTemperature { get; set; }

        [JsonProperty("SurfacePressure", NullValueHandling = NullValueHandling.Ignore)]
        public double? SurfacePressure { get; set; }

        [JsonProperty("Landable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Landable { get; set; }

        [JsonProperty("Materials", NullValueHandling = NullValueHandling.Ignore)]
        public MaterialComposition[] Materials { get; set; }

        [JsonProperty("Composition", NullValueHandling = NullValueHandling.Ignore)]
        public Composition Composition { get; set; }

        [JsonProperty("SemiMajorAxis", NullValueHandling = NullValueHandling.Ignore)]
        public long? SemiMajorAxis { get; set; }

        [JsonProperty("Eccentricity", NullValueHandling = NullValueHandling.Ignore)]
        public double? Eccentricity { get; set; }

        [JsonProperty("OrbitalInclination", NullValueHandling = NullValueHandling.Ignore)]
        public double? OrbitalInclination { get; set; }

        [JsonProperty("Periapsis", NullValueHandling = NullValueHandling.Ignore)]
        public double? Periapsis { get; set; }

        [JsonProperty("OrbitalPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public long? OrbitalPeriod { get; set; }

        [JsonProperty("RotationPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public double? RotationPeriod { get; set; }

        [JsonProperty("AxialTilt", NullValueHandling = NullValueHandling.Ignore)]
        public double? AxialTilt { get; set; }

        [JsonProperty("Rings", NullValueHandling = NullValueHandling.Ignore)]
        public Ring[] Rings { get; set; }

        [JsonProperty("ReserveLevel", NullValueHandling = NullValueHandling.Ignore)]
        public string ReserveLevel { get; set; }

        [JsonProperty("StarType", NullValueHandling = NullValueHandling.Ignore)]
        public string StarType { get; set; }

        [JsonProperty("Subclass", NullValueHandling = NullValueHandling.Ignore)]
        public long? Subclass { get; set; }

        [JsonProperty("StellarMass", NullValueHandling = NullValueHandling.Ignore)]
        public double? StellarMass { get; set; }

        [JsonProperty("AbsoluteMagnitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? AbsoluteMagnitude { get; set; }

        [JsonProperty("Age_MY", NullValueHandling = NullValueHandling.Ignore)]
        public long? Age_MY { get; set; }

        [JsonProperty("Luminosity", NullValueHandling = NullValueHandling.Ignore)]
        public string Luminosity { get; set; }

        [JsonProperty("WasDiscovered")]
        public bool WasDiscovered { get; set; }

        [JsonProperty("WasMapped")]
        public bool WasMapped { get; set; }

        public string GetStarTypeFullName()
        {
            string name;

            switch (StarType?.ToLower())
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
    }

    public partial class MaterialComposition
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Percent")]
        public double Percent { get; set; }
    }

    public partial class Composition
    {
        [JsonProperty("Ice")]
        public double Ice { get; set; }

        [JsonProperty("Rock")]
        public double Rock { get; set; }

        [JsonProperty("Metal")]
        public double Metal { get; set; }
    }

    public partial class ParentObject
    {
        [JsonProperty("Star", NullValueHandling = NullValueHandling.Ignore)]
        public long? Star { get; set; }

        [JsonProperty("Null", NullValueHandling = NullValueHandling.Ignore)]
        public long? Null { get; set; }

        [JsonProperty("Ring", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ring { get; set; }

        [JsonProperty("Planet", NullValueHandling = NullValueHandling.Ignore)]
        public long? Planet { get; set; }
    }

    public partial class Ring
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("RingClass")]
        public string RingClass { get; set; }

        [JsonProperty("MassMT", NullValueHandling = NullValueHandling.Ignore)]
        public long? MassMT { get; set; }

        [JsonProperty("InnerRad", NullValueHandling = NullValueHandling.Ignore)]
        public long? InnerRad { get; set; }

        [JsonProperty("OuterRad", NullValueHandling = NullValueHandling.Ignore)]
        public long? OuterRad { get; set; }
    }
}
