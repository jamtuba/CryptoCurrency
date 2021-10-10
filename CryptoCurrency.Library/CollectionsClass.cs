using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CryptoCurrency.Library
{
    public class CollectionsClass
    {
        Dictionary<string, double> MyTestDictionary;

        public CollectionsClass()
        {
            MyTestDictionary = new Dictionary<string, double>
            {
                { "TestCrypto1", 5.00 },
                { "TestCrypto2", 10.00 }
            };
        }

        public Dictionary<string, double> GetDictionary()
        {
            return MyTestDictionary;
        }

        
    }
}
