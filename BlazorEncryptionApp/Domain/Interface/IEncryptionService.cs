namespace BlazorEncryptionApp.Domain.Interface
{
    public interface IEncryptionService
    {

        byte[] EncryptPassword(string plaintext);

        string DecryptPassword(byte[] ciphertext);
    }
}
