﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class LispInteger : LispNumber
    {
        public readonly int Value;

        public LispInteger(int value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(FormatProvider);
        }

        public override LispNumber Negate()
        {
            return new LispInteger(-Value);
        }

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return Add(this, addend);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return addend.Add(this);
        }
    }
}
