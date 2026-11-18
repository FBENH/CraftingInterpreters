using InterpreterProject.ScannerModule;
using System.Security.Cryptography.X509Certificates;

namespace InterpreterProject.InterpreterModule
{
    public class Interpreter : IExprVisitor<object?>, IStmtVisitor<object?>
    {
        public readonly Environment globals;
        public Environment environment;

        public Interpreter()
        {
            globals = new Environment();
            environment = globals;

            globals.Define("clock", new ClockFunction());            
        }

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
                    throw new RunTimeError(expr.Opertor, "Operands must be two numbers or two strings");             
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Opertor, left, right);
                    return (double)left * (double)right;
            }

            // Unreachable.
            return null;
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

        public object? Visit(Expr.Assign expr)
        {
            object? value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
        }

        public object? Visit(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        public object ? Visit(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else if (stmt.ElseBranch != null)
            {
                Execute(stmt.ElseBranch);
            }

            return null;
        }

        public object? Visit(Expr.Logical expr)
        {
            object? left = Evaluate(expr.Left);

            if (expr.Opertor.Type == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.Right);
        }

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        public object? Visit(Stmt.While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }

            return null;
        }

        public object? Visit(Expr.Call expr)
        {
            object? callee = Evaluate(expr.Callee);

            List<object> arguments = new();

            foreach (Expr argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (!(callee is LoxCallable))
            {
                throw new RunTimeError(expr.Paren, "Can only call functions and classes.");
            }

            LoxCallable function = (LoxCallable)callee;
            if (arguments.Count != function.Arity())
            {
                throw new RunTimeError(expr.Paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
            }

            return function.Call(this, arguments);
        }

        public object? Visit(Stmt.Function stmt)
        {
            var function = new LoxFunction(stmt);
            environment.Define(stmt.Name.Lexeme, function);

            return null;
        }

        public object? Visit(Stmt.Return stmt)
        {
            object? value = null;
            if (stmt.Value != null) value = Evaluate(stmt.Value);

            throw new Return(value);
        }
    }
}