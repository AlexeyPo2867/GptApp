using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Text;

namespace GptApp
{
    public partial class MainPage : ContentPage
    {

        string endpoint = "https://api.openai.com/v1/chat/completions";
        string apiKey = "";
        List<Message> messages = new List<Message>();
        HttpClient httpClient = new HttpClient();

        double width   = DeviceDisplay.MainDisplayInfo.Width;
        double height  = DeviceDisplay.MainDisplayInfo.Height;
        double density = DeviceDisplay.MainDisplayInfo.Density;

        Email email;
        string filename;
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public MainPage()
        {
            InitializeComponent();

            filename = "GPT_" + DateTime.UtcNow.ToString("MM-dd-yyyy") + ".txt";


            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            UrlEntry.Focus();
            UrlEntry.Text = "";

            ResponseEditor.Text = "" + width + "  " + height; // 800 1280
         //   ResponseEditor.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            ResponseEditor.FontSize = 14;
            UrlEntry.FontSize = 14;

            email = new Email();
        }

        private async void OnSendRequestClicked(object sender, EventArgs e)
        {
            // Получение URL-адреса из Entry
           
            string text =  UrlEntry.Text;

            ResponseEditor.Text = await GetRequest(text);

            UrlEntry.Text = String.Empty;
            UrlEntry.Focus();
        }

        private void OnSaveToFile(object sender, EventArgs e)
        {
            // Сохранение запросов в файл 
            File.AppendAllText(Path.Combine(folderPath, filename), ResponseEditor.Text+"\r\n");
            DisplayAlert("Сохранение в файл", "Ответ успешно сохранен", "OK");
        }
/*
        private async void  OnViewToFile(object sender, EventArgs e)
        {
            // Переход на страницу E-mail

            await Navigation.PushAsync(new Email());
        }
*/
        private async void OnViewToFile(object sender, EventArgs e)
        {
            // Просмотреть список сохраненных запросов

            await Navigation.PushAsync(new ViewFiles());

        }

        public async Task<string> GetRequest(string req)
        {

            Message message = new Message()
            {
                Role = "user",
                Content = req
            };

            // добавляем сообщение в список сообщений
            messages.Add(message);

            // формируем отправляемые данные
            var requestData = new Request()
            {
                ModelId = "gpt-3.5-turbo",
                Messages = messages
            };

            // отправляем запрос

            string json = 
            "{\"model\": \"gpt-3.5-turbo\", \"messages\": " +
            "[{ \"role\": \"user\", \"content\": " + "\"" + req + "\"" + "}]}";

           // json = $"{{\"model\": \"gpt - 3.5 - turbo\",\"messages\": [{{ \"role\": \"user\", \"content\": \"{req}\"}}]}}}}";


            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return "Ошибка запроса \r\n" + response.Headers.ToString();
            }


            // MsgChatGprt.Add("Вопрос : ");
            // MsgChatGprt.Add(message.Content);

            // получаем данные ответа
            string responseData = null;
            try
            {
                responseData = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return "Ошибка ответа \r\n" + responseData;
            }

            string t = "\"content\":\"";
            int start = responseData.IndexOf(t);
            int end = responseData.IndexOf("\"},\"", start + t.Length + 1);
            string resp = responseData.Substring(start+ t.Length, end - start - t.Length);

            return resp.Replace('\n',' ').Replace('\r', ' ');

        }


        class Message
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = "";
            [JsonPropertyName("content")]
            public string Content { get; set; } = "";
        }

        class Request
        {
            [JsonPropertyName("model")]
            public string ModelId { get; set; } = "";
            [JsonPropertyName("messages")]
            public List<Message> Messages { get; set; } = new List<Message>();
        }

        class ResponseData
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = "";
            [JsonPropertyName("object")]
            public string Object { get; set; } = "";
            [JsonPropertyName("created")]
            public ulong Created { get; set; }
            [JsonPropertyName("choices")]
            public List<Choice> Choices { get; set; } = new List<Choice>();
            [JsonPropertyName("usage")]
            public Usage Usage { get; set; } = new Usage();
        }

        class Choice
        {
            [JsonPropertyName("index")]
            public int Index { get; set; }
            [JsonPropertyName("message")]
            public Message Message { get; set; } = new Message();
            [JsonPropertyName("finish_reason")]
            public string FinishReason { get; set; } = "";
        }

        class Usage
        {
            [JsonPropertyName("prompt_tokens")]
            public int PromptTokens { get; set; }
            [JsonPropertyName("completion_tokens")]
            public int CompletionTokens { get; set; }
            [JsonPropertyName("total_tokens")]
            public int TotalTokens { get; set; }
        }

        private void UrlEntry_Focused(object sender, FocusEventArgs e)
        {

        }

       
    }
}
