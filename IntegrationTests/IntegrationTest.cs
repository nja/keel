using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Keel.Builtins;
using Keel;
using Keel.Objects;

namespace IntegrationTests
{
    public class IntegrationTest
    {
        private LispEnvironment Env;
        private Reader Reader;

        public IntegrationTest()
        {
            this.Reader = new Reader();
            this.Env = new LibraryEnvironment(Reader);
        }

        public IntegrationTest(string setup)
            : this()
        {
            Setup(setup);
        }

        private void Setup(string setup)
        {
            var setupForms = Reader.Read(setup, Env.Symbols);

            foreach (var form in setupForms)
            {
                Env.Eval(form);
            }
        }

        public void Test(string input, params string[] output)
        {
            var forms = Reader.Read(input, Env.Symbols);
            var results = forms.Select(f => Env.Eval(f)).ToList();

            for (int i = 0; i < Math.Max(forms.Count, output.Length); i++)
            {
                if (forms.Count <= i)
                {
                    Assert.Fail("No input form for output: " + output[i]);
                }
                else if (output.Length <= i)
                {
                    Assert.Fail("No output for input form: " + forms[i].ToString());
                }

                var printedResult = Print.PrintObject(results[i]);
                Assert.AreEqual(output[i], printedResult);
            }            
        }
    }
}
