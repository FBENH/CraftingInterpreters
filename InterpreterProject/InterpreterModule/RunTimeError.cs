using InterpreterProject.ScannerModule;

namespace InterpreterProject.InterpreterModule
{
    public class  RunTimeError : Exception
    {
        public readonly Token Token;

        public RunTimeError(Token token, string? message) : base(message)
        {
            this.Token = token;
        }
    }
}