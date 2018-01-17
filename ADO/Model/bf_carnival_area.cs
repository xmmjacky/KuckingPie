using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_carnival_area:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_carnival_area
	{
		public bf_carnival_area()
		{}
		#region Model
		private int _id;
		private int _areaid;
		private int _carnivalid;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AreaId
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CarnivalId
		{
			set{ _carnivalid=value;}
			get{return _carnivalid;}
		}
		#endregion Model

	}
}

