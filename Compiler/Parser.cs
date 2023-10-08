using Microsoft.VisualBasic;

namespace Compiler
{
    public static class Parser
    {
        public static Node Parse(List<Token> tokens)
        {
            //se recorre la lista de tokens en busca de tokens de primer nivel
            Node node = null;
             int p = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                //se verifica que no se este tomando terminos que esten dentro de parentesis.
               
                if(tokens[i].Content == "(")
                p += 1;
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                }
                
                if(LV1.Contains(tokens[i].Content) && p == 0 )
                {
                    List<Token> l1 = new List<Token>();
                    for (int j = 0; j < i; j++)
                    {
                        l1.Add(tokens[j]);
                    }
                    List<Token> l2 = new List<Token>();
                    for (int j = i + 1; j < tokens.Count; j++)
                    {
                        l2.Add(tokens[j]);
                    }

                    if(tokens.IsTheSameLevel(LV1, i))
                    {
                          l1 = new List<Token>();
                         for (int j = 0; j < i + 2; j++)
                       {
                        l1.Add(tokens[j]);
                        }
                          l2 = new List<Token>();
                         for (int j = i + 3; j < tokens.Count; j++)
                         {
                        l2.Add(tokens[j]);
                         }

                         node = new Node(){token = tokens[i + 2], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                    }
                    //si se encuentra tokens de este nivel, se crea su nodo correspondiente y se parsean sus hijos
                      node = new Node(){token = tokens[i], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                }

                
            }
            //Se realiza el proceso para el nivel 1_1
            
            p=0;
            for (int i = 0; i < tokens.Count; i++)
            {
                //se verifica que no se este tomando terminos que esten dentro de parentesis.
                //int p = 0;
                if(tokens[i].Content == "(")
                p += 1;
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                }
                
                if(LV1_1.Contains(tokens[i].Content) && p == 0 )
                {
                    List<Token> l1 = new List<Token>();
                    for (int j = 0; j < i; j++)
                    {
                        l1.Add(tokens[j]);
                    }
                    List<Token> l2 = new List<Token>();
                    for (int j = i + 1; j < tokens.Count; j++)
                    {
                        l2.Add(tokens[j]);
                    }

                    if(tokens.IsTheSameLevel(LV1_1, i))
                    {

                        int k = tokens.GetMaxIndex(LV1_1,i); // se cambio i+2 => i

                        if(k == -1)
                        {
                            k = i + 2;
                        }
                        
                          l1 = new List<Token>();
                         for (int j = 0; j < k; j++)
                       {
                        l1.Add(tokens[j]);
                        }
                          l2 = new List<Token>();
                         for (int j = k + 1; j < tokens.Count; j++)
                         {
                             l2.Add(tokens[j]);
                         }

                         node = new Node(){token = tokens[k], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                    }
                    //si se encuentra tokens de este nivel, se crea su nodo correspondiente y se parsean sus hijos
                      node = new Node(){token = tokens[i], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                }

                
            }

            //en caso de agotar los tokens de primer nivel, se procede a buscar con los de segundo nivel
             p=0;
             for (int i = 0; i < tokens.Count; i++)
            {
                
                //se verifica que no se este tomando terminos que esten dentro de parentesis.
                
                if(tokens[i].Content == "(")
                p += 1;
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                }

                if(LV2.Contains(tokens[i].Content) && p == 0 )
                {
                    List<Token> l1 = new List<Token>();
                    for (int j = 0; j < i; j++)
                    {
                        l1.Add(tokens[j]);
                    }
                    List<Token> l2 = new List<Token>();
                    for (int j = i + 1; j < tokens.Count; j++)
                    {
                        l2.Add(tokens[j]);
                    }

                    if(tokens.IsTheSameLevel(LV2, i))
                    {

                        int k = tokens.GetMaxIndex(LV2,i+2);

                        if(k == -1)
                        {
                            k = i + 2;
                        }
                        
                          l1 = new List<Token>();
                         for (int j = 0; j < k; j++)
                       {
                        l1.Add(tokens[j]);
                        }
                          l2 = new List<Token>();
                         for (int j = k + 1; j < tokens.Count; j++)
                         {
                             l2.Add(tokens[j]);
                         }

                         node = new Node(){token = tokens[k], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                    }
                    //si se encuentra tokens de este nivel, se crea su nodo correspondiente y se parsean sus hijos
                      node = new Node(){token = tokens[i], childs = new Node[]{Parse(l1),Parse(l2)}};
                      return node;
                }

                
            }

            //Ahora se buscan los tokens atomicos
            p=0;
            for (int i = 0; i < tokens.Count; i++)
            {
                //se verifica que no se este tomando terminos que esten dentro de parentesis.
                //int p = 0;
                if(tokens[i].Content == "(")
                p += 1;
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                }

                if(LV3.Contains(tokens[i].Type) && p == 0 )
                {
                    
                    //si se encuentra tokens de este nivel, se crea su nodo correspondiente y se parsean sus hijos
                      node = new Node(){token = tokens[i], IsTerminal = true};
                      return node;
                }

                
            }

            //Ahora se procesan los parentesis
            p = 0;
             List<Token> list = new List<Token>();
             
             //Verdadero si ya se ha abierto algun parentesis
             bool Started = false;
            for (int i = 0; i < tokens.Count; i++)
            {
               
                //int p = 0;
                if(tokens[i].Content == "(")
                {
                    p += 1;
                    if(!Started)
                    {
                        Started = true;
                        continue;
                    }
                }
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                }

                if (p > 0)
                {
                    list.Add(tokens[i]);
                }

                if(p == 0 && list.Count > 0)
                {
                    Token tokn = new Token(){Content = "()", Type = "parn"};
                     node = new Node(){token = tokn, IsTerminal = false, childs = new Node[]{Parse(list)}};
                     System.Console.WriteLine("candela");
                     return node;
                }
            }



