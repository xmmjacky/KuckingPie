using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace BookingFood.DAL
{
	/// <summary>
	/// 数据访问类:bf_company
	/// </summary>
	public partial class bf_company
	{
		public bf_company()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "bf_company"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from bf_company");
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
		public int Add(BookingFood.Model.bf_company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into bf_company(");
			strSql.Append("CompanyName,Address,PersonCount,Telphone,AcceptName,AddTime,Status,RequestUserId,ContactCompanyId,CompleteTime,AreaId,AreaName,BeginTime,EndTime)");
			strSql.Append(" values (");
			strSql.Append("@CompanyName,@Address,@PersonCount,@Telphone,@AcceptName,@AddTime,@Status,@RequestUserId,@ContactCompanyId,@CompleteTime,@AreaId,@AreaName,@BeginTime,@EndTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@CompanyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,500),
					new SqlParameter("@PersonCount", SqlDbType.Int,4),
					new SqlParameter("@Telphone", SqlDbType.NVarChar,50),
					new SqlParameter("@AcceptName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@RequestUserId", SqlDbType.Int,4),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@CompleteTime", SqlDbType.DateTime),
					new SqlParameter("@AreaId", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,50),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
			parameters[0].Value = model.CompanyName;
			parameters[1].Value = model.Address;
			parameters[2].Value = model.PersonCount;
			parameters[3].Value = model.Telphone;
			parameters[4].Value = model.AcceptName;
			parameters[5].Value = model.AddTime;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.RequestUserId;
			parameters[8].Value = model.ContactCompanyId;
			parameters[9].Value = model.CompleteTime;
			parameters[10].Value = model.AreaId;
			parameters[11].Value = model.AreaName;
			parameters[12].Value = model.BeginTime;
			parameters[13].Value = model.EndTime;

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
		public bool Update(BookingFood.Model.bf_company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update bf_company set ");
			strSql.Append("CompanyName=@CompanyName,");
			strSql.Append("Address=@Address,");
			strSql.Append("PersonCount=@PersonCount,");
			strSql.Append("Telphone=@Telphone,");
			strSql.Append("AcceptName=@AcceptName,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("Status=@Status,");
			strSql.Append("RequestUserId=@RequestUserId,");
			strSql.Append("ContactCompanyId=@ContactCompanyId,");
			strSql.Append("CompleteTime=@CompleteTime,");
			strSql.Append("AreaId=@AreaId,");
			strSql.Append("AreaName=@AreaName,");
			strSql.Append("BeginTime=@BeginTime,");
			strSql.Append("EndTime=@EndTime");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@CompanyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,500),
					new SqlParameter("@PersonCount", SqlDbType.Int,4),
					new SqlParameter("@Telphone", SqlDbType.NVarChar,50),
					new SqlParameter("@AcceptName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@RequestUserId", SqlDbType.Int,4),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@CompleteTime", SqlDbType.DateTime),
					new SqlParameter("@AreaId", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,50),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.CompanyName;
			parameters[1].Value = model.Address;
			parameters[2].Value = model.PersonCount;
			parameters[3].Value = model.Telphone;
			parameters[4].Value = model.AcceptName;
			parameters[5].Value = model.AddTime;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.RequestUserId;
			parameters[8].Value = model.ContactCompanyId;
			parameters[9].Value = model.CompleteTime;
			parameters[10].Value = model.AreaId;
			parameters[11].Value = model.AreaName;
			parameters[12].Value = model.BeginTime;
			parameters[13].Value = model.EndTime;
			parameters[14].Value = model.Id;

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
			strSql.Append("delete from bf_company ");
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
			strSql.Append("delete from bf_company ");
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
		public BookingFood.Model.bf_company GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,CompanyName,Address,PersonCount,Telphone,AcceptName,AddTime,Status,RequestUserId,ContactCompanyId,CompleteTime,AreaId,AreaName,BeginTime,EndTime from bf_company ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			BookingFood.Model.bf_company model=new BookingFood.Model.bf_company();
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
		public BookingFood.Model.bf_company DataRowToModel(DataRow row)
		{
			BookingFood.Model.bf_company model=new BookingFood.Model.bf_company();
			if (row != null)
			{
				if(row["Id"]!=null && row["Id"].ToString()!="")
				{
					model.Id=int.Parse(row["Id"].ToString());
				}
				if(row["CompanyName"]!=null)
				{
					model.CompanyName=row["CompanyName"].ToString();
				}
				if(row["Address"]!=null)
				{
					model.Address=row["Address"].ToString();
				}
				if(row["PersonCount"]!=null && row["PersonCount"].ToString()!="")
				{
					model.PersonCount=int.Parse(row["PersonCount"].ToString());
				}
				if(row["Telphone"]!=null)
				{
					model.Telphone=row["Telphone"].ToString();
				}
				if(row["AcceptName"]!=null)
				{
					model.AcceptName=row["AcceptName"].ToString();
				}
				if(row["AddTime"]!=null && row["AddTime"].ToString()!="")
				{
					model.AddTime=DateTime.Parse(row["AddTime"].ToString());
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["RequestUserId"]!=null && row["RequestUserId"].ToString()!="")
				{
					model.RequestUserId=int.Parse(row["RequestUserId"].ToString());
				}
				if(row["ContactCompanyId"]!=null && row["ContactCompanyId"].ToString()!="")
				{
					model.ContactCompanyId=int.Parse(row["ContactCompanyId"].ToString());
				}
				if(row["CompleteTime"]!=null && row["CompleteTime"].ToString()!="")
				{
					model.CompleteTime=DateTime.Parse(row["CompleteTime"].ToString());
				}
				if(row["AreaId"]!=null && row["AreaId"].ToString()!="")
				{
					model.AreaId=int.Parse(row["AreaId"].ToString());
				}
				if(row["AreaName"]!=null)
				{
					model.AreaName=row["AreaName"].ToString();
				}
				if(row["BeginTime"]!=null && row["BeginTime"].ToString()!="")
				{
					model.BeginTime=DateTime.Parse(row["BeginTime"].ToString());
				}
				if(row["EndTime"]!=null && row["EndTime"].ToString()!="")
				{
					model.EndTime=DateTime.Parse(row["EndTime"].ToString());
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
			strSql.Append("select Id,CompanyName,Address,PersonCount,Telphone,AcceptName,AddTime,Status,RequestUserId,ContactCompanyId,CompleteTime,AreaId,AreaName,BeginTime,EndTime ");
			strSql.Append(" FROM bf_company ");
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
			strSql.Append(" Id,CompanyName,Address,PersonCount,Telphone,AcceptName,AddTime,Status,RequestUserId,ContactCompanyId,CompleteTime,AreaId,AreaName,BeginTime,EndTime ");
			strSql.Append(" FROM bf_company ");
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
			strSql.Append("select count(1) FROM bf_company ");
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
			strSql.Append(")AS Row, T.*  from bf_company T ");
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
			parameters[0].Value = "bf_company";
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

