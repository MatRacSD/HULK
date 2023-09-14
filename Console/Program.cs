using Compiler;


class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            
            Console.WriteLine("holero");
            string input = Console.ReadLine();
            var list = Compiler.Lexer.TokensInit(input);
            foreach (var token in list)
            {
                Console.WriteLine(token.ToString());
            }
            Console.ReadKey();
        }
    }
}