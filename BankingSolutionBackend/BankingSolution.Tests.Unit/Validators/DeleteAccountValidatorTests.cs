namespace BankingSolution.Tests.Unit.Validators;

using Application.Commands.DeleteAccount;

public class DeleteAccountValidatorTests
{
    private readonly DeleteAccountValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        var command = new DeleteAccountCommand(1);
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Fail_When_Id_Is_Invalid(int id)
    {
        var command = new DeleteAccountCommand(id);
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Id");
    }
}