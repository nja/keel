using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel;

namespace KeelConsole
{
    class Program
    {
        static void Main(string[] args)
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
