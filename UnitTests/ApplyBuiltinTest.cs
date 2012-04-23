namespace UnitTests
{
    using Keel.Objects;
    using Keel.Builtins;

    using NUnit.Framework;

    [TestFixture]
    public class ApplyBuiltinTest
    {
        [Test]
        public void SpreadNilTest()
        {
            var args = new Cons(LispNull.Nil, LispNull.Nil);
            var result = ApplyBuiltin.Spread(args);
            Assert.AreSame(LispNull.Nil, result);
        }

        [Test]
        public void SpreadTest()
        {
            Symbol a = new Symbol("A"),
                   b = new Symbol("B"),
                   c = new Symbol("C");

            var args = Cons.ToList(a, b, Cons.ToList(c));

            var result = ApplyBuiltin.Spread(args);

            Assert.AreSame(a, result.Car);
            Assert.AreSame(b, result.Cdr.As<Cons>().Car);
            Assert.AreSame(c, result.Cdr.As<Cons>().Cdr.As<Cons>().Car);
        }
    }
}
