using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class SpecialFormTests
    {
        [TestCase("(define x)", "X")]
        public void Define(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(define x 1) x", "X", "1")]
        public void Define(string input, params string[] output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(if t 1)", "1")]
        [TestCase("(if nil 1)", "NIL")]
        [TestCase("(if t 1 2)", "1")]
        [TestCase("(if nil 1 2)", "2")]
        [TestCase("(if (car '(x)) 1 2)", "1")]
        [TestCase("(if (cdr '(x)) 1 2)", "2")]
        public void If(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase(@"
 (do ((temp-one 1 (1+ temp-one))
       (temp-two 0 (1- temp-two)))
      ((> (- temp-one temp-two) 5) temp-one))", "4")]
        [TestCase(@"
 (do ((temp-one 1 (1+ temp-one))
       (temp-two 0 (1+ temp-one)))     
      ((= 3 temp-two) temp-one))", "3")]
        [TestCase(@"
 (do* ((temp-one 1 (1+ temp-one))
        (temp-two 0 (1+ temp-one)))
       ((= 3 temp-two) temp-one))", "2")]
        public void Do(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }
    }
}
