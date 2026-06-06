namespace ShellLink;

public sealed partial class Shortcut
{
    private void AnalyseExtraData(BinaryReader reader)
    {
        Console.WriteLine();
        Console.WriteLine("ExtraData  (Start: 0x{reader.Position:X})");
    }
    private void ReadExtraData(BinaryReader reader)
    {

    }

    private void WriteExtraData(BinaryWriter writer)
    {

    }
}