namespace TagsCloudCli
{
    public static class CharExtensions
    {
        public static bool IsHex(this char symbol)
        {
            symbol = char.ToLower(symbol);
            return ('0' <= symbol && symbol <= '9') ||
                   ('a' <= symbol && symbol <= 'f');
        }
    }
}
