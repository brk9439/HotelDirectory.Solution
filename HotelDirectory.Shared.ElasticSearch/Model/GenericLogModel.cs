using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Shared.ElasticSearch.Model
{
    public class GenericLogModel
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public Type Type { get; set; }
        public object Object { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public GenericLogModel()
        {
            Type = Type.Information;
            Date = DateTime.UtcNow;
            Controller = string.Empty;
            Method = string.Empty;
            Object = string.Empty;
            Message = string.Empty;
        }
    }

    public enum Type
    {
        Information = 100,
        Success = 200,
        Created = 201,
        Warning = 400,
        NotFound = 404,
        Error = 500
    }
}
