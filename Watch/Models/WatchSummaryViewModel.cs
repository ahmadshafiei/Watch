using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watch.Models
{
    public class WatchSummaryViewModel
    {
        public int Id;
        public string Name;
        public decimal Price;
        public string MainImagePath;

        public static explicit operator WatchSummaryViewModel(Watch model)
        {
            return model == null ? null : new WatchSummaryViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                MainImagePath = model.MainImagePath,
            };
        }
    }
}