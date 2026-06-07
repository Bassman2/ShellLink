using IWshRuntimeLibrary;

namespace ShortcutDemo;

// https://stackoverflow.com/questions/4897655/create-a-shortcut-on-desktop
// http://www.vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var shell = new WshShell();
        var x = shell.CreateShortcut(@"D:\_Data\Shortcut\Create.jpg.lnk");
        var windowsApplicationShortcut = (IWshShortcut)x;
        windowsApplicationShortcut.Description = "Description";
        //windowsApplicationShortcut.WorkingDirectory = "C:\\temp\\ZipLink\\Demo";
        windowsApplicationShortcut.TargetPath = @"C:\Users\Ralf\Pictures\EOS R6 Mark II\2024-08-22\0K8A0423.JPG";
        windowsApplicationShortcut.Save();
    }
}
