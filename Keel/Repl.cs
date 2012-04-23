namespace Keel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Keel.Builtins;
    using Keel.Objects;

    public class Repl
    {
        #region Constants and Fields

        private const string ContPrompt = "  ";

        private const string Prompt = "> ";

        private const string Result = " => ";

        private readonly LispEnvironment environment;

        private readonly TextReader input;
        private readonly TextWriter output;

        private readonly Reader reader = new Reader();

        private readonly Symbol[] stars;

        private readonly Tokenizer tokenizer = new Tokenizer();

        #endregion

        #region Constructors and Destructors

        public Repl(TextReader input, TextWriter output)
        {
            this.environment = new LibraryEnvironment(reader);

            this.stars = InternStars();

            this.input = input;
            this.output = output;
        }

        #endregion

        #region Public Methods and Operators

        public void Loop()
        {
            string line;
            var unreadTokens = new List<Token>();
            bool loop = true;
            var loopEnv = GetLoopEnvironment(() => loop = false);
            var previousResults = new Queue<LispObject>();

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

                bool doneReading;
                
                try
                {
                    doneReading = reader.TryRead(unreadTokens, loopEnv.Symbols, out forms);
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
                        var results = forms.Select(loopEnv.Eval).ToList();

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

        #endregion

        #region Methods

        private LispEnvironment GetLoopEnvironment(Action action)
        {
            var symbol = environment.Symbols.Intern("QUIT");
            var builtin = new DelegateBuiltin(symbol.Name, () => { action(); return T.True; });

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
            var name = string.Empty;
            var symbols = new Symbol[3];

            for (int i = 0; i < symbols.Length; i++)
            {
                name += "*";
                symbols[i] = environment.Symbols.Intern(name);
            }

            return symbols;
        }

        #endregion
    }
}
