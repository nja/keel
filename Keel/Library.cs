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

(defun not (x)
  (if (eq x nil)
    t
    nil))

(defun length (lst)
  (if (null lst)
      0
      (+ 1 (length (cdr lst)))))

(defun list x x)

(defun caar (x) (car (car x)))
(defun cadr (x) (car (cdr x)))

(defun reverse (lst)
  (defun recur (acc lst)
    (if (null lst)
        acc
        (recur (cons (car lst) acc) (cdr lst))))
  (recur nil lst))

(defun some (test list)
  (if (null list)
      nil
      (if (test (car list))
          t
          (some test (cdr list)))))

(defun every (test list)
  (if (null list)
      t
      (if (test (car list))
          (every test (cdr list))
          nil)))

(defun map (fn . lists)
  (defun take (fn lists)
    (if (null lists)
        nil
        (cons (fn (car lists))
              (take fn (cdr lists)))))
  (defun cars () (take car lists))
  (defun cdrs () (take cdr lists))
  (if (some null lists)
      nil
      (cons (apply fn (cars))
            (apply map fn (cdrs)))))

";
    }
}
