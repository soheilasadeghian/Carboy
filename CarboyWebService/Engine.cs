using CarBoyWebservice.ClassCollection;
using CarBoyWebservice.Models.Product;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using GoogleMapsApi.Entities.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CarBoyWebservice
{
    public partial class Engine
    {
        /// <summary>
        /// این متد وظیفه ذخیره مکان کاربوی ها بصورت لحظه ای را برعهده دارد
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public string registerUserLocation(long userID, double latitude, double longitude)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();

            var db = new DataAccessDataContext();
            var dt = new DateTime();
            dt = DateTime.Now;

            var lastTrack = db.UserGeoTrackTbls.SingleOrDefault(c => c.userId == userID);
            if (lastTrack == null)
            {
                var newTrack = new UserGeoTrackTbl();
                newTrack.longitude = longitude;
                newTrack.latitude = latitude;
                newTrack.userId = userID;
                newTrack.lastUpdate = dt;
                db.UserGeoTrackTbls.InsertOnSubmit(newTrack);
                db.SubmitChanges();
            }
            else
            {
                lastTrack.latitude = latitude;
                lastTrack.longitude = longitude;
                lastTrack.lastUpdate = dt;
                db.SubmitChanges();
            }


            return js.Serialize("");
        }

        /// <summary>
        /// این متد وظیفه ارسال لیست کالاهایی که تحویل کاربر است را برعهده دارد
        /// </summary>
        /// <returns></returns>
        public string getUserProductList(long userID)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();



#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var dt = new DateTime();
            dt = DateTime.Now;

            var user = db.UserTbls.Single(c => c.ID == userID);

            try
            {
                var deleting = user.AssetTbls.Where(c => c.quantity == 0);
                db.AssetTbls.DeleteAllOnSubmit(deleting);
                db.SubmitChanges();
            }
            catch { }

            var products = user.AssetTbls.Where(c => c.quantity > 0);
            if (products.Any())
            {
                var result = new
                {

                    product = products.Select(c =>
                    new
                    {
                        code = c.productCode,
                        name = c.ProductTbl.name,
                        quantity = c.quantity,
                        image = !c.ProductTbl.ProductImageTbls.Any() ? ""
                        : ClassCollection.Method.Url + c.ProductTbl.ProductImageTbls.Where(x => x.isMainImage == true)
                        .Take(1).Single().name
                    })

                };
                return js.Serialize(result);
            }
            else
            {
                var result = new List<Object>();
                return js.Serialize(new { product = result });
            }

        }

        /// <summary>
        /// این متد وظیفه ارسال لیست کالاهایی که مورد نیاز توزیع کننده در روز مورد نظر است را برعهده دارد
        /// </summary>
        /// <returns></returns>
        public string getRequiredDistributorProduct(long userID, long date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();


#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var dt = new DateTime();
            dt = DateTime.Now;

            var userDate = ClassCollection.Method.FromUnixTime(date);

            if (userDate.Date < dt.Date)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.DateTimeInvalid());
            }

            var nextTwoDay = userDate.AddDays(2);

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isDistributor)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var services = user.ServiceOrderTbls.Where(c => c.requestTime.Date >= userDate.Date && c.requestTime <= nextTwoDay.Date
            && c.state != (byte)ClassCollection.ServiceOrderState.CanceledByCustomer
            && c.state != (byte)ClassCollection.ServiceOrderState.CanceledByOperator
            && c.state != (byte)ClassCollection.ServiceOrderState.Done
            ).Select(c => c.ServiceOrderProductDetailTbls);

            var prod = new
            {
                product = services.SelectMany(x =>

                x.Select(p =>
                new
                {

                    code = p.productCode,
                    name = p.productName,
                    quantity = p.quantity,
                    requiredQuantity = p.quantity - (user.AssetTbls.Any(a => a.productCode == p.productCode) ? user.AssetTbls.Single(a => a.productCode == p.productCode).quantity : 0) <= 0 ? 0 :
                    Math.Abs(p.quantity - (user.AssetTbls.Any(a => a.productCode == p.productCode) ? user.AssetTbls.Single(a => a.productCode == p.productCode).quantity : 0)),
                    image = !p.ProductTbl.ProductImageTbls.Any() ? ""
                    : ClassCollection.Method.Url + p.ProductTbl.ProductImageTbls.Where(f => f.isMainImage == true)
                    .Take(1).Single().name

                }))
            };

            return js.Serialize(prod);
        }

        /// <summary>
        ///پیاده سازی متد مشاهده مکان قرارگیری اولیه توزیع کننده در روز مورد نظر
        /// </summary>
        public string geDistributorStartLocation(long userID, long date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();


            var db = new DataAccessDataContext();
            var dt = new DateTime();
            dt = DateTime.Now;

            var userDate = ClassCollection.Method.FromUnixTime(date);

            if (userDate.Date < dt.Date)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.DateTimeInvalid());
            }

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isDistributor)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var result = new Models.Global.LatLng();

            var start = user.DistributorStartLocationTbls.SingleOrDefault(c => c.regDate.Date == userDate.Date);

            if (start == null)
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.LocationNotFound());


            result.latitude = start.lat;
            result.longitude = start.lng;

            return js.Serialize(result);
        }

        /// <summary>
        ///این متد وظیفه ارسال تصویر کلی مسیر کاربوی برای روز کورد نظر را برعهده دارد
        /// </summary>
        public string getCarboyPath(long userID, long date, int width)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;
#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
            date = ClassCollection.Method.ToUnixTime(dt.Date);
#else
            var db = new DataAccessDataContext();
