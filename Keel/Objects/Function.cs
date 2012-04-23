namespace Keel.Objects
{
    public abstract class Function : LispObject
    {
        #region Constants and Fields

        private readonly LispObject args;

        #endregion

        #region Constructors and Destructors

        public Function(LispObject args)
        {
            this.args = args;
        }

        #endregion

        #region Public Properties

        public LispObject Arguments
        {
            get { return args; }
        }

        #endregion

        #region Properties

        protected string ArgumentsString
        {
            get { return Arguments.IsNil ? "()" : Arguments.ToString(); }
        }

        #endregion

        #region Public Methods and Operators

        public abstract LispObject Apply(Cons arguments, LispEnvironment env);

        #endregion
    }
}
