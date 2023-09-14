namespace Compiler
{
    public static class Parser
    {
       public static AST Parse(List<Token> tokens)
       {

        throw new NotImplementedException();
       }
    }

    public class AST
    {
        public AST(Token token)
        {
           root = new Node(){currentToken = token, Parent = null, LeftChild = null, RightChild = null};
        }
        public Node root;

        public void Insert(Token token)
        {

        }
    }

    public class Node
    {
        public Token currentToken;

        public Node? Parent;       
        public Node? LeftChild;
        public Node? RightChild;
    }

    public class Expression
    {
        public List<Token> exp;
    }
}