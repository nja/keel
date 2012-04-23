namespace IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Keel;
    using Keel.Builtins;
    using Keel.Objects;

    using NUnit.Framework;

    public class IntegrationTest
    {
        #region Constants and Fields

        private readonly LispEnvironment env;

        private readonly Reader reader;

        #endregion

        #region Constructors and Destructors

        public IntegrationTest()
        {
            this.reader = new Reader();
            this.env = new LibraryEnvironment(this.reader);
        }

        public IntegrationTest(string setup)
            : this()
        {
            this.Setup(setup);
        }

        #endregion

        #region Public Methods and Operators

        public void Test(string input, params string[] output)
        {
            IList<LispObject> forms = this.reader.Read(input, this.env.Symbols);
            List<LispObject> results = forms.Select(f => this.env.Eval(f)).ToList();

            for (int i = 0; i < Math.Max(forms.Count, output.Length); i++)
            {
                if (forms.Count <= i)
                {
                    Assert.Fail("No input form for output: " + output[i]);
                }
                else if (output.Length <= i)
                {
                    Assert.Fail("No output for input form: " + forms[i]);
                }

                string printedForm = Print.PrintObject(forms[i]);
                string printedResult = Print.PrintObject(results[i]);
                Assert.AreEqual(
                    output[i], 
                    printedResult, 
                    string.Format("Wanted {0} got {1} from {2}", output[i], printedResult, printedForm));
            }
        }

        #endregion

        #region Methods

        private void Setup(string setup)
        {
            IList<LispObject> setupForms = this.reader.Read(setup, this.env.Symbols);

            foreach (LispObject form in setupForms)
            {
                this.env.Eval(form);
            }
        }

        #endregion
    }
}