using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watch.Models
{
    public class PagedResult<T>
    {
        public List<T> Data;
        public int Count;
    }
}