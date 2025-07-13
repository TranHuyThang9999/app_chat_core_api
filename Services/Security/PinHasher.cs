namespace PaymentCoreServiceApi.Services.Security;

public class PinHasher: IPinHasher
{
    public string HashPin(string pin)
    {
        return BCrypt.Net.BCrypt.HashPassword(pin);
    }

    public bool VerifyPin(string pin, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(pin, hash);
    }
}