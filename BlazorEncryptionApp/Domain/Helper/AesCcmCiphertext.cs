using System.Security.Cryptography;

namespace BlazorEncryptionApp.Domain.Helper
{
    public class AesCcmCiphertext
    {
        public byte[] Nonce { get; }
        public byte[] Tag { get; }
        public byte[] CiphertextBytes { get; }

        /// <summary>Froms the byte array.</summary>
        /// <param name="data">The data.</param>
        /// <returns> Returns object initialized with extracted nonce, ciphertext, and tag. </returns>
        public static AesCcmCiphertext FromByteArray(byte[] data)
        {
            var dataBytes = data;
            return new AesCcmCiphertext(
                data.Take(AesCcm.NonceByteSizes.MaxSize).ToArray(),
                data[^AesCcm.TagByteSizes.MaxSize..],
                data[AesCcm.NonceByteSizes.MaxSize..^AesCcm.TagByteSizes.MaxSize]
            );
        }

        /// <summary>Initializes a new instance of the <see cref="AesCcmCiphertext" /> class.</summary>
        /// <param name="nonce">The nonce.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="ciphertextBytes">The ciphertext bytes.</param>
        public AesCcmCiphertext(byte[] nonce, byte[] tag, byte[] ciphertextBytes)
        {
            Nonce = nonce;
            Tag = tag;
            CiphertextBytes = ciphertextBytes;
        }

        /// <summary>Converts to byte.</summary>
        /// <returns> A byte array representing the concatenated nonce, ciphertext, and tag. </returns>
        public byte[] ToByte()
        {
            return Nonce.Concat(CiphertextBytes).Concat(Tag).ToArray();
        }
    }
}
