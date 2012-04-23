using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntegrationTests
{
    using System.Globalization;

    [TestFixture]
    public class BuiltinTests
    {
        [TestCase("(+)", "0")]
        [TestCase("(+ 1)", "1")]
        [TestCase("(+ 1 2)", "3")]
        [TestCase("(+ -1)", "-1")]
        [TestCase("(+ 1 -2 3 -4)", "-2")]
        [TestCase("(+ 1 2000000000 2000000000)", "4000000001")]
        [TestCase("(+ 2000000000 2000000000)", "4000000000")]
        [TestCase("(+ -2000000000 -2000000000)", "-4000000000")]
        public void Add(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(apply + nil)", "0")]
        [TestCase("(apply + '(1))", "1")]
        [TestCase("(apply + 1 '(2))", "3")]
        [TestCase("(apply + 1 2 '(3))", "6")]
        [TestCase("(apply + 1 2 3 '(4 5))", "15")]
        public void Apply(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(atom nil)", "T")]
        [TestCase("(atom 'x)", "T")]
        [TestCase("(atom 1)", "T")]
        [TestCase("(atom (lambda (x) x))", "T")]
        [TestCase("(atom (list))", "T")]
        [TestCase("(atom (list 1))", "NIL")]
        [TestCase("(atom (cons 1 2))", "NIL")]
        public void Atom(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(car nil)", "NIL")]
        [TestCase("(car '(a))", "A")]
        [TestCase("(car '((1 2) 3))", "(1 2)")]
        public void Car(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(cdr nil)", "NIL")]
        [TestCase("(cdr (cons 1 2))", "2")]
        [TestCase("(cdr '(1 2))", "(2)")]
        [TestCase("(cdr '(1 2 3))", "(2 3)")]
        [TestCase("(cdr (cons 1 (cons 2 3)))", "(2 . 3)")]
        public void Cdr(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(cons 1 2)", "(1 . 2)")]
        [TestCase("(cons 1 nil)", "(1)")]
        [TestCase("(cons nil 2)", "(NIL . 2)")]
        [TestCase("(cons 1 (cons 2 (cons 3 (cons 4 nil))))", "(1 2 3 4)")]
        [TestCase("(cons 'a 'b)", "(A . B)")]
        [TestCase("(cons 'a (cons 'b (cons 'c '())))", "(A B C)")]
        [TestCase("(cons 'a '(b c d))", "(A B C D)")]
        public void Cons(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(consp nil)", "NIL")]
        [TestCase("(consp 1)", "NIL")]
        [TestCase("(consp (lambda () t))", "NIL")]
        [TestCase("(consp (cons 1 2))", "T")]
        public void Consp(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(eq nil nil)", "T")]
        [TestCase("(eq t t)", "T")]
        [TestCase("(eq nil t)", "NIL")]
        [TestCase("(eq 'x 'x)", "T")]
        [TestCase("(eq 'x 'y)", "NIL")]
        [TestCase("(eq 1 1)", "T")]
        [TestCase("(eq 1 0)", "NIL")]
        [TestCase("(eq (+ 1 2) 3)", "T")]
        [TestCase("(eq 0 nil)", "NIL")]
        [TestCase("(eq '() '())", "T")]
        [TestCase("(eq (cons 1 2) (cons 1 2))", "NIL")]
        [TestCase("(let ((x (cons 1 2))) (eq x x))", "T")]
        [TestCase("(let* ((x (cons 1 2)) (y x)) (eq x y))", "T")]
        public void Eq(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(eval nil)", "NIL")]
        [TestCase("(eval t)", "T")]
        [TestCase("(eval '(+ 1 2))", "3")]
        public void Eval(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(define x 2) (eval 'x)", "X", "2")]
        public void Eval(string input, params string[] output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(- 1)", "-1")]
        [TestCase("(- 4000000000)", "-4000000000")]
        [TestCase("(- 6 3 2 1)", "0")]
        [TestCase("(- 1 .01 .001)", "0.989")]
        [TestCase("(- 2000000000 -2000000000)", "4000000000")]
        public void Subtract(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("1")]
        [TestCase("0.0625")]
        public void Wiggle(string input)
        {
            var test = new IntegrationTest();
            var smidgen = Math.Pow(2, -32).ToString(CultureInfo.InvariantCulture);

            test.Test(string.Format("(+ {0} {0} -{0})", input), input);
            test.Test(string.Format("(= {0} (+ {0} {0} -{0}))", input), "T");

            test.Test(string.Format("(+ {0} {1} -{1})", input, smidgen), input);
            test.Test(string.Format("(= {0} (+ {0} {1} -{1}))", input, smidgen), "T");

            test.Test(string.Format("(< {0} (+ {0} {1}))", input, smidgen), "T");
            test.Test(string.Format("(< (- {0} {1}) {0})", input, smidgen), "T");
            
            test.Test(string.Format("(> {0} (- {0} {1}))", input, smidgen), "T");
            test.Test(string.Format("(> (+ {0} {1}) {0})", input, smidgen), "T");

            test.Test(string.Format("(= {0} (+ {0} {0}))", input), "NIL");
            test.Test(string.Format("(= {0} (+ {0} {1}))", input, smidgen), "NIL");
            test.Test(string.Format("(< {0} (- {0} {1}))", input, smidgen), "NIL");
            test.Test(string.Format("(> {0} (+ {0} {1}))", input, smidgen), "NIL");
        }
    }
}
