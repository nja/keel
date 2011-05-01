﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class Symbol : LispObject
    {
        private readonly string name;

        public Symbol(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public static string Canonicalize(string name)
        {
            return name.ToUpperInvariant();
        }

        public bool SameName(string otherName)
        {
            return Canonicalize(name) == Canonicalize(otherName);
        }
    }
}
