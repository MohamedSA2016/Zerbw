using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : MasterDetailPage
	{
        private Users user;
		public MenuPage (Users userResponse)
		{
			InitializeComponent ();

            user = userResponse;
            BindingContext = user;
            NavigationPage.SetHasNavigationBar(this, false);

            Detail = new NavigationPage(new Dashboard())
            {
                BarBackgroundColor = Color.FromHex("#894183")
            };

            IsPresented = false;
            var items = new List<MenuItemNav>()
            {
                new MenuItemNav
                {
                     itemName = "Dashboard",
                     itemIcon = "",

                     
                },
                new MenuItemNav
                {
                    itemName ="Profile",
                    itemIcon = ""
                   
                },

               new MenuItemNav
                {
                    itemName = "Routes",
                    itemIcon = "paw.png"
                },
                new MenuItemNav
                {
                    itemName = "Reservations",
                    itemIcon = "logout.png"
                }   
            };
            listView.ItemsSource = items;

        }
        private void Button_OnClicked(object sender, EventArgs e)
        {

            Detail = new NavigationPage(new Dashboard());
            IsPresented = false;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var tappedItem = e.SelectedItem as MenuItemNav;
        
            if (tappedItem.itemName == "Dashboard")
            {

                Detail = new NavigationPage(new Dashboard())
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = true;
            }
            else if (tappedItem.itemName == "Profile")
            {
                Detail = new NavigationPage(new Profile())
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = true;
            }
            else if (tappedItem.itemName == "Routes")
            {
                Detail = new NavigationPage(new MyRoutes())
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = true;
            }
            else if (tappedItem.itemName == "Reservations")
            {
                Detail = new NavigationPage(new ReservationsView())
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = true;
            }
            listView.SelectedItem = null;
        }
    }
}