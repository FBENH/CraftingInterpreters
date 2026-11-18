using InterpreterProject.InterpreterModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterProject
{
    public class LoxFunction : LoxCallable
    {
        private readonly Stmt.Function declaration;

        public LoxFunction(Stmt.Function declaration)
        {
            this.declaration = declaration;
        }

        public int Arity()
        {
            return declaration.Params.Count;
        }

        public object? Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(interpreter.globals);

            for (int i = 0; i < declaration.Params.Count; i++)
            {
                environment.Define(declaration.Params[i].Lexeme, arguments[i]);
            }

            interpreter.ExecuteBlock(declaration.Body, environment);
            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.Name.Lexeme}>";
        }
    }
}
