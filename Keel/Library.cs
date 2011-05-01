using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class Library
    {
        public const string Text = @"
(defun null (x) (eq x nil))

(defun length (lst)
  (if (null lst)
      0
      (+ 1 (length (cdr lst)))))
";
    }
}
