using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zerbow.Models;
using Zerbow.Services;

namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void SignIn(object sender, EventArgs e)
        {
            string email = this.email.Text;
            string password = this.password.Text;
            var user = new Users
            {
                Email = email,
                Password = password
            };
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                activityIndicator.IsRunning = true;

                Users userResponse = await AzureManager.AzureManagerInstance.GetUserWhere(userSelect => userSelect.Email == user.Email && userSelect.Password == user.Password);

                activityIndicator.IsRunning = false;

                if (userResponse != null && userResponse.Email.Equals(email, StringComparison.Ordinal) && userResponse.Password.Equals(password, StringComparison.Ordinal))
                {
                    Application.Current.Properties["user"] = userResponse;
                    Application.Current.MainPage = new NavigationPage(new MenuPage (userResponse));
                }
                else
                {
                    await DisplayAlert("Incorrect", "Your email or password is incorrect, please try again.", "Close");
                    this.email.Text = "";
                    this.email.Text = "";
                }
            }
            else
            {
                await DisplayAlert("Incorrect", "The fields Email or Password can't be empty, please insert valid values.", "Close");
                this.email.Text = "";
                this.password.Text = "";
            }

        }

        private async void SignUp(object sender, EventArgs e)
        {
            var signUpPage = new RegisterUser();
            await Navigation.PushAsync(signUpPage);
        }
    }
}