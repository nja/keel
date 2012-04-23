namespace UnitTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Keel;
    using Keel.Builtins;
    using Keel.Objects;

    using NUnit.Framework;

    [TestFixture]
    public class ReaderTest
    {
        #region Constants and Fields

        private int tokenCount;

        #endregion

        #region Public Methods and Operators

        [TestCase("foo", ".")]
        [TestCase("foo", ".", "bar")]
        [TestCase("(", ".", "foo")]
        [TestCase("foo", ".", "bar", ")")]
        [TestCase("(", "bar", ".")]
        [TestCase("(", "bar", ".", ")")]
        [ExpectedException(typeof(ReaderException))]
        public void DotExceptionTest(params string[] tokenNames)
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = GetTokens(tokenNames);
            reader.Read(tokens, symbols);
        }

        [Test]
        public void DottedConsTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            const string CarName = "car";
            const string CdrName = "cdr";
            var tokens = GetTokens("(", CarName, ".", CdrName, ")");

            var result = reader.Read(tokens, symbols);
            Assert.AreEqual(1, result.Count);

            var car = (Symbol)Car.Of(result[0]);
            var cdr = (Symbol)Cdr.Of(result[0]);

            Assert.That(car.SameName(CarName));
            Assert.That(cdr.SameName(CdrName));
        }

        [Test]
        public void ReadEmptyListTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = new[] { "(", ")" }.Select(s => new Token(s));

            var result = reader.Read(tokens, symbols);

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(LispNull.Nil, result[0]);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ReadListTest(int count)
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();
            var innerTokens = GetTokens(count);
            
            var tokens = new List<Token> { new Token("(") };
            tokens.AddRange(innerTokens);
            tokens.Add(new Token(")"));

            var result = reader.Read(tokens, symbols);

            Assert.AreEqual(1, result.Count);

            AssertList(result[0], innerTokens);
        }

        [Test]
        public void ReadNestedLists()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = new[] 
            {
                // (a ((b)) ())
                "(", "a", "(", "(", "b", ")", ")", "(", ")", ")"
            }.Select(s => new Token(s)).ToList();

            var result = reader.Read(tokens, symbols);

            Assert.AreEqual(1, result.Count);

            var list = (Cons)result[0];
            var car = (Symbol)Car.Of(list);
            Assert.That(car.SameName("a"));

            var cdr = Cdr.Of(list); // (((b)) ())
            var cadr = (Cons)Car.Of(cdr); // ((b))
            Assert.That(cadr.Cdr.IsNil);
            var caadr = (Cons)Car.Of(cadr); // (b)
            Assert.That(caadr.Cdr.IsNil);
            var caaadr = (Symbol)Car.Of(caadr); // b
            Assert.That(caaadr.SameName("b"));

            var cddr = (Cons)Cdr.Of(cdr); // (())
            Assert.That(cddr.Car.IsNil);
            Assert.That(cddr.Cdr.IsNil);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ReadSymbolsTest(int count)
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = GetTokens(count);

            var result = reader.Read(tokens, symbols);

            Assert.AreEqual(tokens.Count, result.Count);

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                var symbol = (Symbol)result[i];
                Assert.That(symbol.SameName(token.Name));
            }
        }

        [TestCase("(")]
        [TestCase(")")]
        [ExpectedException(typeof(ReaderException))]
        public void ReadUnbalancedExceptionTest(string tokenName)
        {
            ReadUnbalancedExceptionTest(new[] { tokenName });
        }

        [TestCase("(", "a")]
        [TestCase("(", "a", "(", ")")]
        [TestCase("(", "a", ")", ")")]
        [ExpectedException(typeof(ReaderException))]
        public void ReadUnbalancedExceptionTest(params string[] tokenNames)
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = GetTokens(tokenNames);

            reader.Read(tokens, symbols);
        }

        [Test]
        public void SameInternedSymbolTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            const string Name = "foo";
            var tokens = GetTokens(Name, Name);

            var result = reader.Read(tokens, symbols);
            Assert.AreSame(result[0], result[1]);
        }

        [Test]
        public void TickQuoteAtomTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            const string Name = "quoted";
            var tokens = GetTokens("'", Name);

            var result = reader.Read(tokens, symbols);
            Assert.AreEqual(1, result.Count);
            
            var quote = (Symbol)Car.Of(result[0]);
            var quoted = (Symbol)Car.Of(Cdr.Of(result[0]));
            
            Assert.That(quote.SameName("quote"));
            Assert.That(quoted.SameName(Name));
        }

        [TestCase(")")]
        [ExpectedException(typeof(ReaderException))]
        public void TryReadUnbalancedExceptionTest(string tokenName)
        {
            TryReadUnbalancedExceptionTest(new[] { tokenName });
        }

        [TestCase("(", "a", ")", ")")]
        [ExpectedException(typeof(ReaderException))]
        public void TryReadUnbalancedExceptionTest(params string[] tokenNames)
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = GetTokens(tokenNames);

            IList<LispObject> result;
            reader.TryRead(tokens, symbols, out result);
        }

        #endregion

        #region Methods

        private void AssertList(LispObject list, List<Token> tokens)
        {
            if (list.IsNil)
            {
                Assert.IsEmpty(tokens);
            }
            else
            {
                var cons = (Cons)list;
                var symbol = (Symbol)cons.Car;
                
                Assert.That(symbol.SameName(tokens[0].Name));

                AssertList(cons.Cdr, tokens.GetRange(1, tokens.Count - 1));
            }
        }

        private List<Token> GetTokens(int count)
        {
            var tokens = new List<Token>();

            for (int i = 0; i < count; i++)
            {
                var name = "TokenName" + tokenCount++;
                tokens.Add(new Token(name));
            }

            return tokens;
        }

        private List<Token> GetTokens(IEnumerable<string> names)
        {
            return names.Select(n => new Token(n)).ToList();
        }

        private List<Token> GetTokens(params string[] names)
        {
            return GetTokens((IEnumerable<string>)names);
        }

        #endregion
    }
}
