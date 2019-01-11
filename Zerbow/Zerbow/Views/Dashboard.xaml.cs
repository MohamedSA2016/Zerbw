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
	public partial class Dashboard : ContentPage
	{
        private Users currentUser;
        private List<Routes> routesList;
        private List<Reservations> reservationsList;
        private ToolbarItem reservationsButton;

        private List<Users> userList;
        
        private IEnumerable<UserRoute> usersRoutes;
        public Dashboard ()
		{
			InitializeComponent ();
            currentUser = (Users)Application.Current.Properties["user"];
            userList = new List<Users>();

            routesList = new List<Routes>();

            reservationsList = new List<Reservations>();

            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));

            routesListView.ItemTapped += RoutesListView_ItemTapped;

            routesListView.Refreshing += RoutesListView_Refreshing;

            reservationsButton = new ToolbarItem
            {
                Text = "Reservations",
                Command = new Command(this.Reservations),
                Order = ToolbarItemOrder.Primary,
                Priority = 3
            };
        }

        private async void Reservations(object obj)
        {
            await Navigation.PushAsync(new ReservationsView());
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            searchBar.Text = "";
            if (ToolbarItems.Contains(reservationsButton))
            {
                ToolbarItems.Remove(reservationsButton);

            }
            LoadRouteList();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRouteList();

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if(reservationsList.Count >0)
            {
                ToolbarItems.Remove(reservationsButton);
            }
        }
        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            
          
          await Navigation.PushAsync(new RouteDetails(userRoute.IdRoute));

           
          
        }
        private async void LoadRouteList()
        {
            userList = await AzureManager.AzureManagerInstance.GetUsersWhere(user => user.ID != currentUser.ID);
            reservationsList = new List<Reservations>();
            routesListView.IsRefreshing = true;
            routesList = await AzureManager.AzureManagerInstance.ListRoutesWhere(route => route.Id_User != currentUser.ID && route.Depart_Date > DateTime.Now);
            reservationsList = await AzureManager.AzureManagerInstance.GetReservationsWhere(reservation => reservation.ID_User == currentUser.ID);

            foreach( var reservation in reservationsList)
            {
                routesList.Remove(routesList.Find(route => route.Id == reservation.ID_Route));

            }
            for(int i =0; i <routesList.Count; i++)
            {
                List<Reservations> rl = await AzureManager.AzureManagerInstance.GetReservationsWhere(reservation => reservation.ID_Route == routesList.ElementAt(i).Id);
                if (rl.Count == routesList.ElementAt(i).Capacity)
                {
                    routesList.RemoveAt(i);
                }

            }
            if(routesList.Count !=0)
            {
                routesListView.BackgroundColor = Color.White;
                errorLayout.IsVisible = false;

            }
            else
            {
                errorLabel.Text = "No Routes Available";
                errorLayout.IsVisible = true;
                routesListView.BackgroundColor = Color.FromHex("#009688");
            }

            usersRoutes = from r in routesList
                          join u in userList on r.Id_User equals u.ID
                          select new UserRoute { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To ,carModel = r.CarModel };

            routesListView.ItemsSource = usersRoutes;

            if(reservationsList.Count >0)
            {
                ToolbarItems.Add(reservationsButton);

            }
            routesListView.IsRefreshing = false;
        }

       

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                routesListView.ItemsSource =
                    routesList.Where(
                        route => route.From.ToLower().Contains(e.NewTextValue.ToLower()) || route.To.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            else
            {
                routesListView.ItemsSource = routesList;
            }
        }

        private  void LogOut(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
         
        }

        
    }
}