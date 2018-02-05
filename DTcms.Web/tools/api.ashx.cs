using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using DTcms.Common;
using TeeGonSdk.Request;
using TeeGonSdk.Response;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Xml.Linq;
using Aop.Api.Request;

namespace DTcms.Web.tools
{
    /// <summary>
    /// api 的摘要说明
    /// </summary>
    public class api : IHttpHandler
    {
        private static TeeGonSdk.ITopClient Client = new TeeGonSdk.DefaultTopClient("https://api.teegon.com/", "bxkgovptblsbxe4zyi7ixbdh", "ot5rhjgescrhcewcex65uamkcypaaxfu");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            string action = context.Request.Params["action"];
            switch (action)
            {
                case "downloadorderlist":
                    DownloadOrderList(context);
                    break;
                case "downloadorderlist2":
                    DownloadOrderList2(context);
                    break;
                case "downloadorderlistfordispatch":
                    DownloadOrderListForDispatch(context);
                    break;
                case "login":
                    Login(context);
                    break;
                case "post":
                    Post(context);
                    break;
                case "scpost":
                    ScPost(context);
                    break;
                case "stj":
                    Stj(context);
                    break;
                case "waiters":
                    Waiter(context);
                    break;
                case "cashiers":
                    Cashier(context);
                    break;
                case "uploadofflineorder":
                    UploadOfflineOrder(context);
                    break;
                case "uploadofflineorderfortest":
                    UploadOfflineOrderForTest(context);
                    break;
                case "switchgoodlock":
                    SwitchGoodLock(context);
                    break;
                case "switchgoodguqing":
                    SwitchGoodGuQing(context);
                    break;
                case "getgoodslist":
                    GetGoodsList(context);
                    break;
                case "getgoodcombolist":
                    GetGoodComboList(context);
                    break;
                case "getNewgoodlist":
                    GetNewGoodList(context);
                    break;
                case "getbacklist":
                    GetBackList(context);
                    break;
                case "downconfirm":
                    BackDownConfirm(context);
                    break;
                case "taste":
                    GetTasteList(context);
                    break;
                case "condition":
                    GetConditionList(context);
                    break;
                case "getaudit":
                    GetAudit(context);
                    break;
                case "getpayresult":
                    GetPayResult(context);
                    break;
                case "calltakeout":
                    CallTakeOut(context);
                    break;
                case "switcharea":
                    SwitchArea(context);
                    break;
                case "checkofflinepaystate":
                    CheckOfflinePayState(context);
                    break;
            }
        }

        private void DownloadOrderList(HttpContext context)
        {
            string username = context.Request.Params["username"];
            //DTcms.Common.Log.Info("downloadlist:username_" + username);
            DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
            if (dtUser.Rows.Count == 0)
            {
                return;
            }

            //DTcms.Common.Log.Info("downloadlist:areaids_" + dtUser.Rows[0]["orderarea"].ToString());
            DTcms.BLL.orders bllOrder = new BLL.orders();
            DataTable dtOrder = bllOrder.GetList(30, " ((status=1 and (payment_id=1 or payment_id=2) and payment_status in (1,2)) or (status=1 and (payment_id in (3,5,6,7,8)) and payment_status=2)) and area_id in (" + dtUser.Rows[0]["orderarea"].ToString() + ")", " id asc").Tables[0];
            string ret = string.Empty;
            DTcms.Model.orders order = null;
            string orderdetail = string.Empty;
            foreach (DataRow item in dtOrder.Rows)
            {
                orderdetail = string.Empty;
                order = bllOrder.GetModel(int.Parse(item["id"].ToString()));
                if (order.order_goods != null)
                {
                    foreach (var detail in order.order_goods)
                    {
                        if (detail.type == "combo")
                        {
                            orderdetail += "\r" + detail.goods_name;
                            orderdetail += " " + detail.quantity + "份 " + (detail.goods_price * detail.quantity) + "元";
                            if (!string.IsNullOrEmpty(detail.subgoodsid))
                            {
                                foreach (var sub in detail.subgoodsid.Split('†'))
                                {
                                    if (sub.Split('‡')[0] == "combo")
                                    {
                                        orderdetail += "\r*" + sub.Split('‡')[2] + " ";
                                    }
                                    else if (sub.Split('‡')[0] == "taste")
                                    {
                                        orderdetail += "\r*" + "口味：" + sub.Split('‡')[2];
                                    }
                                }
                            }

                        }
                        else if (detail.type == "one")
                        {
                            orderdetail += "\r" + detail.goods_name;
                            orderdetail += " " + detail.quantity + "份 " + (detail.goods_price * detail.quantity) + "元";
                            if (!string.IsNullOrEmpty(detail.subgoodsid))
                            {
                                orderdetail += "\r*口味：" + detail.subgoodsid.Split('‡')[2];
                            }

                        }
                    }
                }

                orderdetail += order.real_freight > 0 ? "\r外送费:" + order.real_freight : "";
                ret += order.address + "♂"
                    + "单号:" + order.order_no
                    + "\r" + order.email
                    + "\r" + orderdetail
                    + "\r" + order.message
                    + "\r" + "\r" + (order.order_amount > 0 ? order.order_amount.ToString() : "")
                    + "\r" + order.address
                    + "\r" + order.telphone
                    + "\r" + order.add_time.ToString() + "\r"
                    + "♂" + "\r" + order.order_amount + "   " + order.add_time.ToString("HH:mm:ss")
                    + "♂" + order.id
                    + "♂" + order.payment_status
                    + "♂" + order.order_no
                    + "♀";
            }
            context.Response.Write(ret);
            //DTcms.Common.Log.Info("downloadlist:" + ret);
        }

