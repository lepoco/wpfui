// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WPFUI.Theme;

namespace WPFUI.Common
{
    /// <summary>
    /// Collection of available languages.
    /// </summary>
    internal enum SyntaxLanguage
    {
        Autodetect,
        XAML,
        CSHARP
    }

    // TODO: This class is work in progress.

    /// <summary>
    /// Formats a string of code into <see cref="System.Windows.Controls.TextBox"/> control.
    /// <para>Implementation and regex patterns inspired by <see href="https://github.com/antoniandre/simple-syntax-highlighter"/>.</para>
    /// </summary>
    internal static class Syntax
    {
        private const string EndlinePattern = /* language=regex */ "(\n)";

        private const string TabPattern = /* language=regex */ "(\t)";

        private const string QuotePattern = /* language=regex */ "(\"(?:\\\"|[^\"])*\")|('(?:\\'|[^'])*')";

        private const string CommentPattern = /* language=regex */ @"(\/\/.*?(?:\n|$)|\/\*.*?\*\/)";

        private const string TagPattern = /* language=regex */ @"(<\/?)([a-zA-Z\-:]+)(.*?)(\/?>)";

        private const string EntityPattern = /* language=regex */ @"(&[a-zA-Z0-9#]+;)";

        private const string PunctuationPattern = /* language=regex */ @"(!==?|(?:[[\\] (){}.:;,+\\-?=!]|&lt;|&gt;)+|&&|\\|\\|)";

        private const string NumberPattern = /* language=regex */ @"(-? (?:\.\d+|\d+(?:\.\d+)?))";

        private const string BooleanPattern = /* language=regex */ "\b(true|false)\b";

        private const string AttributePattern = /* language=regex */ "(\\s*)([a-zA-Z\\d\\-:]+)=(\" | ')(.*?)\\3";

        public static TextBlock Format(object code)
        {
            return Format(code as string ?? String.Empty);
        }

        public static TextBlock Format(string code, SyntaxLanguage language = SyntaxLanguage.Autodetect)
        {
            TextBlock returnText = new TextBlock();
            Regex rgx = new(GetPattern(language, code));

            Group codeMatched;
            bool lightTheme = IsLightTheme();

            foreach (Match match in rgx.Matches(code))
            {
                foreach (object group in match.Groups)
                {
                    // Remove whole matches
                    if (group is Match) continue;

                    // Cast to group
                    codeMatched = (Group)group;

                    // Remove empty groups
                    if (String.IsNullOrEmpty(codeMatched.Value)) continue;

                    if (codeMatched.Value.Contains("\n"))
                    {
                        returnText.Inlines.Add(Line("\n", Brushes.Transparent));
                    }
                    if (codeMatched.Value.Contains("\t"))
                    {
                        returnText.Inlines.Add(Line("\t", Brushes.Transparent));
                    }
                    else if (codeMatched.Value.Contains("/*") || codeMatched.Value.Contains("//"))
                    {
                        returnText.Inlines.Add(Line(codeMatched.Value, Brushes.Orange));
                    }
                    else if (codeMatched.Value.Contains("<") || codeMatched.Value.Contains(">"))
                    {
                        returnText.Inlines.Add(Line(codeMatched.Value, lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                    }
                    else if (codeMatched.Value.Contains("\""))
                    {
                        string[] attributeArray = codeMatched.Value.Split('"');

                        if (attributeArray.Length == 3)
                        {
                            returnText.Inlines.Add(Line(attributeArray[0], lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                            returnText.Inlines.Add(Line("\"", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            returnText.Inlines.Add(Line(attributeArray[1], Brushes.Coral));
                            returnText.Inlines.Add(Line("\"", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            returnText.Inlines.Add(Line(attributeArray[2], lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                        }
                        else
                        {
                            returnText.Inlines.Add(Line(codeMatched.Value, lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                        }
                    }
                    else if (codeMatched.Value.Contains("'"))
                    {
                        string[] attributeArray = codeMatched.Value.Split('\'');

                        if (attributeArray.Length == 3)
                        {
                            returnText.Inlines.Add(Line(attributeArray[0], lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                            returnText.Inlines.Add(Line("'", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            returnText.Inlines.Add(Line(attributeArray[1], Brushes.Coral));
                            returnText.Inlines.Add(Line("'", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            returnText.Inlines.Add(Line(attributeArray[2], lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                        }
                        else
                        {
                            returnText.Inlines.Add(Line(codeMatched.Value, lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                        }
                    }
                    else
                    {
                        returnText.Inlines.Add(Line(codeMatched.Value, lightTheme ? Brushes.CornflowerBlue : Brushes.Aqua));
                    }
                }
            }

            return returnText;
        }

        public static string Clean(string code)
        {
            code = code.Replace(@"\n", "\n");
            code = code.Replace(@"\t", "\t");
            code = code.Replace("&lt;", "<");
            code = code.Replace("&gt;", ">");
            code = code.Replace("&amp;", "&");
            code = code.Replace("&quot;", "\"");
            code = code.Replace("&apos;", "'");

            return code;
        }

        private static Run Line(string line, SolidColorBrush brush)
        {
            return new Run(line) { Foreground = brush };
        }

        private static bool IsLightTheme()
        {
            Style theme = Manager.Current;

            return !(theme == Style.Dark || theme == Style.Glow || theme == Style.CapturedMotion);
        }

        private static string GetPattern(SyntaxLanguage language, string code = "")
        {
            string pattern = "";

            if (language == SyntaxLanguage.Autodetect)
            {
                // TODO: Autodected
                language = SyntaxLanguage.XAML;
            }

            switch (language)
            {
                case SyntaxLanguage.CSHARP:
                    pattern += EndlinePattern;
                    pattern += "|" + TabPattern;
                    pattern += "|" + QuotePattern;
                    pattern += "|" + CommentPattern;
                    pattern += "|" + EntityPattern;
                    pattern += "|" + TagPattern;
                    break;

                case SyntaxLanguage.XAML:
                    pattern += EndlinePattern;
                    pattern += "|" + TabPattern;
                    pattern += "|" + QuotePattern;
                    pattern += "|" + CommentPattern;
                    pattern += "|" + EntityPattern;
                    pattern += "|" + TagPattern;
                    break;
            }

            return pattern;
        }
    }
}