#endif



            var userDate = ClassCollection.Method.FromUnixTime(date);

            if (userDate.Date < dt.Date)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.DateTimeInvalid());
            }

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            string fileName = user.ID + "_" + ClassCollection.Method.ToUnixTime(userDate.Date) + ".jpg";


            if (File.Exists(HttpContext.Current.Server.MapPath(ClassCollection.Method.UploadPath + fileName)))
            {

                var result = new
                {
                    url = ClassCollection.Method.UploadUrl + fileName
                };

                return js.Serialize(result);
            }
            else
            {

                var services = user.ServiceOrderTbls1.Where(c => c.requestTime.Date == userDate.Date);
                string u = "";

                if (services.Any())
                {
                    var pathes = services.Select(c => new Location(c.latitude, c.longitude));

                    var img = ClassCollection.Method.getMap(pathes.ToList(), width);

                    img.Save(HttpContext.Current.Server.MapPath(ClassCollection.Method.UploadPath + fileName));

                    u = ClassCollection.Method.UploadUrl + fileName;

                }

                var result = new
                {
                    url = u
                };

                return js.Serialize(result);

            }
        }

        /// <summary>
        ///این متد وظیفه درخواست انتقال کالا از یک شخص به شخص دیگر را بر عهده دارد
        /// </summary>
        public string userRequestPassProduct(long userID, string phone, string product)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif

            if (!MBProtoLib.Utils.Validation.IsMobile(phone))
            {
                throw new MBProtoLib.Exceptions.AuthException(new MBProtoLib.Exceptions.AuthException.PhoneNumberInvalid(phone));
            }

            List<DeliverProduct> productList = null;
            var user = db.UserTbls.Single(c => c.ID == userID);

            var destinationUser = db.UserTbls.SingleOrDefault(
                c => c.mobile == phone
            && c.status == (byte)ClassCollection.DeleteStatus.Active);

            if (destinationUser == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.UserNotRegistered());
            }

            try
            {
                productList = js.Deserialize<List<DeliverProduct>>(product);

                var products = user.AssetTbls.Where(c => c.quantity > 0).Select(c =>
                    new
                    {
                        code = c.productCode,
                        quantity = c.quantity
                    });

                foreach (var pro in productList)
                {
                    var p = products.SingleOrDefault(c => c.code == pro.code);

                    if (p == null)
                        throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());

                    if (pro.quantity > p.quantity)
                        throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());
                }
            }
            catch
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());
            }

            var hash = ClassCollection.Method.Encrypt(product + "|" + phone + "|" + userID + "|" + ClassCollection.Method.ToUnixTime(dt));

            destinationUser.signCode = ClassCollection.Method.RandomNumber();
            destinationUser.signCodeRegDate = dt;
            db.SubmitChanges();

            MBProto.Utils.SMS.SendSms(phone, destinationUser.signCode);
            ClassCollection.PushManagement.SendToUser(destinationUser.ID, "امضا تحویل کالا\n" + destinationUser.signCode, "تحویل کالا در کاربوی");
            //push
            //email


            return js.Serialize(new { hash = hash });
        }

        /// <summary>
        ///این متد وظیفه انتقال نهایی کالا از یک شخص به شخص دیگر را بر عهده دارد
        /// </summary>
        public string userPassProduct(long userID, string hash, string smsCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif

            string decryptedHash = "";
            long embedUser = 0;
            string phone = "";

            try
            {
                decryptedHash = ClassCollection.Method.Decrypt(hash).Split('|')[0];
                phone = ClassCollection.Method.Decrypt(hash).Split('|')[1];
                embedUser = long.Parse(ClassCollection.Method.Decrypt(hash).Split('|')[2]);
                long date = long.Parse(ClassCollection.Method.Decrypt(hash).Split('|')[3]);

                if ((dt - ClassCollection.Method.FromUnixTime(date)).TotalMinutes > 2)
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductHashInvalid());

                if (embedUser != userID)
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductHashInvalid());

            }
            catch
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductHashInvalid());
            }

            var destinationUser = db.UserTbls.SingleOrDefault(
              c => c.mobile == phone &&
              c.status == (byte)ClassCollection.DeleteStatus.Active);


            if (destinationUser == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.UserNotRegistered());
            }

            if (destinationUser.signCode != smsCode)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.SmsCodeInvalid());
            }
            if (destinationUser.signCodeRegDate.HasValue && (dt - destinationUser.signCodeRegDate.Value).TotalMinutes > 2)
            {
                destinationUser.signCode = "";
                destinationUser.signCodeRegDate = null;
                db.SubmitChanges();

                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.SmsCodeExpired());
            }

            var user = db.UserTbls.Single(c => c.ID == userID);

            try
            {
                List<DeliverProduct> productList = js.Deserialize<List<DeliverProduct>>(decryptedHash);

                var products = user.AssetTbls.Where(c => c.quantity > 0).Select(c =>
                    new
                    {
                        ID = c.productID,
                        code = c.productCode,
                        quantity = c.quantity,
                        description = c.ProductTbl.description
                    });

                foreach (var pro in productList)
                {
                    var p = products.SingleOrDefault(c => c.code == pro.code);

                    if (p == null)
                        throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());

                    if (pro.quantity > p.quantity)
                        throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());
                }

                var Receipt = new ReceiptTbl();

                Receipt.code = ClassCollection.Method.RNGCharacterMask();
                Receipt.description = "انتقال کالا از " + (user.name + " " + user.family) + " به " + (destinationUser.name + " " + destinationUser.family) + "در تاریخ " + ClassCollection.Method.persianFormatDate_n(dt);
                Receipt.providerUserFamily = user.family;
                Receipt.providerUserId = user.ID;
                Receipt.providerUserMobile = user.mobile;
                Receipt.providerUserName = user.name;
                Receipt.receiverUserFamily = destinationUser.family;
                Receipt.receiverUserId = destinationUser.ID;
                Receipt.receiverUserName = destinationUser.name;
                Receipt.regDate = dt;
                Receipt.status = (byte)ClassCollection.DeleteStatus.Active;

                db.ReceiptTbls.InsertOnSubmit(Receipt);
                db.SubmitChanges();

                foreach (var item in productList)
                {

                    var p = db.ProductTbls.SingleOrDefault(c => c.code == item.code);
                    var destAssets = destinationUser.AssetTbls.SingleOrDefault(c => c.productID == p.ID);
                    var userAssets = user.AssetTbls.SingleOrDefault(c => c.productID == p.ID);

                    if (destAssets == null)
                    {
                        var ass = new AssetTbl();
                        ass.productCode = p.code;
                        ass.productID = p.ID;
                        ass.quantity = (int)p.quantity;
                        ass.userID = destinationUser.ID;

                        db.AssetTbls.InsertOnSubmit(ass);
                        db.SubmitChanges();
                    }
                    else
                    {
                        destAssets.quantity += item.quantity;
                        db.SubmitChanges();
                    }

                    userAssets.quantity -= item.quantity;
                    db.SubmitChanges();

                    var recDetail = new ReceiptDetailTbl();

                    recDetail.productCode = p.code;
                    recDetail.productDescription = p.description;
                    recDetail.productId = p.ID;
                    recDetail.productName = p.name;
                    recDetail.productPrice = p.price;
                    recDetail.productUnitName = p.UnitTbl.name;
                    recDetail.quantity = item.quantity;
                    recDetail.receiptId = Receipt.ID;
                    recDetail.unitPrice = p.price;

                    db.ReceiptDetailTbls.InsertOnSubmit(recDetail);
                    db.SubmitChanges();

                    foreach (var attr in p.Product_AttributeTbls)
                    {
                        var field = new ReceiptDetailProductFieldsTbl();

                        field.name = attr.AttributeTbl.name;
                        field.value = attr.value;

                        db.ReceiptDetailProductFieldsTbls.InsertOnSubmit(field);
                    }

                    foreach (var spec in p.Product_SpecificationOptionTbls)
                    {
                        var field = new ReceiptDetailProductFieldsTbl();

                        field.name = spec.SpecificationOptionTbl.SpecificationTbl.name;
                        field.value = spec.SpecificationOptionTbl.name;

                        db.ReceiptDetailProductFieldsTbls.InsertOnSubmit(field);
                    }

                    db.SubmitChanges();

                }

            }
            catch
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ProductListInvalid());
            }

            return js.Serialize("");
        }

        /// <summary>
        ///این متد وظیفه درخواست اعلام پایان کار توسط کاربر را برعهده دارد
        /// </summary>
        public string carboyRequestServiceComplete(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;


#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif


            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(c => c.code == serviceCode && c.state == (byte)ClassCollection.ServiceOrderState.Proccessing);

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }
            if (serviceOrder.ServiceOrderProductDetailTbls.Any() && !serviceOrder.distributeDone)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceProductNotDelivered());
            }
            var hash = ClassCollection.Method.Encrypt(serviceOrder.ID + "|" + userID + "|" + ClassCollection.Method.ToUnixTime(dt));

            if (serviceOrder.signCodeRerty.HasValue == false)
            {
                serviceOrder.signCodeRerty = 0;
                db.SubmitChanges();
            }

            if (serviceOrder.signCodeRerty.Value <= 1)
            {
                serviceOrder.signCode = ClassCollection.Method.RandomNumber();
                serviceOrder.signCodeRegDate = dt;
                serviceOrder.signCodeRerty = (byte)(serviceOrder.signCodeRerty.Value + 1);
                db.SubmitChanges();

            }
            else
            {
                serviceOrder.signCodeRerty = (byte)(serviceOrder.signCodeRerty.Value + 1);
                db.SubmitChanges();
                CallManagement.SendToFileShare(serviceOrder.customerMobile, "CallServiceOrderFinish.carboy", serviceOrder.signCode.ToCharArray());

                return js.Serialize(new { hash = hash, payment = serviceOrder.payStatus != (byte)ClassCollection.ServiceOrderPaymentStatus.No_Pay });
            }



            var customerMessage = new CustomerMessageTbl();
            customerMessage.customerId = serviceOrder.customerId;
            customerMessage.isRead = false;
            customerMessage.mobile = serviceOrder.customerMobile;
            customerMessage.regDate = dt;
            customerMessage.text = "امضا پایان خدمت" + "\n" + serviceOrder.signCode;
            customerMessage.type = (byte)ClassCollection.CustomerMessageType.System;
            db.CustomerMessageTbls.InsertOnSubmit(customerMessage);
            db.SubmitChanges();

            //MBProto.Utils.SMS.SendSms(serviceOrder.customerMobile, "لطفا کد " + serviceOrder.signCode + " جهت اتمام سرویس به کاربوی اعلام گردد.");
            //PushManagement.SendToCustomer(serviceOrder.customerId, "امضا پایان خدمت" + "\n" + serviceOrder.signCode, "درخواست پایان کار در کاربوی");

            return js.Serialize(new { hash = hash, payment = serviceOrder.payStatus != (byte)ClassCollection.ServiceOrderPaymentStatus.No_Pay });
        }

        /// <summary>
        ///این متد وظیفه اعلام پایان کار نهایی توسط کاربر را برعهده دارد
        /// </summary>
        public string carboyServiceComplete(long userID, string hash, string smsCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif


            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            long decryptedHash = 0;
            long embedUser = 0;

            try
            {
                decryptedHash = long.Parse(ClassCollection.Method.Decrypt(hash).Split('|')[0]);
                embedUser = long.Parse(ClassCollection.Method.Decrypt(hash).Split('|')[1]);
                long date = long.Parse(ClassCollection.Method.Decrypt(hash).Split('|')[2]);

                if ((dt - ClassCollection.Method.FromUnixTime(date)).TotalMinutes > 2)
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceOrderHashInvalid());

                if (embedUser != userID)
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceOrderHashInvalid());

            }
            catch
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceOrderHashInvalid());
            }

            var serviceOrder = db.ServiceOrderTbls.SingleOrDefault(c => c.ID == decryptedHash && c.state == (byte)ClassCollection.ServiceOrderState.Proccessing);

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            //تعیین نحوه پرداخت اگر نقدی پرداخت شده بود
            if (serviceOrder.payStatus == (byte)ClassCollection.ServiceOrderPaymentStatus.No_Pay)
                serviceOrder.payStatus = (byte)ClassCollection.ServiceOrderPaymentStatus.Pay_Cash;

            //تغییر وضعیت سفارش به انجام شده
            serviceOrder.state = (byte)ClassCollection.ServiceOrderState.Done;
            serviceOrder.DoneDate = dt;

            //AddOrderCount
            //تغییر تعداد سرویس های انجام شده مشتری 
            var customer = serviceOrder.CustomerTbl;
            customer.serviceCount += 1;
            db.SubmitChanges();

            var ServiceOrderCountForGiftCard = db.SettingTbls.SingleOrDefault(p => p.name == "ServiceOrderCountForGiftCard").value;
            var packageId = db.PackageTbls.SingleOrDefault(p => p.name == "کارواش سبز").ID;
            if (customer.serviceCount >= Int32.Parse(ServiceOrderCountForGiftCard))
            {
                //تولید کارت هدیه
                Random generator = new Random((int)serviceOrder.customerId);
                var rnd = generator.Next(100, 999).ToString("D3");
                string code = "CarWash" + serviceOrder.customerId + rnd;
                var startDate = dt;
                var endDate = dt.AddDays(30);

                var gCard = new GiftCardTbl();
                gCard.code = code;
                gCard.startTime = startDate;
                gCard.endTime = endDate;
                gCard.isPercent = true;
                gCard.maxPrice = 30000;
                gCard.maxReuse = 1;
                gCard.totalReuse = 0;
                gCard.name = "جایزه تعداد سفارشات";
                gCard.presenterId = null;
                gCard.type = (byte)GiftType.OrderCount;
                gCard.customerId = serviceOrder.customerId;
                gCard.status = 0;
                gCard.value = 50;
                gCard.regDate = dt;
                gCard.packageId = packageId;
                db.GiftCardTbls.InsertOnSubmit(gCard);
                db.SubmitChanges();

                var customerGift = new Customer_GiftCardTbl();
                customerGift.customerFamily = customer.family;
                customerGift.customerId = customer.ID;
                customerGift.customerMobile = customer.mobile;
                customerGift.customerName = customer.name;
                customerGift.date = dt;
                customerGift.giftCardId = gCard.ID;
                customerGift.reUsedCount = gCard.maxReuse;
                db.Customer_GiftCardTbls.InsertOnSubmit(customerGift);
                db.SubmitChanges();

                #region send message

                var sms = db.SystemMessageTextTbls.SingleOrDefault(c => c.name == "SMS_NEW_OrderCountGiftCard");
                if (sms != null)
                {
                    var msg = sms.value.Replace("[customerName]", customerGift.customerName)
                        .Replace("[customerFamily]", customerGift.customerFamily)
                        .Replace("[code]", code);

                    MBProto.Utils.SMS.SendSms(customerGift.customerMobile, msg);
                    PushManagement.SendToCustomer(customerGift.customerId, msg, "کارت هدیه");

                    var customerMessage = new CustomerMessageTbl();
                    customerMessage.customerId = customerGift.customerId;
                    customerMessage.text = msg;
                    customerMessage.mobile = customerGift.customerMobile;
                    customerMessage.regDate = dt;
                    customerMessage.type = (byte)ClassCollection.CustomerMessageType.System;
                    db.CustomerMessageTbls.InsertOnSubmit(customerMessage);
                    db.SubmitChanges();
                }
                #endregion
            }
            //AddOrderCount
            //خروج کالا از تحویلی های کاربوی
            if (serviceOrder.ServiceOrderProductDetailTbls.Any())
            {
                foreach (var pro in serviceOrder.ServiceOrderProductDetailTbls)
                {
                    var asset = user.AssetTbls.SingleOrDefault(c => c.productID == pro.productId);
                    if (asset != null)
                    {
                        asset.quantity -= (int)pro.quantity;
                    }
                }
            }

            db.SubmitChanges();

            #region ویرایش زمان رزور شده

            var Date = serviceOrder.requestTime.Date;
            var time = serviceOrder.requestTime.TimeOfDay;
            var activeDay = db.ActiveDayTbls.Single(c => c.isActive == true && c.date.Date == Date);
            var timeSheet = user.RepairmanTimeSheet1Tbls.SingleOrDefault(c => c.customerId == serviceOrder.customerId && c.startTime == time && c.activeDayId == activeDay.ID);

            #region محاسبه میانگین زمان گردش در مناطقی که یک تعمیرکار در آنها کار می کند
            var avgTicks = db.Repairman_AreaTbls.Where(c => c.repairmanId == serviceOrder.repairmanId).Average(c => c.AreaTbl.time.Ticks);
            var avgTimeSpan = new TimeSpan((long)avgTicks);
            #endregion

            //درصورتی که کاربر کار را زودتر از مدت زمان تعیین شده انجام داد و
            //همچنین مدت زمان باقی مانده تا پایان سرویس که در سیستم ثبت شده بیشتر باشد از زمان لازم برای رفت و آمد کاربر تا سرویس بعدی
            if (dt.TimeOfDay < timeSheet.endTime && avgTimeSpan < (timeSheet.endTime - dt.TimeOfDay))
            {
                var endTime = dt.TimeOfDay + avgTimeSpan;
                timeSheet.endTime = new TimeSpan(endTime.Hours, endTime.Minutes, endTime.Seconds);
                db.SubmitChanges();
            }

            //اینمورد هرگز نباید اتفاق بیفتد.چون در متد شروع سرویس توسط کاربوی ،زمان شروع سرویس رادر تایمشیت آپدیت می کنیم
            //if (dt < serviceOrder.requestTime)
            //{
            //    #region محاسبه میانگین زمان گردش در مناطقی که یک تعمیرکار در آنها کار می کند
            //    var avgTicks = db.Repairman_AreaTbls.Where(c => c.repairmanId == serviceOrder.repairmanId).Average(c => c.AreaTbl.time.Ticks);
            //    var avgTimeSpan = new TimeSpan((long)avgTicks);
            //    #endregion

            //    var endTime = dt.TimeOfDay + avgTimeSpan;
            //    timeSheet.endTime = new TimeSpan(endTime.Hours, endTime.Minutes, endTime.Seconds);
                  
            //    db.SubmitChanges();
            //}
            #endregion

            #region جایزه دادن به معرفی کننده در صورت وجود

            if (serviceOrder.customer_GiftCardId.HasValue)
            {

                var giftCard = serviceOrder.Customer_GiftCardTbl.GiftCardTbl;
                if (giftCard.type == (byte)GiftType.ByPresenter)
                {
                    var referalGiftCardSetting = db.ReferalGiftSettingTbls.SingleOrDefault(c => c.status == 0 && c.code == "presenter");
                    var startDate = dt;
                    var enDate = dt.AddDays(referalGiftCardSetting.days);

                    if (giftCard.presenterId.HasValue && referalGiftCardSetting != null)
                    {
                        var presenter = giftCard.CustomerTbl;

                        Random generator = new Random((int)presenter.ID);
                        var rnd = generator.Next(100, 999).ToString("D3");

                        string refid = "ref" + serviceOrder.customerId + rnd;

                        #region تولید کارت هدیه برای معرفی کننده
                        var gCard = new GiftCardTbl();
                        gCard.code = refid;
                        gCard.startTime = startDate;
                        gCard.endTime = enDate;
                        gCard.isPercent = referalGiftCardSetting.isPercent;
                        gCard.maxPrice = referalGiftCardSetting.maxPrice;
                        gCard.maxReuse = referalGiftCardSetting.maxReuse;
                        gCard.totalReuse = referalGiftCardSetting.totalReuse;
                        gCard.name = "جایزه معرفی";
                        gCard.presenterId = serviceOrder.customerId;
                        gCard.type = (byte)GiftType.ByRepresenter;
                        gCard.customerId = giftCard.presenterId;
                        gCard.status = 0;
                        gCard.value = referalGiftCardSetting.value;
                        gCard.regDate = dt;
                        db.GiftCardTbls.InsertOnSubmit(gCard);
                        db.SubmitChanges();
                        #endregion

                        #region ثبت کارت هدیه در لیست کارت های هدیه معرفی کننده
                        var customerGift = new Customer_GiftCardTbl();
                        customerGift.customerFamily = giftCard.CustomerTbl.family;
                        customerGift.customerId = giftCard.presenterId.Value;
                        customerGift.customerMobile = giftCard.CustomerTbl.mobile;
                        customerGift.customerName = giftCard.CustomerTbl.name;
                        customerGift.date = dt;
                        customerGift.giftCardId = gCard.ID;
                        customerGift.reUsedCount = gCard.maxReuse;
                        db.Customer_GiftCardTbls.InsertOnSubmit(customerGift);

                        if (giftCard.totalReuse != -1)
                            giftCard.totalReuse -= 1;

                        db.SubmitChanges();
                        #endregion

                        #region send message

                        //SMS_NEW_ReferalGiftCard
                        var sms = db.SystemMessageTextTbls.SingleOrDefault(c => c.name == "SMS_NEW_ReferalGiftCard");
                        if (sms != null)
                        {
                            var msg = sms.value.Replace("[customerName]", customerGift.customerName)
                                .Replace("[customerFamily]", customerGift.customerFamily)
                                .Replace("[code]", refid);

                            MBProto.Utils.SMS.SendSms(customerGift.customerMobile, msg);
                            PushManagement.SendToCustomer(customerGift.customerId, msg, "کارت هدیه");


                            var customerMessage = new CustomerMessageTbl();
                            customerMessage.customerId = customerGift.customerId;
                            customerMessage.text = msg;
                            customerMessage.mobile = customerGift.customerMobile;
                            customerMessage.regDate = dt;
                            customerMessage.type = (byte)ClassCollection.CustomerMessageType.System;
                            db.CustomerMessageTbls.InsertOnSubmit(customerMessage);
                            db.SubmitChanges();
                        }


                        #endregion

                    }

                }
            }

            #endregion

            #region ثبت در جدول نظرسنجی های کاربوی

            var pendingRate = new CustomerPendingRateTbl();
            pendingRate.customerId = serviceOrder.customerId;
            pendingRate.repairmanId = serviceOrder.repairmanId;
            pendingRate.serviceOrderCode = serviceOrder.code;
            pendingRate.serviceOrderId = serviceOrder.ID;
            db.CustomerPendingRateTbls.InsertOnSubmit(pendingRate);
            db.SubmitChanges();

            #endregion

            //ارسال پیام پایان کار

            string message = db.SystemMessageTextTbls.Single(c => c.name == "SMS_ServiceOrderComplete").value;

            message = message.Replace("[customerName]", serviceOrder.customerName).Replace("[customerFamily]", serviceOrder.customerFamily).Replace("[code]", serviceOrder.code);

            MBProto.Utils.SMS.SendSms(serviceOrder.customerMobile, message);

            ClassCollection.PushManagement.SendToCustomer(serviceOrder.customerId, message, "خدمت شما با موفقیت پایان یافت");
            return js.Serialize("");
        }

        /// <summary>
        ///این متد وظیفه اعلام عدم امکان ارائه خدمت توسط کاربوی را برعهده دارد
        /// </summary>
        public string carboyRequestCancel(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(c => c.code == serviceCode
            && (c.state == (byte)ClassCollection.ServiceOrderState.Proccessing
            || c.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer
            || c.state == (byte)ClassCollection.ServiceOrderState.Idle)
            && c.requestTime.Date == dt.Date
            );

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            var carborCancelling = new RepairmanCancelingOrderTbl();
            carborCancelling.byRepairman = true;
            carborCancelling.registerDate = dt;
            carborCancelling.serviceOrderId = serviceOrder.ID;

            db.RepairmanCancelingOrderTbls.InsertOnSubmit(carborCancelling);
            db.SubmitChanges();

            return js.Serialize("");
        }

        /// <summary>
        ///این متد وظیفه اعلام عدم امکان ارائه خدمت توسط توزیع کننده را برعهده دارد
        /// </summary>
        public string distributorRequestCancel(long userID)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isDistributor)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var disCancelling = new DistributorCancelingOrderTbl();
            disCancelling.byDistributor = true;
            disCancelling.regDate = dt;
            db.DistributorCancelingOrderTbls.InsertOnSubmit(disCancelling);
            db.SubmitChanges();

            return js.Serialize("");
        }

        /// <summary>
        ///این متد وظیفه اعلام شروع به حرکت به سمت مشتری توسط کاربوی را برعهده دارد
        /// </summary>
        public string carboyMoveToCustomer(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var activeService = user.ServiceOrderTbls1.Where(c =>
            c.requestTime.Date == dt.Date
            &&
            (c.state == (byte)ClassCollection.ServiceOrderState.Idle))
            .OrderBy(c => c.requestTime).Take(1).SingleOrDefault();

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(c => c.code == serviceCode);

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            if (activeService == null || serviceOrder.ID != activeService.ID)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            if (user.ServiceOrderTbls1.Any(c =>
            (c.state == (byte)ClassCollection.ServiceOrderState.Idle
             ||
             c.state == (byte)ClassCollection.ServiceOrderState.Proccessing
             ||
             c.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer)
             &&
             (c.ID != serviceOrder.ID
             &&
             c.requestTime.Date == serviceOrder.requestTime.Date
             &&
             c.requestTime <= serviceOrder.requestTime)
             ))
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            serviceOrder.state = (byte)ClassCollection.ServiceOrderState.MoveToCustomer;
            serviceOrder.moveToCustomerDate = dt;
            db.SubmitChanges();

            UserGeoTrackTbl geoTrackID = null;

            if (user.UserGeoTrackTbls.Any())
            {
                geoTrackID = user.UserGeoTrackTbls.SingleOrDefault();
            }

            string TrackHash = Base64UrlEncoder.Encode(ClassCollection.Method.Encrypt(

                geoTrackID.ID
                + "|" + serviceOrder.ID
                ));

            UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAw7DIrQdashMVoy6KrbVRla97e_DoOzYE",
                ApplicationName = "ir.tana.carboy",
            });

            var m = new Google.Apis.Urlshortener.v1.Data.Url();
            m.LongUrl = "https://carman.carboy.info/tracker/" + TrackHash;
            string link = service.Url.Insert(m).Execute().Id;

            var SMS_MoveToCustomer = db.SystemMessageTextTbls.SingleOrDefault(c => c.name == "SMS_MoveToCustomer").value;

            var sms = SMS_MoveToCustomer.Replace("[link]", link);

            MBProto.Utils.SMS.SendSms(serviceOrder.customerMobile, sms);
            ClassCollection.PushManagement.SendToCustomer(serviceOrder.customerId, sms, "کاربوی به سمت شما حرکت کرد", link);

            return js.Serialize("");
        }

        /// <summary>
        ///پیاده سازی متد اعلام شروع به کار توسط کاربوی
        /// </summary>
        public string carboyStartService(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(c => c.code == serviceCode && c.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer);

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            if (serviceOrder.ServiceOrderProductDetailTbls.Any() && !serviceOrder.distributeDone)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceProductNotDelivered());
            }

            serviceOrder.state = (byte)ClassCollection.ServiceOrderState.Proccessing;
            serviceOrder.processingDate = dt;
            db.SubmitChanges();


            #region ویرایش زمان رزور شده

            var Date = serviceOrder.requestTime.Date;
            var time = serviceOrder.requestTime.TimeOfDay;
            var activeDay = db.ActiveDayTbls.Single(c => c.isActive == true && c.date.Date == Date);
            var timeSheet = user.RepairmanTimeSheet1Tbls.SingleOrDefault(c => c.customerId == serviceOrder.customerId && c.startTime == time && c.activeDayId == activeDay.ID);

            #region محاسبه میانگین زمان گردش در مناطقی که یک تعمیرکار در آنها کار می کند
            var avgTicks = db.Repairman_AreaTbls.Where(c => c.repairmanId == serviceOrder.repairmanId).Average(c => c.AreaTbl.time.Ticks);
            var avgTimeSpan = new TimeSpan((long)avgTicks);
            #endregion

            //درصورتی که کاربر کار را زودتر از مدت زمان تعیین شده انجام داد و
            //همچنین مدت زمان باقی مانده تا پایان سرویس که در سیستم ثبت شده بیشتر باشد از زمان لازم برای رفت و آمد کاربر تا سرویس بعدی
            if (dt.TimeOfDay < timeSheet.endTime && avgTimeSpan < (timeSheet.endTime - dt.TimeOfDay))
            {
                var endTime = dt.TimeOfDay + avgTimeSpan;
                timeSheet.endTime = new TimeSpan(endTime.Hours, endTime.Minutes, endTime.Seconds);
                db.SubmitChanges();
            }
            #endregion
            MBProto.Utils.SMS.SendSms(serviceOrder.customerMobile, "کاربوی شروع به انجام سرویس شما کرده است");
            ClassCollection.PushManagement.SendToCustomer(serviceOrder.customerId, "کاربوی شروع به انجام سرویس شما کرده است", "شروع به کار");
            return js.Serialize("");
        }

        /// <summary>
        ///پیاده سازی متد متد اعلام دریافت کالاهای یک سرویس
        /// </summary>
        public string carboyServiceProductDelivered(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(c => c.code == serviceCode && c.distributeDone == false);

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            serviceOrder.distributeDone = true;
            db.SubmitChanges();

            return js.Serialize("");
        }

        /// <summary>
        /// این متد وظیفه ارسال لیست شرویس های یک کاربوی برای تاریخ مورد نظر را باز میگرداند
        /// </summary>
        public string getCarboyServiceList(long userID, long date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();
            var userDate = ClassCollection.Method.FromUnixTime(date);


            if (userDate.Date < dt.Date)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.DateTimeInvalid());
            }

            var nextTwoDay = userDate.AddDays(2);

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID && c.status == (byte)ClassCollection.DeleteStatus.Active);
            if (user == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.UserNotRegistered());
            }

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrders = user.ServiceOrderTbls1.Where(c =>
            c.requestTime.Date >= userDate.Date && c.requestTime.Date <= nextTwoDay.Date && (
            c.state != (byte)ClassCollection.ServiceOrderState.CanceledByCustomer
            && c.state != (byte)ClassCollection.ServiceOrderState.CanceledByOperator)
            && c.state != (byte)ClassCollection.ServiceOrderState.Done).OrderBy(c => c.requestTime);

            List<object> result = new List<object>();

            foreach (var srv in serviceOrders)
            {
                dynamic t = new
                {

                    serviceImage = string.IsNullOrEmpty(srv.PackageTbl.image) ? "" : ClassCollection.Method.Url + srv.PackageTbl.image,
                    serviceCode = srv.code,
                    serviceName = srv.PackageTbl.name,
                    customerFullName = srv.customerName + " " + srv.customerFamily,
                    customerPhone = srv.customerMobile,
                    dateTime = ClassCollection.Method.persianFormatDate_d(srv.requestTime) + " - " + ClassCollection.Method.persianFormatDate_H(srv.requestTime),
                    customerCar = srv.CarTbl.CarTypeTbl.CarBrandTbl.name + " - " + srv.CarTbl.CarTypeTbl.name,
                    isInProgress = (srv.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer) || (srv.state == (byte)ClassCollection.ServiceOrderState.Proccessing)
                };

                result.Add(t);
            }

            return js.Serialize(new { serviceList = result });
        }


        /// <summary>
        /// این متد وظیفه ارسال لیست شرویس های انجام شده ی یک کاربوی را برمیگرداند
        /// </summary>
        public string getCarboyHistoryServiceList(long userID, int pageIndex, int count)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();

            if (pageIndex < 1)
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PageIndexInvalid());

            if (count < 0)
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PageCountInvalid());

            var skipCount = count * (pageIndex - 1);

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID && c.status == (byte)ClassCollection.DeleteStatus.Active);
            if (user == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.UserNotRegistered());
            }

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrders = user.ServiceOrderTbls1.Where(c => c.state == (byte)ClassCollection.ServiceOrderState.Done).OrderBy(c => c.requestTime).AsQueryable();

            var result = new
            {
                totalCount = serviceOrders.Count(),
                pageIndex = pageIndex,
                serviceList = new List<object>()
            };

            serviceOrders = serviceOrders.Skip(skipCount).Take(count == 0 ? result.totalCount : count);

            foreach (var srv in serviceOrders)
            {
                dynamic t = new
                {

                    serviceImage = string.IsNullOrEmpty(srv.PackageTbl.image) ? "" : ClassCollection.Method.Url + srv.PackageTbl.image,
                    serviceCode = srv.code,
                    serviceName = srv.PackageTbl.name,
                    customerFullName = srv.customerName + " " + srv.customerFamily,
                    customerPhone = srv.customerMobile,
                    dateTime = ClassCollection.Method.persianFormatDate_d(srv.requestTime) + " - " + ClassCollection.Method.persianFormatDate_H(srv.requestTime),
                    customerCar = srv.CarTbl.CarTypeTbl.CarBrandTbl.name + " - " + srv.CarTbl.CarTypeTbl.name,
                    isInProgress = (srv.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer) || (srv.state == (byte)ClassCollection.ServiceOrderState.Proccessing)
                };

                result.serviceList.Add(t);
            }

            return js.Serialize(result);
        }

        /// <summary>
        ///نمایش جزییات یک سرویس برای کاربوی
        /// </summary>
        public string getCarboyServiceDetail(long userID, string serviceCode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(
                c => c.code == serviceCode
                //&& c.state != (byte)ClassCollection.ServiceOrderState.Done
            );

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            if (serviceOrder.state == (byte)ClassCollection.ServiceOrderState.CanceledByCustomer)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceCancelByCustomer());
            }
            if (serviceOrder.state == (byte)ClassCollection.ServiceOrderState.CanceledByOperator)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceCancelByOperator());
            }

            dynamic result = new ExpandoObject();

            if (!File.Exists(HttpContext.Current.Server.MapPath(ClassCollection.Method.UploadPath + "\\" + serviceOrder.CarTbl.plateNumber + ".png")))
            {
                var bitmap = ClassCollection.Method.generatePlate(serviceOrder.CarTbl.plateNumber);
                bitmap.Save(HttpContext.Current.Server.MapPath(ClassCollection.Method.UploadPath + "\\" + serviceOrder.CarTbl.plateNumber + ".png"));
            }

            List<object> serviceDetail = new List<object>();
            foreach (var srv in serviceOrder.ServiceOrderServiceDetailTbls.Where(c => c.status != (byte)ClassCollection.ServiceOrderServiceStatus.Deleted))
            {
                serviceDetail.Add(
                    new
                    {
                        name = srv.Service_CarTypeTbl.ServiceTbl.name,
                        isExtra = srv.status == (byte)ClassCollection.ServiceOrderServiceStatus.New

                    });
            }

            List<object> productDetail = new List<object>();
            foreach (var prod in serviceOrder.ServiceOrderProductDetailTbls.Where(c => c.status != (byte)ClassCollection.ServiceOrderProductStatus.Deleted))
            {
                productDetail.Add(
                    new
                    {
                        name = prod.productName,
                        quantity = (int)prod.quantity,
                        isExtra = prod.status == (byte)ClassCollection.ServiceOrderProductStatus.New,
                        code = prod.productCode,
                        unit = prod.productUnitName
                    });
            }


            result = new
            {
                customer = new
                {
                    fullName = serviceOrder.customerName + " " + serviceOrder.customerFamily,
                    phone = serviceOrder.customerMobile,
                    address = new
                    {
                        description = serviceOrder.addressTitle,
                        houseNumber = serviceOrder.houseNumber,
                        houseUnit = !serviceOrder.houseUnit.HasValue ? 0 : serviceOrder.houseUnit.Value,
                        lat = serviceOrder.latitude,
                        lng = serviceOrder.longitude
                    }
                },
                car = new
                {
                    brandName = serviceOrder.CarTbl.CarTypeTbl.CarBrandTbl.name,
                    carTypeName = serviceOrder.CarTbl.CarTypeTbl.name,
                    plateNumber = ClassCollection.Method.UploadUrl + "\\" + serviceOrder.CarTbl.plateNumber + ".png",
                    kilometer = serviceOrder.CarTbl.kilometer,
                    year = serviceOrder.CarTbl.year,
                    color = serviceOrder.CarTbl.ColorTbl.name,
                    ev = serviceOrder.CarTbl.EngineVolumeTbl.name
                },
                general = new
                {
                    dateTime = ClassCollection.Method.persianFormatDate_d(serviceOrder.requestTime) + " - "
                    + ClassCollection.Method.persianFormatDate_H(serviceOrder.requestTime),
                    payment = serviceOrder.payStatus != (byte)ClassCollection.ServiceOrderPaymentStatus.No_Pay
                },
                service = new
                {
                    name = serviceOrder.PackageTbl.name,
                    image = string.IsNullOrEmpty(serviceOrder.PackageTbl.image) ? "" : ClassCollection.Method.Url + serviceOrder.PackageTbl.image,
                    code = serviceOrder.code,
                    price = (int)(serviceOrder.finalPrice
                    - (serviceOrder.payStatus == (byte)ClassCollection.ServiceOrderPaymentStatus.Pay_Online ? serviceOrder.onlinePayOrderDiscountValue : 0)
                    + serviceOrder.exraTotalPrice),
                    state = (byte)serviceOrder.state,
                    serviceDetail = serviceDetail,
                    productDetail = productDetail
                }
            };

            return js.Serialize(result);
        }

        /// <summary>
        /// این متد وظیفه ویرایش پروفایل کاربوی را برعهده دارد
        /// </summary>
        /// <returns></returns>
        public string editCarboyProfile(long userID, string oldPassword, string newPassword)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();


            var db = new DataAccessDataContext();
            var now = new DateTime();
            now = DateTime.Now;

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID);


            if (newPassword != "")
            {
                oldPassword = ClassCollection.Method.convertPersianNumberToEnglishNumber(oldPassword);
                newPassword = ClassCollection.Method.convertPersianNumberToEnglishNumber(newPassword);

                if (newPassword.Length < 5 || newPassword.Length >= 250)
                {
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PasswordInvalid());
                }

                if (user.password != ClassCollection.Method.md5(oldPassword))
                {
                    throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PasswordInvalid());
                }

                user.password = ClassCollection.Method.md5(newPassword);

            }

            db.SubmitChanges();

            return js.Serialize("");
        }

        /// <summary>
        /// این متد وظیفه ویرایش کلیومتر خودرو توسط کاربوی را برعهده دارد
        /// </summary>
        /// <returns></returns>
        public string editCustomerCar(long userID, string serviceCode, int kilometer)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();


            if (kilometer <= 0)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.KilometerInvalid());
            }


