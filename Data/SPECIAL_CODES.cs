using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAnalisys.Data
{
    public static class SPECIAL_CODES
    {
        public const int END_OF_TEXT = 3;
        public const int BACKSPACE_CHAR = 8;
        public const int HORIZONTAL_TAB = 9;
        public const int NEW_LINE = 10;
        public const int CARRIAGE_RETURN = 13;
        public const int SPACE_CHAR = 32;
        //inner Code values

        public const int ISWHITESPACES_CODE = 0;
        public const int CONST_CODE = 1;
        public const int IDENTIFIERS = 2;
        public const int SINGLE_DIGIT_DELIMITERS = 3;
        public const int MULTI_CHARACTER_DELIMITERS = 4;
        public const int COMMENTS_CODE = 5;
        public const int UNEXPECTED_CHAR_ERROR_CODE = 6;

        public const int START_KEY_WORDS = 301;
        public const int START_IDENT = 401;
        public const int START_CONST = 501;
        public const int START_DELIM = 55;

    }
}
