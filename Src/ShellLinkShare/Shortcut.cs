
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ShellLinkAnalyser")]

namespace ShellLink;

public sealed partial class Shortcut
{
    

    private Shortcut()
    {
        // Private constructor to prevent direct instantiation
    }

    public static Shortcut CreateShortcut() => new();

    internal static void Analyse(string pathLink)
    {
        var shortcut = new Shortcut();
        using var file = File.OpenRead(pathLink);
        using var reader = new BinaryReader(file);
        shortcut.Analyse(reader);
    }

    public static Shortcut Load(string pathLink)
    {
        var shortcut = new Shortcut();
        using var file = File.OpenRead(pathLink);
        using var reader = new BinaryReader(file);
        shortcut.Read(reader);
        return shortcut;
    }
    
    public void Save(string pathLink)
    {
        using var file = File.Create(pathLink);
        using var writer = new BinaryWriter(file);
        Write(writer);
    }

    internal void Analyse(BinaryReader reader)
    {
        AnalyseShellLinkHeader(reader);
        AnalyseLinkTargetIDList(reader);
        AnalyseLinkInfo(reader);
        AnalyseStringData(reader);
        AnalyseExtraData(reader);
    }

    private void Read(BinaryReader reader)
    {
        ReadShellLinkHeader(reader);
        ReadLinkTargetIDList(reader);
        ReadLinkInfo(reader);
        ReadStringData(reader);
        ReadExtraData(reader);
    }

    private void Write(BinaryWriter writer)
    {
        WriteShellLinkHeader(writer);
        WriteLinkTargetIDList(writer);
        WriteLinkInfo(writer);
        WriteStringData(writer);
        WriteExtraData(writer);
    }

    public string? FullName { get; private set; }


    public string? Arguments { get; set; }


    public string? Description { get; set; }


    public string? Hotkey { get; set; }


    public string? IconLocation { get; set; }


    public string? RelativePath { get;  set; }


    public string? TargetPath { get; set; }


    public int WindowStyle { get; set; }


    public string? WorkingDirectory { get; set; }

}
