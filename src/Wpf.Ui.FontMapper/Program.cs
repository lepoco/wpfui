// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Net.Http.Json;
using System.Text;
using Wpf.Ui.FontMapper;

Console.WriteLine("Fluent System Icons Mapper");
System.Diagnostics.Debug.WriteLine("INFO | Fluent System Icons Mapper", "Wpf.Ui.FontMapper");

var workingDirectory = Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().Location
);

var regularIcons = new FontSource(
        "SymbolRegular",
        "Represents a list of regular Fluent System Icons <c>v.{{FLUENT_SYSTEM_ICONS_VERSION}}</c>.\n<para>May be converted to <see langword=\"char\"/> using <c>GetGlyph()</c> or to <see langword=\"string\"/> using <c>GetString()</c></para>",
        @"https://raw.githubusercontent.com/microsoft/fluentui-system-icons/main/fonts/FluentSystemIcons-Regular.json",
        "generated\\SymbolRegular.cs"
);
var filledIcons = new FontSource(
        "SymbolFilled",
        "Represents a list of filled Fluent System Icons <c>v.{{FLUENT_SYSTEM_ICONS_VERSION}}</c>.\n<para>May be converted to <see langword=\"char\"/> using <c>GetGlyph()</c> or to <see langword=\"string\"/> using <c>GetString()</c></para>",
        @"https://raw.githubusercontent.com/microsoft/fluentui-system-icons/main/fonts/FluentSystemIcons-Filled.json",
        "generated\\SymbolFilled.cs"
);

var fetchVersionAsync = async () =>
{
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

    return (await httpClient.GetFromJsonAsync<IEnumerable<GitTag>>(@"https://api.github.com/repos/microsoft/fluentui-system-icons/git/refs/tags"))?.Last()?.Ref.Replace("refs/tags/", String.Empty).Trim()
        ?? throw new Exception("Unable to parse the verison string");
};

var formatIconName = (string rawIconName) =>
{
    rawIconName = rawIconName.Replace("ic_fluent_", String.Empty)
            .Replace("_regular", String.Empty)
            .Replace("_filled", String.Empty);

    var iconName = String.Empty;

    foreach (var newPart in rawIconName.Split('_'))
    {
        var charactersArray = newPart.ToCharArray();
        charactersArray[0] = Char.ToUpper(charactersArray[0]);

        iconName += new string(charactersArray);
    }

    return iconName;
};

var fetchFontContentsAsync = async (FontSource source, string version) =>
{
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

    var sourceJsonContent = await httpClient.GetFromJsonAsync<Dictionary<string, long>>(source.SourcePath) ?? throw new Exception("Unable to obtain JSON data");

    sourceJsonContent = sourceJsonContent
        .OrderBy(x => x.Value)
        .ToDictionary(
        k => formatIconName(k.Key),
        v => (v.Value > 65535 ? v.Value - 65536 : v.Value));

    source.SetContents(sourceJsonContent);
    source.UpdateVersion(version);
};

var recentVersion = await fetchVersionAsync();

await fetchFontContentsAsync(regularIcons, recentVersion);
await fetchFontContentsAsync(filledIcons, recentVersion);

var regularKeys = regularIcons.Contents.Keys;
var filledKeys = filledIcons.Contents.Keys;
var keysToRemove = regularKeys.Except(filledKeys).Concat(filledKeys.Except(regularKeys));

foreach (var key in keysToRemove)
{
    regularIcons.Contents.Remove(key);
    filledIcons.Contents.Remove(key);

    Console.WriteLine($"Deleted key \"{key}\" because no duplicate found in all lists");
}

var writeToFileAsync = async (FontSource singleFont, string workingDirectory) =>
{
    var destinationPath = Path.Combine(workingDirectory, singleFont.DestinationPath);

    var enumName = singleFont.Name;

    var enumMapStringBuilder = new StringBuilder();

    enumMapStringBuilder.AppendLine(
        "// This Source Code Form is subject to the terms of the MIT License."
    );
    enumMapStringBuilder.AppendLine(
        "// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT."
    );
    enumMapStringBuilder.AppendLine(
        "// Copyright (C) Leszek Pomianowski and WPF UI Contributors."
    );
    enumMapStringBuilder.AppendLine("// All Rights Reserved.");
    enumMapStringBuilder.AppendLine("");
    enumMapStringBuilder.AppendLine("namespace Wpf.Ui.Common;");
    enumMapStringBuilder.AppendLine("");

    enumMapStringBuilder.AppendLine("/// <summary>");
    enumMapStringBuilder.AppendLine($"/// {singleFont.Description.Replace("\n", "\n/// ")}");
    enumMapStringBuilder.AppendLine("/// </summary>");

    enumMapStringBuilder.AppendLine("#pragma warning disable CS1591");

    enumMapStringBuilder.AppendLine("public enum " + enumName);
    enumMapStringBuilder.AppendLine("{");
    enumMapStringBuilder.AppendLine("    /// <summary>");
    enumMapStringBuilder.AppendLine(
        "    /// Actually, this icon is not empty, but makes it easier to navigate."
    );
    enumMapStringBuilder.AppendLine("    /// </summary>");
    enumMapStringBuilder.AppendLine("    Empty = 0x0,");
    enumMapStringBuilder.AppendLine("");
    enumMapStringBuilder.AppendLine("    // Automatically generated, may contain bugs.");
    enumMapStringBuilder.AppendLine("");

    foreach (var singleIcon in singleFont.Contents)
    {
        // Older versions
        if (singleIcon.Value < 32)
        {
            enumMapStringBuilder.AppendLine($"");
            enumMapStringBuilder.AppendLine($"    /// <summary>");
            enumMapStringBuilder.AppendLine($"    /// Blank icon.");
            enumMapStringBuilder.AppendLine($"    /// </summary>");
        }

        enumMapStringBuilder.AppendLine($"    {singleIcon.Key} = 0x{singleIcon.Value:X},");
    }

    enumMapStringBuilder.AppendLine("}");
    enumMapStringBuilder.AppendLine("");
    enumMapStringBuilder.AppendLine("#pragma warning restore CS1591");
    enumMapStringBuilder.AppendLine("");

    var fileInfo = new FileInfo(destinationPath);

    if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
        lock (fileInfo.Directory)
        {
            fileInfo.Directory.Create();
        }

    File.WriteAllText(destinationPath, enumMapStringBuilder.ToString());
};

await writeToFileAsync(regularIcons, workingDirectory);
await writeToFileAsync(filledIcons, workingDirectory);

Console.WriteLine("Done.");
System.Diagnostics.Debug.WriteLine("INFO | Done.", "Wpf.Ui.FontMapper");

return 0;
