using Encryptor.Model;

namespace Encryptor.Services
{
    public interface IDecryptor
    {
        string Decrypt(EncryptionType encryptionType, string data);
    }
}
