using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public IList<LispObject> Read(IEnumerable<Token> tokens, SymbolsTable symbols)
        {
            var result = new List<LispObject>();

            var enumerator = tokens.GetEnumerator();

            while (enumerator.MoveNext())
            {
                result.Add(Read(enumerator, symbols));
            }

            return result;
        }

        /// <summary>
        /// Assumes tokens.Current is available
        /// </summary>
        public LispObject Read(IEnumerator<Token> tokens, SymbolsTable symbols)
        {
            if (OpensList(tokens.Current))
            {
                return ReadList(tokens, symbols);
            }
            else if (ClosesList(tokens.Current))
            {
                throw new ReaderException("Unbalanced parens");
            }
            else
            {
                return ReadAtom(tokens, symbols);
            }
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
        public LispObject ReadList(IEnumerator<Token> tokens, SymbolsTable symbols)
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
                        return LispNull.Nil;
                    }
                    else
                    {
                        return firstCons;
                    }
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

            throw new ReaderException("Unbalanced parens");
        }

        private LispObject ReadAtom(IEnumerator<Token> tokens, SymbolsTable symbols)
        {
            return symbols.Intern(tokens.Current.Name);
        }
    }
}
