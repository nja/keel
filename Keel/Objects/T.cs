namespace Keel.Objects
{
    public class T : Symbol
    {
        public const string TrueName = "T";
        public static readonly T True = new T();

        private T()
            : base(TrueName)
        { }
    }
}
