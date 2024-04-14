namespace Wpf.Ui.FontMapper;

class FontSource
{
    public string Name { get; }
    public string Description { get; private set; }
    public string SourcePath { get; }
    public string DestinationPath { get; }
    public IDictionary<string, long> Contents { get; set; } = new Dictionary<string, long>();

    public FontSource(string name, string description, string sourcePath, string destinationPath)
    {
        Name = name;
        Description = description;
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public void SetContents(IDictionary<string, long> contents)
    {
        Contents = contents;
    }

    public void UpdateVersion(string version)
    {
        Description = Description.Replace("{{FLUENT_SYSTEM_ICONS_VERSION}}", version);
    }
}
