namespace BankingSolution.Tests.Unit.Validators;

using Application.Commands.CreateAccount;

public class CreateAccountValidatorTests
{
    private readonly CreateAccountValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var command = new CreateAccountCommand { OwnerName = "John", InitialBalance = 100 };
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Fail_When_OwnerName_Is_Empty(string? name)
    {
        var command = new CreateAccountCommand { OwnerName = name!, InitialBalance = 100 };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "OwnerName");
    }

    [Fact]
    public void Should_Fail_When_InitialBalance_Is_Negative()
    {
        var command = new CreateAccountCommand { OwnerName = "Valid", InitialBalance = -1 };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InitialBalance");
    }
}