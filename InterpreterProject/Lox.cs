using InterpreterProject.ParserModule;
using InterpreterProject.ScannerModule;
using System.Text;
using static InterpreterProject.ParserModule.Expr;

internal class Lox
{
    static bool hadError = false;

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
        if (hadError) return;
        byte[] bytes = File.ReadAllBytes(path);
        Run(Encoding.Default.GetString(bytes));
    }

    private static void Run(string sourceCode)
    {
        Scanner scanner = new Scanner(sourceCode);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new Parser(tokens);
        Expr? expression = parser.Parse();

        // Stop if there was a syntax error
        if (hadError) return;

        // for now just print the tokens
        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }

        foreach(Token token in tokens)
        {
            Console.WriteLine(new AstPrinter().Print(expression));

        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        string output = $"[line {line}] Error {where} : {message}";
        Console.Error.WriteLine(output);
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
}