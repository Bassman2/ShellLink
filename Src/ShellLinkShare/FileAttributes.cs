namespace ShellLink;

[Flags]
public enum FileAttributes : uint
{
    None = 0x0000,
    Readonly = 0x0001,
    Hidden = 0x0002,
    System = 0x0004,
    Reserved1 = 0x0008,
    Directory = 0x0010,
    Archive = 0x0020,
    Reserved2 = 0x0040,
    Normal = 0x0080,
    Temporary = 0x0100,
    SparseFile = 0x0200,
    ReparsePoint = 0x0400,
    Compressed = 0x0800,
    Offline = 0x1000,
    NotContentIndexed = 0x2000,
    Encrypted = 0x4000

}
