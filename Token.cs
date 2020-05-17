namespace Lox.Lexical
{
    class Token
    {
        private readonly TokenType tokenType;
        private readonly string lexeme;
        private readonly object literal;
        private readonly int line;

        public Token(TokenType tokenType, string lexeme, object literal, int line)
        {
            this.tokenType = tokenType;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            return $"{tokenType} {lexeme} {literal}";
        }
    }
}