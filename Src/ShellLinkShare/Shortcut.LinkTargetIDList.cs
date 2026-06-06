namespace ShellLink;

public sealed partial class Shortcut
{

    private void AnalyseLinkTargetIDList(BinaryReader reader)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            Console.WriteLine();
            Console.WriteLine("LinkTargetIDList (Start: 0x{reader.Position:X})");

            int idListSize = reader.ReadInt16();
            int idListStart = reader.Position; // not including idListSize
            Console.WriteLine($"  IDListSize: {idListSize}");

            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  itemIDSize: {itemIDSize}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }

            int idListEnd = reader.Position;
            if (idListStart + idListSize == idListEnd)
            {
                Console.WriteLine($"  End: {idListStart + idListSize} == {idListEnd}");
            }
            else
            {
                Console.Error.WriteLine($"Invalid IDListSize: {idListSize} instead of actual size {idListEnd - idListStart}");
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
