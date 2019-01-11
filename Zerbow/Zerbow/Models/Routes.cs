using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Zerbow.Models
{
  public class Routes

    {
    
        public string Id { get; set; }


        public string Id_User { get; set; }

        public string Id_Car { get; set; }

        public string From { get; set; }

        public string From_Longitude { get; set; }

        public string From_Latitude { get; set; }

        public string To { get; set; }

        public string To_Latitude { get; set; }

        public string To_Longitude { get; set; }

        public int Capacity { get; set; }

        public string Comments { get; set; }

        public string Depart_Time { get; set; }

        public DateTime Depart_Date { get; set; }
        public string CarModel { get; set; }


        public override string ToString()
        {
            return string.Format("[Routes: ID={1}, " +
                                 "ID_User={2}, " +
                                 "ID_Car={3}, " +
                                 "From={4}, " +
                                 "From_Longitude={5}," +
                                 "From_Latitude={6}, " +
                                 "To={7}," +
                                 "To_Latitude={8}," +
                                 "To_Longitude={9}," +
                                 "Capacity={10}," +
                                 "Comments={11}," +
                                 "depart_time={12}]",
                                 Id, Id_User, Id_Car, From, From_Longitude, From_Latitude, To, To_Latitude, To_Longitude, Capacity, Comments, Depart_Time);
        }

    }
}
