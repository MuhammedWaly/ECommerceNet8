

using ECommerceNet8.DTOs.OrderDtos.Request;
using ECommerceNet8.DTOs.OrderDtos.Response;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Models.OrderModels;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace ECommerceNet8.Core.Reposiatories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        //private readonly ISendGridClient _sendGridClient;

        public OrderRepository(ApplicationDbContext db,

            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
,
            UserManager<ApplicationUser> userManager
            //ISendGridClient sendGridClient)
            )
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _userManager = userManager;
        }



        public async Task<Order> GetOrderForPdf(int OrderId)
        {
            var order = await _db.Orders
                .Include(oo => oo.PdfInfo)
                .FirstOrDefaultAsync(o => o.Id == OrderId);

            return order;
        }





        #region  HelperFunctions

        //private async Task<string> GenerateId()
        //{
        //    char[] Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        //    Random random = new Random();

        //    string randomLetters = "";
        //    for(int i = 0; i<3; i++)
        //    {
        //        randomLetters += Letters[random.Next(Letters.Length)];
        //    }
        //    int randomNumber = random.Next(100000000, 999999999);

        //    string UniqueIdentifier = randomLetters +  randomNumber.ToString();

        //    var existingIdentifier  = await _db.Orders
        //        .FirstOrDefaultAsync(o=>o.Id == UniqueIdentifier);
        //    if(existingIdentifier != null)
        //    {
        //        GenerateId();
        //    }

        //    return UniqueIdentifier;
        //}

        //private async Task<bool> HasEnoughItems(int productVariantId, int quantity)
        //{
        //    var existingProductVariant = await _db.productVariants
        //        .FirstOrDefaultAsync(pv=>pv.Id == productVariantId);
        //    if(existingProductVariant.Quantity < quantity)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private async Task<bool> RemoveItemQuantity(int productVariantId, int quantity)
        //{
        //    var existingProductVariant = await _db.productVariants
        //        .FirstOrDefaultAsync(pv=>pv.Id == productVariantId);
        //    existingProductVariant.Quantity = existingProductVariant.Quantity - quantity;
        //    await _db.SaveChangesAsync();

        //    return true;
        //}

        #endregion


        public async Task<string> CreatePdf(Order existingOrder, string UserId)
        {
            

            var date = DateTime.Now.ToShortDateString().Replace("/", "_");
            string fileName = "PDF_" + date + "_" + DateTime.UtcNow.Millisecond + ".pdf";
            string folderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "PDF", existingOrder.Id.ToString(), existingOrder.CustomerEmail);
            string path = System.IO.Path.Combine(folderPath, fileName);

            decimal TotalDiscountOfAllItems = 0;
            decimal TotalPriceOfAllItems = 0;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }




            #region New


            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(path));
            iText.Layout.Document doc = new iText.Layout.Document(pdfDoc, PageSize.A4, true);
            doc.SetMargins(25, 25, 25, 25);

            var font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);
            var aligmentLeft = iText.Layout.Properties.TextAlignment.LEFT;
            var aligmentCenter = iText.Layout.Properties.TextAlignment.CENTER;
            string imagePath = _webHostEnvironment.WebRootPath + "\\ImageForPDF\\Logo.png";
            iText.Layout.Element.Image image = new iText.Layout.Element.Image
                (ImageDataFactory.Create(imagePath));
            image.SetFixedPosition(400, 700);
            image.ScaleToFit(90, 90);
            doc.Add(image);

            //customer info
            var Address = await _db.Addresses.FirstOrDefaultAsync(ad => ad.ApplicationUserId == UserId);
            if (Address == null) return "Add your address first";

            Paragraph name = new Paragraph(Address.ApplicationUser.FirstName + " " + Address.ApplicationUser.LastName)
                .SetFont(font)
                .SetFontSize(12)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(name);

            Paragraph email = new Paragraph(Address.ApplicationUser.Email)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(email);

            Paragraph street = new Paragraph(Address.Street)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(street);

            if (Address.AppartmentNumber == null || Address.AppartmentNumber == 0)
            {
                Paragraph addressHouseNum = new Paragraph(Address.HouseNumber.ToString())
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
                doc.Add(addressHouseNum);
            }
            else
            {
                Paragraph addressHouseApp = new Paragraph(Address.HouseNumber.ToString() + "-" + Address.AppartmentNumber.ToString())
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
                doc.Add(addressHouseApp);
            }

            Paragraph addressCity = new Paragraph(Address.City)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(addressCity);

            Paragraph addressRegion = new Paragraph(Address.Region)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(addressRegion);

            Paragraph addressCountry = new Paragraph(Address.Country)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(addressCountry);

            Paragraph postalCode = new Paragraph( Address.PostalCode)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(aligmentLeft)
                .SetMarginBottom(0);
            doc.Add(postalCode);

            Paragraph busineesName = new Paragraph("Online Shopping")
               .SetFont(font)
               .SetFontSize(12)
               .SetTextAlignment(aligmentCenter)
               .SetPaddingTop(20);
            doc.Add(busineesName);

            Paragraph orderId = new Paragraph("Invoice for order: " + existingOrder.Id)
               .SetFont(font)
               .SetFontSize(12)
               .SetTextAlignment(aligmentCenter)
               .SetPaddingTop(5);
            doc.Add(orderId);


            //order items table

            Table ItemTable = new Table(7);
            ItemTable.SetMarginTop(20);
            ItemTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            ItemTable.SetWidth(500);


            //iText.Kernel.Colors
            Color textColorTableHeadings = new DeviceRgb(255, 255, 255);
            Color bgColorTableHeadings = new DeviceRgb(1, 1, 1);

            Cell cell1 = new Cell().Add(new Paragraph("PRODUCT NAME")
                .SetFontColor(textColorTableHeadings)
                .SetFontSize(8)
                .SetTextAlignment(aligmentCenter));
            cell1.SetBackgroundColor(bgColorTableHeadings);
            cell1.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell1);

            Cell cell2 = new Cell().Add(new Paragraph("PRODUCT COLOR")
                .SetFontColor(textColorTableHeadings)
                .SetFontSize(8)
                .SetTextAlignment(aligmentCenter));
            cell2.SetBackgroundColor(bgColorTableHeadings);
            cell2.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell2);

            Cell cell3 = new Cell().Add(new Paragraph("PRODUCT SIZE")
                .SetFontColor(textColorTableHeadings)
                .SetFontSize(8)
                .SetTextAlignment(aligmentCenter));
            cell3.SetBackgroundColor(bgColorTableHeadings);
            cell3.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell3);

            Cell cell4 = new Cell().Add(new Paragraph("QUANTITY")
               .SetFontColor(textColorTableHeadings)
               .SetFontSize(8)
               .SetTextAlignment(aligmentCenter));
            cell4.SetBackgroundColor(bgColorTableHeadings);
            cell4.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell4);

            Cell cell5 = new Cell().Add(new Paragraph("PRICE")
               .SetFontColor(textColorTableHeadings)
               .SetFontSize(8)
               .SetTextAlignment(aligmentCenter));
            cell5.SetBackgroundColor(bgColorTableHeadings);
            cell5.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell5);

            Cell cell6 = new Cell().Add(new Paragraph("DISCOUNT IN %")
               .SetFontColor(textColorTableHeadings)
               .SetFontSize(8)
               .SetTextAlignment(aligmentCenter));
            cell6.SetBackgroundColor(bgColorTableHeadings);
            cell6.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell6);

            Cell cell7 = new Cell().Add(new Paragraph("TOTAL PRICE")
                .SetFontColor(textColorTableHeadings)
                .SetFontSize(8)
                .SetTextAlignment(aligmentCenter));
            cell7.SetBackgroundColor(bgColorTableHeadings);
            cell7.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            ItemTable.AddCell(cell7);

            

            foreach (var item in existingOrder.OrderItems)
            {

                var productvariant = await _db.productVariants.FindAsync(item.ProductVariantId);
                var baseproduct = await _db.BaseProducts.FindAsync(productvariant.BaseProductId);
                var productcolor = await _db.productColors.FindAsync(productvariant.ProductColorId);
                var productsize = await _db.productSizes.FindAsync(productvariant.ProductSizeId);


                Cell CellProductName = new Cell().Add(new Paragraph(baseproduct.Name)
                    .SetTextAlignment(aligmentCenter)
                    .SetFontSize(8));
                CellProductName.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductName);

                Cell CellProductColor = new Cell().Add(new Paragraph(productcolor.Name)
                    .SetTextAlignment(aligmentCenter)
                    .SetFontSize(8));
                CellProductColor.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductColor);

                Cell CellProductSize = new Cell().Add(new Paragraph(productsize.Name)
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
                CellProductSize.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductSize);

                Cell CellProductQuantity = new Cell().Add(new Paragraph(item.Quantity.ToString())
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
                CellProductQuantity.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductQuantity);

                Cell CellProductPrice = new Cell().Add(new Paragraph(item.UnitPrice.ToString())
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
                CellProductPrice.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductPrice);

                Cell CellProductDiscount = new Cell().Add(new Paragraph(baseproduct.Discount.ToString())
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
                CellProductDiscount.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProductDiscount);


                Cell CellProdutTotalPrice = new Cell().Add(new Paragraph(item.TotalPrice.ToString())
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
                CellProductDiscount.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
                ItemTable.AddCell(CellProdutTotalPrice);


                var totalDiscount = ((item.UnitPrice * item.Quantity) * baseproduct.Discount / 100);
                var totalDiscountDecimal = decimal.Round(totalDiscount, 2);
                TotalDiscountOfAllItems += totalDiscountDecimal;
                TotalPriceOfAllItems += item.TotalPrice;

            }

            doc.Add(ItemTable);



            Table PriceTable = new Table(2);
            PriceTable.SetMarginTop(20);
            PriceTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            PriceTable.SetWidth(200);

            //Cell cellDiscountName = new Cell().Add(new Paragraph("DISCOUNT")
            //    .SetFontColor(textColorTableHeadings)
            //    .SetFontSize(8)
            //    .SetTextAlignment(aligmentCenter));
            //cellDiscountName.SetBackgroundColor(bgColorTableHeadings);
            //cellDiscountName.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            //PriceTable.AddCell(cellDiscountName);

            //Cell CellDiscountValue = new Cell().Add(new Paragraph("$" + TotalDiscountOfAllItems.ToString())
            //       .SetTextAlignment(aligmentCenter)
            //       .SetFontSize(8));
            //CellDiscountValue.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            //PriceTable.AddCell(CellDiscountValue);

            //Cell cellPriceName = new Cell().Add(new Paragraph("PRICE")
            //   .SetFontColor(textColorTableHeadings)
            //   .SetFontSize(8)
            //   .SetTextAlignment(aligmentCenter));
            //cellPriceName.SetBackgroundColor(bgColorTableHeadings);
            //cellPriceName.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            //PriceTable.AddCell(cellPriceName);

            //Cell CellPriceValue = new Cell().Add(new Paragraph("$" + TotalPriceOfAllItems.ToString())
            //       .SetTextAlignment(aligmentCenter)
            //       .SetFontSize(8));
            //CellDiscountValue.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            //PriceTable.AddCell(CellPriceValue);



            var totalPriceInCurrency = TotalPriceOfAllItems;

            Cell cellTotalPriceName = new Cell().Add(new Paragraph("TOTAL PRICE")
              .SetFontColor(textColorTableHeadings)
              .SetFontSize(8)
              .SetTextAlignment(aligmentCenter));
            cellTotalPriceName.SetBackgroundColor(bgColorTableHeadings);
            cellTotalPriceName.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            PriceTable.AddCell(cellTotalPriceName);

            Cell CellTotalPriceValue = new Cell().Add(new Paragraph("$" + totalPriceInCurrency)
                   .SetTextAlignment(aligmentCenter)
                   .SetFontSize(8));
            CellTotalPriceValue.SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            PriceTable.AddCell(CellTotalPriceValue);


            doc.Add(PriceTable);

            doc.Close();
            return path;

            #endregion


        }

    }
}
