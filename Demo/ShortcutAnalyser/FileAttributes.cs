using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ShortcutAnalyser;

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

//public static class FileAttributesExtensions
//{
//    /// <summary>
//    /// Converts the LinkFlags value to a readable string listing all set flags.
//    /// </summary>
//    /// <param name="flags">The LinkFlags value to convert.</param>
//    /// <returns>A string listing all set flags, separated by " | ", or "None" if no flags are set.</returns>
//    public static string ToFlagsString(this FileAttributes flags)
//    {
//        if (flags == FileAttributes.None)
//        {
//            return "None";
//        }

//        var setFlags = new List<string>();

//        foreach (FileAttributes flag in Enum.GetValues<LinkFlags>())
//        {
//            // Skip None and Unused flags
//            if (flag == FileAttributes.None)
//            {
//                continue;
//            }

//            if (flags.HasFlag(flag))
//            {
//                setFlags.Add(flag.ToString());
//            }
//        }

//        return setFlags.Count > 0 ? string.Join(" | ", setFlags) : "None";
//    }

//    /// <summary>
//    /// Converts the LinkFlags value to a detailed multi-line string with descriptions.
//    /// </summary>
//    /// <param name="flags">The LinkFlags value to convert.</param>
//    /// <returns>A multi-line string with each set flag and its description.</returns>
//    public static string ToDetailedString(this FileAttributes flags)
//    {
//        if (flags == FileAttributes.None)
//        {
//            return "None";
//        }

//        var lines = new List<string>();

//        foreach (FileAttributes flag in Enum.GetValues<FileAttributes>())
//        {
//            // Skip None and Unused flags
//            if (flag == FileAttributes.None)
//            {
//                continue;
//            }

//            if (flags.HasFlag(flag))
//            {
//                lines.Add($"    - {flag}");
//            }
//        }

//        return lines.Count > 0 ? string.Join(Environment.NewLine, lines) : "None";
//    }
//}
