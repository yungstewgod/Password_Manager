using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace Saugumo_4_praktinis.Files
{
    class FailuEncryption
    {
        private static ManualResetEvent resetEvent = new ManualResetEvent(true);

        public static void EncryptCombo(string inputFile, string password = "afsdfyujklmhtefjhxdcvbyaqrtplkgh")
        {
            FileEncrypt(inputFile, password);
            File.Delete(inputFile);
        }

        public static void DecryptCombo(string inputFile, string password = "afsdfyujklmhtefjhxdcvbyaqrtplkgh")
        {
            FileDecrypt(inputFile, password);
            File.Delete(inputFile);
        }

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                    resetEvent.WaitOne();
                }
            }

            return data;
        }

        public static void FileEncrypt(string inputFile, string password)
        {
            if (!(File.Exists(inputFile)))
                throw new Exception("Failas neegzistuoja");
            byte[] salt = GenerateRandomSalt();
            FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            AES.Mode = CipherMode.CFB;

            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                    resetEvent.WaitOne();
                }

                fsIn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
        }

        public static bool FileDecrypt(string inputFile, string password)
        {
            if (!(File.Exists(inputFile)))
                throw new Exception("Failas neegzistuoja");

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(inputFile.Remove(inputFile.Length - 4), FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            bool failed = false;
            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                    resetEvent.WaitOne();
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                failed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                failed = true;
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }

            return failed;
        }

    }
}
