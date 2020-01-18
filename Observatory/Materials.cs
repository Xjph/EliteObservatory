using System;
using System.Collections.Generic;
using System.Linq;

namespace Observatory
{
    [Flags]
    enum Materials
    {
        None =          0,
        Antimony =      0b0000000000000000000000000001,
        Arsenic =       0b0000000000000000000000000010,
        Boron =         0b0000000000000000000000000100,
        Cadmium =       0b0000000000000000000000001000,
        Carbon =        0b0000000000000000000000010000,
        Chromium =      0b0000000000000000000000100000,
        Germanium =     0b0000000000000000000001000000,
        Iron =          0b0000000000000000000010000000,
        Lead =          0b0000000000000000000100000000,
        Manganese =     0b0000000000000000001000000000,
        Mercury =       0b0000000000000000010000000000,
        Molybdenum =    0b0000000000000000100000000000,
        Nickel =        0b0000000000000001000000000000,
        Niobium =       0b0000000000000010000000000000,
        Phosphorus =    0b0000000000000100000000000000,
        Polonium =      0b0000000000001000000000000000,
        Rhenium =       0b0000000000010000000000000000,
        Ruthenium =     0b0000000000100000000000000000,
        Selenium =      0b0000000001000000000000000000,
        Sulphur =       0b0000000010000000000000000000,
        Technetium =    0b0000000100000000000000000000,
        Tellurium =     0b0000001000000000000000000000,
        Tin =           0b0000010000000000000000000000,
        Tungsten =      0b0000100000000000000000000000,
        Vanadium =      0b0001000000000000000000000000,
        Yttrium =       0b0010000000000000000000000000,
        Zinc =          0b0100000000000000000000000000,
        Zirconium =     0b1000000000000000000000000000,
    }

    //A dictionary lookup should be much more performant than comparing the journal text against Enum.ToString()
    public static class MaterialLookup
    {
        private static readonly Dictionary<string, int> MaterialDict = Enum.GetValues(typeof(Materials)).Cast<Materials>().ToDictionary(mat => mat.ToString().ToLower(), mat => (int)mat);

        public static int ByName(string materialName)
        {
            return MaterialDict[materialName];
        }
    }
}
