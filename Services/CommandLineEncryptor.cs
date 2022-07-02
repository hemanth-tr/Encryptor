using Encryptor.Model;
using System.Diagnostics;

namespace Encryptor.Services
{
    internal class CommandLineEncryptor : IEncryptor
    {
        public CommandLineEncryptor(string fileName)
        {
            this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        private string FileName { get; set; }

        public string Encrypt(EncryptionType encryptionType, string data)
        {
            string command = $"";

            var startInfo = new ProcessStartInfo
            {
                FileName = this.FileName,
                Arguments = command,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var cmd = Process.Start(startInfo);
            if (cmd == null)
            {
                throw new Exception("Error locating exe file");
            }

            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();

            return output;
        }
    }
}
