using Lexer.Core.Helpers;

namespace Lexer.Core.UseCase
{
    public class LexerAnalyzer
    {
        public LexerAnalyzer() { }
        public IList<Lexeme> TryToAnalyze(string sourceCode)
        {
            return LexemeHelper.MapLexemes(sourceCode);
        }
    }
}
