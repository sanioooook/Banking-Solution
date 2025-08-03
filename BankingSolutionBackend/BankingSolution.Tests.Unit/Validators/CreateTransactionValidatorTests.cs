namespace BankingSolution.Tests.Unit.Validators;

using Application.Commands.CreateTransaction;

public class CreateTransactionValidatorTests
{
    private readonly CreateTransactionValidator _validator = new();

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var command = new CreateTransactionCommand { ToAccountId = 1, Amount = 100 };
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(null)]
    public void Should_Fail_When_ToAccountId_Invalid(int? id)
    {
        var command = new CreateTransactionCommand { ToAccountId = id ?? 0, Amount = 100 };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ToAccountId");
    }

    [Theory]
    [InlineData(0)]
    public void Should_Fail_When_Amount_Is_Zero(decimal amount)
    {
        var command = new CreateTransactionCommand { ToAccountId = 1, Amount = amount };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Amount");
    }
}
