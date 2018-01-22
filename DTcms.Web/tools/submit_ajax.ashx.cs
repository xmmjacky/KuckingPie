using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.SessionState;
using DTcms.Web.UI;
using DTcms.Common;
using LitJson;
using System.Linq;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Xml.Linq;
using TeeGonSdk.Domain;
using TeeGonSdk.Request;
using TeeGonSdk.Response;
using System.Web.Script.Serialization;

namespace DTcms.Web.tools
{
    /// <summary>
    /// AJAX提交处理
    /// </summary>
    public class submit_ajax : IHttpHandler, IRequiresSessionState
    {
        Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
        Model.userconfig userConfig = new BLL.userconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_USER_XML_CONFING));
        private static TeeGonSdk.ITopClient Client = new TeeGonSdk.DefaultTopClient("https://api.teegon.com/", "bxkgovptblsbxe4zyi7ixbdh", "ot5rhjgescrhcewcex65uamkcypaaxfu");
        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = DTRequest.GetQueryString("action");

            switch (action)
            {
                #region Web
                case "digg_add": //顶踩
                    digg_add(context);
                    break;
                case "comment_add": //提交评论
                    comment_add(context);
                    break;
                case "comment_list": //评论列表
                    comment_list(context);
                    break;
                case "validate_username": //验证用户名
                    validate_username(context);
                    break;
                case "user_register": //用户注册
                    user_register(context);
                    break;
                case "user_invite_code": //申请邀请码
                    user_invite_code(context);
                    break;
                case "user_verify_email": //发送注册验证邮件
                    user_verify_email(context);
                    break;
                case "user_login": //用户登录
                    user_login(context);
                    break;
                case "user_oauth_bind": //绑定第三方登录账户
                    user_oauth_bind(context);
                    break;
                case "user_oauth_register": //注册第三方登录账户
                    user_oauth_register(context);
                    break;
                case "user_info_edit": //修改用户信息
                    user_info_edit(context);
                    break;
                case "user_avatar_crop": //确认裁剪用户头像
                    user_avatar_crop(context);
                    break;
                case "user_password_edit": //修改密码
                    user_password_edit(context);
                    break;
                case "user_getpassword": //邮箱取回密码
                    user_getpassword(context);
                    break;
                case "user_repassword": //邮箱重设密码
                    user_repassword(context);
                    break;
                case "user_message_delete": //删除短信息
                    user_message_delete(context);
                    break;
                case "user_message_add": //发布短信息
                    user_message_add(context);
                    break;
                case "user_point_convert": //用户兑换积分
                    user_point_convert(context);
                    break;
                case "user_point_delete": //删除用户积分明细
                    user_point_delete(context);
                    break;
                case "user_amount_recharge": //用户在线充值
                    user_amount_recharge(context);
                    break;
                case "user_amount_delete": //删除用户收支明细
                    user_amount_delete(context);
                    break;
                case "cart_goods_add": //购物车加入商品
                    cart_goods_add(context);
                    break;
                case "cart_goods_update": //购物车修改商品
                    cart_goods_update(context);
                    break;
                case "cart_goods_delete": //购物车删除商品
                    cart_goods_delete(context);
                    break;
                case "cart_goods_empty": //购物车删除商品
                    cart_goods_empty(context);
                    break;
                case "order_save": //保存订单
                    order_save(context);
                    break;
                case "order_cancel": //用户取消订单
                    order_cancel(context);
                    break;
                case "getgoodslist":
                    GetGoodsList(context);
                    break;
                case "getuserinfo":
                    GetUserInfo(context);
                    break;
                case "show_cart_goods":
                    ShowCartGoods(context);
                    break;
                case "get_last_order":
                    GetLastOrder(context);
                    break;
                case "get_area":
                    GetArea(context);
                    break;
                case "get_area_type":
                    GetAreaType(context);
                    break;
                case "do_quick":
                    DoQuick(context);
                    break;
                case "check_less":
                    CheckLess(context);
                    break;
                case "add_pc_less":
                    AddPcLess(context);
                    break;
                case "check_additional":
                    CheckAdditional(context);
                    break;
                case "getmppayresult":
                    GetMpPayResult(context);
                    break;
                #endregion

                #region MP
                case "mp_getgoodslist":
                    MP_GetGoodsList(context);
                    break;
                case "mp_getlastorder":
                    GetMpLastOrder(context);
                    break;
                case "mp_ordersave":
                    mp_ordersave(context);
                    break;
                case "add_less":
                    add_less(context);
                    break;
                case "get_company_list":
                    GetCompanyList(context);
                    break;
                case "get_polygon_contain":
                    GetPolygonContain(context);
                    break;
                case "get_polygon_contain_for_take":
                    GetPolygonContainForTake(context);
                    break;
                case "get_carnival_offline":
                    GetCarnivalOffline(context);
                    break;
                case "get_carnival_line":
                    GetCarnivalLine(context);
                    break;
                case "join_company":
                    JoinCompany(context);
                    break;
                    #endregion
            }
        }
        #region Web

        #region 顶和踩的处理方法OK==============================
        private void digg_add(HttpContext context)
        {
            string digg_type = DTRequest.GetFormString("digg_type");
            int article_id = DTRequest.GetFormInt("article_id");
            //检查是否重复点击
            if (article_id > 0)
            {
                string cookie = Utils.GetCookie(DTKeys.COOKIE_DIGG_KEY, "diggs_" + article_id.ToString());
                if (cookie == article_id.ToString())
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"您刚刚提交过，体息一会吧！\"}");
                    return;
                }
            }
            BLL.article_diggs bll = new BLL.article_diggs();
            if (!bll.Exists(article_id))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"信息不存在或已删除！\"}");
                return;
            }
            //自动+1
            if (digg_type == "good")
            {
                bll.UpdateField(article_id, "digg_good=digg_good+1");
            }
            else
            {
                bll.UpdateField(article_id, "digg_bad=digg_bad+1");
            }
            //返回成功
            Model.article_diggs model = bll.GetModel(article_id);
            context.Response.Write("{\"msg\":1, \"digggood\":" + model.digg_good + ", \"diggbad\":" + model.digg_bad + ", \"msgbox\":\"成功顶或踩了一下！\"}");
            Utils.WriteCookie(DTKeys.COOKIE_DIGG_KEY, "diggs_" + article_id.ToString(), article_id.ToString(), 8640);
            return;

        }
        #endregion

        #region 提交评论的处理方法OK============================
        private void comment_add(HttpContext context)
        {
            StringBuilder strTxt = new StringBuilder();
            BLL.article_comment bll = new BLL.article_comment();
            Model.article_comment model = new Model.article_comment();

            string code = DTRequest.GetFormString("txtCode");
            int article_id = DTRequest.GetQueryInt("article_id");
            string _content = DTRequest.GetFormString("txtContent");
            //校检验证码
            string result = verify_code(context, code);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            if (article_id == 0)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"对不起，参数传输有误！\"}");
                return;
            }
            if (string.IsNullOrEmpty(_content))
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"对不起，请输入评论的内容！\"}");
                return;
            }
            //检查用户是否登录
            int user_id = 0;
            string user_name = "匿名用户";
            Model.users userModel = new Web.UI.BasePage().GetUserInfo();
            if (userModel != null)
            {
                user_id = userModel.id;
                user_name = userModel.user_name;
            }
            model.article_id = article_id;
            model.content = Utils.ToHtml(_content);
            model.user_id = user_id;
            model.user_name = user_name;
            model.user_ip = DTRequest.GetIP();
            model.is_lock = siteConfig.commentstatus; //审核开关
            model.add_time = DateTime.Now;
            model.is_reply = 0;
            if (bll.Add(model) > 0)
            {
                context.Response.Write("{\"msg\": 1, \"msgbox\": \"留言提交成功啦！\"}");
                return;
            }
            context.Response.Write("{\"msg\": 0, \"msgbox\": \"对不起，保存过程中发生错误！\"}");
            return;
        }
        #endregion

        #region 取得评论列表方法OK==============================
        private void comment_list(HttpContext context)
        {
            int article_id = DTRequest.GetQueryInt("article_id");
            int page_index = DTRequest.GetQueryInt("page_index");
            int page_size = DTRequest.GetQueryInt("page_size");
            int totalcount;
            StringBuilder strTxt = new StringBuilder();

            if (article_id == 0 || page_size == 0)
            {
                context.Response.Write("获取失败，传输参数有误！");
                return;
            }

            BLL.article_comment bll = new BLL.article_comment();
            DataSet ds = bll.GetList(page_size, page_index, string.Format("is_lock=0 and article_id={0}", article_id.ToString()), "add_time asc", out totalcount);
            //如果记录存在
            if (ds.Tables[0].Rows.Count > 0)
            {
                strTxt.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    //strTxt.Append("<li>\n");
                    //strTxt.Append("<div class=\"title\"><span>" + dr["add_time"] + "</span>" + dr["user_name"] + "</div>");
                    //strTxt.Append("<div class=\"box\">" + dr["content"] + "</div>");
                    //if (Convert.ToInt32(dr["is_reply"]) == 1)
                    //{
                    //    strTxt.Append("<div class=\"reply\">");
                    //    strTxt.Append("<strong>管理员回复：</strong>" + dr["reply_content"].ToString());
                    //    strTxt.Append("<span class=\"time\">" + dr["reply_time"].ToString() + "</span>");
                    //    strTxt.Append("</div>");
                    //}
                    //strTxt.Append("</li>\n");

                    strTxt.Append("{");
                    strTxt.Append("\"user_id\":" + dr["user_id"]);
                    strTxt.Append(",\"user_name\":\"" + dr["user_name"] + "\"");
                    if (Convert.ToInt32(dr["user_id"]) > 0)
                    {
                        Model.users userModel = new BLL.users().GetModel(Convert.ToInt32(dr["user_id"]));
                        if (userModel != null)
                        {
                            strTxt.Append(",\"avatar\":\"" + userModel.avatar + "\"");
                        }
                    }
                    strTxt.Append("");
                    strTxt.Append(",\"content\":\"" + Microsoft.JScript.GlobalObject.escape(dr["content"]) + "\"");
                    strTxt.Append(",\"add_time\":\"" + dr["add_time"] + "\"");
                    strTxt.Append(",\"is_reply\":" + dr["is_reply"]);
                    if (Convert.ToInt32(dr["is_reply"]) == 1)
                    {
                        strTxt.Append(",\"reply_content\":\"" + Microsoft.JScript.GlobalObject.escape(dr["reply_content"]) + "\"");
                        strTxt.Append(",\"reply_time\":\"" + dr["reply_time"] + "\"");
                    }
                    strTxt.Append("}");
                    //是否加逗号
                    if (i < ds.Tables[0].Rows.Count - 1)
                    {
                        strTxt.Append(",");
                    }

                }
                strTxt.Append("]");
            }
            //else
            //{
            //    strTxt.Append("<p>暂无评论，快来抢沙发吧！</p>");
            //}
            context.Response.Write(strTxt.ToString());
        }
        #endregion

        #region 验证用户名是否可用OK============================
        private void validate_username(HttpContext context)
        {
            string username = DTRequest.GetString("username");
            //如果为Null，退出
            if (string.IsNullOrEmpty(username))
            {
                context.Response.Write("null");
                return;
            }
            //过滤注册用户名字符
            string[] strArray = userConfig.regkeywords.Split(',');
            foreach (string s in strArray)
            {
                if (s.ToLower() == username.ToLower())
                {
                    context.Response.Write("lock");
                    return;
                }
            }
            BLL.users bll = new BLL.users();
            //查询数据库
            if (!bll.Exists(username.Trim()))
            {
                context.Response.Write("true");
                return;
            }
            context.Response.Write("false");
            return;
        }
        #endregion

        #region 用户注册OK=====================================
        private void user_register(HttpContext context)
        {
            string code = DTRequest.GetFormString("txtCode").Trim();
            string invitecode = DTRequest.GetFormString("txtInviteCode").Trim();
            string username = DTRequest.GetFormString("txtUserName").Trim();
            string password = DTRequest.GetFormString("txtPassword").Trim();
            string email = DTRequest.GetFormString("txtEmail").Trim();
            string userip = DTRequest.GetIP();

            #region 检查各项并提示
            //检查是否开启会员功能
            if (siteConfig.memberstatus == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，会员功能已被关闭，无法注册新会员！\"}");
                return;
            }
            if (userConfig.regstatus == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，系统暂不允许注册新用户！\"}");
                return;
            }
            //校检验证码
            string result = verify_code(context, code);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            //检查用户输入信息是否为空
            if (username == "" || password == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"用户名和密码不能为空！\"}");
                return;
            }
            if (email == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"电子邮箱不能为空！\"}");
                return;
            }

            //检查用户名
            BLL.users bll = new BLL.users();
            Model.users model = new Model.users();
            if (bll.Exists(username))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"该用户名已经存在！\"}");
                return;
            }
            //检查同一IP注册时隔
            if (userConfig.regctrl > 0)
            {
                if (bll.Exists(userip, userConfig.regctrl))
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，同一IP在" + userConfig.regctrl + "小时内不能注册多个用户！\"}");
                    return;
                }
            }
            //不允许同一Email注册不同用户
            if (userConfig.regemailditto == 0)
            {
                if (bll.ExistsEmail(email))
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"Email不允许重复注册，如果你忘记用户名，请找回密码！\"}");
                    return;
                }
            }
            //检查默认组别是否存在
            Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
            if (modelGroup == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"系统尚未分组，请联系管理员设置会员分组！\"}");
                return;
            }
            //检查是否通过邀请码注册
            if (userConfig.regstatus == 2)
            {
                string result1 = verify_invite_reg(username, invitecode);
                if (result1 != "success")
                {
                    context.Response.Write(result1);
                    return;
                }
            }
            #endregion

            //保存注册信息
            model.group_id = modelGroup.id;
            model.user_name = username;
            model.password = DESEncrypt.Encrypt(password);
            model.email = email;
            model.reg_ip = userip;
            model.reg_time = DateTime.Now;
            model.is_lock = userConfig.regverify; //设置为对应状态
            int newId = bll.Add(model);
            if (newId < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"系统故障，注册失败，请联系网站管理员！\"}");
                return;
            }
            model = bll.GetModel(newId);
            //赠送积分金额
            if (modelGroup.point > 0)
            {
                new BLL.point_log().Add(model.id, model.user_name, modelGroup.point, "注册赠送积分");
            }
            if (modelGroup.amount > 0)
            {
                new BLL.amount_log().Add(model.id, model.user_name, DTEnums.AmountTypeEnum.SysGive.ToString(), modelGroup.amount, "注册赠送金额", 1);
            }
            //判断是否发送站内短消息
            if (userConfig.regmsgstatus == 1)
            {
                new BLL.user_message().Add(1, "", model.user_name, "欢迎您成为本站会员", userConfig.regmsgtxt);
            }
            //需要Email验证
            if (userConfig.regverify == 1)
            {
                string result2 = verify_email(model);
                if (result2 != "success")
                {
                    context.Response.Write(result2);
                    return;
                }
                context.Response.Write("{\"msg\":1, \"url\":\"" + new Web.UI.BasePage().linkurl("register") + "?action=sendmail&username=" + Utils.UrlEncode(model.user_name) + "\", \"msgbox\":\"注册成功，请进入邮箱验证激活账户！\"}");
            }
            //需要人工审核
            else if (userConfig.regverify == 2)
            {
                context.Response.Write("{\"msg\":1, \"url\":\"" + new Web.UI.BasePage().linkurl("register") + "?action=verify&username=" + Utils.UrlEncode(model.user_name) + "\", \"msgbox\":\"注册成功，请等待审核通过！\"}");
            }
            else
            {
                context.Response.Write("{\"msg\":1, \"url\":\"" + new Web.UI.BasePage().linkurl("register") + "?action=succeed&username=" + Utils.UrlEncode(model.user_name) + "\", \"msgbox\":\"注册成功啦！\"}");
            }
            return;
        }

        #region 邀请注册处理方法OK==============================
        private string verify_invite_reg(string user_name, string invite_code)
        {
            if (string.IsNullOrEmpty(invite_code))
            {
                return "{\"msg\":0, \"msgbox\":\"邀请码不能为空！\"}";
            }
            BLL.user_code codeBll = new BLL.user_code();
            Model.user_code codeModel = codeBll.GetModel(invite_code);
            if (codeModel == null)
            {
                return "{\"msg\":0, \"msgbox\":\"邀请码不正确或已过期啦！\"}";
            }
            if (userConfig.invitecodecount > 0)
            {
                if (codeModel.count >= userConfig.invitecodecount)
                {
                    codeModel.status = 1;
                    return "{\"msg\":0, \"msgbox\":\"该邀请码已经被使用啦！\"}";
                }
            }
            //检查是否给邀请人增加积分
            if (userConfig.pointinvitenum > 0)
            {
                new BLL.point_log().Add(codeModel.user_id, codeModel.user_name, userConfig.pointinvitenum, "邀请用户【" + user_name + "】注册获得积分");
            }
            //更改邀请码状态
            codeModel.count += 1;
            codeBll.Update(codeModel);
            return "success";
        }
        #endregion

        #region Email验证发送邮件OK=============================
        private string verify_email(Model.users userModel)
        {
            //生成随机码
            string strcode = Utils.GetCheckCode(20);
            BLL.user_code codeBll = new BLL.user_code();
            Model.user_code codeModel;
            //检查是否重复提交
            codeModel = codeBll.GetModel(userModel.user_name, DTEnums.CodeEnum.RegVerify.ToString());
            if (codeModel == null)
            {
                codeModel = new Model.user_code();
                codeModel.user_id = userModel.id;
                codeModel.user_name = userModel.user_name;
                codeModel.type = DTEnums.CodeEnum.RegVerify.ToString();
                codeModel.str_code = strcode;
                codeModel.eff_time = DateTime.Now.AddDays(userConfig.regemailexpired);
                codeModel.add_time = DateTime.Now;
                new BLL.user_code().Add(codeModel);
            }
            //获得邮件内容
            Model.mail_template mailModel = new BLL.mail_template().GetModel("regverify");
            if (mailModel == null)
            {
                return "{\"msg\":0, \"msgbox\":\"邮件发送失败，邮件模板内容不存在！\"}";
            }
            //替换模板内容
            string titletxt = mailModel.maill_title;
            string bodytxt = mailModel.content;
            titletxt = titletxt.Replace("{webname}", siteConfig.webname);
            titletxt = titletxt.Replace("{username}", userModel.user_name);
            bodytxt = bodytxt.Replace("{webname}", siteConfig.webname);
            bodytxt = bodytxt.Replace("{username}", userModel.user_name);
            bodytxt = bodytxt.Replace("{linkurl}", Utils.DelLastChar(siteConfig.weburl, "/") + new Web.UI.BasePage().linkurl("register") + "?action=checkmail&strcode=" + codeModel.str_code);
            //发送邮件
            try
            {
                DTMail.sendMail(siteConfig.emailstmp,
                    siteConfig.emailusername,
                    DESEncrypt.Decrypt(siteConfig.emailpassword),
                    siteConfig.emailnickname,
                    siteConfig.emailfrom,
                    userModel.email,
                    titletxt, bodytxt);
            }
            catch
            {
                return "{\"msg\":0, \"msgbox\":\"邮件发送失败，请联系本站管理员！\"}";
            }
            return "success";
        }

        #endregion

        #endregion

        #region 申请邀请码OK===================================
        private void user_invite_code(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            //检查是否开启邀请注册
            if (userConfig.regstatus != 2)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，系统不允许通过邀请注册！\"}");
                return;
            }
            BLL.user_code codeBll = new BLL.user_code();
            //检查申请是否超过限制
            if (userConfig.invitecodenum > 0)
            {
                int result = codeBll.GetCount("user_name='" + model.user_name + "' and type='" + DTEnums.CodeEnum.Register.ToString() + "' and datediff(d,add_time,getdate())=0");
                if (result >= userConfig.invitecodenum)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您申请的邀请码数量已超过每天的限制！\"}");
                    return;
                }
            }
            //删除过期的邀请码
            codeBll.Delete("type='" + DTEnums.CodeEnum.Register.ToString() + "' and status=1 or datediff(d,eff_time,getdate())>0");
            //随机取得邀请码
            string str_code = Utils.GetCheckCode(8);
            Model.user_code codeModel = new Model.user_code();
            codeModel.user_id = model.id;
            codeModel.user_name = model.user_name;
            codeModel.type = DTEnums.CodeEnum.Register.ToString();
            codeModel.str_code = str_code;
            if (userConfig.invitecodeexpired > 0)
            {
                codeModel.eff_time = DateTime.Now.AddDays(userConfig.invitecodeexpired);
            }
            codeBll.Add(codeModel);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"申请邀请码已成功！\"}");
            return;
        }
        #endregion

        #region 发送注册验证邮件OK=============================
        private void user_verify_email(HttpContext context)
        {
            string username = DTRequest.GetFormString("username");
            //检查是否过快
            string cookie = Utils.GetCookie("user_reg_email");
            if (cookie == username)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"发送邮件间隔为20分钟，您刚才已经提交过啦，休息一下再来吧！\"}");
                return;
            }
            Model.users model = new BLL.users().GetModel(username);
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"该用户不存在或已删除！\"}");
                return;
            }
            if (model.is_lock != 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"该用户无法进行邮箱验证！\"}");
                return;
            }
            string result = verify_email(model);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"邮件已经发送成功啦！\"}");
            Utils.WriteCookie("user_reg_email", username, 20); //20分钟内无重复发送
            return;
        }
        #endregion

        #region 用户登录OK=====================================
        private void user_login(HttpContext context)
        {
            string username = DTRequest.GetFormString("txtUserName");
            string password = DTRequest.GetFormString("txtPassword");
            string remember = DTRequest.GetFormString("chkRemember");
            //检查用户名密码
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"温馨提示：请输入用户名或密码！\"}");
                return;
            }

            BLL.users bll = new BLL.users();
            Model.users model = bll.GetModel(username, DESEncrypt.Encrypt(password), userConfig.emaillogin);
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"错误提示：用户名或密码错误，请重试！\"}");
                return;
            }
            //检查用户是否通过验证
            if (model.is_lock == 1) //待验证
            {
                context.Response.Write("{\"msg\":1, \"url\":\"" + new Web.UI.BasePage().linkurl("register") + "?action=sendmail&username=" + Utils.UrlEncode(model.user_name) + "\", \"msgbox\":\"会员尚未通过验证！\"}");
                return;
            }
            else if (model.is_lock == 2) //待审核
            {
                context.Response.Write("{\"msg\":1, \"url\":\"" + new Web.UI.BasePage().linkurl("register") + "?action=verify&username=" + Utils.UrlEncode(model.user_name) + "\", \"msgbox\":\"会员尚未通过审核！\"}");
                return;
            }
            context.Session[DTKeys.SESSION_USER_INFO] = model;
            context.Session.Timeout = 45;
            //记住登录状态下次自动登录
            if (remember.ToLower() == "true")
            {
                Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", model.user_name, 43200);
                Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", model.password, 43200);
            }
            else
            {
                //防止Session提前过期
                Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", model.user_name);
                Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", model.password);
            }

            //写入登录日志
            new BLL.user_login_log().Add(model.id, model.user_name, "会员登录", DTRequest.GetIP());
            //返回URL
            context.Response.Write("{\"msg\":1, \"msgbox\":\"会员登录成功！\"}");
            return;
        }
        #endregion

        #region 绑定第三方登录账户OK============================
        private void user_oauth_bind(HttpContext context)
        {
            //检查URL参数
            if (context.Session["oauth_name"] == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误提示：授权参数不正确！\"}");
                return;
            }
            //获取授权信息
            string result = Utils.UrlExecute(siteConfig.webpath + "api/oauth/" + context.Session["oauth_name"].ToString() + "/result_json.aspx");
            if (result.Contains("error"))
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误提示：请检查URL是否正确！\"}");
                return;
            }
            //反序列化JSON
            Dictionary<string, object> dic = JsonMapper.ToObject<Dictionary<string, object>>(result);
            if (dic["ret"].ToString() != "0")
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误代码：" + dic["ret"] + "，描述：" + dic["msg"] + "\"}");
                return;
            }

            //检查用户名密码
            string username = DTRequest.GetString("txtUserName");
            string password = DTRequest.GetString("txtPassword");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"温馨提示：请输入用户名或密码！\"}");
                return;
            }
            BLL.users bll = new BLL.users();
            Model.users model = bll.GetModel(username, DESEncrypt.Encrypt(password), userConfig.emaillogin);
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"错误提示：用户名或密码错误，请重试！\"}");
                return;
            }
            //开始绑定
            Model.user_oauth oauthModel = new Model.user_oauth();
            oauthModel.oauth_name = dic["oauth_name"].ToString();
            oauthModel.user_id = model.id;
            oauthModel.user_name = model.user_name;
            oauthModel.oauth_access_token = dic["oauth_access_token"].ToString();
            oauthModel.oauth_openid = dic["oauth_openid"].ToString();
            int newId = new BLL.user_oauth().Add(oauthModel);
            if (newId < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"错误提示：绑定过程中出现错误，请重新登录授权！\"}");
                return;
            }
            context.Session[DTKeys.SESSION_USER_INFO] = model;
            context.Session.Timeout = 45;
            //记住登录状态，防止Session提前过期
            Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", model.user_name);
            Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", model.password);
            //写入登录日志
            new BLL.user_login_log().Add(model.id, model.user_name, "会员登录", DTRequest.GetIP());
            //返回URL
            context.Response.Write("{\"msg\":1, \"msgbox\":\"会员登录成功！\"}");
            return;
        }
        #endregion

        #region 注册第三方登录账户OK============================
        private void user_oauth_register(HttpContext context)
        {
            //检查URL参数
            if (context.Session["oauth_name"] == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误提示：授权参数不正确！\"}");
                return;
            }
            //获取授权信息
            string result = Utils.UrlExecute(siteConfig.webpath + "api/oauth/" + context.Session["oauth_name"].ToString() + "/result_json.aspx");
            if (result.Contains("error"))
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误提示：请检查URL是否正确！\"}");
                return;
            }
            //反序列化JSON
            Dictionary<string, object> dic = JsonMapper.ToObject<Dictionary<string, object>>(result);
            if (dic["ret"].ToString() != "0")
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"错误代码：" + dic["ret"] + "，" + dic["msg"] + "\"}");
                return;
            }

            string password = DTRequest.GetFormString("txtPassword").Trim();
            string email = DTRequest.GetFormString("txtEmail").Trim();
            string userip = DTRequest.GetIP();
            //检查用户名
            BLL.users bll = new BLL.users();
            Model.users model = new Model.users();
            //检查默认组别是否存在
            Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
            if (modelGroup == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"系统尚未分组，请联系管理员设置会员分组！\"}");
                return;
            }
            //保存注册信息
            model.group_id = modelGroup.id;
            model.user_name = bll.GetRandomName(10);
            model.password = DESEncrypt.Encrypt(password);
            model.email = email;
            if (!string.IsNullOrEmpty(dic["nick"].ToString()))
            {
                model.nick_name = dic["nick"].ToString();
            }
            if (dic["avatar"].ToString().StartsWith("http://"))
            {
                model.avatar = dic["avatar"].ToString();
            }
            if (!string.IsNullOrEmpty(dic["sex"].ToString()))
            {
                model.sex = dic["sex"].ToString();
            }
            if (!string.IsNullOrEmpty(dic["birthday"].ToString()))
            {
                model.birthday = DateTime.Parse(dic["birthday"].ToString());
            }
            model.reg_ip = userip;
            model.reg_time = DateTime.Now;
            model.is_lock = 0; //设置为对应状态
            int newId = bll.Add(model);
            if (newId < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"系统故障，注册失败，请联系网站管理员！\"}");
                return;
            }
            model = bll.GetModel(newId);
            //赠送积分金额
            if (modelGroup.point > 0)
            {
                new BLL.point_log().Add(model.id, model.user_name, modelGroup.point, "注册赠送积分");
            }
            if (modelGroup.amount > 0)
            {
                new BLL.amount_log().Add(model.id, model.user_name, DTEnums.AmountTypeEnum.SysGive.ToString(), modelGroup.amount, "注册赠送金额", 1);
            }
            //判断是否发送站内短消息
            if (userConfig.regmsgstatus == 1)
            {
                new BLL.user_message().Add(1, "", model.user_name, "欢迎您成为本站会员", userConfig.regmsgtxt);
            }
            //绑定到对应的授权类型
            Model.user_oauth oauthModel = new Model.user_oauth();
            oauthModel.oauth_name = dic["oauth_name"].ToString();
            oauthModel.user_id = model.id;
            oauthModel.user_name = model.user_name;
            oauthModel.oauth_access_token = dic["oauth_access_token"].ToString();
            oauthModel.oauth_openid = dic["oauth_openid"].ToString();
            new BLL.user_oauth().Add(oauthModel);

            context.Session[DTKeys.SESSION_USER_INFO] = model;
            context.Session.Timeout = 45;
            //记住登录状态，防止Session提前过期
            Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", model.user_name);
            Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", model.password);
            //写入登录日志
            new BLL.user_login_log().Add(model.id, model.user_name, "会员登录", DTRequest.GetIP());
            //返回URL
            context.Response.Write("{\"msg\":1, \"msgbox\":\"会员登录成功！\"}");
            return;
        }
        #endregion

        #region 修改用户信息OK=================================
        private void user_info_edit(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string email = DTRequest.GetFormString("txtEmail");
            string nick_name = DTRequest.GetFormString("txtNickName");
            string sex = DTRequest.GetFormString("rblSex");
            string birthday = DTRequest.GetFormString("txtBirthday");
            string telphone = DTRequest.GetFormString("txtTelphone");
            string mobile = DTRequest.GetFormString("txtMobile");
            string qq = DTRequest.GetFormString("txtQQ");
            string address = context.Request.Form["txtAddress"];
            string safe_question = context.Request.Form["txtSafeQuestion"];
            string safe_answer = context.Request.Form["txtSafeAnswer"];
            //检查邮箱
            if (nick_name == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入您的姓名昵称！\"}");
                return;
            }
            //检查邮箱
            if (email == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入您邮箱帐号！\"}");
                return;
            }

            //开始写入数据库
            model.email = email;
            model.nick_name = nick_name;
            model.sex = sex;
            DateTime _birthday;
            if (DateTime.TryParse(birthday, out _birthday))
            {
                model.birthday = _birthday;
            }
            model.telphone = telphone;
            model.mobile = mobile;
            model.qq = qq;
            model.address = address;
            model.safe_question = safe_question;
            model.safe_answer = safe_answer;


            new BLL.users().Update(model);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"您的账户资料已修改成功啦！\"}");
            return;
        }
        #endregion

        #region 确认裁剪用户头像OK=============================
        private void user_avatar_crop(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string fileName = DTRequest.GetFormString("hideFileName");
            int x1 = DTRequest.GetFormInt("hideX1");
            int y1 = DTRequest.GetFormInt("hideY1");
            int w = DTRequest.GetFormInt("hideWidth");
            int h = DTRequest.GetFormInt("hideHeight");
            //检查是否图片

            //检查参数
            if (!Utils.FileExists(fileName) || w == 0 || h == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请先上传一张图片！\"}");
                return;
            }
            //取得保存的新文件名
            UpLoad upFiles = new UpLoad();
            bool result = upFiles.cropSaveAs(fileName, fileName, 180, 180, w, h, x1, y1);
            if (!result)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"图片裁剪过程中发生意外错误！\"}");
                return;
            }
            //删除原用户头像
            Utils.DeleteFile(model.avatar);
            model.avatar = fileName;
            //修改用户头像
            new BLL.users().UpdateField(model.id, "avatar='" + model.avatar + "'");
            context.Response.Write("{\"msg\": 1, \"msgbox\": \"" + model.avatar + "\"}");
            return;
        }
        #endregion

        #region 修改登录密码OK=================================
        private void user_password_edit(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            int user_id = model.id;
            string oldpassword = DTRequest.GetFormString("txtOldPassword");
            string password = DTRequest.GetFormString("txtPassword");
            //检查输入的旧密码
            if (string.IsNullOrEmpty(oldpassword))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"请输入您的旧登录密码！\"}");
                return;
            }
            //检查输入的新密码
            if (string.IsNullOrEmpty(password))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"请输入您的新登录密码！\"}");
                return;
            }
            //旧密码是否正确
            if (model.password != DESEncrypt.Encrypt(oldpassword))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您输入的旧密码不正确！\"}");
                return;
            }
            //执行修改操作
            model.password = DESEncrypt.Encrypt(password);
            new BLL.users().Update(model);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"您的密码已修改成功，请记住新密码！\"}");
            return;
        }
        #endregion

        #region 邮箱取回密码OK=================================
        private void user_getpassword(HttpContext context)
        {
            string code = DTRequest.GetFormString("txtCode");
            string username = DTRequest.GetFormString("txtUserName").Trim();
            //检查用户名是否正确
            if (string.IsNullOrEmpty(username))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户名不可为空！\"}");
                return;
            }
            //校检验证码
            string result = verify_code(context, code);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            //检查用户信息
            BLL.users bll = new BLL.users();
            Model.users model = bll.GetModel(username);
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您输入的用户名不存在！\"}");
                return;
            }
            if (string.IsNullOrEmpty(model.email))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您尚未设置邮箱地址，无法使用取回密码功能！\"}");
                return;
            }
            //生成随机码
            string strcode = Utils.GetCheckCode(20);
            //获得邮件内容
            Model.mail_template mailModel = new BLL.mail_template().GetModel("getpassword");
            if (mailModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"邮件发送失败，邮件模板内容不存在！\"}");
                return;
            }
            //检查是否重复提交
            BLL.user_code codeBll = new BLL.user_code();
            Model.user_code codeModel;
            codeModel = codeBll.GetModel(username, DTEnums.CodeEnum.RegVerify.ToString());
            if (codeModel == null)
            {
                codeModel = new Model.user_code();
                //写入数据库
                codeModel.user_id = model.id;
                codeModel.user_name = model.user_name;
                codeModel.type = DTEnums.CodeEnum.Password.ToString();
                codeModel.str_code = strcode;
                codeModel.eff_time = DateTime.Now.AddDays(1);
                codeModel.add_time = DateTime.Now;
                codeBll.Add(codeModel);
            }
            //替换模板内容
            string titletxt = mailModel.maill_title;
            string bodytxt = mailModel.content;
            titletxt = titletxt.Replace("{webname}", siteConfig.webname);
            titletxt = titletxt.Replace("{username}", model.user_name);
            bodytxt = bodytxt.Replace("{webname}", siteConfig.webname);
            bodytxt = bodytxt.Replace("{username}", model.user_name);
            bodytxt = bodytxt.Replace("{linkurl}", Utils.DelLastChar(siteConfig.weburl, "/") + new BasePage().linkurl("repassword1", "reset", strcode)); //此处需要修改
            //发送邮件
            try
            {
                DTMail.sendMail(siteConfig.emailstmp,
                    siteConfig.emailusername,
                    DESEncrypt.Decrypt(siteConfig.emailpassword),
                    siteConfig.emailnickname,
                    siteConfig.emailfrom,
                    model.email,
                    titletxt, bodytxt);
            }
            catch
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"邮件发送失败，请联系本站管理员！\"}");
                return;
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"邮件发送成功，请登录您的邮箱找回登录密码！\"}");
            return;
        }
        #endregion

        #region 邮箱重设密码OK=================================
        private void user_repassword(HttpContext context)
        {
            string code = context.Request.Form["txtCode"];
            string strcode = context.Request.Form["hideCode"];
            string password = context.Request.Form["txtPassword"];

            //校检验证码
            string result = verify_code(context, code);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            //检查验证字符串
            if (string.IsNullOrEmpty(strcode))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"系统找不到邮件验证的字符串！\"}");
                return;
            }
            //检查输入的新密码
            if (string.IsNullOrEmpty(password))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"请输入您的新密码！\"}");
                return;
            }

            BLL.user_code codeBll = new BLL.user_code();
            Model.user_code codeModel = codeBll.GetModel(strcode);
            if (codeModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"邮件验证的字符串不存在或已过期！\"}");
                return;
            }
            //验证用户是否存在
            BLL.users userBll = new BLL.users();
            if (!userBll.Exists(codeModel.user_id))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"该用户不存在或已被删除！\"}");
                return;
            }
            Model.users userModel = userBll.GetModel(codeModel.user_id);
            //执行修改操作
            userModel.password = DESEncrypt.Encrypt(password);
            userBll.Update(userModel);
            //更改验证字符串状态
            codeModel.count = 1;
            codeModel.status = 1;
            codeBll.Update(codeModel);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"修改密码成功，请记住您的新密码！\"}");
            return;
        }
        #endregion

        #region 删除短信息OK===================================
        private void user_message_delete(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string check_id = DTRequest.GetFormString("checkId");
            if (check_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"删除失败，请检查传输参数！\"}");
                return;
            }
            string[] arrId = check_id.Split(',');
            for (int i = 0; i < arrId.Length; i++)
            {
                new BLL.user_message().Delete(int.Parse(arrId[i]), model.user_name);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"删除短信息成功啦！\"}");
            return;
        }
        #endregion

        #region 发布短信息OK===================================
        private void user_message_add(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string code = context.Request.Form["txtCode"];
            string send_save = DTRequest.GetFormString("sendSave");
            string user_name = DTRequest.GetFormString("txtUserName");
            string title = DTRequest.GetFormString("txtTitle");
            string content = DTRequest.GetFormString("txtContent");
            //校检验证码
            string result = verify_code(context, code);
            if (result != "success")
            {
                context.Response.Write(result);
                return;
            }
            //检查用户名
            if (user_name == "" || !new BLL.users().Exists(user_name))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，该用户名不存在或已经被删除啦！\"}");
                return;
            }
            //检查标题
            if (title == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入短消息标题！\"}");
                return;
            }
            //检查内容
            if (content == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入短消息内容！\"}");
                return;
            }
            //保存数据
            Model.user_message modelMessage = new Model.user_message();
            modelMessage.type = 2;
            modelMessage.post_user_name = model.user_name;
            modelMessage.accept_user_name = user_name;
            modelMessage.title = title;
            modelMessage.content = Utils.ToHtml(content);
            new BLL.user_message().Add(modelMessage);
            if (send_save == "true") //保存到收件箱
            {
                modelMessage.type = 3;
                new BLL.user_message().Add(modelMessage);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"发布短信息成功啦！\"}");
            return;
        }
        #endregion

        #region 用户兑换积分OK=================================
        private void user_point_convert(HttpContext context)
        {
            //检查系统是否启用兑换积分功能
            if (userConfig.pointcashrate == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，网站已关闭兑换积分功能！\"}");
                return;
            }
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            int amout = DTRequest.GetFormInt("txtAmount");
            string password = DTRequest.GetFormString("txtPassword");
            if (model.amount < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您账户上的余额不足！\"}");
                return;
            }
            if (amout < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，最小兑换金额为1元！\"}");
                return;
            }
            if (amout > model.amount)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您兑换的金额大于账户余额！\"}");
                return;
            }
            if (password == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入您账户的密码！\"}");
                return;
            }
            //验证密码
            if (DESEncrypt.Encrypt(password) != model.password)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您输入的密码不正确！\"}");
                return;
            }
            //计算兑换后的积分值
            int convertPoint = (int)(Convert.ToDecimal(amout) * userConfig.pointcashrate);
            //扣除金额
            int amountNewId = new BLL.amount_log().Add(model.id, model.user_name, DTEnums.AmountTypeEnum.Convert.ToString(), amout * -1, "用户兑换积分", 1);
            //增加积分
            if (amountNewId < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"转换过程中发生错误，请重新提交！\"}");
                return;
            }
            int pointNewId = new BLL.point_log().Add(model.id, model.user_name, convertPoint, "用户兑换积分");
            if (pointNewId < 1)
            {
                //返还金额
                new BLL.amount_log().Add(model.id, model.user_name, DTEnums.AmountTypeEnum.Convert.ToString(), amout, "用户兑换积分失败，返还金额", 1);
                context.Response.Write("{\"msg\":0, \"msgbox\":\"转换过程中发生错误，请重新提交！\"}");
                return;
            }

            context.Response.Write("{\"msg\":1, \"msgbox\":\"积分兑换成功啦！\"}");
            return;
        }
        #endregion

        #region 删除用户积分明细OK=============================
        private void user_point_delete(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string check_id = DTRequest.GetFormString("checkId");
            if (check_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"删除失败，请检查传输参数！\"}");
                return;
            }
            string[] arrId = check_id.Split(',');
            for (int i = 0; i < arrId.Length; i++)
            {
                new BLL.point_log().Delete(int.Parse(arrId[i]), model.user_name);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"积分明细删除成功啦！\"}");
            return;
        }
        #endregion

        #region 用户在线充值OK=================================
        private void user_amount_recharge(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            decimal amount = DTRequest.GetFormDecimal("order_amount", 0);
            int payment_id = DTRequest.GetFormInt("payment_id");
            if (amount == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入正确的充值金额！\"}");
                return;
            }
            if (payment_id == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请选择正确的支付方式！\"}");
                return;
            }
            if (!new BLL.payment().Exists(payment_id))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您选择的支付方式不存在或已删除！\"}");
                return;
            }
            //生成订单号
            string order_no = Utils.GetOrderNumber(); //订单号
            new BLL.amount_log().Add(model.id, model.user_name, DTEnums.AmountTypeEnum.Recharge.ToString(), order_no, payment_id, amount,
                "账户充值(" + new BLL.payment().GetModel(payment_id).title + ")", 0);
            //保存成功后返回订单号
            context.Response.Write("{\"msg\":1, \"msgbox\":\"订单保存成功！\", \"url\":\"" + new Web.UI.BasePage().linkurl("payment1", "confirm", DTEnums.AmountTypeEnum.Recharge.ToString(), order_no) + "\"}");
            return;

        }
        #endregion

        #region 删除用户收支明细OK=============================
        private void user_amount_delete(HttpContext context)
        {
            //检查用户是否登录
            Model.users model = new BasePage().GetUserInfo();
            if (model == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            string check_id = DTRequest.GetFormString("checkId");
            if (check_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"删除失败，请检查传输参数！\"}");
                return;
            }
            string[] arrId = check_id.Split(',');
            for (int i = 0; i < arrId.Length; i++)
            {
                new BLL.amount_log().Delete(int.Parse(arrId[i]), model.user_name);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"收支明细删除成功啦！\"}");
            return;
        }
        #endregion

        #region 购物车加入商品OK===============================
        private void cart_goods_add(HttpContext context)
        {
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(model.starttime) || DateTime.Now > DateTime.Parse(model.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + model.starttime + "之" + model.endtime + "之间订餐！\"}");
                return;
            }

            string goods_id = DTRequest.GetFormString("goods_id");
            int goods_quantity = DTRequest.GetFormInt("goods_quantity", 1);
            string goods_type = DTRequest.GetFormString("goods_type");
            string subgoodsid = DTRequest.GetFormString("subgoodsid");

            if (goods_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您提交的商品参数有误！\"}");
                return;
            }
            //查找会员组
            int group_id = 0;
            Model.user_groups groupModel = new BLL.user_groups().GetDefault();
            group_id = groupModel.id;
            //统计购物车
            Web.UI.ShopCart.Add(goods_id + "†" + goods_type + "†" + subgoodsid, goods_quantity, subgoodsid);
            //Model.cart_total cartModel = Web.UI.ShopCart.GetTotal(group_id);
            IList<Model.cart_items> listCart = Web.UI.ShopCart.GetList(group_id);
            string rtn = "";
            decimal totalmoney = 0;
            foreach (var item in listCart)
            {
                totalmoney += item.price * item.quantity;
                if (item.type == "one")
                {
                    rtn += string.Format("<div class='item_line' data-id='{4}'>" +
                        "<span class='o_item_count'>" +
                            "<i class='del_item' data-id='{3}' onclick='GoodsDel(this)'>─</i>" +
                            "<span class='count_ctrl'>" +
                                "<i class='plus' data-id='{3}' onclick='GoodsPlus(this)'>+</i>" +
                                "<i class='minus' data-id='{3}' onclick='GoodsMinus(this)'>-</i>" +
                            "</span>" +
                            "<b>{0}</b>" +
                        "</span>" +
                        "<span class='o_item_name'>" +
                            "{1} {5}" +
                        "</span>" +
                        "<span class='o_item_price'>" +
                            "￥{2}元" +
                        "</span>" +
                        "</div>", item.quantity, item.title, (item.price * item.quantity).ToString().Replace(".00", "")
                        , item.id + "†" + item.type + "†" + item.subgoodsid, item.id
                        , !string.IsNullOrEmpty(item.subgoodsid) ? "* " + item.subgoodsid.Split('‡')[2] : "");
                }
                else if (item.type == "combo")
                {
                    rtn += string.Format("<div class='item_line' data-id='{4}'>" +
                        "<span class='o_item_count'>" +
                           "<i class='del_item' data-id='{3}' onclick='GoodsDel(this)'>─</i>" +
                            "<span class='count_ctrl'>" +
                                "<i class='plus' data-id='{3}' onclick='GoodsPlus(this)'>+</i>" +
                                "<i class='minus' data-id='{3}' onclick='GoodsMinus(this)'>-</i>" +
                            "</span>" +
                            "<b>{0}</b>" +
                        "</span>" +
                        "<span class='o_item_name'>" +
                            "{1}" +
                        "</span>" +
                        "<span class='o_item_price'>" +
                            "￥{2}元" +
                        "</span>" +
                        "</div>", item.quantity, item.title, (item.price * item.quantity).ToString().Replace(".00", "")
                        , item.id + "†" + item.type + "†" + item.subgoodsid, item.id);
                    string[] subgoods = item.subgoodsid.Split('†');
                    string tempCombo = string.Empty;
                    foreach (var sub in subgoods)
                    {
                        //if (sub.Split('‡')[0] == "taste") continue;
                        tempCombo += "* " + sub.Split('‡')[2] + " ";
                    }
                    rtn += string.Format("<div class='item_line' data-id='{1}'>" +
                        "<span >" +
                            "{0}" +
                        "</span>" +
                        "</div>", tempCombo, item.id);
                }
            }


            context.Response.Write("{\"msg\":1, \"msgbox\":\"" + rtn + "\", \"amount\":" + totalmoney + "}");
            return;
        }
        #endregion

        #region 修改购物车商品OK===============================
        private void cart_goods_update(HttpContext context)
        {
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(model.starttime) || DateTime.Now > DateTime.Parse(model.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + model.starttime + "之" + model.endtime + "之间订餐！\"}");
                return;
            }
            string goods_id = DTRequest.GetFormString("goods_id");
            int goods_quantity = DTRequest.GetFormInt("goods_quantity");
            if (goods_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您提交的商品参数有误！\"}");
                return;
            }

            if (Web.UI.ShopCart.Update(goods_id, goods_quantity))
            {
                int group_id = 0;
                Model.user_groups groupModel = new BLL.user_groups().GetDefault();
                group_id = groupModel.id;
                Model.cart_total total = Web.UI.ShopCart.GetTotal(group_id);
                Model.cart_items cartitem = Web.UI.ShopCart.GetList(group_id).FirstOrDefault(s => s.id == int.Parse(goods_id.Split('†')[0]));
                context.Response.Write("{\"msg\":1, \"msgbox\":\"商品数量修改成功！\",\"amount\":" + total.payable_amount + ",\"itemtotalprice\":"
                    + (cartitem != null ? (cartitem.quantity * cartitem.price).ToString().Replace(".00", "") : "0") + "}");
            }
            else
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"商品数量更改失败，请检查操作是否有误！\"}");
            }
            return;
        }
        #endregion

        #region 删除购物车商品OK===============================
        private void cart_goods_delete(HttpContext context)
        {
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(model.starttime) || DateTime.Now > DateTime.Parse(model.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + model.starttime + "之" + model.endtime + "之间订餐！\"}");
                return;
            }
            string goods_id = DTRequest.GetFormString("goods_id");
            if (goods_id == "")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您提交的商品参数有误！\"}");
                return;
            }
            Web.UI.ShopCart.Clear(goods_id);
            context.Response.Write("{\"msg\":1, \"msgbox\":\"商品移除成功！\"}");
            return;
        }
        #endregion

        #region 清空购物车商品OK===============================
        private void cart_goods_empty(HttpContext context)
        {
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(model.starttime) || DateTime.Now > DateTime.Parse(model.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + model.starttime + "之" + model.endtime + "之间订餐！\"}");
                return;
            }
            Web.UI.ShopCart.Clear("0");
            context.Response.Write("{\"msg\":1, \"msgbox\":\"清空购物车商品成功！\"}");
            return;
        }
        #endregion

        #region 保存用户订单OK=================================
        private void order_save(HttpContext context)
        {
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig siteConfig = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(siteConfig.starttime) || DateTime.Now > DateTime.Parse(siteConfig.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + siteConfig.starttime + "之" + siteConfig.endtime + "之间订餐！\"}");
                return;
            }
            int payment_id = 1;
            int distribution_id = 1;
            string email = DTRequest.GetFormString("email");
            string telphone = DTRequest.GetFormString("phone");
            string address = DTRequest.GetFormString("address");
            string message = DTRequest.GetFormString("message");
            string areaid = DTRequest.GetFormString("areaid");
            int paymentid = DTRequest.GetFormInt("paymentid");
            int disamountid = DTRequest.GetFormInt("disamountid");
            string additional = context.Request.Params["additional"];
            string nickname = DTRequest.GetFormString("nickname");
            BLL.orders bllOrder = new BLL.orders();
            //检查配送方式
            if (distribution_id == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请选择配送方式！\"}");
                return;
            }
            Model.distribution disModel = new BLL.distribution().GetModel(distribution_id);
            if (disModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您选择的配送方式不存在或已删除！\"}");
                return;
            }
            if (!string.IsNullOrEmpty(email) && string.Equals(email, siteConfig.TaoDianDianAccount))
            {
                payment_id = 2;
            }
            if (paymentid == 3) payment_id = 3;//支付宝
            if (paymentid == 5) payment_id = 5;//微信公众号支付
            if (paymentid == 6) payment_id = 6;//微信扫码支付
            //检查支付方式
            if (payment_id == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请选择支付方式！\"}");
                return;
            }
            Model.payment payModel = new BLL.payment().GetModel(payment_id);
            if (payModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您选择的支付方式不存在或已删除！\"}");
                return;
            }
            //检查地址
            if (string.IsNullOrEmpty(address))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入详细的收货地址！\"}");
                return;
            }
            //邮件合法性
            if (CheckEmail(email))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请重新输入邮件地址！\"}");
                return;
            }
            //检查用户是否登录
            BLL.users bllUsers = new BLL.users();
            Model.users userModel = bllUsers.GetModel(email, telphone);
            if (userModel == null)
            {
                userModel = new Model.users();
                Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                userModel.group_id = modelGroup.id;
                userModel.user_name = Guid.NewGuid().ToString();
                userModel.nick_name = nickname;
                userModel.password = DESEncrypt.Encrypt("111111");
                userModel.email = email;
                userModel.telphone = telphone;
                userModel.address = address;
                userModel.reg_time = DateTime.Now;
                userModel.is_lock = userConfig.regverify; //设置为对应状态
                int newId = bllUsers.Add(userModel);
                userModel.id = newId;
            }
            else
            {
                userModel.email = email;
                userModel.address = address;
                userModel.nick_name = nickname;
                bllUsers.Update(userModel);
            }
            //检查购物车商品
            IList<Model.cart_items> iList = DTcms.Web.UI.ShopCart.GetList(userModel.group_id);
            //if (iList == null)
            //{
            //    context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，购物车为空，无法结算！\"}");
            //    return;
            //}
            //检查是否满足最低起送数量
            if (iList != null)
            {
                BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
                foreach (var item in iList)
                {
                    List<BookingFood.Model.bf_article_low_num> lownum = bllLowNum.GetModelList("ArticleId=" + item.id.ToString() + " And "
                    + "CONVERT(varchar(12),GETDATE(),108) BETWEEN CONVERT(varchar(12),BeginTime,108) AND CONVERT(varchar(12), EndTime, 108)");
                    if (lownum.Count > 0 && lownum[0].LowNum > item.quantity)
                    {
                        context.Response.Write("{\"msg\":0, \"msgbox\":\"" + item.title + "最低起送数量为" + item.low_num + "，请确认数量！\"}");
                        return;
                    }
                }
            }
            if (string.IsNullOrEmpty(areaid))
            {
                context.Response.Write("{\"msg\":911, \"msgbox\":\"对不起，未选择外卖区域！\"}");
                return;
            }
            BookingFood.Model.bf_area areaModel = new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
            if (areaModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，未选择外卖区域！\"}");
                return;
            }
            //检查是否可以补单
            int additional_id = 0;
            if (string.Equals(additional, "1"))
            {
                DataTable dt = bllOrder.GetList(1, " user_id=" + userModel.id + " and OrderType!='催单' and OrderType!='通知' and is_additional=0", " id desc").Tables[0];
                if (dt.Rows.Count == 0)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"您还没有提交过订单！\"}");
                    return;
                }
                if (dt.Rows[0]["additional_count"].ToString() != "0")
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"只能提交一次补单！\"}");
                    return;
                }
                if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(siteConfig.additional_force) < DateTime.Now)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"上一个订单下单后" + siteConfig.additional_force + "分钟内可以提交补单！\"}");
                    return;
                }
                additional_id = int.Parse(dt.Rows[0]["id"].ToString());
            }
            if (disamountid != 1 && disamountid != 2)
            {
                disamountid = 1;
            }
            //统计购物车
            Model.cart_total cartModel = DTcms.Web.UI.ShopCart.GetTotal(userModel.group_id);
            if (cartModel.real_amount < siteConfig.lowamount && email != siteConfig.IsNullCartAccount
                && !string.Equals(additional, "1") && disamountid == 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"" + siteConfig.alarm_message_lowamount.Replace("{0}", siteConfig.lowamount.ToString()) + "\"}");
                return;
            }
            if (cartModel.real_amount < siteConfig.lowamount_2 && email != siteConfig.IsNullCartAccount
                && !string.Equals(additional, "1") && disamountid == 2)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"" + siteConfig.alarm_message_lowamount.Replace("{0}", siteConfig.lowamount_2.ToString()) + "\"}");
                return;
            }
            //检查货到付款使用限制
            if (siteConfig.DeliverPayMaxAmountForWeb > 0 && !string.Equals(additional, "1")
                && cartModel.real_amount <= siteConfig.DeliverPayMaxAmountForWeb && payment_id == 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"抱歉，低于" + siteConfig.DeliverPayMaxAmountForWeb + "元不能使用货到付款\"}");
                return;
            }
            //保存订单=======================================================================
            Model.orders model = new Model.orders();
            model.order_no = Utils.GetOrderNumber(); //订单号
            model.user_id = userModel.id;
            model.user_name = userModel.user_name;
            model.payment_id = payment_id;
            model.distribution_id = distribution_id;
            model.accept_name = userModel.nick_name;
            model.post_code = "";
            model.telphone = userModel.telphone;
            model.mobile = "";
            model.email = userModel.email;
            model.address = address + " " + userModel.nick_name;
            model.message = message;
            if (string.Equals(additional, "1"))
            {
                model.message += "【补单】";
            }
            model.payable_amount = cartModel.payable_amount;
            model.real_amount = cartModel.real_amount;

            model.area_id = areaModel.Id;
            model.area_title = areaModel.Title;
            if (!string.Equals(email, siteConfig.IsNullCartAccount))
            {
                model.OrderType = "网页";
            }
            else
            {
                model.OrderType = "电话";
            }
            model.is_additional = string.Equals(additional, "1") ? 1 : 0;

            //如果是先款后货的话
            if (payModel.type == 1)
            {
                model.payment_fee = 0;
                model.payment_status = 1;
            }
            else
            {
                model.payment_fee = 0;
                model.payment_status = 1;
            }
            //如果是权限邮箱下单则为已支付
            if (payment_id == 2)
            {
                model.payment_status = 2;
            }
            if (model.real_amount >= siteConfig.freedisamount || string.Equals(email, siteConfig.IsNullCartAccount) || additional == "1")
            {
                model.payable_freight = 0; //应付运费
                model.real_freight = 0; //实付运费
            }
            else
            {
                model.payable_freight = siteConfig.disamount; //应付运费
                model.real_freight = siteConfig.disamount; //实付运费
            }
            //订单总金额=实付商品金额+运费+支付手续费
            model.order_amount = model.real_amount + model.real_freight + model.payment_fee;
            //购物积分,可为负数
            model.point = cartModel.total_point;
            model.add_time = DateTime.Now;
            //商品详细列表
            List<Model.order_goods> gls = new List<Model.order_goods>();
            List<BookingFood.Model.bf_order_goods_combo_detail> cls = new List<BookingFood.Model.bf_order_goods_combo_detail>();

            if (iList != null)
            {
                foreach (Model.cart_items item in iList)
                {
                    gls.Add(new Model.order_goods
                    {
                        goods_id = item.id,
                        goods_name = item.title,
                        goods_price = item.price
                        ,
                        real_price = item.user_price,
                        quantity = item.quantity,
                        point = item.point
                        ,
                        type = item.type,
                        subgoodsid = item.subgoodsid,
                        category_title = item.category_title
                    });
                }
            }
            model.order_goods = gls;
            int result = new BLL.orders().Add(model);

            if (result < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"订单保存过程中发生错误，请重新提交！\"}");
                return;
            }
            #region 更新用户最新的区域


            DTcms.BLL.orders orderbll = new DTcms.BLL.orders();
            DTcms.Model.orders orderModel = orderbll.GetModel(result);
            if (orderModel != null)
            {
                if (orderModel.user_address_id != 0)
                {
                    BookingFood.BLL.bf_user_address bllUserAddress = new BookingFood.BLL.bf_user_address();
                    BookingFood.Model.bf_user_address modelUserAddress =
                           bllUserAddress.GetModel(model.user_address_id);

                    modelUserAddress.AreaId = model.area_id;
                    bllUserAddress.Update(modelUserAddress);
                }

            }
            #endregion
            //补单的话，更新被补的订单的additional_count
            if (string.Equals(additional, "1"))
            {
                bllOrder.UpdateField(additional_id, " additional_count=additional_count+1 , additional_id=ISNULL(additional_id,'')+'" + result + ",'");
            }
            //清空购物车
            DTcms.Web.UI.ShopCart.Clear("0");
            if (payModel.type == 2)
            {
                #region 更新后厨推送数据
                BLL.article bllArticle = new BLL.article();
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in gls)
                {
                    if (item.type == "one")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = result,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = model.area_id,
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
                                OrderId = result,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = model.area_id,
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
                    string titletxt = mailModel.maill_title + model.order_no;
                    string bodytxt = mailModel.content;
                    bodytxt = bodytxt.Replace("{useremail}", model.email);
                    bodytxt = bodytxt.Replace("{useraddress}", model.address);
                    bodytxt = bodytxt.Replace("{usertelphone}", model.telphone);
                    bodytxt = bodytxt.Replace("{orderaddtime}", model.add_time.ToString("yyyy-MM-dd HH:mm:ss"));
                    bodytxt = bodytxt.Replace("{orderno}", model.order_no);
                    bodytxt = bodytxt.Replace("{orderamount}", (model.real_freight != 0 ? "外送费：" + model.real_freight.ToString() : "") + "总计：" + model.order_amount.ToString());
                    bodytxt = bodytxt.Replace("{ordermessage}", model.message);
                    string rtn = string.Empty;
                    foreach (var item in model.order_goods)
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
                    try
                    {
                        DTMail.sendMail(siteConfig.emailstmp,
                            siteConfig.emailusername,
                            DESEncrypt.Decrypt(siteConfig.emailpassword),
                            siteConfig.emailnickname,
                            siteConfig.emailfrom,
                            model.email,
                            titletxt, bodytxt);
                        //区域所属管理员邮件地址
                        if (areaModel.ManagerId != null && areaModel.ManagerId != 0)
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
                //提交成功，返回URL
                context.Response.Write("{\"msg\":1, \"msgbox\":\"订单已成功提交！\"}");
            }
            else
            {
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
                    packageReqHandler.SetParameter("out_trade_no", model.order_no);     //商家订单号
                    packageReqHandler.SetParameter("total_fee", (model.order_amount * 100).ToString().Replace(".00", ""));                  //商品金额,以分为单位(money * 100).ToString()
#if DEBUG
                    packageReqHandler.SetParameter("spbill_create_ip", "112.238.70.141");   //用户的公网ip，不是商户服务器IP
#else
                packageReqHandler.SetParameter("spbill_create_ip", context.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
#endif
                    packageReqHandler.SetParameter("notify_url", System.Configuration.ConfigurationManager.AppSettings["TenPayV3_TenpayNativeNotify"]);         //接收财付通通知的URL
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
                        + ", \"code_url\":\"/api/payment/mppay_native/getqr.ashx?code_url=" + Utils.UrlEncode(code_url) + "\"}");
                }
                else
                {
                    ChargeRequest<string> get_req = new ChargeRequest<string>();
                    get_req.amount = model.order_amount;
                    get_req.out_order_no = model.order_no;
                    get_req.pay_channel = "wxpay";
#if DEBUG
                    get_req.ip = "119.180.116.79";
#else
                                                        get_req.ip = context.Request.UserHostAddress;
#endif
                    get_req.subject = siteConfig.webname + "微信订单";
                    get_req.return_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback.aspx";
                    get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_wxpay/feedback.aspx";
                    get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    get_req.device_id = System.Net.Dns.GetHostName();
                    get_req.charge_type = "pay";
                    get_req.account_id = "main";
                    ChargeResponse<string> get_rsp = Client.Execute(get_req);
                    int srcPoint = get_rsp.Result.Action.Params.IndexOf("src = \"") + 7;
                    int srcEndPoint = get_rsp.Result.Action.Params.IndexOf("\"", srcPoint);

                    context.Response.Write("{\"msg\":1,\"msgbox\":\"订单已成功提交！\",\"orderid\":" + result
                        + ", \"code_url\":\"" + get_rsp.Result.Action.Params.Substring(srcPoint, srcEndPoint - srcPoint) + "\"}");
                }


            }
        }
        #endregion

        #region 用户取消订单OK=================================
        private void order_cancel(HttpContext context)
        {
            //检查用户是否登录
            Model.users userModel = new BasePage().GetUserInfo();
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，用户没有登录或登录超时啦！\"}");
                return;
            }
            //检查订单是否存在
            string order_no = DTRequest.GetQueryString("order_no");
            Model.orders orderModel = new BLL.orders().GetModel(order_no);
            if (order_no == "" || orderModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，该订单号不存在啦！\"}");
                return;
            }
            //检查是否自己的订单
            if (userModel.id != orderModel.user_id)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，不能取消别人的订单状态！\"}");
                return;
            }
            //检查订单状态
            if (orderModel.status > 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，该订单不是生成状态，不能取消！\"}");
                return;
            }
            bool result = new BLL.orders().UpdateField(order_no, "status=4");
            if (!result)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，操作过程中发生不可遇知的错误！\"}");
                return;
            }
            //如果是积分换购则返还积分
            if (orderModel.point < 0)
            {
                new BLL.point_log().Add(orderModel.user_id, orderModel.user_name, -1 * orderModel.point, "取消订单，返还换购积分，订单号：" + orderModel.order_no);
            }
            context.Response.Write("{\"msg\":1, \"msgbox\":\"取消订单成功啦！\"}");
            return;
        }
        #endregion

        #region 通用外理方法OK=================================
        //校检验证码
        private string verify_code(HttpContext context, string strcode)
        {
            if (string.IsNullOrEmpty(strcode))
            {
                return "{\"msg\":0, \"msgbox\":\"对不起，请输入验证码！\"}";
            }
            if (context.Session[DTKeys.SESSION_CODE] == null)
            {
                return "{\"msg\":0, \"msgbox\":\"对不起，验证码超时或已过期！\"}";
            }
            if (strcode.ToLower() != (context.Session[DTKeys.SESSION_CODE].ToString()).ToLower())
            {
                return "{\"msg\":0, \"msgbox\":\"您输入的验证码与系统的不一致！\"}";
            }
            context.Session[DTKeys.SESSION_CODE] = null;
            return "success";
        }
        #endregion END通用方法=================================================

        private void GetGoodsList(HttpContext context)
        {
            string categoryid = DTRequest.GetFormString("categoryid");
            //string areaid = context.Request.Params["AreaId"];
            string areaid = DTRequest.GetFormString("areaid");
            if (string.IsNullOrEmpty(areaid))
            {
                areaid = "3";
            }
            DataTable dtcombo = null;
            DataTable dtgoods = null;
            dtcombo = new BookingFood.BLL.bf_good_combo().GetList(" CategoryId=" + categoryid
                + " and Id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='category' AND baa.AreaId=" + areaid + ") order by SortId desc").Tables[0];
            dtgoods = new BLL.article().GetGoodsList(9999, " channel_id!=3 and category_id=" + categoryid
                + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")", " sort_id desc").Tables[0];
            string rtn = string.Empty;
            rtn += "<ul>";
            decimal totalprice = 0;
            //上下架集合
            List<BookingFood.Model.bf_area_article> listAreaArticle = new BookingFood.BLL.bf_area_article().GetModelList(" AreaId=" + areaid);
            string islock = "<div class=\"mask_sold\"></div>";
            foreach (DataRow item in dtcombo.Rows)
            {
                totalprice = 0;
                BookingFood.Model.bf_area_article temp =
                    listAreaArticle.FirstOrDefault(s => s.Type == "category" && s.ArticleId.ToString() == item["Id"].ToString());
                islock = "<div class=\"mask_sold\"></div>";
                if (temp != null && temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                {
                    islock = "";
                }
                rtn += string.Format("<li class=\"l_item\" >" +
                    "<img src=\"{0}\" />" +
                    "{1}" +
                    "{2}" +
                    "<div class=\"info_btm\"></div>" +
                    "<div class=\"info_o\">" +
                    "    <span class=\"p_name\">{3}</span>" +
                     "   <span class=\"p_order\" data-id=\"{6}\" data-type=\"combo\">订购</span>" +
                     "   <span class=\"p_price\">{5}元</span>" +
                    "</div>" +
                    "{4}" +
                "</li>"
                , item["Photo"].ToString()
                , GetComboDetail(item["Id"].ToString(), areaid, out totalprice)
                , GetTaste(item["Taste"].ToString())
                , item["Title"].ToString()
                //, temp != null ? (temp.IsLock.ToString() == "1" ? "<div class=\"mask_sold\"></div>" : "") : ""
                , islock
                , totalprice.ToString().Replace(".00", "")
                , item["Id"].ToString());
            }
            BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
            foreach (DataRow item in dtgoods.Rows)
            {
                BookingFood.Model.bf_area_article temp =
                    listAreaArticle.FirstOrDefault(s => s.Type == "one" && s.ArticleId.ToString() == item["Id"].ToString());
                islock = "<div class=\"mask_sold\"></div>";
                if (temp != null && temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                {
                    islock = "";
                }
                List<BookingFood.Model.bf_article_low_num> lownum = bllLowNum.GetModelList("ArticleId=" + item["Id"].ToString() + " And "
                    + "CONVERT(varchar(12),GETDATE(),108) BETWEEN CONVERT(varchar(12),BeginTime,108) AND CONVERT(varchar(12), EndTime, 108)");
                rtn += string.Format("<li class=\"l_item\">" +
                    "<img src=\"{0}\" />" +
                    "{1}" +
                    "{2}" +
                    "<div class=\"info_btm\"></div>" +
                    "<div class=\"info_o\">" +
                    "    <span class=\"p_name\">{3}</span>" +
                     "   <span class=\"p_order\" data-id=\"{6}\" data-type=\"one\" data-low=\"{7}\">订购</span>" +
                     "   <span class=\"p_price\">{5}元</span>" +
                    "</div>" +
                    "{4}" +
                "</li>"
                , item["img_url"].ToString()
                , ""
                , GetTaste(item["Taste"].ToString())
                , item["title"].ToString()
                //, temp != null ? (temp.IsLock.ToString() == "1" ? "<div class=\"mask_sold\"></div>" : "") : ""
                , islock
                , item["sell_price"].ToString().Replace(".00", "")
                , item["id"].ToString()
                , lownum.Count > 0 ? lownum[0].LowNum.ToString() : "0");
            }
            rtn += "</ul>";
            context.Response.Write(rtn);
        }

        private string GetComboDetail(string comboid, string areaid, out decimal totalprice)
        {
            totalprice = 0;
            string rtn = string.Empty;
            List<BookingFood.Model.bf_good_combo_detail> list =
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + comboid + " Order By SortId Asc");
            BLL.article bllArticle = new BLL.article();
            string dataid = string.Empty;
            decimal dataprice = 0;
            string datatitle = string.Empty;
            string subtaste = string.Empty;
            foreach (var item in list)
            {
                string sub = string.Empty;
                dataid = string.Empty;
                dataprice = 0;
                datatitle = string.Empty;
                DataSet ds = bllArticle.GetGoodsList(99, " category_id=" + item.BusinessId.ToString()
                    + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + " AND IsLock=0)", " sort_id asc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sub += "<ul class=\"l_sub_item\">";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        subtaste = string.Empty;
                        if (i == 0)
                        {
                            dataid = ds.Tables[0].Rows[i]["id"].ToString();
                            dataprice = decimal.Parse(ds.Tables[0].Rows[i]["sell_price"].ToString());
                            datatitle = ds.Tables[0].Rows[i]["title"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["taste"].ToString()))
                        {
                            subtaste += "<div class=\"combo_sub_taste\">";
                            foreach (var taste in ds.Tables[0].Rows[i]["taste"].ToString().Split(','))
                            {
                                subtaste += "<span data-id=\"" + taste + "\" onclick=\"combosubtasteclick(this);\">* " + taste + "</span>";
                            }
                            subtaste += "<button onclick=\"acceptComboTaste(this);\">确定</button>";
                            subtaste += "</div>";
                            sub += string.Format("<li class=\"info_ii \" data-id=\"{1}\" data-price=\"{2}\" ><span>*{0}</span>{3}</li>"
                            , ds.Tables[0].Rows[i]["title"].ToString()
                            , ds.Tables[0].Rows[i]["id"].ToString()
                            , ds.Tables[0].Rows[i]["sell_price"].ToString()
                            , subtaste);
                        }
                        else
                        {
                            sub += string.Format("<li class=\"info_ii \" data-id=\"{1}\" data-price=\"{2}\" onclick=\"SubItemClick(this);\"><span>*{0}</span></li>"
                            , ds.Tables[0].Rows[i]["title"].ToString()
                            , ds.Tables[0].Rows[i]["id"].ToString()
                            , ds.Tables[0].Rows[i]["sell_price"].ToString());
                        }

                    }
                    sub += "</ul>";
                }
                rtn += string.Format("<div class=\"info_i orange\" data-id=\"{2}\" data-price=\"{3}\" data-title=\"{4}\" data-type=\"combo\"><span class=\"info_sub_title\">*{0}</span>{1}</div>"
                    , datatitle, sub, dataid, dataprice, datatitle);
                totalprice += dataprice;
            }
            return rtn;
        }

        private string GetTaste(string taste)
        {
            string rtn = string.Empty;
            if (!string.IsNullOrEmpty(taste))
            {
                string sub = string.Empty;
                string datatitle = string.Empty;
                string[] tastes = taste.Split(',');
                if (tastes.Length > 0)
                {
                    string style = string.Empty;
                    if (tastes.Length < 4)
                    {
                        style = "style=\"width:" + (90 / tastes.Length) + "%;\"";
                    }
                    foreach (var item in tastes)
                    {
                        //if (string.IsNullOrEmpty(datatitle))
                        //{
                        //    datatitle = item;
                        //    sub += string.Format("<span class=\"info_ii_taste select\" data-id=\"{0}\" onclick=\"SubTasteClick(this);\" >*{0}</span> "
                        //    , item);
                        //}
                        //else
                        //{
                        //    sub += string.Format("<span class=\"info_ii_taste \" data-id=\"{0}\" onclick=\"SubTasteClick(this);\" >*{0}</span> "
                        //    , item);
                        //}
                        sub += string.Format("<span class=\"info_ii_taste \" data-id=\"{0}\" onclick=\"SubTasteClick(this);\" {1}>*{0}</span> "
                            , item, style);
                    }
                }
                rtn += string.Format("<div class=\"info_i orangetaste\" data-id=\"\" data-title=\"\" data-type=\"taste\" >{0}</div>", sub);
            }
            return rtn;
        }

        private void GetUserInfo(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel != null)
            {
                context.Response.Write("{\"msg\":1,\"address\":\"" + userModel.address + "\", \"id\":" + userModel.id.ToString() + "}");
                return;
            }
            context.Response.Write("{\"msg\":0}");
        }

        private void ShowCartGoods(HttpContext context)
        {
            string email = context.Request.Params["email"];
            string additional = context.Request.Params["additional"];
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (DateTime.Now < DateTime.Parse(model.starttime) || DateTime.Now > DateTime.Parse(model.endtime))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + model.starttime + "之" + model.endtime + "之间订餐！\"}");
                return;
            }
            decimal distributionamount = siteConfig.disamount;
            decimal totalmoney = 0;
            decimal total = 0;
            //查找会员组
            int group_id = 0;
            Model.user_groups groupModel = new BLL.user_groups().GetDefault();
            group_id = groupModel.id;
            IList<Model.cart_items> listCart = Web.UI.ShopCart.GetList(group_id);
            if (listCart == null || listCart.Count == 0)
            {
                if (string.Equals(email, model.IsNullCartAccount))
                {
                    context.Response.Write("{\"msg\":2}");
                    return;
                }
                else
                {
                    context.Response.Write("{\"msg\":0}");
                    return;
                }

            }
            string rtn = "";
            foreach (var item in listCart)
            {
                totalmoney += item.price * item.quantity;
                if (item.type == "one")
                {
                    rtn += string.Format("<tr>" +
                            "<td class='c_1'>" +
                                "{1} {4}" +
                            "</td>" +
                            "<td class='c_2'>" +
                                "￥{2}" +
                            "</td>" +
                            "<td class='c_3'>" +
                                "{0}" +
                            "</td>" +
                            "<td class='c_4'>" +
                                "￥{3}" +
                            "</td>" +
                        "</tr>"
                        , item.quantity
                        , item.title
                        , item.price.ToString().Replace(".00", "")
                        , (item.quantity * item.price).ToString().Replace(".00", "")
                        , !string.IsNullOrEmpty(item.subgoodsid) ? "(" + item.subgoodsid.Split('‡')[2] + ")" : "");
                }
                else if (item.type == "combo")
                {
                    rtn += string.Format("<tr>" +
                            "<td class='c_1'>" +
                                "{1}" +
                            "</td>" +
                            "<td class='c_2'>" +
                                "￥{2}" +
                            "</td>" +
                            "<td class='c_3'>" +
                                "{0}" +
                            "</td>" +
                            "<td class='c_4'>" +
                                "￥{3}" +
                            "</td>" +
                        "</tr>", item.quantity, item.title, item.price.ToString().Replace(".00", "")
                        , (item.quantity * item.price).ToString().Replace(".00", ""));
                    string[] subgoods = item.subgoodsid.Split('†');
                    string subrtn = string.Empty;
                    foreach (var sub in subgoods)
                    {
                        subrtn += "* " + sub.Split('‡')[2] + " " + (sub.Split('‡').Length == 4 ? "(" + sub.Split('‡')[3] + ")" : "");
                    }
                    rtn += string.Format("<tr>" +
                            "<td class='c_1' colspan='4'>" +
                                "{0}" +
                            "</td>" +
                            "</tr>", subrtn);
                }
            }
            if (totalmoney >= siteConfig.freedisamount || additional == "1")
            {
                distributionamount = 0;
            }
            total = totalmoney + distributionamount;
            context.Response.Write("{\"msg\":1,\"ordertime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + "\", \"distributionamount\":" + distributionamount + ", \"amount\":" + totalmoney
                + ", \"total\":" + total
                + ",\"msgbox\":\"" + rtn.Replace("\b", "") + "\"" + "}");
        }

        private void GetLastOrder(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0}");
                return;
            }
            DataTable dt = new BLL.orders().GetList(1, "user_id=" + userModel.id.ToString() + " and status in (1,2) and is_additional=0", " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":0}");
                return;
            }
            Model.orders model = new BLL.orders().GetModel(int.Parse(dt.Rows[0]["id"].ToString()));
            decimal distributionamount = model.real_freight;
            decimal totalmoney = 0;
            decimal total = 0;
            string rtn = "";
            if (model.order_goods != null)
            {
                GetOrderDetailForPc(model, ref totalmoney, ref rtn);
            }
            if (model.additional_count > 0)
            {
                Model.orders modelAdditional = new BLL.orders().GetModel(int.Parse(model.additional_id));
                if (modelAdditional.order_goods != null)
                {
                    GetOrderDetailForPc(modelAdditional, ref totalmoney, ref rtn);
                }
            }

            total = totalmoney + distributionamount;
            string status = string.Empty;
            if (model.status == 1)
            {
                status = "1";
            }
            else if (model.status == 2 && model.distribution_status == 1)
            {
                status = "2";
            }
            else if (model.status == 2 && model.distribution_status == 2)
            {
                status = "3";
            }
            context.Response.Write("{\"msg\":1,\"ordertime\":\"" + model.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                + "\", \"distributionamount\":" + distributionamount + ", \"amount\":" + totalmoney
                + ", \"total\":" + total + ", \"address\":\"" + model.address + "\""
                + ", \"email\":\"" + model.email + "\"" + ", \"telphone\":\"" + model.telphone + "\""
                + ", \"message\":\"" + model.message.Replace("\n", "").Replace("\r\n", "").Replace("\t", "")
                + "\"" + ", \"status\":" + status + "" + ",\"orderno\":\"" + model.order_no + "\""
                + ",\"msgbox\":\"" + rtn + "\"" + ",\"payment_status\":" + model.payment_status + "}");
        }

        private static void GetOrderDetailForPc(Model.orders model, ref decimal totalmoney, ref string rtn)
        {
            foreach (var item in model.order_goods)
            {
                totalmoney += item.goods_price * item.quantity;
                if (item.type == "one")
                {
                    rtn += string.Format("<tr>" +
                            "<td class='c_1'>" +
                                "{1} {4}" +
                            "</td>" +
                            "<td class='c_2'>" +
                                "￥{2}" +
                            "</td>" +
                            "<td class='c_3'>" +
                                "{0}" +
                            "</td>" +
                            "<td class='c_4'>" +
                                "￥{3}" +
                            "</td>" +
                        "</tr>", item.quantity, item.goods_name, item.goods_price.ToString().Replace(".00", "")
                        , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                        , !string.IsNullOrEmpty(item.subgoodsid) ? "(" + item.subgoodsid.Split('‡')[2] + ")" : "");
                }
                else if (item.type == "combo")
                {
                    rtn += string.Format("<tr>" +
                            "<td class='c_1'>" +
                                "{1}" +
                            "</td>" +
                            "<td class='c_2'>" +
                                "￥{2}" +
                            "</td>" +
                            "<td class='c_3'>" +
                                "{0}" +
                            "</td>" +
                            "<td class='c_4'>" +
                                "￥{3}" +
                            "</td>" +
                        "</tr>", item.quantity, item.goods_name, item.goods_price.ToString().Replace(".00", "")
                        , (item.quantity * item.goods_price).ToString().Replace(".00", ""));
                    string[] subgoods = item.subgoodsid.Split('†');
                    string subrtn = string.Empty;
                    foreach (var sub in subgoods)
                    {
                        subrtn += "* " + sub.Split('‡')[2] + " " + (sub.Split('‡').Length == 4 ? "(" + sub.Split('‡')[3] + ")" : "");
                    }
                    rtn += string.Format("<tr>" +
                            "<td class='c_1' colspan='4'>" +
                                "{0}" +
                            "</td>" +
                            "</tr>", subrtn);
                }
            }
        }

        private void GetArea(HttpContext context)
        {
            string areaid = DTRequest.GetFormString("areaid");
            if (string.IsNullOrEmpty(areaid)) return;
            List<BookingFood.Model.bf_area> areaList =
                new BookingFood.BLL.bf_area().GetModelList(" IsShow=1 AND ParentId=" + areaid + " Order BY SortId Asc");
            string rtn = string.Empty;
            rtn += "<div class=\"balls_tank\">";
            for (int i = 0; i < areaList.Count; i++)
            {
                rtn += string.Format("<span data-id=\"{1}\" >{0}</span>", areaList[i].Title, areaList[i].Id.ToString()
                    , (i == 0 ? "class=\"ball hover\"" : ""));
            }
            rtn += "</div>";
            rtn += "<div class=\"balls_content\">";
            for (int i = 0; i < areaList.Count; i++)
            {
                rtn += string.Format("<div {1} >{0}</span>", areaList[i].Description
                    , (i != 0 ? "style=\"display: none\"" : ""));
            }
            rtn += "</div>";
            rtn += "<span class=\"clearfix\"></span>";
            context.Response.Write(rtn);
        }

        private void GetAreaType(HttpContext context)
        {
            int id = int.Parse(context.Request.Params["id"]);
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
            BookingFood.Model.bf_area bo = bll.GetModel(id);
            if (bo != null)
            {
                context.Response.Write("{\"busy\":" + bo.IsBusy.ToString() + ", \"lock\":" + bo.IsLock.ToString() + "}");
            }
            else
            {
                context.Response.Write("{\"busy\":0, \"lock\":0}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 检测输入的邮件地址strEmail是否合法，非法则返回true。
        /// </summary>
        public bool CheckEmail(string strEmail)
        {
            int i, j;
            string strTmp, strResult;
            string strWords = "abcdefghijklmnopqrstuvwxyz_-.0123456789"; //定义合法字符范围
            bool blResult = false;
            strTmp = strEmail.Trim();
            //检测输入字符串是否为空，不为空时才执行代码。
            if (!(strTmp == "" || strTmp.Length == 0))
            {
                //判断邮件地址中是否存在“@”号
                if ((strTmp.IndexOf("@") < 0))
                {
                    blResult = true;
                    return blResult;
                }
                //以“@”号为分割符，把地址切分成两部分，分别进行验证。
                string[] strChars = strTmp.Split(new char[] { '@' });
                foreach (string strChar in strChars)
                {
                    i = strChar.Length;
                    //“@”号前部分或后部分为空时。
                    if (i == 0)
                    {
                        blResult = true;
                        return blResult;
                    }
                    //逐个字进行验证，如果超出所定义的字符范围strWords，则表示地址非法。
                    for (j = 0; j < i; j++)
                    {
                        strResult = strChar.Substring(j, 1).ToLower();//逐个字符取出比较
                        if (strWords.IndexOf(strResult) < 0)
                        {
                            blResult = true;
                            return blResult;
                        }
                    }
                }
            }
            return blResult;
        }

        private void DoQuick(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0,\"msgbox\":\"未找到对应的用户！\"}");
                return;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_id=" + userModel.id + " and (add_time between '"
                + time + " 00:00:00' and '" + time + " 23:59:59') and OrderType!='催单' and is_additional=0", " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"您今天还没有提交过订单！\"}");
                return;
            }
            if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(40) >= DateTime.Now)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"抱歉，下单40分钟后可以催单！\"}");
                return;
            }
            if (DateTime.Parse("11:40") >= DateTime.Now)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"11点开始配送，正在配送中，请稍候！\"}");
                return;
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["distribution_time"].ToString()) &&
                DateTime.Parse(dt.Rows[0]["distribution_time"].ToString()).AddHours(1) < DateTime.Now)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"派送后一小时内可用！\"}");
                return;
            }
            if (dt.Rows[0]["is_quick"].ToString() == "0")
            {
                BLL.orders bllOrder = new BLL.orders();
                int orderid = int.Parse(dt.Rows[0]["id"].ToString());
                Model.orders orderModel = bllOrder.GetModel(orderid);
                orderModel.OrderType = "催单";
                orderModel.message = "[催单]";
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
                bllOrder.UpdateField(orderid, "is_quick=1");
            }
            context.Response.Write("{\"msg\":0,\"msgbox\":\"催单成功，我们将极速处理\"}");
        }

        private void CheckLess(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0,\"msgbox\":\"未找到对应的用户！\"}");
                return;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_id=" + userModel.id + " and add_time between '" + time + " 00:00:00' and '" + time + " 23:59:59'"
                + " and OrderType!='催单' and OrderType!='通知' and is_additional=0 "
                , " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"您今天没有下单，无法补充送错信息！\"}");
                return;
            }
            if (dt.Rows[0]["is_less"].ToString() == "2")
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"已经补充过信息，无法再次补充！\"}");
                return;
            }
            context.Response.Write("{\"msg\":0}");
        }

        private void AddPcLess(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0,\"msgbox\":\"未找到对应的用户！\"}");
                return;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_id=" + userModel.id + " and add_time between '" + time + " 00:00:00' and '" + time + " 23:59:59'"
                + " and OrderType!='催单' and OrderType!='通知' and is_additional=0 "
                , " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"您今天没有下单，无法补充送错信息！\"}");
                return;
            }
            if (dt.Rows[0]["is_less"].ToString() == "2")
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"已经补充过信息，无法再次补充！\"}");
                return;
            }
            string source = DTRequest.GetFormString("source");
            BLL.orders bllOrder = new BLL.orders();
            Model.orders orderModel = bllOrder.GetModel(int.Parse(dt.Rows[0]["id"].ToString()));
            orderModel.OrderType = "通知";
            orderModel.message = "[" + source + "]";
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
            bllOrder.UpdateField(int.Parse(dt.Rows[0]["id"].ToString()), " is_less=is_less+1");
            context.Response.Write("{\"msg\":0, \"msgbox\":\"您的反馈已直达店铺，并立即处理，也请保持电话畅通！！\"}");
        }

        private void CheckAdditional(HttpContext context)
        {
            string email = DTRequest.GetFormString("email");
            string phone = DTRequest.GetFormString("phone");
            Model.users userModel = new BLL.users().GetModel(email, phone);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0,\"msgbox\":\"未找到对应的用户！\"}");
                return;
            }
            DataTable dt = new BLL.orders().GetList(1
               , " user_id=" + userModel.id + " and OrderType!='催单' and OrderType!='通知' and is_additional=0", " id desc").Tables[0];

            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"您还没有提交过订单！\"}");
                return;
            }
            //if (dt.Rows[0]["additional_count"].ToString() == "2")
            //{
            //    context.Response.Write("{\"msg\":1,\"msgbox\":\"只能提交两次补单！\"}");
            //    return;
            //}
            if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(siteConfig.additional) < DateTime.Now)
            {
                context.Response.Write("{\"msg\":1,\"msgbox\":\"抱歉，已超出" + siteConfig.additional + "分钟补单时间，需再满24元起送！\"}");
                return;
            }
            context.Response.Write("{\"msg\":0}");
        }

        private void GetMpPayResult(HttpContext context)
        {
            int orderid = DTRequest.GetFormInt("id");
            BLL.orders bll = new BLL.orders();
            Model.orders model = bll.GetModel(orderid);
            if (model != null)
            {
                context.Response.Write("{\"msg\":1,\"payment_status\":" + model.payment_status + "}");
            }
            else
            {
                context.Response.Write("{\"msg\":0}");
            }
        }

        #endregion

        #region MP
        private void MP_GetGoodsList(HttpContext context)
        {
            string categoryid = DTRequest.GetFormString("categoryid");
            //string areaid = context.Request.Params["AreaId"];
            string areaid = DTRequest.GetFormString("areaid");
            BookingFood.Model.bf_area modelArea = new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
            DataTable dtcombo = null;
            DataTable dtgoods = null;
            dtcombo = new BookingFood.BLL.bf_good_combo().GetList(" CategoryId=" + categoryid
                + " and Id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='category' AND baa.AreaId=" + areaid + ") order by SortId desc").Tables[0];
            dtgoods = new BLL.article().GetGoodsList(9999, " channel_id!=3 and category_id=" + categoryid
                + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")", " sort_id desc").Tables[0];
            string rtn = string.Empty;
            decimal totalprice = 0;
            //上下架集合
            List<BookingFood.Model.bf_area_article> listAreaArticle = new BookingFood.BLL.bf_area_article().GetModelList(" AreaId=" + areaid);
            string islock = "<div class=\"sold\"></div>";
            string comboorder = "";
            foreach (DataRow item in dtcombo.Rows)
            {
                totalprice = 0;
                BookingFood.Model.bf_area_article temp =
                    listAreaArticle.FirstOrDefault(s => s.Type == "category" && s.ArticleId.ToString() == item["Id"].ToString());
                islock = "<div class=\"sold\"></div>";
                comboorder = "";
                if (temp != null && temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                {
                    islock = "";
                    comboorder = "onclick=\"comboOrder(this);\"";
                }
                rtn += string.Format("<ul class=\"suit\">" +
                                    "<li class=\"title\" data-id=\"{3}\" data-type=\"combo\" {5}> " +
                                        "{4}" +
                                        "{1}" +
                                        "<div class=\"price\">{2}</div>" +
                                    "</li>" +
                                    "{0}" +
                                "</ul><br clear='all' />"
                                , GetMpComboDetail(item["Id"].ToString(), modelArea, out totalprice)
                                , item["Title"].ToString().Length > 2 ?
                                    "<div class=\"kind\">" + item["Title"].ToString().Substring(0, 2) + "</div><div class=\"name\">" + item["Title"].ToString().Substring(2) + "</div>" :
                                    "<div class=\"name\" style=\"padding-top: 14px;\">" + item["Title"].ToString() + "</div>"
                                , totalprice.ToString().Replace(".00", "")
                                , item["Id"].ToString()
                                , islock
                                , comboorder);
            }

            if (dtgoods.Rows.Count > 0)
            {
                rtn += "<ul class=\"suit\">";
                int loop_one = 0;
                string defaulttaste = string.Empty;
                BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
                foreach (DataRow item in dtgoods.Rows)
                {
                    defaulttaste = string.Empty;
                    loop_one++;
                    BookingFood.Model.bf_area_article temp =
                        listAreaArticle.FirstOrDefault(s => s.Type == "one" && s.ArticleId.ToString() == item["Id"].ToString());
                    islock = "<div class=\"sold\"></div>";
                    comboorder = "none";
                    if (temp != null && temp.IsLock == 0 && (temp.GuQingDate == null || ((DateTime)temp.GuQingDate).Date != DateTime.Now.Date))
                    {
                        islock = "";
                        comboorder = "";
                    }
                    List<BookingFood.Model.bf_article_low_num> lownum = bllLowNum.GetModelList("ArticleId=" + item["Id"].ToString() + " And "
                    + "CONVERT(varchar(12),GETDATE(),108) BETWEEN CONVERT(varchar(12),BeginTime,108) AND CONVERT(varchar(12), EndTime, 108)");
                    rtn += string.Format("<li>" +
                                        "<div class=\"item one selected {8}\" data-id=\"{3}\" data-type=\"one\" data-low=\"{9}\" style=\"{10}\">" +
                                        "{7}" +
                                        "{6}" +
                                        "{4}" +
                                        "{1}" +
                                        "<div class=\"price\" style=\"display:none;\">{2}</div>" +
                                        "</div>" +
                                        "{0}" +
                                    "</li>"
                                    , GetMpTaste(item["id"].ToString(), item["Taste"].ToString(), item["condition_price"].ToString(), modelArea, ref defaulttaste)
                                    , "<div class=\"name\" >" + item["Title"].ToString() + "</div>"
                                    , item["sell_price"].ToString().Replace(".00", "")
                                    , item["id"].ToString()
                                    , islock
                                    , ""
                                    //, !string.IsNullOrEmpty(defaulttaste) ? "<div class=\"name taste\" style=\"display:none;\">" + defaulttaste + "</div>" : ""
                                    , "<div class=\"name taste\" style=\"display:none;\"></div>"
                                    , "<div class=\"tag hot " + comboorder + "\">" + item["sell_price"].ToString().Replace(".00", "") + "</div>"
                                    , string.IsNullOrEmpty(item["Taste"].ToString()) && string.IsNullOrEmpty(item["condition_price"].ToString()) ? "nosub" : ""
                                    , lownum.Count > 0 ? lownum[0].LowNum.ToString() : "0"
                                    , string.IsNullOrEmpty(item["mp_img_url"].ToString()) ? "" : "background: url(" + item["mp_img_url"].ToString() + ");"
                                    );
                    if (loop_one % 4 == 0)
                    {
                        rtn += "</ul><ul class=\"suit\">";
                    }
                }
                rtn += "</ul>";
            }

            context.Response.Write(rtn);
        }

        private string GetMpComboDetail(string comboid, BookingFood.Model.bf_area modelArea, out decimal totalprice)
        {
            totalprice = 0;
            string rtn = string.Empty;
            List<BookingFood.Model.bf_good_combo_detail> list =
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + comboid + " Order By SortId Asc");
            BLL.article bllArticle = new BLL.article();
            string dataid = string.Empty;
            decimal dataprice = 0;
            string datatitle = string.Empty;
            string defaulttaste = string.Empty;
            string tastearea = string.Empty;
            foreach (var item in list)
            {
                string sub = string.Empty;
                dataid = string.Empty;
                dataprice = 0;
                datatitle = string.Empty;
                tastearea = string.Empty;
                DataSet ds = bllArticle.GetGoodsList(99, " category_id=" + item.BusinessId.ToString()
                    + " and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + modelArea.Id + " AND IsLock=0)", " sort_id asc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sub += "<li>";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            dataid = ds.Tables[0].Rows[i]["id"].ToString();
                            dataprice = decimal.Parse(ds.Tables[0].Rows[i]["sell_price"].ToString());
                            datatitle = ds.Tables[0].Rows[i]["title"].ToString();
                        }
                        defaulttaste = string.Empty;
                        tastearea += GetComboMpTaste(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Taste"].ToString()
                                , ds.Tables[0].Rows[i]["condition_price"].ToString(), modelArea, ref defaulttaste);
                        sub += string.Format("<div class=\"item combo {3} {6}\" data-id=\"{1}\" data-price=\"{2}\" data-type=\"combo\" style=\"{7}\">" +
                                                "<div class=\"duigou\"></div>" +
                                                "{5}" +
                                                "<div class=\"name\">{0}</div>" +
                                                "<div class=\"price\" style=\"display:none;{4}\">{2}</div>" +

                                                "{8}" +
                                            "</div>"
                                            , ds.Tables[0].Rows[i]["title"].ToString()
                                            , ds.Tables[0].Rows[i]["id"].ToString()
                                            , ds.Tables[0].Rows[i]["sell_price"].ToString().Replace(".00", "")
                                            , i == 0 ? "selected" : "hide"
                                            , ds.Tables[0].Rows[i]["title"].ToString().Length > 5 ? "line-height: 20px;" : ""
                                            , !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["mark"].ToString()) ? "<div class=\"tag hot\">" + ds.Tables[0].Rows[i]["mark"].ToString() + "</div>" : ""
                                            , ds.Tables[0].Rows.Count == 1 ? "nosub" : ""
                                            , string.IsNullOrEmpty(ds.Tables[0].Rows[i]["mp_img_url"].ToString()) ? "" : "background: url(" + ds.Tables[0].Rows[i]["mp_img_url"].ToString() + ");"
                                            //, !string.IsNullOrEmpty(defaulttaste) ? "<div class=\"name taste\" style=\"display:none;\">" + defaulttaste + "</div>" : ""
                                            , "<div class=\"name taste\" style=\"display:none;\"></div>"
                                            );
                    }
                    if (tastearea.Length > 0)
                    {
                        sub += tastearea;
                        sub += "<div class=\"comboconfirm\">确定</div>";
                    }

                    sub += "</li>";
                }
                totalprice += dataprice;
                rtn += sub;
            }
            return rtn;
        }

        private string GetMpTaste(string articleid, string taste, string condition_price, BookingFood.Model.bf_area modelArea, ref string defaulttaste)
        {
            string rtn = string.Empty;
            if (string.IsNullOrEmpty(taste) && string.IsNullOrEmpty(condition_price)) return rtn;
            string sub = string.Empty;
            string datatitle = string.Empty;
            string[] tastes = taste.Split(',');
            string[] conditions = condition_price.Split(',');
            if (tastes.Length > 0 || conditions.Length > 0)
            {
                sub += "<div class=\"tastearea\" data-id=\"" + articleid + "\">";
                foreach (var item in tastes)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    //if (string.IsNullOrEmpty(defaulttaste))
                    //{
                    //    defaulttaste = item;
                    //    sub += string.Format("<div class=\"item taste hide active\" data-id=\"{0}\" data-type=\"taste\" >" +
                    //                        "<div class=\"duigou\"></div>" +
                    //                        "<div class=\"tastename\">{0}</div>" +
                    //                    "</div>"
                    //                    , item);
                    //}
                    //else
                    //{
                    sub += string.Format("<div class=\"item taste hide\" data-id=\"{0}\" data-type=\"taste\" >" +
                                        "<div class=\"duigou\"></div>" +
                                        "<div class=\"tastename\">{0}</div>" +
                                    "</div>"
                                    , item);
                    //}

                }
                if (modelArea.IsShow == 0)
                {
                    foreach (var item in conditions)
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        //if (string.IsNullOrEmpty(defaulttaste))
                        //{
                        //    defaulttaste = item;
                        //    sub += string.Format("<div class=\"item taste hide active\" data-id=\"{0}\" data-type=\"taste\" >" +
                        //                        "<div class=\"duigou\"></div>" +
                        //                        "<div class=\"tastename\">{0}</div>" +
                        //                    "</div>"
                        //                    , item.Split('†')[0]);
                        //}
                        //else
                        //{
                        sub += string.Format("<div class=\"item taste hide\" data-id=\"{0}\" data-type=\"taste\" >" +
                                            "<div class=\"duigou\"></div>" +
                                            "<div class=\"tastename\">{0}</div>" +
                                        "</div>"
                                        , item.Split('†')[0]);
                        //}

                    }
                }

                sub += "</div>";
                if (sub.Length > 0) sub += "<div class=\"confirm\">确定</div>";
            }
            rtn = sub;
            return rtn;
        }

        private string GetComboMpTaste(string articleid, string taste, string condition_price, BookingFood.Model.bf_area modelArea, ref string defaulttaste)
        {
            string rtn = string.Empty;
            if (string.IsNullOrEmpty(taste) && string.IsNullOrEmpty(condition_price)) return rtn;
            string sub = string.Empty;
            string datatitle = string.Empty;
            string[] tastes = taste.Split(',');
            string[] conditions = condition_price.Split(',');
            if (tastes.Length > 0 || conditions.Length > 0)
            {
                sub += "<div class=\"combotastearea\" data-id=\"" + articleid + "\">";
                foreach (var item in tastes)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    //if (string.IsNullOrEmpty(defaulttaste))
                    //{
                    //    defaulttaste = item;
                    //    sub += string.Format("<div class=\"item taste hide active\" data-id=\"{0}\" data-type=\"taste\" >" +
                    //                        "<div class=\"duigou\"></div>" +
                    //                        "<div class=\"tastename\">{0}</div>" +
                    //                    "</div>"
                    //                    , item);
                    //}
                    //else
                    //{
                    sub += string.Format("<div class=\"item taste hide\" data-id=\"{0}\" data-type=\"taste\" >" +
                                        "<div class=\"duigou\"></div>" +
                                        "<div class=\"tastename\">{0}</div>" +
                                    "</div>"
                                    , item);
                    //}

                }
                if (modelArea.IsShow == 0)
                {
                    foreach (var item in conditions)
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        //if (string.IsNullOrEmpty(defaulttaste))
                        //{
                        //    defaulttaste = item;
                        //    sub += string.Format("<div class=\"item taste hide active\" data-id=\"{0}\" data-type=\"taste\" >" +
                        //                        "<div class=\"duigou\"></div>" +
                        //                        "<div class=\"tastename\">{0}</div>" +
                        //                    "</div>"
                        //                    , item.Split('†')[0]);
                        //}
                        //else
                        //{
                        sub += string.Format("<div class=\"item taste hide\" data-id=\"{0}\" data-type=\"taste\" >" +
                                            "<div class=\"duigou\"></div>" +
                                            "<div class=\"tastename\">{0}</div>" +
                                        "</div>"
                                        , item.Split('†')[0]);
                        //}

                    }
                }
                sub += "</div>";
            }
            rtn = sub;
            return rtn;
        }

        private void GetMpLastOrder(HttpContext context)
        {
            string openid = DTRequest.GetFormString("openid");
            Model.users userModel = new BLL.users().GetModel(openid);
            if (userModel == null)
            {
                context.Response.Write("{\"msg\":0}");
                return;
            }
            DataTable dt = new BLL.orders().GetList(1, "user_id=" + userModel.id.ToString() + " and (status=2 or (status=1 and payment_id in (1,2)) or (status=1 and (payment_id=3 or payment_id=5) and payment_status=2)) and OrderType in ('微信','线下订单') and is_additional=0", " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":0}");
                return;
            }
            Model.orders model = new BLL.orders().GetModel(int.Parse(dt.Rows[0]["id"].ToString()));
            decimal distributionamount = model.real_freight;
            decimal totalmoney = 0;
            decimal total = 0;
            string rtn = "";
            if (model.order_goods != null)
            {
                GetOrderDetailForMp(model, ref totalmoney, ref rtn);
            }
            if (model.additional_count > 0)
            {
                Model.orders modelAdditional = new BLL.orders().GetModel(int.Parse(model.additional_id));
                if (modelAdditional.order_goods != null)
                {
                    GetOrderDetailForMp(modelAdditional, ref totalmoney, ref rtn);
                }
            }
            if (totalmoney < 24 && model.MpForHere == "" && model.is_additional != 1)
            {
                rtn += "<li><span class='count'> </span><span class='name'>订单不满24元加5元配送费</span><span class='price'>5</span></li>";
            }
            total = totalmoney + distributionamount;
            string status = string.Empty;
            if (string.IsNullOrEmpty(model.MpForHere))
            {
                if (model.status == 1)
                {
                    status = "1";
                }
                else if (model.status == 2 && model.distribution_status == 1)
                {
                    status = "2";
                }
                else if (model.status == 2 && model.distribution_status == 2)
                {
                    status = "3";
                }
            }
            else
            {
                status = "0";
            }
            context.Response.Write("{\"msg\":1,\"ordertime\":\"" + model.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                + "\", \"distributionamount\":" + distributionamount + ", \"amount\":" + totalmoney
                + ", \"total\":" + total + ", \"address\":\"" + model.address + "\""
                + ", \"email\":\"" + model.email + "\"" + ", \"telphone\":\"" + model.telphone + "\""
                + ", \"message\":\"" + model.message + "\"" + ", \"status\":" + status + "" + ",\"orderno\":\"" + model.order_no + "\""
                + ",\"msgbox\":\"" + rtn + "\"" + "}");
        }

        private static void GetOrderDetailForMp(Model.orders model, ref decimal totalmoney, ref string rtn)
        {
            foreach (var item in model.order_goods)
            {
                totalmoney += item.goods_price * item.quantity;
                if (item.type == "one")
                {
                    rtn += string.Format("<li>" +
                                            "<span class='count'>{0}</span>" +
                                            "<span class='name'>{1}{3}</span>" +
                                            "<span class='price'>{2}</span>" +
                                        "</li>"
                                        , item.quantity, item.goods_name
                                        , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                        , !string.IsNullOrEmpty(item.subgoodsid) ? "/" + item.subgoodsid.Split('‡')[2] : "");
                }
                else if (item.type == "combo")
                {
                    string[] subgoods = item.subgoodsid.Split('†');
                    string subrtn = string.Empty;
                    foreach (var sub in subgoods)
                    {
                        subrtn += string.Format("<p>*{0}</p>", sub.Split('‡')[2] + (sub.Split('‡').Length >= 4 && !string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : ""));
                    }
                    rtn += string.Format("<li>" +
                                            "<span class='count'>{0}</span>" +
                                            "<span class='name'>{1}{3}</span>" +
                                            "<span class='price'>{2}</span>" +
                                        "</li>"
                                        , item.quantity
                                        , item.goods_name
                                        , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                        , subrtn);

                }
                else if (item.type == "full" || item.type == "discount")
                {
                    rtn += string.Format("<li>" +
                                            "<span class='count' style='color: rgb(233,84,18);'>{0}</span>" +
                                            "<span class='name' style='color: rgb(233,84,18);'>{1}{3}</span>" +
                                            "<span class='price' style='color: rgb(233,84,18);'>{2}</span>" +
                                        "</li>"
                                        , item.quantity, item.goods_name
                                        , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                        , !string.IsNullOrEmpty(item.subgoodsid) ? "/" + item.subgoodsid.Split('‡')[2] : "");
                }
            }
        }

        private void mp_ordersave(HttpContext context)
        {
            int takeout = DTRequest.GetFormInt("takeout");
            BLL.orders bllOrders = new BLL.orders();
            //判断是否在订餐时间内
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig siteConfig = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (takeout == 0 && (DateTime.Now < DateTime.Parse(siteConfig.starttime) || DateTime.Now > DateTime.Parse(siteConfig.endtime)))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到订餐时间，请于" + siteConfig.starttime + "之" + siteConfig.endtime + "之间订餐！\"}");
                return;
            }
            else if (takeout > 0 && (DateTime.Now < DateTime.Parse(siteConfig.starttime_here) || DateTime.Now > DateTime.Parse(siteConfig.endtime_here)))
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您好。未到堂吃/外带时间，请于" + siteConfig.starttime + "之" + siteConfig.endtime + "之间订餐！\"}");
                return;
            }
            int payment_id = 1;
            int distribution_id = 1;
            string openid = DTRequest.GetFormString("openid");
            string telphone = DTRequest.GetFormString("phone");
            string nickname = DTRequest.GetFormString("nickname");
            string address = DTRequest.GetFormString("address");
            string message = DTRequest.GetFormString("message");
            string areaid = DTRequest.GetFormString("areaid");
            int paymentid = DTRequest.GetFormInt("paymentid");
            string additional = DTRequest.GetFormString("additional");
            string state = DTRequest.GetFormString("state");
            int dismountid = DTRequest.GetFormInt("dismountid");
            if (dismountid == 0) dismountid = 1;
            string inorout = DTRequest.GetFormString("inorout");
            string remark = DTRequest.GetFormString("remark");
            #region 检查 各种检查
            //检查配送方式
            if (distribution_id == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请选择配送方式！\"}");
                return;
            }
            Model.distribution disModel = new BLL.distribution().GetModel(distribution_id);
            if (disModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您选择的配送方式不存在或已删除！\"}");
                return;
            }
            if (paymentid == 3) payment_id = 3;
            if (paymentid == 5) payment_id = 5;
            //检查支付方式
            if (payment_id == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请选择支付方式！\"}");
                return;
            }
            Model.payment payModel = new BLL.payment().GetModel(payment_id);
            if (payModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，您选择的支付方式不存在或已删除！\"}");
                return;
            }
            //检查地址
            if (string.IsNullOrEmpty(address) && takeout == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，请输入详细的收货地址！\"}");
                return;
            }

            //检查用户是否登录
            BLL.users bllUsers = new BLL.users();
            Model.users userModel = bllUsers.GetModel(openid);
            if (userModel == null)
            {
                userModel = new Model.users();
                Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                userModel.group_id = modelGroup.id;
                userModel.user_name = openid;
                userModel.nick_name = "微信用户";
                userModel.qq = nickname;
                userModel.password = DESEncrypt.Encrypt("111111");
                userModel.email = "";
                userModel.telphone = telphone;
                userModel.address = address;
                userModel.reg_time = DateTime.Now;
                userModel.is_lock = userConfig.regverify; //设置为对应状态
                int newId = bllUsers.Add(userModel);
                userModel.id = newId;
            }
            else
            {
                userModel.address = address;
                userModel.qq = nickname;
                if (!string.IsNullOrEmpty(telphone))
                {
                    userModel.telphone = telphone;
                }
                bllUsers.Update(userModel);
            }
            //检查购物车商品

            BLL.article bllArticle = new BLL.article();
            IList<Model.cart_items> iList = DTcms.Web.UI.ShopCart.GetList(userModel.group_id, takeout);
            //if (iList == null)
            //{
            //    context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，购物车为空，无法结算！\"}");
            //    return;
            //}
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            BookingFood.Model.bf_area areaModel = bllArea.GetModel(int.Parse(areaid));
            if (areaModel == null)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"对不起，未选择外卖区域！\"}");
                return;
            }
            //检查是否可以补单
            int additional_id = 0;
            if (string.Equals(additional, "1"))
            {
                DataTable dt = bllOrders.GetList(1, " user_name='" + openid + "' and OrderType!='催单' and OrderType!='通知' and is_additional=0", " id desc").Tables[0];
                if (dt.Rows.Count == 0)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"您还没有提交过订单！\"}");
                    return;
                }
                if (dt.Rows[0]["additional_count"].ToString() != "0")
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"只能提交一次补单！\"}");
                    return;
                }
                if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(siteConfig.additional_force) < DateTime.Now)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"上一个订单下单后" + siteConfig.additional_force + "分钟内可以提交补单！\"}");
                    return;
                }
                additional_id = int.Parse(dt.Rows[0]["id"].ToString());
            }

            //统计购物车
            Model.cart_total cartModel = DTcms.Web.UI.ShopCart.GetTotal(userModel.group_id, takeout);
            if (cartModel.real_amount < siteConfig.lowamount && takeout == 0 && !string.Equals(additional, "1") && dismountid == 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"" + siteConfig.alarm_message_lowamount.Replace("{0}", siteConfig.lowamount.ToString()) + "\"}");
                return;
            }
            if (cartModel.real_amount < siteConfig.lowamount_2 && takeout == 0 && !string.Equals(additional, "1") && dismountid == 2)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"" + siteConfig.alarm_message_lowamount.Replace("{0}", siteConfig.lowamount_2.ToString()) + "\"}");
                return;
            }
            //检查货到付款使用限制
            if (siteConfig.DeliverPayMaxAmountForMp > 0 && !string.Equals(additional, "1")
                && cartModel.real_amount <= siteConfig.DeliverPayMaxAmountForMp && payment_id == 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"抱歉，低于" + siteConfig.DeliverPayMaxAmountForMp + "元不能使用货到付款\"}");
                return;
            }

            BookingFood.Model.bf_company modelCompany = new BookingFood.BLL.bf_company().GetModel(userModel.company_id);
            BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
            int avaliableCompanyAmount = (int)bllUserVoucher.GetModelList("UserId=" + userModel.id + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
            decimal isVip = cartModel.voucher_total;
            if (takeout == 0 && (cartModel.real_amount - cartModel.voucher_total) >= 99 && modelCompany != null && avaliableCompanyAmount >= (5 + cartModel.voucher_total) && siteConfig.enable_waimai_vip)
            {
                cartModel.real_amount -= 5;
                isVip += 5;
            }
            else if (takeout > 0 && (cartModel.real_amount - cartModel.voucher_total) >= 39 && modelCompany != null && avaliableCompanyAmount >= (2 + cartModel.voucher_total))
            {
                cartModel.real_amount -= 2;
                isVip += 2;
            }
            //判断有效期内的优惠券是否足够支付
            if (isVip > avaliableCompanyAmount)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"抱歉，当前余额不足\"}");
                return;
            }

            //打包的订单，转换区域为外卖区域
            int beforeChangeAreaId = areaModel.Id;
            if (takeout == 2)
            {
                areaModel = bllArea.GetModel(areaModel.OppositeId);
            }
            #endregion

            #region 检查活动商品是否合法
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            BookingFood.Model.bf_carnival_user carnivalUserModel = null;
            BookingFood.BLL.bf_carnival_user bllCarnivalUser = new BookingFood.BLL.bf_carnival_user();
            BookingFood.Model.bf_carnival carnivalModel =
                bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
            Model.article_goods carnivalGoodsModel = null;
            if (carnivalModel != null && iList.Where(s => s.type == "full").Sum(s => s.quantity) > 0)
            {
                if (iList.Where(s => s.type == "full").Sum(s => s.quantity) > 1)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"满送商品数量不能超过1！\"}");
                    return;
                }
                DataTable carnivalDetail = bllArticle.GetGoodsList(0, " category_id=" + carnivalModel.BusinessId, " sort_id asc,add_time desc").Tables[0];
                List<int> listSelectCarnival = iList.Where(s => s.type == "full").Select(s => s.id).ToList();
                foreach (var item in listSelectCarnival)
                {
                    if (carnivalDetail.Select(" id=" + item).Count() == 0)
                    {
                        context.Response.Write("{\"msg\":0, \"msgbox\":\"商品" + iList.First(s => s.id == item).title + "，不在满送活动中！\"}");
                        return;
                    }
                }

                carnivalUserModel = bllCarnivalUser.GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
                carnivalGoodsModel = bllArticle.GetGoodsModel(iList.First(s => s.type == "full").id);
                if (carnivalGoodsModel.change_nums > (carnivalUserModel != null ? carnivalUserModel.Num + 1 : 0))
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"您的订单次数不足！\"}");
                    return;
                }
            }
            else
            {
                if (iList.Where(s => s.type == "full").Sum(s => s.quantity) > 1)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"活动还未开始或已结束！\"}");
                    return;
                }
            }
            bool isHaveDiscountGood = false;
            BookingFood.Model.bf_carnival carnivalOffline =
                bllCarnival.GetModelList(" Type=2 And GetDate() Between BeginTime And EndTime And Id In (Select CarnivalId From bf_carnival_area Where AreaId="
                + areaid + ") Order By BeginTime Asc").FirstOrDefault();
            if (carnivalOffline != null && iList.Where(s => s.type == "discount").Sum(s => s.quantity) > 0)
            {
                if (iList.Where(s => s.type == "discount").Sum(s => s.quantity) > 1)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"加价购商品数量不能超过1！\"}");
                    return;
                }
                DataTable carnivalDetail = bllArticle.GetGoodsList(0, " category_id in (" + carnivalOffline.BusinessId
                    + ") and id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")"
                    , " sort_id asc,add_time desc").Tables[0];
                List<int> listSelectCarnival = iList.Where(s => s.type == "discount").Select(s => s.id).ToList();
                foreach (var item in listSelectCarnival)
                {
                    if (carnivalDetail.Select(" id=" + item).Count() == 0)
                    {
                        context.Response.Write("{\"msg\":0, \"msgbox\":\"商品" + iList.First(s => s.id == item).title + "，不在加价购活动中！\"}");
                        return;
                    }
                }
                //2017-04-03 加价购变成从余额中扣除, 一天可以享受多次
                //BookingFood.BLL.bf_carnival_user_log bllCarUserLog = new BookingFood.BLL.bf_carnival_user_log();
                //if (bllCarUserLog.GetRecordCount(" UserId=" + userModel.id + " And CarnivalId=" + carnivalOffline.Id + " And AreaId=" + areaid
                //    + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" 
                //    + "' And IsPayForTakeOut=1") >0)
                //{

                //    context.Response.Write("{\"msg\":0, \"msgbox\":\"一天仅能享受一次优惠！\"}");
                //    return;
                //}
                isHaveDiscountGood = true;
            }
            else
            {
                if (iList.Where(s => s.type == "discount").Sum(s => s.quantity) > 1)
                {
                    context.Response.Write("{\"msg\":0, \"msgbox\":\"活动还未开始或已结束！\"}");
                    return;
                }
            }

            #endregion

            //保存订单=======================================================================
            #region 组装订单对象
            Model.orders model = new Model.orders();
            model.order_no = Utils.GetOrderNumberForMp(); //订单号
            model.user_id = userModel.id;
            model.user_name = userModel.user_name;
            model.payment_id = payment_id;
            model.distribution_id = distribution_id;
            model.accept_name = userModel.nick_name;
            model.which_mp = state;
            model.post_code = "";
            model.mobile = userModel.qq;
            model.email = "From WeChat";
            model.address = (takeout == 0 ? address : "");
            model.message = message;
            if (string.Equals(additional, "1"))
            {
                model.message += "【补单】";
            }
            model.payable_amount = cartModel.payable_amount;
            model.real_amount = cartModel.real_amount - cartModel.voucher_total;
            model.status = 1;
            model.area_id = areaModel.Id;
            model.area_title = areaModel.Title;
            model.before_change_area_id = beforeChangeAreaId;
            model.takeout = takeout;
            model.voucher_total = isVip;
            model.area_type = dismountid;
            if (takeout == 0)
            {
                model.OrderType = "微信";
                model.telphone = telphone;
            }
            else
            {
                model.OrderType = "线下订单";
                model.telphone = inorout;
            }
            model.is_additional = string.Equals(additional, "1") ? 1 : 0;


            //如果是先款后货的话
            if (payModel.type == 1)
            {
                model.payment_fee = 0;
                model.payment_status = 1;
            }
            else
            {
                model.payment_fee = 0;
                model.payment_status = 1;
            }
            if (takeout == 0)//takeout 0:外卖,1:堂吃,2:外带
            {
                if (model.real_amount >= siteConfig.freedisamount || string.Equals(additional, "1"))
                {
                    model.payable_freight = 0; //应付运费
                    model.real_freight = 0; //实付运费
                }
                else
                {
                    model.payable_freight = siteConfig.disamount; //应付运费
                    model.real_freight = siteConfig.disamount; //实付运费
                }
            }
            else
            {
                model.payable_freight = 0; //应付运费
                model.real_freight = 0; //实付运费
            }

            //订单总金额=实付商品金额+运费+支付手续费 - 优惠券支付金额
            model.order_amount = model.real_amount + model.real_freight + model.payment_fee;
            //购物积分,可为负数
            model.point = cartModel.total_point;
            model.add_time = DateTime.Now;
            if (takeout > 0)
            {
                //BLL.siteconfig bllSite = new BLL.siteconfig();
                //siteConfig = bllSite.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                //model.MpForHere = ++siteConfig.ForHerNumber;
                model.MpForHere = model.order_no.Substring(model.order_no.Length - 3);
                //bllSite.saveConifg(siteConfig, Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                if (takeout == 2)
                {
                    model.message += "打包取餐号：" + model.MpForHere.ToString();
                    model.address += "打包取餐号：" + model.MpForHere.ToString();
                }
                else if (takeout == 1)
                {
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        model.telphone = remark;
                    }
                    model.message += "堂吃取餐号：" + model.MpForHere.ToString();
                    model.address += "堂吃取餐号：" + model.MpForHere.ToString();
                }
            }




            //商品详细列表
            List<Model.order_goods> gls = new List<Model.order_goods>();
            List<BookingFood.Model.bf_order_goods_combo_detail> cls = new List<BookingFood.Model.bf_order_goods_combo_detail>();

            if (iList != null)
            {
                foreach (Model.cart_items item in iList)
                {
                    gls.Add(new Model.order_goods
                    {
                        goods_id = item.id,
                        goods_name = item.title,
                        goods_price = item.price
                        ,
                        real_price = item.user_price,
                        quantity = item.quantity,
                        point = item.point
                        ,
                        type = item.type,
                        subgoodsid = item.subgoodsid,
                        category_title = item.category_title
                    });
                }
            }
            model.order_goods = gls;
            #endregion
            int result = bllOrders.Add(model);
            model.order_no = result.ToString() + model.order_no;
            bllOrders.UpdateField(result, "order_no='" + model.order_no + "'");
            if (result < 1)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"订单保存过程中发生错误，请重新提交！\"}");
                return;
            }

            //补单的话，更新被补的订单的additional_count
            if (string.Equals(additional, "1"))
            {
                bllOrders.UpdateField(additional_id, " additional_count=additional_count+1 , additional_id=ISNULL(additional_id,'')+'" + result + ",'");
            }
            //更新后厨推送数据   
            #region 后厨
            if (payModel.type == 2)
            {
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in gls)
                {
                    if (item.type == "one" || item.type == "full" || item.type == "discount")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = result,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = model.area_id,
                            IsDown = 0,
                            Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                            Freight = takeout == 2 ? "打包" : takeout == 1 ? "堂吃" : "外卖"
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
                                OrderId = result,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = model.area_id,
                                IsDown = 0,
                                Taste = sub.Split('‡')[3],
                                Freight = takeout == 2 ? "打包" : takeout == 1 ? "堂吃" : "外卖"
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
            }

            #endregion
            //扣除积分
            if (model.point < 0)
            {
                new BLL.point_log().Add(model.user_id, model.user_name, model.point, "积分换购，订单号：" + model.order_no);
            }
            //清空购物车
            DTcms.Web.UI.ShopCart.Clear("0");
            //获得邮件内容
            #region 发送邮件
            Model.mail_template mailModel = new BLL.mail_template().GetModel("ordermail");
            if (mailModel != null && false)
            {
                //替换模板内容
                string titletxt = mailModel.maill_title + model.order_no;
                string bodytxt = mailModel.content;
                bodytxt = bodytxt.Replace("{useremail}", model.email);
                bodytxt = bodytxt.Replace("{useraddress}", model.address);
                bodytxt = bodytxt.Replace("{usertelphone}", model.telphone);
                bodytxt = bodytxt.Replace("{orderaddtime}", model.add_time.ToString("yyyy-MM-dd HH:mm:ss"));
                bodytxt = bodytxt.Replace("{orderno}", model.order_no);
                bodytxt = bodytxt.Replace("{orderamount}", (model.real_freight != 0 ? "外送费：" + model.real_freight.ToString() : "") + "总计：" + model.order_amount.ToString());
                bodytxt = bodytxt.Replace("{ordermessage}", model.message);
                string rtn = string.Empty;
                foreach (var item in model.order_goods)
                {
                    if (item.type == "one" || item.type == "discount")
                    {
                        rtn += string.Format("<tr style=\"line-height: 14px;\">" +
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
                            , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                    }
                    else if (item.type == "combo")
                    {
                        rtn += string.Format("<tr style=\"line-height: 12px;\">" +
                                "<td  style=\"width:60px;text-align:center;\">" +
                                    "{0}" +
                                "</td>" +
                                "<td  style=\"width:160px;\">" +
                                    "{1}" +
                                "</td>" +
                                "<td  >" +
                                    "{3}" +
                                "</td>" +
                            "</tr>"
                            , item.quantity
                            , item.goods_name
                            , item.goods_price.ToString().Replace(".00", "")
                            , (item.quantity * item.goods_price).ToString().Replace(".00", ""));
                        string[] subgoods = item.subgoodsid.Split('†');
                        foreach (var sub in subgoods)
                        {
                            rtn += string.Format("<tr style=\"line-height: 12px;\">" +
                                "<td  style=\"width:60px;text-align:center;\"></td>" +
                                "<td  style=\"width:160px;\">" +
                                    "{0}" +
                                "</td>" +
                                "<td ></td>" +
                                "</tr>"
                                , sub.Split('‡')[2] + (!string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : "")
                                );
                        }
                    }
                    else if (item.type == "full")
                    {
                        rtn += string.Format("<tr style=\"line-height: 14px;\">" +
                                "<td style=\"width:60px;text-align:center;\">" +
                                    "{0}" +
                                "</td>" +
                                "<td  style=\"width:160px;\">" +
                                    "{1} {4}" +
                                "</td>" +
                                "<td >" +
                                    "{3}" +
                                "</td>" +
                            "</tr>", item.quantity, item.goods_name, item.goods_price.ToString().Replace(".00", "")
                            , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                            , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                    }
                }
                if (!string.IsNullOrEmpty(rtn))
                {
                    rtn = "<table style=\"font-sze:18px;\">" + rtn + "</table>";
                }
                bodytxt = bodytxt.Replace("{orderdetail}", rtn);

                //发送邮件
                try
                {
                    //DTMail.sendMail(siteConfig.emailstmp,
                    //    siteConfig.emailusername,
                    //    DESEncrypt.Decrypt(siteConfig.emailpassword),
                    //    siteConfig.emailnickname,
                    //    siteConfig.emailfrom,
                    //    model.email,
                    //    titletxt, bodytxt);
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
            //提交成功，返回URL
            if (payModel.type == 1)
            {
                if (payModel.id == 3)
                {
                    context.Response.Write("{\"msg\":1, \"msgbox\":\"订单已成功提交！\",\"url\":\"/api/payment/" + payModel.api_path
                    + "/index.aspx?pay_order_no=" + model.order_no + "&pay_order_amount=" + model.order_amount + "&pay_user_name=" + openid + "&pay_subject=微信订单\"}");
                }
                else if (payModel.id == 5)
                {
                    if (siteConfig.RunTigoon == 0)
                    {
                        #region 生成微信支付链接
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
                        packageReqHandler.SetParameter("body", siteConfig.webname + "微信订单");
                        packageReqHandler.SetParameter("out_trade_no", model.order_no);     //商家订单号
                        packageReqHandler.SetParameter("total_fee", (model.order_amount * 100).ToString().Replace(".00", ""));                  //商品金额,以分为单位(money * 100).ToString()
                        packageReqHandler.SetParameter("spbill_create_ip", context.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                        packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);          //接收财付通通知的URL
                        packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());                        //交易类型
                        packageReqHandler.SetParameter("openid", openid);                       //用户的openId
                                                                                                //packageReqHandler.SetParameter("openid", "11");	                    //用户的openId

                        string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                        packageReqHandler.SetParameter("sign", sign);                       //签名

                        string data = packageReqHandler.ParseXML();

                        var mppay_result = TenPayV3.Unifiedorder(data);
                        var res = XDocument.Parse(mppay_result);
                        string prepayId = string.Empty;
                        try
                        {
                            prepayId = res.Element("xml").Element("prepay_id").Value;
                        }
                        catch (Exception)
                        {
                            Log.Info(res.ToString());
                        }


                        //设置支付参数
                        RequestHandler paySignReqHandler = new RequestHandler(null);
                        paySignReqHandler.SetParameter("appId", siteConfig.mp_slave_appid);
                        paySignReqHandler.SetParameter("timeStamp", timeStamp);
                        paySignReqHandler.SetParameter("nonceStr", nonceStr);
                        paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                        paySignReqHandler.SetParameter("signType", "MD5");
                        string paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                        string package = string.Format("prepay_id={0}", prepayId);
                        context.Response.Write("{\"msg\":1,\"mppay\":\"1\", \"timestamp\":\"" + timeStamp + "\",\"noncestr\":\"" + nonceStr
                            + "\",\"paysign\":\"" + paySign + "\",\"package\":\"" + package + "\",\"forhere\":\"" + model.MpForHere + "\"}");
                        #endregion
                    }
                    else
                    {
                        #region 基于天宫生成支付链接
                        ChargeRequest<string> get_req = new ChargeRequest<string>();
                        get_req.amount = model.order_amount;
                        get_req.out_order_no = model.order_no;
                        get_req.pay_channel = "wxpay_jsapi";
#if DEBUG
                        get_req.ip = "119.180.116.79";
#else
                                            get_req.ip = context.Request.UserHostAddress;
#endif
                        get_req.subject = siteConfig.webname + "微信订单";
                        get_req.return_url = "http://www.4008317417.cn/payment.aspx";
                        get_req.notify_url = "http://www.4008317417.cn/api/payment/teegon_jsapi/feedback.aspx";
                        get_req.metadata = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        get_req.device_id = System.Net.Dns.GetHostName();
                        get_req.charge_type = "pay";
                        get_req.account_id = "main";
                        get_req.wx_openid = openid;
                        get_req.metadata = "{\"mpforhere\":\"" + model.MpForHere + "\",\"takeout\":" + takeout + "}";
                        ChargeResponse<string> get_rsp = Client.Execute(get_req);

                        context.Response.Write("{\"msg\":1,\"mppay\":\"2\", \"pay_location\":\""
                            + get_rsp.Result.Action.Params.Replace("window.location=\"", "").TrimEnd('"') + "\",\"forhere\":\"" + model.MpForHere + "\"}");
                        #endregion
                    }

                }

            }
            #region 处理满送业务
            if (takeout == 0 && payModel.id == 1)//外卖订单，增加满送记录
            {
                //在满送活动期间内数量+1
                if (carnivalModel != null)
                {
                    bool isUpdate = false;
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
                            OrderId = result,
                            UserId = userModel.id,
                            AreaId = areaModel.Id
                        });
                        if (carnivalUserModel == null)
                        {
                            carnivalUserModel = bllCarnivalUser.GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
                            if (carnivalUserModel == null)
                            {
                                carnivalUserModel = new BookingFood.Model.bf_carnival_user()
                                {
                                    CarnivalId = carnivalModel.Id,
                                    Num = 1,
                                    Openid = userModel.user_name,
                                    UserId = userModel.id,
                                    AreaId = areaModel.Id
                                };
                                bllCarnivalUser.Add(carnivalUserModel);
                            }
                            else
                            {
                                carnivalUserModel.Num += 1;
                                isUpdate = true;
                            }
                        }
                        else
                        {
                            carnivalUserModel.Num += 1;
                            isUpdate = true;
                        }
                    }
                    if (carnivalGoodsModel != null)
                    {
                        //更新用户的兑换次数
                        carnivalUserModel.Num -= carnivalGoodsModel.change_nums;
                        isUpdate = true;
                    }
                    if (isUpdate) bllCarnivalUser.Update(carnivalUserModel);
                }
                if (payModel.type == 2)
                {
                    context.Response.Write("{\"msg\":1, \"msgbox\":\"订单已成功提交！\",\"carnivalnums\":\"" + (carnivalUserModel != null ? carnivalUserModel.Num.ToString() : "") + "\"}");
                }
            }
            else if (takeout != 0 && payModel.id == 1) //堂吃/外带订单，增加使用记录，一天只能享受一次加价购
            {
                //if(carnivalOffline!=null && isHaveDiscountGood)
                //{
                //    BookingFood.BLL.bf_carnival_user_log bllCarnivalUserLog = new BookingFood.BLL.bf_carnival_user_log();
                //    bllCarnivalUserLog.Add(new BookingFood.Model.bf_carnival_user_log()
                //    {
                //        AddTime = DateTime.Now,
                //        CarnivalId = carnivalOffline.Id,
                //        OpenId = userModel.user_name,
                //        OrderId = result,
                //        UserId = userModel.id,
                //        AreaId = int.Parse(areaid),
                //        IsPayForTakeOut=1
                //    });
                //}
            }
            #endregion
            #region 发送微信模板消息
            if (model.payment_id == 1)
            {
                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.add_time.ToString("yyyy-MM-dd HH:mm")));
                tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_amount.ToString("0.00")));
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
                tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.address));
                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_remark));
                string accessToken = string.Empty;
                switch (state)
                {
                    case "master":
                        accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                        try
                        {
                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "REmtsGZMK7-3NXNJf3NZMOfmH9dKwkvwvCBww5F9VYQ"
                            , "#173177", "", tempData);
                        }
                        catch (Exception ex)
                        {
                            Log.Info(ex.Message);
                        }
                        break;
                    case "slave":
                        accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                        try
                        {
                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "gYklU4AeAT7KCehbfRP5emhBsSNkhMVDVtdIBFlhn8Y"
                            , "#173177", "", tempData);
                        }
                        catch (Exception ex)
                        {
                            Log.Info(ex.Message);
                        }
                        break;
                }

            }
            #endregion

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

        private void add_less(HttpContext context)
        {
            string source = DTRequest.GetFormString("source");
            string openid = DTRequest.GetFormString("openid");

            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_name='" + openid + "' and add_time between '" + time + " 00:00:00' and '" + time + " 23:59:59'"
                + " and OrderType!='催单' and OrderType!='通知' and is_additional=0 "
                , " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"您今天没有下单，无法补充送错信息！\"}");
                return;
            }
            if (dt.Rows[0]["is_less"].ToString() == "2")
            {
                context.Response.Write("{\"msg\":0, \"msgbox\":\"已经补充过信息，无法再次补充！\"}");
                return;
            }
            BLL.orders bllOrder = new BLL.orders();
            Model.orders orderModel = bllOrder.GetModel(int.Parse(dt.Rows[0]["id"].ToString()));
            orderModel.OrderType = "通知";
            orderModel.message = "[" + source + "]";
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
            bllOrder.UpdateField(int.Parse(dt.Rows[0]["id"].ToString()), " is_less=is_less+1");
            context.Response.Write("{\"msg\":1, \"msgbox\":\"我们将极速处理！\"}");

            Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("您的反馈已直达店铺，并立即处理，也请保持电话畅通！"));
            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("一级"));
            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(orderModel.area_title));
            tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
            tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(source));
            tempData.Add("keyword5", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("正跟进中处理中！"));
            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(""));
            string accessToken = string.Empty;
            switch (orderModel.which_mp)
            {
                case "master":
                    accessToken = Senparc.Weixin.MP.CommonAPIs
                        .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                    try
                    {
                        Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, orderModel.user_name, "0oNzZem6I7-dMxcdFkP4A9JJh74bP-uRy8wtNieMadE"
                        , "#173177", "", tempData);
                    }
                    catch (Exception) { }
                    break;
                case "slave":
                    accessToken = Senparc.Weixin.MP.CommonAPIs
                        .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                    try
                    {
                        Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, orderModel.user_name, "Wfr_wFIqZlZvIqBrtzbDxdo4DZ1DYq16uGl9sA1GR7g"
                        , "#173177", "", tempData);
                    }
                    catch (Exception) { }
                    break;
            }

        }

        private void GetCompanyList(HttpContext context)
        {
            string lat = context.Request.Params["lat"];
            string lng = context.Request.Params["lng"];
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
            DataSet ds = bll.GetListByPositionByTransform(999, lng, lat, " And IsShow=0 AND ParentId=1  ");
            string rtn = string.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                rtn += string.Format("<li class=\"item \" data-id=\"{0}\" data-content=\"{2}\""
                    + " data-title=\"{1}\">{1}{4}"
                    + "<a style=\"float:right; margin-top:-5px; \" href=\"http://apis.map.qq.com/uri/v1/marker?marker=coord:{5},{6};title:{1};addr:{2}\"><img src=\"/templates/green/images/20150731114208930_easyicon_net_32.png\"/></a>"
                        + "<span>{2}</span></li>"
                    , item["Id"].ToString()
                    , item["Title"].ToString()
                    , item["Description"].ToString()
                    , (decimal.Parse(item["calcdistance"].ToString()) / 1000).ToString("0.00") + "KM"
                    , item["Address"].ToString()
                    , item["Lat"].ToString()
                    , item["Lng"].ToString()
                    );
            }

            context.Response.Write(rtn);
        }

        private void GetPolygonContain(HttpContext context)
        {
            string position = DTRequest.GetFormString("position");
            string openid = DTRequest.GetFormString("openid");
            Log.Info("外卖ip:" + Common.DTRequest.GetIP() + " " + position);
#if DEBUG
            position = "31.19206539,121.43165358";
            position = "31.183325,121.425657";//北科
#endif
            string prevAreaId = string.Empty;
            if (!string.IsNullOrEmpty(openid))
            {
                BLL.orders bll = new BLL.orders();
                DataTable dt = bll.GetList(1, " user_name='" + openid + "' And takeout=0", " id desc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    prevAreaId = dt.Rows[0]["area_id"].ToString();
                }
            }
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            DataTable dtArea = bllArea.
                    GetList(" IsShow=1 AND ParentId=1 Order By SortId Asc").Tables[0];

            string rtn = "{\"Status\":0}";
            foreach (DataRow item in dtArea.Rows)
            {
                if (string.IsNullOrEmpty(item["DistributionArea"].ToString())) continue;
                bool isInArea = Polygon.GetResult(position, item["DistributionArea"].ToString());
                if (isInArea)
                {
                    if (string.IsNullOrEmpty(prevAreaId) || prevAreaId != item["Id"].ToString())
                    {
                        rtn = "{\"Status\":1,\"Type\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":1,\"Address\":\"" + item["Address"].ToString() + "\"}";
                    }
                    else
                    {
                        rtn = "{\"Status\":1,\"Type\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":0,\"Address\":\"" + item["Address"].ToString() + "\"}";
                    }
                    break;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["DistributionArea_2"].ToString()))
                    {
                        isInArea = Polygon.GetResult(position, item["DistributionArea_2"].ToString());
                        if (isInArea)
                        {
                            if (string.IsNullOrEmpty(prevAreaId) || prevAreaId != item["Id"].ToString())
                            {
                                rtn = "{\"Status\":1,\"Type\":2,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":1,\"Address\":\"" + item["Address"].ToString() + "\"}";
                            }
                            else
                            {
                                rtn = "{\"Status\":1,\"Type\":2,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":0,\"Address\":\"" + item["Address"].ToString() + "\"}";
                            }

                            break;
                        }
                    }

                }
            }
            context.Response.Write(rtn);
        }

        private void GetPolygonContainForTake(HttpContext context)
        {
            string position = DTRequest.GetFormString("position");
            string openid = DTRequest.GetFormString("openid");
#if DEBUG
            //position = "31.19206539,121.43165358";
            //position = "31.183325,121.425657";//北科
#endif
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            string prevAreaId = string.Empty;
            if (!string.IsNullOrEmpty(openid))
            {
                BLL.orders bll = new BLL.orders();
                DataTable dt = bll.GetList(1, " user_name='" + openid + "' And takeout in (1,2)", " id desc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["takeout"].ToString() == "2")
                    {
                        BookingFood.Model.bf_area areaModel = bllArea.GetModelList("OppositeId=" + dt.Rows[0]["area_id"].ToString())[0];
                        prevAreaId = areaModel.Id.ToString();
                    }
                    else
                    {
                        prevAreaId = dt.Rows[0]["area_id"].ToString();
                    }

                }
            }
            //Log.Info("ip:" + Common.DTRequest.GetIP() + " " + position + " " + openid);

            DataTable dtArea = bllArea.
                    GetList(" IsShow=0 AND IsLock=0 AND ParentId=1 Order By SortId Asc").Tables[0];
            string rtn = "{\"Status\":0}";
            foreach (DataRow item in dtArea.Rows)
            {
                if (string.IsNullOrEmpty(item["DistributionArea"].ToString())) continue;
                bool isInArea = Polygon.GetResult(position, item["DistributionArea"].ToString());
                if (isInArea)
                {
                    if (string.IsNullOrEmpty(prevAreaId) || prevAreaId != item["Id"].ToString())
                    {
                        rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString()
                            + "\",\"ShowConfirm\":1,\"Address\":\"" + item["Address"].ToString() + "\"}";
                    }
                    else
                    {
                        rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":0}";
                    }

                    break;
                }
            }
            context.Response.Write(rtn);
        }
        //
        private void GetCarnivalOffline(HttpContext context)
        {
            string areaid = DTRequest.GetFormString("areaid");
            string openid = DTRequest.GetFormString("openid");
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            BookingFood.Model.bf_carnival carnivalOffline = bllCarnival.GetModelList(
                " Type=2 And GetDate() Between BeginTime And EndTime And Id In (Select CarnivalId From bf_carnival_area Where AreaId="
                + areaid + ") Order By BeginTime Asc").FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            DataTable carnivalOfflineDetail = null;
            BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
            Model.users userModel = new BLL.users().GetModel(openid);
            int avaliableCompanyAmount = (int)bllUserVoucher.GetModelList("UserId=" + userModel.id + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
            //20170526 因线上活动没有显示出商品,暂时去掉5元优惠券的条件  && avaliableCompanyAmount>=5
            //20170730 重新加上这个条件,不满足5元时不显示,降低用户学习成本
            if (carnivalOffline != null && avaliableCompanyAmount >= 5 && userModel.company_id != 0)
            {
                BookingFood.Model.bf_area modelArea = new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
                BLL.users bllUser = new BLL.users();
                Model.users modelUser = bllUser.GetModel(openid);
                bool isShowCompanyArticle = false;
                if (modelUser.company_id > 0)
                {
                    BookingFood.Model.bf_company modelCompany = new BookingFood.BLL.bf_company().GetModel(modelUser.company_id);
                    if (modelCompany != null && modelCompany.BeginTime != null && ((DateTime)modelCompany.BeginTime) < DateTime.Now
                        && modelCompany.EndTime != null && ((DateTime)modelCompany.EndTime) > DateTime.Now)
                    {
                        isShowCompanyArticle = true;
                    }
                }
                //BookingFood.BLL.bf_carnival_user_log bllCarUserLog = new BookingFood.BLL.bf_carnival_user_log();
                //if (bllCarUserLog.GetRecordCount(" OpenId='" + openid + "' And CarnivalId=" + carnivalOffline.Id + " And AreaId=" + areaid
                //    + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" 
                //    + "' And IsPayForTakeOut=1") > 0)
                //{
                //    context.Response.Write("<span>今天优惠已使用</span>");
                //    return;
                //}

                sb.Append("<ul style=\"overflow: hidden; margin: 10px auto 40px auto;\" id=\"carnivalOfflineMenu\">");
                carnivalOfflineDetail = get_goods_list(3, carnivalOffline.BusinessId, 0, "id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")");
                foreach (DataRow item_s in carnivalOfflineDetail.Rows)
                {
                    if (item_s["only_company"].ToString() == "0")
                    {
                        sb.Append("<li>");
                        sb.Append("<div class=\"item one selected\" data-headlinetitle=\"" + item_s["goods_no"].ToString() + "\" data-headline=\"" + item_s["head_line"].ToString()
                            + "\" data-headlineimg=\"" + item_s["headline_img_url"].ToString() + "\" data-id=\"" + item_s["id"].ToString()
                            + "\" data-price=\"" + item_s["sell_price"].ToString() + "\" data-title=\""
                            + item_s["title"].ToString() + "\" data- style=\"background-image:url(" + item_s["mp_img_url"].ToString() + ");\">");
                        sb.Append("<div class=\"unselect\"></div>");
                        sb.Append("<span style=\"background: rgb(243,152,0); position:absolute;right:0px;width:20px;height:20px;border-radius:50%;color:white;font-size:14px;\" >"
                            + item_s["sell_price"].ToString().Replace(".00", "") + " </span>");
                        sb.Append("<div class=\"name\">");
                        sb.Append(item_s["title"].ToString());
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                    else if (item_s["only_company"].ToString() == "1" && isShowCompanyArticle)
                    {
                        sb.Append("<li>");
                        sb.Append("<div class=\"item one selected\" data-onlycompany=\"" + item_s["only_company"].ToString() + "\" data-headlinetitle=\"" + item_s["goods_no"].ToString()
                            + "\" data-headline=\"" + item_s["head_line"].ToString() + "\" data-headlineimg=\"" + item_s["headline_img_url"].ToString()
                            + "\" data-id=\"" + item_s["id"].ToString() + "\" data-price=\"" + item_s["sell_price"].ToString() + "\" data-title=\""
                            + item_s["title"].ToString() + "\" data- style=\"background-image:url(" + item_s["mp_img_url"].ToString() + ");\">");
                        sb.Append("<div class=\"unselect\"></div>");
                        sb.Append("<span style=\"background: rgb(243,152,0); position:absolute;right:0px;width:20px;height:20px;border-radius:50%;color:white;font-size:14px;\" >"
                            + item_s["sell_price"].ToString().Replace(".00", "") + " </span>");
                        sb.Append("<div class=\"name\">");
                        sb.Append(item_s["title"].ToString());
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
            }
            context.Response.Write(sb.ToString());
        }

        private void GetCarnivalLine(HttpContext context)
        {
            string areaid = DTRequest.GetFormString("areaid");
            string openid = DTRequest.GetFormString("openid");
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            BookingFood.Model.bf_carnival carnivalOffline = bllCarnival.GetModelList(
                " Type=1 And GetDate() Between BeginTime And EndTime And Id In (Select CarnivalId From bf_carnival_area Where AreaId="
                + areaid + ") Order By BeginTime Asc").FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            DataTable carnivalOfflineDetail = null;
            Model.users userModel = new BLL.users().GetModel(openid);        
            if (carnivalOffline != null && userModel.company_id != 0)
            {
                BookingFood.Model.bf_area modelArea = new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
                BLL.users bllUser = new BLL.users();
                Model.users modelUser = bllUser.GetModel(openid);
                bool isShowCompanyArticle = false;
                if (modelUser.company_id > 0)
                {
                    BookingFood.Model.bf_company modelCompany = new BookingFood.BLL.bf_company().GetModel(modelUser.company_id);
                    if (modelCompany != null && modelCompany.BeginTime != null && ((DateTime)modelCompany.BeginTime) < DateTime.Now
                        && modelCompany.EndTime != null && ((DateTime)modelCompany.EndTime) > DateTime.Now)
                    {
                        isShowCompanyArticle = true;
                    }
                }
                sb.Append("<ul style=\"overflow: hidden; margin: 10px auto 40px auto;\" id=\"carnivalOfflineMenu\">");
                carnivalOfflineDetail = get_goods_list(3, carnivalOffline.BusinessId, 0, "id in (SELECT baa.ArticleId FROM bf_area_article baa WHERE baa.[Type]='one' AND baa.AreaId=" + areaid + ")");
                foreach (DataRow item_s in carnivalOfflineDetail.Rows)
                {
                    if (item_s["only_company"].ToString() == "0")
                    {
                        sb.Append("<li>");
                        sb.Append("<div class=\"item one selected\" data-headlinetitle=\"" + item_s["goods_no"].ToString() + "\" data-headline=\"" + item_s["head_line"].ToString()
                            + "\" data-headlineimg=\"" + item_s["headline_img_url"].ToString() + "\" data-id=\"" + item_s["id"].ToString()
                            + "\" data-price=\"" + item_s["sell_price"].ToString() + "\" data-title=\""
                            + item_s["title"].ToString() + "\" data- style=\"background-image:url(" + item_s["mp_img_url"].ToString() + ");\">");
                        sb.Append("<div class=\"unselect\"></div>");
                        sb.Append("<span style=\"background: rgb(243,152,0); position:absolute;right:0px;width:20px;height:20px;border-radius:50%;color:white;font-size:14px;\" >"
                            + item_s["sell_price"].ToString().Replace(".00", "") + " </span>");
                        sb.Append("<div class=\"name\">");
                        sb.Append(item_s["title"].ToString());
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                    else if (item_s["only_company"].ToString() == "1" && isShowCompanyArticle)
                    {
                        sb.Append("<li>");
                        sb.Append("<div class=\"item one selected\" data-onlycompany=\"" + item_s["only_company"].ToString() + "\" data-headlinetitle=\"" + item_s["goods_no"].ToString()
                            + "\" data-headline=\"" + item_s["head_line"].ToString() + "\" data-headlineimg=\"" + item_s["headline_img_url"].ToString()
                            + "\" data-id=\"" + item_s["id"].ToString() + "\" data-price=\"" + item_s["sell_price"].ToString() + "\" data-title=\""
                            + item_s["title"].ToString() + "\" data- style=\"background-image:url(" + item_s["mp_img_url"].ToString() + ");\">");
                        sb.Append("<div class=\"unselect\"></div>");
                        sb.Append("<span style=\"background: rgb(243,152,0); position:absolute;right:0px;width:20px;height:20px;border-radius:50%;color:white;font-size:14px;\" >"
                            + item_s["sell_price"].ToString().Replace(".00", "") + " </span>");
                        sb.Append("<div class=\"name\">");
                        sb.Append(item_s["title"].ToString());
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
            }
            context.Response.Write(sb.ToString());
        }
        private DataTable get_goods_list(int channel_id, int category_id, int top, string strwhere)
        {
            DataTable dt = new DataTable();
            if (channel_id > 0)
            {
                string _where = "channel_id=" + channel_id;
                if (category_id > 0)
                {
                    _where += " and category_id in(select id from dt_category where channel_id=" + channel_id + " and class_list like '%," + category_id + ",%')";
                }
                if (!string.IsNullOrEmpty(strwhere))
                {
                    _where += " and " + strwhere;
                }
                dt = new DTcms.BLL.article().GetGoodsList(top, _where, "sort_id asc,add_time desc").Tables[0];
            }
            return dt;
        }

        private void JoinCompany(HttpContext context)
        {
            string txtCompanyName = DTRequest.GetFormString("txtCompanyName");
            string txtCompanyAddress = DTRequest.GetFormString("txtCompanyAddress");
            string txtAcceptName = DTRequest.GetFormString("txtAcceptName");
            string rtn = string.Empty;
            if (string.IsNullOrEmpty(txtCompanyName) || string.IsNullOrEmpty(txtCompanyAddress))
            {
                rtn = "{\"status\":0,\"msg\":\"请输入公司和地址\"}";
                context.Response.Write(rtn);
                return;
            }
            string openid = DTRequest.GetFormString("openid");
            //int areaid = DTRequest.GetFormInt("areaid");
            BLL.users bllUsers = new BLL.users();
            Model.users modelUser = bllUsers.GetModel(openid);
            BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
            if (bllCompany.GetRecordCount("RequestUserId=" + modelUser.id) > 0)
            {
                rtn = "{\"status\":0,\"msg\":\"已注册过群组\"}";
                context.Response.Write(rtn);
                return;
            }
            BookingFood.Model.bf_company model = new BookingFood.Model.bf_company();
            model.AcceptName = txtAcceptName;
            model.Address = txtCompanyAddress;
            model.AddTime = DateTime.Now;
            model.CompanyName = txtCompanyName;
            model.PersonCount = 0;
            model.Status = 1;
            model.Telphone = string.Empty;
            model.RequestUserId = modelUser.id;
            model.AreaId = 0;
            model.AreaName = "";
            model.PersonCount = 1;

            model.Id = bllCompany.Add(model);

            if (model.Id > 0)
            {
                modelUser.company_id = model.Id;
                bllUsers.Update(modelUser);
                GenerateCompanyWelcome(model.Id);
                rtn = "{\"status\":1}";
                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.CompanyName + "申请加入群组"));//"您申请的贵司VIP卡已通过,其他同事扫码加入即获88元现金券"
                tempData.Add("cardNumber", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.Id.ToString()));
                tempData.Add("type", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.CompanyName));//"商户"
                tempData.Add("address", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.Address));//
                tempData.Add("VIPName", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.AcceptName));
                tempData.Add("VIPPhone", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.Telphone));
                tempData.Add("expDate", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.AddMonths(2).ToString("yyyy年MM月dd日")));
                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("申请加入群组")); //"满50位同事后，更有1分钱吃馍专享活动！"
                string accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                try
                {
                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, "omc8At72JW3L8jxIb7C9g3gRUUGc", "XeehJQ-7hsp_JxTnXRPO1zKRtWf1raMi-5jScgbXfLU"
                    , "#173177", "", tempData);
                }
                catch (Exception) { }


                if (siteConfig.enable_register_company_send_voucher)
                {
                    BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
                    bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                    {
                        AddTime = DateTime.Now,
                        Amount = 10,
                        CompanyId = model.Id,
                        ExpireTime = DateTime.Now.AddMonths(1),
                        UserId = model.RequestUserId
                    });
                }
                BookingFood.BLL.bf_company_user_log bllCompanyUser = new BookingFood.BLL.bf_company_user_log();
                bllCompanyUser.Add(new BookingFood.Model.bf_company_user_log()
                {
                    AddTime = DateTime.Now,
                    CompanyId = model.Id,
                    UserId = modelUser.id
                });

                //发送申请通过消息
                tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_approve_title));//"您申请的贵司VIP卡已通过,其他同事扫码加入即获88元现金券"
                tempData.Add("cardNumber", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.id.ToString()));
                tempData.Add("type", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_name));//"商户"
                tempData.Add("address", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_companyname));//"中山西路1919号馍王"
                tempData.Add("VIPName", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.nick_name));
                tempData.Add("VIPPhone", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.telphone));
                tempData.Add("expDate", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.AddMonths(2).ToString("yyyy年MM月dd日")));
                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_footer)); //"满50位同事后，更有1分钱吃馍专享活动！"
                accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                try
                {
                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelUser.user_name, "XeehJQ-7hsp_JxTnXRPO1zKRtWf1raMi-5jScgbXfLU"
                    , "#173177", "", tempData);
                }
                catch (Exception) { }
            }
            else
            {
                rtn = "{\"status\":0}";
            }
            context.Response.Write(rtn);
        }

        private void GenerateCompanyWelcome(int id)
        {
            string accessToken = Senparc.Weixin.MP.CommonAPIs
                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
            CreateQrCodeResult qrResut = QrCode.Create(accessToken, 0, id);
            string qr_url = QrCode.GetShowQrCodeUrl(qrResut.ticket);
            qr_url = Utils.DownloadImg(qr_url, "/company_qr/", "qr_" + id.ToString() + ".jpg");
            DTcms.Common.WaterMark.AddImageToPic("/templates/green/images/qr_company.jpg", "/company_qr/" + id.ToString() + ".jpg"
                , qr_url, 2, 100, 10);
        }

        #endregion
    }
}