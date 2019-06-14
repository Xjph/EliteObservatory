using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace EDDisco
{
    static class EDDisco
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //JObject test = JObject.Parse("{ \"timestamp\":\"2019 - 06 - 05T01: 41:44Z\", \"event\":\"Scan\", \"ScanType\":\"Detailed\", \"BodyName\":\"Plaa Fleau QS-J c22-0 5 a\", \"BodyID\":8, \"Parents\":[ {\"Planet\":7}, {\"Star\":0} ], \"DistanceFromArrivalLS\":3068.617188, \"TidalLock\":false, \"TerraformState\":\"\", \"PlanetClass\":\"Icy body\", \"Atmosphere\":\"\", \"AtmosphereType\":\"None\", \"Volcanism\":\"\", \"MassEM\":0.039342, \"Radius\":2984278.000000, \"SurfaceGravity\":1.760719, \"SurfaceTemperature\":67.210930, \"SurfacePressure\":90.113243, \"Landable\":true, \"Materials\":[ { \"Name\":\"sulphur\", \"Percent\":27.437029 }, { \"Name\":\"carbon\", \"Percent\":23.071699 }, { \"Name\":\"phosphorus\", \"Percent\":14.770898 }, { \"Name\":\"iron\", \"Percent\":11.999499 }, { \"Name\":\"nickel\", \"Percent\":9.075918 }, { \"Name\":\"chromium\", \"Percent\":5.396573 }, { \"Name\":\"germanium\", \"Percent\":3.174578 }, { \"Name\":\"vanadium\", \"Percent\":2.946659 }, { \"Name\":\"cadmium\", \"Percent\":0.931815 }, { \"Name\":\"antimony\", \"Percent\":0.671342 }, { \"Name\":\"mercury\", \"Percent\":0.523998 } ], \"Composition\":{ \"Ice\":0.878241, \"Rock\":0.104724, \"Metal\":0.017035 }, \"SemiMajorAxis\":2443254784.000000, \"Eccentricity\":0.000000, \"OrbitalInclination\":-37.623867, \"Periapsis\":149.435867, \"OrbitalPeriod\":53715820.000000, \"RotationPeriod\":176866.078125, \"AxialTilt\":-0.354162, \"WasDiscovered\":false, \"WasMapped\":false }");
            //ScanEvent scanEvent;
            //scanEvent = test.ToObject<ScanEvent>();
            //Properties.Settings.Default.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EDDiscoFrm());
        }
    }
}
