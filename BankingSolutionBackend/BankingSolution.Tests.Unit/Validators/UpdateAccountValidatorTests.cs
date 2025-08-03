namespace BankingSolution.Tests.Unit.Validators;

using Application.Commands.UpdateAccount;

public class UpdateAccountValidatorTests
{
    private readonly UpdateAccountValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var command = new UpdateAccountCommand { Id = 1, OwnerName = "John" };
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Zero()
    {
        var command = new UpdateAccountCommand { Id = 0, OwnerName = "John" };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Id");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Fail_When_OwnerName_Is_Invalid(string? name)
    {
        var command = new UpdateAccountCommand { Id = 1, OwnerName = name! };
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "OwnerName");
    }
}