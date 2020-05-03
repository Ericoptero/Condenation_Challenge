using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodenationChallenge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try 
            {
                HttpClient client = new HttpClient();

                Console.Write("Write your token: ");
                string token = Console.ReadLine();

                // Realiza o GET do JSON de acordo com o token do candidato
                string responseJson = await client.GetStringAsync("https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=" + token);

                // Converte o JSON em objeto, decodifica o codigo e então o codifica para SHA1.
                Message message = JsonConvert.DeserializeObject<Message>(responseJson);
                message.Decoder();
                message.GetSha1();

                // Armazena o answer.json na pasta do executável.
                var path = @$"{Path.GetFullPath("answer.json")}";
                File.WriteAllText(path, JsonConvert.SerializeObject(message));

                // POST no formato multipart/formdata
                using (var form = new MultipartFormDataContent())
                {
                    // Faz uma stream do arquivo e o envia
                    Stream fileStream = new FileStream(path, FileMode.Open);
                    form.Add(new StreamContent(fileStream), "answer", "answer");


                    var post = await client.PostAsync("https://api.codenation.dev/v1/challenge/dev-ps/submit-solution?token=" + token, form);
                    var result = await post.Content.ReadAsStringAsync();

                    Console.WriteLine(result);
                }
            } 
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }


        }
    }
}
