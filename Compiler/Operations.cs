namespace Compiler
{
    public static class Operations
    {
        private static double ToInt(this Token t)
        {
            if(t.Type != "number")
            {
                throw new ArgumentException("Cannot convert the token Content to int in class Operations due to an invalid token type");
            }
            return double.Parse(t.Content);
        }
        public static Token BinaryOperation(Token t1, Token t2,string op)
        {
            if(t1.Type != "number")
            {
                return new Token { Type = "error" , Content = "SYNTAX ERROR: " + t1.Content + " must be a number" };
            }
            else if(t2.Type != "number")
            {
                return new Token { Type = "error" , Content = "SYNTAX ERROR: " + t2.Content + " must be a number" };
            }
            if(op == "+"){
            return new Token { Type = "number", Content = (t1.ToInt() + t2.ToInt()).ToString()};
        }
        else if (op == "-")
        {
            return new Token { Type = "number", Content = (t1.ToInt() - t2.ToInt()).ToString()};
        }
        else if(op == "*")
        {
            return new Token { Type = "number", Content = (t1.ToInt() * t2.ToInt()).ToString()};
        }
        else if (op == "/")
        {
            return new Token { Type = "number", Content = (t1.ToInt() / t2.ToInt()).ToString()};
        }

        return null;
        }
    }
}