// Generado por GenerateAst. No editar a mano: modificar GenerateAst/Program.cs y volver a ejecutarlo.
using InterpreterProject.ScannerModule;

namespace InterpreterProject;

public abstract class Expr
{
    public abstract T Accept<T>(IExprVisitor<T> visitor);

    public class Binary : Expr
    {
        public Binary(Expr left, Token opertor, Expr right)
        {
            Left = left;
            Opertor = opertor;
            Right = right;
        }

        public Expr Left { get; }
        public Token Opertor { get; }
        public Expr Right { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Conditional : Expr
    {
        public Conditional(Expr condition, Expr thenBranch, Expr elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public Expr Condition { get; }
        public Expr ThenBranch { get; }
        public Expr ElseBranch { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Literal : Expr
    {
        public Literal(object? value)
        {
            Value = value;
        }

        public object? Value { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Unary : Expr
    {
        public Unary(Token opertor, Expr right)
        {
            Opertor = opertor;
            Right = right;
        }

        public Token Opertor { get; }
        public Expr Right { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }
}

public interface IExprVisitor<T>
{
    T Visit(Expr.Binary binary);
    T Visit(Expr.Conditional conditional);
    T Visit(Expr.Grouping grouping);
    T Visit(Expr.Literal literal);
    T Visit(Expr.Unary unary);
}
