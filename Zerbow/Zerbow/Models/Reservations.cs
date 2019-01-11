using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Zerbow.Models
{
  public  class Reservations
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "id_user")]
        public string ID_User { get; set; }

        [JsonProperty(PropertyName = "id_route")]
        public string ID_Route { get; set; }
        [JsonProperty(PropertyName = "id_owner")]
        public string Id_Owner { get; set; }
    }
}
