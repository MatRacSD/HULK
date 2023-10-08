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
            Node node = Parser.Parse(list);
            Console.WriteLine(node.GetValue().ToString());
            //foreach (var token in list)
            //{
             //   Console.WriteLine(token.ToString());
            //}
            //Console.ReadKey();
        }
    }
}