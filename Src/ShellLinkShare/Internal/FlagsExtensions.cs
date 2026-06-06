namespace ShellLink;

internal static class FlagsExtensions
{
    public static string ToFlagsString<T>(this T flags) where T : Enum
    {
        var defaultValue = (T)(object)0;
        if (flags.Equals(defaultValue))
        {
            return "None";
        }

        var setFlags = new List<string>();

        foreach (T flag in Enum.GetValues(typeof(T)))
        {
            // Skip None and Unused flags
            if (flag.Equals(defaultValue))
            {
                continue;
            }

            if (flags.HasFlag(flag))
            {
                setFlags.Add(flag.ToString());
            }
        }

        return setFlags.Count > 0 ? string.Join(" | ", setFlags) : "None";
    }

    /// <summary>
    /// Converts the LinkFlags value to a detailed multi-line string with descriptions.
    /// </summary>
    /// <param name="flags">The LinkFlags value to convert.</param>
    /// <returns>A multi-line string with each set flag and its description.</returns>
    public static string ToDetailedString<T>(this T flags) where T : Enum
    {
        var defaultValue = default(T); // (T)(object)0;
        if (flags.Equals(defaultValue))
        {
            return "None";
        }

        var lines = new List<string>();

        foreach (T flag in Enum.GetValues(typeof(T)))
        {
            // Skip None and Unused flags
            if (flag.Equals(defaultValue))
            {
                continue;
            }

            if (flags.HasFlag(flag))
            {
                lines.Add($"      - {flag}");
            }
        }

        return lines.Count > 0 ? Environment.NewLine + string.Join(Environment.NewLine, lines) : "None";
    }
}