namespace ShortcutShare
{
    internal static class BinaryExtentions
    {
        /// <summary>
        /// Reads a 16-byte GUID/UUID from the stream and converts it to a string.
        /// </summary>
        /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
        public static Guid ReadGuid(this BinaryReader reader)
        {
            Span<byte> buffer = stackalloc byte[16];
            reader.BaseStream.ReadExactly(buffer);
            return new Guid(buffer);
        }

        /// <summary>
        /// Reads a 16-byte GUID/UUID from the stream and returns it as a formatted string.
        /// </summary>
        /// <returns>The GUID as a formatted string (e.g., "00021401-0000-0000-C000-000000000046").</returns>
        public static string ReadGuidString(this BinaryReader reader)
        {
            return reader.ReadGuid().ToString();
        }
    }
}
