namespace UnitTests
{
    using System.Collections.Generic;

    using Keel;

    using NUnit.Framework;

    [TestFixture]
    public class TokenizerTest
    {
        [Test]
        public void EmptyStringNoTokensTest()
        {
            var tokenizer = new Tokenizer();
            var tokens = new List<Token>(tokenizer.Tokenize(string.Empty));
            Assert.IsEmpty(tokens);
        }

        [TestCase("", 0)]
        [TestCase("a", 1)]
        [TestCase("aaa", 1)]
        [TestCase("aba bab", 2)]
        [TestCase("(xxx", 2)]
        public void TokenCountTest(string str, int count)
        {
            var tokenizer = new Tokenizer();
            var tokens = new List<Token>(tokenizer.Tokenize(str));
            Assert.AreEqual(count, tokens.Count);
        }

        [TestCase("foo bar", "foo", "bar")]
        [TestCase("(foo, bar", "(", "foo,", "bar")]
        [TestCase("   foo   'foo!   ", "foo", "'", "foo!")]
        [TestCase("123 foo 123!", "123", "foo", "123!")]
        [TestCase("foo (bar) fie", "foo", "(", "bar", ")", "fie")]
        [TestCase("'aaa'bbb'", "'", "aaa", "'", "bbb")]
        [TestCase("'(abc)", "'", "(", "abc", ")")]
        public void TokensTest(string text, params string[] tokenNames)
        {
            var tokenizer = new Tokenizer();
            var tokens = new List<Token>(tokenizer.Tokenize(text));

            for (int i = 0; i < tokenNames.Length; i++)
            {
                Assert.AreEqual(tokenNames[i], tokens[i].Name);
            }
        }
    }
}
