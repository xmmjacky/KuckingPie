using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_area_article:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_area_article
	{
		public bf_area_article()
		{}
		#region Model
		private int _id;
		private int _areaid;
		private int _articleid;
		private string _type;
		private int? _islock;
		private DateTime? _guqingdate;
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
		public int ArticleId
		{
			set{ _articleid=value;}
			get{return _articleid;}
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
		public int? IsLock
		{
			set{ _islock=value;}
			get{return _islock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? GuQingDate
		{
			set{ _guqingdate=value;}
			get{return _guqingdate;}
		}
		#endregion Model

	}
}

