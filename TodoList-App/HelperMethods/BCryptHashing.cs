namespace TodoList_App.HelperMethods;

using BCrypt.Net;
public static class BCryptHashing
{
    public static string HashPassword(string passwordToHash)
    {
        string salt = GenerateSalt();

        return BCrypt.HashPassword(passwordToHash, salt);
    }

    public static string GenerateSalt(int workFactor = 10)
    {
        return BCrypt.GenerateSalt(workFactor);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new ArgumentException("Password and hashedPassword cannot be null or empty");
        }

        return BCrypt.Verify(password, hashedPassword);
    }

}
