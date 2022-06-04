// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Text;
using System.Text.Json;
using WPFUI.FontMapper;

Console.WriteLine("Fluent System Icons Mapper");
System.Diagnostics.Debug.WriteLine("INFO | Fluent System Icons Mapper", "WPFUI.FontMapper");

var fluentSystemIconsVersion = "1.1.172";
var executingPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var fountSources = new FontSource[]
{
    new()
    {
        Name = "SymbolRegular",
        Description = $"Represents a list of regular Fluent System Icons <c>v.{fluentSystemIconsVersion}</c>.\n<para>May be converted to <see langword=\"char\"/> using <c>GetGlyph()</c> or to <see langword=\"string\"/> using <c>GetString()</c></para>",
        SourcePath = "FluentSystemIcons-Regular.json",
        DestinationPath = "generated\\SymbolRegular.cs"
    },
    new()
    {
        Name = "SymbolFilled",
        Description = $"Represents a list of filled Fluent System Icons <c>v.{fluentSystemIconsVersion}</c>.\n<para>May be converted to <see langword=\"char\"/> using <c>GetGlyph()</c> or to <see langword=\"string\"/> using <c>GetString()</c></para>",
        SourcePath = "FluentSystemIcons-Filled.json",
        DestinationPath = "generated\\SymbolFilled.cs"
    },
    new()
    {
        Name = "SymbolResizable",
        Description = $"Represents a list of resizable Fluent System Icons <c>v.{fluentSystemIconsVersion}</c>.\n<para>May be converted to <see langword=\"char\"/> using <c>GetGlyph()</c> or to <see langword=\"string\"/> using <c>GetString()</c></para>",
        SourcePath = "FluentSystemIcons-Resizable.json",
        DestinationPath = "generated\\SymbolResizable.cs"
    }
};

Parallel.ForEach(fountSources, singleFont =>
{
    var sourcePath = Path.Combine(executingPath, singleFont.SourcePath);
    var destinationPath = Path.Combine(executingPath, singleFont.DestinationPath);

    if (!File.Exists(sourcePath))
        return;

    Console.WriteLine($"Mapping {singleFont.Name}");
    System.Diagnostics.Debug.WriteLine($"INFO | Mapping {singleFont.Name}", "WPFUI.FontMapper");

    var fileStream = new FileStream(sourcePath, FileMode.Open);
    using var reader = new StreamReader(fileStream);

    var jsonData = JsonSerializer.Deserialize<Dictionary<string, long>>(reader.ReadToEnd()) ?? new Dictionary<string, long>();
    var enumName = singleFont.Name;

    var enumMapStringBuilder = new StringBuilder();

    enumMapStringBuilder.AppendLine("// This Source Code Form is subject to the terms of the MIT License.");
    enumMapStringBuilder.AppendLine("// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.");
    enumMapStringBuilder.AppendLine("// Copyright (C) Leszek Pomianowski and WPF UI Contributors.");
    enumMapStringBuilder.AppendLine("// All Rights Reserved.");
    enumMapStringBuilder.AppendLine("");
    enumMapStringBuilder.AppendLine("namespace WPFUI.Common;");
    enumMapStringBuilder.AppendLine("");

    enumMapStringBuilder.AppendLine("/// <summary>");
    enumMapStringBuilder.AppendLine($"/// {singleFont.Description.Replace("\n", "\n///")}");
    enumMapStringBuilder.AppendLine("/// <summary>");

    enumMapStringBuilder.AppendLine("public enum " + enumName);
    enumMapStringBuilder.AppendLine("{");
    enumMapStringBuilder.AppendLine("    /// <summary>");
    enumMapStringBuilder.AppendLine("    /// Actually, this icon is not empty, but makes it easier to navigate.");
    enumMapStringBuilder.AppendLine("    /// </summary>");
    enumMapStringBuilder.AppendLine("    Empty = 0x0,");
    enumMapStringBuilder.AppendLine("");
    enumMapStringBuilder.AppendLine("    // Automatically generated, may contain bugs.");
    enumMapStringBuilder.AppendLine("");

    var parsedJsonData = new Dictionary<string, long>();

    foreach (var singleItem in jsonData)
    {
        var iconName = String.Empty;
        var iconId = singleItem.Value;
        var name = singleItem.Key
            .Replace("ic_fluent_", String.Empty)
            .Replace("_regular", String.Empty)
            .Replace("_filled", String.Empty);

        if (iconId > 65535)
            iconId -= 65536;

        foreach (var newPart in name.Split('_'))
        {
            var charactersArray = newPart.ToCharArray();
            charactersArray[0] = Char.ToUpper(charactersArray[0]);

            iconName += new string(charactersArray);
        }

        if (!parsedJsonData.ContainsKey(iconName))
            parsedJsonData.Add(iconName, iconId);
    }

    parsedJsonData = parsedJsonData.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

    foreach (var singleIcon in parsedJsonData)
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

    var fileInfo = new FileInfo(destinationPath);

    if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
        lock (fileInfo.Directory)
        { fileInfo.Directory.Create(); }

    File.WriteAllText(destinationPath, enumMapStringBuilder.ToString());
});

Console.WriteLine("Done.");
System.Diagnostics.Debug.WriteLine("INFO | Done.", "WPFUI.FontMapper");

return 0;
