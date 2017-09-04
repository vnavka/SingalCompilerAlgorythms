using CoreAnalisys.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAnalisys.CharBase
{
    public static class CharTypeIdentifier
    {
        public static int GetCharType(int code)
        {
            if (IsWhiteSpace(code))
            {
                return SPECIAL_CODES.ISWHITESPACES_CODE;
            }
            if (IsLiteral(code))
            {
                return SPECIAL_CODES.IDENTIFIERS;
            }
            if (IsDigit(code))
            {
                return SPECIAL_CODES.CONST_CODE;
            }
            if (IsDelimeter(code))
            {
                if (IsMultyDelimeter(code))
                    return SPECIAL_CODES.MULTI_CHARACTER_DELIMITERS;
                return SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS;
            }
            return SPECIAL_CODES.UNEXPECTED_CHAR_ERROR_CODE;
        }
        private static bool IsWhiteSpace(int code)
        {
            return (code == SPECIAL_CODES.END_OF_TEXT
                   || code == SPECIAL_CODES.BACKSPACE_CHAR
                   || code == SPECIAL_CODES.HORIZONTAL_TAB
                   || code == SPECIAL_CODES.NEW_LINE
                   || code == SPECIAL_CODES.CARRIAGE_RETURN
                   || code == SPECIAL_CODES.SPACE_CHAR);
        }
        private static bool IsLiteral(int code)
        {
            return ((int)'A' <= code && code <= (int)'Z');
        }
        private static bool IsDigit(int code)
        {
            return ((int)'0' <= code && code <= (int)'9');
        }
        private static bool IsDelimeter(int code)
        {
            return (code == (int)';'
                    || code == (int)'+'
                    || code == (int)'-'
                    || code == (int)':'
                    || code == (int)'=');
        }
        private static bool IsMultyDelimeter(int code)
        {
            return (code == (int)':');
        }
    }
}
