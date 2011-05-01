using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Keel
{
    public class Tokenizer
    {
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

        abstract class State
        {
            private readonly ICollection<Token> tokens;

            protected State(ICollection<Token> tokens)
            {
                this.tokens = tokens;
            }

            protected State(State state)
            {
                this.tokens = state.tokens;
            }

            protected void Add(Token token)
            {
                tokens.Add(token);
            }

            public abstract State Handle(char ch);
            public abstract void End();
        }

        class SkippingWhitespace : State
        {
            public SkippingWhitespace(State state)
                : base(state)
            { }

            public SkippingWhitespace(ICollection<Token> tokens)
                : base(tokens)
            { }

            public override State Handle(char ch)
            {
                if (ch.IsWhiteSpace())
                {
                    return this;
                }
                else
                {
                    var next = new BuildingToken(this);
                    return next.Handle(ch);
                }
            }

            public override void End()
            { }
        }

        class BuildingToken : State
        {
            private string token = "";

            public BuildingToken(State state)
                : base(state)
            { }

            public BuildingToken(ICollection<Token> tokens)
                : base(tokens)
            { }

            const string OneCharTokens = "()'";

            private bool IsOneCharToken(char ch)
            {
                return OneCharTokens.IndexOf(ch) > -1;
            }

            private bool IsFirstChar()
            {
                return token == "";
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
                else if (ch.IsWhiteSpace())
                {
                    Add(new Token(token));
                    return new SkippingWhitespace(this);
                }
                else
                {
                    token += ch;
                    return this;
                }
            }

            public override void End()
            {
                if (!IsFirstChar())
                {
                    Add(new Token(token));
                }
            }
        }
    }

    public class Token
    {
        private readonly string name;

        public Token(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name == "") throw new ArgumentException("Empty token name", "name");
            if (name[0].IsWhiteSpace()) throw new ArgumentException("Whitespace name", "name");

            this.name = name;
        }

        public Token(char ch)
            : this(ch.ToString())
        { }

        public string Name { get { return name; } }
    }
}
