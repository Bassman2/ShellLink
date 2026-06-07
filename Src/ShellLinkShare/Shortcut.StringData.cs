using ShellLink.Internal;

namespace ShellLink;

public sealed partial class Shortcut
{
    private void AnalyseStringData(BinaryReader reader)
    {
        using var _ = new OpenTag(reader, "StringData");

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
        bool isUnicode = linkFlags.HasFlag(LinkFlags.IsUnicode);

        if (linkFlags.HasFlag(LinkFlags.HasName))
        {
            FullName = reader.ReadString(isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasRelativePath))
        {
            RelativePath = reader.ReadString(isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasWorkingDir))
        {
            WorkingDirectory = reader.ReadString(isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasArguments))
        {
            Arguments = reader.ReadString(isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasIconLocation))
        {
            IconLocation = reader.ReadString(isUnicode);
        }
    }

    private void WriteStringData(BinaryWriter writer)
    {
        bool isUnicode = linkFlags.HasFlag(LinkFlags.IsUnicode);

        if (linkFlags.HasFlag(LinkFlags.HasName))
        {
            writer.Write(FullName, isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasRelativePath))
        {
            writer.Write(RelativePath, isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasWorkingDir))
        {
            writer.Write(WorkingDirectory, isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasArguments))
        {
            writer.Write(Arguments, isUnicode);
        }
        if (linkFlags.HasFlag(LinkFlags.HasIconLocation))
        {
            writer.Write(IconLocation, isUnicode);
        }

    }
}       
    