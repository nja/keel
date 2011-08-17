﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Keel.Objects
{
    public class LispBigInteger : LispNumber
    {
        public readonly BigInteger Value;

        public LispBigInteger(LispInteger value)
        {
            this.Value = value.Value;
        }

        public LispBigInteger(BigInteger value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(FormatProvider);
        }

        public override LispNumber Negate()
        {
            return new LispBigInteger(Value * -1);
        }

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return new LispBigInteger(addend).Add(this);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return new LispDouble(this).Add(addend);
        }

        public override LispNumber Add(LispBigInteger addend)
        {
            return Add(this, addend);
        }

        public override bool NumberEquals(LispNumber number)
        {
            return number.NumberEquals(this);
        }

        public override bool NumberEquals(LispInteger number)
        {
            return NumberEquals(number, this);
        }

        public override bool NumberEquals(LispDouble number)
        {
            return NumberEquals(this, number);
        }

        public override bool NumberEquals(LispBigInteger number)
        {
            return NumberEquals(this, number);
        }
    }
}
