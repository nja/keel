using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Print : Builtin
    {
        public static readonly Print Instance = new Print();

        private Print()
            : base("PRINT", "X")
        { }

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            PrintObject(args[0]);
            return args[0];
        }

        public static void PrintObject(LispObject x)
        {
            if (x.IsAtom)
            {
                Console.Write(x.ToString());
            }
            else
            {
                Console.Write("(");
                PrintObject(Car.Of(x));
                PrintCdr(Cdr.Of(x));
                Console.Write(")");
            }
        }

        private static void PrintCdr(LispObject x)
        {
            if (x.IsNil)
            {
                return;
            }
            else if (x.IsAtom)
            {
                Console.Write(" . " + x);
            }
            else
            {
                Console.Write(" ");
                PrintObject(Car.Of(x));
                PrintCdr(Cdr.Of(x));
            }
        }
    }
}
