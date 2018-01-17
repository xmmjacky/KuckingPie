using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_company:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_company
	{
		public bf_company()
		{}
		#region Model
		private int _id;
		private string _companyname;
		private string _address;
		private int _personcount=0;
		private string _telphone;
		private string _acceptname;
		private DateTime _addtime;
		private int _status;
		private int _requestuserid;
		private int _contactcompanyid;
		private DateTime? _completetime;
		private int _areaid=0;
		private string _areaname;
		private DateTime? _begintime;
		private DateTime? _endtime;
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
		public string CompanyName
		{
			set{ _companyname=value;}
			get{return _companyname;}
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
		public int PersonCount
		{
			set{ _personcount=value;}
			get{return _personcount;}
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
		public string AcceptName
		{
			set{ _acceptname=value;}
			get{return _acceptname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 0:申请,1:通过,2:已有进行合并,3:未通过
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RequestUserId
		{
			set{ _requestuserid=value;}
			get{return _requestuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ContactCompanyId
		{
			set{ _contactcompanyid=value;}
			get{return _contactcompanyid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CompleteTime
		{
			set{ _completetime=value;}
			get{return _completetime;}
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
		public string AreaName
		{
			set{ _areaname=value;}
			get{return _areaname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? BeginTime
		{
			set{ _begintime=value;}
			get{return _begintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

