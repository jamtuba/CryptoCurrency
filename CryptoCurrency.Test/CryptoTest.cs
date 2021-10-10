using System;
using Xunit;

namespace CryptoCurrency.Test
{
    public class CryptoTest
    {
        /// <summary>
        /// 
        /// Ækvivalensklasser for SetPricePerUnit:
        /// 
        /// Gyldige:
        /// price : 0.00, 1.00
        /// 
        /// cryptoname : "SomeName"
        /// 
        /// Opdatere en valutas værdi
        /// 
        /// Ugyldige:
        /// price : -1.00
        /// 
        /// cryptoname : "", null
        /// 
        /// 
        /// Ækvivalensklasser for Convert:
        /// 
        /// Gyldige: 
        /// 
        /// fromCurrency "SomeName1"
        /// 
        /// toCurrency "SomeName2"
        /// 
        /// amount : 0.00, 1.00
        /// 
        /// Ugyldige:
        /// 
        /// fromCurrency : "", null
        /// 
        /// toCurrency : "", null
        /// 
        /// amount : -1.00
        /// 
        /// </summary>


        // Test at SetPricePerUnit tilføjer en ny valuta uden at kaste en Exception af sig
        [Theory]
        [InlineData("TestCrypto3", 0.00)]
        [InlineData("TestCrypto4", 1.00)]
        [InlineData("", 1.00)]
        [InlineData(null, 1.00)]
        public void SetPricePerUnit_Add_New_Cryptocurrency(string cryptoName, double cryptoPrice)
        {
            // Arrange
            var sut = new Converter();
            
            // Act
            var exception = Record.Exception(() => sut.SetPricePerUnit(cryptoName, cryptoPrice));

            // Assert
            Assert.Null(exception);
        }


        // Test af negativt input i SetPricePerUnit
        [Fact]
        public void SetPricePerUnit_Negative_Price_Returns_ArgumentsException()
        {
            // Arrange
            var sut = new Converter();

            // Act
            sut.SetPricePerUnit("TestCrypto1", 2.00);
            sut.SetPricePerUnit("TestCrypto3", -1.00);

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Convert("TestCrypto1", "TestCrypto3", 1.00));
        }


        // Opdater en valuta
        [Fact]
        public void SetPricePerUnit_Update_A_Cryptocurrency()
        {
            // Arrange
            var sut = new Converter();

            // Act
            sut.SetPricePerUnit("Crypto1", 2.00);
            sut.SetPricePerUnit("Crypto1", 3.00);
            sut.SetPricePerUnit("Crypto2", 10.00);
            var result = sut.Convert("Crypto1", "Crypto2", 1.00);

            // Assert
            Assert.Equal(0.30, result);
        }


        // Test at Convert sender det rigtige beløb retur
        [Theory]
        [InlineData("TestCrypto1", 1.00, "TestCrypto2", 2.00, 1.00, 0.50)]
        [InlineData("TestCrypto1", 1.00, "TestCrypto2", 2.00, 0, 0)]
        [InlineData("TestCrypto1", 1.00, "TestCrypto2", 2.00, -1.00, -0.50)]
        public void Convert_Returns_Correct_Amount(string fromCurrencyName, double fromPrice, string toCurrencyName, double toPrice, double amount, double returnedAmount)
        {
            // Arrange
            var sut = new Converter();

            // Act
            sut.SetPricePerUnit(fromCurrencyName, fromPrice);
            sut.SetPricePerUnit(toCurrencyName, toPrice);
            var result = sut.Convert(fromCurrencyName, toCurrencyName, amount);

            // Assert
            Assert.Equal(returnedAmount, result);
        }


        // Test at Convert laver en ArgumentsException ved ukorrekt cryptonavn
        [Theory]
        [InlineData("TestCrypto1", "TestCrypto3", 1.00)]
        [InlineData("TestCrypto4", "TestCrypto2", 1.00)]
        [InlineData("", "TestCrypto2", 1.00)]
        [InlineData("TestCrypto1", "", 1.00)]
        [InlineData(null, "TestCrypto2", 1.00)]
        [InlineData("TestCrypto1", null, 1.00)]
        public void Convert_Throws_Arguments_Exception_By_Cryptoname_Not_Found(string fromCurrencyName, string toCurrencyName, double amount)
        {
            // Arrange
            var sut = new Converter();

            // Act
            sut.SetPricePerUnit("TestCrypto1", 1.00);
            sut.SetPricePerUnit("TestCrypto2", 2.00);

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Convert(fromCurrencyName, toCurrencyName, amount));
        }

    }
}
