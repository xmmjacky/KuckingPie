using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingFood.Model.Pay
{
   public class dt_user_top_up
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public decimal? Amount { get; set; }

        public int? AreaId { get; set; }

        public string AreaName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? IsDeleted { get; set; }

        public int? Stage { get; set; }
    }
}
