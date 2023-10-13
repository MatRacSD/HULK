using Compiler;


class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            
            //List<Token> tokens = new List<Token>(){new Token(){Type="number",Content="4"},new Token(){Type = "Operator",Content = "+"},new Token(){Type="iden",Content ="x"}};
            //Function fa = new Function(new Token(){Type = "mix",Content = "cpar",exp = new List<Token>(){new Token(){Type="iden",Content="x"}}},tokens,new Token(){Type = "iden",Content="test"} );
            var asd = Function.ActiveFunctions;
            
          var asd1 = Function.ActiveFunctions;

            
            string input = Console.ReadLine();
            var list = Lexer.TokensInit(input);
            var l2 = Lexer.GetToken2(list);
            if(Function.GetFunction(l2)) continue;

            var asd2 = Function.ActiveFunctions;

            Node node = Parser.Parse(l2,1);
            Console.WriteLine(node.GetValue().ToString());
            continue;

            foreach (var token in l2)
            {
                Console.WriteLine(token.ToString());
            }
            //Console.ReadKey();
        }
    }
}