using ShellLink.Internal;

namespace ShellLink;

public sealed partial class Shortcut
{

    private void AnalyseLinkTargetIDList(BinaryReader reader)
    {
        if (linkFlags.HasFlag(LinkFlags.HasLinkTargetIDList))
        {
            int offset = 2; // not including idListSize

            int idListStart = reader.Position; 
            int idListSize = reader.ReadInt16();

            ConHelp.StartTag("LinkTargetIDList", idListStart, idListSize);
            
            while (true)
            {
                short itemIDSize = reader.ReadInt16();
                Console.WriteLine($"  ItemIDSize: 0x{itemIDSize:X}");
                if (itemIDSize == 0) break;

                reader.BaseStream.Seek(itemIDSize - 2, SeekOrigin.Current);
            }

            int idListEnd = reader.Position;
            ConHelp.EndTag("LinkTargetIDList", idListStart, idListSize, idListEnd, offset);
        }   
    }

    private void ReadLinkTargetIDList(BinaryReader reader)
    {
    }

    private void WriteLinkTargetIDList(BinaryWriter writer)
    {
    }
}
