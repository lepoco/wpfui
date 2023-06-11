// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Syntax;

// TODO: This class is work in progress.

/// <summary>
/// Formats a string of code into <see cref="System.Windows.Controls.TextBox"/> control.
/// <para>Implementation and regex patterns inspired by <see href="https://github.com/antoniandre/simple-syntax-highlighter"/>.</para>
/// </summary>
internal static class Highlighter
{
    private const string EndlinePattern = /* language=regex */ "(\n)";

    private const string TabPattern = /* language=regex */ "(\t)";

    private const string QuotePattern = /* language=regex */ "(\"(?:\\\"|[^\"])*\")|('(?:\\'|[^'])*')";

    private const string CommentPattern = /* language=regex */ @"(\/\/.*?(?:\n|$)|\/\*.*?\*\/)";

    private const string TagPattern = /* language=regex */ @"(<\/?)([a-zA-Z\-:]+)(.*?)(\/?>)";

    private const string EntityPattern = /* language=regex */ @"(&[a-zA-Z0-9#]+;)";

    private const string PunctuationPattern = /* language=regex */
        @"(!==?|(?:[[\\] ()\{\}.:;,+\\-?=!]|&lt;|&gt;)+|&&|\\|\\|)";

    private const string NumberPattern = /* language=regex */ @"(-? (?:\.\d+|\d+(?:\.\d+)?))";

    private const string BooleanPattern = /* language=regex */ "\b(true|false)\b";

    private const string AttributePattern = /* language=regex */ "(\\s*)([a-zA-Z\\d\\-:]+)=(\" | ')(.*?)\\3";

    public static Paragraph FormatAsParagraph(string code, SyntaxLanguage language = SyntaxLanguage.Autodetect)
    {
        var paragraph = new Paragraph();
        Regex rgx = new(GetPattern(language, code));

        bool lightTheme = IsLightTheme();

        foreach (Match match in rgx.Matches(code))
        {
            foreach (object group in match.Groups)
            {
                // Remove whole matches
                if (group is Match)
                    continue;

                // Cast to group
                Group codeMatched = (Group)group;

                // Remove empty groups
                if (String.IsNullOrEmpty(codeMatched.Value))
                    continue;

                if (codeMatched.Value.Contains("\t"))
                {
                    paragraph.Inlines.Add(Line("  ", Brushes.Transparent));
                }
                else if (codeMatched.Value.Contains("/*") || codeMatched.Value.Contains("//"))
                {
                    paragraph.Inlines.Add(Line(codeMatched.Value, Brushes.Orange));
                }
                else if (codeMatched.Value.Contains("<") || codeMatched.Value.Contains(">"))
                {
                    paragraph.Inlines.Add(Line(codeMatched.Value,
                        lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                }
                else if (codeMatched.Value.Contains("\""))
                {
                    string[] attributeArray = codeMatched.Value.Split('"');
                    attributeArray = attributeArray.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();

                    if (attributeArray.Length % 2 == 0)
                    {
                        for (int i = 0; i < attributeArray.Length; i += 2)
                        {
                            paragraph.Inlines.Add(Line(attributeArray[i],
                                lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                            paragraph.Inlines.Add(Line("\"",
                                lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            paragraph.Inlines.Add(Line(attributeArray[i + 1], Brushes.Coral));
                            paragraph.Inlines.Add(Line("\"",
                                lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                        }
                    }
                    else
                    {
                        paragraph.Inlines.Add(Line(codeMatched.Value,
                            lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                    }
                }
                else if (codeMatched.Value.Contains("'"))
                {
                    string[] attributeArray = codeMatched.Value.Split('\'');
                    attributeArray = attributeArray.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();

                    if (attributeArray.Length % 2 == 0)
                    {
                        for (int i = 0; i < attributeArray.Length; i += 2)
                        {
                            paragraph.Inlines.Add(Line(attributeArray[i],
                                lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                            paragraph.Inlines.Add(
                                Line("'", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                            paragraph.Inlines.Add(Line(attributeArray[i + 1], Brushes.Coral));
                            paragraph.Inlines.Add(
                                Line("'", lightTheme ? Brushes.DarkCyan : Brushes.CornflowerBlue));
                        }
                    }
                    else
                    {
                        paragraph.Inlines.Add(Line(codeMatched.Value,
                            lightTheme ? Brushes.DarkSlateGray : Brushes.WhiteSmoke));
                    }
                }
                else
                {
                    paragraph.Inlines.Add(Line(codeMatched.Value,
                        lightTheme ? Brushes.CornflowerBlue : Brushes.Aqua));
                }
            }
        }

        return paragraph;
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
        return Appearance.Theme.GetAppTheme() == ThemeType.Light;
    }

    private static string GetPattern(SyntaxLanguage language)
    {
        return GetPattern(language, String.Empty);
    }

    private static string GetPattern(SyntaxLanguage language, string code)
    {
        var pattern = String.Empty;

        // TODO: Auto detected
        if (language == SyntaxLanguage.Autodetect)
            language = SyntaxLanguage.XAML;

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
