// Author: William Daniel
// Date: 19/09/2022

using System.Text.RegularExpressions;
/* Alteração não mesclada do projeto 'Lexer (net6.0-maccatalyst)'
Antes:
namespace GrauB_Lexer
Após:
namespace GrauB_Lexer;
*/


namespace Lexer.Core.Helpers
{
    public class Lexeme
    {
        public string value { get; set; }
        public TokenType token { get; set; }
        public int index { get; set; }

        public Lexeme() { }

        public Lexeme(string value, TokenType token, int index)
        {
            this.value = value;
            this.token = token;
            this.index = index;
        }

        public override string ToString()
        {
            var paddingLength = 20 - value.Length;
            var newString = $"'{value}'".PadRight(paddingLength >= 0 ? paddingLength : 0);

            return $"{newString}-> {token.GetName()}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return index == ((Lexeme)obj).index && token == ((Lexeme)obj).token && value == ((Lexeme)obj).value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode() + token.GetName().GetHashCode() + index.GetHashCode();
        }
    }

    public class LexemeComparer : IEqualityComparer<Lexeme>
    {

        public bool Equals(Lexeme x, Lexeme y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Lexeme obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class LexemeHelper
    {
        private static List<(TokenType token, Regex regexPattern)> lexemeDictionary
        {
            get
            {
                return new List<(TokenType token, Regex regexPattern)> {
                    (TokenType.DELIM,       new Regex("[(){}]")),
                    (TokenType.NUM,         new Regex(@"\d+(\.\d*)?")),
                    (TokenType.CMD_IF,      new Regex("(if)")),
                    (TokenType.CMD_ELSE,    new Regex("(else)")),
                    (TokenType.CMD_WHILE,   new Regex("(while)")),
                    (TokenType.ID,          new Regex("[A-Za-z]+")),
                    (TokenType.OP_ADD,      new Regex(@"\+")),
                    (TokenType.OP_SUB,      new Regex(@"\-")),
                    (TokenType.OP_MUL,      new Regex(@"\*")),
                    (TokenType.OP_DIV,      new Regex(@"\/")),
                    (TokenType.OP_GT,       new Regex(@"\>")),
                    (TokenType.OP_GT_OR_EQ, new Regex("(>=)")),
                    (TokenType.OP_LT,       new Regex(@"\<")),
                    (TokenType.OP_LT_OR_EQ, new Regex("(<=)")),
                    (TokenType.OP_NOT_EQ,   new Regex("(!=)")),
                    (TokenType.OP_EQ,       new Regex("(==)")),
                    (TokenType.CMD_ATR,     new Regex("(=)")),
                    (TokenType.SKIP,        new Regex("[ \r\n\t]"))
                };
            }
        }

        public static IList<Lexeme> MapLexemes(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new Exception("Your source code is empty.");

            var strs = input.Split("\r");
            var regex = GetCompleteRegexPattern();

            IList<Lexeme> lexemes = new List<Lexeme>();
            foreach (var str in strs)
            {
                var matches = new Regex(regex).Matches(str);
                var nonMatchingString = str;

                foreach (Match match in matches.Where(x => !x.Value.Equals("") && !x.Value.Contains("\r")))
                {
                    nonMatchingString = nonMatchingString.Remove(match.Index, match.Length);
                    nonMatchingString = nonMatchingString.PadLeft(nonMatchingString.Length + match.Length);

                    foreach (var (token, regexPattern) in lexemeDictionary)
                        if (regexPattern.IsMatch(match.Value) && token != TokenType.SKIP)
                        {
                            lexemes.Add(new Lexeme(match.Value, token, match.Index));
                            break;
                        }
                }

                nonMatchingString = nonMatchingString.Replace(" ", "");
                if (!string.IsNullOrEmpty(nonMatchingString))
                    throw new Exception($"Token(s) found -> {nonMatchingString}");
            }

            return lexemes;
        }

        public static IList<Lexeme> RemoveDuplicated(this IList<Lexeme> lexemes)
        {
            IList<Lexeme> newLexemes = new List<Lexeme>();

            foreach (var obj in lexemes.Select((lexeme, index) => (lexeme, index)))
            {
                if (obj.index == 0 || obj.index > 0 && !obj.lexeme.value.Equals(lexemes[obj.index - 1].value))
                    newLexemes.Add(obj.lexeme);
            }

            return newLexemes;
        }

        private static string GetCompleteRegexPattern()
        {
            string regex = string.Empty;
            foreach (var reg in lexemeDictionary)
            {
                regex += reg.regexPattern.ToString() + "|";
            }

            return regex;
        }

        public static Lexeme Empty(this Lexeme lexeme) => new Lexeme("&", TokenType.END, -1);
    }
}
