using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DTcms.Common;
using LitJson;

namespace DTcms.Web.UI
{
    /// <summary>
    /// 购物车帮助类
    /// </summary>
    public partial class ShopCart
    {
        #region 基本增删改方法====================================
        /// <summary>
        /// 获得购物车列表
        /// </summary>
        public static IList<Model.cart_items> GetList(int group_id, int takeout=0)
        {
            //IDictionary<string, CartDetail> dic = GetCart();
            IDictionary<string, CartDetail> dic = GetCartByPost();
            if (dic != null)
            {
                IList<Model.cart_items> iList = new List<Model.cart_items>();
                BLL.category bllCategory = new BLL.category ();
                foreach (var item in dic)
                {
                    if (item.Key.Split('†')[1] == "one" || item.Key.Split('†')[1] == "discount")
                    {
                        BLL.article bll = new BLL.article();
                        Model.article_goods model = bll.GetGoodsModel(Convert.ToInt32(item.Key.Split('†')[0]));
                        if (model == null)
                        {
                            continue;
                        }
                        Model.cart_items modelt = new Model.cart_items();
                        modelt.id = model.id;
                        modelt.title = model.title + (takeout == 2 ? "*打包" : "");
                        modelt.img_url = model.img_url;
                        modelt.point = model.point;
                        if (item.Key.Split('†')[1] == "discount")
                        {
                            if (takeout == 1 || takeout==2)
                            {
                                modelt.voucher_price = model.sell_price;
                            }
                        }
                        modelt.price = model.sell_price;
                        modelt.user_price = model.sell_price;
                        modelt.stock_quantity = model.stock_quantity;
                        modelt.category_title = bllCategory.GetModel(model.category_id).title;
                        
                        //会员价格
                        //if (model.goods_group_prices != null)
                        //{
                        //    Model.goods_group_price gmodel = model.goods_group_prices.Find(p => p.group_id == group_id);
                        //    if (gmodel != null)
                        //    {
                        //        modelt.user_price = gmodel.price;
                        //    }
                        //}
                        modelt.quantity = item.Value.quantity;
                        modelt.low_num = model.low_num;
                        modelt.subgoodsid = item.Value.subgoodsid ;
                        modelt.type = item.Key.Split('†')[1];
                        iList.Add(modelt);
                    }
                    else if (item.Key.Split('†')[1] == "combo")
                    {
                        BookingFood.BLL.bf_good_combo bll = new BookingFood.BLL.bf_good_combo();
                        BookingFood.Model.bf_good_combo model = bll.GetModel(Convert.ToInt32(item.Key.Split('†')[0]));
                        if (model == null)
                        {
                            continue;
                        }
                        Model.cart_items modelt = new Model.cart_items();
                        modelt.id = model.Id;
                        modelt.title = model.Title;
                        modelt.img_url = model.Photo;
                        modelt.point = 0;
                        modelt.category_title = bllCategory.GetModel(model.CategoryId).title;
                        string[] subgoods = item.Value.subgoodsid.Split('†');
                        BLL.article bllArticle = new BLL.article();
                        Model.article_goods article = null;
                        foreach (var sub in subgoods)
                        {
                            if (!string.IsNullOrEmpty(modelt.subgoodsid))
                            {
                                modelt.subgoodsid += "†";
                            }
                            
                            if (sub.Split('‡')[0] == "taste")
                            {
                                modelt.subgoodsid += sub ;
                                modelt.taste = sub.Split('‡')[1];                               
                            }
                            else
                            {
                                modelt.subgoodsid += (sub + (takeout == 2 ? "*打包" : ""));
                                article= bllArticle.GetGoodsModel(Convert.ToInt32(sub.Split('‡')[1]));
                                modelt.price += article.sell_price;                                
                            }
                        }
                        modelt.user_price = modelt.price;
                        modelt.stock_quantity = 9999;
                        //会员价格
                        modelt.user_price = modelt.price;
                        modelt.quantity = item.Value.quantity;
                        modelt.type = "combo";
                        //modelt.subgoodsid = item.Value.subgoodsid;
                        iList.Add(modelt);
                    }
                    else if (item.Key.Split('†')[1] == "full")
                    {
                        BLL.article bll = new BLL.article();
                        Model.article_goods model = bll.GetGoodsModel(Convert.ToInt32(item.Key.Split('†')[0]));
                        if (model == null)
                        {
                            continue;
                        }
                        Model.cart_items modelt = new Model.cart_items();
                        modelt.id = model.id;
                        modelt.title = string.Format("满{0}次送{1}",model.change_nums, model.title);
                        modelt.img_url = model.img_url;
                        modelt.point = model.point;
                        modelt.price = model.sell_price;
                        modelt.user_price = model.sell_price;
                        modelt.stock_quantity = model.stock_quantity;
                        modelt.category_title = bllCategory.GetModel(model.category_id).title;                        
                        modelt.quantity = item.Value.quantity;
                        modelt.subgoodsid = item.Value.subgoodsid ;
                        modelt.type = "full";
                        iList.Add(modelt);
                    }                    
                }
                return iList;
            }
            return null;
        }
        

