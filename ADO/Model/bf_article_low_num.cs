using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_article_low_num:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_article_low_num
	{
		public bf_article_low_num()
		{}
		#region Model
		private int _id;
		private int _articleid;
		private int _lownum;
		private DateTime _begintime;
		private DateTime _endtime;
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
		public int ArticleId
		{
			set{ _articleid=value;}
			get{return _articleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LowNum
		{
			set{ _lownum=value;}
			get{return _lownum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

