using InterpreterProject.InterpreterModule;
using InterpreterProject.ScannerModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterProject
{
    public class Environment
    {
        private readonly Environment? enclosing;
        private readonly Dictionary<string, object> values = new();

        public Environment()
        {
            enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

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

            if (enclosing != null) return enclosing.Get(name);

            throw new RunTimeError(name, $"Undefined variable {name.Lexeme}.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }

            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new RunTimeError(name, $"Undefined variable {name.Lexeme}.");
        }
    }
}
