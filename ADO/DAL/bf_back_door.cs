using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace BookingFood.DAL
{
	/// <summary>
	/// 数据访问类:bf_back_door
	/// </summary>
	public partial class bf_back_door
	{
		public bf_back_door()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "bf_back_door"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from bf_back_door");
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
		public int Add(BookingFood.Model.bf_back_door model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into bf_back_door(");
			strSql.Append("OrderId,GoodsName,GoodsCount,CategoryId,AreaId,IsDown,Taste,Freight)");
			strSql.Append(" values (");
			strSql.Append("@OrderId,@GoodsName,@GoodsCount,@CategoryId,@AreaId,@IsDown,@Taste,@Freight)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@OrderId", SqlDbType.Int,4),
					new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
					new SqlParameter("@GoodsCount", SqlDbType.Int,4),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@AreaId", SqlDbType.Int,4),
					new SqlParameter("@IsDown", SqlDbType.Int,4),
					new SqlParameter("@Taste", SqlDbType.NVarChar,50),
					new SqlParameter("@Freight", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.OrderId;
			parameters[1].Value = model.GoodsName;
			parameters[2].Value = model.GoodsCount;
			parameters[3].Value = model.CategoryId;
			parameters[4].Value = model.AreaId;
			parameters[5].Value = model.IsDown;
			parameters[6].Value = model.Taste;
			parameters[7].Value = model.Freight;

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
		public bool Update(BookingFood.Model.bf_back_door model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update bf_back_door set ");
			strSql.Append("OrderId=@OrderId,");
			strSql.Append("GoodsName=@GoodsName,");
			strSql.Append("GoodsCount=@GoodsCount,");
			strSql.Append("CategoryId=@CategoryId,");
			strSql.Append("AreaId=@AreaId,");
			strSql.Append("IsDown=@IsDown,");
			strSql.Append("Taste=@Taste,");
			strSql.Append("Freight=@Freight");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@OrderId", SqlDbType.Int,4),
					new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
					new SqlParameter("@GoodsCount", SqlDbType.Int,4),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@AreaId", SqlDbType.Int,4),
					new SqlParameter("@IsDown", SqlDbType.Int,4),
					new SqlParameter("@Taste", SqlDbType.NVarChar,50),
					new SqlParameter("@Freight", SqlDbType.NVarChar,50),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.OrderId;
			parameters[1].Value = model.GoodsName;
			parameters[2].Value = model.GoodsCount;
			parameters[3].Value = model.CategoryId;
			parameters[4].Value = model.AreaId;
			parameters[5].Value = model.IsDown;
			parameters[6].Value = model.Taste;
			parameters[7].Value = model.Freight;
			parameters[8].Value = model.Id;

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
			strSql.Append("delete from bf_back_door ");
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
			strSql.Append("delete from bf_back_door ");
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
		public BookingFood.Model.bf_back_door GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,OrderId,GoodsName,GoodsCount,CategoryId,AreaId,IsDown,Taste,Freight from bf_back_door ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			BookingFood.Model.bf_back_door model=new BookingFood.Model.bf_back_door();
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
		public BookingFood.Model.bf_back_door DataRowToModel(DataRow row)
		{
			BookingFood.Model.bf_back_door model=new BookingFood.Model.bf_back_door();
			if (row != null)
			{
				if(row["Id"]!=null && row["Id"].ToString()!="")
				{
					model.Id=int.Parse(row["Id"].ToString());
				}
				if(row["OrderId"]!=null && row["OrderId"].ToString()!="")
				{
					model.OrderId=int.Parse(row["OrderId"].ToString());
				}
				if(row["GoodsName"]!=null)
				{
					model.GoodsName=row["GoodsName"].ToString();
				}
				if(row["GoodsCount"]!=null && row["GoodsCount"].ToString()!="")
				{
					model.GoodsCount=int.Parse(row["GoodsCount"].ToString());
				}
				if(row["CategoryId"]!=null && row["CategoryId"].ToString()!="")
				{
					model.CategoryId=int.Parse(row["CategoryId"].ToString());
				}
				if(row["AreaId"]!=null && row["AreaId"].ToString()!="")
				{
					model.AreaId=int.Parse(row["AreaId"].ToString());
				}
				if(row["IsDown"]!=null && row["IsDown"].ToString()!="")
				{
					model.IsDown=int.Parse(row["IsDown"].ToString());
				}
				if(row["Taste"]!=null)
				{
					model.Taste=row["Taste"].ToString();
				}
				if(row["Freight"]!=null)
				{
					model.Freight=row["Freight"].ToString();
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
			strSql.Append("select Id,OrderId,GoodsName,GoodsCount,CategoryId,AreaId,IsDown,Taste,Freight ");
			strSql.Append(" FROM bf_back_door ");
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
			strSql.Append(" Id,OrderId,GoodsName,GoodsCount,CategoryId,AreaId,IsDown,Taste,Freight ");
			strSql.Append(" FROM bf_back_door ");
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
			strSql.Append("select count(1) FROM bf_back_door ");
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
			strSql.Append(")AS Row, T.*  from bf_back_door T ");
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
			parameters[0].Value = "bf_back_door";
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

