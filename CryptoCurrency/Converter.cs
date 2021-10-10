using System;
using System.Collections.Generic;

namespace CryptoCurrency
{
    public class Converter
    {
        Dictionary<string, double> MyCryptoCurrencies = new Dictionary<string, double>
            {
                { "TestCrypto1", 5.00 },
                { "TestCrypto2", 10.00 }
            };

        /// <summary>
        /// Angiver prisen for en enhed af en kryptovaluta. Prisen angives i dollars.
        /// Hvis der tidligere er angivet en værdi for samme kryptovaluta, 
        /// bliver den gamle værdi overskrevet af den nye værdi
        /// </summary>
        /// <param name="currencyName">Navnet på den kryptovaluta der angives</param>
        /// <param name="price">Prisen på en enhed af valutaen målt i dollars. Prisen kan ikke være negativ</param>
        public void SetPricePerUnit(String currencyName, double price)
        {
            if (!string.IsNullOrEmpty(currencyName) && price >= 0)
            {
                if (MyCryptoCurrencies.ContainsKey(currencyName))
                    MyCryptoCurrencies[currencyName] = price;
                else
                    MyCryptoCurrencies.Add(currencyName, price);
            }
        }

        /// <summary>
        /// Konverterer fra en kryptovaluta til en anden. 
        /// Hvis en af de angivne valutaer ikke findes, kaster funktionen en ArgumentException
        /// 
        /// </summary>
        /// <param name="fromCurrencyName">Navnet på den valuta, der konverteres fra</param>
        /// <param name="toCurrencyName">Navnet på den valuta, der konverteres til</param>
        /// <param name="amount">Beløbet angivet i valutaen angivet i fromCurrencyName</param>
        /// <returns>Værdien af beløbet i toCurrencyName</returns>
        public double Convert(String fromCurrencyName, String toCurrencyName, double amount)
        {
            if (string.IsNullOrEmpty(fromCurrencyName) || string.IsNullOrEmpty(toCurrencyName))
            {
                throw new ArgumentException();
            }

            var fromCrypto = (MyCryptoCurrencies.ContainsKey(fromCurrencyName)) ? MyCryptoCurrencies[fromCurrencyName] : 0;

            var toCrypto = (MyCryptoCurrencies.ContainsKey(toCurrencyName)) ? MyCryptoCurrencies[toCurrencyName] : 0;


            if (!MyCryptoCurrencies.ContainsKey(fromCurrencyName) || !MyCryptoCurrencies.ContainsKey(toCurrencyName) || fromCrypto < 0 || toCrypto < 0)
                throw new ArgumentException();
            else if (toCrypto == 0 || fromCrypto == 0)
                return 0;

            // fromCurrency divideret med toCurrency
            var newAmount = MyCryptoCurrencies[fromCurrencyName] / MyCryptoCurrencies[toCurrencyName] * amount;

            return Math.Round(newAmount, 2);
        }

    }
}
