// Generado por GenerateAst. No editar a mano: modificar GenerateAst/Program.cs y volver a ejecutarlo.
using InterpreterProject.ScannerModule;

namespace InterpreterProject;

public abstract class Stmt
{
    public abstract T Accept<T>(IStmtVisitor<T> visitor);

    public class Expression : Stmt
    {
        public Expression(Expr expr)
        {
            Expr = expr;
        }

        public Expr Expr { get; }

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
}

public interface IStmtVisitor<T>
{
    T Visit(Stmt.Expression expression);
    T Visit(Stmt.Print print);
}
