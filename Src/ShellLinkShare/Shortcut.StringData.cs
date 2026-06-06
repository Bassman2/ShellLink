namespace ShellLink;

public sealed partial class Shortcut
{
    private void AnalyseStringData(BinaryReader reader)
    {
        Console.WriteLine();
        Console.WriteLine("StringData");

        if (linkFlags.HasFlag(LinkFlags.HasName))
        {
            string nameString = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  NameString: {nameString}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasRelativePath))
        {
            string relativePath = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  RelativePath: {relativePath}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasWorkingDir))
        {
            string workingDir = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  WorkingDir: {workingDir}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasArguments))
        {
            string commandLineArguments = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  CommandLineArguments: {commandLineArguments}");
        }
        if (linkFlags.HasFlag(LinkFlags.HasIconLocation))
        {
            string iconLocation = reader.ReadString(linkFlags.HasFlag(LinkFlags.IsUnicode));
            Console.WriteLine($"  IconLocation: {iconLocation}");
        }
    }

    private void ReadStringData(BinaryReader reader)
    {
    }

    private void WriteStringData(BinaryWriter writer)
    {

    }
}       
    