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
            return declaration.Parameters.Count;
        }

        public object? Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(interpreter.globals);

            for (int i = 0; i < declaration.Parameters.Count; i++)
            {
                environment.Define(declaration.Parameters[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.Body, environment);

            }
            catch (Return returnValue)
            {
                return returnValue.value;                
            }
            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.Name.Lexeme}>";
        }
    }
}
