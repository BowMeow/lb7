using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ЛР1
{
    public class Lexer
    {
        private string _input;
        private int _position;
        private List<Token> _tokens;

        private static readonly Dictionary<string, string> TokenPatterns = new Dictionary<string, string>
    {
        { "IF", "^IF" },
        { "THEN", "^THEN" },
        { "REL_OP", "^(==|<=|>=|!=|<|>|=)" },
        { "PLUS", "^\\+" },
        { "MULTIPLY", "^\\*" },
        { "LPAREN", "^\\(" },
        { "RPAREN", "^\\)" },
        { "ID", "^[a-zA-Z][a-zA-Z0-9]*" },
        { "NUMBER", "^\\d+" },
        { "WHITESPACE", "^\\s+" }
    };

        public Lexer(string input)
        {
            _input = input;
            _tokens = new List<Token>();
            _position = 0;
        }

        public List<Token> Tokenize()
        {
            while (_position < _input.Length)
            {
                bool matched = false;
                foreach (var pattern in TokenPatterns)
                {
                    var regex = new Regex(pattern.Value);
                    var match = regex.Match(_input.Substring(_position));

                    if (match.Success)
                    {
                        if (pattern.Key != "WHITESPACE")
                        {
                            _tokens.Add(new Token(pattern.Key, match.Value));
                        }
                        _position += match.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    throw new Exception($"Unexpected character at position {_position}");
                }
            }
            return _tokens;
        }
    }
}
