using System;
namespace BookingFood.Model
{
	/// <summary>
	/// bf_carnival_user_log:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bf_carnival_user_log
	{
		public bf_carnival_user_log()
		{}
		#region Model
		private int _id;
		private int _userid;
		private string _openid;
		private int _carnivalid;
		private int _orderid;
		private DateTime _addtime;
		private int _areaid=0;
		private int _ispayfortakeout=0;
		private int _isavaliable=0;
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
		public string OpenId
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
		public int OrderId
		{
			set{ _orderid=value;}
			get{return _orderid;}
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
		/// 
		/// </summary>
		public int AreaId
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		/// <summary>
		/// 堂吃外带订单是否已支付
		/// </summary>
		public int IsPayForTakeOut
		{
			set{ _ispayfortakeout=value;}
			get{return _ispayfortakeout;}
		}
		/// <summary>
		/// 是否参与返20元的就餐次数. 1是不参与
		/// </summary>
		public int IsAvaliable
		{
			set{ _isavaliable=value;}
			get{return _isavaliable;}
		}
		#endregion Model

	}
}

