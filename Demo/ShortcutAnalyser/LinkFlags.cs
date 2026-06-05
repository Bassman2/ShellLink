//namespace ShortcutAnalyser;

///// <summary>
///// Flags that specify which shell link structures are present in the file.
///// Based on the Windows Shell Link (.LNK) Binary File Format specification.
///// </summary>
//[Flags]
//public enum LinkFlags : uint
//{
//    /// <summary>
//    /// No flags set.
//    /// </summary>
//    None = 0x00000000,

//    /// <summary>
//    /// The shell link is saved with an item ID list (IDList). If this bit is set, a LinkTargetIDList structure must follow the ShellLinkHeader.
//    /// </summary>
//    HasLinkTargetIDList = 0x00000001,

//    /// <summary>
//    /// The shell link is saved with link information. If this bit is set, a LinkInfo structure must be present.
//    /// </summary>
//    HasLinkInfo = 0x00000002,

//    /// <summary>
//    /// The shell link is saved with a name string. If this bit is set, a NAME_STRING StringData structure must be present.
//    /// </summary>
//    HasName = 0x00000004,

//    /// <summary>
//    /// The shell link is saved with a relative path string. If this bit is set, a RELATIVE_PATH StringData structure must be present.
//    /// </summary>
//    HasRelativePath = 0x00000008,

//    /// <summary>
//    /// The shell link is saved with a working directory string. If this bit is set, a WORKING_DIR StringData structure must be present.
//    /// </summary>
//    HasWorkingDir = 0x00000010,

//    /// <summary>
//    /// The shell link is saved with command line arguments. If this bit is set, a COMMAND_LINE_ARGUMENTS StringData structure must be present.
//    /// </summary>
//    HasArguments = 0x00000020,

//    /// <summary>
//    /// The shell link is saved with an icon location string. If this bit is set, an ICON_LOCATION StringData structure must be present.
//    /// </summary>
//    HasIconLocation = 0x00000040,

//    /// <summary>
//    /// The shell link contains Unicode encoded strings. This bit should be set if the strings in the StringData structures are Unicode.
//    /// </summary>
//    IsUnicode = 0x00000080,

//    /// <summary>
//    /// The LinkInfo structure is ignored.
//    /// </summary>
//    ForceNoLinkInfo = 0x00000100,

//    /// <summary>
//    /// The shell link is saved with an EnvironmentVariableDataBlock.
//    /// </summary>
//    HasExpString = 0x00000200,

//    /// <summary>
//    /// The target is run in a separate virtual machine when launching a link target that is a 16-bit application.
//    /// </summary>
//    RunInSeparateProcess = 0x00000400,

//    /// <summary>
//    /// A bit that is undefined and must be ignored.
//    /// </summary>
//    Unused1 = 0x00000800,

//    /// <summary>
//    /// The shell link is saved with a DarwinDataBlock.
//    /// </summary>
//    HasDarwinID = 0x00001000,

//    /// <summary>
//    /// The application is run as a different user when the target of the shell link is activated.
//    /// </summary>
//    RunAsUser = 0x00002000,

//    /// <summary>
//    /// The shell link is saved with an IconEnvironmentDataBlock.
//    /// </summary>
//    HasExpIcon = 0x00004000,

//    /// <summary>
//    /// The file system location is represented in the shell namespace when the path to an item is parsed into an IDList.
//    /// </summary>
//    NoPidlAlias = 0x00008000,

//    /// <summary>
//    /// A bit that is undefined and must be ignored.
//    /// </summary>
//    Unused2 = 0x00010000,

//    /// <summary>
//    /// The shell link is saved with a ShimDataBlock.
//    /// </summary>
//    RunWithShimLayer = 0x00020000,

//    /// <summary>
//    /// The TrackerDataBlock is ignored.
//    /// </summary>
//    ForceNoLinkTrack = 0x00040000,

//    /// <summary>
//    /// The shell link attempts to collect target properties and store them in the PropertyStoreDataBlock when the link target is set.
//    /// </summary>
//    EnableTargetMetadata = 0x00080000,

//    /// <summary>
//    /// The EnvironmentVariableDataBlock is ignored.
//    /// </summary>
//    DisableLinkPathTracking = 0x00100000,

//    /// <summary>
//    /// The SpecialFolderDataBlock and the KnownFolderDataBlock are ignored when loading the shell link.
//    /// </summary>
//    DisableKnownFolderTracking = 0x00200000,

//    /// <summary>
//    /// If the link has a KnownFolderDataBlock, the unaliased form of the known folder IDList should be used when translating the target IDList at the time that the link is loaded.
//    /// </summary>
//    DisableKnownFolderAlias = 0x00400000,

//    /// <summary>
//    /// Creating a link that references another link is enabled. Otherwise, specifying a link as the target IDList will not be allowed.
//    /// </summary>
//    AllowLinkToLink = 0x00800000,

//    /// <summary>
//    /// When saving a link for which the target IDList is under a known folder, either the unaliased form of that known folder or the target IDList should be used.
//    /// </summary>
//    UnaliasOnSave = 0x01000000,

//    /// <summary>
//    /// The target IDList should not be stored; instead, the path specified in the EnvironmentVariableDataBlock should be used to refer to the target.
//    /// </summary>
//    PreferEnvironmentPath = 0x02000000,

//    /// <summary>
//    /// When the target is a UNC name that refers to a location on a local machine, the local path IDList in the PropertyStoreDataBlock should be stored, so it can be used when the link is loaded on the local machine.
//    /// </summary>
//    KeepLocalIDListForUNCTarget = 0x04000000
//}

///// <summary>
///// Extension methods for LinkFlags enum.
///// </summary>
////public static class LinkFlagsExtensions
////{
////    /// <summary>
////    /// Converts the LinkFlags value to a readable string listing all set flags.
////    /// </summary>
////    /// <param name="flags">The LinkFlags value to convert.</param>
////    /// <returns>A string listing all set flags, separated by " | ", or "None" if no flags are set.</returns>
////    public static string ToFlagsString(this LinkFlags flags)
////    {
////        if (flags == 0)
////        {
////            return "None";
////        }

////        var setFlags = new List<string>();

////        foreach (LinkFlags flag in Enum.GetValues<LinkFlags>())
////        {
////            // Skip None and Unused flags
////            if (flag == LinkFlags.None)
////            {
////                continue;
////            }

////            if (flags.HasFlag(flag))
////            {
////                setFlags.Add(flag.ToString());
////            }
////        }

////        return setFlags.Count > 0 ? string.Join(" | ", setFlags) : "None";
////    }

////    /// <summary>
////    /// Converts the LinkFlags value to a detailed multi-line string with descriptions.
////    /// </summary>
////    /// <param name="flags">The LinkFlags value to convert.</param>
////    /// <returns>A multi-line string with each set flag and its description.</returns>
////    public static string ToDetailedString(this LinkFlags flags)
////    {
////        if (flags == LinkFlags.None)
////        {
////            return "None";
////        }

////        var lines = new List<string>();

////        foreach (LinkFlags flag in Enum.GetValues<LinkFlags>())
////        {
////            // Skip None and Unused flags
////            if (flag == LinkFlags.None || flag == LinkFlags.Unused1 || flag == LinkFlags.Unused2)
////            {
////                continue;
////            }

////            if (flags.HasFlag(flag))
////            {
////                lines.Add($"    - {flag}");
////            }
////        }

////        return lines.Count > 0 ? string.Join(Environment.NewLine, lines) : "None";
////    }
////}
