using System.Text;

namespace InterpreterProject.ParserModule
{
    public class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string Visit(Expr.Binary expr)
        {
            return Parenthesize(expr.Opertor.Lexeme, [expr.Left, expr.Right]);
        }

        public string Visit(Expr.Grouping expr)
        {
            return Parenthesize("group", [expr.Expression]);
        }

        public string Visit(Expr.Literal expr)
        {
            if (expr.Value == null) return "nil";

            return expr.Value.ToString()!;
        }

        public string Visit(Expr.Unary expr)
        {
            return Parenthesize(expr.Opertor.Lexeme, [expr.Right]);
        }    

        private string Parenthesize(string name, Expr[] exprs)
        {
            StringBuilder builder = new();
            builder.Append("(").Append(name);

            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
