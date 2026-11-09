using InterpreterProject.ScannerModule;

namespace InterpreterProject.InterpreterModule
{
    public class Interpreter : IExprVisitor<object?>, IStmtVisitor<object?>
    {
        private Environment environment = new();

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt stmt in statements)
                {
                    Execute(stmt);
                }
            }
            catch (RunTimeError error)
            {
                Lox.RunTimeError(error);
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private string Stringify(object value)
        {
            if (value == null) return "nil";

            if (value is double)
            {
                string text = value.ToString();

                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }

                return text;
            }

            return value.ToString()!;
        }

        public object? Visit(Expr.Unary expr)
        {
            Object right = Evaluate(expr.Right);
            switch (expr.Opertor.Type) 
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Opertor, right);
                    return -(double)right;
            }

            // Unreachable.
            return null;
         }

        private void CheckNumberOperand(Token opertor, object right)
        {
            if (right is double) return;
            throw new RunTimeError(opertor, "Operand must be a number.");
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;

            if (obj is bool) return (bool)obj;

            return true;
        }        

        public object? Visit(Expr.Binary expr)
        {
            Object left = Evaluate(expr.Left);
            Object right = Evaluate(expr.Right);

            switch (expr.Opertor.Type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    if (left is string leftText && right is double rightNumber)
                    {
                        return $"{leftText + rightNumber}";
                    }
                    if (left is double leftNumber && right is string rightText)
                    {
                        return $"{leftNumber + rightText}";
                    }
                    throw new RunTimeError(expr.Opertor, "Operands must be two numbers or two strings");             
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Opertor, left, right);
                    CheckZeroDivider(expr.Opertor, (double)right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left * (double)right;
            }

            // Unreachable.
            return null;
        }

        private void CheckZeroDivider(Token token,double right)
        {
            if (right == 0)
            {
                throw new RunTimeError(token ,"Divider cannot be 0");
            }
        }

        private void CheckNumberOperands(Token opertor, object left, object right)
        {
            if (left is double && right is double) return;

            throw new RunTimeError(opertor, "Operands must be numbers.");
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        public object? Visit(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object? Visit(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object? Visit(Expr.Conditional expr)
        {
            throw new NotImplementedException();
        }


        private object Evaluate(Expr expr)
        {            
            return expr.Accept(this);            
        }

        public object? Visit(Stmt.Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public object? Visit(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object? Visit(Expr.Variable expr)
        {
            return environment.Get(expr.Name);
        }

        public object? Visit(Stmt.Var stmt)
        {
            object? value = null;

            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environment.Define(stmt.Name.Lexeme, value);
            return null;
        }
    }
}