        /// <summary>
        /// 添加到购物车
        /// </summary>
        public static bool Add(string Key, int Quantity, string subgoodsid)
        {
            IDictionary<string, CartDetail> dic = GetCart();
            if (dic != null)
            {
                if (dic.ContainsKey(Key))
                {
                    dic[Key].quantity += Quantity;
                    AddCookies(JsonMapper.ToJson(dic));
                    return true;
                }
            }
            else
            {
                dic = new Dictionary<string, CartDetail>();
            }
            //不存在的则新增
            dic.Add(Key, new CartDetail() {quantity=Quantity,subgoodsid=subgoodsid });
            AddCookies(JsonMapper.ToJson(dic));
            return true;
        }

        /// <summary>
        /// 更新购物车数量
        /// </summary>
        public static bool Update(string Key, int Quantity)
        {
            if (Quantity == 0)
            {
                Clear(Key);
                return true;
            }
            IDictionary<string, CartDetail> dic = GetCart();
            if (dic != null && dic.ContainsKey(Key))
            {
                dic[Key].quantity = Quantity;
                AddCookies(JsonMapper.ToJson(dic));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除购物车
        /// </summary>
        /// <param name="Key">主键 0为清理所有的购物车信息</param>
        public static void Clear(string Key)
        {
            if (Key == "0")//为0的时候清理全部购物车cookies
            {
                Utils.WriteCookie(DTKeys.COOKIE_SHOPPING_CART, "", -43200);
            }
            else
            {
                IDictionary<string, CartDetail> dic = GetCart();
                if (dic != null)
                {
                    dic.Remove(Key);
                    AddCookies(JsonMapper.ToJson(dic));
                }
            }
        }
        #endregion

        #region 扩展方法==========================================
        public static Model.cart_total GetTotal(int group_id, int takeout=0)
        {
            Model.cart_total model = new Model.cart_total();
            IList<Model.cart_items> iList = GetList(group_id, takeout);
            if (iList != null)
            {
                foreach (Model.cart_items modelt in iList)
                {
                    model.total_num++;
                    model.total_quantity += modelt.quantity;
                    model.payable_amount += modelt.price * modelt.quantity;
                    model.real_amount += modelt.user_price * modelt.quantity;
                    model.total_point += modelt.point * modelt.quantity;
                    model.voucher_total += modelt.voucher_price * modelt.quantity;
                }
            }
            return model;
        }
        #endregion

        #region 私有方法==========================================
        /// <summary>
        /// 获取cookies值
        /// </summary>
        private static IDictionary<string, CartDetail> GetCart()
        {
            IDictionary<string, CartDetail> dic = new Dictionary<string, CartDetail>();
            if (!string.IsNullOrEmpty(GetCookies()))
            {
                return JsonMapper.ToObject<Dictionary<string, CartDetail>>(GetCookies());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加对象到cookies
        /// </summary>
        /// <param name="strValue"></param>
        private static void AddCookies(string strValue)
        {
            Utils.WriteCookie(DTKeys.COOKIE_SHOPPING_CART, strValue, 43200); //存储一个月
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <returns></returns>
        private static string GetCookies()
        {
            return Utils.GetCookie(DTKeys.COOKIE_SHOPPING_CART);
        }

        private static IDictionary<string, CartDetail> GetCartByPost()
        {
            IDictionary<string, CartDetail> dic = new Dictionary<string, CartDetail>();
            if (!string.IsNullOrEmpty(DTRequest.GetFormString("order")))
            {
                return JsonMapper.ToObject<Dictionary<string, CartDetail>>(DTRequest.GetFormString("order"));
            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    public class CartDetail
    {
        public string title { get; set; }
        public int quantity { get; set; }
        public string subgoodsid { get; set; }
        public decimal price { get; set; }
        public int low { get; set; }
    }
}
