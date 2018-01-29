using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingFood.Model.Pay
{
   public class dt_user_confirm_top
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string OpneId { get; set; }

        public int? IsDeleted { get; set; }

        public int? AreaId { get; set; }

        public string AreaName { get; set; }

        public string NickName { get; set; }

        public int UserId { get; set; }

        public string AddAmount { get; set; }

        public int type { get; set; }
    }
}