#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var now = new DateTime();
            now = DateTime.Now;

            var user = db.UserTbls.Single(c => c.ID == userID);

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrder = user.ServiceOrderTbls1.SingleOrDefault(
                c => c.code == serviceCode
            );

            if (serviceOrder == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.ServiceInvalid());
            }

            var car = serviceOrder.CarTbl;

            car.kilometer = kilometer;
            db.SubmitChanges();

            return js.Serialize("");
        }


        /// <summary>
        /// این متد وظیفه ارسال لیست شرویس های یک کاربوی برای تاریخ مورد نظر را باز میگرداند
        /// </summary>
        public string getCarboyServiceLocation(long userID, long date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var dt = new DateTime();
            dt = DateTime.Now;

            var db = new DataAccessDataContext();
            var userDate = ClassCollection.Method.FromUnixTime(date);

            if (userDate.Date < dt.Date)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.DateTimeInvalid());
            }

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID && c.status == (byte)ClassCollection.DeleteStatus.Active);
            if (user == null)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.UserNotRegistered());
            }

            if (!user.isRepairman)
            {
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.AccessDenied());
            }

            var serviceOrders = user.ServiceOrderTbls1.Where(c =>
            c.requestTime.Date == userDate.Date && (
            c.state != (byte)ClassCollection.ServiceOrderState.CanceledByCustomer
            && c.state != (byte)ClassCollection.ServiceOrderState.CanceledByOperator)
            );

            List<object> result = new List<object>();

            foreach (var srv in serviceOrders)
            {

                dynamic t = new
                {
                    lat = srv.latitude,
                    lng = srv.longitude,
                    state = (srv.state == (byte)ClassCollection.ServiceOrderState.MoveToCustomer || srv.state == (byte)ClassCollection.ServiceOrderState.Proccessing)
                    ? (byte)ClassCollection.ServiceOrderState.MoveToCustomer
                    : (srv.state == (byte)ClassCollection.ServiceOrderState.Done ? (byte)ClassCollection.ServiceOrderState.Proccessing : (byte)0)
                };
                result.Add(t);
            }

            return js.Serialize(new { locationList = result });
        }

        /// <summary>
        /// این متد وظیفه ارسال تعداد پیام های خوانده نشده کاربوی را برعهده دارد
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string getUserUnreadMessageCount(long userID)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            var result = new Models.Global.LongValue();

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var now = new DateTime();
            now = DateTime.Now;

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID);

            result.value = user.UserMessageTbls.Count(c => c.isRead == false);

            string ret = js.Serialize(result);

            return ret;
        }

        /// <summary>
        /// این متد وظیفه ارسال لیست پیام های یک کاربوی را  برعهده دارد
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string getUserMessageList(long userID, string filter, int pageIndex, int count)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (pageIndex < 1)
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PageIndexInvalid());

            if (count < 0)
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.PageCountInvalid());

            filter = filter.TrimEnd().TrimStart();

            var result = new Models.UserMessage.MessageList();
            result.message = new List<Models.UserMessage.Message>();

            var skipCount = count * (pageIndex - 1);

