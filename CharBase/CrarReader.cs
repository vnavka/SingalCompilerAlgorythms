using CoreAnalisys.CharBase.models;
using CoreAnalisys.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreAnalisys.CharBase
{
    
    
    class CrarReader
    {
        public bool canRead { get; set; }
        private StreamReader stream;
        private CharPosition currentCharPosition;
        private const string FILE_PATH = @"Data/InputData/coreSample.txt";
        public CrarReader()
        {
            stream = null;
            canRead = true;
        }

        public CharInfo ReadNextChar()
        {
            try
            {
                var currentChar = GetSingleChar();
                return currentChar;
            }
            catch (Exception e)
            {
                ReadErrorRun();
                ArgumentException argEx = new ArgumentException("Problem with reading from file", e.Message, e);
                throw argEx;
            }
            finally
            {
                if (stream.EndOfStream)
                {
                    this.canRead = false;
                    stream.Dispose();
                }
            }
        }
        private void ReadErrorRun()
        {
            this.canRead = false;
            stream.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private StreamReader sibgleStream
        {
            get
            {
                if (stream == null)
                {
                    stream = File.OpenText(FILE_PATH);
                }
                return stream;
            }
        }
        private void IncrementCharPosition(int charCode)
        {
            currentCharPosition.currentColmn++;
            if (charCode == SPECIAL_CODES.NEW_LINE)
            {
                currentCharPosition.currentRow++;
                currentCharPosition.currentColmn = 0;
            }
        }
        private CharInfo GetSingleChar()
        {
            var a_local_char = sibgleStream.Read();
            var currentChar = new CharInfo()
            {
                charcode = a_local_char,
                chartype = CharTypeIdentifier.GetCharType(a_local_char),
                colmn = currentCharPosition.currentColmn,
                row = currentCharPosition.currentRow
            };
            IncrementCharPosition(currentChar.charcode);
            return currentChar;
        }
    }
}
