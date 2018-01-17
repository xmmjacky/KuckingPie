using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.SessionState;
using DTcms.Web.UI;
using DTcms.Common;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DTcms.Web.tools
{
    /// <summary>
    /// 管理后台AJAX处理页
    /// </summary>
    public class admin_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = DTRequest.GetQueryString("action");

            switch (action)
            {
                case "sys_channel_load": //加载频道管理菜单
                    sys_channel_load(context);
                    break;
                case "plugins_nav_load": //加载插件管理菜单
                    plugins_nav_load(context);
                    break;
                case "sys_channel_validate": //验证频道名称是否重复
                    sys_channel_validate(context);
                    break;
                case "sys_urlrewrite_validate": //验证URL重写是否重复
                    sys_urlrewrite_validate(context);
                    break;
                case "validate_username": //验证会员用户名是否重复
                    validate_username(context);
                    break;
                case "set_area":
                    SetArea(context);
                    break;
                case "get_init_worker_area":
                    GetInitWorkerArea(context);
                    break;
                case "get_worker_edit":
                    GetWorkerEdit(context);
                    break;
                case "add_worker":
                    AddWorker(context);
                    break;
                case "repeat_order":
                    RepeatOrder(context);
                    break;
                case "switch_busy":
                    SwitchBusy(context);
                    break;
                case "get_order":
                    GetModel(context);
                    break;
                case "update_order":
                    UpdateModel(context);
                    break;
                case "change_area":
                    ChangeArea(context);
                    break;
                case "switch_area":
                    SwitchArea(context);
                    break;
                case "area_switch_type":
                    AreaSwitchType(context);
                    break;
                case "query_mp_pay_status":
                    QueryMpPayStatus(context);
                    break;
                case "confirm_sync_status":
                    ConfirmSyncStatus(context);
                    break;
                case "query_unconfirm_sync_order":
                    QueryUnConfirmSyncOrder(context);
                    break;
            }

        }

        #region 加载频道管理菜单================================
        private void sys_channel_load(HttpContext context)
        {
            StringBuilder strTxt = new StringBuilder();
            BLL.sys_channel bll = new BLL.sys_channel();
            DataTable dt = bll.GetList("").Tables[0];
            strTxt.Append("[");
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                Model.manager admin_info = new ManagePage().GetAdminInfo();
                if (!new BLL.manager_role().Exists(admin_info.role_id, Convert.ToInt32(dr["id"]), DTEnums.ActionEnum.View.ToString()))
                {
                    continue;
                }
                BLL.sys_model bll2 = new BLL.sys_model();
                Model.sys_model model2 = bll2.GetModel(Convert.ToInt32(dr["model_id"]));
                strTxt.Append("{");
                strTxt.Append("\"text\":\"" + dr["title"] + "\",");
                strTxt.Append("\"isexpand\":\"false\",");
                strTxt.Append("\"children\":[");
                if (model2.sys_model_navs != null)
                {
                    int j = 1;
                    foreach (Model.sys_model_nav nav in model2.sys_model_navs)
                    {
                        strTxt.Append("{");
                        strTxt.Append("\"text\":\"" + nav.title + "\",");
                        strTxt.Append("\"url\":\"" + nav.nav_url + "?channel_id=" + dr["id"] + "\""); //此处要优化，加上nav.nav_url网站目录标签替换
                        strTxt.Append("}");
                        if (j < model2.sys_model_navs.Count)
                        {
                            strTxt.Append(",");
                        }
                        j++;
                    }
                }
                strTxt.Append("]");
                strTxt.Append("}");
                strTxt.Append(",");
                i++;
            }
            string newTxt = Utils.DelLastChar(strTxt.ToString(), ",") + "]";
            context.Response.Write(newTxt);
            return;
        }
        #endregion

        #region 加载插件管理菜单================================
        private void plugins_nav_load(HttpContext context)
        {
            BLL.plugin bll = new BLL.plugin();
            DirectoryInfo dirInfo = new DirectoryInfo(Utils.GetMapPath("../plugins/"));
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                Model.plugin aboutInfo = bll.GetInfo(dir.FullName + @"\");
                if (aboutInfo.isload == 1 && File.Exists(dir.FullName + @"\admin\index.aspx"))
                {
                    context.Response.Write("<li><a class=\"l-link\" href=\"javascript:f_addTab('plugin_" + dir.Name
                        + "','" + aboutInfo.name + "','../../plugins/" + dir.Name + "/admin/index.aspx')\">" + aboutInfo.name + "</a></li>\n");
                }
            }
            return;
        }
        #endregion

        #region 验证频道名称是否重复============================
        private void sys_channel_validate(HttpContext context)
        {
            string channelname = DTRequest.GetFormString("channelname");
            string oldname = DTRequest.GetFormString("oldname");
            if (string.IsNullOrEmpty(channelname))
            {
                context.Response.Write("false");
                return;
            }
            //检查是否与站点根目录下的目录同名
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            DirectoryInfo dirInfo = new DirectoryInfo(Utils.GetMapPath(siteConfig.webpath));
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (channelname.ToLower() == dir.Name)
                {
                    context.Response.Write("false");
                    return;
                }
            }
            //检查是否修改操作
            if (channelname == oldname)
            {
                context.Response.Write("true");
                return;
            }
            //检查Key是否与已存在
            BLL.sys_channel bll = new BLL.sys_channel();
            if (bll.Exists(channelname))
            {
                context.Response.Write("false");
                return;
            }
            context.Response.Write("true");
            return;
        }
        #endregion

        #region 验证URL重写是否重复=============================
        private void sys_urlrewrite_validate(HttpContext context)
        {
            string rewritekey = DTRequest.GetFormString("rewritekey");
            string oldkey = DTRequest.GetFormString("oldkey");
            if (string.IsNullOrEmpty(rewritekey))
            {
                context.Response.Write("false1");
                return;
            }
            //检查是否修改操作
            if (rewritekey.ToLower() == oldkey.ToLower())
            {
                context.Response.Write("true");
                return;
            }
            //检查站点URL配置文件节点是否重复
            List<Model.url_rewrite> ls = new BLL.url_rewrite().GetList("");
            foreach (Model.url_rewrite model in ls)
            {
                if (model.name.ToLower() == rewritekey.ToLower())
                {
                    context.Response.Write("false2");
                    return;
                }
            }
            context.Response.Write("true");
            return;
        }
        #endregion

        #region 验证用户名是否可用==============================
        private void validate_username(HttpContext context)
        {
            string username = DTRequest.GetFormString("username");
            string oldusername = DTRequest.GetFormString("oldusername");
            //如果为Null，退出
            if (string.IsNullOrEmpty(username))
            {
                context.Response.Write("false");
                return;
            }
            Model.userconfig userConfig = new BLL.userconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_USER_XML_CONFING));
            //过滤注册用户名字符
            string[] strArray = userConfig.regkeywords.Split(',');
            foreach (string s in strArray)
            {
                if (s.ToLower() == username.ToLower())
                {
                    context.Response.Write("false");
                    return;
                }
            }
            //检查是否修改操作
            if (username == oldusername)
            {
                context.Response.Write("true");
                return;
            }
            BLL.users bll = new BLL.users();
            //查询数据库
            if (bll.Exists(username.Trim()))
            {
                context.Response.Write("false");
                return;
            }
            context.Response.Write("true");
            return;
        }
        #endregion

        private void SetArea(HttpContext context)
        {
            string areaid = DTRequest.GetFormString("areaid");
            context.Session["AreaId"] = areaid;
            context.Response.Write("true");
        }

        private void GetInitWorkerArea(HttpContext context)
        {
            string areaid = context.Session["AreaId"].ToString();
            List<BookingFood.Model.bf_area> areaList =
                new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
            string rtn = string.Empty;
            foreach (var item in areaList)
            {
                rtn += string.Format("<label><input type=\"checkbox\" data-id=\"{1}\" /><b>{0}</b></label>"
                    , item.Title, item.Id.ToString());
            }
            context.Response.Write(rtn);
        }

        private void GetWorkerEdit(HttpContext context)
        {
            string workerid = DTRequest.GetFormString("workerid");
            BookingFood.Model.bf_worker workerModel = new BookingFood.BLL.bf_worker().GetModel(int.Parse(workerid));
            List<BookingFood.Model.bf_area_worker> areaworkerList =
                new BookingFood.BLL.bf_area_worker().GetModelList(" WorkerId=" + workerid);
            string areaid = context.Session["AreaId"].ToString();
            List<BookingFood.Model.bf_area> areaList =
                new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
            string rtn = string.Empty;
            foreach (var item in areaList)
            {
                bool isExist = areaworkerList.Exists(s => s.AreaId == item.Id);
                rtn += string.Format("<label><input type='checkbox' data-id='{1}' {2}/><b>{0}</b></label>"
                    , item.Title, item.Id.ToString(), isExist ? "checked='checked'" : "");
            }
            context.Response.Write("{\"msg\":1,\"sortid\":\"" + workerModel.SortId.ToString()
                + "\", \"title\":\"" + workerModel.Title + "\", \"telphone\":\"" + workerModel.Telphone
                + "\", \"workertype\":\"" + workerModel.WorkerType
                + "\", \"operatetype\":\"" + workerModel.OperateType                
                + "\",\"msgbox\":\"" + rtn + "\"" + "}");
        }

        private void AddWorker(HttpContext context)
        {
            string workerid = DTRequest.GetFormString("workerid");
            string title = DTRequest.GetFormString("title");
            string telphone = DTRequest.GetFormString("telphone");
            string sortid = DTRequest.GetFormString("sortid");
            string workertype = DTRequest.GetFormString("workertype");
            int operate = DTRequest.GetFormInt("operate");
            string areaid = DTRequest.GetFormString("areaid");
            BookingFood.Model.bf_worker worker = new BookingFood.Model.bf_worker();
            BookingFood.BLL.bf_worker bllWorker = new BookingFood.BLL.bf_worker();
            int identity = 0;
            if (string.IsNullOrEmpty(workerid))
            {
                worker.SortId = int.Parse(sortid);
                worker.Telphone = telphone;
                worker.Title = title;
                worker.WorkerType = workertype;
                worker.OperateType = operate;
                identity = bllWorker.Add(worker);
            }
            else
            {
                worker = bllWorker.GetModel(int.Parse(workerid));
                identity = worker.Id;
                worker.SortId = int.Parse(sortid);
                worker.Telphone = telphone;
                worker.Title = title;
                worker.WorkerType = workertype;
                worker.OperateType = operate;
                bllWorker.Update(worker);
            }
            BookingFood.BLL.bf_area_worker bllAreaWorker = new BookingFood.BLL.bf_area_worker();
            List<BookingFood.Model.bf_area_worker> areaworkerList =
                bllAreaWorker.GetModelList(" WorkerId=" + identity.ToString());
            foreach (var item in areaworkerList)
            {
                bllAreaWorker.Delete(item.Id);
            }
            foreach (var item in areaid.Split(','))
            {
                bllAreaWorker.Add(new BookingFood.Model.bf_area_worker()
                {
                    AreaId = int.Parse(item),
                    Type = "",
                    WorkerId = identity
                });
            }
            context.Response.Write("{\"msgbox\":\"操作成功！\"" + "}");
        }

        private void RepeatOrder(HttpContext context)
        {
            int orderid = DTRequest.GetFormInt("id");
            string message = DTRequest.GetFormString("message");
            BLL.orders bllOrder = new BLL.orders();
            Model.orders orderModel = bllOrder.GetModel(orderid);
            orderModel.OrderType = "通知";
            orderModel.message = "[" + message + "]";
            orderModel.order_no = Utils.GetOrderNumber(); //订单号
            orderModel.add_time = DateTime.Now;
            orderModel.order_goods = null;
            orderModel.status = 1;
            orderModel.confirm_time = null;
            orderModel.worker_id = 0;
            orderModel.worker_name = "";
            orderModel.real_amount = 0;
            orderModel.real_freight = 0;
            orderModel.payable_amount = 0;
            orderModel.payable_freight = 0;
            orderModel.payment_fee = 0;
            orderModel.order_amount = 0;
            bllOrder.Add(orderModel);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"操作成功！\"}");
        }

        private void SwitchBusy(HttpContext context)
        {
            string status = context.Request.Params["status"];
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            model.busy = status;
            bll.saveConifg(model, Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            context.Response.Write("{\"msg\":1, \"msgbox\":\"操作成功！\"}");
        }

        private void GetModel(HttpContext context)
        {
            string orderid = context.Request.Params["id"];
            DTcms.Model.orders orderModel = new DTcms.BLL.orders().GetModel(int.Parse(orderid));
            if (orderModel != null)
            {
                context.Response.Write("{\"telphone\":\"" + orderModel.telphone + "\", \"address\":\"" + orderModel.address + "\", \"message\":\"" + orderModel.message.Replace("\n","") + "\"}");
                return;
            }
        }

        private void UpdateModel(HttpContext context)
        {
            string orderid = context.Request.Params["id"];
            string telphone = context.Request.Params["telphone"];
            string address = context.Request.Params["address"];
            string message = context.Request.Params["message"];
            DTcms.BLL.orders bll = new DTcms.BLL.orders();
            DTcms.Model.orders orderModel = bll.GetModel(int.Parse(orderid));
            if (orderModel != null)
            {
                orderModel.telphone = telphone;
                orderModel.address = address;
                orderModel.message = message;
                bll.Update(orderModel);
                BLL.users bllUsers = new BLL.users();
                Model.users userModel = bllUsers.GetModel(orderModel.user_id);
                userModel.telphone = telphone;
                userModel.address = address;
                BookingFood.BLL.bf_user_address bllUserAddress = new BookingFood.BLL.bf_user_address();
                if(orderModel.user_address_id!=0)
                {
                    BookingFood.Model.bf_user_address modelUserAddress = 
                        bllUserAddress.GetModel(orderModel.user_address_id);
                
                    modelUserAddress.Telphone = telphone;
                    modelUserAddress.Address = address;
                    bllUserAddress.Update(modelUserAddress);
                }
                context.Response.Write("{\"msg\":1, \"msgbox\":\"修改成功！\"}");
                return;
            }
            context.Response.Write("{\"msg\":0, \"msgbox\":\"修改失败！\"}");
        }

        private void ChangeArea(HttpContext context)
        {
            string orderid = context.Request.Params["id"];
            string areaid = context.Request.Params["changeareaid"];
            DTcms.BLL.orders bll = new DTcms.BLL.orders();
            DTcms.Model.orders orderModel = bll.GetModel(int.Parse(orderid));
            int oldid = 0;
            string oldtitle = string.Empty, oldorderno = string.Empty;
            if (orderModel != null)
            {
                BookingFood.Model.bf_area areaModel =
                    new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
                bll.UpdateField(int.Parse(orderid), "status=5");
                oldid = orderModel.area_id;
                oldtitle = orderModel.area_title;
                oldorderno = orderModel.order_no;
                
                orderModel.OrderType = "转单(区域)";
                orderModel.order_no = Utils.GetOrderNumber();
                orderModel.add_time = DateTime.Now;
                orderModel.status = 1;
                orderModel.distribution_status = 1;
                orderModel.confirm_time = null;
                orderModel.distribution_time = null;
                orderModel.area_id = areaModel.Id;
                orderModel.area_title = areaModel.Title;
                bll.Add(orderModel);

                orderModel.order_no = Utils.GetOrderNumber();
                orderModel.area_id = oldid;
                orderModel.area_title = oldtitle;
                orderModel.order_no = oldorderno;
                orderModel.order_goods = new List<Model.order_goods>();
                orderModel.order_amount = 0;
                orderModel.message = "[已转单]";
                //bll.Add(orderModel);
                BookingFood.BLL.bf_user_address bllUserAddress = new BookingFood.BLL.bf_user_address();
                if (orderModel.user_address_id != 0)
                {
                    BookingFood.Model.bf_user_address modelUserAddress =
                        bllUserAddress.GetModel(orderModel.user_address_id);

                    modelUserAddress.AreaId = orderModel.area_id;
                    bllUserAddress.Update(modelUserAddress);
                }
                context.Response.Write("{\"msg\":1, \"msgbox\":\"修改成功！\"}");
                return;
            }
            context.Response.Write("{\"msg\":0, \"msgbox\":\"修改失败！\"}");
        }

        private void SwitchArea(HttpContext context)
        {
            string areaid = context.Request.Params["changeareaid"];
            string articleid = context.Request.Params["articleid"];
            string type = context.Request.Params["type"];
            BookingFood.BLL.bf_area_article bll = new BookingFood.BLL.bf_area_article();
            List<BookingFood.Model.bf_area_article> list =
                bll.GetModelList(string.Format(" AreaId={0} and ArticleId={1} and Type='{2}'", areaid, articleid, type));
            if (list.Count > 0)
            {
                if (list[0].IsLock == 0)
                {
                    list[0].IsLock = 1;
                    bll.Update(list[0]);
                }
                else
                {
                    list[0].IsLock = 0;
                    bll.Update(list[0]);
                }
            }
            //else
            //{
            //    bll.Add(new BookingFood.Model.bf_area_article() { AreaId = int.Parse(areaid), ArticleId = int.Parse(articleid), Type = type, IsLock = 0 });
            //}
            context.Response.Write("{\"msg\":1, \"msgbox\":\"操作成功！\"}");
        }

        private void AreaSwitchType(HttpContext context)
        {
            int id = int.Parse(context.Request.Params["id"]);
            string type = context.Request.Params["type"];
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
            BookingFood.Model.bf_area bo = bll.GetModel(id);
            switch(type)
            {
                case "busy":
                    if (bo.IsBusy == 0)
                    {
                        bo.IsBusy = 1;
                    }
                    else
                    {
                        bo.IsBusy = 0;
                    }
                    break;
                case "lock":
                    if (bo.IsLock == 0)
                    {
                        bo.IsLock = 1;
                    }
                    else
                    {
                        bo.IsLock = 0;
                    }
                    break;
            }
            bll.Update(bo);
            List<BookingFood.Model.bf_area> list = bll.GetModelList(" ParentId=" + id.ToString());
            foreach (var item in list)
            {
                switch (type)
                {
                    case "busy":
                        item.IsBusy = bo.IsBusy;
                        break;
                    case "lock":
                        item.IsLock = bo.IsLock;
                        break;
                }
                bll.Update(item);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"操作成功！\"}");
        }

        private void QueryMpPayStatus(HttpContext context)
        {
            string orderno = DTRequest.GetFormString("orderno");
            BLL.orders bll = new BLL.orders();
            Model.orders modelOrder = bll.GetModel(orderno);
            if(modelOrder.payment_id!=5 && modelOrder.payment_id!=6 && modelOrder.payment_id != 8)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"非微信支付方式无需查询支付状态\"}");
                return;
            }
            if(modelOrder.payment_status==2)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"订单已支付\"}");
                return;
            }
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            string timeStamp = TenPayUtil.GetTimestamp();
            string nonceStr = TenPayUtil.GetNoncestr();


            //设置package订单参数
            packageReqHandler.SetParameter("appid", siteConfig.mp_slave_appid);       //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);         //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("out_trade_no", orderno);
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);                       //签名

            string data = packageReqHandler.ParseXML();
            var mppay_result = TenPayV3.OrderQuery(data);
            var res = XDocument.Parse(mppay_result);
            if(res.Element("xml").Element("return_code").Value=="FAIL")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\""+ res.Element("xml").Element("return_msg").Value + "\"}");
                return;
            }
            if (res.Element("xml").Element("result_code").Value == "FAIL")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"" + res.Element("xml").Element("err_code_des").Value + "\"}");
                return;
            }
            string _pay_title = string.Empty, _pay_status = "0";
            switch(res.Element("xml").Element("trade_state").Value)
            {
                case "SUCCESS":
                    _pay_title = "支付成功";
                    _pay_status = "1";
                    break;
                case "REFUND":
                    _pay_title = "转入退款";
                    break;
                case "NOTPAY":
                    _pay_title = "未支付";
                    break;
                case "CLOSED":
                    _pay_title = "已关闭";
                    break;
                case "REVOKED":
                    _pay_title = "已撤销（刷卡支付）";
                    break;
                case "USERPAYING":
                    _pay_title = "用户支付中";
                    break;
                case "PAYERROR":
                    _pay_title = "支付失败(其他原因，如银行返回失败)";
                    break;
            }
            //处理确认收款的业务
            if(modelOrder.payment_id==5 && string.Equals(_pay_status,"1"))
            {
                bll.UpdateField(modelOrder.id, "payment_status=2,payment_time='" + DateTime.Now + "'");

                #region 更新活动日志中对应的使用记录的支付状态
                BookingFood.BLL.bf_carnival_user_log bllCarUserLog = new BookingFood.BLL.bf_carnival_user_log();
                BookingFood.Model.bf_carnival_user_log modelCarUserLog = bllCarUserLog.GetModelList(" OrderId=" + modelOrder.id).FirstOrDefault();
                if (modelCarUserLog != null)
                {
                    modelCarUserLog.IsPayForTakeOut = 1;
                    bllCarUserLog.Update(modelCarUserLog);
                }
                #endregion

                #region 后厨
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                BLL.article bllArticle = new BLL.article();
                foreach (var item in modelOrder.order_goods)
                {
                    if (item.type == "one" || item.type == "full" || item.type == "discount")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = modelOrder.id,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = modelOrder.area_id,
                            IsDown = 0,
                            Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                            Freight = modelOrder.takeout == 2 ? "外带" : modelOrder.takeout == 1 ? "堂吃" : "外卖"
                        };
                        if (goodsModel.nick_id != 0)
                        {
                            nickModel = bllNick.GetModel(goodsModel.nick_id);
                            back.GoodsName = nickModel.Title;
                        }
                        else
                        {
                            back.GoodsName = item.goods_name;
                        }
                        listBack.Add(back);
                    }
                    else if (item.type == "combo")
                    {
                        string[] subgoods = item.subgoodsid.Split('†');
                        foreach (var sub in subgoods)
                        {
                            if (sub.Split('‡')[0] == "taste") continue;
                            goodsModel = bllArticle.GetGoodsModel(int.Parse(sub.Split('‡')[1]));
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = modelOrder.id,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = modelOrder.area_id,
                                IsDown = 0,
                                Taste = sub.Split('‡')[3],
                                Freight = modelOrder.takeout == 2 ? "外带" : modelOrder.takeout == 1 ? "堂吃" : "外卖"
                            };
                            if (goodsModel.nick_id != 0)
                            {
                                nickModel = bllNick.GetModel(goodsModel.nick_id);
                                back.GoodsName = nickModel.Title;
                            }
                            else
                            {
                                back.GoodsName = goodsModel.title;
                            }
                            listBack.Add(back);
                        }
                    }
                }
                BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                foreach (var item in listBack)
                {
                    bllBack.Add(item);
                }
                #endregion

                //扣除积分
                if (modelOrder.point < 0)
                {
                    new BLL.point_log().Add(modelOrder.user_id, modelOrder.user_name, modelOrder.point, "换购扣除积分，订单号：" + modelOrder.order_no);
                }

                #region 在满送活动期间内数量+1
                BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
                BookingFood.Model.bf_carnival carnivalModel =
                    bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
                if (carnivalModel != null)
                {
                    Model.users userModel = new BLL.users().GetModel(modelOrder.user_id);
                    BookingFood.BLL.bf_carnival_user_log bllCarnivalUserLog = new BookingFood.BLL.bf_carnival_user_log();
                    int joinNums = bllCarnivalUserLog.GetRecordCount(" CarnivalId=" + carnivalModel.Id
                        + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' And '"
                        + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "' And UserId=" + userModel.id);
                    if (joinNums == 0)
                    {
                        bllCarnivalUserLog.Add(new BookingFood.Model.bf_carnival_user_log()
                        {
                            AddTime = DateTime.Now,
                            CarnivalId = carnivalModel.Id,
                            OpenId = userModel.user_name,
                            OrderId = modelOrder.id,
                            UserId = userModel.id
                        });
                        BookingFood.BLL.bf_carnival_user bllCarnivalUser = new BookingFood.BLL.bf_carnival_user();
                        BookingFood.Model.bf_carnival_user carnivalUserModel =
                            bllCarnivalUser.GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
                        carnivalUserModel.Num += 1;
                        bllCarnivalUser.Update(carnivalUserModel);
                    }

                }
                #endregion

                #region 发送微信模板消息
                string accessToken = string.Empty;
                if (modelOrder.MpForHere != "")
                {
                    Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                    tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.add_time.ToString("yyyy-MM-dd HH:mm")));
                    tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.order_amount.ToString("0.00")));
                    string tempMessage = string.Empty;
                    foreach (var item in modelOrder.order_goods)
                    {
                        if (item.type == "one")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                        else if (item.type == "combo")
                        {
                            string[] subgoods = item.subgoodsid.Split('†');
                            string subrtn = string.Empty;
                            foreach (var sub in subgoods)
                            {
                                subrtn += string.Format("*{0}", sub.Split('‡')[2] + (!string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : ""));
                            }
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , subrtn);

                        }
                        else if (item.type == "full" || item.type == "discount")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                    }
                    tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                    tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.address));
                    tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(
                            modelOrder.takeout == 2 ? "外带取餐号：" + modelOrder.MpForHere :
                                modelOrder.takeout == 1 ? "堂吃取餐号：" + modelOrder.MpForHere : ""));

                    switch (modelOrder.which_mp)
                    {
                        case "master":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelOrder.user_name, "REmtsGZMK7-3NXNJf3NZMOfmH9dKwkvwvCBww5F9VYQ"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                        case "slave":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelOrder.user_name, "gYklU4AeAT7KCehbfRP5emhBsSNkhMVDVtdIBFlhn8Y"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                    }
                }
                else if (modelOrder.payment_id == 3 || modelOrder.payment_id == 5)
                {
                    Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                    tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.add_time.ToString("yyyy-MM-dd HH:mm")));
                    tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.order_amount.ToString("0.00")));
                    string tempMessage = string.Empty;
                    foreach (var item in modelOrder.order_goods)
                    {
                        if (item.type == "one")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                        else if (item.type == "combo")
                        {
                            string[] subgoods = item.subgoodsid.Split('†');
                            string subrtn = string.Empty;
                            foreach (var sub in subgoods)
                            {
                                subrtn += string.Format("*{0}", sub.Split('‡')[2] + (!string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : ""));
                            }
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , subrtn);

                        }
                        else if (item.type == "full" || item.type == "discount")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                    }
                    tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                    tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelOrder.address));
                    tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_remark));

                    switch (modelOrder.which_mp)
                    {
                        case "master":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelOrder.user_name, "REmtsGZMK7-3NXNJf3NZMOfmH9dKwkvwvCBww5F9VYQ"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                        case "slave":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelOrder.user_name, "gYklU4AeAT7KCehbfRP5emhBsSNkhMVDVtdIBFlhn8Y"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                    }

                }
                #endregion
            }
            else if(modelOrder.payment_id== 6 && string.Equals(_pay_status, "1"))
            {
                bll.UpdateField(modelOrder.id, "payment_status=2,payment_time='" + DateTime.Now + "'");

                #region 更新后厨推送数据
                BLL.article bllArticle = new BLL.article();
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in modelOrder.order_goods)
                {
                    if (item.type == "one")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = modelOrder.id,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = modelOrder.area_id,
                            IsDown = 0,
                            Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                            Freight = "外卖"
                        };
                        if (goodsModel.nick_id != 0)
                        {
                            nickModel = bllNick.GetModel(goodsModel.nick_id);
                            back.GoodsName = nickModel.Title;
                        }
                        else
                        {
                            back.GoodsName = item.goods_name;
                        }
                        listBack.Add(back);
                    }
                    else if (item.type == "combo")
                    {
                        string[] subgoods = item.subgoodsid.Split('†');
                        foreach (var sub in subgoods)
                        {
                            if (sub.Split('‡')[0] == "taste") continue;
                            goodsModel = bllArticle.GetGoodsModel(int.Parse(sub.Split('‡')[1]));
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = modelOrder.id,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = modelOrder.area_id,
                                IsDown = 0,
                                Taste = sub.Split('‡').Length == 4 ? sub.Split('‡')[3] : "",
                                Freight = "外卖"
                            };
                            if (goodsModel.nick_id != 0)
                            {
                                nickModel = bllNick.GetModel(goodsModel.nick_id);
                                back.GoodsName = nickModel.Title;
                            }
                            else
                            {
                                back.GoodsName = goodsModel.title;
                            }
                            listBack.Add(back);
                        }
                    }
                }
                BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                foreach (var item in listBack)
                {
                    bllBack.Add(item);
                }
                #endregion

                #region 发送邮件
                Model.mail_template mailModel = new BLL.mail_template().GetModel("ordermail");
                if (mailModel != null)
                {
                    //替换模板内容
                    string titletxt = mailModel.maill_title + modelOrder.order_no;
                    string bodytxt = mailModel.content;
                    bodytxt = bodytxt.Replace("{useremail}", modelOrder.email);
                    bodytxt = bodytxt.Replace("{useraddress}", modelOrder.address);
                    bodytxt = bodytxt.Replace("{usertelphone}", modelOrder.telphone);
                    bodytxt = bodytxt.Replace("{orderaddtime}", modelOrder.add_time.ToString("yyyy-MM-dd HH:mm:ss"));
                    bodytxt = bodytxt.Replace("{orderno}", modelOrder.order_no);
                    bodytxt = bodytxt.Replace("{orderamount}", (modelOrder.real_freight != 0 ? "外送费：" + modelOrder.real_freight.ToString() : "") + "总计：" + modelOrder.order_amount.ToString());
                    bodytxt = bodytxt.Replace("{ordermessage}", modelOrder.message);
                    string rtn = string.Empty;
                    foreach (var item in modelOrder.order_goods)
                    {
                        if (item.type == "one")
                        {
                            rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td style=\"width:60px;text-align:center;\">" +
                                        "{0}" +
                                    "</td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{1} {4}" +
                                    "</td>" +
                                    "<td >" +
                                        "{3}" +
                                    "</td>" +
                                "</tr>"
                                , item.quantity
                                , item.goods_name
                                , item.goods_price.ToString().Replace(".00", "")
                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                , !string.IsNullOrEmpty(item.subgoodsid) ? "(" + item.subgoodsid.Split('‡')[2] + ")" : "");
                        }
                        else if (item.type == "combo")
                        {
                            rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td  style=\"width:60px;text-align:center;\">" +
                                        "{0}" +
                                    "</td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{1}" +
                                    "</td>" +
                                    "<td  >" +
                                        "{3}" +
                                    "</td>" +
                                "</tr>", item.quantity, item.goods_name, item.goods_price.ToString().Replace(".00", "")
                                , (item.quantity * item.goods_price).ToString().Replace(".00", ""));
                            string[] subgoods = item.subgoodsid.Split('†');
                            foreach (var sub in subgoods)
                            {
                                rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td  style=\"width:60px;text-align:center;\"></td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{0} {1}" +
                                    "</td>" +
                                    "<td ></td>" +
                                    "</tr>"
                                    , sub.Split('‡')[2]
                                    , sub.Split('‡').Length == 4 ? "(" + sub.Split('‡')[3] + ")" : ""
                                    );
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(rtn))
                    {
                        rtn = "<table style=\"font-sze:18px;\">" + rtn + "</table>";
                    }
                    bodytxt = bodytxt.Replace("{orderdetail}", rtn);

                    //发送邮件
                    BookingFood.Model.bf_area areaModel = new BookingFood.BLL.bf_area().GetModel(modelOrder.area_id);
                    try
                    {
                        DTMail.sendMail(siteConfig.emailstmp,
                            siteConfig.emailusername,
                            DESEncrypt.Decrypt(siteConfig.emailpassword),
                            siteConfig.emailnickname,
                            siteConfig.emailfrom,
                            modelOrder.email,
                            titletxt, bodytxt);
                        //区域所属管理员邮件地址
                        if (areaModel.ManagerId != null)
                        {
                            DTMail.sendMail(siteConfig.emailstmp,
                            siteConfig.emailusername,
                            DESEncrypt.Decrypt(siteConfig.emailpassword),
                            siteConfig.emailnickname,
                            siteConfig.emailfrom,
                            new BLL.manager().GetModel((int)areaModel.ManagerId).user_name,
                            titletxt, bodytxt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info("{\"msg\":0, \"msgbox\":\"邮件发送失败，请联系本站管理员！" + ex.Message + "_" + ex.InnerException.Message + "\"}");
                    }

                }
                #endregion
            }
            //else if(modelOrder.payment_id==8 && string.Equals(_pay_status, "1"))
            //{
            //    bll.UpdateField(modelOrder.id, "payment_status=2,payment_time='" + DateTime.Now + "'");
            //    #region 后厨
            //    Model.article_goods goodsModel = null;
            //    BookingFood.Model.bf_back_door back = null;
            //    BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
            //    BookingFood.Model.bf_good_nickname nickModel = null;
            //    List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
            //    BLL.article bllArticle = new BLL.article();
            //    foreach (var item in modelOrder.order_goods)
            //    {
            //        if (item.type == "one" || item.type == "full" || item.type == "discount")
            //        {
            //            goodsModel = bllArticle.GetGoodsModel(item.goods_id);
            //            back = new BookingFood.Model.bf_back_door()
            //            {
            //                OrderId = modelOrder.id,
            //                GoodsCount = item.quantity,
            //                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
            //                AreaId = modelOrder.area_id,
            //                IsDown = 0,
            //                Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
            //                Freight = modelOrder.takeout == 2 ? "外带" : modelOrder.takeout == 1 ? "堂吃" : "外卖"
            //            };
            //            if (goodsModel.nick_id != 0)
            //            {
            //                nickModel = bllNick.GetModel(goodsModel.nick_id);
            //                back.GoodsName = nickModel.Title;
            //            }
            //            else
            //            {
            //                back.GoodsName = item.goods_name;
            //            }
            //            listBack.Add(back);
            //        }
            //        else if (item.type == "combo")
            //        {
            //            string[] subgoods = item.subgoodsid.Split('†');
            //            foreach (var sub in subgoods)
            //            {
            //                if (sub.Split('‡')[0] == "taste") continue;
            //                goodsModel = bllArticle.GetGoodsModel(int.Parse(sub.Split('‡')[1]));
            //                back = new BookingFood.Model.bf_back_door()
            //                {
            //                    OrderId = modelOrder.id,
            //                    GoodsCount = item.quantity,
            //                    CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
            //                    AreaId = modelOrder.area_id,
            //                    IsDown = 0,
            //                    Taste = sub.Split('‡')[3],
            //                    Freight = modelOrder.takeout == 2 ? "外带" : modelOrder.takeout == 1 ? "堂吃" : "外卖"
            //                };
            //                if (goodsModel.nick_id != 0)
            //                {
            //                    nickModel = bllNick.GetModel(goodsModel.nick_id);
            //                    back.GoodsName = nickModel.Title;
            //                }
            //                else
            //                {
            //                    back.GoodsName = goodsModel.title;
            //                }
            //                listBack.Add(back);
            //            }
            //        }
            //    }
            //    BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
            //    foreach (var item in listBack)
            //    {
            //        bllBack.Add(item);
            //    }
            //    #endregion
            //}
            context.Response.Write("{\"msg\":1,\"paystatus\":"+ _pay_status + ", \"msgbox\":\"" + _pay_title + "\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private TenPayV3Info _tenPayV3Info;
        public TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }

        private void ConfirmSyncStatus(HttpContext context)
        {
            string orderno = DTRequest.GetFormString("orderno");
            BLL.orders bll = new BLL.orders();
            Model.orders modelOrder = bll.GetModel(orderno);
            if (modelOrder.payment_status!=12 && modelOrder.payment_status!=11)
            {
                context.Response.Write("{\"msg\":1, \"msgbox\":\"非同步订单状态无须确认\"}");
                return;
            }
            if (modelOrder.payment_status == 2 || modelOrder.payment_status == 1)
            {
                context.Response.Write("{\"msg\":1, \"msgbox\":\"订单已确认\"}");
                return;
            }
            bll.UpdateField(modelOrder.id, "payment_status="+(modelOrder.payment_status-10));
            context.Response.Write("{\"msg\":0, \"msgbox\":\"订单已确认\"}");
        }

        private void QueryUnConfirmSyncOrder(HttpContext context)
        {
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if(siteConfig.RunLoopThirdOrder==0)
            {
                return;
            }
            else
            {
                BLL.orders bll = new BLL.orders();
                int count = bll.GetCount("(payment_status=12 or payment_status=11) and status !=5");
                context.Response.Write("{\"msg\":0, \"count\":" + count + "}");
            }
            
        }
    }
}