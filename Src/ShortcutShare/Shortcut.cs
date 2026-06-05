namespace ShortcutShare;

public class Shortcut
{
    private const string ShellLinkCLSID = "00021401-0000-0000-C000-000000000046";

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
        Analyse(reader);
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

    internal static void Analyse(BinaryReader reader)
    {
        #region ShellLinkHeader

        Console.WriteLine($"ShellLinkHeader");
        Console.WriteLine($"  HeaderSize: {reader.ReadInt32()}");
        Console.WriteLine($"  LinkCLSID: {reader.ReadGuid()}");


        #endregion
    }

    private void Read(BinaryReader reader)
    {
        #region ShellLinkHeader
        #endregion
    }

    private void Write(BinaryWriter writer)
    {
        #region ShellLinkHeader
        #endregion
    }



    /*

    public string FullName { get; }


    public string Arguments { get; set; }


    public string Description { get; set; }


    public string Hotkey { get; set; }


    public string IconLocation { get; set; }


    public string RelativePath { set; }


    public string TargetPath { get; set; }


    public int WindowStyle { get; set; }


    public string WorkingDirectory { get; set; }

    */
}
