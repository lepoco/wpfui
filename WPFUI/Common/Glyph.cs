using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.Common
{
    public class Glyph
    {
        public static readonly char
            Play16 = '\0';

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="char"/> based on the ID, if <see langword="null"/> or error, returns <see cref="Glyph.Play16"/>
        /// </summary>
        public static char ToGlyph(Common.Icon? icon)
        {
            if (icon == null)
                return Glyph.Play16;

            char? character = typeof(Glyph).GetField(icon.ToString()).GetValue(null) as char?;

            if (character == null)
                return Glyph.Play16;
            else
                return (char)character;
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="string"/> based on the ID, if <see langword="null"/> or error, returns <see cref="Glyph.Play16"/>
        /// </summary>
        public static string ToString(Common.Icon? icon)
        {
            return Glyph.ToGlyph(icon).ToString();
        }
    }
}
