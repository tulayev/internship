using System.Collections.Generic;
using System.Globalization;

namespace Task6.Utils
{
    public static class Locales
    {
        public static string[] Languages { get; } = new string[] { "ru", "en", "fr" };

        public static CultureInfo[] GetCultures { get; } = new CultureInfo[]
        {
            new CultureInfo(Languages[0]),
            new CultureInfo(Languages[1]),
            new CultureInfo(Languages[2])
        };

        public static Dictionary<string, string> Alphabet { get; } = new Dictionary<string, string>
        {
            { Languages[0], "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя" },
            { Languages[1], "AaBbCcDdEeFfGgHhIjJkKLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" },
            { Languages[2], "AaBbCcDdEeFfGgHhIjJkKLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" },
        };
    }
}
