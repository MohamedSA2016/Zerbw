using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Zerbow.Models
{
  public  class Users
    {

     
        public string ID { get; set; }
      
        public string Email { get; set; }
       
        public string Password { get; set; }
    
        public string Name { get; set; }
    
        public int Age { get; set; }
   
        public string Phone { get; set; }
   
        public string Photo { get; set; }
        public string ResourceName { get; set; }
  
    }
}
