namespace PostgresWpfDemo.Validation;

public enum UserNameValidationResult
{
    Valid,
    NameRequired,
    NameTooShort
}

public sealed class UserValidator
{
    public UserNameValidationResult ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return UserNameValidationResult.NameRequired;

        var trimmedName = name.Trim();

        return trimmedName.Length < 2
            ? UserNameValidationResult.NameTooShort
            : UserNameValidationResult.Valid;
    }
}