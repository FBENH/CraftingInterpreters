namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
namespace Craftinginterpreters.lox;

abstract class Expr {

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
    }

    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public Expr Expression {get;}
    }

    public class Literal : Expr
    {
        public Literal(Object value)
        {
            Value = value;
        }

        public Object Value {get;}
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
    }

}
