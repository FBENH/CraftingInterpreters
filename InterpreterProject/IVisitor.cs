using static InterpreterProject.Expr;

namespace InterpreterProject
{
    public interface IVisitor<T>
    {
        public T Visit(Unary expr);
        public T Visit(Binary expr);
        public T Visit(Grouping expr);
        public T Visit(Literal expr);
        public T Visit(Conditional expr);
    }
}
