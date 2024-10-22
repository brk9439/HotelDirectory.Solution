using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Shared.ElasticSearch.Model
{
    public class GenericLogModel
    {
        public string LogCustomId { get; set; } = null!;
        public string Controller { get; set; } = null!;
        public string Method { get; set; } = null!;
        public Type Type { get; set; } = Type.Information;
        public object Object { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
    public enum Type
    {
        Information = 100,
        Success = 200,
        Created = 201,
        Warning = 400,
        Error = 500
    }
}