        private void DownloadOrderList2(HttpContext context)
        {
            string username = context.Request.Params["username"];
            //DTcms.Common.Log.Info("downloadlist2:username_" + username);
            DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
            if (dtUser.Rows.Count == 0)
            {
                return;
            }

            //DTcms.Common.Log.Info("downloadlist2:areaids_" + dtUser.Rows[0]["orderarea"].ToString());
            DTcms.BLL.orders bllOrder = new BLL.orders();
            DataTable dtOrder = bllOrder.GetList(30, " ((status=1 and (payment_id=1 or payment_id=2) and payment_status in (1,2)) or (status=1 and (payment_id in (3,5,6,7,8)) and payment_status=2)) and area_id in (" + dtUser.Rows[0]["orderarea"].ToString() + ")", " id asc").Tables[0];
            string ret = string.Empty;
            DTcms.Model.orders order = null;
            string orderdetail = string.Empty;
            string subdetail = string.Empty;
            string taste = string.Empty;
            List<DownLoadOrder> listOrder = new List<DownLoadOrder>();
            foreach (DataRow item in dtOrder.Rows)
            {
                order = bllOrder.GetModel(int.Parse(item["id"].ToString()));
                orderdetail = string.Empty;
                subdetail = string.Empty;
                if (order.order_goods != null)
                {
                    foreach (var detail in order.order_goods)
                    {
                        subdetail = string.Empty;
                        taste = string.Empty;
                        if (!string.IsNullOrEmpty(detail.subgoodsid))
                        {
                            if (detail.type == "combo")
                            {
                                foreach (var sub in detail.subgoodsid.Split('†'))
                                {
                                    if (sub.Split('‡')[0] == "combo")
                                    {
                                        subdetail += "    *" + sub.Split('‡')[2] + ((sub.Split('‡').Length == 4 || sub.Split('‡').Length == 5) ? "*" + sub.Split('‡')[3] : "") + "\n";
                                    }
                                    else if (sub.Split('‡')[0] == "taste")
                                    {
                                        taste = sub.Split('‡')[2];
                                    }
                                }
                            }
                            else if (detail.type == "one")
                            {
                                taste = detail.subgoodsid.Split('‡')[2];
                            }
                        }
                        orderdetail += string.Format("{0}‡{1}{3}‡{2}\n"
                            , detail.quantity, detail.goods_name, detail.quantity * detail.goods_price
                            , !string.IsNullOrEmpty(taste) ? "*" + taste : "");
                        orderdetail += subdetail;
                    }

                }
                listOrder.Add(new DownLoadOrder()
                {
                    addr = order.address,
                    email = order.email,
                    liuyan = order.message,
                    tel = order.telphone,
                    time = order.add_time.ToString("MM-dd HH:mm"),
                    totalprice = order.order_amount,
                    order = orderdetail,
                    bianhao = order.order_no,
                    orderid = order.id.ToString(),
                    ispaid = order.payment_status.ToString()
                });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ret = serializer.Serialize(listOrder);
            context.Response.Write(ret);
            //DTcms.Common.Log.Info("downloadlist:" + ret);
        }

        private void DownloadOrderListForDispatch(HttpContext context)
        {
            string username = context.Request.Params["username"];
            //DTcms.Common.Log.Info("downloadlistfordispatch:username_" + username);
            DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
            if (dtUser.Rows.Count == 0)
            {
                return;
            }

            //DTcms.Common.Log.Info("downloadlistfordispatch:areaids_" + dtUser.Rows[0]["orderarea"].ToString());
            DTcms.BLL.orders bllOrder = new BLL.orders();
            DataTable dtOrder = bllOrder.GetList(30,
                " ((is_download=0 and (payment_id=1 or payment_id=2)) or (is_download=0 and (payment_id in (3,5,6,7,8,9)) and payment_status=2))"
                + " and OrderType='线下订单' and status in (1,2,3) "
                + " and area_id in (" + dtUser.Rows[0]["orderarea"].ToString() + ")", " id asc").Tables[0];
            string ret = string.Empty;
            DTcms.Model.orders order = null;
            string orderdetail = string.Empty;
            string subdetail = string.Empty;
            string taste = string.Empty;
            List<DownLoadOrder> listOrder = new List<DownLoadOrder>();
            foreach (DataRow item in dtOrder.Rows)
            {
                order = bllOrder.GetModel(int.Parse(item["id"].ToString()));
                orderdetail = string.Empty;
                subdetail = string.Empty;
                if (order.order_goods != null)
                {
                    foreach (var detail in order.order_goods)
                    {
                        subdetail = string.Empty;
                        taste = string.Empty;
                        if (!string.IsNullOrEmpty(detail.subgoodsid))
                        {
                            if (detail.type == "combo")
                            {
                                foreach (var sub in detail.subgoodsid.Split('†'))
                                {
                                    if (sub.Split('‡')[0] == "combo")
                                    {
                                        subdetail += "    *" + sub.Split('‡')[2] + ((sub.Split('‡').Length == 4 || sub.Split('‡').Length == 5) && !string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : "") + "\n";
                                    }
                                    else if (sub.Split('‡')[0] == "taste")
                                    {
                                        taste = sub.Split('‡')[2];
                                    }
                                }
                            }
                            else if (detail.type == "one")
                            {
                                taste = detail.subgoodsid.Split('‡')[2];
                            }
                        }
                        orderdetail += string.Format("{0}   {1}{3}  {2}\n"
                            , detail.quantity, detail.goods_name, detail.quantity * detail.goods_price
                            , !string.IsNullOrEmpty(taste) ? "/" + taste : "");
                        orderdetail += subdetail;
                    }

                }
                listOrder.Add(new DownLoadOrder()
                {
                    addr = order.address,
                    email = order.email,
                    liuyan = order.message,
                    tel = order.telphone,
                    time = order.add_time.ToString("MM-dd HH:mm"),
                    totalprice = order.order_amount,
                    order = orderdetail,
                    bianhao = (order.order_no != null && order.order_no.Length > 3 ? order.order_no.Substring(order.order_no.Length - 3) : ""),
                    orderid = order.id.ToString(),
                    ispaid = order.payment_status.ToString(),
                    takeout = order.takeout.ToString()
                });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ret = serializer.Serialize(listOrder);
            context.Response.Write(ret);
            //DTcms.Common.Log.Info("downloadlist:" + ret);
        }

        private void Login(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            Common.Log.Info("Login:username_" + username + " password_" + password);
            DTcms.Model.manager bo = new DTcms.BLL.manager().GetModel(username.ToLower(), DTcms.Common.DESEncrypt.Encrypt(password.ToLower()));
            if (bo == null)
            {
                context.Response.Write("err");
                return;
            }

            context.Response.Write(bo.goodsarea);
        }

        private void Post(HttpContext context)
        {
            if (!VerifyAccount(context))
            {
                return;
            }
            string ids = context.Request.Params["ids"];
            DTcms.BLL.orders bll = new BLL.orders();
            DataTable dt = bll.GetList(0, " id in (" + ids + ") and (status=1 or is_download=0)", " id asc").Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                if (item["status"].ToString() == "1")
                {
                    bll.UpdateField(int.Parse(item["id"].ToString()), "status=2,is_download=1,confirm_time='" + DateTime.Now + "'");
                }
                else if (item["status"].ToString() == "3")
                {
                    bll.UpdateField(int.Parse(item["id"].ToString()), "is_download=1,confirm_time='" + DateTime.Now + "'");
                }
            }
            context.Response.Write("ok");
            //DTcms.Common.Log.Info("post:" + ids);
        }

        private bool VerifyAccount(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            DTcms.Model.manager bo = new DTcms.BLL.manager().GetModel(username, DTcms.Common.DESEncrypt.Encrypt(password));
            if (bo == null)
            {
                context.Response.Write("err");
                return false;
            }
            return true;
        }

        private void ScPost(HttpContext context)
        {
            //DTcms.Common.Log.Info("scpost:username_" + context.Request.Params["username"] + "_password_" + context.Request.Params["password"]);

            if (!VerifyAccount(context))
            {
                return;
            }
            string sid = context.Request.Params["sid"];
            string orderid = context.Request.Params["orderid"];
            //DTcms.Common.Log.Info("scpost:sid_" + sid + "_orderid_" + orderid);
            orderid = orderid.Trim(',');
            BookingFood.Model.bf_worker worker = new BookingFood.BLL.bf_worker().GetModel(int.Parse(sid));
            if (string.IsNullOrEmpty(orderid))
            {
                context.Response.Write("ok");
                return;
            }
            DTcms.BLL.orders bll = new BLL.orders();
            DTcms.Model.orders bo = null;
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            string accessToken = string.Empty;
            for (int y = 0; y < orderid.Split(',').Length; y++)
            {
                bo = bll.GetModel(int.Parse(orderid.Split(',')[y]));
                if (bo == null) continue;
                bo.worker_id = worker.Id;
                bo.worker_name = worker.Title;

                //发送微信模板消息
                if (worker.OperateType == 2 && bo.distribution_status == 1)//超范围
                {
                    #region 超范围

                    if (bo.OrderType == "微信")
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_range_out_first));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.add_time.ToString("yyyy-MM-dd HH:mm:ss")));
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.order_amount.ToString("0.00")));
                        string orderdetail = string.Empty;
                        foreach (var item in bo.order_goods)
                        {
                            orderdetail += item.goods_name;
                            if (!string.IsNullOrEmpty(item.subgoodsid) && item.subgoodsid.Length > 0)
                            {
                                orderdetail += "(";
                                for (int i = 0; i < item.subgoodsid.Split('†').Length; i++)
                                {
                                    orderdetail += item.subgoodsid.Split('†')[i].Split('‡')[2] + " ";
                                }
                                orderdetail += ")";
                            }
                            orderdetail += "，";
                        }
                        tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(orderdetail.TrimEnd(',')));
                        tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("已取消"));
                        tempData.Add("keyword5", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("超出配送范围"));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_range_out_remark));
                        switch (bo.which_mp)
                        {
                            case "master":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "dyZz3EJi4IhaQ9JH_BuD8txU6S50KIO31jVAqtIAV54"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                            case "slave":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "eIWZHt9mwnjSOgPP7sTXYw58btBc5zPn4vCdOk8xR2s"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                        }

                    }
                    else if (bo.OrderType == "网页")
                    {
                        Model.mail_template mailModel = new BLL.mail_template().GetModel("rangeout");
                        if (mailModel == null) return;
                        string bodytext = string.Format("订单编号：{0}", bo.order_no);
                        DTMail.sendMail(siteConfig.emailstmp,
                                siteConfig.emailusername,
                                DESEncrypt.Decrypt(siteConfig.emailpassword),
                                siteConfig.emailnickname,
                                siteConfig.emailfrom,
                                bo.email,
                                mailModel.maill_title, mailModel.content.Replace("{innercontent}", bodytext));
                    }
                    #endregion
                }
                else if (worker.OperateType == 3 && bo.distribution_status == 1)//订单区域错误
                {
                    #region 当前餐厅忙

                    if (bo.OrderType == "微信")
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_error_area_first));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.add_time.ToString("yyyy-MM-dd HH:mm:ss")));
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.order_amount.ToString("0.00")));
                        string orderdetail = string.Empty;
                        foreach (var item in bo.order_goods)
                        {
                            orderdetail += item.goods_name;
                            if (!string.IsNullOrEmpty(item.subgoodsid) && item.subgoodsid.Length > 0)
                            {
                                orderdetail += "(";
                                for (int i = 0; i < item.subgoodsid.Split('†').Length; i++)
                                {
                                    orderdetail += item.subgoodsid.Split('†')[i].Split('‡')[2] + " ";
                                }
                                orderdetail += ")";
                            }
                            orderdetail += "，";
                        }
                        tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(orderdetail.TrimEnd(',')));
                        tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("已取消"));
                        tempData.Add("keyword5", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("当前餐厅忙"));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_error_area_remark));
                        switch (bo.which_mp)
                        {
                            case "master":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "dyZz3EJi4IhaQ9JH_BuD8txU6S50KIO31jVAqtIAV54"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                            case "slave":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "eIWZHt9mwnjSOgPP7sTXYw58btBc5zPn4vCdOk8xR2s"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                        }

                    }
                    else if (bo.OrderType == "网页")
                    {
                        Model.mail_template mailModel = new BLL.mail_template().GetModel("errorarea");
                        if (mailModel == null) return;
                        string bodytext = string.Format("订单编号：{0}", bo.order_no);
                        DTMail.sendMail(siteConfig.emailstmp,
                                siteConfig.emailusername,
                                DESEncrypt.Decrypt(siteConfig.emailpassword),
                                siteConfig.emailnickname,
                                siteConfig.emailfrom,
                                bo.email,
                                mailModel.maill_title, mailModel.content.Replace("{innercontent}", bodytext));
                    }
                    #endregion
                }
                else if (worker.OperateType == 5 && bo.distribution_status == 1)//订单区域错误
                {
                    #region 订单区域错误

                    if (bo.OrderType == "微信")
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_much_condition_first));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.add_time.ToString("yyyy-MM-dd HH:mm:ss")));
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.order_amount.ToString("0.00")));
                        string orderdetail = string.Empty;
                        foreach (var item in bo.order_goods)
                        {
                            orderdetail += item.goods_name;
                            if (!string.IsNullOrEmpty(item.subgoodsid) && item.subgoodsid.Length > 0)
                            {
                                orderdetail += "(";
                                for (int i = 0; i < item.subgoodsid.Split('†').Length; i++)
                                {
                                    orderdetail += item.subgoodsid.Split('†')[i].Split('‡')[2] + " ";
                                }
                                orderdetail += ")";
                            }
                            orderdetail += "，";
                        }
                        tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(orderdetail.TrimEnd(',')));
                        tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("已取消"));
                        tempData.Add("keyword5", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("条件无法满足"));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_much_condition_remark));
                        switch (bo.which_mp)
                        {
                            case "master":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "dyZz3EJi4IhaQ9JH_BuD8txU6S50KIO31jVAqtIAV54"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                            case "slave":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "eIWZHt9mwnjSOgPP7sTXYw58btBc5zPn4vCdOk8xR2s"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                        }

                    }
                    else if (bo.OrderType == "网页")
                    {
                        Model.mail_template mailModel = new BLL.mail_template().GetModel("muchcondition");
                        if (mailModel == null) return;
                        string bodytext = string.Format("订单编号：{0}", bo.order_no);
                        DTMail.sendMail(siteConfig.emailstmp,
                                siteConfig.emailusername,
                                DESEncrypt.Decrypt(siteConfig.emailpassword),
                                siteConfig.emailnickname,
                                siteConfig.emailfrom,
                                bo.email,
                                mailModel.maill_title, mailModel.content.Replace("{innercontent}", bodytext));
                    }
                    #endregion
                }
                else if (worker.OperateType == 0 && bo.distribution_status == 1)//正常发送派送通知
                {
                    #region 正常发送订单派送通知

                    if (bo.OrderType == "微信")
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_distribution_takeout_first));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.order_no));
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.accept_name));
                        tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.address));
                        tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(new BLL.payment().GetTitle(bo.payment_id) + " " + bo.order_amount.ToString("0.00")));
                        tempData.Add("keyword5", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_distribution_takeout_keyword5));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_distribution_takeout_remark));
                        switch (bo.which_mp)
                        {
                            case "master":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "O09v_izQ8GLYUwW2BLWJAtJjQ6xKhZikBXYrFnv5yO8"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                            case "slave":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "GbOae-SJRuSZA1pa1E0IC0Ung284bpc_ZrNMXEGOfuM"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                        }

                    }
                    else if (bo.OrderType == "网页")
                    {
                        Model.mail_template mailModel = new BLL.mail_template().GetModel("dispatch");
                        if (mailModel == null) return;
                        string bodytext = string.Format("订单编号：{0}<br/>付款方式：{1}({2})", bo.order_no, bo.order_amount.ToString("0.00"), new BLL.payment().GetTitle(bo.payment_id));
                        DTMail.sendMail(siteConfig.emailstmp,
                                siteConfig.emailusername,
                                DESEncrypt.Decrypt(siteConfig.emailpassword),
                                siteConfig.emailnickname,
                                siteConfig.emailfrom,
                                bo.email,
                                mailModel.maill_title, mailModel.content.Replace("{innercontent}", bodytext));
                    }
                    #endregion
                }
                else if (worker.OperateType == 4 && bo.distribution_status == 1)//正常发送派送通知
                {
                    #region 通知/少送已成功处理

                    if (bo.OrderType == "通知" && (string.Equals(bo.which_mp, "master") || string.Equals(bo.which_mp, "slave")))
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("已处理。有错少送，会第一时间补送。催单已电话骑士，请稍等！"));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.webname));
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.message));
                        tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.area_title));
                        tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.ToString("HH:mm:ss")));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(""));
                        switch (bo.which_mp)
                        {
                            case "master":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "2EfVaoRij1UCHxxZQCG1GBgrQwG8ZSbYkQMD_LumrlQ"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                            case "slave":
                                accessToken = Senparc.Weixin.MP.CommonAPIs
                                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                try
                                {
                                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "uZN_a9jPvIU3gyGTJYDulXIAwnGMUNOwDH7XMVIrglE"
                                    , "#173177", "", tempData);
                                }
                                catch (Exception) { }
                                break;
                        }

                    }
                    else if (bo.OrderType == "通知" && string.IsNullOrEmpty(bo.which_mp))
                    {
                        Model.mail_template mailModel = new BLL.mail_template().GetModel("receive");
                        if (mailModel == null) return;
                        string bodytext = string.Format("已处理。有错少送，会第一时间补送。催单已电话骑士，请稍等！<br/>内容：{0}", bo.message);
                        DTMail.sendMail(siteConfig.emailstmp,
                                siteConfig.emailusername,
                                DESEncrypt.Decrypt(siteConfig.emailpassword),
                                siteConfig.emailnickname,
                                siteConfig.emailfrom,
                                bo.email,
                                mailModel.maill_title, mailModel.content.Replace("{innercontent}", bodytext));
                    }
                    #endregion
                }
                else if (worker.OperateType == 6 && bo.distribution_status == 1)//外带单子通知
                {
                    if (bo.OrderType == "线下订单" && bo.takeout == 2)
                    {
                        Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_call_takeout_first));
                        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(bo.MpForHere));
                        string tempMessage = string.Empty;
                        foreach (var item in bo.order_goods)
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
                        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_call_takeout_remark));
                        accessToken = string.Empty;
                        accessToken = Senparc.Weixin.MP.CommonAPIs
                                        .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                        try
                        {
                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, bo.user_name, "vjB5zJBDeQaFRA5Ep1zDkT8cXQ_E3a-XMVH1_mKTyO4"
                            , "#173177", "", tempData);
                        }
                        catch (Exception ex)
                        {
                            Log.Info(ex.Message);
                        }
                    }
                }
                bo.distribution_status = 2;
                bo.distribution_time = DateTime.Now;
                bll.Update(bo);
            }

            context.Response.Write("ok");

        }

        private void Stj(HttpContext context)
        {
            string suid = context.Request.Params["suid"];
            DataTable dt = new BLL.orders().GetList(0, " worker_id=" + suid + " and (distribution_time between '"
                    + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and '"
                    + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59')", " id asc").Tables[0];
            decimal total = 0;
            foreach (DataRow item in dt.Rows)
            {
                total += decimal.Parse(item["order_amount"].ToString());
            }
            context.Response.Write(total.ToString() + "(" + dt.Rows.Count.ToString() + ")");
        }

        private void Waiter(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            DTcms.Model.manager bo = new DTcms.BLL.manager().GetModel(username, DTcms.Common.DESEncrypt.Encrypt(password));
            if (bo == null)
            {
                context.Response.Write("err");
                return;
            }
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            List<BookingFood.Model.bf_area> listArea = bllArea.GetModelList("Id In (" + bo.orderarea + ")");
            string areas = string.Join(",", listArea.Select(s => s.ChangeOrderArea).ToArray());
            string changeares = "[]";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (!string.IsNullOrEmpty(areas))
            {
                listArea =
                bllArea.GetModelList("Id IN (" + areas + ")");
                var changearea = listArea.Select(s => new { s.Id, s.Title });
                changeares = serializer.Serialize(changearea);
            }

            List<BookingFood.Model.bf_worker> listworker = new BookingFood.BLL.bf_worker().GetModelList(
                " WorkerType IN ('长工','钟点工') AND Id IN (SELECT baw.WorkerId FROM bf_area_worker baw WHERE baw.AreaId IN (" + bo.waiterarea + ")) Order By SortId Asc");
            var func = listworker.Where(s => s.OperateType == 2 || s.OperateType == 3 || s.OperateType == 4 || s.OperateType == 5).Select(s => new { s.Id, s.Title, s.Telphone });
            var worker = listworker.Where(s => s.OperateType != 2 && s.OperateType != 3 && s.OperateType != 4 && s.OperateType != 5).Select(s => new { s.Id, s.Title, s.Telphone });

            string json = string.Format("{{\"func\":{0},\"worker\":{1},\"changearea\":{2}}}", serializer.Serialize(func), serializer.Serialize(worker), changeares);
            context.Response.Write(json);
        }

        private void Cashier(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            DTcms.Model.manager bo = new DTcms.BLL.manager().GetModel(username, DTcms.Common.DESEncrypt.Encrypt(password));
            if (bo == null)
            {
                context.Response.Write("err");
                return;
            }

            List<BookingFood.Model.bf_worker> listworker = new BookingFood.BLL.bf_worker().GetModelList(
                " WorkerType='收银员' AND Id IN (SELECT baw.WorkerId FROM bf_area_worker baw WHERE baw.AreaId IN (" + bo.waiterarea + ")) Order By SortId Asc");
            string rtn = string.Empty;
            foreach (var item in listworker)
            {
                rtn += item.Id.ToString() + "♂" + item.Title.Trim() + "♂" + item.Telphone.Trim() + "♀";
            }
            context.Response.Write(rtn);
        }

        private void UploadOfflineOrder(HttpContext context)
        {
            string arg = context.Request.Params["orderinfo"];
            //Common.Log.Info("UploadOfficelineOrder:"+arg);
            arg = arg.Replace('\"', '"');
            arg = HttpUtility.UrlDecode(arg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            OfflineOrder offlineorder = serializer.Deserialize<OfflineOrder>(arg);
            Model.orders order = new Model.orders();
            order.accept_name = "线下订单";
            order.add_time = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(offlineorder.time));
            order.confirm_time = order.add_time;
            order.complete_time = order.add_time;
            order.address = offlineorder.num;
            BookingFood.Model.bf_area area = new BookingFood.BLL.bf_area().GetModel(int.Parse(offlineorder.area));
            order.area_id = area.Id;
            order.area_title = area.Title;
            order.distribution_id = 1;
            order.distribution_status = 2;
            order.order_amount = decimal.Parse(offlineorder.total);
            order.order_no = DTcms.Common.Utils.GetOrderNumber(); //订单号
            order.OrderType = "线下订单";
            order.payable_amount = order.order_amount;
            order.payable_freight = 0;
            order.payment_fee = 0;
            order.payment_id = 1;
            order.payment_status = 2;
            order.real_amount = order.order_amount;
            order.real_freight = 0;
            order.status = 3;
            order.user_id = 0;
            order.worker_id = 0;
            List<Model.order_goods> ordergoods = new List<Model.order_goods>();
            Model.article_goods good = null;
            BLL.article bllArticle = new BLL.article();
            BLL.category bllCategory = new BLL.category();
            foreach (var item in offlineorder.order)
            {
                good = bllArticle.GetGoodsModel(int.Parse(item.id));
                if (good == null) return;
                ordergoods.Add(new Model.order_goods()
                {
                    goods_id = good.id,
                    goods_name = good.title,
                    goods_price = decimal.Parse(item.price) / int.Parse(item.count)
                    ,
                    point = 0,
                    quantity = int.Parse(item.count),
                    real_price = decimal.Parse(item.price) / int.Parse(item.count),
                    type = "one",
                    subgoodsid = "taste‡" + item.taste + "‡" + item.taste,
                    category_title = bllCategory.GetModel(good.category_id).title
                });
            }
            order.order_goods = ordergoods;
            int result = 0;
            try
            {
                result = new BLL.orders().Add(order);
            }
            catch (Exception ex)
            {
                Common.Log.Info(ex.Message + ex.InnerException.Message);
                context.Response.Write("error");
                return;
            }
            if (result != 0)
            {
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in order.order_goods)
                {
                    goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                    back = new BookingFood.Model.bf_back_door()
                    {
                        OrderId = result,
                        GoodsCount = item.quantity,
                        CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                        AreaId = order.area_id,
                        IsDown = 0,
                        Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                        Freight = "堂吃"
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
                BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                foreach (var item in listBack)
                {
                    bllBack.Add(item);
                }
            }
            context.Response.Write("ok");
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

        private void UploadOfflineOrderForTest(HttpContext context)
        {

            string arg = context.Request.Params["orderinfo"];
            string payment_title = context.Request.Params["payment"];
            string auth_code = context.Request.Params["auth_code"];
            try
            {
                string t = context.Request.Params["t"];
                DateTime now = ConvertUnixStampToDateTime(double.Parse(t) / 1000);
                if (now.AddMinutes(5) < DateTime.Now) return;
            }
            catch (Exception ex)
            {
                //Log.Info(HttpContext.Current.Request.Params.ToString());
                //Log.Info(ex.Message + " " + ex.StackTrace);
            }

            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            Common.Log.Info("UploadOfficelineOrderForTest:" + arg);
            Common.Log.Info("UploadOfficelineOrderForTest_Payment:" + payment_title + "," + auth_code);
            arg = arg.Replace('\"', '"');
            arg = HttpUtility.UrlDecode(arg);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            OfflineOrderForTest offlineorder = serializer.Deserialize<OfflineOrderForTest>(arg);
            BLL.orders bll = new BLL.orders();
            BookingFood.Model.bf_area area = new BookingFood.BLL.bf_area().GetModel(int.Parse(offlineorder.area));
            //检测订单号是否重复
            if (bll.GetCount(" order_no='" + offlineorder.order_no + "' and area_id=" + offlineorder.area) > 0)
            {
                Model.orders modelOld = bll.GetModel(offlineorder.order_no);
                if (modelOld.payment_status == 2)
                {
                    context.Response.Write("ok");
                    return;
                }
                if (string.IsNullOrEmpty(payment_title))
                {
                    payment_title = "cash";
                }
                switch (payment_title)
                {
                    case "alipay":
                        if (string.IsNullOrEmpty(auth_code))
                        {
                            context.Response.Write("付款条码为空");
                            return;
                        }
                        bll.UpdateField(modelOld.id, "payment_id=7");
                        modelOld.payment_id = 7;
                        break;
                    case "mppay":
                        if (string.IsNullOrEmpty(auth_code))
                        {
                            context.Response.Write("付款条码为空");
                            return;
                        }
                        bll.UpdateField(modelOld.id, "payment_id=8");
                        modelOld.payment_id = 8;
                        break;
                    case "cash":
                        bll.UpdateField(modelOld.id, "payment_id=1,payment_status=2");
                        modelOld.payment_id = 1;
                        break;
                }
                string pay_result = string.Empty;
                switch (modelOld.payment_id)
                {
                    case 7:
                        pay_result = API.Payment.Alipay_ScanCode.AlipayF2F.PayOrder(modelOld.order_no, auth_code, modelOld.order_amount.ToString(), area.Title, area.Id.ToString());
                        if (!string.Equals(pay_result, "支付成功"))
                        {
                            context.Response.Write(pay_result);
                            Log.Info("pay_result:" + pay_result);
                            return;
                        }
                        bll.UpdateField(modelOld.id, "payment_status=2");
                        break;
                    case 8:

                        if (siteConfig.RunTigoon == 0)
                        {
                            pay_result = API.Payment.MpMicroPay.MicroPay.Run(modelOld.order_no, auth_code, Convert.ToInt16(modelOld.order_amount * 100).ToString(), area.Title);
                            if (!string.Equals(pay_result, "支付成功"))
                            {
                                context.Response.Write(pay_result);
                                Log.Info("pay_result:" + pay_result);
                                return;
                            }
                            bll.UpdateField(modelOld.id, "payment_status=2");
                        }
                        else
                        {
                            ChargeRequest<string> get_req = new ChargeRequest<string>();
                            get_req.amount = modelOld.order_amount;
                            get_req.out_order_no = modelOld.order_no;
                            get_req.pay_channel = "wxpay_p2p";
                            get_req.channel = "barcode_pay";
#if DEBUG
                            get_req.ip = "119.180.116.79";
#else
                                                    get_req.ip = context.Request.UserHostAddress;
#endif
                            get_req.subject = area.Title;
                            get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            get_req.device_id = System.Net.Dns.GetHostName();
                            get_req.auth_code = auth_code;
                            get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_jsapi/feedback2.aspx";
                            ChargeResponse<string> get_rsp = Client.Execute(get_req);
                            if (get_rsp.IsError)
                            {
                                context.Response.Write(get_rsp.ErrorMsg);
                                return;
                            }
                            ChargeGetRequest tg_result = new ChargeGetRequest();
                            tg_result.id = get_rsp.Result.Id;
                            for (int i = 0; i < 10; i++)
                            {
                                ChargeGetResponse tg_pay_result = Client.Execute(tg_result);
                                if (tg_pay_result.Result.Paid)
                                {
                                    Log.Info("订单号:" + modelOld.order_no + " 返回的支付结果:" + tg_pay_result.Result.Paid);
                                    context.Response.Write("支付成功");
                                    bll.UpdateField(modelOld.id, "payment_status=2");
                                    break;
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                        }
                        break;
                    case 1:

                        break;
                }
                #region 后厨
                BLL.article bllArticle = new BLL.article();
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in modelOld.order_goods)
                {
                    if (item.type == "one")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = modelOld.id,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = modelOld.area_id,
                            IsDown = 0,
                            Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                            Freight = "堂吃"
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
                            string[] subs = sub.Split('‡');
                            goodsModel = bllArticle.GetGoodsModel(int.Parse(subs[1]));
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = modelOld.id,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = modelOld.area_id,
                                IsDown = 0,
                                Taste = (subs.Length == 4 || subs.Length == 5) ? subs[3] : "",
                                Freight = "堂吃"
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
            }
            else
            {
                #region 组装订单信息
                if (string.IsNullOrEmpty(payment_title))
                {
                    //context.Response.Write("付款方式为空");
                    //return;
                    payment_title = "cash";
                }
                Model.orders order = new Model.orders();
                order.accept_name = "线下订单";
                order.add_time = DateTime.Now;
                order.confirm_time = order.add_time;
                order.complete_time = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(offlineorder.time));
                order.address = offlineorder.num;

                order.area_id = area.Id;
                order.area_title = area.Title;
                order.distribution_id = 1;
                order.distribution_status = 2;
                order.order_amount = decimal.Parse(offlineorder.total);
                //order.order_no = DTcms.Common.Utils.GetOrderNumber(); //订单号
                order.order_no = offlineorder.order_no;
                order.OrderType = "线下订单";
                order.payable_amount = order.order_amount;
                order.payable_freight = 0;
                order.payment_fee = 0;
                switch (payment_title)
                {
                    case "alipay":
                        if (string.IsNullOrEmpty(auth_code))
                        {
                            context.Response.Write("付款条码为空");
                            return;
                        }
                        order.payment_id = 7;
                        order.payment_status = 1;
                        break;
                    case "alipayscan":
                        order.payment_id = 7;
                        order.payment_status = 1;
                        break;
                    case "mppay":
                        if (string.IsNullOrEmpty(auth_code))
                        {
                            context.Response.Write("付款条码为空");
                            return;
                        }
                        order.payment_id = 8;
                        order.payment_status = 1;
                        break;
                    case "mppayscan":
                        order.payment_id = 8;
                        order.payment_status = 1;
                        break;
                    case "cash":
                        order.payment_id = 1;
                        order.payment_status = 2;
                        break;
                }
                order.real_amount = order.order_amount;
                order.real_freight = 0;
                order.status = 3;
                order.user_id = 0;
                order.worker_id = 0;
                #endregion
                #region 组织订单商品
                List<Model.order_goods> ordergoods = new List<Model.order_goods>();
                Model.article_goods good = null;
                BLL.article bllArticle = new BLL.article();
                BLL.category bllCategory = new BLL.category();
                BookingFood.BLL.bf_good_combo bllGoodCombo = new BookingFood.BLL.bf_good_combo();
                BookingFood.Model.bf_good_combo model = null;
                foreach (var item in offlineorder.order)
                {
                    if (item.type == "one")
                    {
                        good = bllArticle.GetGoodsModel(int.Parse(item.id));
                        if (good == null) return;
                        ordergoods.Add(new Model.order_goods()
                        {
                            goods_id = good.id,
                            goods_name = good.title,
                            goods_price = decimal.Parse(item.price) / int.Parse(item.count)
                            ,
                            point = 0,
                            quantity = int.Parse(item.count),
                            real_price = decimal.Parse(item.price) / int.Parse(item.count),
                            //type = "one",
                            //subgoodsid = "taste‡" + item.taste + "‡"+item.taste,
                            type = item.type,
                            subgoodsid = item.type == "one" ? "taste‡" + item.taste + "‡" + item.taste : item.subgoods,
                            category_title = bllCategory.GetModel(good.category_id).title,
                            condition_price = item.condition_price
                        });
                    }
                    else if (item.type == "combo")
                    {
                        model = bllGoodCombo.GetModel(int.Parse(item.id));
                        if (model == null) return;
                        string[] subgoods = item.subgoods.Split('†');
                        string _subgoodsid = string.Empty;
                        string _taste = string.Empty;
                        decimal _prive = 0;
                        foreach (var sub in subgoods)
                        {
                            if (string.IsNullOrEmpty(sub)) continue;
                            if (!string.IsNullOrEmpty(_subgoodsid))
                            {
                                _subgoodsid += "†";
                            }

                            if (sub.Split('‡')[0] == "taste")
                            {
                                _subgoodsid += sub;
                                _taste = sub.Split('‡')[1];
                            }
                            else
                            {
                                _subgoodsid += sub;
                                good = bllArticle.GetGoodsModel(Convert.ToInt32(sub.Split('‡')[1]));
                                _prive += good.sell_price;
                            }
                        }
                        ordergoods.Add(new Model.order_goods()
                        {
                            goods_id = model.Id,
                            goods_name = model.Title,
                            goods_price = _prive / int.Parse(item.count),
                            point = 0,
                            quantity = int.Parse(item.count),
                            real_price = _prive / int.Parse(item.count),
                            //type = "one",
                            //subgoodsid = "taste‡" + item.taste + "‡"+item.taste,
                            type = item.type,
                            subgoodsid = _subgoodsid,
                            category_title = bllCategory.GetModel(good.category_id).title
                        });
                    }
                }
                order.order_goods = ordergoods;
                #endregion
                int result = 0;
                try
                {
#if DEBUG
                    //result = new BLL.orders().Add(order);
#else
                    result = new BLL.orders().Add(order);
#endif

                }
                catch (Exception ex)
                {
                    Common.Log.Info(ex.Message + ex.InnerException.Message);
                    context.Response.Write("error");
                    return;
                }
                if (result != 0)
                {
                    string pay_result = string.Empty;
                    ChargeRequest<string> get_req = null;
                    ChargeResponse<string> get_rsp = null;
                    int srcPoint = 0, srcEndPoint = 0;
                    switch (payment_title)
                    {
                        case "alipay":
                            if (siteConfig.RunTigoon == 0)
                            {
                                pay_result = API.Payment.Alipay_ScanCode.AlipayF2F.PayOrder(order.order_no, auth_code, order.order_amount.ToString(), area.Title, area.Id.ToString());
                                if (!string.Equals(pay_result, "支付成功"))
                                {
                                    context.Response.Write(pay_result);
                                    Log.Info("pay_result:" + pay_result);
                                    return;
                                }
                                bll.UpdateField(result, "payment_status=2");
                            }
                            else
                            {
                                get_req = new ChargeRequest<string>();
                                get_req.amount = order.order_amount;
                                get_req.out_order_no = order.order_no;
                                //get_req.pay_channel = "wxpay_p2p";//alipay_p2p
                                get_req.channel = "barcode_pay";
#if DEBUG
                                get_req.ip = "119.180.116.79";
#else
                                                    get_req.ip = context.Request.UserHostAddress;
#endif
                                get_req.subject = area.Title;
                                get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                get_req.device_id = System.Net.Dns.GetHostName();
                                get_req.auth_code = auth_code;
                                get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_jsapi/feedback2.aspx";

                                try
                                {
                                    get_rsp = Client.Execute(get_req);
                                }
                                catch (Exception ex)
                                {
                                    Log.Info("订单号:" + order.order_no + "天工扫码支付报错" + ex.Message + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                bool isAsyncConfirm = false;
                                if (!get_rsp.Result.Paid)
                                {
                                    bool isPay = false;
                                    ChargePayStatusRequest<string> tg_result = null;
                                    try
                                    {
                                        tg_result = new ChargePayStatusRequest<string>();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Info("订单号:" + order.order_no + "天工扫码支付 查询支付结果报错" + ex.Message + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        return;
                                    }
                                    tg_result.charge_id = get_rsp.Result.Id;

                                    for (int i = 0; i < 30; i++)
                                    {
                                        ChargePayStatusResponse<string> tg_pay_result = Client.Execute(tg_result);
                                        if (tg_pay_result.Result.trade_status.ToUpper() == "TRADE_SUCCESS" || tg_pay_result.Result.trade_status.ToUpper() == "SUCCESS")
                                        {
                                            Log.Info("订单号:" + order.order_no + " 天工P2P轮询" + i + "次 支付成功" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            isPay = true;
                                            break;
                                        }
                                        else
                                        {
                                            Log.Info("订单号:" + order.order_no + " 天工P2P轮询" + i + "次" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            System.Threading.Thread.Sleep(2000);
                                        }
                                        //ChargePayStatusResponse<string> tg_pay_result = Client.Execute(tg_result);
                                    }
                                    if (isPay)
                                    {
                                        bll.UpdateField(result, "payment_status=2");
                                    }
                                    else
                                    {
                                        Log.Info("订单号:" + order.order_no + " 天工P2P支付失败" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        context.Response.Write("支付失败");
                                        return;
                                    }
                                }
                                else if (get_rsp.IsError)
                                {
                                    context.Response.Write(get_rsp.ErrorMsg);
                                    return;
                                }
                            }

                            break;
                        case "mppay":
                            if (siteConfig.RunTigoon == 0)
                            {
                                pay_result = API.Payment.MpMicroPay.MicroPay.Run(order.order_no, auth_code, Convert.ToInt16(order.order_amount * 100).ToString(), area.Title);
                                if (!string.Equals(pay_result, "支付成功"))
                                {
                                    context.Response.Write(pay_result);
                                    Log.Info("pay_result:" + pay_result);
                                    return;
                                }
                                bll.UpdateField(result, "payment_status=2");
                            }
                            else
                            {
                                get_req = new ChargeRequest<string>();
                                get_req.amount = order.order_amount;
                                get_req.out_order_no = order.order_no;
                                //get_req.pay_channel = "wxpay_p2p";//alipay_p2p
                                get_req.channel = "barcode_pay";
#if DEBUG
                                get_req.ip = "119.180.116.79";
#else
                                                    get_req.ip = context.Request.UserHostAddress;
#endif
                                get_req.subject = area.Title;
                                get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                get_req.device_id = System.Net.Dns.GetHostName();
                                get_req.auth_code = auth_code;
                                get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_jsapi/feedback2.aspx";

                                try
                                {
                                    get_rsp = Client.Execute(get_req);
                                }
                                catch (Exception ex)
                                {
                                    Log.Info("订单号:" + order.order_no + "天工扫码支付报错" + ex.Message + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                bool isAsyncConfirm = false;
                                if (!get_rsp.Result.Paid)
                                {
                                    bool isPay = false;
                                    ChargePayStatusRequest<string> tg_result = null;
                                    try
                                    {
                                        tg_result = new ChargePayStatusRequest<string>();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Info("订单号:" + order.order_no + "天工扫码支付 查询支付结果报错" + ex.Message + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        return;
                                    }
                                    tg_result.charge_id = get_rsp.Result.Id;

                                    for (int i = 0; i < 30; i++)
                                    {
                                        ChargePayStatusResponse<string> tg_pay_result = Client.Execute(tg_result);
                                        if (tg_pay_result.Result.trade_status.ToUpper() == "TRADE_SUCCESS" || tg_pay_result.Result.trade_status.ToUpper() == "SUCCESS")
                                        {
                                            Log.Info("订单号:" + order.order_no + " 天工P2P轮询" + i + "次 支付成功" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            isPay = true;
                                            break;
                                        }
                                        else
                                        {
                                            Log.Info("订单号:" + order.order_no + " 天工P2P轮询" + i + "次" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            System.Threading.Thread.Sleep(2000);
                                        }
                                        //ChargePayStatusResponse<string> tg_pay_result = Client.Execute(tg_result);
                                    }
                                    if (isPay)
                                    {
                                        bll.UpdateField(result, "payment_status=2");
                                    }
                                    else
                                    {
                                        Log.Info("订单号:" + order.order_no + " 天工P2P支付失败" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        context.Response.Write("支付失败");
                                        return;
                                    }
                                }
                                else if (get_rsp.IsError)
                                {
                                    context.Response.Write(get_rsp.ErrorMsg);
                                    return;
                                }

                            }
                            break;
                        case "mppayscan":
                            if (siteConfig.RunTigoon == 0)
                            {
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
                                packageReqHandler.SetParameter("body", siteConfig.webname + "外卖订单");
                                packageReqHandler.SetParameter("out_trade_no", order.order_no);     //商家订单号
                                packageReqHandler.SetParameter("total_fee", (order.order_amount * 100).ToString().Replace(".00", ""));                  //商品金额,以分为单位(money * 100).ToString()
#if DEBUG
                                packageReqHandler.SetParameter("spbill_create_ip", "112.238.70.141");   //用户的公网ip，不是商户服务器IP
#else
                packageReqHandler.SetParameter("spbill_create_ip", context.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
#endif
                                packageReqHandler.SetParameter("notify_url", "http://www.4008317417.cn/api/payment/mppay_native/feedback_offline.aspx");         //接收财付通通知的URL
                                packageReqHandler.SetParameter("trade_type", TenPayV3Type.NATIVE.ToString());                       //交易类型
                                packageReqHandler.SetParameter("product_id", result.ToString());                        //商品ID

                                string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                                packageReqHandler.SetParameter("sign", sign);                       //签名

                                string data = packageReqHandler.ParseXML();

                                var mppay_result = TenPayV3.Unifiedorder(data);
                                var res = XDocument.Parse(mppay_result);
                                string prepayId = string.Empty;
                                string code_url = string.Empty;
                                try
                                {
                                    prepayId = res.Element("xml").Element("prepay_id").Value;
                                    code_url = res.Element("xml").Element("code_url").Value;
                                }
                                catch (Exception)
                                {
                                    Log.Info(res.ToString());
                                }
                                context.Response.Write("{\"msg\":1,\"msgbox\":\"订单已成功提交！\",\"orderid\":" + result
                                    + ",\"orderno\":\"" + order.order_no + "\", \"code_url\":\"http://www.4008317417.cn/api/payment/mppay_native/getqr.ashx?code_url=" + Utils.UrlEncode(code_url) + "\"}");
                                return;
                            }
                            else
                            {
                                #region 天工

                                get_req = new ChargeRequest<string>();
                                get_req.amount = order.order_amount;
                                get_req.out_order_no = order.order_no;
                                get_req.pay_channel = "wxpay";
#if DEBUG
                                get_req.ip = "119.180.116.79";
#else
                                                        get_req.ip = context.Request.UserHostAddress;
#endif
                                get_req.subject = siteConfig.webname + "微信订单";
                                get_req.return_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback_offline.aspx";
                                get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback_offline.aspx";
                                get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                get_req.device_id = System.Net.Dns.GetHostName();
                                get_req.charge_type = "pay";
                                get_req.account_id = "main";
                                get_rsp = Client.Execute(get_req);
                                srcPoint = get_rsp.Result.Action.Params.IndexOf("src = \"") + 7;
                                srcEndPoint = get_rsp.Result.Action.Params.IndexOf("\"", srcPoint);

                                context.Response.Write("{\"msg\":1,\"msgbox\":\"订单已成功提交！\",\"orderid\":" + result
                                    + ",\"orderno\":\"" + order.order_no + "\",\"code_url\":\"" + get_rsp.Result.Action.Params.Substring(srcPoint, srcEndPoint - srcPoint) + "\"}");
                                return;
                                #endregion
                            }

                            break;
                        case "alipayscan":
                            if (siteConfig.RunTigoon == 0)
                            {
                                var BizContent = "{" +
                                     "\"out_trade_no\":\"" + order.order_no + "\"," +
                                     "\"seller_id\":\"" + DTcms.API.Payment.Alipay_ScanCode.Config.pid + "\"," +
                                     "\"total_amount\":" + (order.real_amount * 100).ToString().Replace(".00", "") + "," +
                                     "\"subject\":\"" + order.order_goods[0].goods_name + "等\"," +
                                     "\"goods_detail\":[{" +
                                     "\"goods_id\":\"" + order.order_goods[0].goods_id + "\"," +
                                     "\"goods_name\":\"" + order.order_goods[0].goods_name + "\"," +
                                     "\"quantity\":1," +
                                     "\"price\":" + order.order_goods[0].goods_price + "," +
                                     "\"body\":\"外卖小吃\"," +
                                     "}]," +
                                     "\"body\":\"" + siteConfig.webname + "外卖订单" + "\"," +
                                     "\"operator_id\":\"001\"," +
                                     "\"store_id\":\"" + order.area_title + "\"," +
                                     "\"disable_pay_channels\":\"pcredit,moneyFund,debitCardExpress\"," +
                                     "\"enable_pay_channels\":\"pcredit,moneyFund,debitCardExpress\"," +
                                     "\"terminal_id\":\"001\"," +
                                     "\"extend_params\":{" +
                                     "\"sys_service_provider_id\":\"" + DTcms.API.Payment.Alipay_ScanCode.Config.pid + "\"" +
                                     "}," +
                                     "\"timeout_express\":\"90m\"" +
                                     "}";
                                var data = API.Payment.Alipay_ScanCode.AlipayF2F.PreCreatePay(BizContent.Trim());
                                Log.Info("支付宝扫码请求参数:" + BizContent);
                                Log.Info("支付宝返回结果：" + data);
                                var res = XDocument.Parse(data);
                                string prepayId = string.Empty;
                                string code_url = string.Empty;
                                try
                                {
                                    prepayId = res.Element("xml").Element("out_trade_no").Value;
                                    code_url = res.Element("xml").Element("qr_code").Value;
                                }
                                catch (Exception)
                                {
                                    Log.Info(data);
                                }
                                context.Response.Write("{\"msg\":1,\"msgbox\":\"订单已成功提交！\",\"orderid\":" + result
                                    + ",\"orderno\":\"" + order.order_no + "\", \"code_url\":\"http://www.4008317417.cn/api/payment/mppay_native/getqr.ashx?code_url=" + Utils.UrlEncode(code_url) + "\"}");
                                return;
                            }
                            else
                            {
                                get_req = new ChargeRequest<string>();
                                get_req.amount = order.order_amount;
                                get_req.out_order_no = order.order_no;
                                get_req.pay_channel = "alipay";
#if DEBUG
                                get_req.ip = "119.180.116.79";
#else
                                                        get_req.ip = context.Request.UserHostAddress;
#endif
                                get_req.subject = siteConfig.webname + "微信订单";
                                get_req.return_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback_offline.aspx";
                                get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback_offline.aspx";
                                get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                get_req.device_id = System.Net.Dns.GetHostName();
                                get_req.charge_type = "pay";
                                get_req.account_id = "main";
                                get_rsp = Client.Execute(get_req);
                                srcPoint = get_rsp.Result.Action.Params.IndexOf("src = \"") + 7;
                                srcEndPoint = get_rsp.Result.Action.Params.IndexOf("\"", srcPoint);

                                context.Response.Write("{\"msg\":1,\"msgbox\":\"订单已成功提交！\",\"orderid\":" + result
                                    + ",\"orderno\":\"" + order.order_no + "\",\"code_url\":\"" + get_rsp.Result.Action.Params.Substring(srcPoint, srcEndPoint - srcPoint) + "\"}");
                                return;
                            }
                        case "cash":

                            break;
                    }
                    if (!string.Equals(payment_title, "mppayscan") && !string.Equals(payment_title, "alipayscan"))
                    {
                        #region 后厨
                        Model.article_goods goodsModel = null;
                        BookingFood.Model.bf_back_door back = null;
                        BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                        BookingFood.Model.bf_good_nickname nickModel = null;
                        List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                        foreach (var item in order.order_goods)
                        {
                            if (item.type == "one")
                            {
                                goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                                back = new BookingFood.Model.bf_back_door()
                                {
                                    OrderId = result,
                                    GoodsCount = item.quantity,
                                    CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                    AreaId = order.area_id,
                                    IsDown = 0,
                                    Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                                    Freight = "堂吃"
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
                                    string[] subs = sub.Split('‡');
                                    goodsModel = bllArticle.GetGoodsModel(int.Parse(subs[1]));
                                    back = new BookingFood.Model.bf_back_door()
                                    {
                                        OrderId = result,
                                        GoodsCount = item.quantity,
                                        CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                        AreaId = order.area_id,
                                        IsDown = 0,
                                        Taste = (subs.Length == 4 || subs.Length == 5) ? subs[3] : "",
                                        Freight = "堂吃"
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
                    }
                }

            }


            context.Response.Write("ok");
        }

        private void CheckOfflinePayState(HttpContext context)
        {
            string orderno = DTRequest.GetQueryString("orderno");
            DTcms.BLL.orders bll = new BLL.orders();
            Model.orders order = bll.GetModel(orderno);
            if (order.payment_status == 2)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("未支付");
            }
        }

        private void SwitchGoodLock(HttpContext context)
        {
            try
            {
                string id = context.Request.Params["id"];
                string islock = context.Request.Params["islock"];
                string username = context.Request.Params["username"];
                string type = context.Request.Params["type"];
                type = string.IsNullOrEmpty(type) ? "one" : type == "combo" ? "category" : type;
                DTcms.Common.Log.Info("SwitchGoodLock:" + context.Request.Url.Query);
                DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
                if (dtUser.Rows.Count == 0)
                {
                    return;
                }
                BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();
                List<BookingFood.Model.bf_area_article> listAreaArticle = bllAreaArticle.GetModelList(" Type='" + type + "' and ArticleId=" + id + " and AreaId IN (" + dtUser.Rows[0]["busyarea"].ToString() + ")");
                foreach (var item in listAreaArticle)
                {
                    if (item.IsLock == 0)
                    {
                        item.IsLock = 1;
                        bllAreaArticle.Update(item);
                    }
                    else
                    {
                        item.IsLock = 0;
                        bllAreaArticle.Update(item);
                    }
                }
                //foreach (var item in dtUser.Rows[0]["busyarea"].ToString().Split(','))
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article() { AreaId = int.Parse(item), ArticleId = int.Parse(id), Type = "one", IsLock = int.Parse(islock) });
                //}
            }
            catch (Exception ex)
            {
                Common.Log.Info(ex.Message + ex.InnerException.Message);
                context.Response.Write("error");
                return;
            }
            context.Response.Write("ok");
        }

        private void SwitchGoodGuQing(HttpContext context)
        {
            try
            {
                string id = context.Request.Params["id"];
                string username = context.Request.Params["username"];
                string islock = context.Request.Params["islock"];
                string type = context.Request.Params["type"];
                type = string.IsNullOrEmpty(type) ? "one" : type == "combo" ? "category" : type;
                DTcms.Common.Log.Info("SwitchGoodLock:" + context.Request.Url.Query);
                DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
                if (dtUser.Rows.Count == 0)
                {
                    return;
                }
                BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();
                List<BookingFood.Model.bf_area_article> listAreaArticle = bllAreaArticle.GetModelList(" Type='" + type + "' and ArticleId=" + id + " and AreaId IN (" + dtUser.Rows[0]["busyarea"].ToString() + ")");
                foreach (var item in listAreaArticle)
                {
                    if (islock == "0")
                    {
                        item.GuQingDate = null;
                        bllAreaArticle.Update(item);
                    }
                    else
                    {
                        if (item.IsLock == 0)
                        {
                            item.GuQingDate = DateTime.Now.Date;
                            bllAreaArticle.Update(item);
                        }
                    }


                }
                //foreach (var item in dtUser.Rows[0]["busyarea"].ToString().Split(','))
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article() { AreaId = int.Parse(item), ArticleId = int.Parse(id), Type = "one", IsLock = int.Parse(islock) });
                //}
            }
            catch (Exception ex)
            {
                Common.Log.Info(ex.Message + ex.InnerException.Message);
                context.Response.Write("error");
                return;
            }
            context.Response.Write("ok");
        }

        private void GetGoodsList(HttpContext context)
        {
            string areaid = DTcms.Common.DTRequest.GetQueryString("area");
            //Common.Log.Info("GetGoodsList:username_" + areaid);
            if (string.IsNullOrEmpty(areaid))
            {
                context.Response.Write("");
                return;
            }
            //DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + areaid + "'", " id asc").Tables[0];
            //if (dtUser.Rows.Count == 0)
            //{
            //    return;
            //}
            BLL.article bll = new BLL.article();
            DataTable dtgoods = null;
            dtgoods = new BLL.article().GetGoodsList(0, " channel_id!=3 And Id In (select ArticleId From bf_area_article where AreaId=" + areaid + " and Type='one')", " category_id asc,sort_id desc").Tables[0];
            List<BookingFood.Model.bf_area_article> listAreaArticle = new BookingFood.BLL.bf_area_article().GetModelList(" Type='one' And AreaId=" + areaid);
            string rtn = string.Empty;
            string categoryid = string.Empty;
            BLL.category bllCategory = new BLL.category();
            Model.article_goods modelGoods = null;
            foreach (DataRow item in dtgoods.Rows)
            {
                if (categoryid != item["category_id"].ToString())
                {
                    if (!string.IsNullOrEmpty(rtn))
                    {
                        rtn = rtn.TrimEnd(',');
                        rtn += "}},";
                    }
                    else
                    {
                        rtn += "{";
                    }
                    categoryid = item["category_id"].ToString();
                    rtn += "\"" + categoryid + "\":{\"catname\":\"" + bllCategory.GetModel(int.Parse(categoryid)).title + "\",\"detail\":{";

                }
                BookingFood.Model.bf_area_article temp = listAreaArticle.FirstOrDefault(s => s.ArticleId.ToString() == item["id"].ToString());
                string islock = "1";
                string guqing = "1";
                if (temp != null)
                {
                    if (temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                    {
                        guqing = "0";
                    }
                    islock = temp.IsLock.ToString();
                }
                modelGoods = bll.GetGoodsModel(int.Parse(item["id"].ToString()));
                rtn += "\"" + item["id"].ToString() + "\":{\"sort\":\"" + item["sort_id"].ToString()
                    + "\",\"name\":\"" + item["title"].ToString() + "\",\"price\":\"" + item["sell_price"].ToString()
                    + "\",\"img\":\"" + (modelGoods.albums.Count > 0 ? "http://www.4008317417.cn" + modelGoods.albums[0].big_img : string.Empty)
                    + "\",\"taste\":\"" + (!string.IsNullOrEmpty(item["taste"].ToString()) ? item["taste"].ToString() + "," : string.Empty)
                                        + string.Join(",", item["condition_price"].ToString().Split(',').Select(s => s.Split('†')[0]))
                    + "\",\"islock\":\"" + islock + "\",\"condition_price\":\"\",\"guqing\":\"" + guqing + "\"},";
            }
            if (!string.IsNullOrEmpty(rtn))
            {
                rtn = rtn.TrimEnd(',');
                rtn += "}}}";
            }
            context.Response.Write(rtn);
            //Common.Log.Info("GetGoodsList:" + rtn);
        }

        private void GetNewGoodList(HttpContext context)
        {
            string categoryid = DTRequest.GetQueryString("categoryid");
            string areaid = DTcms.Common.DTRequest.GetQueryString("area");
            if (string.IsNullOrEmpty(areaid))
            {
                context.Response.Write("");
                return;
            }
            BLL.article bll = new BLL.article();
            Model.article_goods modelGoods = null;
            BookingFood.Model.bf_area modelArea = new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
            DataTable dtcombo = null;
            DataTable dtgoods = null;
            BLL.article bllArticle = new BLL.article();
            dtcombo = new BookingFood.BLL.bf_good_combo().GetList(" CategoryId=" + categoryid
                + " and Id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='category' AND baa.AreaId=" + areaid + ") order by SortId desc").Tables[0];
            dtgoods = new BLL.article().GetGoodsList(9999, " channel_id!=3 and category_id=" + categoryid
                + "and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")", " sort_id desc").Tables[0];
            //上下架集合
            List<BookingFood.Model.bf_area_article> listAreaArticle = new BookingFood.BLL.bf_area_article().GetModelList(" AreaId=" + areaid);
            var  newcombos = new DownLoadNewCombo();
            List<BookingFood.Model.bf_area_article> listAreaArticleOne = new BookingFood.BLL.bf_area_article().GetModelList(" Type='one' And AreaId=" + areaid);
            BookingFood.Model.bf_area_article temp = null;

            #region 套餐
            newcombos.combolist = new List<DownLoadCombo>();
            foreach (DataRow item in dtcombo.Rows)
            {
                DownLoadCombo combo = new DownLoadCombo();
                //newcombos.combolist.Add(combo);
                combo.Id = int.Parse(item["Id"].ToString());
                combo.SortId = int.Parse(item["SortId"].ToString());
                combo.Taste = item["Taste"].ToString();
                combo.Title = item["Title"].ToString();
                combo.Img = "http://www.4008317417.cn" + item["Photo"].ToString();
                temp = listAreaArticle.FirstOrDefault(s => s.ArticleId.ToString() == item["id"].ToString());
                string islock = "1";
                string guqing = "1";
                if (temp != null)
                {
                    if (temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                    {
                        guqing = "0";
                    }
                    islock = temp.IsLock.ToString();
                }
                combo.IsLock = islock;
                combo.GuQing = guqing;
                combo.List = new List<DownLoadComboGood>();
                List<BookingFood.Model.bf_good_combo_detail> listGoodComboDetail =
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + combo.Id.ToString() + " Order By SortId Asc");
               
                foreach (var itemGoodComboDetail in listGoodComboDetail)
                {
                    DownLoadComboGood combodetail = new DownLoadComboGood();
                    combo.List.Add(combodetail);
                    combodetail.Id = itemGoodComboDetail.Id;
                    combodetail.SortId = (int)itemGoodComboDetail.SortId;
                    combodetail.Title = itemGoodComboDetail.BUsinessTitle;
                    combodetail.List = new List<DownLoadComboGoodDetail>();
                    DataSet ds = bllArticle.GetGoodsList(99, " category_id=" + itemGoodComboDetail.BusinessId.ToString()
                    + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + " )", " sort_id asc");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            modelGoods = bllArticle.GetGoodsModel(int.Parse(ds.Tables[0].Rows[i]["id"].ToString()));
                            temp = listAreaArticleOne.FirstOrDefault(s => s.ArticleId.ToString() == ds.Tables[0].Rows[i]["id"].ToString());
                            DownLoadComboGoodDetail combogooddetail = new DownLoadComboGoodDetail();
                            combogooddetail.Id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                            combogooddetail.Price = decimal.Parse(ds.Tables[0].Rows[i]["sell_price"].ToString());
                            combogooddetail.SortId = int.Parse(ds.Tables[0].Rows[i]["sort_id"].ToString());
                            combogooddetail.Title = ds.Tables[0].Rows[i]["title"].ToString();
                            combogooddetail.Taste = (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["taste"].ToString()) ? ds.Tables[0].Rows[i]["taste"].ToString() + "," : string.Empty)
                                        + string.Join(",", ds.Tables[0].Rows[i]["condition_price"].ToString().Split(',').Select(s => s.Split('†')[0]));
                            combogooddetail.Img = "http://www.4008317417.cn" + ds.Tables[0].Rows[i]["mp_img_url"].ToString();
                            combogooddetail.IsLock = (temp != null ? temp.IsLock.ToString() : "1");
                            combodetail.List.Add(combogooddetail);
                        }
                    }
                }
                newcombos.combolist.Add(combo);
            }
            #endregion
            newcombos.goods = new DownLoadGood();
            BLL.category bllCategory = new BLL.category();
            newcombos.goods.List = new List<DownLoadGoodDetail>();
            foreach (DataRow item in dtgoods.Rows)
            {
                var downloadgooddetail = new DownLoadGoodDetail();
                BookingFood.Model.bf_area_article temp1 = listAreaArticle.FirstOrDefault(s => s.ArticleId.ToString() == item["id"].ToString());
                string islock = "1";
                string guqing = "1";
                if (temp1 != null)
                {
                    if (temp1.IsLock == 0 && (temp1.GuQingDate == null || ((DateTime)temp1.GuQingDate).Date != DateTime.Now.Date))
                    {
                        guqing = "0";
                    }
                    islock = temp1.IsLock.ToString();
                }
                modelGoods = bll.GetGoodsModel(int.Parse(item["id"].ToString()));
                downloadgooddetail.Id = Convert.ToInt32(item["id"]);
                downloadgooddetail.SortId = Convert.ToInt32(item["sort_id"]);
                downloadgooddetail.Name = item["title"].ToString();
                downloadgooddetail.Price = Convert.ToDecimal(item["sell_price"]);
                downloadgooddetail.Img = modelGoods.albums.Count > 0 ? "http://www.4008317417.cn" + modelGoods.albums[0].big_img : string.Empty;
                //downloadgooddetail.condition_price = item["condition_price"].ToString();
                downloadgooddetail.IsLock = islock;
                downloadgooddetail.guqing = guqing;
                downloadgooddetail.Taste = item["taste"].ToString()+ "," + (item["condition_price"].ToString().Replace("0.00", "").Replace("†", ""));
                newcombos.goods.List.Add(downloadgooddetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(newcombos);
            context.Response.Write(ret);
            //Common.Log.Info("GetGoodComboList:" + ret);
        }

        private void GetGoodComboList(HttpContext context)
        {
            string areaid = DTcms.Common.DTRequest.GetQueryString("area");
            //Common.Log.Info("GetGoodComboList:username_" + areaid);
            if (string.IsNullOrEmpty(areaid))
            {
                context.Response.Write("");
                return;
            }
            BLL.article bllArticle = new BLL.article();
            List<DownLoadCombo> list = new List<DownLoadCombo>();
            DataTable dtcombo = null;
            dtcombo = new BookingFood.BLL.bf_good_combo().GetList(" Id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='category' AND baa.AreaId=" + areaid + " ) order by SortId asc").Tables[0];
            List<BookingFood.Model.bf_area_article> listAreaArticle = new BookingFood.BLL.bf_area_article().GetModelList(" Type='category' And AreaId=" + areaid);
            List<BookingFood.Model.bf_area_article> listAreaArticleOne = new BookingFood.BLL.bf_area_article().GetModelList(" Type='one' And AreaId=" + areaid);
            BookingFood.Model.bf_area_article temp = null;
            foreach (DataRow item in dtcombo.Rows)
            {
                DownLoadCombo combo = new DownLoadCombo();
                list.Add(combo);
                combo.Id = int.Parse(item["Id"].ToString());
                combo.SortId = int.Parse(item["SortId"].ToString());
                combo.Taste = item["Taste"].ToString();
                combo.Title = item["Title"].ToString();
                combo.Img = "http://www.4008317417.cn" + item["Photo"].ToString();
                temp = listAreaArticle.FirstOrDefault(s => s.ArticleId.ToString() == item["id"].ToString());
                string islock = "1";
                string guqing = "1";
                if (temp != null)
                {
                    if (temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                    {
                        guqing = "0";
                    }
                    islock = temp.IsLock.ToString();
                }
                combo.IsLock = islock;
                combo.GuQing = guqing;
                combo.List = new List<DownLoadComboGood>();
                List<BookingFood.Model.bf_good_combo_detail> listGoodComboDetail =
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + combo.Id.ToString() + " Order By SortId Asc");
                foreach (var itemGoodComboDetail in listGoodComboDetail)
                {
                    DownLoadComboGood combodetail = new DownLoadComboGood();
                    combo.List.Add(combodetail);
                    combodetail.Id = itemGoodComboDetail.Id;
                    combodetail.SortId = (int)itemGoodComboDetail.SortId;
                    combodetail.Title = itemGoodComboDetail.BUsinessTitle;
                    combodetail.List = new List<DownLoadComboGoodDetail>();
                    DataSet ds = bllArticle.GetGoodsList(99, " category_id=" + itemGoodComboDetail.BusinessId.ToString()
                    + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + " )", " sort_id asc");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.article_goods modelGoods = null;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            modelGoods = bllArticle.GetGoodsModel(int.Parse(ds.Tables[0].Rows[i]["id"].ToString()));
                            temp = listAreaArticleOne.FirstOrDefault(s => s.ArticleId.ToString() == ds.Tables[0].Rows[i]["id"].ToString());
                            DownLoadComboGoodDetail combogooddetail = new DownLoadComboGoodDetail();
                            combodetail.List.Add(combogooddetail);
                            combogooddetail.Id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                            combogooddetail.Price = decimal.Parse(ds.Tables[0].Rows[i]["sell_price"].ToString());
                            combogooddetail.SortId = int.Parse(ds.Tables[0].Rows[i]["sort_id"].ToString());
                            combogooddetail.Title = ds.Tables[0].Rows[i]["title"].ToString();
                            combogooddetail.Taste = (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["taste"].ToString()) ? ds.Tables[0].Rows[i]["taste"].ToString() + "," : string.Empty)
                                        + string.Join(",", ds.Tables[0].Rows[i]["condition_price"].ToString().Split(',').Select(s => s.Split('†')[0]));
                            combogooddetail.Img = "http://www.4008317417.cn" + ds.Tables[0].Rows[i]["mp_img_url"].ToString();
                            combogooddetail.IsLock = (temp != null ? temp.IsLock.ToString() : "1");

                        }
                    }
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(list);
            context.Response.Write(ret);
            //Common.Log.Info("GetGoodComboList:" + ret);
        }

        private void GetBackList(HttpContext context)
        {
            string username = context.Request.Params["username"];
            //DTcms.Common.Log.Info("backlist:username_" + username);
            DataTable dtUser = new DTcms.BLL.manager().GetList(0, " user_name='" + username + "'", " id asc").Tables[0];
            if (dtUser.Rows.Count == 0)
            {
                return;
            }

            string categoryid = context.Request.Params["categoryid"];
            //DTcms.Common.Log.Info("backlist:categoryid_" + categoryid);
            //DTcms.Common.Log.Info("backlist:areaid_" + dtUser.Rows[0]["backarea"].ToString());
            if (string.IsNullOrEmpty(categoryid) || string.IsNullOrEmpty(dtUser.Rows[0]["backarea"].ToString())) return;
            BookingFood.BLL.bf_back_door bllDoor = new BookingFood.BLL.bf_back_door();
            List<BookingFood.Model.bf_back_door> list =
                bllDoor.GetModelList(" AreaId In (" + dtUser.Rows[0]["backarea"].ToString() + ") And CategoryId In (" + categoryid + ") And IsDown=0 Order By OrderId Asc");
            //foreach (var item in list)
            //{
            //    item.IsDown = 1;
            //    bllDoor.Update(item);
            //}
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(list);
            context.Response.Write(ret);
            //DTcms.Common.Log.Info("backlist:" + ret);
        }

        private void BackDownConfirm(HttpContext context)
        {
            string ids = context.Request.Params["ids"];
            //DTcms.Common.Log.Info("backdownconfirm:ids" + ids);
            ids = ids.Replace('_', ',').Trim(',');
            BookingFood.BLL.bf_back_door bll = new BookingFood.BLL.bf_back_door();
            BookingFood.Model.bf_back_door model = null;
            foreach (string item in ids.Split(','))
            {
                model = bll.GetModel(int.Parse(item));
                if (model == null) continue;
                model.IsDown = 1;
                bll.Update(model);
            }
            context.Response.Write("ok");
        }

        private void GetTasteList(HttpContext context)
        {
            List<BookingFood.Model.bf_condition> list = new BookingFood.BLL.bf_condition().GetModelList(" 1=1 Order By SortId Asc");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(list);
            context.Response.Write(ret);
        }

        private void GetConditionList(HttpContext context)
        {
            List<BookingFood.Model.bf_condition_price> list = new BookingFood.BLL.bf_condition_price().GetModelList(" 1=1 Order By SortId Asc");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(list);
            context.Response.Write(ret);
        }

        private void GetAudit(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            DTcms.Model.manager bo = new DTcms.BLL.manager().GetModel(username, DTcms.Common.DESEncrypt.Encrypt(password));
            if (bo == null)
            {
                context.Response.Write("err");
                return;
            }
            StringBuilder strTemp = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            strTemp.Append(" 1=1");
            strTemp.Append(string.Format(" and do.add_time >='{0}'", time + " 00:00:00"));
            strTemp.Append(string.Format(" and do.add_time <='{0}'", time + " 23:59:59"));
            strTemp.Append(string.Format(" and do.area_id in (" + bo.busyarea + ")"));
            DTcms.BLL.orders bll = new BLL.orders();
            List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" Id in (" + bo.busyarea + ") order by IsShow desc");
            string area = string.Empty;
            foreach (var item in listArea)
            {
                area += string.Format("[{0}],[{0}_On],", item.Title);
            }
            area = area.TrimEnd(',');
            int totalcount = 0;
            DataTable dt = bll.GetListForAreaBi(1, 1, strTemp.ToString(), out totalcount, area).Tables[0];
            List<Audit> list = new List<Audit>();
            foreach (DataRow item in dt.Rows)
            {
                foreach (var itemarea in listArea)
                {
                    list.Add(new Audit() { title = itemarea.Title, offline = item[itemarea.Title].ToString(), online = item[itemarea.Title + "_On"].ToString() });
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(list);
            context.Response.Write(ret);
        }

        private void GetPayResult(HttpContext context)
        {
            string orderno = context.Request.Params["orderno"];
            Log.Info("GetPayResult:" + orderno);
            BLL.orders bll = new BLL.orders();
            if (bll.GetCount(" order_no='" + orderno + "'") > 0)
            {
                Model.orders model = bll.GetModel(orderno);
                if (model.payment_status == 2)
                {
                    context.Response.Write("ok");
                    return;
                }
                if (model.payment_id == 7)
                {
                    string alipayResult = API.Payment.Alipay_ScanCode.AlipayF2F.Query(model.order_no);
                    if (alipayResult == "支付成功")
                    {
                        context.Response.Write("ok");
                        bll.UpdateField(model.id, "payment_status=2");
                        return;
                    }
                    else
                    {
                        context.Response.Write(alipayResult);
                        return;
                    }
                }
                else if (model.payment_id == 8)
                {
                    int queryTimes = 10;//查询次数计数器
                    while (queryTimes-- > 0)
                    {
                        int succResult = 0;//查询结果
                        API.Payment.MpMicroPay.WxPayData queryResult = API.Payment.MpMicroPay.MicroPay.Query(model.order_no, out succResult);
                        //如果需要继续查询，则等待2s后继续
                        if (succResult == 2)
                        {
                            System.Threading.Thread.Sleep(2000);
                            continue;
                        }
                        //查询成功,返回订单查询接口返回的数据
                        else if (succResult == 1)
                        {
                            //Log.Debug("MicroPay", "Mircopay success, return order query result : " + queryResult.ToXml());
                            bll.UpdateField(model.id, "payment_status=2");
                            context.Response.Write("ok");
                            return;
                        }
                        //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                        else
                        {
                            //Log.Error("MicroPay", "Micropay failure, return micropay result : " + result.ToXml());
                            context.Response.Write(queryResult.IsSet("err_code_des") ? queryResult.GetValue("err_code_des").ToString() : "");
                            return;
                        }
                    }

                }
                else
                {
                    context.Response.Write("非扫码支付方式无需查询");
                    return;
                }
            }
            else
            {
                context.Response.Write("无此订单号");
            }
        }

        private void CallTakeOut(HttpContext context)
        {
            int orderid = DTRequest.GetQueryInt("orderid");
            Log.Info("CallTakeOut OrderId:" + orderid);
            Model.orders model = new BLL.orders().GetModel(orderid);
            if (model == null)
            {
                context.Response.Write("无此订单");
                return;
            }
            if (model.takeout == 0)
            {
                context.Response.Write("ok");
                return;
            }
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_call_takeout_first));
            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.MpForHere));
            string tempMessage = string.Empty;
            foreach (var item in model.order_goods)
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
            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_call_takeout_remark));
            string accessToken = string.Empty;
            accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
            try
            {
                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "vjB5zJBDeQaFRA5Ep1zDkT8cXQ_E3a-XMVH1_mKTyO4"
                , "#173177", "", tempData);
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
            }
            context.Response.Write("ok");
        }

        private void SwitchArea(HttpContext context)
        {
            string orderid = context.Request.Params["id"];
            string areaid = context.Request.Params["changeareaid"];
            try
            {
                string t = context.Request.Params["t"];
                DateTime now = ConvertUnixStampToDateTime(double.Parse(t) / 1000);
                if (now.AddMinutes(5) < DateTime.Now) return;
            }
            catch (Exception ex)
            {
                Log.Info(HttpContext.Current.Request.Params.ToString());
                Log.Info(ex.Message + " " + ex.StackTrace);
            }
            DTcms.BLL.orders bll = new DTcms.BLL.orders();
            DTcms.Model.orders orderModel = bll.GetModel(int.Parse(orderid));
            if (orderModel != null)
            {
                BookingFood.Model.bf_area areaModel =
                    new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
                bll.UpdateField(int.Parse(orderid), "status=5");

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

                context.Response.Write("ok");
                return;
            }
            context.Response.Write("err");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static DateTime ConvertUnixStampToDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

    }

    #region 下载订单
    public class DownLoadOrder
    {
        public string addr { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string time { get; set; }
        public string order { get; set; }
        public decimal totalprice { get; set; }
        public string liuyan { get; set; }
        public string bianhao { get; set; }
        public string orderid { get; set; }
        public string ispaid { get; set; }
        public string takeout { get; set; }

    }
    #endregion

    #region 线下订单类
    public class OfflineOrder
    {
        public string total { get; set; }
        public string time { get; set; }
        public string area { get; set; }
        public string iid { get; set; }
        public string num { get; set; }
        public List<OfflineOrderDetail> order { get; set; }
    }
    public class OfflineOrderDetail
    {
        public string id { get; set; }
        public string taste { get; set; }
        public string count { get; set; }
        public string price { get; set; }
    }

    public class OfflineOrderForTest
    {
        public string total { get; set; }
        public string time { get; set; }
        public string area { get; set; }
        public string iid { get; set; }
        public string num { get; set; }
        public List<OfflineOrderDetailForTest> order { get; set; }
        public string order_no { get; set; }
        public string payment_title { get; set; }
        public string auth_code { get; set; }
    }
    public class OfflineOrderDetailForTest
    {
        public string id { get; set; }
        public string taste { get; set; }
        public string count { get; set; }
        public string price { get; set; }
        public string type { get; set; }
        public string subgoods { get; set; }
        public string condition_price { get; set; }
    }
    #endregion

    #region 下载套餐菜单

    public class DownLoadCombo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortId { get; set; }
        public string Taste { get; set; }
        public string IsLock { get; set; }
        public string GuQing { get; set; }
        public string Img { get; set; }
        public List<DownLoadComboGood> List { get; set; }
    }

    public class DownLoadNewCombo
    {
        public List<DownLoadCombo> combolist { get; set; }

        public DownLoadGood goods { get; set; }
    }

    public class DownLoadComboGood
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortId { get; set; }
        public List<DownLoadComboGoodDetail> List { get; set; }
    }

    public class DownLoadGood
    {
        public List<DownLoadGoodDetail> List { get; set; }
    }
    public class DownLoadGoodDetail
    {
        public int Id { get; set; }
        public int SortId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
        public string Taste { get; set; }
        public string IsLock { get; set; }
        public string guqing { get; set; }
    }
    public class DownLoadComboGoodDetail
    {
        public int Id { get; set; }
        public int SortId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
        public string IsLock { get; set; }
        public string Taste { get; set; }
    }

    #endregion

    public class Audit
    {
        public string title { get; set; }
        public string offline { get; set; }
        public string online { get; set; }
    }

}