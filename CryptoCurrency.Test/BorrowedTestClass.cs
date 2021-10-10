using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CryptoCurrency.Test
{
    public class BorrowedTestClass
    {

        [Fact]
        public void Test_SetPricePerUnit_NullKey_DoesNotFail()
        {
            //Arrange
            Converter converter = new Converter();

            //Act
            Exception exception = Record.Exception(() => converter.SetPricePerUnit(null, 1.00));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Test_Converter_EmptyCurrencyKeys_Throws_Exception()
        {
            //Arrange
            Converter converter = new Converter();

            //Act
            converter.SetPricePerUnit("EUR", 1.16);

            //Assert
            Assert.Throws<ArgumentException>(() => converter.Convert("", "EUR", 1.00));
            Assert.Throws<ArgumentException>(() => converter.Convert("EUR", "", 1.00));
        }

        [Fact]
        public void Test_Converter_NegativeCurrencyPriceDoesNotAddCurrency()
        {
            //Arrange
            Converter converter = new Converter();

            //Act
            converter.SetPricePerUnit("DKK", 0.16);
            converter.SetPricePerUnit("EUR", -1.16);

            //Assert
            Assert.Throws<ArgumentException>(() => converter.Convert("DKK", "EUR", 1.00));
        }

        [Fact]
        public void Test_Converter_AlreadyUsedKeyOverwritesValue()
        {
            //Arrange
            Converter converter = new Converter();

            //Act
            converter.SetPricePerUnit("EUR", 0.16);
            converter.SetPricePerUnit("EUR", 1.16);
            converter.SetPricePerUnit("DKK", 1.16);
            converter.SetPricePerUnit("DKK", 0.16);
            double result = converter.Convert("DKK", "EUR", 1.00);

            //Assert
            Assert.Equal<double>(0.14, result);
        }

        [Theory]
        [InlineData("DKK", 0.00, "EUR", 1.16, 1.00, 0.00)] // Empty fromPrice
        [InlineData("DKK", 0.16, "EUR", 0.00, 1.00, 0.00)] // Empty toPrice
        [InlineData("DKK", 0.16, "EUR", 1.16, 0.00, 0.00)] // Empty amount
        [InlineData("DKK", 0.16, "EUR", 1.16, -1.00, -0.14)] // DKK to EUR - negative value
        [InlineData("DKK", 0.16, "EUR", 1.16, 1.00, 0.14)] // DKK to EUR
        [InlineData("DKK", 0.16, "EUR", 1.16, 1000000.00, 137931.03)] // DKK to EUR - high value
        public void Test_ConverterValues(String fromCurrency, double fromPrice, String toCurrency, double toPrice, double fromAmount, double toAmount)
        {
            //Arrange
            Converter converter = new Converter();
            converter.SetPricePerUnit(fromCurrency, fromPrice);
            converter.SetPricePerUnit(toCurrency, toPrice);

            //Act
            double result = converter.Convert(fromCurrency, toCurrency, fromAmount);

            //Assert
            Assert.Equal<double>(Math.Round(toAmount, 2), result);
        }
    }
}
