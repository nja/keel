namespace Keel
{
    using System;
    using System.Globalization;

    public class Token
    {
        #region Constants and Fields

        private readonly string name;

        #endregion

        #region Constructors and Destructors

        public Token(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            if (name == string.Empty)
            {
                throw new ArgumentException("Empty token name", "name");
            }

            if (name[0].IsWhiteSpace())
            {
                throw new ArgumentException("Whitespace name", "name");
            }

            this.name = name;
        }

        public Token(char ch)
            : this(ch.ToString(CultureInfo.InvariantCulture))
        { }

        #endregion

        #region Public Properties

        public string Name { get { return this.name; } }

        #endregion
    }
}