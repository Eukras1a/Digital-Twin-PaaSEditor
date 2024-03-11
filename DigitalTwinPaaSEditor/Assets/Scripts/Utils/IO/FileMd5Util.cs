using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utils.IO
{
    public static class FileMd5Util
    {
        public static string GetMd5(string filePath)
        {
            using var md5 = MD5.Create();
            byte[] hashBytes;
                
            try
            {
                using var stream = File.OpenRead(filePath);
                hashBytes = md5.ComputeHash(stream);
            }
            catch (Exception ex)
            {
                throw new Exception("happen a error in get md5!", ex);
            }
                
            var sb = new StringBuilder();
            foreach (var @byte in hashBytes)
            {
                sb.Append(@byte.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}