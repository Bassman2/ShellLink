using ShellLink.Internal;

namespace ShellLink;

public sealed partial class Shortcut
{

    private void AnalyseLinkTargetIDList(BinaryReader reader)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            int offset = 2; // not including idListSize
            using var linkTargetIDListTag = new Size16Tag(reader, "LinkTargetIDList", offset);
            
            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  ItemIDSize: 0x{itemIDSize:X}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }
        }   
    }

    private void ReadLinkTargetIDList(BinaryReader reader)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  ItemIDSize: 0x{itemIDSize:X}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }
        }
    }

    private void WriteLinkTargetIDList(BinaryWriter writer)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            writer.Write((Int16)0); // empty IDList
        }
    }
}
