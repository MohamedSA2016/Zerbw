using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zerbow.Services;
namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCar : ContentPage
	{
        private Users currentUser;
       
        public AddCar ()
		{
			InitializeComponent ();
            currentUser = (Users)Application.Current.Properties["user"];
        }
         async Task AddNewCar(Cars car)
        {
            await AzureManager.AzureManagerInstance.SaveCarAsync(car);
        }
        public async void OnAdd(object sender, EventArgs e)
        {
            string model = modelEntry.Text;
            string color = colorEntry.Text;

            activityIndicator.IsRunning = true;

            var car = new Cars { Color = color, Model = model, ID_User = currentUser.ID };

            if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(color))
            {
                await AddNewCar(car);
                activityIndicator.IsRunning = false;

                await DisplayAlert("Success", "Car added successfully", "Accept");
                await Navigation.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Incorrect", "Model or Color fields cannot be empty, please insert a value.", "Close");
                activityIndicator.IsRunning = false;
            }
        }


    }
}