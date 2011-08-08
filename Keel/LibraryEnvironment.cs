using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using System.IO;

namespace Keel
{
    public class LibraryEnvironment : LispEnvironment
    {
        public LibraryEnvironment(Reader reader)
            : base(new DefaultEnvironment())
        {
            Read(reader);
        }

        private void Read(Reader reader)
        {
            using (var file = new StreamReader("Library.lisp"))
            {
                foreach (var form in reader.Read(file, Symbols))
                {
                    Eval(form);
                }
            }
        }
    }
}
