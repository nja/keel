namespace Keel
{
    using System.IO;

    using Keel.Objects;

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
