
using ShellLink.Internal;
using System.Runtime;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ShellLinkAnalyser")]

namespace ShellLink;

// https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-propstore/39ea873f-7af5-44dd-92f9-bc1f293852cc

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
        AnalyseShellLinkHeader(reader);     // must have
        AnalyseLinkTargetIDList(reader);    // optional
        AnalyseLinkInfo(reader);            // optional
        AnalyseStringData(reader);          // optional
        AnalyseExtraData(reader);           // optional

        Assert.FileEnding(reader);
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
        linkFlags =
            (LinkFlags)(0
                | (FullName != null ? LinkFlags.HasName : 0)
                | (RelativePath != null ? LinkFlags.HasRelativePath : 0)
                | (WorkingDirectory != null ? LinkFlags.HasWorkingDir : 0)
                | (Arguments != null ? LinkFlags.HasArguments : 0)
                | (IconLocation != null ? LinkFlags.HasIconLocation : 0)
                
            );

        if (Directory.Exists(TargetPath))
        {
            var dirInfo = new DirectoryInfo(TargetPath);
            fileAttributes = (FileAttributesFlags)dirInfo.Attributes;
            creationTime = dirInfo.CreationTimeUtc;
            accessTime = dirInfo.LastAccessTimeUtc; 
            writeTime = dirInfo.LastWriteTimeUtc;
            fileSize = 0;
        }
        else if (File.Exists(TargetPath))
        {

            var fileInfo = new FileInfo(TargetPath);
            fileAttributes = (FileAttributesFlags)fileInfo.Attributes;
            creationTime = fileInfo.CreationTimeUtc;
            accessTime = fileInfo.LastAccessTimeUtc;
            writeTime = fileInfo.LastWriteTimeUtc;
            //  If the link target file is larger than 0xFFFFFFFF, this value specifies the least significant 32 bits of the link target file size.
            fileSize = (uint)(fileInfo.Length & 0xFFFFFFFF);
        }
        else
        {
            ShortcutException.ThrowException($"TargetPath '{TargetPath}' does not exist.");
        }
                
        
        WriteShellLinkHeader(writer);
        WriteLinkTargetIDList(writer);
        WriteLinkInfo(writer);
        WriteStringData(writer);
        WriteExtraData(writer);
    }

    public bool IsUnicode { get; set; }

    public string? FullName { get; private set; }


    public string? Arguments { get; set; }


    public string? Description { get; set; }


    public string? Hotkey { get; set; }


    public string? IconLocation { get; set; }

    public int IconIndex { get; set; }

    private ShowCommand ShowCommand { get; set; } = ShowCommand.ShowNormal;

    private HotKeys HotKey { get; set; } = HotKeys.None;

    private HotKeyModifierKeys HotKeyModifiers { get; set; } = HotKeyModifierKeys.None;

    public string? RelativePath { get;  set; }


    public string? TargetPath { get; set; }


    public int WindowStyle { get; set; }


    public string? WorkingDirectory { get; set; }

}
