using System.Text;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: generate_ast <output directory>");
    return;
}

string outputDir = args.First();

DefineAst(outputDir, "Expr", new List<string>
{
    "Binary   : Expr left, Token opertor, Expr right",
    "Grouping : Expr expression",
    "Literal  : Object value",
    "Unary    : Token opertor, Expr right"
}); 

static void DefineAst(string outputDir, string baseName, List<string> types)
{
    string path = outputDir + "/" + baseName + ".cs";
    using StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8);

    writer.WriteLine("namespace Craftinginterpreters.lox;");
    writer.WriteLine();
    writer.WriteLine($"abstract class {baseName} {{");
    writer.WriteLine();
    // The AST classes
    foreach (string type in types)
    {
        string[] parts = type.Split(":", 2);
        string className = parts[0].Trim();
        string fields = type.Split(":")[1].Trim();
        DefineType(writer, baseName, className, fields);
    }


    writer.WriteLine("}");
    writer.Close();

    
}

static void DefineType(StreamWriter writer, string baseName, string className, string fields)
{
    writer.WriteLine("    public class " + className + " : " + baseName);
    writer.WriteLine("    {");

    // Constructor
    writer.WriteLine($"        public {className}({fields})");
    writer.WriteLine("        {");

    string[] splittedFields = fields.Split(", ");
    foreach (string f in splittedFields)
    {
        string[] parts = f.Split(' ', 2);
        string type = parts[0];
        string name = parts[1];

        writer.WriteLine($"            {Capitalize(name)} = {name};");
    }

    writer.WriteLine("        }");
    writer.WriteLine();

    // Properties
    foreach (string f in splittedFields)
    {
        string[] parts = f.Split(' ', 2);
        string type = parts[0];
        string name = parts[1];

        writer.WriteLine($"        public {type} {Capitalize(name)} {{get;}}");
    }


    writer.WriteLine("    }");
    writer.WriteLine();
}

static string Capitalize(string name)
{
    return char.ToUpper(name[0]) + name[1..];
}