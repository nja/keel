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
        
        private SymbolsTable symbols;
        private LispEnvironment environment;
        private Symbol[] stars;

        const string Prompt = "> ",
                 ContPrompt = "  ",
                     Result = " => ";

        public Repl(TextReader input, TextWriter output)
        {
            this.symbols = new DefaultSymbols();
            this.environment = GetLibraryEnvironment();

            this.stars = InternStars();

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

            for (int i = 0; i < stars.Length; i++)
            {
                loopEnv.AddBinding(stars[i], LispNull.Nil);
            }

            return loopEnv;
        }

        private Symbol[] InternStars()
        {
            var name = "";
            var stars = new Symbol[3];

            for (int i = 0; i < stars.Length; i++)
            {
                name += "*";
                stars[i] = symbols.Intern(name);
            }

            return stars;
        }

        public void Loop()
        {
            string line;
            List<Token> unreadTokens = new List<Token>();
            bool loop = true;
            var loopEnv = GetLoopEnvironment(() => loop = false);
            Queue<LispObject> previousResults = new Queue<LispObject>();

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
                            previousResults.Enqueue(r);
                            Console.Write(Result);
                            Console.WriteLine(Print.PrintObject(r));
                        }

                        while (previousResults.Count > stars.Length)
                        {
                            previousResults.Dequeue();
                        }

                        var starValues = previousResults.Reverse().ToArray();
                        for (int i = 0; i < previousResults.Count; i++)
                        {   
                            loopEnv.SetValue(stars[i], starValues[i]);
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
