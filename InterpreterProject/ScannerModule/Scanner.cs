using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Text;

namespace InterpreterProject.ScannerModule
{
    internal class Scanner
    {
        private readonly string sourceCode;
        private readonly List<Token> tokens = new();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "true", TokenType.TRUE },
            { "fun", TokenType.FUN },
            { "for", TokenType.FOR },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };

        
        public Scanner(string sourceCode) 
        {
            this.sourceCode = sourceCode;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return this.tokens;
        }

        private bool IsAtEnd()
        {
            return current >= sourceCode.Length;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {                
                case '(': AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')': AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }                        
                    }
                    else if (Match('*'))
                    {
                        while (!IsAtEnd())
                        {
                            if (Peek() == '\n')
                            {
                                line++;
                            }

                            if (Peek() == '*' && PeekNext() == '/')
                            {
                                break;
                            }

                            Advance();
                        }     
                        
                        if (IsAtEnd())
                        {
                            Lox.Error(line, "Unterminated comment");
                            return;
                        }

                        // closing */
                        current = current + 2;
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    String();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Lox.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string text = sourceCode[start..current];
            TokenType type = keywords.GetValueOrDefault(text);
            if (type == TokenType.NONE)
            {
                type = TokenType.IDENTIFIER;
            }
            AddToken(type);
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAlpha(char c)
        {
            return  (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c == '_');
        }

        private void Number()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.NUMBER, double.Parse(sourceCode[start..current]));
        }

        private char PeekNext()
        {
            if (current + 1 >= sourceCode.Length)
            {
                return '\0';
            }

            return sourceCode[current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    line++;
                }

                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string");
                return;
            }

            // The closing
            Advance();

            // Trim the surrounding quotes
            string value = sourceCode[(start + 1) .. (current - 1)];
            AddToken(TokenType.STRING, value);
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }

            return sourceCode[current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }

            if (sourceCode[current] != expected)
            {
                return false;
            }

            current++;
            return true;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? literal)
        {
            string text = sourceCode[start .. current];
            tokens.Add(new Token(type, text, literal, line));
        }

        private char Advance()
        {
            current++;
            return sourceCode[current - 1];
        }
    }
}
