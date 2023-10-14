
using Compiler;


class Program
{
    static void Main(string[] args)
    {
        Function.InitBasicFunctions();
        while (true)
        {
            
            //List<Token> tokens = new List<Token>(){new Token(){Type="number",Content="4"},new Token(){Type = "Operator",Content = "+"},new Token(){Type="iden",Content ="x"}};
            //Function fa = new Function(new Token(){Type = "mix",Content = "cpar",exp = new List<Token>(){new Token(){Type="iden",Content="x"}}},tokens,new Token(){Type = "iden",Content="test"} );
           

            
            string input = Console.ReadLine();
            var list = Lexer.TokensInit(input);

            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }
            var l2 = Lexer.GetToken2(list);

            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }
             
             if(l2.Count == 0) continue;
             
            
            
            if(Function.GetFunction(l2)) continue;

            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }

            

            Node node = Parser.Parse(l2,1);

            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }
            Token? token = node.GetValue();

            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }

            string? value = token.Content.ToString();
            if(Error.errors.Count > 0)
            {
                foreach (Error e in Error.errors)
                {
                    System.Console.WriteLine(e.ToString());
                }
                Error.errors.Clear();

                continue;
            }
            Console.WriteLine(value);
            continue;

            
        }

        
    }

    
}