#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var now = new DateTime();
            now = DateTime.Now;

            var user = db.UserTbls.SingleOrDefault(c => c.ID == userID);

            var message = user.UserMessageTbls.AsQueryable();
            if (filter != "")
            {
                message = message.Where(c => c.text.Contains(filter));
            }

            message = message.OrderByDescending(c => c.regDate).AsQueryable();

            result.totalCount = message.Count();
            result.pageIndex = pageIndex;

            message = message.Skip(skipCount).Take(count == 0 ? result.totalCount : count);


            foreach (var item in message)
            {
                var t = new Models.UserMessage.Message();
                t.ID = item.ID;
                t.isRead = item.isRead;
                t.regDate = ClassCollection.Method.persianFormatDate_n(item.regDate);

                t.text = item.text;

                t.type = item.type;

                result.message.Add(t);

                item.isRead = true;
            }

            db.SubmitChanges();


            string ret = js.Serialize(result);

            return ret;
        }

        /// <summary>
        /// این متد وظیفه ثبت توکن کاربر را برعهده دارد
        /// </summary>
        /// <returns></returns>
        public string registerUserPush(long userId, string token, string platform)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
#if DEBUG
            var db = new DataAccessDataContext("Data Source=.;Initial Catalog=.;Persist Security Info=True;User ID=.;Password=.");
#else
            var db = new DataAccessDataContext();
#endif
            var now = new DateTime();
            now = DateTime.Now;

            if (token == "")
                throw new MBProto.Exceptions.AuthException(new MBProto.Exceptions.AuthException.TokenInvalid());

            var user = db.UserTbls.Single(c => c.ID == userId);
            var userToken = db.userPushTbls.SingleOrDefault(p => p.userId == userId);

            if (userToken == null)
            {
                userToken = new userPushTbl();
                userToken.userId = userId;
                userToken.lastUpdate = now;
                userToken.openedCount = 1;
                userToken.regDate = now;
                userToken.token = token;
                userToken.platform = platform;
                db.userPushTbls.InsertOnSubmit(userToken);
                db.SubmitChanges();
            }
            else
            {
                userToken.lastUpdate = now;
                userToken.openedCount += 1;
                userToken.token = token;
                db.SubmitChanges();
            }

            return js.Serialize("");

        }

    }
}