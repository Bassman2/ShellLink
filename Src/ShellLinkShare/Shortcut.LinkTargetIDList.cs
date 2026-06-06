namespace ShellLink;

public sealed partial class Shortcut
{

    private void AnalyseLinkTargetIDList(BinaryReader reader)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            int idListSize = reader.ReadInt16();
            int idListStart = reader.Position; // not including idListSize

            Console.WriteLine();
            Console.WriteLine($"LinkTargetIDList (Start: 0x{idListStart - 2:X}, Size: 0x{idListSize:X})");

            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  itemIDSize: {itemIDSize}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }

            int idListEnd = reader.Position;
            Console.WriteLine($"LinkTargetIDList End: 0x{idListStart + idListSize:X} == 0x{idListEnd:X}");
            if (idListStart + idListSize != idListEnd)
            {
                Console.Error.WriteLine($"Error: Invalid IDListSize: 0x{idListSize:X} instead of actual size 0x{idListEnd - idListStart:X}");
            }
        }   
    }

    private void ReadLinkTargetIDList(BinaryReader reader)
    {
    }

    private void WriteLinkTargetIDList(BinaryWriter writer)
    {
    }
}
