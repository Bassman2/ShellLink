////using IWshRuntimeLibrary;
//using File = System.IO.File;

//namespace ShortcutAnalyser;

//public static class Shortcut
//{
//    public static void CreateShortcut(string linkPath, string targetPath)
//    {
//        //if (!File.Exists(targetPath))
//        //{
//        //    throw new ArgumentException($"Target file does not exist: {targetPath}");
//        //}

//        //Directory.CreateDirectory(Path.GetDirectoryName(linkPath)!);
//        //File.CreateSymbolicLink(linkPath, targetPath);

//        if (File.Exists(linkPath))
//        {
//            File.Delete(linkPath);
//        }

//        //var shell = new WshShell();
//        //var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(linkPath);
//        //windowsApplicationShortcut.Description = "Test for ConfluenceExporter";
//        //windowsApplicationShortcut.WorkingDirectory = "C:\\temp\\ZipLink\\Demo";
//        //windowsApplicationShortcut.TargetPath = "C:\\temp\\ZipLink\\Demo\\Ordner\\demo.txt";
//        //windowsApplicationShortcut.Save();

//        string txt = File.ReadAllText(linkPath);

//        Analyse(@"C:\temp\ZipMove\Demo\relative.txt.lnk");


//    }

//    public static void Analyse(string path)
//    {
//        using var file = File.OpenRead(path);
//        using var reader = new BinReader(file);

//        Console.WriteLine($"Analyse: {path}");
//        Console.WriteLine($"ShellLinkHeader");

//        Console.WriteLine($"  HeaderSize: {reader.ReadInt32LittleEndian()}");
//        Console.WriteLine($"  LinkCLSID: {reader.ReadGuidString()}");

//        LinkFlags linkFlags = (LinkFlags)reader.ReadInt32LittleEndian();
//        Console.WriteLine($"  LinkFlags:");
//        Console.WriteLine(linkFlags.ToDetailedString());

//        FileAttributes fileAttributes = (FileAttributes)reader.ReadInt32LittleEndian();
//        Console.WriteLine($"  FileAttributes: {fileAttributes:X}");
//        Console.WriteLine(fileAttributes.ToDetailedString());

        
//        Console.WriteLine($"  CreationTime: {reader.ReadFileTimeString()}");
//        Console.WriteLine($"  AccessTime: {reader.ReadFileTimeString()}");
//        Console.WriteLine($"  WriteTime: {reader.ReadFileTimeString()}");
//        Console.WriteLine($"  FileSize: {reader.ReadInt32LittleEndian()} bytes");

//        Console.WriteLine($"  IconIndex: {reader.ReadInt32LittleEndian()} bytes");
//        Console.WriteLine($"  ShowCommand: 0x{reader.ReadInt32LittleEndian():X8}");

//        Console.WriteLine($"  HotKey: {reader.ReadInt16LittleEndian()}");

//        Console.WriteLine($"  Reserved1: {reader.ReadInt16LittleEndian()}");
//        Console.WriteLine($"  Reserved2: {reader.ReadInt32LittleEndian()}");
//        Console.WriteLine($"  Reserved3: {reader.ReadInt32LittleEndian()}");

//        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
//        {
//            Console.WriteLine("LinkTargetIDList");
//            Console.WriteLine($"  IDListSize: {reader.ReadInt16LittleEndian()}");

//            while(true)
//            {
//                short itemIDSize = reader.ReadInt16LittleEndian();
//                Console.WriteLine($"  itemIDSize: {itemIDSize}");
//                if (itemIDSize == 0) break;

//                reader.Seek(itemIDSize, SeekOrigin.Current);
//            }
//        }

//        Console.WriteLine("LinkInfo");
//        Console.WriteLine($"  LinkInfoSize: {reader.ReadInt32LittleEndian()} bytes");
//        Console.WriteLine($"  LinkInfoHeaderSize: {reader.ReadInt32LittleEndian()} bytes");


//        Console.WriteLine("StringData");

//        Console.WriteLine("ExtraData");
//    }
//}


