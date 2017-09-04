using CoreAnalisys.Data;
using CoreAnalisys.TokenBase;
using CoreAnalisys.TokenBase.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreAnalisys.TokenBase
{
    public class InformationalTables
    {
        public Dictionary<string, int> _KeyWords;
        public Dictionary<string, int> _Identifiers;
        public Dictionary<string, int> _Constants;
        public Dictionary<string, int> _Delimeters;

        private const string INIT_TOKENS_PATH = @"Data/InputData/InformationalTables.json";
        private const string OUTPUT_PATH = @"Data/OutputData/";

        private int GenerateDictKey(TokenInfo token, Dictionary<string, int> dict)
        {
            if (dict.Keys.Count > 0)
            {
                return GetDictMaxKey(dict);
            }
            else
            {
                if (token.type == SPECIAL_CODES.IDENTIFIERS)
                {
                    return SPECIAL_CODES.START_IDENT;
                }
                if (token.type == SPECIAL_CODES.CONST_CODE)
                {
                    return SPECIAL_CODES.START_CONST;
                }
                if (token.type == SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS)
                {
                    return SPECIAL_CODES.START_DELIM;
                }
            }
            return 0;
        }
        public void DumpTablesCurrentState()
        {
            WriteToJson("KeyWords.json", JsonConvert.SerializeObject(_KeyWords));
            WriteToJson("Identifiers.json", JsonConvert.SerializeObject(_Identifiers));
            WriteToJson("Constants.json", JsonConvert.SerializeObject(_Constants));
            WriteToJson("Delimeters.json", JsonConvert.SerializeObject(_Delimeters));
        }
        
        
        public int GetDictMaxKey(Dictionary<string, int> dict)
        {
            var values = new List<int>(dict.Values);
            values.Sort();
            var max_val = values[0];
            values.Clear();
            return max_val;
        }
        private int AddValueToDict(TokenInfo token, Dictionary<string, int> destination)
        {
            var tKey = GenerateDictKey(token, destination);
            destination.Add(token.value, tKey);
            return tKey;
        }

        private int AddIdentifierToDict(TokenInfo token, Dictionary<string, int> destination)
        {
            if (IsInKeyWord(token.value))
            {
                return _KeyWords[token.value];
            }
            else
            {
                if (IsInIndent(token.value))
                {
                    return _Identifiers[token.value];
                }
            }
            return AddValueToDict(token, _Identifiers);
        }
        private int AddConstToDict(TokenInfo token, Dictionary<string, int> destination)
        {
            if (IsInConsts(token.value))
            {
                return _Constants[token.value];
            }
            return AddValueToDict(token, destination);
        }
        public int AddTokenToDict(TokenInfo token)
        {
            if (token.type == SPECIAL_CODES.IDENTIFIERS)
            {
                return AddIdentifierToDict(token, _KeyWords);
            }
            if (token.type == SPECIAL_CODES.CONST_CODE)
            {
                return AddConstToDict(token, _Constants);
            }
            if (token.type == SPECIAL_CODES.SINGLE_DIGIT_DELIMITERS)
            {
                return _Delimeters[token.value];
            }
            return 0;
        }
        private bool IsInKeyWord(string val)
        {
            return _KeyWords.ContainsKey(val);
        }
        private bool IsInIndent(string val)
        {
            return _Identifiers.ContainsKey(val);
        }
        private bool IsInConsts(string val)
        {
            return _Constants.ContainsKey(val);
        }

        public InformationalTables()
        {
            var RawInfo = GetRawInformation();
            InitInnerTable(ref _KeyWords, RawInfo["KeyWords"]);
            InitInnerTable(ref _Identifiers, RawInfo["Identifiers"]);
            InitInnerTable(ref _Constants, RawInfo["Constants"]);
            InitInnerTable(ref _Delimeters, RawInfo["Delimiters"]);
            RawInfo.Clear();
        }

        private void InitInnerTable(ref Dictionary<string, int> detstination, Dictionary<string, int> sourse)
        {
            detstination = new Dictionary<string, int>(sourse);
        }

        private ComplexDict GetRawInformation()
        {
            using (var jsonStream = new FileStream(INIT_TOKENS_PATH, FileMode.Open))
            {
                using (var jsonReader = new StreamReader(jsonStream))
                {
                    return JsonConvert.DeserializeObject<ComplexDict>(jsonReader.ReadToEnd());
                }
            }
        }
        public void WriteToJson(string fileName, string source)
        {
            using (var jsonStream = new FileStream(OUTPUT_PATH + fileName, FileMode.Truncate, FileAccess.Write))
            {
                using (var jsonwritter = new StreamWriter(jsonStream))
                {
                    jsonwritter.Write(source);
                }
            }
        }
    }
}
