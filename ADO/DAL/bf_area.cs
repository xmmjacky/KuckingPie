using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace BookingFood.DAL
{
	/// <summary>
	/// 数据访问类:bf_area
	/// </summary>
	public partial class bf_area
	{
		public bf_area()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "bf_area"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from bf_area");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BookingFood.Model.bf_area model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into bf_area(");
			strSql.Append("Title,SortId,ParentId,ParentTitle,ManagerId,ManagerName,Description,IsShow,IsBusy,IsLock,Lat,Lng,DistributionArea,Address,DistributionArea_2,OppositeId,ChangeOrderArea,IsUnWelcome,BaiduCookie,MeituanCookie,ElemeCookie)");
			strSql.Append(" values (");
			strSql.Append("@Title,@SortId,@ParentId,@ParentTitle,@ManagerId,@ManagerName,@Description,@IsShow,@IsBusy,@IsLock,@Lat,@Lng,@DistributionArea,@Address,@DistributionArea_2,@OppositeId,@ChangeOrderArea,@IsUnWelcome,@BaiduCookie,@MeituanCookie,@ElemeCookie)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SortId", SqlDbType.Int,4),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@ParentTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@ManagerId", SqlDbType.Int,4),
					new SqlParameter("@ManagerName", SqlDbType.NVarChar,500),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@IsShow", SqlDbType.Int,4),
					new SqlParameter("@IsBusy", SqlDbType.Int,4),
					new SqlParameter("@IsLock", SqlDbType.Int,4),
					new SqlParameter("@Lat", SqlDbType.Decimal,9),
					new SqlParameter("@Lng", SqlDbType.Decimal,9),
					new SqlParameter("@DistributionArea", SqlDbType.NVarChar,500),
					new SqlParameter("@Address", SqlDbType.NVarChar,500),
					new SqlParameter("@DistributionArea_2", SqlDbType.NVarChar,500),
					new SqlParameter("@OppositeId", SqlDbType.Int,4),
					new SqlParameter("@ChangeOrderArea", SqlDbType.NVarChar,50),
					new SqlParameter("@IsUnWelcome", SqlDbType.Int,4),
					new SqlParameter("@BaiduCookie", SqlDbType.NText),
					new SqlParameter("@MeituanCookie", SqlDbType.NText),
					new SqlParameter("@ElemeCookie", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.Title;
			parameters[1].Value = model.SortId;
			parameters[2].Value = model.ParentId;
			parameters[3].Value = model.ParentTitle;
			parameters[4].Value = model.ManagerId;
			parameters[5].Value = model.ManagerName;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.IsShow;
			parameters[8].Value = model.IsBusy;
			parameters[9].Value = model.IsLock;
			parameters[10].Value = model.Lat;
			parameters[11].Value = model.Lng;
			parameters[12].Value = model.DistributionArea;
			parameters[13].Value = model.Address;
			parameters[14].Value = model.DistributionArea_2;
			parameters[15].Value = model.OppositeId;
			parameters[16].Value = model.ChangeOrderArea;
			parameters[17].Value = model.IsUnWelcome;
			parameters[18].Value = model.BaiduCookie;
			parameters[19].Value = model.MeituanCookie;
			parameters[20].Value = model.ElemeCookie;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BookingFood.Model.bf_area model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update bf_area set ");
			strSql.Append("Title=@Title,");
			strSql.Append("SortId=@SortId,");
			strSql.Append("ParentId=@ParentId,");
			strSql.Append("ParentTitle=@ParentTitle,");
			strSql.Append("ManagerId=@ManagerId,");
			strSql.Append("ManagerName=@ManagerName,");
			strSql.Append("Description=@Description,");
			strSql.Append("IsShow=@IsShow,");
			strSql.Append("IsBusy=@IsBusy,");
			strSql.Append("IsLock=@IsLock,");
			strSql.Append("Lat=@Lat,");
			strSql.Append("Lng=@Lng,");
			strSql.Append("DistributionArea=@DistributionArea,");
			strSql.Append("Address=@Address,");
			strSql.Append("DistributionArea_2=@DistributionArea_2,");
			strSql.Append("OppositeId=@OppositeId,");
			strSql.Append("ChangeOrderArea=@ChangeOrderArea,");
			strSql.Append("IsUnWelcome=@IsUnWelcome,");
			strSql.Append("BaiduCookie=@BaiduCookie,");
			strSql.Append("MeituanCookie=@MeituanCookie,");
			strSql.Append("ElemeCookie=@ElemeCookie");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SortId", SqlDbType.Int,4),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@ParentTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@ManagerId", SqlDbType.Int,4),
					new SqlParameter("@ManagerName", SqlDbType.NVarChar,500),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@IsShow", SqlDbType.Int,4),
					new SqlParameter("@IsBusy", SqlDbType.Int,4),
					new SqlParameter("@IsLock", SqlDbType.Int,4),
					new SqlParameter("@Lat", SqlDbType.Decimal,9),
					new SqlParameter("@Lng", SqlDbType.Decimal,9),
					new SqlParameter("@DistributionArea", SqlDbType.NVarChar,500),
					new SqlParameter("@Address", SqlDbType.NVarChar,500),
					new SqlParameter("@DistributionArea_2", SqlDbType.NVarChar,500),
					new SqlParameter("@OppositeId", SqlDbType.Int,4),
					new SqlParameter("@ChangeOrderArea", SqlDbType.NVarChar,50),
					new SqlParameter("@IsUnWelcome", SqlDbType.Int,4),
					new SqlParameter("@BaiduCookie", SqlDbType.NText),
					new SqlParameter("@MeituanCookie", SqlDbType.NText),
					new SqlParameter("@ElemeCookie", SqlDbType.NVarChar,500),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.Title;
			parameters[1].Value = model.SortId;
			parameters[2].Value = model.ParentId;
			parameters[3].Value = model.ParentTitle;
			parameters[4].Value = model.ManagerId;
			parameters[5].Value = model.ManagerName;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.IsShow;
			parameters[8].Value = model.IsBusy;
			parameters[9].Value = model.IsLock;
			parameters[10].Value = model.Lat;
			parameters[11].Value = model.Lng;
			parameters[12].Value = model.DistributionArea;
			parameters[13].Value = model.Address;
			parameters[14].Value = model.DistributionArea_2;
			parameters[15].Value = model.OppositeId;
			parameters[16].Value = model.ChangeOrderArea;
			parameters[17].Value = model.IsUnWelcome;
			parameters[18].Value = model.BaiduCookie;
			parameters[19].Value = model.MeituanCookie;
			parameters[20].Value = model.ElemeCookie;
			parameters[21].Value = model.Id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from bf_area ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from bf_area ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BookingFood.Model.bf_area GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,Title,SortId,ParentId,ParentTitle,ManagerId,ManagerName,Description,IsShow,IsBusy,IsLock,Lat,Lng,DistributionArea,Address,DistributionArea_2,OppositeId,ChangeOrderArea,IsUnWelcome,BaiduCookie,MeituanCookie,ElemeCookie from bf_area ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			BookingFood.Model.bf_area model=new BookingFood.Model.bf_area();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BookingFood.Model.bf_area DataRowToModel(DataRow row)
		{
			BookingFood.Model.bf_area model=new BookingFood.Model.bf_area();
			if (row != null)
			{
				if(row["Id"]!=null && row["Id"].ToString()!="")
				{
					model.Id=int.Parse(row["Id"].ToString());
				}
				if(row["Title"]!=null)
				{
					model.Title=row["Title"].ToString();
				}
				if(row["SortId"]!=null && row["SortId"].ToString()!="")
				{
					model.SortId=int.Parse(row["SortId"].ToString());
				}
				if(row["ParentId"]!=null && row["ParentId"].ToString()!="")
				{
					model.ParentId=int.Parse(row["ParentId"].ToString());
				}
				if(row["ParentTitle"]!=null)
				{
					model.ParentTitle=row["ParentTitle"].ToString();
				}
				if(row["ManagerId"]!=null && row["ManagerId"].ToString()!="")
				{
					model.ManagerId=int.Parse(row["ManagerId"].ToString());
				}
				if(row["ManagerName"]!=null)
				{
					model.ManagerName=row["ManagerName"].ToString();
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["IsShow"]!=null && row["IsShow"].ToString()!="")
				{
					model.IsShow=int.Parse(row["IsShow"].ToString());
				}
				if(row["IsBusy"]!=null && row["IsBusy"].ToString()!="")
				{
					model.IsBusy=int.Parse(row["IsBusy"].ToString());
				}
				if(row["IsLock"]!=null && row["IsLock"].ToString()!="")
				{
					model.IsLock=int.Parse(row["IsLock"].ToString());
				}
				if(row["Lat"]!=null && row["Lat"].ToString()!="")
				{
					model.Lat=decimal.Parse(row["Lat"].ToString());
				}
				if(row["Lng"]!=null && row["Lng"].ToString()!="")
				{
					model.Lng=decimal.Parse(row["Lng"].ToString());
				}
				if(row["DistributionArea"]!=null)
				{
					model.DistributionArea=row["DistributionArea"].ToString();
				}
				if(row["Address"]!=null)
				{
					model.Address=row["Address"].ToString();
				}
				if(row["DistributionArea_2"]!=null)
				{
					model.DistributionArea_2=row["DistributionArea_2"].ToString();
				}
				if(row["OppositeId"]!=null && row["OppositeId"].ToString()!="")
				{
					model.OppositeId=int.Parse(row["OppositeId"].ToString());
				}
				if(row["ChangeOrderArea"]!=null)
				{
					model.ChangeOrderArea=row["ChangeOrderArea"].ToString();
				}
				if(row["IsUnWelcome"]!=null && row["IsUnWelcome"].ToString()!="")
				{
					model.IsUnWelcome=int.Parse(row["IsUnWelcome"].ToString());
				}
				if(row["BaiduCookie"]!=null)
				{
					model.BaiduCookie=row["BaiduCookie"].ToString();
				}
				if(row["MeituanCookie"]!=null)
				{
					model.MeituanCookie=row["MeituanCookie"].ToString();
				}
				if(row["ElemeCookie"]!=null)
				{
					model.ElemeCookie=row["ElemeCookie"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,Title,SortId,ParentId,ParentTitle,ManagerId,ManagerName,Description,IsShow,IsBusy,IsLock,Lat,Lng,DistributionArea,Address,DistributionArea_2,OppositeId,ChangeOrderArea,IsUnWelcome,BaiduCookie,MeituanCookie,ElemeCookie ");
			strSql.Append(" FROM bf_area ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Id,Title,SortId,ParentId,ParentTitle,ManagerId,ManagerName,Description,IsShow,IsBusy,IsLock,Lat,Lng,DistributionArea,Address,DistributionArea_2,OppositeId,ChangeOrderArea,IsUnWelcome,BaiduCookie,MeituanCookie,ElemeCookie ");
			strSql.Append(" FROM bf_area ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM bf_area ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Id desc");
			}
			strSql.Append(")AS Row, T.*  from bf_area T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "bf_area";
			parameters[1].Value = "Id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

