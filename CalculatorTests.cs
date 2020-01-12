using System;
using Xunit;

namespace string_calculator
{
    public abstract class CalculatorTests
    {        
        private int Add(string numbers) => SUT.Add(numbers);
        protected abstract ICalculator SUT { get; }     

        [Fact]
        public void should_throw_argument_exception_when_null()
        {
            Assert.Throws<ArgumentNullException>(() => Add(null));
        }

        [Fact]
        public void should_return_0_when_string_empty()
        {
            Assert.Equal(0, Add(string.Empty));
        }

        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("12", 12)]
        [Theory]
        public void should_return_single_number(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }
        
        [InlineData("1,2", 3)]
        [InlineData("0,0", 0)]
        [InlineData("11,20", 31)]
        [InlineData("1,2,3", 6)]
        [InlineData("1,2\n3", 6)]
        [InlineData("1\n2,3", 6)]
        [Theory]
        public void should_add_numbers_between_commas_or_newlines(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }

        [InlineData("//-\n1-2", 3)]
        [InlineData("//+\n1+2", 3)]
        [InlineData("///\n1/2", 3)]
        [InlineData("//\\\n1\\2", 3)]
        [InlineData("//\\\n1\\2\n3", 6)]
        [InlineData("//\n\n1\n2", 3)]
        [Theory]
        public void should_support_different_delimiter(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }

        [InlineData("-1", "negatives not allowed: -1")]
        [InlineData("-1,-2", "negatives not allowed: -1, -2")]
        [Theory]
        public void should_throw_when_argument_negative(string numbers, string expected)
        {
            var exc = Assert.Throws<ArgumentException>(() => Add(numbers));
            Assert.Equal(expected, exc.Message);
        }

        [InlineData("1000", 1000)]
        [InlineData("1001", 0)]
        [InlineData("2, 1001", 2)]
        [Theory]
        public void should_ignore_numbers_bigger_than_1000(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }
        
        [InlineData("//[***]\n1***2***3", 6)]        
        [InlineData("//[ab]\n1ab2\n3", 6)]        
        [Theory]
        public void should_support_delimiters_longer_than_1_char(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }
        
        [InlineData("//[***][ab]\n1***2ab3", 6)]                
        [Theory]
        public void should_support_multiple_custom_delimiters(string numbers, int expected)
        {
            Assert.Equal(expected, Add(numbers));
        }

    }

    public class TddTests : CalculatorTests
    {
        protected override ICalculator SUT => new TddCalculator();
    }

    public class SpracheCalculatorTests : CalculatorTests
    {
        protected override ICalculator SUT => new ParserCombinatorCalculator();
    }
}
