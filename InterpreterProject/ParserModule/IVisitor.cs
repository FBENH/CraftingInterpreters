using static InterpreterProject.ParserModule.Expr;

namespace InterpreterProject.ParserModule
{
    public interface IVisitor<T>
    {
        public T Visit(Unary expr);
        public T Visit(Binary expr);
        public T Visit(Grouping expr);
        public T Visit(Literal expr);
    }
}
