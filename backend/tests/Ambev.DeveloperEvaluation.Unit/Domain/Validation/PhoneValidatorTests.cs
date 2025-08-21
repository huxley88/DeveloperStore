using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{

    public class PhoneValidatorTests
    {
        [Theory(DisplayName = "Given a phone number When validating Then should validate according to regex pattern")]
        [InlineData("+123456789", true)]      
        [InlineData("123456789", true)]       
        [InlineData("+551199999999", true)]   
        [InlineData("11999999999", true)]     
        [InlineData("999999999", true)]      
        [InlineData("+0123456789", false)]   
        [InlineData("0123456789", false)]     
        [InlineData("+", false)]             
        [InlineData("+12345678901234567", false)] 
        [InlineData("12345678901234567", false)]  
        [InlineData("abc12345678", false)]   
        [InlineData("12.34567890", false)]   
        [InlineData("", false)]               
        public void Given_PhoneNumber_When_Validating_Then_ShouldValidateAccordingToPattern(string phone, bool expectedResult)
        {
            // Arrange
            var validator = new PhoneValidator();

            // Act
            var result = validator.Validate(phone);

            // Assert
            result.IsValid.Should().Be(expectedResult);
        }
    }
}
