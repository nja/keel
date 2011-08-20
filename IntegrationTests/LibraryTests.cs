using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class LibraryTest
    {
        [TestCase("(append)", "NIL")]
        [TestCase("(append '(a b c) '(d e f) '() '(g))", "(A B C D E F G)")]
        [TestCase("(append '(a b c) 'd)", "(A B C . D)")]
        [TestCase("(append 'a)", "A")]
        public void Append(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(define lst '(a b c)) lst (append lst '(d)) lst", "LST", "(A B C)", "(A B C D)", "(A B C)")]
        public void Append(string input, params string[] output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(cond)", "NIL")]
        [TestCase("(cond (t 1))", "1")]
        [TestCase("(cond (nil 2) (t 3))", "3")]
        [TestCase(@"
(cond ((car nil) (+ 1 1))
      ((car (list 1 2)) (+ 3 4))
      (nil (undefined foo)))", "7")]
        public void Cond(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(unless t 1)", "NIL")]
        [TestCase("(unless t unbound)", "NIL")]
        [TestCase("(unless nil 2)", "2")]
        [TestCase("(unless (cdr '(x)) 3)", "3")]
        public void Unless(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(when nil 1)", "NIL")]
        [TestCase("(when (cdr nil) 2)", "NIL")]
        [TestCase("(when t 3)", "3")]
        [TestCase("(when (car '(x)) 4)", "4")]
        public void When(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(length nil)", "0")]
        [TestCase("(length '(1))", "1")]
        [TestCase("(length '(1 2))", "2")]
        [TestCase("(length '(a b c))", "3")]
        [TestCase("(length '(a b (1 2 3) d))", "4")]
        public void Length(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(let ())", "NIL")]
        [TestCase("(let ((x)) x)", "NIL")]
        [TestCase("(let ((x 1)) x)", "1")]
        [TestCase("(let ((x) (y 2)) (list x y))", "(NIL 2)")]
        [TestCase("(let ((x 1)) (list x (let ((x 2)) x)))", "(1 2)")]
        public void Let(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(let* ())", "NIL")]
        [TestCase("(let* ((x)) x)", "NIL")]
        [TestCase("(let* ((x 1) (y (+ x 1))) (list x y))", "(1 2)")]
        public void LetStar(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(list)", "NIL")]
        [TestCase("(list 'a)", "(A)")]
        [TestCase("(list 'a 'b)", "(A B)")]
        [TestCase("(list (list))", "(NIL)")]
        public void List(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(list* 'a)", "A")]
        [TestCase("(list* 'a 'b)", "(A . B)")]
        [TestCase("(list* 'a 'b 'c)", "(A B . C)")]
        public void ListStar(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(reverse nil)", "NIL")]
        [TestCase("(reverse '(a))", "(A)")]
        [TestCase("(reverse '(a b))", "(B A)")]
        [TestCase("(reverse '(a b c))", "(C B A)")]
        [TestCase("(reverse '(a (b c) d))", "(D (B C) A)")]
        public void Reverse(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(some identity nil)", "NIL")]
        [TestCase("(some identity '(1))", "T")]
        [TestCase("(some identity '(1 2))", "T")]
        [TestCase("(some identity '(nil nil))", "NIL")]
        [TestCase("(some identity '(nil 3 nil))", "T")]
        public void Some(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(every identity nil)", "T")]
        [TestCase("(every identity '(1))", "T")]
        [TestCase("(every identity '(1 2))", "T")]
        [TestCase("(every identity '(nil))", "NIL")]
        [TestCase("(every identity '(1 2 nil 3))", "NIL")]
        public void Every(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(null nil)", "T")]
        [TestCase("(null (list))", "T")]
        [TestCase("(null '())", "T")]
        [TestCase("(null 'NIL)", "T")]
        [TestCase("(null (car '(nil)))", "T")]
        [TestCase("(null (list 1))", "NIL")]
        [TestCase("(null 1)", "NIL")]
        [TestCase("(null (lambda () nil))", "NIL")]
        public void Null(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }

        [TestCase("(map car nil)", "NIL")]
        [TestCase("(map cons nil nil)", "NIL")]
        [TestCase("(map car '((1 2) (3 4) nil))", "(1 3 NIL)")]
        [TestCase("(map cons '(1 2 3) '(4 5 nil))", "((1 . 4) (2 . 5) (3))")]
        public void Map(string input, string output)
        {
            new IntegrationTest().Test(input, output);
        }
    }
}
