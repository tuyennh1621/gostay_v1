using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Air
{
    public class RequestData<T>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public T Data { get; set; }
    }
    public class ResultData<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
}
