namespace KeelConsole
{
    using System;

    using Keel;
    
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Repl repl = new Repl(Console.In, Console.Out);
                repl.Loop();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("REPL exception: " + ex.Message);
            }
        }
    }
}
