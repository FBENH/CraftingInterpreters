using InterpreterProject;
using InterpreterProject.InterpreterModule;
using InterpreterProject.ParserModule;
using InterpreterProject.ScannerModule;
using System.Text;

public class Lox
{
    static bool hadError = false;
    static bool hadRuntimeError = false;
    private static readonly Interpreter interpreter = new Interpreter();

    private static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: cslox [script]");
            return;
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }      
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            hadError = false;
        }
    }

    private static void RunFile(string path)
    {
        if (hadError) System.Environment.Exit(65);
        if (hadRuntimeError) System.Environment.Exit(70);

        byte[] bytes = File.ReadAllBytes(path);
        Run(Encoding.Default.GetString(bytes));
    }

    private static void Run(string sourceCode)
    {
        Scanner scanner = new Scanner(sourceCode);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new Parser(tokens);
        List<Stmt>? statements = parser.Parse();

        // Stop if there was a syntax error
        if (hadError) return;

        interpreter.Interpret(statements);
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        string output = $"[line {line}] Error {where} : {message}";
        Console.Error.WriteLine(output);
        hadError = true;
    }

    internal static void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, " at '" + token.Lexeme + "'", message);
        }
    }

    public static void RunTimeError(RunTimeError error)
    {
        Console.WriteLine(error.Message + $"\n[line {error.Token.Line}]");
        hadRuntimeError = true;
    }
}