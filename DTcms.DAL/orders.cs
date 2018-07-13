using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
	/// <summary>
	/// 商品订单
	/// </summary>
	public partial class orders
	{
		public orders()
		{}
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from dt_orders");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from dt_orders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
        }

		/// <summary>
		/// 增加一条数据,及其子表数据
		/// </summary>
		public int Add(Model.orders model)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into dt_orders(");
            strSql.Append("order_no,user_id,user_name,payment_id,distribution_id,status,payment_status,distribution_status,delivery_name,delivery_no,accept_name,post_code,telphone,mobile,address,message,payable_amount,real_amount,payable_freight,real_freight,payment_fee,order_amount,point,add_time,payment_time,confirm_time,distribution_time,complete_time,area_id,area_title,email,OrderType,worker_id,worker_name,restore_status,MpForHere,takeout,is_additional,additional_count,is_quick,is_less,is_download,which_mp,additional_id,before_change_area_id,IsCountFreeAvaliable,voucher_total,area_type,user_address_id,officelinetype)");
            strSql.Append(" values (");
            strSql.Append("@order_no,@user_id,@user_name,@payment_id,@distribution_id,@status,@payment_status,@distribution_status,@delivery_name,@delivery_no,@accept_name,@post_code,@telphone,@mobile,@address,@message,@payable_amount,@real_amount,@payable_freight,@real_freight,@payment_fee,@order_amount,@point,@add_time,@payment_time,@confirm_time,@distribution_time,@complete_time,@area_id,@area_title,@email,@OrderType,@worker_id,@worker_name,@restore_status,@MpForHere,@takeout,@is_additional,@additional_count,@is_quick,@is_less,@is_download,@which_mp,@additional_id,@before_change_area_id,@IsCountFreeAvaliable,@voucher_total,@area_type,@user_address_id,@officelinetype)");
			strSql.Append(";set @ReturnValue= @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@order_no", SqlDbType.NVarChar,100),
					new SqlParameter("@user_id", SqlDbType.Int,4),
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100),
					new SqlParameter("@payment_id", SqlDbType.Int,4),
					new SqlParameter("@distribution_id", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.TinyInt,1),
					new SqlParameter("@payment_status", SqlDbType.TinyInt,1),
					new SqlParameter("@distribution_status", SqlDbType.TinyInt,1),
                    new SqlParameter("@delivery_name", SqlDbType.NVarChar,100),
					new SqlParameter("@delivery_no", SqlDbType.NVarChar,100),
					new SqlParameter("@accept_name", SqlDbType.NVarChar,50),
					new SqlParameter("@post_code", SqlDbType.NVarChar,20),
					new SqlParameter("@telphone", SqlDbType.NVarChar,30),
					new SqlParameter("@mobile", SqlDbType.NVarChar,30),
					new SqlParameter("@address", SqlDbType.NVarChar,255),
					new SqlParameter("@message", SqlDbType.NVarChar,500),
					new SqlParameter("@payable_amount", SqlDbType.Decimal,5),
					new SqlParameter("@real_amount", SqlDbType.Decimal,5),
					new SqlParameter("@payable_freight", SqlDbType.Decimal,5),
					new SqlParameter("@real_freight", SqlDbType.Decimal,5),
					new SqlParameter("@payment_fee", SqlDbType.Decimal,5),
					new SqlParameter("@order_amount", SqlDbType.Decimal,5),
					new SqlParameter("@point", SqlDbType.Int,4),
					new SqlParameter("@add_time", SqlDbType.DateTime),
					new SqlParameter("@payment_time", SqlDbType.DateTime),
                    new SqlParameter("@confirm_time", SqlDbType.DateTime),
					new SqlParameter("@distribution_time", SqlDbType.DateTime),
					new SqlParameter("@complete_time", SqlDbType.DateTime),
                    new SqlParameter("@area_id", SqlDbType.Int,4),
                    new SqlParameter("@area_title", SqlDbType.NVarChar,500),
                    new SqlParameter("@email", SqlDbType.NVarChar,500),
                    new SqlParameter("@OrderType", SqlDbType.NVarChar,50),
                    new SqlParameter("@worker_id", SqlDbType.Int,4),
                    new SqlParameter("@worker_name", SqlDbType.NVarChar,50),
                    new SqlParameter("@restore_status", SqlDbType.TinyInt,1),
                    new SqlParameter("@MpForHere", SqlDbType.NVarChar,50),
                    new SqlParameter("@takeout", SqlDbType.Int),
                    new SqlParameter("@is_additional", SqlDbType.Int),
                    new SqlParameter("@additional_count", SqlDbType.Int),
                    new SqlParameter("@is_quick", SqlDbType.Int),
                    new SqlParameter("@is_less", SqlDbType.Int),
                    new SqlParameter("@is_download", SqlDbType.Int),
                    new SqlParameter("@which_mp", SqlDbType.NVarChar,50),
                    new SqlParameter("@additional_id", SqlDbType.NVarChar,100),
                    new SqlParameter("@before_change_area_id", SqlDbType.Int),
                    new SqlParameter("@IsCountFreeAvaliable", SqlDbType.Int),
                    new SqlParameter("@voucher_total", SqlDbType.Decimal,5),
                    new SqlParameter("@area_type", SqlDbType.Int),
                    new SqlParameter("@user_address_id", SqlDbType.Int),
                    new SqlParameter("@officelinetype", SqlDbType.Int),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)};
            parameters[0].Value = model.order_no;
            parameters[1].Value = model.user_id;
            parameters[2].Value = model.user_name;
            parameters[3].Value = model.payment_id;
            parameters[4].Value = model.distribution_id;
            parameters[5].Value = model.status;
            parameters[6].Value = model.payment_status;
            parameters[7].Value = model.distribution_status;
            parameters[8].Value = model.delivery_name;
            parameters[9].Value = model.delivery_no;
            parameters[10].Value = model.accept_name;
            parameters[11].Value = model.post_code;
            parameters[12].Value = model.telphone;
            parameters[13].Value = model.mobile;
            parameters[14].Value = model.address;
            parameters[15].Value = model.message;
            parameters[16].Value = model.payable_amount;
            parameters[17].Value = model.real_amount;
            parameters[18].Value = model.payable_freight;
            parameters[19].Value = model.real_freight;
            parameters[20].Value = model.payment_fee;
            parameters[21].Value = model.order_amount;
            parameters[22].Value = model.point;
            parameters[23].Value = model.add_time;
            parameters[24].Value = model.payment_time;
            parameters[25].Value = model.confirm_time;
            parameters[26].Value = model.distribution_time;
            parameters[27].Value = model.complete_time;
            parameters[28].Value = model.area_id;
            parameters[29].Value = model.area_title;
            parameters[30].Value = model.email;
            parameters[31].Value = model.OrderType;
            parameters[32].Value = model.worker_id;
            parameters[33].Value = model.worker_name;
            parameters[34].Value = model.restore_status;
            parameters[35].Value = model.MpForHere;
            parameters[36].Value = model.takeout;
            parameters[37].Value = model.is_additional;
            parameters[38].Value = model.additional_count;
            parameters[39].Value = model.is_quick;
            parameters[40].Value = model.is_less;
            parameters[41].Value = model.is_download;
            parameters[42].Value = model.which_mp;
            parameters[43].Value = model.additional_id;
            parameters[44].Value = model.before_change_area_id;
            parameters[45].Value = model.IsCountFreeAvaliable;
            parameters[46].Value = model.voucher_total;
            parameters[47].Value = model.area_type;
            parameters[48].Value = model.user_address_id;
            parameters[49].Value = model.officelinetype;
            parameters[50].Direction = ParameterDirection.Output;

			List<CommandInfo> sqllist = new List<CommandInfo>();
			CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
			sqllist.Add(cmd);

            //订单商品列表
            if (model.order_goods != null)
            {
                StringBuilder strSql2;
                foreach (Model.order_goods models in model.order_goods)
                {
                    strSql2 = new StringBuilder();
                    strSql2.Append("insert into dt_order_goods(");
                    strSql2.Append("order_id,goods_id,goods_name,goods_price,real_price,quantity,point,type,subgoodsid,category_title,condition_price)");
                    strSql2.Append(" values (");
                    strSql2.Append("@order_id,@goods_id,@goods_name,@goods_price,@real_price,@quantity,@point,@type,@subgoodsid,@category_title,@condition_price)");
                    SqlParameter[] parameters2 = {
						    new SqlParameter("@order_id", SqlDbType.Int,4),
						    new SqlParameter("@goods_id", SqlDbType.Int,4),
						    new SqlParameter("@goods_name", SqlDbType.NVarChar,100),
						    new SqlParameter("@goods_price", SqlDbType.Decimal,5),
						    new SqlParameter("@real_price", SqlDbType.Decimal,5),
						    new SqlParameter("@quantity", SqlDbType.Int,4),
						    new SqlParameter("@point", SqlDbType.Int,4),
                                                 new SqlParameter("@type", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@subgoodsid", SqlDbType.NVarChar,100),
                                                 new SqlParameter("@category_title", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@condition_price", SqlDbType.NVarChar,500)};
                    parameters2[0].Direction = ParameterDirection.InputOutput;
                    parameters2[1].Value = models.goods_id;
                    parameters2[2].Value = models.goods_name;
                    parameters2[3].Value = models.goods_price;
                    parameters2[4].Value = models.real_price;
                    parameters2[5].Value = models.quantity;
                    parameters2[6].Value = models.point;
                    parameters2[7].Value = models.type;
                    parameters2[8].Value = models.subgoodsid;
                    parameters2[9].Value = models.category_title;
                    parameters2[10].Value = models.condition_price;

                    cmd = new CommandInfo(strSql2.ToString(), parameters2);
                    sqllist.Add(cmd);
                }
            }
			DbHelperSQL.ExecuteSqlTranWithIndentity(sqllist);
            return (int)parameters[50].Value;
		}

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_orders set " + strValue);
            strSql.Append(" where id=" + id);
            Log.Info("更新订单信息========>" + strSql.ToString());
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public bool UpdateField(string order_no, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_orders set " + strValue);
            strSql.Append(" where order_no='" + order_no + "'");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rowsAffected > 0)
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
		public bool Update(Model.orders model)
		{
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
			            StringBuilder strSql=new StringBuilder();
                        strSql.Append("update dt_orders set ");
                        strSql.Append("order_no=@order_no,");
                        strSql.Append("user_id=@user_id,");
                        strSql.Append("user_name=@user_name,");
                        strSql.Append("payment_id=@payment_id,");
                        strSql.Append("distribution_id=@distribution_id,");
                        strSql.Append("status=@status,");
                        strSql.Append("payment_status=@payment_status,");
                        strSql.Append("distribution_status=@distribution_status,");
                        strSql.Append("delivery_name=@delivery_name,");
                        strSql.Append("delivery_no=@delivery_no,");
                        strSql.Append("accept_name=@accept_name,");
                        strSql.Append("post_code=@post_code,");
                        strSql.Append("telphone=@telphone,");
                        strSql.Append("mobile=@mobile,");
                        strSql.Append("address=@address,");
                        strSql.Append("message=@message,");
                        strSql.Append("payable_amount=@payable_amount,");
                        strSql.Append("real_amount=@real_amount,");
                        strSql.Append("payable_freight=@payable_freight,");
                        strSql.Append("real_freight=@real_freight,");
                        strSql.Append("payment_fee=@payment_fee,");
                        strSql.Append("order_amount=@order_amount,");
                        strSql.Append("point=@point,");
                        strSql.Append("add_time=@add_time,");
                        strSql.Append("payment_time=@payment_time,");
                        strSql.Append("confirm_time=@confirm_time,");
                        strSql.Append("distribution_time=@distribution_time,");
                        strSql.Append("complete_time=@complete_time,");
                        strSql.Append("area_id=@area_id,");
                        strSql.Append("area_title=@area_title,");
                        strSql.Append("email=@email,");
                        strSql.Append("OrderType=@OrderType,");
                        strSql.Append("worker_id=@worker_id,");
                        strSql.Append("worker_name=@worker_name,");
                        strSql.Append("restore_status=@restore_status,");
                        strSql.Append("MpForHere=@MpForHere,");
                        strSql.Append("takeout=@takeout,");
                        strSql.Append("is_additional=@is_additional,");
                        strSql.Append("additional_count=@additional_count,");
                        strSql.Append("is_quick=@is_quick,");
                        strSql.Append("is_less=@is_less,");
                        strSql.Append("is_download=@is_download,");
                        strSql.Append("which_mp=@which_mp,");
                        strSql.Append("additional_id=@additional_id,");
                        strSql.Append("before_change_area_id=@before_change_area_id,");
                        strSql.Append("IsCountFreeAvaliable=@IsCountFreeAvaliable,");
                        strSql.Append("voucher_total=@voucher_total,");
                        strSql.Append("area_type=@area_type,");
                        strSql.Append("user_address_id=@user_address_id");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
					            new SqlParameter("@order_no", SqlDbType.NVarChar,100),
					            new SqlParameter("@user_id", SqlDbType.Int,4),
                                new SqlParameter("@user_name", SqlDbType.NVarChar,100),
					            new SqlParameter("@payment_id", SqlDbType.Int,4),
					            new SqlParameter("@distribution_id", SqlDbType.Int,4),
					            new SqlParameter("@status", SqlDbType.TinyInt,1),
					            new SqlParameter("@payment_status", SqlDbType.TinyInt,1),
					            new SqlParameter("@distribution_status", SqlDbType.TinyInt,1),
                                new SqlParameter("@delivery_name", SqlDbType.NVarChar,100),
					            new SqlParameter("@delivery_no", SqlDbType.NVarChar,100),
					            new SqlParameter("@accept_name", SqlDbType.NVarChar,50),
					            new SqlParameter("@post_code", SqlDbType.NVarChar,20),
					            new SqlParameter("@telphone", SqlDbType.NVarChar,30),
					            new SqlParameter("@mobile", SqlDbType.NVarChar,30),
					            new SqlParameter("@address", SqlDbType.NVarChar,255),
					            new SqlParameter("@message", SqlDbType.NVarChar,500),
					            new SqlParameter("@payable_amount", SqlDbType.Decimal,5),
					            new SqlParameter("@real_amount", SqlDbType.Decimal,5),
					            new SqlParameter("@payable_freight", SqlDbType.Decimal,5),
					            new SqlParameter("@real_freight", SqlDbType.Decimal,5),
					            new SqlParameter("@payment_fee", SqlDbType.Decimal,5),
					            new SqlParameter("@order_amount", SqlDbType.Decimal,5),
					            new SqlParameter("@point", SqlDbType.Int,4),
					            new SqlParameter("@add_time", SqlDbType.DateTime),
					            new SqlParameter("@payment_time", SqlDbType.DateTime),
                                new SqlParameter("@confirm_time", SqlDbType.DateTime),
					            new SqlParameter("@distribution_time", SqlDbType.DateTime),
					            new SqlParameter("@complete_time", SqlDbType.DateTime),
                                new SqlParameter("@area_id", SqlDbType.Int,4),
                                new SqlParameter("@area_title", SqlDbType.NVarChar,500),
                                new SqlParameter("@email", SqlDbType.NVarChar,500),
                                new SqlParameter("@OrderType", SqlDbType.NVarChar,50),
                                new SqlParameter("@worker_id", SqlDbType.Int,4),
                                new SqlParameter("@worker_name", SqlDbType.NVarChar,50),
                                new SqlParameter("@restore_status", SqlDbType.TinyInt,1),
                                new SqlParameter("@MpForHere", SqlDbType.NVarChar,50),
                                new SqlParameter("@takeout", SqlDbType.TinyInt,1),
                                new SqlParameter("@is_additional", SqlDbType.TinyInt,1),
                                new SqlParameter("@additional_count", SqlDbType.TinyInt,1),
                                new SqlParameter("@is_quick", SqlDbType.TinyInt,1),
                                new SqlParameter("@is_less", SqlDbType.TinyInt,1),
                                new SqlParameter("@is_download", SqlDbType.TinyInt,1),
                                new SqlParameter("@which_mp", SqlDbType.NVarChar,50),
                                new SqlParameter("@additional_id", SqlDbType.NVarChar,100),
                                new SqlParameter("@before_change_area_id", SqlDbType.TinyInt,1),
                                new SqlParameter("@IsCountFreeAvaliable", SqlDbType.TinyInt,1),
                                new SqlParameter("@voucher_total", SqlDbType.Decimal,5),
                                new SqlParameter("@area_type", SqlDbType.TinyInt,1),
                                new SqlParameter("@user_address_id", SqlDbType.Int,4),
                                new SqlParameter("@id", SqlDbType.Int,4)};
                        parameters[0].Value = model.order_no;
                        parameters[1].Value = model.user_id;
                        parameters[2].Value = model.user_name;
                        parameters[3].Value = model.payment_id;
                        parameters[4].Value = model.distribution_id;
                        parameters[5].Value = model.status;
                        parameters[6].Value = model.payment_status;
                        parameters[7].Value = model.distribution_status;
                        parameters[8].Value = model.delivery_name;
                        parameters[9].Value = model.delivery_no;
                        parameters[10].Value = model.accept_name;
                        parameters[11].Value = model.post_code;
                        parameters[12].Value = model.telphone;
                        parameters[13].Value = model.mobile;
                        parameters[14].Value = model.address;
                        parameters[15].Value = model.message;
                        parameters[16].Value = model.payable_amount;
                        parameters[17].Value = model.real_amount;
                        parameters[18].Value = model.payable_freight;
                        parameters[19].Value = model.real_freight;
                        parameters[20].Value = model.payment_fee;
                        parameters[21].Value = model.order_amount;
                        parameters[22].Value = model.point;
                        parameters[23].Value = model.add_time;
                        parameters[24].Value = model.payment_time;
                        parameters[25].Value = model.confirm_time;
                        parameters[26].Value = model.distribution_time;
                        parameters[27].Value = model.complete_time;
                        parameters[28].Value = model.area_id;
                        parameters[29].Value = model.area_title;
                        parameters[30].Value = model.email;
                        parameters[31].Value = model.OrderType;
                        parameters[32].Value = model.worker_id;
                        parameters[33].Value = model.worker_name;
                        parameters[34].Value = model.restore_status;
                        parameters[35].Value = model.MpForHere;
                        parameters[36].Value = model.takeout;
                        parameters[37].Value = model.is_additional;
                        parameters[38].Value = model.additional_count;
                        parameters[39].Value = model.is_quick;
                        parameters[40].Value = model.is_less;
                        parameters[41].Value = model.is_download;
                        parameters[42].Value = model.which_mp;
                        parameters[43].Value = model.additional_id;
                        parameters[44].Value = model.before_change_area_id;
                        parameters[45].Value = model.IsCountFreeAvaliable;
                        parameters[46].Value = model.voucher_total;
                        parameters[47].Value = model.area_type;
                        parameters[48].Value = model.user_address_id;
                        parameters[49].Value = model.id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        //删除已删除的商品
                        DeleteGoodsList(conn, trans, model.order_goods, model.id);

                        //添加/修改商品
                        if (model.order_goods != null)
                        {
                            StringBuilder strSql2;
                            foreach (Model.order_goods models in model.order_goods)
                            {
                                strSql2 = new StringBuilder();
                                if (models.id > 0)
                                {
                                    strSql2.Append("update dt_order_goods set ");
                                    strSql2.Append("order_id=@order_id,");
                                    strSql2.Append("goods_id=@goods_id,");
                                    strSql2.Append("goods_name=@goods_name,");
                                    strSql2.Append("goods_price=@goods_price,");
                                    strSql2.Append("real_price=@real_price,");
                                    strSql2.Append("quantity=@quantity,");
                                    strSql2.Append("point=@point,");
                                    strSql2.Append("type=@type,");
                                    strSql2.Append("subgoodsid=@subgoodsid,");
                                    strSql2.Append("category_title=@category_title,");
                                    strSql2.Append("condition_price=@condition_price");
                                    strSql2.Append(" where id=@id");
                                    SqlParameter[] parameters2 = {
					                        new SqlParameter("@order_id", SqlDbType.Int,4),
					                        new SqlParameter("@goods_id", SqlDbType.Int,4),
					                        new SqlParameter("@goods_name", SqlDbType.NVarChar,100),
					                        new SqlParameter("@goods_price", SqlDbType.Decimal,5),
					                        new SqlParameter("@real_price", SqlDbType.Decimal,5),
					                        new SqlParameter("@quantity", SqlDbType.Int,4),
					                        new SqlParameter("@point", SqlDbType.Int,4),
                                            new SqlParameter("@type", SqlDbType.NVarChar,50),
                                            new SqlParameter("@subgoodsid", SqlDbType.NVarChar,500),
                                            new SqlParameter("@category_title", SqlDbType.NVarChar,50),
                                            new SqlParameter("@condition_price", SqlDbType.NVarChar,500),
					                        new SqlParameter("@id", SqlDbType.Int,4)};
                                    parameters2[0].Value = models.order_id;
                                    parameters2[1].Value = models.goods_id;
                                    parameters2[2].Value = models.goods_name;
                                    parameters2[3].Value = models.goods_price;
                                    parameters2[4].Value = models.real_price;
                                    parameters2[5].Value = models.quantity;
                                    parameters2[6].Value = models.point;
                                    parameters2[7].Value = models.type;
                                    parameters2[8].Value = models.subgoodsid;
                                    parameters2[9].Value = models.category_title;
                                    parameters2[10].Value = models.condition_price;
                                    parameters2[11].Value = models.id;
                                    DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                                else
                                {
                                    strSql2.Append("insert into dt_order_goods(");
                                    strSql2.Append("order_id,goods_id,goods_name,goods_price,real_price,quantity,point,type,subgoodsid,category_title,condition_price)");
                                    strSql2.Append(" values (");
                                    strSql2.Append("@order_id,@goods_id,@goods_name,@goods_price,@real_price,@quantity,@point,@type,@subgoodsid,@category_title,@condition_price)");
                                    SqlParameter[] parameters2 = {
					                        new SqlParameter("@order_id", SqlDbType.Int,4),
					                        new SqlParameter("@goods_id", SqlDbType.Int,4),
					                        new SqlParameter("@goods_name", SqlDbType.NVarChar,100),
					                        new SqlParameter("@goods_price", SqlDbType.Decimal,5),
					                        new SqlParameter("@real_price", SqlDbType.Decimal,5),
					                        new SqlParameter("@quantity", SqlDbType.Int,4),
					                        new SqlParameter("@point", SqlDbType.Int,4),
                                            new SqlParameter("@type", SqlDbType.NVarChar,50),
                                                                 new SqlParameter("@subgoodsid", SqlDbType.NVarChar,500),
                                                                 new SqlParameter("@category_title", SqlDbType.NVarChar,50),
                                                                 new SqlParameter("@condition_price", SqlDbType.NVarChar,500)};
                                    parameters2[0].Value = models.order_id;
                                    parameters2[1].Value = models.goods_id;
                                    parameters2[2].Value = models.goods_name;
                                    parameters2[3].Value = models.goods_price;
                                    parameters2[4].Value = models.real_price;
                                    parameters2[5].Value = models.quantity;
                                    parameters2[6].Value = models.point;
                                    parameters2[7].Value = models.type;
                                    parameters2[8].Value = models.subgoodsid;
                                    parameters2[9].Value = models.category_title;
                                    parameters2[10].Value = models.condition_price; 
                                    DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                }
                            }
                        }

                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
		}

		/// <summary>
		/// 删除一条数据，及子表所有相关数据
		/// </summary>
		public bool Delete(int id)
		{
			List<CommandInfo> sqllist = new List<CommandInfo>();
			StringBuilder strSql2=new StringBuilder();
			strSql2.Append("delete from dt_order_goods ");
			strSql2.Append(" where order_id=@order_id ");
			SqlParameter[] parameters2 = {
					new SqlParameter("@order_id", SqlDbType.Int,4)};
			parameters2[0].Value = id;

			CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2);
			sqllist.Add(cmd);
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from dt_orders ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			cmd = new CommandInfo(strSql.ToString(), parameters);
			sqllist.Add(cmd);
			int rowsAffected=DbHelperSQL.ExecuteSqlTran(sqllist);
			if (rowsAffected > 0)
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
		public Model.orders GetModel(int id)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 * from dt_orders ");
            strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			Model.orders model=new Model.orders();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				#region  父表信息
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["order_no"] != null && ds.Tables[0].Rows[0]["order_no"].ToString() != "")
                {
                    model.order_no = ds.Tables[0].Rows[0]["order_no"].ToString();
                }
                if (ds.Tables[0].Rows[0]["user_id"] != null && ds.Tables[0].Rows[0]["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_name"] != null && ds.Tables[0].Rows[0]["user_name"].ToString() != "")
                {
                    model.user_name = ds.Tables[0].Rows[0]["user_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["payment_id"] != null && ds.Tables[0].Rows[0]["payment_id"].ToString() != "")
                {
                    model.payment_id = int.Parse(ds.Tables[0].Rows[0]["payment_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_id"] != null && ds.Tables[0].Rows[0]["distribution_id"].ToString() != "")
                {
                    model.distribution_id = int.Parse(ds.Tables[0].Rows[0]["distribution_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["status"] != null && ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    model.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_status"] != null && ds.Tables[0].Rows[0]["payment_status"].ToString() != "")
                {
                    model.payment_status = int.Parse(ds.Tables[0].Rows[0]["payment_status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_status"] != null && ds.Tables[0].Rows[0]["distribution_status"].ToString() != "")
                {
                    model.distribution_status = int.Parse(ds.Tables[0].Rows[0]["distribution_status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["delivery_name"] != null && ds.Tables[0].Rows[0]["delivery_name"].ToString() != "")
                {
                    model.delivery_name = ds.Tables[0].Rows[0]["delivery_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["delivery_no"] != null && ds.Tables[0].Rows[0]["delivery_no"].ToString() != "")
                {
                    model.delivery_no = ds.Tables[0].Rows[0]["delivery_no"].ToString();
                }
                if (ds.Tables[0].Rows[0]["accept_name"] != null && ds.Tables[0].Rows[0]["accept_name"].ToString() != "")
                {
                    model.accept_name = ds.Tables[0].Rows[0]["accept_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["post_code"] != null && ds.Tables[0].Rows[0]["post_code"].ToString() != "")
                {
                    model.post_code = ds.Tables[0].Rows[0]["post_code"].ToString();
                }
                if (ds.Tables[0].Rows[0]["telphone"] != null && ds.Tables[0].Rows[0]["telphone"].ToString() != "")
                {
                    model.telphone = ds.Tables[0].Rows[0]["telphone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["mobile"] != null && ds.Tables[0].Rows[0]["mobile"].ToString() != "")
                {
                    model.mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["address"] != null && ds.Tables[0].Rows[0]["address"].ToString() != "")
                {
                    model.address = ds.Tables[0].Rows[0]["address"].ToString();
                }
                if (ds.Tables[0].Rows[0]["message"] != null && ds.Tables[0].Rows[0]["message"].ToString() != "")
                {
                    model.message = ds.Tables[0].Rows[0]["message"].ToString();
                }
                if (ds.Tables[0].Rows[0]["payable_amount"] != null && ds.Tables[0].Rows[0]["payable_amount"].ToString() != "")
                {
                    model.payable_amount = decimal.Parse(ds.Tables[0].Rows[0]["payable_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["real_amount"] != null && ds.Tables[0].Rows[0]["real_amount"].ToString() != "")
                {
                    model.real_amount = decimal.Parse(ds.Tables[0].Rows[0]["real_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payable_freight"] != null && ds.Tables[0].Rows[0]["payable_freight"].ToString() != "")
                {
                    model.payable_freight = decimal.Parse(ds.Tables[0].Rows[0]["payable_freight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["real_freight"] != null && ds.Tables[0].Rows[0]["real_freight"].ToString() != "")
                {
                    model.real_freight = decimal.Parse(ds.Tables[0].Rows[0]["real_freight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_fee"] != null && ds.Tables[0].Rows[0]["payment_fee"].ToString() != "")
                {
                    model.payment_fee = decimal.Parse(ds.Tables[0].Rows[0]["payment_fee"].ToString());
                }
                if (ds.Tables[0].Rows[0]["order_amount"] != null && ds.Tables[0].Rows[0]["order_amount"].ToString() != "")
                {
                    model.order_amount = decimal.Parse(ds.Tables[0].Rows[0]["order_amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["point"] != null && ds.Tables[0].Rows[0]["point"].ToString() != "")
                {
                    model.point = int.Parse(ds.Tables[0].Rows[0]["point"].ToString());
                }
                if (ds.Tables[0].Rows[0]["add_time"] != null && ds.Tables[0].Rows[0]["add_time"].ToString() != "")
                {
                    model.add_time = DateTime.Parse(ds.Tables[0].Rows[0]["add_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment_time"] != null && ds.Tables[0].Rows[0]["payment_time"].ToString() != "")
                {
                    model.payment_time = DateTime.Parse(ds.Tables[0].Rows[0]["payment_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["confirm_time"] != null && ds.Tables[0].Rows[0]["confirm_time"].ToString() != "")
                {
                    model.confirm_time = DateTime.Parse(ds.Tables[0].Rows[0]["confirm_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["distribution_time"] != null && ds.Tables[0].Rows[0]["distribution_time"].ToString() != "")
                {
                    model.distribution_time = DateTime.Parse(ds.Tables[0].Rows[0]["distribution_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["complete_time"] != null && ds.Tables[0].Rows[0]["complete_time"].ToString() != "")
                {
                    model.complete_time = DateTime.Parse(ds.Tables[0].Rows[0]["complete_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["area_id"] != null && ds.Tables[0].Rows[0]["area_id"].ToString() != "")
                {
                    model.area_id = int.Parse(ds.Tables[0].Rows[0]["area_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["area_title"] != null && ds.Tables[0].Rows[0]["area_title"].ToString() != "")
                {
                    model.area_title = ds.Tables[0].Rows[0]["area_title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["email"] != null && ds.Tables[0].Rows[0]["email"].ToString() != "")
                {
                    model.email = ds.Tables[0].Rows[0]["email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["OrderType"] != null && ds.Tables[0].Rows[0]["OrderType"].ToString() != "")
                {
                    model.OrderType = ds.Tables[0].Rows[0]["OrderType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["worker_id"] != null && ds.Tables[0].Rows[0]["worker_id"].ToString() != "")
                {
                    model.worker_id = int.Parse(ds.Tables[0].Rows[0]["worker_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["worker_name"] != null && ds.Tables[0].Rows[0]["worker_name"].ToString() != "")
                {
                    model.worker_name = ds.Tables[0].Rows[0]["worker_name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["restore_status"] != null && ds.Tables[0].Rows[0]["restore_status"].ToString() != "")
                {
                    model.restore_status = int.Parse(ds.Tables[0].Rows[0]["restore_status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MpForHere"] != null && ds.Tables[0].Rows[0]["MpForHere"].ToString() != "")
                {
                    model.MpForHere = ds.Tables[0].Rows[0]["MpForHere"].ToString();
                }
                if (ds.Tables[0].Rows[0]["takeout"] != null && ds.Tables[0].Rows[0]["takeout"].ToString() != "")
                {
                    model.takeout = int.Parse(ds.Tables[0].Rows[0]["takeout"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_additional"] != null && ds.Tables[0].Rows[0]["is_additional"].ToString() != "")
                {
                    model.is_additional = int.Parse(ds.Tables[0].Rows[0]["is_additional"].ToString());
                }
                if (ds.Tables[0].Rows[0]["additional_count"] != null && ds.Tables[0].Rows[0]["additional_count"].ToString() != "")
                {
                    model.additional_count = int.Parse(ds.Tables[0].Rows[0]["additional_count"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_quick"] != null && ds.Tables[0].Rows[0]["is_quick"].ToString() != "")
                {
                    model.is_quick = int.Parse(ds.Tables[0].Rows[0]["is_quick"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_less"] != null && ds.Tables[0].Rows[0]["is_less"].ToString() != "")
                {
                    model.is_less = int.Parse(ds.Tables[0].Rows[0]["is_less"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_download"] != null && ds.Tables[0].Rows[0]["is_download"].ToString() != "")
                {
                    model.is_download = int.Parse(ds.Tables[0].Rows[0]["is_download"].ToString());
                }
                if (ds.Tables[0].Rows[0]["which_mp"] != null && ds.Tables[0].Rows[0]["which_mp"].ToString() != "")
                {
                    model.which_mp = ds.Tables[0].Rows[0]["which_mp"].ToString();
                }
                if (ds.Tables[0].Rows[0]["additional_id"] != null && ds.Tables[0].Rows[0]["additional_id"].ToString() != "")
                {
                    model.additional_id = ds.Tables[0].Rows[0]["additional_id"].ToString();
                }
                if (ds.Tables[0].Rows[0]["before_change_area_id"] != null && ds.Tables[0].Rows[0]["before_change_area_id"].ToString() != "")
                {
                    model.before_change_area_id = int.Parse(ds.Tables[0].Rows[0]["before_change_area_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsCountFreeAvaliable"] != null && ds.Tables[0].Rows[0]["IsCountFreeAvaliable"].ToString() != "")
                {
                    model.IsCountFreeAvaliable = int.Parse(ds.Tables[0].Rows[0]["IsCountFreeAvaliable"].ToString());
                }
                if (ds.Tables[0].Rows[0]["voucher_total"] != null && ds.Tables[0].Rows[0]["voucher_total"].ToString() != "")
                {
                    model.voucher_total = decimal.Parse(ds.Tables[0].Rows[0]["voucher_total"].ToString());
                }
                if (ds.Tables[0].Rows[0]["area_type"] != null && ds.Tables[0].Rows[0]["area_type"].ToString() != "")
                {
                    model.area_type = int.Parse(ds.Tables[0].Rows[0]["area_type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["user_address_id"] != null && ds.Tables[0].Rows[0]["user_address_id"].ToString() != "")
                {
                    model.user_address_id = int.Parse(ds.Tables[0].Rows[0]["user_address_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["officelinetype"] != null && ds.Tables[0].Rows[0]["officelinetype"].ToString() != "")
                {
                    model.officelinetype = int.Parse(ds.Tables[0].Rows[0]["officelinetype"].ToString());
                }
                #endregion  父表信息end

                #region  子表信息
                StringBuilder strSql2=new StringBuilder();
				strSql2.Append("select * from dt_order_goods ");
				strSql2.Append(" where order_id=@id ");
			    SqlParameter[] parameters2 = {
					    new SqlParameter("@id", SqlDbType.Int,4)};
			    parameters2[0].Value = id;

				DataSet ds2=DbHelperSQL.Query(strSql2.ToString(),parameters2);
				if(ds2.Tables[0].Rows.Count>0)
				{
					#region  子表字段信息
					int i = ds2.Tables[0].Rows.Count;
					List<Model.order_goods> models = new List<Model.order_goods>();
					Model.order_goods modelt;
					for (int n = 0; n < i; n++)
					{
						modelt = new Model.order_goods();
						if(ds2.Tables[0].Rows[n]["id"]!=null && ds2.Tables[0].Rows[n]["id"].ToString()!="")
						{
							modelt.id=int.Parse(ds2.Tables[0].Rows[n]["id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["order_id"]!=null && ds2.Tables[0].Rows[n]["order_id"].ToString()!="")
						{
							modelt.order_id=int.Parse(ds2.Tables[0].Rows[n]["order_id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["goods_id"]!=null && ds2.Tables[0].Rows[n]["goods_id"].ToString()!="")
						{
							modelt.goods_id=int.Parse(ds2.Tables[0].Rows[n]["goods_id"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["goods_name"]!=null && ds2.Tables[0].Rows[n]["goods_name"].ToString()!="")
						{
							modelt.goods_name=ds2.Tables[0].Rows[n]["goods_name"].ToString();
						}
						if(ds2.Tables[0].Rows[n]["goods_price"]!=null && ds2.Tables[0].Rows[n]["goods_price"].ToString()!="")
						{
							modelt.goods_price=decimal.Parse(ds2.Tables[0].Rows[n]["goods_price"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["real_price"]!=null && ds2.Tables[0].Rows[n]["real_price"].ToString()!="")
						{
							modelt.real_price=decimal.Parse(ds2.Tables[0].Rows[n]["real_price"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["quantity"]!=null && ds2.Tables[0].Rows[n]["quantity"].ToString()!="")
						{
							modelt.quantity=int.Parse(ds2.Tables[0].Rows[n]["quantity"].ToString());
						}
						if(ds2.Tables[0].Rows[n]["point"]!=null && ds2.Tables[0].Rows[n]["point"].ToString()!="")
						{
							modelt.point=int.Parse(ds2.Tables[0].Rows[n]["point"].ToString());
						}
                        if (ds2.Tables[0].Rows[n]["type"] != null && ds2.Tables[0].Rows[n]["type"].ToString() != "")
                        {
                            modelt.type = ds2.Tables[0].Rows[n]["type"].ToString();
                        }
                        if (ds2.Tables[0].Rows[n]["subgoodsid"] != null && ds2.Tables[0].Rows[n]["subgoodsid"].ToString() != "")
                        {
                            modelt.subgoodsid = ds2.Tables[0].Rows[n]["subgoodsid"].ToString();
                        }
                        if (ds2.Tables[0].Rows[n]["category_title"] != null && ds2.Tables[0].Rows[n]["category_title"].ToString() != "")
                        {
                            modelt.category_title = ds2.Tables[0].Rows[n]["category_title"].ToString();
                        }
                        if (ds2.Tables[0].Rows[n]["condition_price"] != null && ds2.Tables[0].Rows[n]["condition_price"].ToString() != "")
                        {
                            modelt.condition_price = ds2.Tables[0].Rows[n]["condition_price"].ToString();
                        }
						models.Add(modelt);
					}
					model.order_goods = models;
					#endregion  子表字段信息end
				}
				#endregion  子表信息end

				return model;
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 根据订单号返回一个实体
        /// </summary>
        public Model.orders GetModel(string order_no)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from dt_orders");
            strSql.Append(" where order_no=@order_no");
            SqlParameter[] parameters = {
					new SqlParameter("@order_no", SqlDbType.NVarChar,100)};
            parameters[0].Value = order_no;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return GetModel(Convert.ToInt32(obj));
            }
            return null;
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
            strSql.Append(" * ");
            strSql.Append(" FROM dt_orders ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetListForPage(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount) 
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select do.*,tempMorning.order_amount as morningorderamount,tempMorning.total as morningtotal,tempAfternoon.order_amount as afternoonorderamount,tempAfternoon.total as afternoontotal FROM dt_orders do");
            strSql.Append(" LEFT JOIN  ");
            strSql.Append(" ( ");
            strSql.Append(" SELECT do.worker_id,do.area_id, SUM(do.order_amount) AS order_amount,COUNT(1) AS total  ");
            strSql.Append(" FROM dt_orders do ");
            strSql.Append(" WHERE do.status in (1,2,3) and do.add_time BETWEEN '" + now + " 00:00:00' AND '" + now + " 13:59:59' ");
            strSql.Append(" GROUP BY do.worker_id,do.area_id ");
            strSql.Append(" ) AS tempMorning ");
            strSql.Append(" ON tempMorning.worker_id = do.worker_id and tempMorning.area_id = do.area_id ");
            strSql.Append(" LEFT JOIN ( ");
            strSql.Append(" SELECT do.worker_id,do.area_id,SUM(do.order_amount) AS order_amount,COUNT(1) AS total  ");
            strSql.Append(" FROM dt_orders do ");
            strSql.Append(" WHERE do.status in (1,2,3) and do.add_time BETWEEN '" + now + " 13:59:59' AND '" + now + " 23:59:59' ");
            strSql.Append(" GROUP BY do.worker_id,do.area_id ");
            strSql.Append(" ) AS tempAfternoon ");
            strSql.Append(" ON tempAfternoon.worker_id = do.worker_id and tempAfternoon.area_id = do.area_id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM dt_orders");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        public DataSet GetListForAreaBi(int pageSize, int pageIndex, string strWhere, out int recordCount, string area, string orderby = " riqi desc")
        {
            StringBuilder strSql = new StringBuilder();
            var endstr = "";
            if (!strWhere.Contains("do.add_time")){
                strSql.Append("SELECT top 1 temp4.addtime AS riqi, temp1.* ");
                endstr = "Order by temp4.addtime desc";
            }
            else
            {
                strSql.Append("SELECT temp4.addtime AS riqi, temp1.* ");
            }
            
            strSql.Append(",temp2.orderamount AS  onlineamount,temp3.orderamount AS offlineamount ");
            strSql.Append(",temp4.orderamount AS totalamount ");
            strSql.Append("FROM  ");
            strSql.Append("( ");
            strSql.Append("SELECT CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.order_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do ");
            strSql.Append("WHERE " + strWhere + " and do.OrderType IN ('网页','转单(区域)','线下订单','电话','微信')  and do.status in (1,2,3) and (payment_id=1 or ((payment_id in (2,3,5,6,7,8,9,10)) and payment_status=2))");
            strSql.Append("GROUP BY CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp4 ");
            strSql.Append("LEFT JOIN  ");
            strSql.Append("( ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append("SELECT ba.Title,CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.order_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do,bf_area ba ");
            strSql.Append("WHERE " + strWhere + " and do.OrderType IN ('网页','转单(区域)','线下订单','电话','微信') and do.Status in(1,2,3) and payment_id=1");
            strSql.Append("AND do.area_id=ba.Id ");
            strSql.Append("GROUP BY ba.Title,CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append("Union ");
            strSql.Append("SELECT ba.Title+'_On',CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.order_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do,bf_area ba ");
            strSql.Append("WHERE " + strWhere + " and do.OrderType IN ('网页','转单(区域)','线下订单','电话','微信') and do.Status in(1,2,3) and ((payment_id in (2,3,5,6,7,8,9,10)) and payment_status=2)");
            strSql.Append("AND do.area_id=ba.Id ");
            strSql.Append("GROUP BY ba.Title,CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS tmep ");
            strSql.Append("PIVOT ");
            strSql.Append("( ");
            strSql.Append("SUM(tmep.orderamount) for tmep.Title IN (" + area + ") ");
            strSql.Append(")t) AS temp1 ");
            strSql.Append("ON temp4.addtime=temp1.addtime ");
            strSql.Append("LEFT JOIN( ");
            strSql.Append("SELECT CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.order_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do ");
            strSql.Append("WHERE " + strWhere + " and do.OrderType IN ('网页','转单(区域)','电话','微信') and do.status in (1,2,3) and ((payment_id=1 or payment_id=2) or ((payment_id in (3,5,6,7,8,9,10)) and payment_status=2))");
            strSql.Append("GROUP BY CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp2 ");
            strSql.Append("ON temp4.addtime=temp2.addtime ");
            strSql.Append("LEFT JOIN( ");
            strSql.Append("SELECT CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.order_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do ");
            strSql.Append("WHERE " + strWhere + " and do.OrderType = '线下订单' and do.status in (1,2,3)  and ((payment_id=1 or payment_id=2) or ((payment_id in (3,5,6,7,8,9,10)) and payment_status=2))");
            //strSql.Append("WHERE " + strWhere + " and do.OrderType = '线下订单' and do.status in (1,2,3)  and (payment_id in( 1,2,3,5,6,7,8)) and payment_status=2))");
            strSql.Append("GROUP BY CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp3 ");
            strSql.Append("ON temp4.addtime=temp3.addtime ");
            strSql.Append(endstr);

            recordCount = 15;
            //recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), orderby));
        }

        public DataSet GetListForCategoryBi(int pageSize, int pageIndex, string strWhere, out int recordCount, string area)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT temp1.* ");            
            strSql.Append("FROM  ");
            strSql.Append("( ");
            strSql.Append("SELECT CONVERT(VARCHAR(10),do.add_time,102) AS addtime ");
            strSql.Append(",SUM(do.real_amount) AS orderamount ");
            strSql.Append("FROM dt_orders do,bf_area ba ");
            strSql.Append("WHERE " + strWhere + " AND do.area_id=ba.Id and do.OrderType IN ('网页','转单(区域)','线下订单','电话')  and do.status in (1,2,3) ");
            strSql.Append("GROUP BY CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp4 ");
            strSql.Append("LEFT JOIN  ");
            strSql.Append("( ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append("SELECT dog.category_title as title,SUM(dog.goods_price*dog.quantity) AS price ");
	        strSql.Append(",CONVERT(VARCHAR(10),do.add_time,102) AS addtime  ");
            strSql.Append("FROM dt_orders do,dt_order_goods dog,bf_area ba ");
            strSql.Append("WHERE " + strWhere + " and do.id=dog.order_id   ");
            strSql.Append("AND do.area_id=ba.Id  ");
            strSql.Append("AND do.OrderType IN ('网页','转单(区域)','线下订单','电话') and do.Status in(1,2,3) ");
            strSql.Append("GROUP BY dog.category_title,CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp ");
            strSql.Append("PIVOT  ");
            strSql.Append("(  ");
            strSql.Append("SUM(temp.price) for temp.title IN ("+area+") ");
            strSql.Append(")t) AS temp1 ");
            strSql.Append("ON temp4.addtime=temp1.addtime ");            

            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), " addtime desc"));
        }

        public DataSet GetListForGoodsBi(int pageSize, int pageIndex, string strWhere, out int recordCount, string area)
        {
            StringBuilder strSql = new StringBuilder();
            
            strSql.Append("SELECT * FROM ( ");
            strSql.Append("SELECT dog.goods_name ,SUM(dog.goods_price*dog.quantity) AS price ");
            strSql.Append(",CONVERT(VARCHAR(10),do.add_time,102) AS addtime  ");
            strSql.Append("FROM dt_orders do,dt_order_goods dog,bf_area ba ");
            strSql.Append("WHERE " + strWhere + " and do.id=dog.order_id   ");
            strSql.Append("AND do.area_id=ba.Id  ");
            strSql.Append("AND do.OrderType IN ('网页','转单(区域)','线下订单','电话') and do.Status in(1,2,3) ");
            strSql.Append("GROUP BY dog.goods_name,CONVERT(VARCHAR(10),do.add_time,102) ");
            strSql.Append(") AS temp ");
            strSql.Append("PIVOT  ");
            strSql.Append("(  ");
            strSql.Append("SUM(temp.price) for temp.goods_name IN (" + area + ") ");
            strSql.Append(")t ");
            

            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), " addtime desc"));
        }

        public double GetSum(string strWhere,string column)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(" + column + ") as H ");
            strSql.Append(" from dt_orders do");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToDouble(DbHelperSQL.GetSingle(strSql.ToString()));
        }

        public double GetOrderGoodsSum(string strWhere, string column)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(" + column + ") as H ");
            strSql.Append(" from dt_order_goods dog");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Convert.ToDouble(DbHelperSQL.GetSingle(strSql.ToString()));
        }

        public DataSet GetOrderGoodsDistinct(string strWhere, string column)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT " + column + "  ");
            strSql.Append(" from dt_order_goods dog");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

		#endregion  Method

        #region 扩展方法============================================
        /// <summary>
        /// 查找不存在的商品并删除
        /// </summary>
        private void DeleteGoodsList(SqlConnection conn, SqlTransaction trans, List<Model.order_goods> models, int order_id)
        {
            StringBuilder idList = new StringBuilder();
            if (models != null)
            {
                foreach (Model.order_goods modelt in models)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
            }
            string id_list = Utils.DelLastChar(idList.ToString(), ",");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_order_goods where order_id=" +order_id);
            if (!string.IsNullOrEmpty(id_list))
            {
                strSql.Append(" and id not in(" + id_list + ")");
            }
            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString());
        }
        #endregion 扩展方法end

    }
}


