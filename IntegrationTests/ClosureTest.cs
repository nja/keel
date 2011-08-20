using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class ClosureTest
    {
        public void Adder()
        {
            var test = new IntegrationTest(@"
(defun make-adder (x)
  (lambda (y)
    (+ x y)))
(define add2 (make-adder 2))
(define add3 (make-adder 3))
");
            test.Test("(add2 3) (add3 3)", "5", "6");
        }

        [Test]
        public void Count()
        {
            var test = new IntegrationTest(@"
(defun make-counter ()
  (let ((count 0))
    (lambda () (set! count (+ 1 count)))))
(define count-a (make-counter))
(define count-b (make-counter))
");
            test.Test("(count-a) (count-b) (count-a) (count-a) (count-b)",
                "1", "1", "2", "3", "2");    
        }
    }
}
