using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Services;
using Zerbow.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;

namespace Zerbow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUser : ContentPage
    {
        private bool img;
        private string userUri;
        public RegisterUser()
        {
            InitializeComponent();
        }

        private async void Register(object sender, EventArgs e)
        {
           
            regbtn.IsEnabled = false;

            // Perform await function call to id
            Users newUser = new Users()
            {
                Name = firstname.Text,
                Email = email.Text.ToLower(),
                Password = password.Text,
                Photo = img == true ? userUri : "",
                Phone = phone.Text,
               
              
            };

            bool res = true;

            List<Users> users = await AzureManager.AzureManagerInstance.GetUsers();
            if (users.Where(s => s.Email == newUser.Email).Count() > 0)
            {   // User exists
                await DisplayAlert("User exists", "User with specified email exists", "OK");
               
                return;
            }

            if (newUser.Password == null || newUser.Name == null || newUser.Email == null)
            {
                await DisplayAlert("Incomplete Information", "Please complete the required* fields", "OK");
                res = false;
            }
            else if (newUser.Password.Length < 1 || newUser.Name.Length < 1 || newUser.Email.Length < 1)
            {
                await DisplayAlert("Incomplete Information", "Please complete the required* fields", "OK");
                res = false;
            }

           

            if (res)
            {
                await AzureManager.AzureManagerInstance.AddUser(newUser);
                await DisplayAlert("Success", "You have been registered to Fabrikam", "OK");
                await Navigation.PushAsync(new Login());
            }

            regbtn.IsEnabled = true;
          
        }

        private async void Gallary(object sender, EventArgs e)
        {
            regbtn.IsEnabled = false;
        

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null) return;

            string responseID = await ApiManager.ApiManagerInstance.GetPostedImageUri(file.GetStream());
            file.Dispose();
            // Show user
            img = true;
            userUri = "https://i.imgur.com/" + responseID + ".jpg";
     image.Source = new Uri(userUri);

        
            regbtn.IsEnabled = true;
        }
    }
}