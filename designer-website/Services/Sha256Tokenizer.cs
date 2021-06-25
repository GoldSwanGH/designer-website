using System;
using System.Security.Cryptography;
using System.Text;
using designer_website.Interfaces;

namespace designer_website.Services
{
    public class Sha256Tokenizer : ITokenizer
    {
        public string GetRandomToken()
        {
            string numGen = (RandomNumberGenerator.GetInt32(Int32.MinValue, Int32.MaxValue)).ToString();
            return GetToken(numGen);
        }

        public string GetToken(string value)
        {
            var data = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            
            StringBuilder sBuilder = new StringBuilder();
            
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            
            return sBuilder.ToString();
        }
    }
}