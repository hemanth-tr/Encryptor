using Encryptor.Model;

namespace Encryptor.Services
{
    public interface IEncryptor
    {
        string Encrypt(string data);
    }
}
