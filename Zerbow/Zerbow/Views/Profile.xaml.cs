using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zerbow.Models;

namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Profile : ContentPage
	{
   

        private Users currentUser;
        public Profile ()
		{
			InitializeComponent ();
            currentUser = (Users)Application.Current.Properties["user"];
            loadData();
        }

        private async void loadData()
        {
            currentUser = await AzureManager.AzureManagerInstance.GetUserWhere(currentUser => currentUser.ID == currentUser.ID);

         
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}