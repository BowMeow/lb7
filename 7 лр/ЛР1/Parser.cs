using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _position;
        private List<string> _parseTree;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _parseTree = new List<string>();
        }

        public void Parse()
        {
            ConditionalOperator();
            if (_position < _tokens.Count)
            {
                throw new Exception("Unexpected tokens after valid input");
            }
        }

        public List<string> GetParseTree()
        {
            return _parseTree;
        }

        private void ConditionalOperator()
        {
            Match("IF");
            _parseTree.Add("<условный оператор>");
            Condition();
            Match("THEN");
            Operator();
        }

        private void Condition()
        {
            _parseTree.Add("<условие>");
            Expression();
            RelationalOperation();
            Expression();
        }

        private void Expression()
        {
            _parseTree.Add("<выражение>");
            Term();
            while (CurrentToken().Type == "PLUS")
            {
                Match("PLUS");
                Term();
            }
        }

        private void Term()
        {
            _parseTree.Add("<терм>");
            Factor();
            while (CurrentToken().Type == "MULTIPLY")
            {
                Match("MULTIPLY");
                Factor();
            }
        }

        private void Factor()
        {
            _parseTree.Add("<множитель>");
            if (CurrentToken().Type == "ID")
            {
                Identifier();
            }
            else if (CurrentToken().Type == "NUMBER")
            {
                _parseTree.Add("<число>");
                Match("NUMBER");
            }
            else if (CurrentToken().Type == "LPAREN")
            {
                Match("LPAREN");
                Expression();
                Match("RPAREN");
            }
            else
            {
                throw new Exception($"Unexpected token: {CurrentToken().Value}");
            }
        }

        private void Identifier()
        {
            _parseTree.Add("<идентификатор>");
            Match("ID");
        }

        private void Operator()
        {
            _parseTree.Add("<оператор>");
            if (CurrentToken().Type == "IF")
            {
                ConditionalOperator();
            }
            else if (CurrentToken().Type == "ID")
            {
                Assignment();
            }
            else
            {
                Expression();
            }
        }

        private void Assignment()
        {
            Identifier();
            Match("REL_OP");
            Expression();
        }

        private void RelationalOperation()
        {
            _parseTree.Add("<операция отношения>");
            switch (CurrentToken().Type)
            {
                case "REL_OP":
                    Match("REL_OP");
                    break;
                default:
                    throw new Exception($"Unexpected token: {CurrentToken().Value}");
            }
        }

        private void Match(string tokenType)
        {
            if (_position < _tokens.Count && _tokens[_position].Type == tokenType)
            {
                
                _position++;
            }
            else
            {
                throw new Exception($"Expected token {tokenType} but found {_tokens[_position].Type}");
            }
        }

        private Token CurrentToken()
        {
            if (_position < _tokens.Count)
            {
                return _tokens[_position];
            }
            return new Token("EOF", ""); // EOF (End of File) token
        }
    }
}
