namespace Keel.Builtins
{
    using System.Linq;

    using Keel.Objects;

    public abstract class Builtin : Function
    {
        private readonly string name;

        public Builtin(string name, params string[] args)
            : base(Cons.ToList(args.Select(s => new Symbol(s))))
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public override string ToString()
        {
            return string.Format("<Builtin function {0}: {1}>", Name, ArgumentsString);
        }
    }
}
