using System.Text.RegularExpressions;
using Lunari.Tsuki.Runtime;

namespace GMTK.Common {
    public static class TMPHelper {
        public static string RemoveRichText(string input) {
            if (input.IsNullOrEmpty()) return string.Empty;
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}