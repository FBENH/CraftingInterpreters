// Generado por GenerateAst. No editar a mano: modificar GenerateAst/Program.cs y volver a ejecutarlo.
using InterpreterProject.ScannerModule;

namespace InterpreterProject;

public abstract class Expr
{
    public abstract T Accept<T>(IExprVisitor<T> visitor);

    public class Assign : Expr
    {
        public Assign(Token name, Expr value)
        {
            Name = name;
            Value = value;
        }

        public Token Name { get; }
        public Expr Value { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }

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

    public class Call : Expr
    {
        public Call(Expr callee, Token paren, List<Expr> arguments)
        {
            Callee = callee;
            Paren = paren;
            Arguments = arguments;
        }

        public Expr Callee { get; }
        public Token Paren { get; }
        public List<Expr> Arguments { get; }

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

    public class Logical : Expr
    {
        public Logical(Expr left, Token opertor, Expr right)
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

    public class Variable : Expr
    {
        public Variable(Token name)
        {
            Name = name;
        }

        public Token Name { get; }

        public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
    }
}

public interface IExprVisitor<T>
{
    T Visit(Expr.Assign assign);
    T Visit(Expr.Binary binary);
    T Visit(Expr.Call call);
    T Visit(Expr.Grouping grouping);
    T Visit(Expr.Literal literal);
    T Visit(Expr.Logical logical);
    T Visit(Expr.Unary unary);
    T Visit(Expr.Variable variable);
}
