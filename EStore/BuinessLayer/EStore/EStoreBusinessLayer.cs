using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;
using EStore.Helper;
using System.Linq.Expressions;
using EStore.Services;
using Microsoft.Extensions.Configuration;

namespace EStore.BuinessLayer.EStore
{
    public class EStoreBusinessLayer : IEStoreBusinessLayer
    {
        private ESDBContext dbContext;
        private ITokenService tokenService;
        private IConfiguration configuration;
        public EStoreBusinessLayer(ITokenService _tokenService, IConfiguration config, ESDBContext context)
        {
            dbContext = context;
            tokenService = _tokenService;
            configuration = config;
        }
        public async Task<LoginRespModel> Login(LoginRequestModel obj)
        {
            var respModel = new LoginRespModel();
            var result = dbContext.Admins.Where(a => a.Name == obj.UserName && a.Password == obj.Password).FirstOrDefault();
            if (result != null)
            {
                respModel.UserID = result.ID;
                respModel.SessionID = Helper.Helper.EncryptString(tokenService.BuildToken(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), result));
                respModel.RespDescription = "Success";
            }
            else
            {
                respModel.UserID = -1;
                respModel.SessionID = null;
                respModel.RespDescription = "Incorrect Username or Password";
            }
            
            return respModel;
        }
        public async Task<BuyEVoucherRespModel> BuyEVoucher(BuyEVoucherRequestModel obj)
        {
            var respModel = new BuyEVoucherRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Please try to login again!";
                return respModel;
            }
            #endregion

            bool allow = false;
            var buyStatus = CheckValidateToBuy(obj.PhoneNumber, obj.VoucherID, obj.Quantity, out allow);
            if (!allow)
            {
                respModel.status = "Fail";
                respModel.reason = buyStatus;
                return respModel;
            }
            var voucherInfo = dbContext.Vouchers.Where(a => a.ID == obj.VoucherID).FirstOrDefault();
            decimal discount = 0;
            if (voucherInfo.PaymentMethod == obj.PaymentType || voucherInfo.PaymentMethod == "all")
            {
                discount = voucherInfo.Discount.Value;
            }

