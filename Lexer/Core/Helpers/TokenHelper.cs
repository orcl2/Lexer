// Author: William Daniel
// Date: 19/09/2022

namespace Lexer.Core.Helpers
{
    public enum TokenType
    {
        DELIM, NUM, CMD_IF, CMD_ELSE, CMD_WHILE, ID, OP_ADD, OP_SUB, OP_MUL, OP_DIV, OP_GT, OP_GT_OR_EQ, OP_LT, OP_LT_OR_EQ, OP_NOT_EQ, OP_EQ, CMD_ATR, SKIP, END, INVALID
    }

    public static class TokenHelper
    {
        public static string GetName(this TokenType value) => Enum.GetName(typeof(TokenType), value);
    }
}
