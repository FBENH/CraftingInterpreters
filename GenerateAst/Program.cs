using System.Text;

// GenerateAst: genera las clases del árbol sintáctico (AST) de Lox en C#.
// Equivalente a tool/GenerateAst.java de "Crafting Interpreters",
// adaptado a las convenciones de InterpreterProject:
//   - namespace InterpreterProject; Token vive en InterpreterProject.ScannerModule
//   - propiedades PascalCase de solo lectura
//   - patrón Visitor: Accept<T> genérico + interfaz I{Base}Visitor<T> con sobrecargas Visit(...)
//
// Uso: dotnet run --project GenerateAst -- <directorio de salida>
// (el perfil de launchSettings.json apunta a InterpreterProject\InterpreterProject)

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: generate_ast <output directory>");
    Environment.Exit(64);
}

string outputDir = args[0];

// Cada línea: "NombreDeClase : Tipo campo, Tipo campo, ..."
DefineAst(outputDir, "Expr",
[
    "Binary      : Expr left, Token opertor, Expr right",
    "Conditional : Expr condition, Expr thenBranch, Expr elseBranch",
    "Grouping    : Expr expression",
    "Literal     : object? value",
    "Unary       : Token opertor, Expr right",
]);

DefineAst(outputDir, "Stmt",
[
    "Expression : Expr expr",
    "Print      : Expr expr",
]);

static void DefineAst(string outputDir, string baseName, List<string> types)
{
    string visitorName = $"I{baseName}Visitor";

    // "Nombre : campos" -> (Nombre, [(Tipo, campo), ...])
    var parsed = new List<(string ClassName, string FieldList, List<(string Type, string Name)> Fields)>();
    foreach (string line in types)
    {
        string[] parts = line.Split(':', 2);
        string className = parts[0].Trim();
        string fieldList = parts[1].Trim();

        var fields = new List<(string Type, string Name)>();
        foreach (string field in fieldList.Split(", "))
        {
            string[] fieldParts = field.Trim().Split(' ', 2);
            string type = fieldParts[0];
            string name = fieldParts[1].Trim();

            // C# no permite un miembro con el mismo nombre que su clase (CS0542).
            if (Capitalize(name) == className)
            {
                Console.Error.WriteLine(
                    $"Error en {baseName}.{className}: el campo '{name}' se convertiría en la propiedad " +
                    $"'{Capitalize(name)}', igual al nombre de la clase (CS0542). Renombra el campo.");
                Environment.Exit(65);
            }

            fields.Add((type, name));
        }

        parsed.Add((className, fieldList, fields));
    }

    var sb = new StringBuilder();
    sb.AppendLine("// Generado por GenerateAst. No editar a mano: modificar GenerateAst/Program.cs y volver a ejecutarlo.");
    sb.AppendLine("using InterpreterProject.ScannerModule;");
    sb.AppendLine();
    sb.AppendLine("namespace InterpreterProject;");
    sb.AppendLine();
    sb.AppendLine($"public abstract class {baseName}");
    sb.AppendLine("{");
    sb.AppendLine($"    public abstract T Accept<T>({visitorName}<T> visitor);");

    foreach (var (className, fieldList, fields) in parsed)
    {
        sb.AppendLine();
        DefineType(sb, baseName, visitorName, className, fieldList, fields);
    }

    sb.AppendLine("}");
    sb.AppendLine();
    DefineVisitor(sb, baseName, visitorName, parsed.Select(p => p.ClassName));

    // WriteAllText sobrescribe el archivo: ejecutar el generador varias veces da siempre el mismo resultado.
    string path = Path.Combine(outputDir, baseName + ".cs");
    File.WriteAllText(path, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    Console.WriteLine($"Generado: {Path.GetFullPath(path)}");
}

static void DefineType(StringBuilder sb, string baseName, string visitorName,
                       string className, string fieldList, List<(string Type, string Name)> fields)
{
    sb.AppendLine($"    public class {className} : {baseName}");
    sb.AppendLine("    {");

    // Constructor: guarda cada parámetro en su propiedad.
    sb.AppendLine($"        public {className}({fieldList})");
    sb.AppendLine("        {");
    foreach (var (_, name) in fields)
        sb.AppendLine($"            {Capitalize(name)} = {name};");
    sb.AppendLine("        }");
    sb.AppendLine();

    // Propiedades de solo lectura.
    foreach (var (type, name) in fields)
        sb.AppendLine($"        public {type} {Capitalize(name)} {{ get; }}");
    sb.AppendLine();

    // Visitor: el compilador elige la sobrecarga Visit(...) según el tipo estático de 'this'.
    sb.AppendLine($"        public override T Accept<T>({visitorName}<T> visitor) => visitor.Visit(this);");
    sb.AppendLine("    }");
}

static void DefineVisitor(StringBuilder sb, string baseName, string visitorName, IEnumerable<string> classNames)
{
    sb.AppendLine($"public interface {visitorName}<T>");
    sb.AppendLine("{");
    foreach (string className in classNames)
        sb.AppendLine($"    T Visit({baseName}.{className} {SafeIdentifier(Decapitalize(className))});");
    sb.AppendLine("}");
}

static string Capitalize(string name) => char.ToUpperInvariant(name[0]) + name[1..];

static string Decapitalize(string name) => char.ToLowerInvariant(name[0]) + name[1..];

// Antepone @ si el nombre es una palabra reservada de C# (p. ej. "operator", "object", "string").
static string SafeIdentifier(string name) => CSharpKeywords.Contains(name) ? "@" + name : name;

static partial class Program
{
    static readonly HashSet<string> CSharpKeywords =
    [
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
        "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
        "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
        "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
        "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
        "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
        "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this",
        "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
        "using", "virtual", "void", "volatile", "while",
    ];
}
