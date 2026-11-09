using InterpreterProject.InterpreterModule;
using InterpreterProject.ScannerModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterProject
{
    public class Environment
    {
        private readonly Dictionary<string, object> values = new();

        public void Define(string name, object value)
        {
            values.Add(name, value);
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            throw new RunTimeError(name, $"Undefined variable {name.Lexeme}.");
        }
    }
}
