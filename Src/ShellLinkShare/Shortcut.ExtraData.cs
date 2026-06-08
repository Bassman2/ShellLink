using ShellLink.Internal;
using System.Windows.Markup;

namespace ShellLink;

public sealed partial class Shortcut
{
    
    private const uint ConsoleDataBlockSignature = 0xA0000002;
    private const uint ConsoleFEDataBlockSignature = 0xA0000004;
    private const uint DarwinDataBlockSignature = 0xA0000006;
    private const uint EnvironmentVariableDataBlockSignature = 0xA0000001;
    private const uint IconEnvironmentDataBlockSignature = 0xA0000007;
    private const uint KnownFolderDataBlockSignature = 0xA000000B;
    private const uint PropertyStoreDataBlockSignature = 0xA0000009;
    private const uint ShimDataBlockSignature = 0xA0000008;
    private const uint SpecialFolderDataBlockSignature = 0xA0000005;
    private const uint TrackerDataBlockSignature = 0xA0000003;
    private const uint VistaAndAboveIDListDataBlockSignature = 0xA000000C;

    private const uint TerminalBlockSize = 0x00000004;
    private const uint ConsoleDataBlockSize = 0x000000CC;
    private const uint ConsoleFEDataBlockSize = 0x0000000C;
    private const uint DarwinDataBlockSize = 0x00000314;
    private const uint EnvironmentVariableDataBlockSize = 0x00000314;
    private const uint IconEnvironmentDataBlockSize = 0x00000314;
    private const uint KnownFolderDataBlockSize = 0x0000001C;
    private const uint PropertyStoreDataBlockSize = 0x0000000C;
    private const uint ShimDataBlockSize = 0x00000088;
    private const uint SpecialFolderDataBlockSize = 0x00000010;
    private const uint TrackerDataBlockSize = 0x00000060;
    private const uint VistaAndAboveIDListDataBlockSize = 0x0000000A;

    private const uint PropertyStoreDataBlockSignatureVersion = 0x53505331;
    private void AnalyseExtraData(BinaryReader reader)
    {
        using var _ = new OpenTag(reader, "ExtraData");

        while (true)
        {
            uint blockSize = reader.ReadUInt32();
            if (blockSize < TerminalBlockSize)
            {
                break; 
            }
            uint blockSignature = reader.ReadUInt32();

            switch (blockSignature)
            {
            case ConsoleDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  ConsoleDataBlock", blockSize);
                    Assert.Equal("ConsoleDataBlockSize", blockSize, ConsoleDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case ConsoleFEDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  ConsoleFEDataBlock", blockSize);
                    Assert.Equal("ConsoleFEDataBlockSize", blockSize, ConsoleFEDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case DarwinDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  DarwinDataBlock", blockSize);
                    Assert.Equal("DarwinDataBlockSize", blockSize, DarwinDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case EnvironmentVariableDataBlockSignature:
                { 
                    using var tag = new ExtraTag(reader, "  EnvironmentVariableDataBlock", blockSize);
                    Assert.Equal("EnvironmentVariableDataBlockSize", blockSize, EnvironmentVariableDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case IconEnvironmentDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  IconEnvironmentDataBlock", blockSize);
                    Assert.Equal("IconEnvironmentDataBlockSize", blockSize, IconEnvironmentDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case KnownFolderDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  KnownFolderDataBlock", blockSize);
                    Assert.Equal("KnownFolderDataBlockSize", blockSize, KnownFolderDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case PropertyStoreDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  PropertyStoreDataBlock", blockSize);
                    //Assert.Equal("KnownFolderDataBlockSize", blockSize, KnownFolderDataBlockSize);

                    uint storeSize = reader.ReadUInt32();
                    Console.WriteLine($"  StoreSize: 0x{storeSize:X}");

                    while (true)
                    {
                        uint storageSize = reader.ReadUInt32();
                        Console.WriteLine($"  StorageSize: 0x{storageSize:X}");

                        if (storageSize == 0)
                        {
                            break;
                        }
                        uint version = reader.ReadUInt32();
                        Console.WriteLine($"  Version: 0x{version:X}");
                        Assert.Equal("Version", version, PropertyStoreDataBlockSignatureVersion);
                        Console.WriteLine($"  Format ID: {reader.ReadGuid()}");
                    }

                    //reader.Position += blockSize - 8 - 4;
                }
                break;
            case ShimDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  ShimDataBlock", blockSize);
                    Assert.Equal("ShimDataBlockSize", blockSize, ShimDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case SpecialFolderDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  SpecialFolderDataBlock", blockSize);
                    Assert.Equal("SpecialFolderDataBlockSize", blockSize, SpecialFolderDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            case TrackerDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  TrackerDataBlock", blockSize);
                    Assert.Equal("TrackerDataBlockSize", blockSize, TrackerDataBlockSize);

                    uint length = reader.ReadUInt32();
                    Console.WriteLine($"  Length: 0x{length:X}");

                    Console.WriteLine($"  Version: {reader.ReadUInt32()}");
                    Console.WriteLine($"  MachineID: {reader.ReadString(16)}");

                    
                    Console.WriteLine($"  Droid 1: {reader.ReadGuid()}");
                    Console.WriteLine($"  Droid 2: {reader.ReadGuid()}");

                    Console.WriteLine($"  Droid Birth 1: {reader.ReadGuid()}");
                    Console.WriteLine($"  Droid Birth 2: {reader.ReadGuid()}");
                }
                break;
            case VistaAndAboveIDListDataBlockSignature:
                {
                    using var tag = new ExtraTag(reader, "  VistaAndAboveIDListDataBlock", blockSize);
                    Assert.Equal("VistaAndAboveIDListDataBlockSize", blockSize, VistaAndAboveIDListDataBlockSize);
                    reader.Position += blockSize - 8;
                }
                break;
            }

        }
    }

    

    private void ReadExtraData(BinaryReader reader)
    {

    }

    private void WriteExtraData(BinaryWriter writer)
    {

    }
}