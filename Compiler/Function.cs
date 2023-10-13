namespace Compiler
{
    public class Function
    {
        public Function(Token vrs, List<Token> bdy, Token nme)
        {
            if(vrs.Content != "cpar")
            {
               throw new ArgumentException();
            }
            List<string> names = new List<string>();
            int a = 0;
            
            for (int i = 0; i < vrs.exp.Count; i++)
            {
                if(vrs.exp[i].Type == "iden" && a == 0)
                {
                    names.Add(vrs.exp[i].Content);
                    a = 1;
                }
                else if(vrs.exp[i].Content == ",")
                {
                    a = 0;
                }
                else throw new ArgumentException();
            }

            vars_0 = names.ToArray();
            body_0 = bdy;
            name = nme.Content;

            ActiveFunctions.Add(this);
        }
        public  Token Call(Token token_a)
        {
            Token token = token_a.Clone();
            Dictionary<string,Token> var_assignation = new Dictionary<string, Token>();
            List<Token> body = body_0.Clone();
            string[] vars = (string[])vars_0.Clone();

            List<Token> aux_list = new List<Token>();
            if(token.Content != "cpar")
            {
                throw new ArgumentException();
            }
            int aux = 0;
            for (int i = 0; i < token.exp.Count; i++)

            {
                if(token.exp[i].Content == ",")
                {
                    var_assignation.Add(vars[aux],Parser.Parse(aux_list,1).GetValue());
                    aux_list.Clear();
                    aux += 1;
                }
                else if(i == token.exp.Count - 1)
                {
                    aux_list.Add(token.exp[i]);
                     var_assignation.Add(vars[aux],Parser.Parse(aux_list,1).GetValue());
                    aux_list.Clear();
                    aux += 1;
                }

                else
                {
                    aux_list.Add(token.exp[i]);
                }                
            }


            for (int i = 0; i < body.Count; i++)
            {
                if(body[i].Type == "iden")
                {
                    if(var_assignation.ContainsKey(body[i].Content))
                    {
                        body[i] = var_assignation[body[i].Content];
                    }
                   
                }

                 else if(body[i].Type == "mix" || body[i].Type == "func")
                 {
                    body[i].Remplace(var_assignation);
                 }
            }

            return Parser.Parse(body,1).GetValue();

            throw new ArgumentException();
        }
        
        public static bool GetFunction(List<Token> tokens)
        {
            if(tokens[0].Type == "iden" && tokens[0].Content == "function")
            {
                if(tokens[1].Type == "func")
                {
                     string fname = tokens[1].Content;
                     Token f = tokens[1];
                     List<Token> tks = new List<Token>();

                     if(tokens[2].Content == "=>")
                     {
                         for (int i = 3; i < tokens.Count; i++)
                         {
                            tks.Add(tokens[i]);
                         }

                         Function function = new Function(f.exp[0],tks,new Token(){Type = "iden", Content = fname});
                         return true;
                     }
                     else throw new ArgumentException("=> expected");
                }
                else throw new ArgumentException("invalid token at function declaration");
            }

            return false;
        }
        public string name;
        private string[] vars_0;
        private List<Token> body_0;
        
        
        public static List<Function> ActiveFunctions = new List<Function>();
    }
}