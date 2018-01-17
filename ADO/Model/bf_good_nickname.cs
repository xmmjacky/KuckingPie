using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_good_nickname:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_good_nickname
	{
		public bf_good_nickname()
		{}
		#region Model
		private int _id;
		private string _title;
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
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		#endregion Model

	}
}

