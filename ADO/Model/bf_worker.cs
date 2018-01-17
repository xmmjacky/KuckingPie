using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_worker:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_worker
	{
		public bf_worker()
		{}
		#region Model
		private int _id;
		private string _title;
		private int _sortid;
		private string _telphone;
		private string _workertype;
		private int _operatetype=0;
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
		public string Telphone
		{
			set{ _telphone=value;}
			get{return _telphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkerType
		{
			set{ _workertype=value;}
			get{return _workertype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OperateType
		{
			set{ _operatetype=value;}
			get{return _operatetype;}
		}
		#endregion Model

	}
}