            return node;

        }

        private static Node TryParse(this List<Token> tokens,List<string> LVS)
        {
            
        }
        

        /// /
        /// /////////////////////
        ///         /// ///////////////////////////////////////////////////////
        /// ////////////////////////
        /// ///////////////////////////////////
        static List<string> LV1 = new List<string> {"+"};
        static List<string> LV1_1 = new List<string> {"-"};
        
        static List<string> LV2 = new List<string> {"*","/"};

        static List<string> LV2_1 = new List<string> {"let","in","print"};

        static List<string> LV3 = new List<string>{"number"};

        


        /// <summary>
    /// Retorna verdadero si el siguiente token es del mismo nivel.
    /// </summary>
        public static bool IsTheSameLevel(this List<Token> tokens,List<string> LV,int index)
        {
            if(index + 2 < tokens.Count)
            {
                if(LV.Contains(tokens[index + 2].Content))
                {
                    return true;
                }
            }

            return false;
        }

/// <summary>
/// Retorna el indice del token del mismo nivel consecutivo mas lejano
/// </summary>
/// <param name="tokens"></param>
/// <param name="LV"></param>
/// <param name="index"></param>
/// <returns></returns>
        public static int GetMaxIndex(this List<Token> tokens, List<string> LV, int index)
        {
            int a = -1;
            int p = 0;
             for (int k = index ; k < tokens.Count; k += 2)
             {
                //se verifica que no se este tomando terminos que esten dentro de parentesis.
                if(tokens[k].Content == "(")
                p += 1;
                else if (tokens[k].Content == ")")
                {
                    p -= 1;
                }
               if(k >= index + 2 && p == 0)
               { 
                
                if(LV.Contains(tokens[k].Content))
                {
                    a = k;
                }
                else break;
                }
             }

             return a;
        }
    }

    public class Node
    {
        
        public Token GetValue()
        {
            if(IsTerminal)
            return token;
            else if(token.Content == "+" || token.Content == "-" || token.Content == "*" || token.Content == "/")
            {
                return Operations.BinaryOperation(childs[0].GetValue(),childs[1].GetValue(),token.Content);

            }
            else if(token.Type == "parn" )
            {
                return childs[0].GetValue();
            }
            throw new ArgumentException("ERROR AT GETVALUE METHOD ON NODE");
        }
        public Node? Parent { get; set;}

        public  bool IsTerminal;
       
        
        /// <summary>
        /// Representa una epresion
        /// </summary>
        public static List<Token> expp ;
        public Token token {get; set;}

        public Node[]? childs {get; set;}
        
    }

    
    

}