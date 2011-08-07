using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using System.IO;

namespace Keel.Builtins
{
    public class Print : Builtin
    {
        public Print()
            : base("PRINT", "X")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            Console.WriteLine(PrintObject(argumentValues.Car));
            return argumentValues.Car;
        }

        public static string PrintObject(LispObject x)
        {
            var sb = new StringBuilder();
            PrintObject(x, sb);
            return sb.ToString();
        }

        private static void PrintObject(LispObject x, StringBuilder output)
        {
            if (x.IsAtom)
            {
                output.Append(x.ToString());
            }
            else
            {
                output.Append("(");
                PrintObject(Car.Of(x), output);
                PrintCdr(Cdr.Of(x), output);
                output.Append(")");
            }
        }

        private static void PrintCdr(LispObject x, StringBuilder output)
        {
            if (x.IsNil)
            {
                return;
            }
            else if (x.IsAtom)
            {
                output.Append(" . " + x);
            }
            else
            {
                output.Append(" ");
                PrintObject(Car.Of(x), output);
                PrintCdr(Cdr.Of(x), output);
            }
        }
    }
}
