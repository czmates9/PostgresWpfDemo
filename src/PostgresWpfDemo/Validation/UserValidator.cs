namespace PostgresWpfDemo.Validation
{
    public class UserValidator
    {
        public bool IsValidName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return name.Trim().Length >= 2;
        }
    }
}