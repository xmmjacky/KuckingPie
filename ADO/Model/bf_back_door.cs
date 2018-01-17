using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_back_door:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_back_door
	{
		public bf_back_door()
		{}
		#region Model
		private int _id;
		private int _orderid;
		private string _goodsname;
		private int _goodscount;
		private int _categoryid;
		private int _areaid;
		private int _isdown;
		private string _taste;
		private string _freight;
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
		public int OrderId
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsName
		{
			set{ _goodsname=value;}
			get{return _goodsname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsCount
		{
			set{ _goodscount=value;}
			get{return _goodscount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
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
		public int IsDown
		{
			set{ _isdown=value;}
			get{return _isdown;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Taste
		{
			set{ _taste=value;}
			get{return _taste;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Freight
		{
			set{ _freight=value;}
			get{return _freight;}
		}
		#endregion Model

	}
}

