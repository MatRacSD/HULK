using Microsoft.VisualBasic.CompilerServices;
namespace Compiler
{
    public static class Operations
    {
        private static double ToInt(this Token t)
        {
            if (t.Type != "number")
            {
                throw new ArgumentException("Cannot convert the token Content to int in class Operations due to an invalid token type");
            }
            return double.Parse(t.Content);
        }
        public static Token BinaryOperation(Token t1, Token t2, string op)
        {
            if (t1.Type != "number")
            {
                return new Token { Type = "error", Content = "SYNTAX ERROR: " + t1.Content + " must be a number" };
            }
            else if (t2.Type != "number")
            {
                return new Token { Type = "error", Content = "SYNTAX ERROR: " + t2.Content + " must be a number" };
            }
            if (op == "+")
            {
                return new Token { Type = "number", Content = (t1.ToInt() + t2.ToInt()).ToString() };
            }
            else if (op == "-")
            {
                return new Token { Type = "number", Content = (t1.ToInt() - t2.ToInt()).ToString() };
            }
            else if (op == "*")
            {
                return new Token { Type = "number", Content = (t1.ToInt() * t2.ToInt()).ToString() };
            }
            else if (op == "/")
            {
                return new Token { Type = "number", Content = (t1.ToInt() / t2.ToInt()).ToString() };
            }
            else if (op == "^")
            {
                return new Token { Type = "number", Content = Math.Pow(t1.ToInt(), t2.ToInt()).ToString() };
            }
            else if (op == "%")
            {
                return new Token { Type = "number", Content = (t1.ToInt() % t2.ToInt()).ToString() };
            }
            

            return null;
        }

        public static Token LogicOperation(Token t1, Token t2, string op)
        {

            if (t1.Type != t2.Type && (t1.Type == "number" || t1.Type == "bool" || t1.Type == "strings"))
            {
                throw new ArgumentException("invalid use of " + op + " operator cause tokens " + t1 + " and " + "t2" + t2 + "arent the same");

            }
            else if (op == "==")
            {
                if (t1.Type == "bool")
                {
                    throw new ArgumentException("invalid bool token in ==");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.Content == t2.Content)
                    {
                        token.Content = "true";
                    }
                    else
                     { 
                        token.Content = "false";
                     }
                    return token;
                }
            }

            else if (op == "!=")
            {
                if (t1.Type == "bool")
                {
                    throw new ArgumentException("invalid bool token in !=");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.Content == t2.Content)
                    {
                        token.Content = "false";
                    }
                    else
                     { 
                        token.Content = "true";
                     }
                    return token;
                }
            }

