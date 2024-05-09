using System.Collections;

namespace RBP.Services.Models
{
    public class BadRequestReturn
    {
        public string Exception { get; set; }
        public string Message { get; set; }
        public IDictionary Data { get; set; }
    }
}