using System.Collections.Generic;
using System.Security.Principal;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace Compiler
{

   
    public class Token
    {

        public override string ToString()
        {
            return Type + "{" + Content + "}";
        }

        public  Token Clone()
        {
           Token token = new Token();
           token.Type = Type;
           token.Content = Content;
           token.bool_exp = bool_exp.Clone();
           token.exp = exp.Clone();
           token.exp_2 = exp_2.Clone();
           return token;
        }


        //
        public void Remplace(Dictionary<string, Token> vars)
        {
            
          
             if(Type == "mix" && Content == "cpar")
            {
                for (int i = 0; i < exp.Count; i++)
                {
                    
                    if(exp[i].Type == "iden")
                    {
                        if(vars.ContainsKey(exp[i].Content))
                        {
                                exp[i] = vars[exp[i].Content];
                        }
                    }
                    else exp[i].Remplace(vars);
                }
            }
           else if (Type != "let-in")
            {
                if (Content == "if-else")
                {
                    for (int i = 0; i < bool_exp.Count; i++)
                    {
                        {
                            bool_exp[0].Remplace(vars);
                        }

                    }

                    for (int i = 0; i < exp.Count; i++)
                    {
                        if (exp[i].Type == "iden")

                        {
                            if (vars.ContainsKey(exp[i].Content))
                            {
                                exp[i] = vars[exp[i].Content];
                            }
                        }

                        else if (exp[i].Type == "mix"|| exp[i].Type == "func")
                        {
                            exp[i].Remplace(vars);
                        }

                    }
                    for (int i = 0; i < exp_2.Count; i++)
                    {
                        if (exp_2[i].Type == "iden")

                        {
                            if (vars.ContainsKey(exp_2[i].Content))
                            {
                                exp_2[i] = vars[exp_2[i].Content];
                            }
                        }
                        else if (exp_2[i].Type == "mix" || exp_2[i].Type == "func")
                        {
                            exp_2[i].Remplace(vars);
                        }

                    }
                }
                if (exp != null)
                {
                    for (int i = 0; i < exp.Count; i++)
                    {
                        if (exp[i].Type == "iden")

                        {
                            if (vars.ContainsKey(exp[i].Content))
                            {
                                exp[i] = vars[exp[i].Content];
                            }
                        }

                        else if (exp[i].Type == "mix" || exp[i].Type == "func")
                        {
                            exp[i].Remplace(vars);
                        }

                    }
                }
                else if (exp_2 != null)
                {
                    for (int i = 0; i < exp_2.Count; i++)
                    {
                        if (exp_2[i].Type == "iden")

                        {
                            if (vars.ContainsKey(exp_2[i].Content))
                            {
                                exp_2[i] = vars[exp_2[i].Content];
                            }
                        }
                        else if (exp_2[i].Type == "mix" || exp_2[i].Type == "func")
                        {
                            exp_2[i].Remplace(vars);
                        }

                    }
                }
            }
            else if(Type == "func")
            {
                exp[0].Remplace(vars);
            }
            else if (Type == "let-in")
            {
                for (int i = 0; i < exp_2.Count; i++)
                {
                    if (exp_2[i].Type == "iden")

                    {
                        if (vars.ContainsKey(exp_2[i].Content))
                        {
                            exp_2[i] = vars[exp_2[i].Content];
                        }
                    }
                    else if (exp_2[i].Type == "mix")
                    {
                        exp_2[i].Remplace(vars);
                    }

                }
            }

            
        }

        public void TryConvertToBool()
        {
            if (Type == "iden")
            {
                if (Content == "true" || Content == "false")
                {
                    Type = "bool";
                }


            }
        }


        public bool IsSimple;
        public string? Type;
        public string? Content;

        public List<Token>? bool_exp = null;
        public List<Token>? exp = null;
        public List<Token>? exp_2 = null;
    }

    public static class Lexer
    {

        public static List<Token> TokensInit(string input)
        {
            List<Token> tlist = new List<Token>();
            Token? currentToken = null;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '"')
                {
                    if (state == "strings")
                    {
                        //currentToken.Content += '"';
                        tlist.Add(currentToken);
                        currentToken = null;
                        state = "none";
                        continue;
                    }
                    else
                    {
                        currentToken = new Token { Type = "strings", Content = "" };
                        state = "strings";
                        continue;
                    }
                }
                else if (state == "strings")
                {
                    currentToken.Content += input[i];
                    continue;
                }

                if (Char.IsLetter(input[i]))
                {
                    if (state == "none")
                    {
                        currentToken = new Token { Type = "iden", Content = input[i].ToString() };
                        state = "iden";
                    }

                    else if (state == "iden")
                    {
                        currentToken.Content = currentToken.Content + input[i];
                    }

                    else if (state == "Symbol" || state == "Operator" || state == "Parnt")
                    {
                        tlist.Add(currentToken);
                        currentToken = new Token { Type = "iden", Content = input[i].ToString() };
                        state = "iden";

                    }
                    else if (state == "number")
                    {
                        string token = currentToken.Content;
                        currentToken = new Token { Type = "LEXICAL ERROR:" + token + " INVALID TOKEN" };
                        return new List<Token> { currentToken };
                    }
                }

                else if (Char.IsNumber(input[i]))
                {
                    if (state == "number")
                    {
                        currentToken.Content = currentToken.Content + input[i];
                    }

                    else if (state == "none")
                    {
                        currentToken = new Token { Type = "number", Content = input[i].ToString() };
                        state = "number";
                    }
                    else if (state == "Symbol" || state == "Operator" || state == "Parnt")
                    {
                        tlist.Add(currentToken);
                        currentToken = new Token { Type = "number", Content = input[i].ToString() };
                        state = "number";

                    }
                    else if (state == "iden")
                    {
                        string token = currentToken.Content;
                        currentToken = new Token { Type = "LEXICAL ERROR:" + token + " INVALID TOKEN" };
                        return new List<Token> { currentToken };
                    }
                }

                else if (input[i] == ')' || input[i] == '(')
                {
                    if (currentToken != null)
                    {
                        tlist.Add(currentToken);
                        state = "none";
                        currentToken = null;

                    }



                    tlist.Add(new Token { Type = "Parnt", Content = input[i].ToString() });
                }
                else if (input[i] == '+' || input[i] == '-' || input[i] == '*' || input[i] == '/' || input[i] == '^' || input[i] == '&' || input[i] == '|' || input[i] == '!' || input[i] == '<' || input[i] == '>' || input[i] == '@' || input[i] == '%')
                {

                    if (currentToken != null)
                    {
                        if (currentToken.Content == "=" && input[i] == '>')
                        {
                            currentToken.Content = "=>";
                            tlist.Add(currentToken);
                            state = "none";
                            currentToken = null;
                            continue;
                        }
                        tlist.Add(currentToken);

                    }

                    state = "Operator";

                    currentToken = new Token { Type = "Operator", Content = input[i].ToString() };
                }

                else if (input[i] == '=')
                {
                    if (state == "Operator")
                    {
                        if (true)
                            currentToken.Content += "=";
                        else
                        {

                            string token = currentToken.Content;
                            currentToken = new Token { Type = "LEXICAL ERROR:" + token + " INVALID TOKEN CANDELA---" };
                            return new List<Token> { currentToken };
                        }
                    }
                    else
                    {
                        if (state != "none")
                        {
                            tlist.Add(currentToken);
                        }
                        currentToken = new Token { Type = "Operator", Content = input[i].ToString() };
                        state = "Operator";
                    }
                }
                else if (input[i] == ' ')
                {

                    if (state != "none")
                    {
                        tlist.Add(currentToken);
                        currentToken = null;
                        state = "none";
                    }
                    continue;

                }

                else if (input[i] == ',')
                {
                    if (state != "none")
                    {
                        tlist.Add(currentToken);
                        currentToken = null;
                    }
                    tlist.Add(new Token() { Type = "Symbol", Content = "," });
                    state = "none";
                }


            }

            if (currentToken != null)
            {
                tlist.Add(currentToken);
            }

            currentToken = null;
            state = "none";
            return tlist;
        }

        //Se utiliza para inicializar los tokens compuestos
        public static List<Token> GetToken2(List<Token> tokens)
        {
            List<Token> toks = new List<Token>();
            List<Token> toks_aux;
            int p = 0;
            Token? cToken = null;


            bool IsOpen = false; //Verdadero si se esta construyendo un token mixto.


            //Se construyen primero expresiones con los parentesis.
            for (int i = 0; i < tokens.Count; i++)
            {
                tokens[i].TryConvertToBool();
                if (tokens[i].Content == "(")
                {
                    p += 1;
                    if (p == 1 && !IsOpen)
                    {
                        cToken = new Token() { Type = "mix", Content = "cpar", exp = new List<Token>() };
                        IsOpen = true;
                        continue;
                    }
                    else if (IsOpen)
                    {
                        cToken.exp.Add(tokens[i]);
                        continue;
                    }
                }
                else if (tokens[i].Content == ")")
                {
                    p -= 1;
                    if (p == 0 && IsOpen)
                    {
                        cToken.exp = GetToken2(cToken.exp);
                        toks.Add(cToken);
                        cToken = null;
                        IsOpen = false;
                        continue;
                    }

                    else if (p < 0) throw new ArgumentException("INVALID ) TOKEN");

                    else cToken.exp.Add(tokens[i]);
                }

                else if (IsOpen)
                {
                    cToken.exp.Add(tokens[i]);
                    continue;

                }
                else
                {
                    toks.Add(tokens[i]);
                    continue;
                }

            }

            //Se resetean los datos
            cToken = null;
            IsOpen = false;
            toks_aux = toks;
            toks = new List<Token>();
            int part = 0;


            //Se inician las expresiones let-in
            for (int i = 0; i < toks_aux.Count; i++)
            {

                if (!IsOpen)
                {
                    if (toks_aux[i].Content == "let")
                    {
                        cToken = new Token() { Type = "mix", Content = "let-in", exp = new List<Token>(), exp_2 = new List<Token>() };
                        part = 1;
                        IsOpen = true;
                        continue;

                    }
                    else if (toks_aux[i].Content == "in")
                    {
                        throw new ArgumentException("INVALID USE OF RESERVED WORD in");
                    }
                    else toks.Add(toks_aux[i]);
                    continue;
                }
                else if (IsOpen)
                {
                    if (toks_aux[i].Content == "in")
                    {
                        part = 2;
                    }
                    else
                    {
                        if (part == 1)
                        {
                            cToken.exp.Add(toks_aux[i]);
                        }

                        else if (part == 2)
                        {
                            cToken.exp_2.Add(toks_aux[i]);
                        }
                    }
                }


            }
            if (IsOpen)
            {
                cToken.exp = GetToken2(cToken.exp);
                cToken.exp_2 = GetToken2(cToken.exp_2);

                toks.Add(cToken);

                IsOpen = false;
            }


            //Se resetean los datos
            cToken = null;
            IsOpen = false;
            toks_aux = toks;
            toks = new List<Token>();
            part = 0;

            //Se inician los tokens tipo if-else

            for (int i = 0; i < toks_aux.Count; i++)
            {
                if (!IsOpen)
                {
                    if (toks_aux[i].Content == "if")
                    {
                        cToken = new Token() { Type = "mix", Content = "if-else", exp = new List<Token>(), exp_2 = new List<Token>() };
                        part = 1;
                        IsOpen = true;
                        continue;

                    }
                    else if (toks_aux[i].Content == "else")
                    {
                        throw new ArgumentException("INVALID USE OF RESERVED WORD else ");
                    }
                    else toks.Add(toks_aux[i]);
                    continue;
                }
                else if (IsOpen)
                {
                    if (part == 1)
                    {
                        if (toks_aux[i].Content == "cpar")
                        {
                            cToken.bool_exp = new List<Token>() { toks_aux[i] };
                            part = 2;
                            continue;
                        }
                        else throw new ArgumentException("missing () expression");
                    }
                    if (part == 2)
                    {
                        if (toks_aux[i].Content == "else")
                        {
                            part = 3;
                            continue;
                        }
                        else cToken.exp.Add(toks_aux[i]); ;
                    }
                    else if (part == 3)
                    {

                        cToken.exp_2.Add(toks_aux[i]);

                    }
                }
            }

            if (IsOpen)
            {

                cToken.exp = GetToken2(cToken.exp);
                cToken.exp_2 = GetToken2(cToken.exp_2);

                toks.Add(cToken);

                IsOpen = false;
            }

             cToken = null;
            IsOpen = false;
            toks_aux = toks;
            toks = new List<Token>();
            part = 0;

            //Se inician las funciones
            for (int i = 0; i < toks_aux.Count; i++)
            {
                if(toks_aux[i].Type == "iden" && i + 1 < toks_aux.Count  && toks_aux[i + 1].Content == "cpar")
                {
                    Token tk = new Token(){Type = "func", Content = toks_aux[i].Content,exp = new List<Token>(){toks_aux[i + 1]}};
                    i += 1;
                    toks.Add(tk);
                }
                  else toks.Add(toks_aux[i]); 
            }

            return toks;


        }

        //static Token? currentToken = null;
        static string state = "none";
    }
}