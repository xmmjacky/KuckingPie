using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_order_goods_combo_detail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_order_goods_combo_detail
	{
		public bf_order_goods_combo_detail()
		{}
		#region Model
		private int _id;
		private int _order_id;
		private int _order_goods_id;
		private int _goods_id;
		private string _goods_name;
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
		public int order_id
		{
			set{ _order_id=value;}
			get{return _order_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int order_goods_id
		{
			set{ _order_goods_id=value;}
			get{return _order_goods_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int goods_id
		{
			set{ _goods_id=value;}
			get{return _goods_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string goods_name
		{
			set{ _goods_name=value;}
			get{return _goods_name;}
		}
		#endregion Model

	}
}

