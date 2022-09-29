namespace Minsk.CodeAnalysis
{
    class Lexer
    {
        private readonly string _text;
        private int _postion;
        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            this._text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current
        {
            get
            {
                if (_postion >= _text.Length)
                    return '\0';
                return _text[_postion];
            }
        }

        private void Next()
        {
            _postion++;
        }

        public SyntaxToken NextToken()
        {

            if (_postion >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _postion, "\0", null);

            if (char.IsDigit(Current))
            {
                var start = _postion;

                while (char.IsDigit(Current))
                    Next();

                var length = _postion - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                {
                    _diagnostics.Add($"The number {_text} isn't valid Int32");
                }
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _postion;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _postion - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _postion++, "+", null);
            else if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToken, _postion++, "-", null);
            else if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _postion++, "*", null);
            else if (Current == '/')
                return new SyntaxToken(SyntaxKind.SlashToken, _postion++, "/", null);
            else if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _postion++, "(", null);
            else if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _postion++, ")", null);

            _diagnostics.Add($"ERROR - bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _postion++, _text.Substring(_postion - 1, 1), null);
        }

    }

}