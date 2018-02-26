using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watch.Models
{
    public class Response<T> : IResponse 
    {
        public Response()
        {
            Result = new PagedResult<T>();
        }

        public bool Success = true;
        public string Message;
        public PagedResult<T> Result;
    }
}