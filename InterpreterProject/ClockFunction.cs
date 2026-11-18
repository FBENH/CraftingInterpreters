using InterpreterProject.InterpreterModule;

namespace InterpreterProject
{
    public class ClockFunction : LoxCallable
    {
        public int Arity() => 0;


        public object Call(Interpreter interpreter, List<object> arguments) => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;

        public override string ToString() => "<native fn>";


    }
}
