using CoreAnalisys.CharBase;
using CoreAnalisys.CharBase.models;
using CoreAnalisys.Data;
using CoreAnalisys.TokenBase.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreAnalisys.TokenBase
{
    
    
    /// <summary>
    /// process words and add to main table
    /// </summary>
    class TokenProcessor
    {
        InformationalTables TokenTable;
        List<TokenTableInfo> lexicalAnalysisResult;
        private const string LEXICALAN_ALYSIS_RESULT_PATH = @"Data/OutputData/lexRes.txt";
        public TokenProcessor()
        {
            TokenTable = new InformationalTables();
            lexicalAnalysisResult = new List<TokenTableInfo>();
        }
        private void AddToLexicalResult(TokenInfo token)
        {
            bool contain_dell_flag = false;
            if (token.type == SPECIAL_CODES.ISWHITESPACES_CODE)
                return;
            if (token.value.Length > 1 && token.value.Contains(";"))
            {
                contain_dell_flag = true;
                token.value = token.value.Remove(token.value.Length - 1);
            }

            var tokenTableItem = new TokenTableInfo()
            {
                value = token.value,
                key = TokenTable.AddTokenToDict(token),
                type = token.type,
                colmn = token.colmn,
                row = token.row
            };
            lexicalAnalysisResult.Add(tokenTableItem);

            if (contain_dell_flag)
            {
                ProcessTokenEndDelimeter(token);
            }
        }

        public void ProcessSourceCode()
        {
            StringBuilder a_token = new StringBuilder("");

            var charReader = new CrarReader();
            while (charReader.canRead)
            {
                var code_symbol = charReader.ReadNextChar();
                var token = new TokenInfo()
                {
                    row = code_symbol.row,
                    colmn = code_symbol.colmn
                };
                if (IsErrorCode(code_symbol))
                    continue;               
                if (code_symbol.chartype == SPECIAL_CODES.IDENTIFIERS)
                {
                    a_token.Append((char)code_symbol.charcode);
                    ProcessIdenifierToken(ref a_token, charReader);
                    token.type = SPECIAL_CODES.IDENTIFIERS;
                }
                if (code_symbol.chartype == SPECIAL_CODES.CONST_CODE)
                {
                    a_token.Append((char)code_symbol.charcode);
                    ProcessConstantToken(ref a_token, charReader);
                    token.type = SPECIAL_CODES.CONST_CODE;
                }
                if (code_symbol.chartype == SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS)
                {
                    a_token.Append((char)code_symbol.charcode);
                    if (code_symbol.chartype == SPECIAL_CODES.MULTI_CHARACTER_DELIMITERS)
                    {
                        ProcessMultyDelimeterToken(ref a_token, charReader);
                    }
                    token.type = SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS;               
                }
                token.value = a_token.ToString();
                AddToLexicalResult(token);
                a_token.Clear();
            }
            TokenTable.DumpTablesCurrentState();
            TokenTable.WriteToJson("lexicalData.json", JsonConvert.SerializeObject(lexicalAnalysisResult));
            SaveToFileLexicalResult();
        }
        private void ProcessTokenEndDelimeter(TokenInfo tInfo)
        {
            var token = new TokenInfo()
            {
                value = ";",
                type = SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS,
                row = tInfo.row,
                colmn = tInfo.colmn
            };
            AddToLexicalResult(token);
        }
        private void ProcessMultyDelimeterToken(ref StringBuilder token, CrarReader reader)
        {
            var code_symbol = reader.ReadNextChar();
            if (code_symbol.chartype == SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS)
            {
                token.Append((char)code_symbol.charcode);
                ProcessMultyDelimeterToken(ref token, reader);
            }
            return;
        }
        private void ProcessConstantToken(ref StringBuilder token, CrarReader reader)
        {
            var code_symbol = reader.ReadNextChar();
            if (code_symbol.chartype == SPECIAL_CODES.CONST_CODE)
            {
                token.Append((char)code_symbol.charcode);
                ProcessConstantToken(ref token, reader);
            }
            else
            {
                if (code_symbol.charcode == (int)';')
                {
                    token.Append((char)code_symbol.charcode);
                }
            }
            return;
        }
        private void ProcessIdenifierToken(ref StringBuilder token, CrarReader reader)
        {
            var code_symbol = reader.ReadNextChar();
            if (code_symbol.chartype == SPECIAL_CODES.IDENTIFIERS
                || code_symbol.chartype == SPECIAL_CODES.CONST_CODE)
            {
                token.Append((char)code_symbol.charcode);
                ProcessIdenifierToken(ref token, reader);
            }
            else
            {
                if (code_symbol.charcode == (int)';')
                {
                    token.Append((char)code_symbol.charcode);
                }
            }
                
            return;
        }
        
        private bool IsErrorCode(CharInfo source)
        {
            if (source.chartype == SPECIAL_CODES.UNEXPECTED_CHAR_ERROR_CODE)
            {
                Console.Write("WARNING!!! Unexpected error code [{0}]-><{1}>",source.charcode, (char)source.charcode);
                Console.Write("__Row={0}_Colmn={1}\n",source.row, source.colmn);
                return true;
            }
            return false;
        }
        private void SaveToFileLexicalResult()
        {
            using (var stream = new FileStream(LEXICALAN_ALYSIS_RESULT_PATH, FileMode.Truncate, FileAccess.Write))
            {
                using (var writter = new StreamWriter(stream))
                {
                    foreach (var item in lexicalAnalysisResult)
                    {
                        writter.WriteLine("[{0}][{1}]<{2}>({3})", item.row, item.colmn, item.key,item.value);
                    }
                }
            }
        }

    }
}
