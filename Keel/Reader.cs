namespace Keel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Numerics;

    using Keel.Objects;

    public class Reader
    {
        #region Constants and Fields

        private const NumberStyles DoubleStyle = NumberStyles.Float;

        private const NumberStyles IntegerStyle = NumberStyles.Integer;

        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        #endregion

        #region Public Methods and Operators

        public IList<LispObject> Read(TextReader text, SymbolsTable symbols)
        {
            var tokens = new Tokenizer().Tokenize(text);
            return Read(tokens, symbols);
        }

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
        /// Tries to read the next form. Returns false when a complete form wasn't read.
        /// Assumes tokens.Current is available.
        /// </summary>
        public bool TryRead(IEnumerator<Token> tokens, SymbolsTable symbols, out LispObject result)
        {
            if (OpensList(tokens.Current))
            {
                return TryReadList(tokens, symbols, out result);
            }
            
            if (this.ClosesList(tokens.Current))
            {
                throw new ReaderException("Unbalanced parens");
            }
            
            if (this.IsDot(tokens.Current))
            {
                throw new ReaderException("Spurious dot");
            }
            
            if (this.QuotesNext(tokens.Current))
            {
                tokens.MoveNext();

                var quote = symbols.Intern("QUOTE");
                var next = this.Read(tokens, symbols);

                result = Cons.ToList(new[] { quote, next });
                return true;
            }
            
            result = this.ReadAtom(tokens, symbols);
            return true;
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

                if (IsDot(tokens.Current))
                {
                    if (previousCons == null)
                    {
                        throw new ReaderException("Spurious dot");
                    }

                    if (!tokens.MoveNext())
                    {
                        break;
                    }

                    LispObject cdr;

                    if (TryRead(tokens, symbols, out cdr))
                    {
                        if (!tokens.MoveNext())
                        {
                            break;
                        }

                        if (!ClosesList(tokens.Current))
                        {
                            throw new ReaderException("Unbalanced parens");
                        }

                        previousCons.Cdr = cdr;

                        result = firstCons;
                        return true;
                    }
                    
                    break;
                }

                var cons = new Cons();
                firstCons = firstCons ?? cons;

                if (previousCons != null)
                {
                    previousCons.Cdr = cons;
                }

                LispObject car;
                if (TryRead(tokens, symbols, out car))
                {
                    cons.Car = car;
                }
                else
                {
                    break;
                }

                previousCons = cons;
            }

            result = LispNull.Nil;
            return false;
        }

        #endregion

        #region Methods

        private bool ClosesList(Token token)
        {
            return token.Name == ")";
        }

        private bool IsDot(Token token)
        {
            return token.Name == ".";
        }

        private bool OpensList(Token token)
        {
            return token.Name == "(";
        }

        private bool QuotesNext(Token token)
        {
            return token.Name == "'";
        }

        private LispObject ReadAtom(IEnumerator<Token> tokens, SymbolsTable symbols)
        {
            string name = tokens.Current.Name;
            int intValue;
            double doubleValue;
            BigInteger bigIntValue;

            if (int.TryParse(name, IntegerStyle, FormatProvider, out intValue))
            {
                return new LispInteger(intValue);
            }
            
            if (BigInteger.TryParse(name, IntegerStyle, FormatProvider, out bigIntValue))
            {
                return new LispBigInteger(bigIntValue);
            }
            
            if (double.TryParse(name, DoubleStyle, FormatProvider, out doubleValue))
            {
                return new LispDouble(doubleValue);
            }

            if (Symbol.Canonicalize(name) == LispNull.Name)
            {
                return LispNull.Nil;
            }
            
            return symbols.Intern(name);
        }

        #endregion
    }
}
