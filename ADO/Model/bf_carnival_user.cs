using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_carnival_user:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_carnival_user
	{
		public bf_carnival_user()
		{}
		#region Model
		private int _id;
		private int _userid;
		private string _openid;
		private int _carnivalid;
		private int _num;
		private int _areaid=0;
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
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Openid
		{
			set{ _openid=value;}
			get{return _openid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CarnivalId
		{
			set{ _carnivalid=value;}
			get{return _carnivalid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AreaId
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		#endregion Model

	}
}

