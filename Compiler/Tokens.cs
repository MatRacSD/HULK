using System;

namespace Compiler
{


public class Token
{

    public override string ToString()
    {
        return Type + "{" + Content + "}";
    }
    public string? Type;
    public string? Content;
}

public static class Lexer
{
    
    public static List<Token> TokensInit(string input)
    {
        List<Token> tlist = new List<Token>();
        
        for (int i = 0; i < input.Length; i++)
        {
            if(input[i]== '"')
            {
                if(state == "Sstring")
                {
                    currentToken.Content += '"';
                    tlist.Add(currentToken);
                    currentToken = null;
                    state = "none";
                    continue;
                }
                else 
                {
                    currentToken = new Token{Type = "Sstring", Content = input[i].ToString()};
                    state = "Sstring";
                    continue;
                }
            }
            else if (state == "Sstring")
            {
                currentToken.Content += input[i];
                continue;
            }
            
            if(Char.IsLetter(input[i]))
            {
                if(state == "none")
                {
                    currentToken = new Token{Type = "string" , Content = input[i].ToString()};
                    state = "string";
                }

                else if(state == "string")
                {
                    currentToken.Content = currentToken.Content + input[i];
                }

                else if (state == "Symbol" || state == "Operator" || state == "Parnt")
                {
                    tlist.Add(currentToken);
                    currentToken = new Token{Type = "string" , Content = input[i].ToString() };
                    state = "string";

                }
                else if (state== "number" )
                {
                    string token = currentToken.Content;
                    currentToken = new Token{Type = "LEXICAL ERROR:" + token + " INVALID TOKEN" };
                    return new List<Token>{currentToken};
                }
            }

            else if (Char.IsNumber(input[i]))
            {
                if(state == "number")
                {
                    currentToken.Content = currentToken.Content + input[i];
                }

                else if (state == "none")
                {
                    currentToken = new Token{Type = "number" , Content = input[i].ToString() };
                    state = "number";
                }
                else if (state == "Symbol" || state == "Operator" || state == "Parnt")
                {
                    tlist.Add(currentToken);
                    currentToken = new Token{Type = "number" , Content = input[i].ToString() };
                    state = "number";

                }
                else if (state== "string" )
                {
                    string token = currentToken.Content;
                    currentToken = new Token{Type = "LEXICAL ERROR:" + token + " INVALID TOKEN" };
                    return new List<Token>{currentToken};
                }
            }

            else if(input[i] == ')'||input[i] == '(')
            {
                if(currentToken != null)
                {
                    tlist.Add(currentToken);
                    state = "none";
                    currentToken = null;

                }

                

                tlist.Add( new Token{Type = "Parnt", Content = input[i].ToString()});
            }
            else if(input[i] == '+' || input[i] == '-' || input[i] == '*' || input[i] == '/'||input[i] == '^'||input[i] == '&'||input[i] == '|'||input[i] == '!'||input[i] == '<'||input[i] == '>')
            {
                  if(currentToken != null)
                {
                    tlist.Add(currentToken);

                }

                state = "Operator";

                currentToken = new Token{Type = "Operator", Content = input[i].ToString()};
            }
            
            else if(input[i]== '=')
            {
                if(state == "Operator"){
                    if(currentToken.Content == "="||currentToken.Content=="!")
                    currentToken.Content += "=";
                    else{
                        
                    string token = currentToken.Content;
                    currentToken = new Token{Type = "LEXICAL ERROR:" + token + " INVALID TOKEN" };
                    return new List<Token>{currentToken};
                    }
                }
                else 
                {
                    currentToken = new Token{Type = "Operator", Content = input[i].ToString()};
                }
            }
            else if(input[i]== ' ')
            {
                tlist.Add(currentToken);
                currentToken = null;
                state = "none";
            }
        
        
        }

        if(currentToken != null)
        {
            tlist.Add(currentToken);
        }

        currentToken = null;
        state =  "none";
        return tlist;
    }
    
    static Token? currentToken = null;
    static string state = "none"; 
}
}