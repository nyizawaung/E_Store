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
using EStore.Model;

namespace EStore.BuinessLayer.EStore
{
    public class EStoreBusinessLayer : IEStoreBusinessLayer
    {
        private ESDBContext dbContext;
        public EStoreBusinessLayer()
        {
            dbContext = new ESDBContext();
        }
        public async Task<BuyEVoucherRespModel> BuyEVoucher(BuyEVoucherRequestModel obj)
        { 
            var respModel = new BuyEVoucherRespModel();
            bool allow = false;
            var buyStatus = Validater.CheckValidateToBuy(obj.PhoneNumber, obj.VoucherID, obj.Quantity, out allow);
            if (!allow)
            {
                respModel.status = "Fail";
                respModel.reason = buyStatus;
                return respModel;
            }
            var voucherInfo = dbContext.Vouchers.Where(a => a.ID == obj.VoucherID).FirstOrDefault();
            for (int i = 0; i < obj.Quantity; i++)
            {
                var customer_voucher = new customer_voucher()
                {
                    UserName = obj.UserName,
                    PhoneNumber = obj.PhoneNumber,
                    VoucherID = obj.VoucherID,
                    CreatedDate = DateTime.Now,
                    IsUsed = 0,
                    PromoCode = GeneratePromoCode()
                };
                customer_voucher.QRCode = GenerateQRCode(customer_voucher.PromoCode);
                dbContext.Add<customer_voucher>(customer_voucher);
                dbContext.SaveChanges();

                #region check validate again while buying
                buyStatus = Validater.CheckValidateToBuy(obj.PhoneNumber, obj.VoucherID, obj.Quantity, out allow);
                if (!allow)
                {
                    respModel.status = "Fail";
                    respModel.reason = $"Sorry we can only managed to buy {i+1} vouchers(s) for {obj.UserName} with Phone Number {obj.PhoneNumber}";
                    respModel.TotalPrice = (i + 1) * voucherInfo.Price;
                    return respModel;
                }
                #endregion
            }
            respModel.status = "Success";
            respModel.reason = "Success";
            respModel.TotalPrice = (obj.Quantity) * voucherInfo.Price;
            return respModel;
        }
        public async Task<GetVoucherRespModel> GetVoucherList(GetVoucherRequestModel obj)
        {
            var respModel = new GetVoucherRespModel();
            Expression<Func<customer_voucher, bool>> voucherTypeFilter = x => true;
            if (obj.VoucherType=="UnUsed")
            {
                voucherTypeFilter = x => x.IsUsed == 1;
            }
            else if(obj.VoucherType=="UnUsed")
            {
                voucherTypeFilter = x => x.IsUsed == 0;
            }
            //var voucherList = dbContext.Customer_Vouchers.Where(voucherTypeFilter).ToList();
            var voucherList = (from r in dbContext.customer_vouchers.Where(voucherTypeFilter)
                               join t in dbContext.Vouchers on r.VoucherID equals t.ID
                               select new VoucherInfoWithExpireDate
                               {
                                   PromoCode = r.PromoCode,
                                   QRCode = r.QRCode,
                                   ExpiryDate = t.Expiry_Date
                               }

                                ).OrderBy(a => a.ExpiryDate).ToList();
            respModel.totalCount = voucherList.Count();
            for(int i = 0; i< respModel.totalCount; i++)
            {
                var voucher = new VoucherInfoRespModel()
                {
                    promoCode = voucherList[i].PromoCode,
                    QRCode = voucherList[i].QRCode,
                    ExpiryDate = voucherList[i].ExpiryDate
                };
                respModel.voucherList.Add(voucher);
            }

            return respModel;
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
    }
}

