using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel;
using NUnit.Framework;
using Keel.Objects;
using Keel.Builtins;

namespace UnitTests
{
    [TestFixture]
    public class ReaderTest
    {
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

        private int tokenCount = 0;

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

        [Test]
        public void ReadEmptyListTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = new string[] { "(", ")" }.Select(s => new Token(s));

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
            
            var tokens = new List<Token>();
            tokens.Add(new Token("("));
            tokens.AddRange(innerTokens);
            tokens.Add(new Token(")"));

            var result = reader.Read(tokens, symbols);

            Assert.AreEqual(1, result.Count);

            AssertList(result[0], innerTokens);
        }

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

        [Test]
        public void ReadNestedLists()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var tokens = new string[] {
                // (a ((b)) ())
                "(","a", "(","(","b",")",")", "(",")" ,")"
            }.Select(s => new Token(s)).ToList<Token>();

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

        [TestCase("(")]
        [TestCase(")")]
        [ExpectedException(typeof(ReaderException))]
        public void ReadUnbalancedExceptionTest(string tokenName)
        {
            ReadUnbalancedExceptionTest(new string[] { tokenName });
        }

        [TestCase(")")]
        [ExpectedException(typeof(ReaderException))]
        public void TryReadUnbalancedExceptionTest(string tokenName)
        {
            TryReadUnbalancedExceptionTest(new string[] { tokenName });
        }

        private List<Token> GetTokens(IEnumerable<string> names)
        {
            return names.Select(n => new Token(n)).ToList<Token>();
        }

        private List<Token> GetTokens(params string[] names)
        {
            return GetTokens((IEnumerable<string>)names);
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

        [Test]
        public void SameInternedSymbolTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var name = "foo";
            var tokens = GetTokens(name, name);

            var result = reader.Read(tokens, symbols);
            Assert.AreSame(result[0], result[1]);
        }

        [Test]
        public void TickQuoteAtomTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            var name = "quoted";
            var tokens = GetTokens("'", name);

            var result = reader.Read(tokens, symbols);
            Assert.AreEqual(1, result.Count);
            
            var quote = (Symbol)Car.Of(result[0]);
            var quoted = (Symbol)Car.Of(Cdr.Of(result[0]));
            
            Assert.That(quote.SameName("quote"));
            Assert.That(quoted.SameName(name));
        }

        [Test]
        public void DottedConsTest()
        {
            var symbols = new SymbolsTable();
            var reader = new Reader();

            string carName = "car", cdrName = "cdr";
            var tokens = GetTokens("(", carName, ".", cdrName, ")");

            var result = reader.Read(tokens, symbols);
            Assert.AreEqual(1, result.Count);

            var car = (Symbol)Car.Of(result[0]);
            var cdr = (Symbol)Cdr.Of(result[0]);

            Assert.That(car.SameName(carName));
            Assert.That(cdr.SameName(cdrName));
        }

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
    }
}
