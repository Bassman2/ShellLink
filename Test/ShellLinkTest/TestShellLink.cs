using Shell32;
using ShellLink;



namespace ShellLinkTest;

[STATestClass]
public sealed class TestShellLink
{
    [TestMethod]
    public void TestOriginalFile()
    {
        string lnkPath = @"D:\_Data\ShellLink\StaticTest\Explorer-Image1.jpg.lnk";
        
        var shl = new Shell32.Shell();
        var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
        var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
        var lnk = (Shell32.ShellLinkObject)itm.GetLink;

        Assert.AreEqual("Image1.jpg", lnk.Target.Name);
        Assert.AreEqual(@"D:\_Data\ShellLink\Targets\Image1.jpg", lnk.Target.Path);
        Assert.IsFalse(lnk.Target.IsLink);
        Assert.IsFalse(lnk.Target.IsFolder);
        Assert.IsTrue(lnk.Target.IsFileSystem);
        Assert.IsFalse(lnk.Target.IsBrowsable);
        Assert.AreEqual(new DateTime(2024, 7, 4, 16, 7, 19), lnk.Target.ModifyDate);
        Assert.AreEqual(12771514, lnk.Target.Size);
        Assert.AreEqual("JPG-Datei", lnk.Target.Type);

        FolderItemVerbs verbs = lnk.Target.Verbs();
        int count = verbs.Count;
        Console.WriteLine($"Verb count: {count}");
        foreach(FolderItemVerb verb in verbs)
        {
            Console.WriteLine($"Verb: '{verb.Name}'");
        }
    }

    [TestMethod]
    public void TestOriginalFolder()
    {
        string lnkPath = @"D:\_Data\ShellLink\StaticTest\Explorer-Folder.lnk";

        var shl = new Shell32.Shell();
        var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
        var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
        var lnk = (Shell32.ShellLinkObject)itm.GetLink;

        Assert.AreEqual("Folder", lnk.Target.Name);
        Assert.AreEqual(@"D:\_Data\ShellLink\Targets\Folder", lnk.Target.Path);
        Assert.IsFalse(lnk.Target.IsLink);
        Assert.IsTrue(lnk.Target.IsFolder);
        Assert.IsTrue(lnk.Target.IsFileSystem);
        Assert.IsFalse(lnk.Target.IsBrowsable);
        Assert.AreEqual(new DateTime(2026, 6, 13, 12, 30, 55), lnk.Target.ModifyDate);
        Assert.AreEqual(0, lnk.Target.Size);
        Assert.AreEqual("Dateiordner", lnk.Target.Type);

        FolderItemVerbs verbs = lnk.Target.Verbs();
        int count = verbs.Count;
        Console.WriteLine($"Verb count: {count}");
        foreach (FolderItemVerb verb in verbs)
        {
            Console.WriteLine($"Verb: '{verb.Name}'");
        }
    }

    [TestMethod]
    public void TestAbsolureUtf8()
    {
        var createShortcut = Shortcut.CreateShortcut();
        createShortcut.IsUnicode = false;
        createShortcut.TargetPath = @"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG";
        createShortcut.Save(@"D:\_Data\ShellLink\DynamicTest\Picture123.lnk");

        //var loadShortcut = Shortcut.Load(@"D:\_Data\ShellLink\DynamicTest\Picture123.lnk");
        //Assert.IsFalse(loadShortcut.IsUnicode);
        //Assert.AreEqual(@"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG", loadShortcut.TargetPath);

        string targetpath = string.Empty;

        //try
        //{
            string lnkPath = @"D:\_Data\ShellLink\DynamicTest\Picture123.lnk";
            var shl = new Shell32.Shell();
            lnkPath = System.IO.Path.GetFullPath(lnkPath);
            var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
            targetpath = lnk.Target.Path;
            //Assert.AreEqual(@"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG", lnk.Target.Path);
        //}
        //catch (Exception ex)
        //{
        //    Assert.Fail(ex.Message);
        //}

        Assert.AreEqual(@"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG", targetpath);
    }
}
