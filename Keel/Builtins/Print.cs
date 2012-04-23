namespace Keel.Builtins
{
    using System;
    using System.Text;

    using Keel.Objects;

    public class Print : Builtin
    {
        public Print()
            : base("PRINT", "X")
        { }

        public static string PrintObject(LispObject x)
        {
            var sb = new StringBuilder();
            PrintObject(x, sb);
            return sb.ToString();
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            Console.WriteLine(PrintObject(argumentValues.Car));
            return argumentValues.Car;
        }

        private static void PrintObject(LispObject x, StringBuilder output)
        {
            if (x.IsAtom)
            {
                output.Append(x.ToString());
            }
            else if (Car.Of(x) is Symbol
                     && ((Symbol)Car.Of(x)).SameName("QUOTE")
                     && !Cdr.Of(x).IsNil
                     && Cdr.Of(Cdr.Of(x)).IsNil)
            {
                output.Append("'");
                PrintObject(Car.Of(Cdr.Of(x)), output);
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
            
            if (x.IsAtom)
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
