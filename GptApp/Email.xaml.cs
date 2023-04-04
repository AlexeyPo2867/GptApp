using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GptApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Email : ContentPage
    {
        public Email()
        {
            InitializeComponent();
          
          //  ShowMsg("class Email");
        }

        public void ShowMsg(string msg)
        {

            DisplayAlert("Сообщение", $"{msg}", "Ok");
        }

        private async void OnSendToEmail(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new Email());
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}