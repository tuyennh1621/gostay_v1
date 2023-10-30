using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class RegulationHelper
    {
        public static string DeRegulate(this string txt)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
                aes.IV = iv;

                var transformer = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var ctream = new CryptoStream((Stream)memoryStream, transformer, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter((Stream)ctream))
                        {
                            streamWriter.Write(txt);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string Regulate(this string txt)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(txt);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
                aes.IV = iv;

                var transformer = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var ctream = new CryptoStream((Stream)memoryStream, transformer, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader((Stream)ctream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
