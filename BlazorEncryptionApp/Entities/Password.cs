namespace BlazorEncryptionApp.Entities
{
    public class Password
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? PasswordValue { get; set; }
        public byte[] EncryptedPassword { get; set; }
    }
}
