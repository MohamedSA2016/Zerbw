using System;
using System.Collections.Generic;
using Plugin.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Zerbow.Models;
using Zerbow.Services;

namespace Zerbow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteDetails : ContentPage
    {
        private Users userRoute;
        private Routes route;
        private Users currentUser;
        private ToolbarItem callbtn;
        private  Cars carslist;
    
        public RouteDetails(string idRoute)
        {

           
            InitializeComponent();
          
            carslist = new Cars();
            currentUser = (Users)Application.Current.Properties["user"];
             
            LoadRouteData(idRoute);

            callbtn = new ToolbarItem
            {
                Text = "Call",
                Command = new Command(Callbt),
                Order = ToolbarItemOrder.Primary,
                Priority = 3,
                Icon = "icons8-phone-50.png"

            };

        }

        private void Callbt(object obj)
        {
            var PhoneCallTask = CrossMessaging.Current.PhoneDialer;
            if (PhoneCallTask.CanMakePhoneCall)
                PhoneCallTask.MakePhoneCall(userRoute.Phone);
        }

        private async void LoadRouteData(string idRoute)
        {
            this.IsBusy = true;



            carslist = await AzureManager.AzureManagerInstance.GetCars(cars => cars.ID == idRoute);
            route = await AzureManager.AzureManagerInstance.GetRouteWhere(route => route.Id == idRoute   );
            userRoute = new Users
            {
                ID = route.Id_User,
                ResourceName = carslist.Model
                 
                
            };
           


                this.LoadReservation();
            this.LoadData();
        }

        private async void LoadData()
        {
            userRoute = await AzureManager.AzureManagerInstance.GetUserWhere(userRoute => userRoute.ID == userRoute.ID  && userRoute.ResourceName == userRoute.ResourceName );

            nameLabel.Text = userRoute.Name;
            carinfo.Text =    carslist.Model;
   
            descriptionLabel.Text = route.Comments;
            departureLabel.Text = route.Depart_Date.ToString("dd/MMMM H:mm ") + "h";
            profileImage.Source =  userRoute.Photo;
            

            Reservations reservation = new Reservations
            {
                ID_Route = route.Id
            };

            List<Reservations> reservations = await AzureManager.AzureManagerInstance.GetReservationsWhere(reserv => reserv.ID_Route == reservation.ID_Route);

            //if user pay for use Appear Call Button
            if (reservations.Count > 0)
            {
                ToolbarItems.Add(callbtn);
            }
            else
            {
                ToolbarItems.Remove(callbtn);
            }


            if (route.Depart_Date.CompareTo(DateTime.Today.Add(DateTime.Now.TimeOfDay)) < 0)
            {
                reserveButton.IsEnabled = false;
            }
            else
            {
                reserveButton.IsEnabled = true;
            }


            seatsLabel.Text = "Seats Available: " + (route.Capacity + reservations.Count);

   




        }

        private async void LoadReservation()
        {
            string id_user = currentUser.ID;
            string id_route = route.Id;

            Reservations reservation = new Reservations
            {
                ID_User = id_user,
                ID_Route = id_route
            };

            List<Reservations> reservationResult = await AzureManager.AzureManagerInstance.GetReservationsWhere(reserv => reserv.ID_Route == reservation.ID_Route && reserv.ID_User == reservation.ID_User);

            if (reservationResult.Count != 0)
            {
                cancelButton.IsVisible = true;
                reserveButton.IsVisible = false;
                
            }
            else
            {
                reserveButton.IsVisible = true;
            }
          
           
            IsBusy = false;

         
        }

        private  void Like(object sender, EventArgs e)
        {

        }
        private  void UnLike(object sender, EventArgs e)
        {

        }
        private async void OnStartingPoint(object sender, EventArgs e)
        {
            var latitude = double.Parse(route.From_Latitude);
            var longitude = double.Parse(route.From_Longitude);

            var position = new Position(latitude, longitude);

            await Xamarin.Essentials.Map.OpenAsync(latitude, longitude, new MapLaunchOptions
            {
                Name = currentUser.Name,
                 
                NavigationMode = NavigationMode.Walking
            });
        }
        private async void OnEndingPoint(object sender, EventArgs e)
        {


            var latitude = double.Parse(route.To_Latitude);
            var longitude = double.Parse(route.To_Latitude);

            var position = new Position(latitude, longitude);
            await Xamarin.Essentials.Map.OpenAsync(latitude, longitude, new MapLaunchOptions
            {
                Name = currentUser.Name,

                NavigationMode = NavigationMode.Walking
            });
        }
        private async void OnReserved(object sender, EventArgs e)
        {
            bool r = await DisplayAlert("Reservation", "Do you want to Reserve with" + userRoute.Name, "Accept", "Cancel");
            if (r == true)
            {
                activityIndicator.IsRunning = true;

                var newReservation = new Reservations
                {
                    ID_Route = route.Id,
                    ID_User = currentUser.ID,
                    Id_Owner = route.Id_User
                };
               
                

                await AzureManager.AzureManagerInstance.SaveReservationAsync(newReservation);

                activityIndicator.IsRunning = false;
                cancelButton.IsVisible = true;
                reserveButton.IsVisible = false;
                LoadData();
            }
        }

        private async void OnCancelReservation(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Cancel Reservation", "Are you sure?", "Accept", "Cancel");
            if (answer)
            {
                var reservation = new Reservations { ID_Route = route.Id, ID_User = currentUser.ID, Id_Owner = route.Id_User };
                IsBusy = true;
                await AzureManager.AzureManagerInstance.DeleteReservationAsync(reservation);
                IsBusy = false;
                await DisplayAlert("Success", "Reservation Canceled", "Accept");
                await Navigation.PopAsync(true);
            }
        }

    }
}