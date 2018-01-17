using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace BookingFood.DAL
{
	/// <summary>
	/// 数据访问类:bf_old_order_offline
	/// </summary>
	public partial class bf_old_order_offline
	{
		public bf_old_order_offline()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "bf_old_order_offline"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from bf_old_order_offline");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(BookingFood.Model.bf_old_order_offline model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into bf_old_order_offline(");
			strSql.Append("id,name,email,tel,adress,ctry,totalPrice,orderdetail,msg,vipuserid,fromid,ischeck,sendtime,date,toDEmail,toget,suid,pstime,iid,num)");
			strSql.Append(" values (");
			strSql.Append("@id,@name,@email,@tel,@adress,@ctry,@totalPrice,@orderdetail,@msg,@vipuserid,@fromid,@ischeck,@sendtime,@date,@toDEmail,@toget,@suid,@pstime,@iid,@num)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@name", SqlDbType.NVarChar,500),
					new SqlParameter("@email", SqlDbType.NVarChar,50),
					new SqlParameter("@tel", SqlDbType.NVarChar,50),
					new SqlParameter("@adress", SqlDbType.NVarChar,500),
					new SqlParameter("@ctry", SqlDbType.NVarChar,50),
					new SqlParameter("@totalPrice", SqlDbType.NVarChar,50),
					new SqlParameter("@orderdetail", SqlDbType.NVarChar,2000),
					new SqlParameter("@msg", SqlDbType.NVarChar,500),
					new SqlParameter("@vipuserid", SqlDbType.NVarChar,50),
					new SqlParameter("@fromid", SqlDbType.Int,4),
					new SqlParameter("@ischeck", SqlDbType.Int,4),
					new SqlParameter("@sendtime", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@toDEmail", SqlDbType.NVarChar,50),
					new SqlParameter("@toget", SqlDbType.Int,4),
					new SqlParameter("@suid", SqlDbType.Int,4),
					new SqlParameter("@pstime", SqlDbType.NVarChar,50),
					new SqlParameter("@iid", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.name;
			parameters[2].Value = model.email;
			parameters[3].Value = model.tel;
			parameters[4].Value = model.adress;
			parameters[5].Value = model.ctry;
			parameters[6].Value = model.totalPrice;
			parameters[7].Value = model.orderdetail;
			parameters[8].Value = model.msg;
			parameters[9].Value = model.vipuserid;
			parameters[10].Value = model.fromid;
			parameters[11].Value = model.ischeck;
			parameters[12].Value = model.sendtime;
			parameters[13].Value = model.date;
			parameters[14].Value = model.toDEmail;
			parameters[15].Value = model.toget;
			parameters[16].Value = model.suid;
			parameters[17].Value = model.pstime;
			parameters[18].Value = model.iid;
			parameters[19].Value = model.num;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(BookingFood.Model.bf_old_order_offline model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update bf_old_order_offline set ");
			strSql.Append("name=@name,");
			strSql.Append("email=@email,");
			strSql.Append("tel=@tel,");
			strSql.Append("adress=@adress,");
			strSql.Append("ctry=@ctry,");
			strSql.Append("totalPrice=@totalPrice,");
			strSql.Append("orderdetail=@orderdetail,");
			strSql.Append("msg=@msg,");
			strSql.Append("vipuserid=@vipuserid,");
			strSql.Append("fromid=@fromid,");
			strSql.Append("ischeck=@ischeck,");
			strSql.Append("sendtime=@sendtime,");
			strSql.Append("date=@date,");
			strSql.Append("toDEmail=@toDEmail,");
			strSql.Append("toget=@toget,");
			strSql.Append("suid=@suid,");
			strSql.Append("pstime=@pstime,");
			strSql.Append("iid=@iid,");
			strSql.Append("num=@num");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,500),
					new SqlParameter("@email", SqlDbType.NVarChar,50),
					new SqlParameter("@tel", SqlDbType.NVarChar,50),
					new SqlParameter("@adress", SqlDbType.NVarChar,500),
					new SqlParameter("@ctry", SqlDbType.NVarChar,50),
					new SqlParameter("@totalPrice", SqlDbType.NVarChar,50),
					new SqlParameter("@orderdetail", SqlDbType.NVarChar,2000),
					new SqlParameter("@msg", SqlDbType.NVarChar,500),
					new SqlParameter("@vipuserid", SqlDbType.NVarChar,50),
					new SqlParameter("@fromid", SqlDbType.Int,4),
					new SqlParameter("@ischeck", SqlDbType.Int,4),
					new SqlParameter("@sendtime", SqlDbType.NVarChar,50),
					new SqlParameter("@date", SqlDbType.NVarChar,50),
					new SqlParameter("@toDEmail", SqlDbType.NVarChar,50),
					new SqlParameter("@toget", SqlDbType.Int,4),
					new SqlParameter("@suid", SqlDbType.Int,4),
					new SqlParameter("@pstime", SqlDbType.NVarChar,50),
					new SqlParameter("@iid", SqlDbType.NVarChar,50),
					new SqlParameter("@num", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.email;
			parameters[2].Value = model.tel;
			parameters[3].Value = model.adress;
			parameters[4].Value = model.ctry;
			parameters[5].Value = model.totalPrice;
			parameters[6].Value = model.orderdetail;
			parameters[7].Value = model.msg;
			parameters[8].Value = model.vipuserid;
			parameters[9].Value = model.fromid;
			parameters[10].Value = model.ischeck;
			parameters[11].Value = model.sendtime;
			parameters[12].Value = model.date;
			parameters[13].Value = model.toDEmail;
			parameters[14].Value = model.toget;
			parameters[15].Value = model.suid;
			parameters[16].Value = model.pstime;
			parameters[17].Value = model.iid;
			parameters[18].Value = model.num;
			parameters[19].Value = model.id;

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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from bf_old_order_offline ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)			};
			parameters[0].Value = id;

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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from bf_old_order_offline ");
			strSql.Append(" where id in ("+idlist + ")  ");
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
		public BookingFood.Model.bf_old_order_offline GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,name,email,tel,adress,ctry,totalPrice,orderdetail,msg,vipuserid,fromid,ischeck,sendtime,date,toDEmail,toget,suid,pstime,iid,num from bf_old_order_offline ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)			};
			parameters[0].Value = id;

			BookingFood.Model.bf_old_order_offline model=new BookingFood.Model.bf_old_order_offline();
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
		public BookingFood.Model.bf_old_order_offline DataRowToModel(DataRow row)
		{
			BookingFood.Model.bf_old_order_offline model=new BookingFood.Model.bf_old_order_offline();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["name"]!=null)
				{
					model.name=row["name"].ToString();
				}
				if(row["email"]!=null)
				{
					model.email=row["email"].ToString();
				}
				if(row["tel"]!=null)
				{
					model.tel=row["tel"].ToString();
				}
				if(row["adress"]!=null)
				{
					model.adress=row["adress"].ToString();
				}
				if(row["ctry"]!=null)
				{
					model.ctry=row["ctry"].ToString();
				}
				if(row["totalPrice"]!=null)
				{
					model.totalPrice=row["totalPrice"].ToString();
				}
				if(row["orderdetail"]!=null)
				{
					model.orderdetail=row["orderdetail"].ToString();
				}
				if(row["msg"]!=null)
				{
					model.msg=row["msg"].ToString();
				}
				if(row["vipuserid"]!=null)
				{
					model.vipuserid=row["vipuserid"].ToString();
				}
				if(row["fromid"]!=null && row["fromid"].ToString()!="")
				{
					model.fromid=int.Parse(row["fromid"].ToString());
				}
				if(row["ischeck"]!=null && row["ischeck"].ToString()!="")
				{
					model.ischeck=int.Parse(row["ischeck"].ToString());
				}
				if(row["sendtime"]!=null)
				{
					model.sendtime=row["sendtime"].ToString();
				}
				if(row["date"]!=null)
				{
					model.date=row["date"].ToString();
				}
				if(row["toDEmail"]!=null)
				{
					model.toDEmail=row["toDEmail"].ToString();
				}
				if(row["toget"]!=null && row["toget"].ToString()!="")
				{
					model.toget=int.Parse(row["toget"].ToString());
				}
				if(row["suid"]!=null && row["suid"].ToString()!="")
				{
					model.suid=int.Parse(row["suid"].ToString());
				}
				if(row["pstime"]!=null)
				{
					model.pstime=row["pstime"].ToString();
				}
				if(row["iid"]!=null)
				{
					model.iid=row["iid"].ToString();
				}
				if(row["num"]!=null)
				{
					model.num=row["num"].ToString();
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
			strSql.Append("select id,name,email,tel,adress,ctry,totalPrice,orderdetail,msg,vipuserid,fromid,ischeck,sendtime,date,toDEmail,toget,suid,pstime,iid,num ");
			strSql.Append(" FROM bf_old_order_offline ");
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
			strSql.Append(" id,name,email,tel,adress,ctry,totalPrice,orderdetail,msg,vipuserid,fromid,ischeck,sendtime,date,toDEmail,toget,suid,pstime,iid,num ");
			strSql.Append(" FROM bf_old_order_offline ");
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
			strSql.Append("select count(1) FROM bf_old_order_offline ");
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
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from bf_old_order_offline T ");
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
			parameters[0].Value = "bf_old_order_offline";
			parameters[1].Value = "id";
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

