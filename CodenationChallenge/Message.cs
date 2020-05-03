using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CodenationChallenge
{
    class Message
    {
        [JsonProperty("numero_casas")]
        public int NumberOfShifts { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("cifrado")]
        public string Cipher { get; set; }
        [JsonProperty("decifrado")]
        public string Decipher { get; set; }
        [JsonProperty("resumo_criptografico")]
        public string Encrypted_Summary { get; set; }

        public Message()
        {
        }

        public Message(int numberOfShifts, string token, string cipher, string decipher, string encrypted_Summary)
        {
            NumberOfShifts = numberOfShifts;
            Token = token;
            Cipher = cipher;
            Decipher = decipher;
            Encrypted_Summary = encrypted_Summary;
        }

        public void Decoder()
        {
            StringBuilder decodedMessage = new StringBuilder();
            for (int letter = 0; letter < Cipher.Length; letter++)
            {
                // Verifica se a iteração é uma letra, se não for, ela simplesmente armazena o char.
                if (!char.IsLetter(Cipher[letter])) 
                    decodedMessage.Append(Cipher[letter]);
                else {
                    char converter = (char)(Cipher[letter] - NumberOfShifts);

                    if (converter > 'z')
                        //Subtrai 26 caso ocorra overflow.
                        converter = (char)(converter - 26);
                    else if (converter < 'a')
                        //Soma 26 caso ocorra underflow.
                        converter = (char)(converter + 26);
                    
                    decodedMessage.Append(converter);
                }
            }
            Decipher = decodedMessage.ToString();

        }

        public void GetSha1()
        {
            // Converte a string para um vetor de bytes e depois passamos pelo metodo de conversão SHA1.
            var data = Encoding.ASCII.GetBytes(Decipher);
            var hashData = new SHA1Managed().ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            foreach (var b in hashData)
            {
                // Converte cada byte em hexadeximal.
                sb.Append(b.ToString("x2"));
            }
            Encrypted_Summary = sb.ToString();
        }
    }
}
