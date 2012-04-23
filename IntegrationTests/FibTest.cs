namespace IntegrationTests
{
    using NUnit.Framework;

    [TestFixture]
    public class FibTest
    {
        [TestCase("(fib 0)", "0")]
        [TestCase("(fib 1)", "1")]
        [TestCase("(fib 2)", "1")]
        [TestCase("(fib 3)", "2")]
        [TestCase("(fib 10)", "55")]
        [TestCase("(fib 100)", "354224848179261915075")]
        public void Fibonacci(string input, string output)
        {
            var test = new IntegrationTest(@"
(defun fib (n)
  (defun iter (i p f)
    (if (eq i n)
        f
        (iter (+ i 1) f (+ p f))))
  (iter 0 1 0))
");
            test.Test(input, output);
        }
    }
}
