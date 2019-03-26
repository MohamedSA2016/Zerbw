using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Zerbow.Models;
using Zerbow.Services;

namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyRoutesDetail : ContentPage
	{

        private Routes route;
        private Users userRoute;
        private Users currentUser;
        private List<Reservations> reservationResult;
        private List<Users> usersList;
     


        public MyRoutesDetail (string routeId)
		{

            reservationResult = new List<Reservations>();
            usersList = new List<Users>();
            currentUser = (Users)Application.Current.Properties["user"];

            InitializeComponent();

            LoadRouteData(routeId);
        }

        private async void LoadRouteData(string routeId)
        {
            route = await AzureManager.AzureManagerInstance.GetRouteWhere(route => route.Id == routeId);

            userRoute = new Users
            {
                ID = this.route.Id_User
            };

            LoadData();
        }

        private async void LoadData()
        {
            if (route.Depart_Date.CompareTo(DateTime.Today.Add(DateTime.Now.TimeOfDay)) >= 0)
            {
                await LoadReservation();
            }
            else
            {
                activateButton.IsVisible = true;
                usersLabel.Text = "The time for this route passed";
            }
            userRoute = await AzureManager.AzureManagerInstance.GetUserWhere(userSelect => userSelect.ID == userRoute.ID);
            int seats = route.Capacity;
            nameLabel.Text = userRoute.Name;
            ageLabel.Text = "Age: " + userRoute.Age;
            phoneLabel.Text = "Phone: " + userRoute.Phone;
            descriptionLabel.Text = route.Comments;
            departureLabel.Text = "Departure: \n" + route.Depart_Date.ToString("dd/MMMM H:mm ") + "h";
            profileImage.Source = userRoute.Photo;
            

            if (reservationResult.Count != 0)
            {
                seats = route.Capacity - reservationResult.Count;
            }
            seatsLabel.Text = "Seats Available: " + seats + "/" + route.Capacity;
            this.IsBusy = false;
        }

        private async Task LoadReservation()
        {
            string id_user = currentUser.ID;
            string id_route = route.Id;

            Reservations reservation = new Reservations
            {
                ID_User = id_user,
                ID_Route = id_route
            };

            reservationResult = await AzureManager.AzureManagerInstance.GetReservationsWhere(res => res.ID_Route == reservation.ID_Route);

            if (reservationResult.Count != 0)
            {
                foreach (var res in reservationResult)
                {
                    usersList.Add(await AzureManager.AzureManagerInstance.GetUserWhere(user => user.ID == res.ID_User));
                }
                foreach (var user in usersList)
                {
                    usersLayout.Children.Add(new Label() { Text = user.Name });
                    usersLayout.Children.Add(new Label() { Text = user.Phone });
                }
            }
            else
            {
                usersLayout.Children.Add(new Label() { Text = "Any user reserved" });
            }
        }

        private async void OnActivate(object sender, EventArgs e)
        {
            var reservations = new Reservations
            {
               ID_Route  = route.Id
            };
            IsBusy = true;
            await AzureManager.AzureManagerInstance.DeleteReservationsAsync(reservations);
            usersLabel.Text = "";
            activateButton.IsVisible = false;
            IsBusy = false;


            if (DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time)).CompareTo(DateTime.Now) <= 0)
            {
                route.Depart_Date = DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time)).AddDays(1);
            }
            else
            {
                route.Depart_Date = DateTime.Today.Add(TimeSpan.Parse(route.Depart_Time));
            }

            await AzureManager.AzureManagerInstance.SaveRouteAsync(route);
            await DisplayAlert("Success", "Route activated", "Accept");

            LoadData();
        }
        private async  void OnStartingPoint(object sender, EventArgs e)
        {

            if (!double.TryParse(route.From_Latitude, out double latitude))
                return;
            if (!double.TryParse(route.From_Longitude, out double longtitude))
                return;

            await Xamarin.Essentials.Map.OpenAsync(latitude, longtitude, new MapLaunchOptions
            {
                Name = userRoute.Name,
                   
                NavigationMode = NavigationMode.Walking
            });
            //var latitude = double.Parse(route.From_Latitude);
            //var longitude = double.Parse(route.From_Longitude);

            //var position = new Position(latitude, longitude);

            //await Navigation.PushAsync(new MapStartingPoint(position));

        }
        private async   void OnEndingPoint(object sender, EventArgs e)
        {
            if (!double.TryParse(route.To_Latitude, out double latitude))
                return;
            if (!double.TryParse(route.To_Longitude, out double longtitude))
                return;

            await Xamarin.Essentials.Map.OpenAsync(latitude, longtitude, new MapLaunchOptions
            {
                Name = userRoute.Name,
                NavigationMode = NavigationMode.Walking
            });
        }

      
    }
}