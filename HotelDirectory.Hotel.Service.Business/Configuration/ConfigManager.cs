using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelDirectory.Hotel.Service.Business.Configuration
{
    public class ConfigManager
    {
        private IConfiguration _configuration { get; }

        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //public string HotelDbConnection = _configuration.Get("ConnectionStrings:HotelDbConnection");
    }
}
