using InterpreterProject.ScannerModule;
using System.ComponentModel;
using System.Drawing;

namespace InterpreterProject.ParserModule
{
    class Parser
    {
        private class ParseError : Exception;
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Stmt?>? Parse()
        {
            try
            {
                List<Stmt?> statements = new();
                while (!IsAtEnd())
                {
                    statements.Add(Declaration());
                }

                return statements;
            }
            catch (ParseError)
            {

                return null;
            }
        }

        private Stmt? Declaration()
        {
            try
            {
                if (Match(TokenType.VAR)) return VarDeclaration();

                return Statement();
            }
            catch (ParseError)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr? initializer = null;
            if (Match(TokenType.EQUAL))
            {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.PRINT)) return PrintStatement();

            return ExpressionStatement();
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new Stmt.Expression(expr);
        }

        private Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Stmt.Print(value);
        }

        private Expr Expression()
        {
            return Comma();
        }

        private Expr Comma()
        {
            Expr expr = Conditional();
            while (Match(TokenType.COMMA))
            {
                Token op = Previous();
                Expr right = Conditional();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Conditional()
        {
            Expr expr = Equality();
            if (Match(TokenType.QUESTION))
            {
                Expr thenBranch = Expression();
                Consume(TokenType.COLON, "Expect ':' after then branch of conditional expression.");
                Expr elseBranch = Conditional();
                expr = new Expr.Conditional(expr, thenBranch, elseBranch);
            }
            return expr;
        }


        private Expr Equality()
        {
            Expr expr = Comparison();

            while(Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;

            return Previous();
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;

            return Peek().Type == type;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while(Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token op = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            if (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Error(Previous(), "Missing left-hand operand.");
                Comparison();
                return null!;
            }
            if (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Error(Previous(), "Missing left-hand operand.");
                Term();
                return null!;
            }
            if (Match(TokenType.PLUS))
            {
                Error(Previous(), "Missing left-hand operand.");
                Factor();
                return null!;
            }
            if (Match(TokenType.SLASH, TokenType.STAR))
            {
                Error(Previous(), "Missing left-hand operand.");
                Unary();
                return null!;
            }


            throw Error(Peek(), "Expect expression.");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

            throw Error(Peek(), message);
        }

        private ParseError Error(Token token, string message)
        {
            Lox.Error(token, message);
            return new ParseError();
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;

                switch (Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }
            }

            Advance();
        }
    }    
}