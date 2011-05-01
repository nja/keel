﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class Defun : SpecialForm
    {
        public static readonly Defun Instance = new Defun();

        private Defun()
            : base("DEFUN")
        { }

        public override LispObject Eval(Cons defunBody, LispEnvironment env)
        {
            var symbol = Car.Of(defunBody).As<Symbol>();
            var lambdaList = Car.Of(Cdr.Of(defunBody)).As<Cons>();
            var lambdaBody = Cdr.Of(Cdr.Of(defunBody)).As<Cons>();
            var progn = Progn.Wrap(lambdaBody);

            var lambda = new Lambda(lambdaList, progn);

            env.AddBinding(symbol, lambda);

            return lambda;
        }
    }
}