using InterpreterProject.ScannerModule;

namespace InterpreterProject.ParserModule;

public abstract class Expr {

    public abstract T Accept<T>(IVisitor<T> visitor);
    public abstract T Accept<T>(IRpnVisitor<T> visitor);

    public class Binary : Expr
    {
        public Binary(Expr left, Token opertor, Expr right)
        {
            Left = left;
            Opertor = opertor;
            Right = right;
        }

        public Expr Left {get;}
        public Token Opertor {get;}
        public Expr Right {get;}

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override T Accept<T>(IRpnVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override T Accept<T>(IRpnVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override T Accept<T>(IRpnVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Unary : Expr
    {
        public Unary(Token opertor, Expr right)
        {
            Opertor = opertor;
            Right = right;
        }

        public Token Opertor {get;}
        public Expr Right {get;}

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override T Accept<T>(IRpnVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

}
