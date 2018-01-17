using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_user_address:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_user_address
	{
		public bf_user_address()
		{}
		#region Model
		private int _id;
		private int _userid;
		private string _nickname;
		private string _address;
		private string _telphone;
		private int _areaid;
		private string _areatitle;
		private int _areatype=1;
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
		public string NickName
		{
			set{ _nickname=value;}
			get{return _nickname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Telphone
		{
			set{ _telphone=value;}
			get{return _telphone;}
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
		public string AreaTitle
		{
			set{ _areatitle=value;}
			get{return _areatitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AreaType
		{
			set{ _areatype=value;}
			get{return _areatype;}
		}
		#endregion Model

	}
}

