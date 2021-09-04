using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DoAn.Controllers.NguoiDung
{
    public class OnepayProperty
    {
        public const string HASH_CODE = "6D0870CDE5F24F34F3915FB0045120DB";
        //"6D0870CDE5F24F34F3915FB0045120DB";

        public const string ACCESS_CODE = "6BEB2546";
        //"6BEB2546";

        public const string MERCHANT_ID = "TESTONEPAY";
        //"TESTONEPAY";

        public const string URL_ONEPAY_TEST = "https://mtf.onepay.vn/vpcpay/vpcpay.op";

        public const string URL_ONEPAY_TRUTH = "https://onepay.vn/vpcpay/vpcpay.op";

        public const string COMMAND = "pay";

        public const string PAYGATE_LANGUAGE = "en";

        public const string VERSION = "2";

        public const string AGAIN_LINK = "onepay.vn";
    }
    public class VPCRequest
    {
        Uri _address;
        SortedList<String, String> _requestFields = new SortedList<String, String>(new VPCStringComparer());
        String _rawResponse;
        SortedList<String, String> _responseFields = new SortedList<String, String>(new VPCStringComparer());
        String _secureSecret;


        public VPCRequest(String URL)
        {
            _address = new Uri(URL);
        }

        public void SetSecureSecret(String secret)
        {
            _secureSecret = secret;
        }

        public void AddDigitalOrderField(String key, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestFields.Add(key, value);
            }
        }

        public String GetResultField(String key, String defValue)
        {
            String value;
            if (_responseFields.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return defValue;
            }
        }

        public String GetResultField(String key)
        {
            return GetResultField(key, "");
        }

        private String GetRequestRaw()
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _requestFields)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value) + "&");
                }
            }
            //remove trailing & from string
            if (data.Length > 0)
                data.Remove(data.Length - 1, 1);
            return data.ToString();
        }

        public string GetTxnResponseCode()
        {
            return GetResultField("vpc_TxnResponseCode");
        }

        //_____________________________________________________________________________________________________
        // Three-Party order transaction processing

        public String Create3PartyQueryString()
        {
            StringBuilder url = new StringBuilder();
            //Payment Server URL
            url.Append(_address);
            url.Append("?");
            //Create URL Encoded request string from request fields 
            url.Append(GetRequestRaw());
            //Hash the request fields
            url.Append("&vpc_SecureHash=");
            url.Append(CreateSHA256Signature(true));
            return url.ToString();
        }

        public string Process3PartyResponse(System.Collections.Specialized.NameValueCollection nameValueCollection)
        {
            foreach (string item in nameValueCollection)
            {
                if (!item.Equals("vpc_SecureHash") && !item.Equals("vpc_SecureHashType"))
                {
                    _responseFields.Add(item, nameValueCollection[item]);
                }

            }

            if (!nameValueCollection["vpc_TxnResponseCode"].Equals("0") && !String.IsNullOrEmpty(nameValueCollection["vpc_Message"]))
            {
                if (!String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
                {
                    if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
                    {
                        return "INVALIDATED";
                    }
                    return "CORRECTED";
                }
                return "CORRECTED";
            }

            if (String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
            {
                return "INVALIDATED";//no sercurehash response
            }
            if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
            {
                return "INVALIDATED";
            }
            return "CORRECTED";
        }

        //_____________________________________________________________________________________________________

        private class VPCStringComparer : IComparer<String>
        {
            /*
             <summary>Customised Compare Class</summary>
             <remarks>
             <para>
             The Virtual Payment Client need to use an Ordinal comparison to Sort on 
             the field names to create the SHA256 Signature for validation of the message. 
             This class provides a Compare method that is used to allow the sorted list 
             to be ordered using an Ordinal comparison.
             </para>
             </remarks>
             */

            public int Compare(String a, String b)
            {
                /*
                 <summary>Compare method using Ordinal comparison</summary>
                 <param name="a">The first string in the comparison.</param>
                 <param name="b">The second string in the comparison.</param>
                 <returns>An int containing the result of the comparison.</returns>
                 */

                // Return if we are comparing the same object or one of the 
                // objects is null, since we don't need to go any further.
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                // Ensure we have string to compare
                string sa = a as string;
                string sb = b as string;

                // Get the CompareInfo object to use for comparing
                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    // Compare using an Ordinal Comparison.
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }

        //______________________________________________________________________________
        // SHA256 Hash Code

        public string CreateSHA256Signature(bool useRequest)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[_secureSecret.Length / 2];
            for (int i = 0; i < _secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            SortedList<String, String> list = (useRequest ? _requestFields : _responseFields);
            foreach (KeyValuePair<string, string> kvp in list)
            {
                if (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_"))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }
    }
    public class CartController : Controller
    {
        // GET: Cart
        readonly TraSuaEntities _db = new TraSuaEntities();
        private readonly IActionContextAccessor _accessor;

        public CartController(IActionContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            var list = new List<GioHangModel>();
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                foreach (var item in cart)
                {
                    var cartitem = new GioHangModel();

                    var product = new ProductDao().viewDetail(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        cartitem.MaSanPham = item.MaSanPham;
                        cartitem.Id = item.Id;
                        cartitem.TenSanPham = product.TenSanPham;
                        cartitem.Anh = product.Anh;
                        cartitem.SoLuong = item.SoLuong;
                        cartitem.SanPhamThu = item.SanPhamThu;
                        cartitem.GiaBan = new CartDao().Tien1LyTraSua(session.UserId, product.Id);
                        cartitem.ThanhTien = cartitem.GiaBan * cartitem.SoLuong;
                        cartitem.MoTa = new CartDao().getMoTa(product.Id, session.UserId);
                        list.Add(cartitem);
                    }

                }
                return View(list);
            }
            else
            {
                return Redirect("/SanPham/Index");
            }

        }
        public ActionResult Onepay()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            var list = new List<GioHangModel>();
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                foreach (var item in cart)
                {
                    var cartitem = new GioHangModel();

                    var product = new ProductDao().viewDetail(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        cartitem.MaSanPham = item.MaSanPham;
                        cartitem.Id = item.Id;
                        cartitem.TenSanPham = product.TenSanPham;
                        cartitem.Anh = product.Anh;
                        cartitem.SoLuong = item.SoLuong;
                        cartitem.SanPhamThu = item.SanPhamThu;
                        cartitem.GiaBan = new CartDao().Tien1LyTraSua(session.UserId, product.Id);
                        cartitem.ThanhTien = cartitem.GiaBan * cartitem.SoLuong;
                        cartitem.MoTa = new CartDao().getMoTa(product.Id, session.UserId);
                        list.Add(cartitem);
                    }

                }

                string amount = list.Sum(x => x.ThanhTien).ToString();
                var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                string url = RedirectOnepay(RandomString(), amount, ip);
                return Redirect(url);
            }
            else
            {
                return Redirect("/SanPham/Index");
            }

        }
        public IActionResult OnepayResponse()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            var list = new List<GioHangModel>();
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                foreach (var item in cart)
                {
                    var cartitem = new GioHangModel();

                    var product = new ProductDao().viewDetail(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        cartitem.MaSanPham = item.MaSanPham;
                        cartitem.Id = item.Id;
                        cartitem.TenSanPham = product.TenSanPham;
                        cartitem.Anh = product.Anh;
                        cartitem.SoLuong = item.SoLuong;
                        cartitem.SanPhamThu = item.SanPhamThu;
                        cartitem.GiaBan = new CartDao().Tien1LyTraSua(session.UserId, product.Id);
                        cartitem.ThanhTien = cartitem.GiaBan * cartitem.SoLuong;
                        cartitem.MoTa = new CartDao().getMoTa(product.Id, session.UserId);
                        list.Add(cartitem);
                    }

                }


                string hashvalidateResult = "";

                // Khoi tao lop thu vien
                VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
                conn.SetSecureSecret(OnepayProperty.HASH_CODE);

                // Xu ly tham so tra ve va du lieu ma hoa
                string a = "https://" + Request.Host.ToString() + "/Cart/Onepay" + Request.QueryString.ToString();
                hashvalidateResult = conn.Process3PartyResponse(HttpUtility.ParseQueryString(new Uri(a).Query));

                // Lay tham so tra ve tu cong thanh toan
                string vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode");
                string amount = conn.GetResultField("vpc_Amount");
                string localed = conn.GetResultField("vpc_Locale");
                string command = conn.GetResultField("vpc_Command");
                string version = conn.GetResultField("vpc_Version");
                string cardType = conn.GetResultField("vpc_Card");
                string orderInfo = conn.GetResultField("vpc_OrderInfo");
                string merchantID = conn.GetResultField("vpc_Merchant");
                string authorizeID = conn.GetResultField("vpc_AuthorizeId");
                string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef");
                string transactionNo = conn.GetResultField("vpc_TransactionNo");
                string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode");
                string txnResponseCode = vpc_TxnResponseCode;
                string message = conn.GetResultField("vpc_Message");

                // Kiem tra 2 tham so tra ve quan trong nhat
                if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
                {
                    var tong = list.Sum(x => x.ThanhTien);
                    return RedirectToAction("CreateBill", "Bill", new { tongtien = tong });
                    //return View("PaySuccess");
                }
                else
                {
                    return RedirectToAction("Index", "Cart", null);
                    //return Content("PayUnSuccess");
                }
            }
            else
            {
                return Redirect("/SanPham/Index");
            }
            
        }
        /// <summary>
        /// Sinh ky tu ngau nhien
        /// </summary>
        private string RandomString()
        {
            var str = new StringBuilder();
            var random = new Random();
            for (int i = 0; i <= 5; i++)
            {
                var c = Convert.ToChar(Convert.ToInt32(random.Next(65, 68)));
                str.Append(c);
            }
            return str.ToString().ToLower();
        }

        /// <summary>
        /// Redirect den onepay
        /// </summary>
        public string RedirectOnepay(string codeInvoice, string amount, string ip)
        {
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayProperty.HASH_CODE);

            // Gan cac thong so de truyen sang cong thanh toan onepay
            conn.AddDigitalOrderField("AgainLink", OnepayProperty.AGAIN_LINK);
            conn.AddDigitalOrderField("Title", "Bách hóa online");
            conn.AddDigitalOrderField("vpc_Locale", OnepayProperty.PAYGATE_LANGUAGE);
            conn.AddDigitalOrderField("vpc_Version", OnepayProperty.VERSION);
            conn.AddDigitalOrderField("vpc_Command", OnepayProperty.COMMAND);
            conn.AddDigitalOrderField("vpc_Merchant", OnepayProperty.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", OnepayProperty.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", RandomString());
            conn.AddDigitalOrderField("vpc_OrderInfo", codeInvoice);
            conn.AddDigitalOrderField("vpc_Amount", amount);
            conn.AddDigitalOrderField("vpc_ReturnURL", Url.Action("OnepayResponse", "Cart", null, Request.Scheme, null));

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            conn.AddDigitalOrderField("vpc_Customer_Phone", "");
            conn.AddDigitalOrderField("vpc_Customer_Email", "");
            conn.AddDigitalOrderField("vpc_Customer_Id", "");
            conn.AddDigitalOrderField("vpc_TicketNo", ip);

            string url = conn.Create3PartyQueryString();
            return url;
        }
        public IActionResult PaypalPartial()
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            var list = new List<GioHangModel>();
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                foreach (var item in cart)
                {
                    var cartitem = new GioHangModel();

                    var product = new ProductDao().viewDetail(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        cartitem.MaSanPham = item.MaSanPham;
                        cartitem.Id = item.Id;
                        cartitem.TenSanPham = product.TenSanPham;
                        cartitem.Anh = product.Anh;
                        cartitem.SoLuong = item.SoLuong;
                        cartitem.SanPhamThu = item.SanPhamThu;
                        cartitem.GiaBan = new CartDao().Tien1LyTraSua(session.UserId, product.Id);
                        cartitem.ThanhTien = cartitem.GiaBan * cartitem.SoLuong;
                        cartitem.MoTa = new CartDao().getMoTa(product.Id, session.UserId);
                        list.Add(cartitem);
                    }

                }
                return PartialView(list);
            }
            else
            {
                return Redirect("/SanPham/Index");
            }
        }
        public ActionResult CreateCart(string listproduct)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            string[] productsid = listproduct.Split(',');

            var sessionSpthu = HttpContext.Session.GetObjectFromJson<Common.Session.SanPhamThuSession>(Common.Constants.SANPHAMTHU_SESSION);
            int spThu = 0;
            if (sessionSpthu != null)
            {
                spThu = sessionSpthu.SanPham_Thu + 1;
            }
            else
            {
                spThu = 1;

            }
            var sanphamThu = new Common.Session.SanPhamThuSession();
            sanphamThu.SanPham_Thu = spThu;
            HttpContext.Session.SetObjectAsJson(Common.Constants.SANPHAMTHU_SESSION, sanphamThu);

            foreach (var item in productsid)
            {

                GioHang giohang = new GioHang
                {
                    MaKhachHang = session.UserId,
                    MaSanPham = int.Parse(item),
                    SoLuong = 1,
                    ThuocSanPham = int.Parse(productsid[0]),
                    SanPhamThu = spThu
                };
                new CartDao().Insert(giohang);

            }
            return Redirect("/SanPham/Index");
        }
        public ActionResult UpdateCart(int productid, int? soluong, int sanphamthu)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var userid = session.UserId;
                var list = new CartDao().layDSSP(productid, userid, sanphamthu);
                foreach (var item in list)
                {
                    var cart = new GioHang();
                    new CartDao().Update(item.Id, soluong);
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult DeleteCart(int productid, int sanphamthu)
        {
            var session = HttpContext.Session.GetObjectFromJson<Common.Session.UserLogin>(Common.Constants.USER_SESSION);
            if (session != null)
            {
                var userid = session.UserId;
                var list = new CartDao().layDSSP(productid, userid, sanphamthu);
                int? spthu = 0;
                foreach (var item in list)
                {
                    new CartDao().Delete(item.Id);
                    spthu = item.SanPhamThu;
                }
                var listconlai = new CartDao().GetProductsByIdUser(userid);
                foreach(var item in listconlai)
                {
                    if(spthu < item.SanPhamThu)
                    {
                        var sanphamGiohang = _db.GioHangs.Find(item.Id);
                        sanphamGiohang.SanPhamThu = item.SanPhamThu - 1;
                        _db.SaveChanges();
                    }
                    
                }
            }
            return RedirectToAction("Index");
        }
    }
}