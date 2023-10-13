using System.Security.Principal;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Compiler
{
    public static class Parser
    {
        public static Node Parse(List<Token> tokens, int startlevel)
        {
            levels.Add(LV0);
            levels.Add(LV0_1);
            levels.Add(LV0_2);
            levels.Add(LV1);
            levels.Add(LV2);
            levels.Add(LV2_1);
            
            levels.Add(LV3);
            levels.Add(LV4);
           
           return ParseLevel(tokens,levels[startlevel - 1],startlevel);

        }

        public static Node ParseLevel(List<Token> tokens, List<string> lvls, int clevel)
        {
            List<Token> aux_token_list = new List<Token>();
            Node? cNode = null;
           

            
              
            if(clevel == 2)
            {
                if(tokens[0].Content == "!")
                {
                    Node node = new  Node{token = tokens[0], childs = new Node[1]};
                      tokens.RemoveAt(0);
                      node.childs[0] = Parse(tokens, 3);
                      return node;
                }
            }

            else if(clevel == 8)
            {
                if(tokens.Count > 1)
                {
                    throw new ArgumentException();
                }

                else if(tokens[0].Type == "number" || tokens[0].Type == "bool" || tokens[0].Type == "strings")
                {
                   return new Node(){token = tokens[0], IsTerminal = true};
                }

                else if(tokens[0].Content == "cpar")
                {
                    Node node= new Node(){token = tokens[0], childs = new Node[1]};
                    node.childs[0] = Parse(tokens[0].exp,1);
                    return node;
                    
                }

                else if(tokens[0].Content == "let-in")
                {
                    Node node = new Node(){token = tokens[0]};
                    return node;
                }

                else if (tokens[0].Content == "if-else")
                {
                    Node node = new Node(){token = tokens[0]};
                    return node;
                }
                else if (tokens[0].Type == "func")
                {
                    Node node = new Node(){token = tokens[0]};
                    return node;
                }

                else throw new ArgumentException();
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if(lvls.Contains(tokens[i].Content))
                {
                    if(tokens[i].Content == "-" && aux_token_list.Count == 0)
                    {
                        aux_token_list.Add(tokens[i]);
                        continue;
                    }
                  else  if(cNode == null)
                   {
                    Node node = new Node(){token = tokens[i], childs = new Node[2]};
                   node.childs[0] = Parse(aux_token_list, clevel + 1);
                   aux_token_list = new List<Token>();
                   cNode = node;
                   }
                   else
                   {
                     cNode.childs[1] = Parse(aux_token_list, clevel + 1);
                     aux_token_list = new List<Token>();
                     Node aux_node = cNode;
                     cNode = new Node(){token = tokens[i], childs = new Node[2]};
                     cNode.childs[0] = aux_node;
                   }
                }
                else aux_token_list.Add(tokens[i]);

                
            }
            if(cNode != null)
            {
                cNode.childs[1] = Parse(aux_token_list,clevel + 1);
            return cNode;
            }

            else return Parse(tokens,clevel + 1);
        }

        

            
            //Se realiza el proceso para el nivel 1_1
               
        
        

        /// /
        /// /////////////////////
        ///         /// ///////////////////////////////////////////////////////
        /// ////////////////////////
        /// ///////////////////////////////////
        static List<List<string>> levels = new List<List<string>>();
        //static List<string> LV1 = new List<string> {"+"};
        
        static List<string> LV0 = new List<string> {"&", "|"};

        static List<string> LV0_1 = new List<string> {"!"};
        static List<string> LV0_2 = new List<string> {"<=",">=","==","!=" , "<" , ">", };
        static List<string> LV1 = new List<string> {"+","-","@"};
        
        static List<string> LV2 = new List<string> {"*","/","%"};

        static List<string> LV2_1 = new List<string> {"^"};

        
        static List<string> LV3 =  new List<string> { "-"};
        static List<string> LV4 = new List<string> {"let-in","print","number","cpar","bool","if-else","strings","func"};

        //static List<string> LV3 = new List<string>{"number"};

        


        /// <summary>
    /// Retorna verdadero si el siguiente token es del mismo nivel.
    /// </summary>
        public static bool IsTheSameLevel(this List<Token> tokens,List<string> LV,int index)
        {
            int p = 0;
            int counter = 0;
            for (int i = index + 1; i < tokens.Count; i++)
            {
                if(tokens[i].Content == "(")
                {
                    p += 1;
                    continue;
                }
                else if(tokens[i].Content == ")")
                {
                    p -= 1;
                    continue;
                }

                if(p == 0 )
                {
                    counter += 1;
                }

                if(counter == 2 && counter < tokens.Count)
                {
                    if(LV.Contains(tokens[i].Content))
                    {
                        return true;
                    }

                    break;
                }


                
            }
            return false;    
             
            /////////////////////////
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
                {
                    p += 1;
                continue;
                }
                else if (tokens[k].Content == ")")
                {
                    p -= 1;
                    continue;
                }
               if(k > index && p == 0 && tokens[k].Type == "Operator")
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
            else if(token.Content == "+" || token.Content == "-" || token.Content == "*" || token.Content == "/" || token.Content == "^" || token.Content == "%")
            {
                return Operations.BinaryOperation(childs[0].GetValue(),childs[1].GetValue(),token.Content);

            }
            else if(token.Content == "!")
            {
              return Operations.UnaryNegation(childs[0].GetValue());
            }

            else if(token.Content == "<=" || token.Content == ">=" || token.Content == "==" || token.Content == "!=" || token.Content == "<" || token.Content == ">"|| token.Content == "&" || token.Content == "|" )
            {
                return Operations.LogicOperation(childs[0].GetValue(),childs[1].GetValue(),token.Content);
            }

            else if(token.Content == "@")
            {
                return Operations.StringSum(childs[0].GetValue(),childs[1].GetValue());
            }
            else if(token.Type == "mix" )
            {
                if(token.Content == "cpar")
                {
                    return childs[0].GetValue();
                }
                else if(token.Content == "let-in")
                {
                    return Operations.LetIn(token);
                }

                else if (token.Content == "if-else")
                {
                    return Operations.IfElse(token);
                }
            }
            else if(token.Type == "func")
            {
                 foreach (Function f in Function.ActiveFunctions)
                 {
                    if(f.name == token.Content)
                    {
                        return f.Call(token.exp[0]);
                    }
                    
                 }

                  throw new ArgumentException("the function " + token.Content + " is not defined");

                 
            }
            throw new ArgumentException("ERROR AT GETVALUE METHOD ON NODE");
        }
        public Node? Parent { get; set;}

        public  bool IsTerminal = false;
       
        
        /// <summary>
        /// Representa una epresion
        /// </summary>
        public static List<Token> expp ;
        public Token token {get; set;}

        public Node[]? childs {get; set;}
        
    }

    
    

}