using Encryptor.Model;
using System.Diagnostics;

namespace Encryptor.Services
{
    internal class CommandLineEncryptionDecryptionService : IEncryptor, IDecryptor
    {
        private const string DEFAULT_ENCRYPTION_MECHANISM = "GenericText_Internal";

        public CommandLineEncryptionDecryptionService(string fileName, EncryptionType encryptionType, string mechanism)
        {
            this.FileName = string.IsNullOrWhiteSpace(fileName) ? throw new ArgumentNullException(nameof(fileName)) : fileName;
            this.Mechanism = string.IsNullOrWhiteSpace(mechanism) ? DEFAULT_ENCRYPTION_MECHANISM : mechanism;
            this.EncryptionType = encryptionType;

        }

        private string FileName { get; set; }

        private EncryptionType EncryptionType { get; set; }

        private string Mechanism { get; set; }

        public string Decrypt(string data)
        {
            var result = this.ExecuteExe(data, toEncrypt: false);
            return result;
        }

        public string Encrypt(string data)
        {
            var result = this.ExecuteExe(data);
            return result;
        }

        private string ExecuteExe(string data, bool toEncrypt = true)
        {
            string command = $"";

            if (!toEncrypt)
            {
                command += " -d";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = this.FileName,
                Arguments = command,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            try
            {
                var cmd = Process.Start(startInfo);
                if (cmd == null)
                {
                    throw new Exception("Error executing exe");
                }

                string output = cmd.StandardOutput.ReadToEnd();
                cmd.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
