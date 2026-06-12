namespace ShellLink;

[Flags]
public enum HotKeyModifierKeys : Byte
{
    None = 0x00,
    Shift = 0x01,
    Control = 0x02,
    Alt = 0x04
}
