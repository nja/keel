using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Keel.Objects;
using Keel.Builtins;

namespace Keel
{
    public class Repl
    {
        private readonly TextReader input;
        private readonly TextWriter output;

        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly Reader reader = new Reader();
        
        private SymbolsTable symbols = new DefaultSymbols();
        private LispEnvironment environment;

        const string Prompt = "> ",
                 ContPrompt = "  ",
                     Result = " => ";

        public Repl(TextReader input, TextWriter output)
        {
            this.environment = GetLibraryEnvironment();

            this.input = input;
            this.output = output;
        }

        private LispEnvironment GetLibraryEnvironment()
        {
            var libEnv = new LispEnvironment(new DefaultEnvironment());

            foreach (var form in reader.Read(Library.Text, symbols))
            {
                libEnv.Eval(form);
            }

            return libEnv;
        }

        private LispEnvironment GetLoopEnvironment(Action action)
        {
            var symbol = symbols.Intern("QUIT");
            var builtin = new DelegateBuiltin(symbol, () => { action(); return DefaultSymbols.T; });

            var loopEnv = new LispEnvironment(environment);
            loopEnv.AddBinding(symbol, builtin);

            return loopEnv;
        }

        public void Loop()
        {
            string line;
            List<Token> unreadTokens = new List<Token>();
            bool loop = true;
            var loopEnv = GetLoopEnvironment(() => loop = false);

            output.Write(Prompt);

            while (loop && (line = input.ReadLine()) != null)
            {
                try
                {
                    unreadTokens.AddRange(tokenizer.Tokenize(line));
                }
                catch (Exception tokenEx)
                {
                    output.WriteLine("Tokenizer exception: " + tokenEx.Message);
                    unreadTokens.Clear();
                    output.Write(Prompt);
                    continue;
                }

                IList<LispObject> forms;

                bool doneReading = false;
                
                try
                {
                     doneReading = reader.TryRead(unreadTokens, symbols, out forms);
                }
                catch (Exception readEx)
                {
                    output.WriteLine("Reader exception: " + readEx.Message);
                    unreadTokens.Clear();
                    output.Write(Prompt);
                    continue;
                }

                if (doneReading)
                {
                    unreadTokens.Clear();

                    try
                    {
                        var results = forms.Select(f => loopEnv.Eval(f)).ToList();

                        foreach (var r in results)
                        {
                            Console.Write(Result);
                            Console.WriteLine(Print.PrintObject(r));
                        }
                    }
                    catch (Exception evalEx)
                    {
                        output.WriteLine("Evaluation exception: " + evalEx.Message);
                        output.Write(Prompt);
                        continue;
                    }

                    output.Write(Prompt);
                }
                else
                {
                    output.Write(ContPrompt);
                }
            }
        }
    }
}
