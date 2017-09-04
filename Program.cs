using CoreAnalisys.Data;
using CoreAnalisys.TokenBase;
using System;
using System.IO;

namespace CoreAnalisys
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokenProcessor = new TokenProcessor();
            tokenProcessor.ProcessSourceCode();
            Console.Write("Done!");
        }
    }
}