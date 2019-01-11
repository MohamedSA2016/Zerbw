using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zerbow.Models;

namespace Zerbow.Services
{
    public class AzureManager
    {
        public static AzureManager instance;
        private IMobileServiceTable<Users> userTable;
        private IMobileServiceTable<Routes> routetabe;
        private IMobileServiceTable<Reservations> reservationTable;
        private IMobileServiceTable<Cars> carsTable;

        private AzureManager()
        {
            AzureClient = new MobileServiceClient("https://zerbow.azurewebsites.net");
            userTable = AzureClient.GetTable<Users>();
            routetabe = AzureClient.GetTable<Routes>();
            reservationTable = AzureClient.GetTable<Reservations>();
            carsTable = AzureClient.GetTable<Cars>();



        }

        public MobileServiceClient AzureClient { get; }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }
                return instance;
            }
        }

        public async Task SaveReservationAsync(Reservations reservation)
        {
            if (reservation.Id == null)
            {
                await reservationTable.InsertAsync(reservation);
            }
            else
            {
                await reservationTable.UpdateAsync(reservation);
            }
        }

        public async Task SaveCarAsync(Cars car)
        {
            if (car.ID == null)
            {
                await carsTable.InsertAsync(car);
            }
            else
            {
                await carsTable.UpdateAsync(car);
            }
        }


        public async Task<List<Cars>> GetMyCarsAsync(Users user)
        {


            try
            {
                return await carsTable.Where(cars => cars.ID_User == user.ID).ToListAsync();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;

        }
        public async Task<Cars> GetCars(Expression<Func<Cars, bool>> linq)
        {
            try
            {
                List<Cars> carlist = await carsTable.Where(linq).Take(1).ToListAsync();
                return carlist.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }



        public async Task<List<Reservations>> GetReservationWhere(Expression<Func<Reservations, bool>> expression)
        {
            return await reservationTable.ToListAsync();


        }
        public async Task<Routes> GetRouteWhere(Expression<Func<Routes, bool>> linq)
        {
            try
            {
                List<Routes> routesList = await routetabe.Where(linq).Take(1).ToListAsync();
                return routesList.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }

        public async Task<List<Routes>> ListRoutesWhere(Expression<Func<Routes, bool>> linq)
        {
            return await routetabe.ToListAsync();

        }

        public async Task<List<Users>> GetUsersWhere(Expression<Func<Users, bool>> linq)
        {
            try
            {
                List<Users> newUser = await userTable.Where(linq).ToListAsync();
                return newUser;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }

        public async Task<Users> GetUserWhere(Expression<Func<Users, bool>> linq)
        {
            try
            {
                List<Users> newUser = await userTable.Where(linq).Take(1).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

            return null;
        }

        public async Task DeleteReservationAsync(Reservations reservation)
        {
            try
            {
                List<Reservations> reservationsList = await GetReservationWhere(res => res.ID_Route == reservation.ID_Route && res.ID_Route == reservation.ID_User);
                foreach (var res in reservationsList)
                {
                    await reservationTable.DeleteAsync(res);
                }

            }


            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

        }

        public async Task DeleteReservationsAsync(Reservations reservation)
        {
            try
            {
                List<Reservations> reservationsList = await GetReservationsWhere(res => res.ID_Route == reservation.ID_Route);
                foreach (var res in reservationsList)
                {
                    await reservationTable.DeleteAsync(res);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

        }

        public async Task<List<Reservations>> GetReservationsWhere(Expression<Func<Reservations, bool>> linq)
        {
            return await reservationTable.ToListAsync();
        }

        public async Task SaveRouteAsync(Routes route)
        {
            if (route.Id == null)
            {
                await routetabe.InsertAsync(route);
            }
            else
            {
                await routetabe.UpdateAsync(route);
            }



        }
        public async Task<Users> SaveGetUserAsync(Users user)
        {
            if (user.ID == null)
            {
                await userTable.InsertAsync(user);
            }
            else
            {
                await userTable.UpdateAsync(user);
            }

            try
            {
                List<Users> newUser = await userTable.Where(userSelect => userSelect.Email == user.Email).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

            return null;
        }

        public async Task<List<Users>> GetUsers()
        {
            return await userTable.ToListAsync();


        }
        public async Task AddUser(Users user)
        {
            await userTable.InsertAsync(user);
        }
        public async Task UpdateUser(Users user)
        {
            await userTable.UpdateAsync(user);
        }
        public async Task DeleteUser(Users user)
        {
            await this.userTable.DeleteAsync(user);
        }








    }
}
