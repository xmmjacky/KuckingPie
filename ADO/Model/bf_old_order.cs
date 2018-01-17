using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_old_order:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_old_order
	{
		public bf_old_order()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _email;
		private string _tel;
		private string _adress;
		private string _ctry;
		private string _totalprice;
		private string _orderdetail;
		private string _msg;
		private string _vipuserid;
		private int? _fromid;
		private int? _ischeck;
		private string _sendtime;
		private string _date;
		private string _todemail;
		private int? _toget;
		private int? _suid;
		private string _pstime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string tel
		{
			set{ _tel=value;}
			get{return _tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string adress
		{
			set{ _adress=value;}
			get{return _adress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ctry
		{
			set{ _ctry=value;}
			get{return _ctry;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string totalPrice
		{
			set{ _totalprice=value;}
			get{return _totalprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string orderdetail
		{
			set{ _orderdetail=value;}
			get{return _orderdetail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string msg
		{
			set{ _msg=value;}
			get{return _msg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string vipuserid
		{
			set{ _vipuserid=value;}
			get{return _vipuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? fromid
		{
			set{ _fromid=value;}
			get{return _fromid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ischeck
		{
			set{ _ischeck=value;}
			get{return _ischeck;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sendtime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string toDEmail
		{
			set{ _todemail=value;}
			get{return _todemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? toget
		{
			set{ _toget=value;}
			get{return _toget;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? suid
		{
			set{ _suid=value;}
			get{return _suid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pstime
		{
			set{ _pstime=value;}
			get{return _pstime;}
		}
		#endregion Model

	}
}

