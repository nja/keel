namespace Keel
{
    public static class CharExtensions
    {
        public static bool IsWhiteSpace(this char ch)
        {
            const string Whitespace = " \t\r\n";

            return Whitespace.IndexOf(ch) > -1;
        }
    }
}
