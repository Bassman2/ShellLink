using ShellLink;



namespace ShellLinkTest;

[STATestClass]
public sealed class TestShellLink
{
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

        try
        {
            string lnkPath = @"D:\_Data\ShellLink\DynamicTest\Picture123.lnk";
            var shl = new Shell32.Shell();
            lnkPath = System.IO.Path.GetFullPath(lnkPath);
            var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
            targetpath = lnk.Target.Path;
            //Assert.AreEqual(@"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG", lnk.Target.Path);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }

        Assert.AreEqual(@"C:\Users\Ralf\Pictures\Canon\EOS R6m2\2024-06-20\0K8A0018.JPG", targetpath);
    }
}
