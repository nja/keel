using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public abstract class Builtin : Function
    {
        private readonly string name;
        
        public Builtin(string name, params string[] args)
            : base(args.Select(s => new Symbol(s)), LispNull.Nil)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        protected static LO Assert<LO>(LispObject x) where LO: LispObject
        {
            LO lo = x as LO;

            if (lo == null)
            {
                throw new FunctionException(x + " is not of type " + typeof(LO).Name);
            }

            return lo;
        }
    }

    public class Car : Builtin
    {
        public Car()
            : base("CAR", "X")
        { }

        public override LispObject Apply(Environment env)
        {
            return Car.Of(env.LookUp(Args[0]));
        }

        public static LispObject Of(LispObject x)
        {
            Cons cons = Assert<Cons>(x);

            return cons.Car;   
        }
    }

    public class Cdr : Builtin
    {
        public Cdr()
            : base("CDR", "X")
        { }

        public override LispObject Apply(Environment env)
        {
            return Cdr.Of(env.LookUp(Args[0]));
        }

        public static LispObject Of(LispObject x)
        {
            Cons cons = Assert<Cons>(x);

            return cons.Cdr;
        }
    }
}
