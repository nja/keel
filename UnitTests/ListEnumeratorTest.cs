﻿namespace UnitTests
{
    using System.Collections.Generic;

    using Keel;
    using Keel.Objects;

    using NUnit.Framework;

    [TestFixture]
    public class ListEnumeratorTest
    {
        [Test]
        public void EmptyListTest()
        {
            var enumerator = new ListEnumerator(LispNull.Nil);
            Assert.IsFalse(enumerator.MoveNext());
        }

        private int symbolCount;

        [Test]
        public void SingleConsTest()
        {
            var symbol = new Symbol("abc");
            var cons = new Cons(symbol, LispNull.Nil);
            var enumerator = new ListEnumerator(cons);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreSame(symbol, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ListTest(int count)
        {
            var symbols = GetSymbols(count);
            var first = Cons.ToList(symbols);
            var list = new List<LispObject>(first);

            Assert.AreEqual(symbols.Count, list.Count);

            for (int i = 0; i < symbols.Count; i++)
            {
                Assert.AreSame(symbols[i], list[i]);
            }
        }

        private List<Symbol> GetSymbols(int count)
        {
            var list = new List<Symbol>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new Symbol("Symbol" + symbolCount++));
            }

            return list;
        }
    }
}
