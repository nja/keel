using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel
{
    public class ReaderException : Exception
    {
        public ReaderException(string msg)
            : base(msg)
        { }

        public ReaderException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }

    public class Reader
    {
        public IList<LispObject> Read(IEnumerable<char> text, SymbolsTable symbols)
        {
            var tokens = new Tokenizer().Tokenize(text);
            return Read(tokens, symbols);
        }

        public IList<LispObject> Read(IEnumerable<Token> tokens, SymbolsTable symbols)
        {
            IList<LispObject> result;

            if (!TryRead(tokens, symbols, out result))
            {
                throw new ReaderException("Unbalanced parens");
            }

            return result;
        }

        public bool TryRead(IEnumerable<Token> tokens, SymbolsTable symbols, out IList<LispObject> result)
        {
            result = new List<LispObject>();

            var enumerator = tokens.GetEnumerator();

            while (enumerator.MoveNext())
            {
                LispObject next;
                if (TryRead(enumerator, symbols, out next))
                {
                    result.Add(next);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Assumes tokens.Current is available
        /// </summary>
        public LispObject Read(IEnumerator<Token> tokens, SymbolsTable symbols)
        {
            LispObject result;

            if (!TryRead(tokens, symbols, out result))
            {
                throw new ReaderException("Unbalanced parens");
            }

            return result;
        }

        /// <summary>
        /// Tries to read the next form. Returns false when a complete form wasn't read.
        /// Assumes tokens.Current is available.
        /// </summary>
        public bool TryRead(IEnumerator<Token> tokens, SymbolsTable symbols, out LispObject result)
        {
            if (OpensList(tokens.Current))
            {
                return TryReadList(tokens, symbols, out result);
            }
            else if (ClosesList(tokens.Current))
            {
                throw new ReaderException("Unbalanced parens");
            }
            else if (QuotesNext(tokens.Current))
            {
                tokens.MoveNext();
                
                var quote = symbols.Intern("QUOTE");
                var next = Read(tokens, symbols);

                result = Cons.ToList(new LispObject[] { quote, next });
                return true;
            }
            else
            {
                result = ReadAtom(tokens, symbols);
                return true;
            }
        }

        private bool QuotesNext(Token token)
        {
            return token.Name == "'";
        }

        private bool OpensList(Token token)
        {
            return token.Name == "(";
        }

        private bool ClosesList(Token token)
        {
            return token.Name == ")";
        }

        /// <summary>
        /// Assumes the enumerator is advanced to the opening paren.
        /// </summary>
        public bool TryReadList(IEnumerator<Token> tokens, SymbolsTable symbols, out LispObject result)
        {
            if (!OpensList(tokens.Current))
            {
                throw new ReaderException("Error reading list: no opening paren");
            }

            Cons firstCons = null, 
                previousCons = null;

            while (tokens.MoveNext())
            {
                if (ClosesList(tokens.Current))
                {
                    if (previousCons == null)
                    {
                        result = LispNull.Nil;
                    }
                    else
                    {
                        result = firstCons;
                    }

                    return true;
                }

                Cons cons = new Cons();
                firstCons = firstCons ?? cons;

                if (previousCons != null)
                {
                    previousCons.Cdr = cons;
                }

                cons.Car = Read(tokens, symbols);

                previousCons = cons;
            }

            result = LispNull.Nil;
            return false;
        }

        private LispObject ReadAtom(IEnumerator<Token> tokens, SymbolsTable symbols)
        {
            int intValue;

            if (int.TryParse(tokens.Current.Name, out intValue))
            {
                return new LispInteger(intValue);
            }
            else
            {
                return symbols.Intern(tokens.Current.Name);
            }
        }
    }
}
