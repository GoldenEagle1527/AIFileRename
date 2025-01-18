namespace AIFileRename
{
    internal class RegularItem(string regularExpression, string regularReplace, bool regularExpressionCheck)
    {
        string _regularExpression = regularExpression;
        string _regularReplace = regularReplace;
        bool _regularExpressionCheck = regularExpressionCheck;

        public string RegularExpression
        {
            get => _regularExpression;
            set
            {
                if (_regularExpression != value)
                    _regularExpression = value;
            }
        }

        public bool RegularExpressionCheck
        {
            get => _regularExpressionCheck;
            set
            {
                if (_regularExpressionCheck != value)
                    _regularExpressionCheck = value;
            }
        }

        public string RegularReplace
        {
            get => _regularReplace;
            set
            {
                if (_regularReplace != value)
                    _regularReplace = value;
            }
        }

    }
}