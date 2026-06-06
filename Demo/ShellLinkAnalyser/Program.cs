using ShellLink;

namespace ShortcutAnalyser;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Shortcut.Analyse(@"D:\_Data\Shortcut\0K8A0001.JPG.lnk");
        //Shortcut.Analyse(@"D:\_Data\Shortcut\Bilder.lnk");

    }
}
