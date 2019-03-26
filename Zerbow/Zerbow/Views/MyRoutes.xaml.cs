using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zerbow.Models;
using Zerbow.Services;

namespace Zerbow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyRoutes : ContentPage
	{
        private Users currentUser;
        private List<Routes> routesList;

        private List<Users> userList;
        private IEnumerable<UserRoute> usersRoutes;
        public MyRoutes ()
		{
            routesList = new List<Routes>();
           
            InitializeComponent ();

            currentUser = (Users)Application.Current.Properties["user"];

            userList = new List<Users>();
            userList.Add(currentUser);

            routesListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));
            routesListView.Refreshing += RoutesListView_Refreshing;
            routesListView.ItemTapped += RoutesListView_ItemTapped;
        }

        private async void RoutesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var userRoute = e.Item as UserRoute;
            
            await Navigation.PushAsync(new MyRoutesDetail(userRoute.IdRoute));
           
          
        }

        private void RoutesListView_Refreshing(object sender, EventArgs e)
        {
            searchBar.Text = "";
            LoadRoutes();
        }

        private async void LoadRoutes()
        {
            routesListView.IsRefreshing = true;

            routesList = await AzureManager.AzureManagerInstance.ListRoutesWhere(route => route.Id_User == currentUser.ID);

            errorLayout.Children.Clear();
            if (routesList.Count == 0)
            {
                errorLayout.Children.Add(new Label
                {
                    Text = "No routes available",
                    TextColor = Color.Black,
                    FontSize = 25,
                    HorizontalTextAlignment = TextAlignment.Center
                });
            }
            else
            {

                usersRoutes = from r in routesList
                              join u in userList on r.Id_User equals u.ID
                              select new UserRoute() { IdRoute = r.Id, Photo = u.Photo, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;
            }

            routesListView.IsRefreshing = false;
        }
        private async void AddRoute(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddRoute());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRoutes();
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
                                                     select new UserRoute() { IdRoute = r.Id, Photo = u.Photo, From = r.From, To = r.To };
                routesListView.ItemsSource = usersRoutes;

            }
        }

       
    }
}