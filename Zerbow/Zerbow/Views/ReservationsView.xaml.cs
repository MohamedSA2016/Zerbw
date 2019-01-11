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
	public partial class ReservationsView : ContentPage
	{

        private Users currentUser;
        private List<Routes> routesList;

        private List<Routes> routesReservations;

        private List<Reservations> reservationsList;

        private IEnumerable<UserRoute> usersRoutes;
        private List<Users> userList;
        public ReservationsView ()
		{
			InitializeComponent ();

            currentUser = (Users)Application.Current.Properties["user"];

            routesList = new List<Routes>();

            userList = new List<Users>();

            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));

            routesListView.ItemTapped += RoutesListView_ItemTapped;

            routesListView.Refreshing += RoutesListView_Refreshing;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRoutes();
        }

        private async void LoadRoutes()
        {

            routesReservations = new List<Routes>();

            userList = await AzureManager.AzureManagerInstance.GetUsersWhere(user => user.ID != currentUser.ID);

            routesList = await AzureManager.AzureManagerInstance.ListRoutesWhere(route => route.Id_User != currentUser.ID && route.Depart_Date > DateTime.Now);
            reservationsList = await AzureManager.AzureManagerInstance.GetReservationsWhere(reservation => reservation.ID_User == currentUser.ID);

            foreach (var reservation in reservationsList)
            {
                var route = routesList.Find(routes => routes.Id == reservation.ID_Route);
                if (route != null)
                {
                    routesReservations.Add(route);
                }
            }

            if (routesReservations.Count > 0)
            {
                errorLayout.IsVisible = false;
            }
            else
            {
                errorLayout.IsVisible = true;
                errorLabel.Text = "No reservations available";
                routesListView.BackgroundColor = Color.FromHex("#009688");
                await Navigation.PopAsync();
            }

            usersRoutes = from r in routesReservations
                          join u in userList on r.Id_User equals u.ID
                          select new UserRoute { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };

            routesListView.ItemsSource = usersRoutes;

            routesListView.IsRefreshing = false;
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            LoadRoutes();

        }

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            await Navigation.PushAsync(new RouteDetails(userRoute.IdRoute));
        }

        private void OnSearch(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                routesListView.ItemsSource = usersRoutes;
            }
            else
            {
                IEnumerable<UserRoute> usersRoutes = from r in routesList
                                                     join u in userList on r.Id_User equals u.ID
                                                     where (r.From.ToLower().Contains(e.NewTextValue.ToLower()) || r.To.ToLower().Contains(e.NewTextValue.ToLower()))
                                                     select new UserRoute() { IdRoute = r.Id, ResourceName = u.ResourceName, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;
            }
        }
    }
}