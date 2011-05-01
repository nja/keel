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
        private LispEnvironment environment = new DefaultEnvironment();

        const string Prompt = "> ",
                 ContPrompt = "  ",
                     Result = " => ";

        public Repl(TextReader input, TextWriter output)
        {
            InitLibrary();

            this.input = input;
            this.output = output;
        }

        private void InitLibrary()
        {
            foreach (var form in reader.Read(Library.Text, symbols))
            {
                environment.Eval(form);
            }
        }

        public void Loop()
        {
            string line;
            List<Token> unreadTokens = new List<Token>();

            output.Write(Prompt);

            while ((line = input.ReadLine()) != null)
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
                        var results = forms.Select(f => environment.Eval(f)).ToList();

                        foreach (var r in results)
                        {
                            Console.Write(Result);
                            Print.PrintObject(r);
                            Console.WriteLine();
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
