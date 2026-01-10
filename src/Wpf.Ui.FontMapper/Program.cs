// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Net.Http;
using System.Net.Http.Json;
using Wpf.Ui.FontMapper;

Console.WriteLine("Fluent System Icons Mapper");
System.Diagnostics.Debug.WriteLine("INFO | Fluent System Icons Mapper", "Wpf.Ui.FontMapper");

var workingDirectory =
    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    ?? throw new InvalidOperationException("Could not determine the working directory.");

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

Task<string> FetchVersion()
{
    // using var httpClient = new HttpClient();
    // httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    //
    // return (
    //         await httpClient.GetFromJsonAsync<IEnumerable<GitTag>>(
    //             @"https://api.github.com/repos/microsoft/fluentui-system-icons/git/refs/tags"
    //         )
    //     )
    //         ?.Last()
    //         ?.Ref.Replace("refs/tags/", string.Empty)
    //         .Trim()
    //     ?? throw new Exception("Unable to parse the version string");
    return Task.FromResult("1.1.316");
}

string FormatIconName(string rawIconName)
{
    rawIconName = rawIconName
        .Replace("ic_fluent_", string.Empty)
        .Replace("_regular", string.Empty)
        .Replace("_filled", string.Empty);

    var iconName = string.Empty;

    foreach (var newPart in rawIconName.Split('_'))
    {
        var charactersArray = newPart.ToCharArray();
        charactersArray[0] = char.ToUpper(charactersArray[0]);

        iconName += new string(charactersArray);
    }

    return iconName;
}

async Task FetchFontContents(FontSource source, string version)
{
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

    Dictionary<string, long> sourceJsonContent =
        await httpClient.GetFromJsonAsync<Dictionary<string, long>>(source.SourcePath)
        ?? throw new Exception("Unable to obtain JSON data");

    sourceJsonContent = sourceJsonContent
        .OrderBy(x => x.Value)
        .ToDictionary(k => FormatIconName(k.Key), v => v.Value);

    source.SetContents(sourceJsonContent);
    source.UpdateVersion(version);
}

var recentVersion = await FetchVersion();

await FetchFontContents(regularIcons, recentVersion);
await FetchFontContents(filledIcons, recentVersion);

ICollection<string> regularKeys = regularIcons.Contents.Keys;
ICollection<string> filledKeys = filledIcons.Contents.Keys;
IEnumerable<string> keysToRemove = regularKeys.Except(filledKeys).Concat(filledKeys.Except(regularKeys));

foreach (var key in keysToRemove)
{
    _ = regularIcons.Contents.Remove(key);
    _ = filledIcons.Contents.Remove(key);

    Console.WriteLine($"Deleted key \"{key}\" because no duplicate found in all lists");
}

async Task WriteToFile(FontSource singleFont, string fileRootDirectory)
{
    var destinationPath = Path.Combine(fileRootDirectory, singleFont.DestinationPath);

    var enumName = singleFont.Name;

    var enumMapStringBuilder = new StringBuilder();

    _ = enumMapStringBuilder
        .AppendLine("// This Source Code Form is subject to the terms of the MIT License.")
        .AppendLine(
            "// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT."
        )
        .AppendLine("// Copyright (C) Leszek Pomianowski and WPF UI Contributors.")
        .AppendLine("// All Rights Reserved.")
        .AppendLine()
        .AppendLine("namespace Wpf.Ui.Controls;")
        .AppendLine()
        .AppendLine("/// <summary>")
        .AppendLine($"/// {singleFont.Description.Replace("\n", "\n/// ")}")
        .AppendLine("/// </summary>")
        .AppendLine("#pragma warning disable CS1591")
        .AppendLine("public enum " + enumName)
        .AppendLine("{")
        .AppendLine("    /// <summary>")
        .AppendLine("    /// Actually, this icon is not empty, but makes it easier to navigate.")
        .AppendLine("    /// </summary>")
        .AppendLine("    Empty = 0x0,")
        .AppendLine()
        .AppendLine("    // Automatically generated, may contain bugs.")
        .AppendLine();

    foreach (KeyValuePair<string, long> singleIcon in singleFont.Contents)
    {
        // Older versions
        if (singleIcon.Value < 32)
        {
            _ = enumMapStringBuilder
                .AppendLine()
                .AppendLine("    /// <summary>")
                .AppendLine("    /// Blank icon.")
                .AppendLine("    /// </summary>");
        }

        _ = enumMapStringBuilder.AppendLine($"    {singleIcon.Key} = 0x{singleIcon.Value:X},");
    }

    _ = enumMapStringBuilder
        .AppendLine("}")
        .AppendLine()
        .AppendLine("#pragma warning restore CS1591")
        .Append("\r\n");

    var fileInfo = new FileInfo(destinationPath);

    if (fileInfo.Directory is { Exists: false })
    {
        fileInfo.Directory.Create();
    }

    await File.WriteAllTextAsync(destinationPath, enumMapStringBuilder.ToString());
    Console.WriteLine($"Wrote to file \"{destinationPath}\"");
}

await WriteToFile(regularIcons, workingDirectory);
await WriteToFile(filledIcons, workingDirectory);

Console.WriteLine("Done.");
System.Diagnostics.Debug.WriteLine("INFO | Done.", "Wpf.Ui.FontMapper");
