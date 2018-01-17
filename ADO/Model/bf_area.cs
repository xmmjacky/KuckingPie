using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_area:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_area
	{
		public bf_area()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _sortid;
		private int _parentid=0;
		private string _parenttitle;
		private int? _managerid;
		private string _managername;
		private string _description;
		private int _isshow;
		private int? _isbusy;
		private int? _islock;
		private decimal _lat=0M;
		private decimal _lng=0M;
		private string _distributionarea;
		private string _address;
		private string _distributionarea_2;
		private int _oppositeid=0;
		private string _changeorderarea;
		private int _isunwelcome=0;
		private string _baiducookie;
		private string _meituancookie;
		private string _elemecookie;
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
		/// <summary>
		/// 
		/// </summary>
		public int SortId
		{
			set{ _sortid=value;}
			get{return _sortid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParentTitle
		{
			set{ _parenttitle=value;}
			get{return _parenttitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ManagerId
		{
			set{ _managerid=value;}
			get{return _managerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ManagerName
		{
			set{ _managername=value;}
			get{return _managername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsShow
		{
			set{ _isshow=value;}
			get{return _isshow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsBusy
		{
			set{ _isbusy=value;}
			get{return _isbusy;}
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
		public decimal Lat
		{
			set{ _lat=value;}
			get{return _lat;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Lng
		{
			set{ _lng=value;}
			get{return _lng;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DistributionArea
		{
			set{ _distributionarea=value;}
			get{return _distributionarea;}
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
		public string DistributionArea_2
		{
			set{ _distributionarea_2=value;}
			get{return _distributionarea_2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OppositeId
		{
			set{ _oppositeid=value;}
			get{return _oppositeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ChangeOrderArea
		{
			set{ _changeorderarea=value;}
			get{return _changeorderarea;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsUnWelcome
		{
			set{ _isunwelcome=value;}
			get{return _isunwelcome;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BaiduCookie
		{
			set{ _baiducookie=value;}
			get{return _baiducookie;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MeituanCookie
		{
			set{ _meituancookie=value;}
			get{return _meituancookie;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ElemeCookie
		{
			set{ _elemecookie=value;}
			get{return _elemecookie;}
		}
		#endregion Model

	}
}

