// Generado por GenerateAst. No editar a mano: modificar GenerateAst/Program.cs y volver a ejecutarlo.
using InterpreterProject.ScannerModule;

namespace InterpreterProject;

public abstract class Stmt
{
    public abstract T Accept<T>(IStmtVisitor<T> visitor);

    public class Block : Stmt
    {
        public Block(List<Stmt> statements)
        {
            Statements = statements;
        }

        public List<Stmt> Statements { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Expression : Stmt
    {
        public Expression(Expr expr)
        {
            Expr = expr;
        }

        public Expr Expr { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }

    public class If : Stmt
    {
        public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public Expr Condition { get; }
        public Stmt ThenBranch { get; }
        public Stmt ElseBranch { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Print : Stmt
    {
        public Print(Expr expr)
        {
            Expr = expr;
        }

        public Expr Expr { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }

    public class Var : Stmt
    {
        public Var(Token name, Expr? initializer)
        {
            Name = name;
            Initializer = initializer;
        }

        public Token Name { get; }
        public Expr? Initializer { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }

    public class While : Stmt
    {
        public While(Expr condition, Stmt body)
        {
            Condition = condition;
            Body = body;
        }

        public Expr Condition { get; }
        public Stmt Body { get; }

        public override T Accept<T>(IStmtVisitor<T> visitor) => visitor.Visit(this);
    }
}

public interface IStmtVisitor<T>
{
    T Visit(Stmt.Block block);
    T Visit(Stmt.Expression expression);
    T Visit(Stmt.If @if);
    T Visit(Stmt.Print print);
    T Visit(Stmt.Var var);
    T Visit(Stmt.While @while);
}
