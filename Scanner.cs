using System;
using System.Collections.Generic;
using System.Text;
using Lox.Lexical;

namespace Lox
{
    class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.STAR); break;
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '/':
                    if (match('/'))
                    {
                        while (peek() != '\n' && !isAtEnd())
                            advance();
                    }
                    else
                        addToken(TokenType.SLASH);
                    break; // todo
                case '"':
                    int closingPos = closingMatchPos('"');
                    if (closingPos > 0)
                    {
                        string literal = source.Substring(current, closingPos - 1);
                        addToken(TokenType.STRING, literal);
                        current += closingPos;
                    }
                    else
                        Lox.Error(line, $"Missing closing \"");

                    break;
                case 'f':
                    // If we get an f, check if we get a n followed by a space 
                    if (match('n'))
                    {
                        if (match(' '))
                        {
                            addToken(TokenType.FUN);
                        }
                    }
                    break;
                case '\t': break;
                case '\r': break;
                case '\n': line++; break;
                case ' ': break;
                default: Lox.Error(line, $"Unexpected character {c}:{(byte)c}"); break;
            }
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return source[current];
        }

        private int closingMatchPos(char c)
        {
            int closingPos = source.IndexOf(c, current);
            if (closingPos > 0)
                closingPos -= (current - 1);

            return closingPos;
        }

        private bool match(char c)
        {
            if (isAtEnd()) return false;
            if (c != source[current]) return false;

            current++;
            return true;
        }

        private void addToken(TokenType tokenType)
        {
            addToken(tokenType, null);
        }

        private void addToken(TokenType tokenType, Object literal)
        {
            var text = source.Substring(start, current - start);
            tokens.Add(new Token(tokenType, text, literal, line));
        }

        private char advance()
        {
            current++;
            return source[current - 1];
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }
    }
}