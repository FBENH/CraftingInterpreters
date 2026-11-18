using InterpreterProject.InterpreterModule;

namespace InterpreterProject
{
    public interface LoxCallable
    {
        object? Call(Interpreter interpreter, List<object> arguments);
        int Arity();
    }
}
