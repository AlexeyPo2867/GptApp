using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mail.Attachment;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GptApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ViewFiles : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string email = "shaman11801@yandex.ru";
        string fileemail;

        public ViewFiles()
        {
            InitializeComponent();

            textEditor.BackgroundColor = Color.Black;
            textEditor.FontSize= 14;
            textEditor.TextColor = Color.Cyan;

            fileNameEntry.FontSize = 14;
            fileNameEntry.TextColor = Color.Cyan;
            fileNameEntry.BackgroundColor = Color.Black;

            this.Appearing += (o, e) =>
            {
                base.OnAppearing();
                UpdateFileList();
            };
        }

/*
        // обновление списка файлов
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFileList();
        }
*/

        // сохранение текста в файл
/*
        async void Save(object sender, EventArgs args)
        {
            string filename = fileNameEntry.Text;
            if (String.IsNullOrEmpty(filename)) return;
            // если файл существует
            if (File.Exists(Path.Combine(folderPath, filename)))
            {
                // запрашиваем разрешение на перезапись
                bool isRewrited = await DisplayAlert("Подтверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");
                if (isRewrited == false) return;
            }
            // перезаписываем файл
            File.WriteAllText(Path.Combine(folderPath, filename), textEditor.Text);
            // обновляем список файлов
            UpdateFileList();
        }
*/

        void FileSelect(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem == null) return;
            // получаем выделенный элемент
            string filename = (string)args.SelectedItem;
            // загружем текст в текстовое поле
            textEditor.Text = File.ReadAllText(Path.Combine(folderPath, (string)args.SelectedItem));
            fileemail = Path.Combine(folderPath, (string)args.SelectedItem);
            // устанавливаем название файла
            fileNameEntry.Text = filename;
            // снимаем выделение
            filesList.SelectedItem = null;

        }


        void Delete(object sender, EventArgs args)
        {
            // получаем имя файла
            string filename = (string)((MenuItem)sender).BindingContext;
            // удаляем файл из списка
            File.Delete(Path.Combine(folderPath, filename));
            // обновляем список файлов
            UpdateFileList();
        }


        // обновление списка файлов
        void UpdateFileList()
        {
            // получаем все файлы
            filesList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
            // снимаем выделение
            filesList.SelectedItem = null;
        }

        private async void SendEmail(object sender, EventArgs e)
        {
           // fileNameEntry.Text = "shaman1801@yandex.ru";
            MailAddress from = new MailAddress("shaman1801@yandex.ru", "Алексей");
            MailAddress to = new MailAddress("shaman1801@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Письмо";
            //   m.Body = textEditor.Text; // "Письмо-тест 2 работы smtp-клиента";
          
            m.Attachments.Add(new Attachment(fileemail));
        //    await DisplayAlert("Ошибка", System.IO.Path.GetFileName(fileemail), "Ok");

            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("shaman1801@yandex.ru", "serafim2867");
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            try
            {
                await smtp.SendMailAsync(m);

            }catch(Exception ex)
            {
                await DisplayAlert("Ошибка", fileemail + "  " + ex.Message, "Ok");
                return;
            }
            await DisplayAlert("", "Письмо отправлено","Ok"); ;
        }
    }

}