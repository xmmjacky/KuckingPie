using System;
using System.Data;
using System.Collections.Generic;
using Maticsoft.Common;
using BookingFood.Model;
namespace BookingFood.BLL
{
	/// <summary>
	/// bf_area
	/// </summary>
	public partial class bf_area
	{
		public DataTable GetListForList(int parent_id)
        {
            return dal.GetListForList(parent_id);
        }

        public DataSet GetListByPositionByTransform(int top, string lng, string lat, string strwhere)
        {
            return dal.GetListByPositionByTransform(top, lng, lat, strwhere);
        }
	}
}

