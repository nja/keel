namespace Keel
{
    using System.Collections.Generic;
    using System.IO;

    public class Tokenizer
    {
        #region Public Methods and Operators

        public IEnumerable<Token> Tokenize(TextReader reader)
        {
            var tokens = new LinkedList<Token>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                foreach (var token in Tokenize(line))
                {
                    tokens.AddLast(token);
                }
            }

            return tokens;
        }

        public IEnumerable<Token> Tokenize(IEnumerable<char> chars)
        {
            var tokens = new LinkedList<Token>();
            State state = new SkippingWhitespace(tokens);

            foreach (var ch in chars)
            {
                state = state.Handle(ch);                
            }

            state.End();

            return tokens;
        }

        #endregion

        private class BuildingToken : State
        {
            #region Constants and Fields

            private const string OneCharTokens = "()'";

            private string token = string.Empty;

            #endregion

            #region Constructors and Destructors

            public BuildingToken(State state)
                : base(state)
            { }

            #endregion

            #region Public Methods and Operators

            public override void End()
            {
                if (!IsFirstChar())
                {
                    Add(new Token(token));
                }
            }

            public override State Handle(char ch)
            {
                if (IsOneCharToken(ch))
                {
                    if (!IsFirstChar())
                    {
                        Add(new Token(token));
                    }

                    Add(new Token(ch));

                    return new SkippingWhitespace(this);
                }
                
                if (ch.IsWhiteSpace())
                {
                    this.Add(new Token(this.token));
                    return new SkippingWhitespace(this);
                }
                
                this.token += ch;
                return this;
            }

            #endregion

            #region Methods

            private bool IsFirstChar()
            {
                return token == string.Empty;
            }

            private bool IsOneCharToken(char ch)
            {
                return OneCharTokens.IndexOf(ch) > -1;
            }

            #endregion
        }

        private class SkippingWhitespace : State
        {
            #region Constructors and Destructors

            public SkippingWhitespace(State state)
                : base(state)
            { }

            public SkippingWhitespace(ICollection<Token> tokens)
                : base(tokens)
            { }

            #endregion

            #region Public Methods and Operators

            public override void End()
            { }

            public override State Handle(char ch)
            {
                if (ch.IsWhiteSpace())
                {
                    return this;
                }
                
                var next = new BuildingToken(this);
                return next.Handle(ch);
            }

            #endregion
        }

        private abstract class State
        {
            #region Constants and Fields

            private readonly ICollection<Token> tokens;

            #endregion

            #region Constructors and Destructors

            protected State(ICollection<Token> tokens)
            {
                this.tokens = tokens;
            }

            protected State(State state)
            {
                this.tokens = state.tokens;
            }

            #endregion

            #region Public Methods and Operators

            public abstract void End();

            public abstract State Handle(char ch);

            #endregion

            #region Methods

            protected void Add(Token token)
            {
                tokens.Add(token);
            }

            #endregion
        }
    }
}