            else if (op == ">")
            {
                if (t1.Type == "bool" || t1.Type == "strings")
                {
                    throw new ArgumentException("invalid bool token in >");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.ToInt() > t2.ToInt())
                    {
                        token.Content = "true";
                    }
                    else
                     { 
                        token.Content = "false";
                     }
                    return token;
                }
            }

              else if (op == "<")
            {
                if (t1.Type == "bool" || t1.Type == "strings")
                {
                    throw new ArgumentException("invalid bool token in <");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.ToInt() < t2.ToInt())
                    {
                        token.Content = "true";
                    }
                    else
                     { 
                        token.Content = "false";
                     }
                    return token;
                }
            }

              else if (op == ">=")
            {
                if (t1.Type == "bool" || t1.Type == "strings")
                {
                    throw new ArgumentException("invalid bool token in >=");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.ToInt() >= t2.ToInt())
                    {
                        token.Content = "true";
                    }
                    else
                     { 
                        token.Content = "false";
                     }
                    return token;
                }
            }
              else if (op == "<=")
            {
                if (t1.Type == "bool" || t1.Type == "strings")
                {
                    throw new ArgumentException("invalid bool token in <");
                }
                else
                {
                    Token token = new Token() { Type = "bool" };
                    if (t1.ToInt() <= t2.ToInt())
                    {
                        token.Content = "true";
                    }
                    else
                     { 
                        token.Content = "false";
                     }
                    return token;
                }
            }

            else if(op == "&")
            {
                if(t1.Type != "bool")
                {
                    throw new ArgumentException("ERROR AT &");
                    
                }
                
                else if(bool.Parse(t1.Content) && bool.Parse(t2.Content))
                {
                     return new Token{Type = "bool", Content = "true"};
                }
                else return new Token{Type = "bool", Content = "false"};
            }

            else if(op == "|")
            {
                if(t1.Type != "bool")
                {
                    throw new ArgumentException("ERROR AT &");
                    
                }
                
                else if(bool.Parse(t1.Content) || bool.Parse(t2.Content))
                {
                     return new Token{Type = "bool", Content = "true"};
                }
                else return new Token{Type = "bool", Content = "false"};
            }
           throw new ArgumentException("Invalid op");
        
        }

        public static Token LetIn(Token token)
        {
            //Diccionario que va guardando las variables y su valor correspondiente
            Dictionary<string, Token> vars = new Dictionary<string, Token>();
            List<Token> aux_toks = new List<Token>();
            string? name = null;
            int count = 0;


            if (token.Content != "let-in")
            {
                throw new ArgumentException("INVALID TOKEN AT LetIn Function, EXPECTED: let-in token");
            }

            //Se guardan las respectivas variables

            for (int i = 0; i < token.exp.Count; i++)
            {
                if (count == 0)
                {
                    if (token.exp[i].Type != "iden")
                    {
                        throw new ArgumentException("INVALID TOKEN in let in expresion");
                    }

                    else
                    {
                        name = token.exp[i].Content;
                        count += 1;
                        continue;
                    }
                }

                else if (count == 1)
                {
                    if (token.exp[i].Content != "=")
                    {
                        throw new ArgumentException("EXPECTED = IN let-in expresion");

                    }
                    else
                    {
                        count += 1;
                        continue;
                    }
                }

                else if (count == 2)
                {
                    if (token.exp[i].Content == ",")
                    {
                        if (!vars.ContainsKey(name))
                        {
                            vars.Add(name, Parser.Parse(aux_toks, 1).GetValue());
                            name = null;
                            aux_toks.Clear();
                            count = 0;
                            continue;
                        }
                        else
                        {
                            vars[name] = Parser.Parse(aux_toks, 1).GetValue();
                            name = null;
                            aux_toks.Clear();
                            count = 0;
                            continue;
                        }
                    }
                    else if (i == token.exp.Count - 1)
                    {
                        if (!vars.ContainsKey(name))
                        {
                            aux_toks.Add(token.exp[i]);
                            vars.Add(name, Parser.Parse(aux_toks, 1).GetValue());
                            name = null;
                            aux_toks.Clear();

                            continue;
                        }
                        else
                        {
                            aux_toks.Add(token.exp[i]);
                            vars[name] = Parser.Parse(aux_toks, 1).GetValue();
                            name = null;
                            aux_toks.Clear();
                            continue;
                        }
                    }
                    else
                    {
                        aux_toks.Add(token.exp[i]);
                    }
                }
            }

            vars.Reverse(); //Para que se sobreescriban si se repiten.

            //ahora se modifica lo que viene despues del in



            for (int i = 0; i < token.exp_2.Count; i++)
            {
                if (token.exp_2[i].Type == "iden")
                {
                    if (vars.ContainsKey(token.exp_2[i].Content))
                    {
                        token.exp_2[i] = vars[token.exp_2[i].Content];
                    }
                }
                else if (token.exp_2[i].Type == "mix" || token.exp_2[i].Type == "func" )
                {

                    token.exp_2[i].Remplace(vars);
                }
            }

            return Parser.Parse(token.exp_2, 1).GetValue();



        }

        public static Token IfElse(Token token)
        {
            string tbool = Parser.Parse(token.bool_exp, 1).GetValue().Content.ToString();
            if (token.Content != "if-else")
            {
                throw new ArgumentException();
            }

            else if (tbool == "true")
            {
                return Parser.Parse(token.exp, 1).GetValue();
            }

            else if (tbool == "false")
            {
                return Parser.Parse(token.exp_2, 1).GetValue();
            }

            throw new ArgumentException();
        }

        public static Token StringSum(Token token_1, Token token_2)
        {
            if (token_1.Type != "strings" && token_1.Type != "number")
            {
                throw new ArgumentException("expected string token at left @ operator");
            }

            else if (token_2.Type == "number" || token_2.Type == "strings")
            {
                return new Token() { Type = "strings", Content = token_1.Content + token_2.Content };
            }

            else throw new ArgumentException("invalid token: " + token_2 + " at @ right ");
        }

        public static Token UnaryNegation(Token token)
        {
            if(token.Type != "bool")
            {
                throw new ArgumentException("expected bool");
            }
            return new Token(){Type = "bool", Content = (!bool.Parse(token.Content)).ToString().ToLower()};
        }
        public static Token UnaryMinus(Token token)
        {
            if(token.Type != "number")
            {
                throw new ArgumentException();
            }

            return new Token(){Type = "number" , Content = (-(token.ToInt())).ToString()};
        }

        public static List<Token> Clone(this List<Token> tokens)
        {
           List<Token> tokns = new List<Token>();
           
           if(tokens == null)
           {
            return null;
           }

           foreach (Token item in tokens)
           {
               tokns.Add(item.Clone());
           }

           return tokns;
        }
    }
}