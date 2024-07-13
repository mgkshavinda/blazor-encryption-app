using BlazorEncryptionApp.Domain.Helper;
using BlazorEncryptionApp.Domain.Interface;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEncryptionApp.Domain.Service
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] encryptionKey;

        public EncryptionService(byte[] encryptionKey)
        {
            this.encryptionKey = encryptionKey;
        }

        public byte[] EncryptPassword(string plaintext)
        {
            // Initializes an AES-CCM instance with the provided encryption key.
            using var aes = new AesCcm(encryptionKey);
            var nonce = new byte[AesCcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var ciphertextBytes = new byte[plaintextBytes.Length];
            var tag = new byte[AesCcm.TagByteSizes.MaxSize];

            // Performs the encryption and computes the authentication tag.
            aes.Encrypt(nonce, plaintextBytes, ciphertextBytes, tag);
            return new AesCcmCiphertext(nonce, tag, ciphertextBytes).ToByte();
        }

        public string DecryptPassword(byte[] ciphertext)
        {
            // extracts the nonce, tag, and ciphertext from the input byte array. 
            var ccmCiphertext = AesCcmCiphertext.FromByteArray(ciphertext);
            using var aes = new AesCcm(encryptionKey);
            var plaintextBytes = new byte[ccmCiphertext.CiphertextBytes.Length];
            aes.Decrypt(ccmCiphertext.Nonce, ccmCiphertext.CiphertextBytes, ccmCiphertext.Tag, plaintextBytes);
            string plaintext = Encoding.UTF8.GetString(plaintextBytes);
            return plaintext;
        }
    }
}
