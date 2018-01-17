using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_good_combo_detail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_good_combo_detail
	{
		public bf_good_combo_detail()
		{}
		#region Model
		private int _id;
		private int _goodcomboid;
		private int _businessid;
		private string _businesstitle;
		private string _type;
		private int? _sortid;
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
		public int GoodComboId
		{
			set{ _goodcomboid=value;}
			get{return _goodcomboid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BusinessId
		{
			set{ _businessid=value;}
			get{return _businessid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BUsinessTitle
		{
			set{ _businesstitle=value;}
			get{return _businesstitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SortId
		{
			set{ _sortid=value;}
			get{return _sortid;}
		}
		#endregion Model

	}
}