            respModel.promoCodes = new List<GeneratedPromo>();
            #region Buying Ticket
            for (int i = 0; i < obj.Quantity; i++)
            {
                #region check validate again while buying
                buyStatus = CheckValidateToBuy(obj.PhoneNumber, obj.VoucherID, obj.Quantity-i, out allow);
                if (!allow)
                {
                    respModel.status = "Fail";
                    respModel.reason = $"Sorry we can only managed to buy {i + 1} vouchers(s) for {obj.UserName} with Phone Number {obj.PhoneNumber}";
                    respModel.TotalPrice = ((i + 1) * voucherInfo.Price) * (100 - discount);
                    return respModel;
                }
                #endregion
                var customer_voucher = new customer_voucher()
                {
                    UserName = obj.UserName,
                    PhoneNumber = obj.PhoneNumber,
                    VoucherID = obj.VoucherID,
                    CreatedDate = DateTime.Now,
                    IsUsed = 0,
                    PromoCode = GeneratePromoCode()
                };

                #region QRCode
                byte[] qrcode = GenerateQRCode(customer_voucher.PromoCode);
                var LongTick = DateTime.Now.Ticks;
                var imageUrl = "";
                var qrimage = Convert.ToBase64String(qrcode, 0, qrcode.Length);
                imageUrl = "Images/QRCodes/QR_" + LongTick + Helper.Helper.GetFileExtension(qrimage);
                Helper.Helper.Base64ToImage(qrimage).Save(imageUrl);
                customer_voucher.QRCode = imageUrl;
                #endregion

                #endregion
                respModel.promoCodes.Add(new GeneratedPromo() { PromoCode=customer_voucher.PromoCode,QRImage=customer_voucher.QRCode});

                dbContext.Add<customer_voucher>(customer_voucher);
                dbContext.SaveChanges();
            }
            respModel.status = "Success";
            respModel.reason = "Success";
            respModel.UserName = obj.UserName;
            respModel.PhoneNumber = obj.PhoneNumber;
            respModel.TotalPrice = ((obj.Quantity) * voucherInfo.Price) * ((100 - discount) / 100);

            
            return respModel;
        }
        public async Task<GetPromoCodeRespModel> GetPromoCodeList(GetPromoCodeRequestModel obj)
        {
            var respModel = new GetPromoCodeRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            #endregion
            Expression<Func<customer_voucher, bool>> voucherTypeFilter = x => true;
            if (obj.promoType == "UnUsed")
            {
                voucherTypeFilter = x => x.IsUsed == 1;
            }
            else if (obj.promoType == "UnUsed")
            {
                voucherTypeFilter = x => x.IsUsed == 0;
            }
            //var voucherList = dbContext.Customer_Vouchers.Where(voucherTypeFilter).ToList();
            respModel.promoCodeList = (from r in dbContext.Customer_Vouchers.Where(voucherTypeFilter)
                               join t in dbContext.Vouchers on r.VoucherID equals t.ID
                               select new PromoCodeInfoRespModel
                               {
                                   promoCode = r.PromoCode,
                                   QRCode = r.QRCode,
                                   ExpiryDate = t.Expiry_Date.Value,
                                   IsUsed = r.IsUsed.Value,
                                   PhoneNumber=r.PhoneNumber,
                                   UserName=r.UserName
                               }

                                ).OrderBy(a => a.ExpiryDate).ToList();
            respModel.totalCount = respModel.promoCodeList.Count();
            //respModel.promoCodeList = new List<PromoCodeInfoRespModel>();
            //for (int i = 0; i < respModel.totalCount; i++)
            //{
            //    var voucher = new PromoCodeInfoRespModel()
            //    {
            //        promoCode = voucherList[i].PromoCode,
            //        QRCode = voucherList[i].QRCode,
            //        ExpiryDate = voucherList[i].ExpiryDate,
            //        IsUsed=voucherList[i].IsUsed
            //    };
            //    respModel.promoCodeList.Add(voucher);
            //}
            respModel.RespDescription = "success";
            return respModel;
        }
        public async Task<VoucherListRespModel> GetVoucherList(GetVoucherRequestModel obj)
        {
            var respModel = new VoucherListRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            #endregion
            Expression<Func<voucher, bool>> voucherTypeFilter = x => true;
            if (obj.VoucherID > 0)
            {
                voucherTypeFilter = x => x.ID == obj.VoucherID;
            }
            var voucherList = dbContext.Vouchers.Where(a => a.IsActive == 1 && a.Expiry_Date >= DateTime.Now).Where(voucherTypeFilter).Select(a => new VoucherInfo()
            {
                Amount = a.Amount.Value,
                BuyType = a.Buy_Type,
                Description = a.Description,
                Discount = a.Discount.Value,
                Title = a.Title,
                ExpiryDate = a.Expiry_Date,
                VoucherID = a.ID,
                Image = a.Image
            }).ToList();

            respModel.totalCount = voucherList.Count();
            respModel.voucherList = voucherList;
            respModel.RespDescription = "success";
            
            return respModel;
        }
        public async Task<ItemListRespModel> GetItemList(ItemListRequestModel obj)
        {
            var respModel = new ItemListRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            #endregion

            respModel.items = dbContext.Items.Where(a => a.IsActive == 1).Select(a => new ItemInfo
            {
                ItemID = a.ID,
                Name = a.Name,
                Price = a.Price.Value
            }).ToList();
            respModel.status = "success";
            respModel.RespDescription = "success";
            respModel.totalCount = respModel.items.Count();
            return respModel;
        }

        public async Task<ItemBuyRespModel> BuyItem(ItemBuyRequestModel obj)
        {
            var respModel = new ItemBuyRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Fail";
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            #endregion
            var itemInfo = dbContext.Items.Where(a => a.ID == obj.ItemID).FirstOrDefault();
            if (itemInfo == null)
            {
                respModel.status = "Fail";
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }

            #region calculate totalprice and netamount
            decimal discountAmount = 0;
            #region check promocode
            if (!string.IsNullOrEmpty(obj.promoCode))
            {

                var voucherInfo = (from r in dbContext.Customer_Vouchers.Where(a => a.PromoCode == obj.promoCode && a.IsUsed == 0 && a.PhoneNumber == obj.PhoneNumber)
                                   join t in dbContext.Vouchers on r.VoucherID equals t.ID
                                   select t).FirstOrDefault();
                if (voucherInfo == null)
                {
                    respModel.status = "Fail";
                    respModel.RespDescription = "Sorry. Incorrect promo code or already used ";
                    return respModel;
                }
                else if (voucherInfo.Expiry_Date < DateTime.Now)
                {
                    respModel.status = "Fail";
                    respModel.RespDescription = "Sorry. PromoCode is expired";
                    return respModel;
                }
                
                var promo = dbContext.Customer_Vouchers.Where(a => a.PromoCode == obj.promoCode).FirstOrDefault();
                promo.IsUsed = 1;
                promo.UsedDate = DateTime.Now;
                dbContext.SaveChanges();
                discountAmount = voucherInfo.Amount.Value;

            }
            #endregion

            respModel.TotalPrice = (obj.qty * itemInfo.Price).Value;
            respModel.NetAmount = respModel.TotalPrice - discountAmount;

            #endregion

            #region 
            //do dummy card transaction here
            #endregion

            #region add item purchase log
            var itempurchasehistory = new item_purchase_history()
            {
                ItemID = obj.ItemID,
                PhoneNumber = obj.PhoneNumber,
                PromoCode = obj.promoCode,
                NetAmount = respModel.NetAmount,
                TotalPrice = respModel.TotalPrice,
                Qty = obj.qty,
                CreatedDate = DateTime.Now,
                UserName = obj.UserName
            };
            #endregion
            respModel.status = "success";
            respModel.RespDescription = "success";
            dbContext.Item_Purchase_Histories.Add(itempurchasehistory);
            dbContext.SaveChanges();
            return respModel;

        }

        public async Task<PaymentMethodRespModel> GetPaymentMethod(PaymentMethodRequestModel obj) {
            var respModel = new PaymentMethodRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Fail";
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            respModel.paymentmethods = dbContext.PaymentMethods.ToList();
            respModel.status = "success";
            respModel.RespDescription = "success";
            return respModel;
            #endregion
        }

        public async Task<ItemPurchaseHistoryRespModel> ItemPurchaseHistory(ItemPurchaseHistoryRequestModel obj)
        {
            var respModel = new ItemPurchaseHistoryRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Fail";
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            respModel.histories = dbContext.Item_Purchase_Histories.OrderByDescending(a=>a.ID).ToList();
            respModel.status = "success";
            respModel.RespDescription = "success";
            return respModel;
            #endregion
        }

        private string GeneratePromoCode(int int_length = 6, int str_length = 5)
        {
            Random random = new Random();
            string promo = "";
            const string int_chars = "0123456789";
            const string str_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int isExist = 0;
            do
            {
                promo = new string(Enumerable.Repeat(int_chars, int_length)
                .Select(s => s[random.Next(s.Length)]).ToArray()) +
                new string(Enumerable.Repeat(str_chars, str_length)
                .Select(s => s[random.Next(s.Length)]).ToArray())
                ;
                isExist = dbContext.Customer_Vouchers.Where(a => a.PromoCode == promo).Count();
            } while (isExist > 0);
            return promo;
        }

        private byte[] GenerateQRCode(string qrInfo)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qrInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            return BitmapToByteArray(QrBitmap);
        }
        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private string CheckValidateToBuy(string phoneNumber, int voucherID, int qty, out bool allow)
        {
            var voucherInfo = dbContext.Vouchers.Where(a => a.ID == voucherID).FirstOrDefault();
            var totalSoldPromo = dbContext.Customer_Vouchers.Count(a => a.VoucherID == voucherID);
            string reason = "Not allowed";
            allow = false;
            if (voucherInfo == null)
            {
                reason = "Voucher ID does not exist in the system";
                allow = false;
                return reason;
            }
            var boughtVoucherCount = dbContext.Customer_Vouchers.Where(a => a.VoucherID == voucherID  && a.PhoneNumber==phoneNumber).Count();
            if (boughtVoucherCount >= voucherInfo.MaximumVoucherPerUser)
            {
                reason = "Sorry. This user has reached the maximum voucher limit per person.";
                allow = false;
                return reason;
            }
            if (totalSoldPromo + qty > voucherInfo.Quantity)
            {
                if (totalSoldPromo == voucherInfo.Quantity)
                {
                    reason = $"Sorry. All promo code has been sold out.";
                }
                else
                {
                    reason = $"Sorry. Only {voucherInfo.Quantity - totalSoldPromo} voucher(s) available. Please adjust the total number of voucher.";
                }

                allow = false;
                return reason;
            }
            if (boughtVoucherCount + qty > voucherInfo.MaximumVoucherPerUser)
            {
                reason = $"Sorry. You can't buy the {qty} voucher(s). maximim voucher per person is {voucherInfo.MaximumVoucherPerUser} and this user already bought {boughtVoucherCount} voucher(s)." +
                            $"This user Can only buy {voucherInfo.MaximumVoucherPerUser - boughtVoucherCount} more voucher(s) for this Voucher type.";
                allow = false;
                return reason;
            }
            
            reason = "success";
            allow = true;

            return reason;
        }
    }
}

