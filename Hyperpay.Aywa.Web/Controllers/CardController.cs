using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Hyperpay.Aywa.Web.Data;
using Hyperpay.Aywa.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Hyperpay.Aywa.Web.Controllers
{

    public class CardController : Controller
    {

        private readonly IDataService _dataService;
        public CardController(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<IActionResult> Index()
        {
            var CardList = await _dataService.GetAywaaCardType();
            var cardModel = new CardModel();
            cardModel.CardList = new List<Itemlist>();
            foreach (var cd in CardList)
            {
                var item = new Itemlist();
                item.Text = cd.NAME;
                item.Value = cd.ID;
                cardModel.CardList.Add(item);
            }

            return View(cardModel);
        }
        public async Task<IActionResult> UploadFile(CardModel model, IFormFile file)
        {
            if (file != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (ExcelPackage package = new ExcelPackage(file.OpenReadStream()))
                {
                    
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    int rows = worksheet.Dimension.Rows; // 20
                    int columns = worksheet.Dimension.Columns; // 7

                    List<AwayaCardModel> listofCards = new List<AwayaCardModel>();  
                    for (int i = 2; i <= rows; i++)
                    {
                        var objCard = new AwayaCardModel();
                        for (int j = 1; j <= columns; j++)
                        {
                            if (j == 1)
                                objCard.CardNumber = worksheet.Cells[i, j].Value.ToString().Trim();
                            if(j==2)
                                objCard.BatchNo = worksheet.Cells[i, j].Value.ToString().Trim();
                            if (j == 3)
                                objCard.ControlNo = worksheet.Cells[i, j].Value.ToString().Trim();
                            if (j == 4)
                                objCard.Status = worksheet.Cells[i, j].Value.ToString().Trim();
                            if (j == 5)
                                objCard.ExpiryDate = worksheet.Cells[i, j].Value.ToString().Trim();
                        }
                        listofCards.Add(objCard);
                    }
                    ViewBag.TotalProcessCard = await _dataService.UploadAwayaCard(model.CardId, listofCards);

                }

                //using (var reader = new StreamReader(file.OpenReadStream()))
                //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                //{
                //    csv.Context.Configuration.Encoding = System.Text.Encoding.UTF8;
                //    csv.Read();
                //    csv.ReadHeader();
                //    // var records = csv.GetRecords<AwayaCardModel>().ToList();
                //    List<AwayaCardModel> rows = new List<AwayaCardModel>();
                //    while (csv.Read())
                //    {
                //        var r = new AwayaCardModel()
                //        {
                //            CardNumber = csv.GetField<string>("CardNumber"),
                //            BatchNo = csv.GetField<string>("BatchNo"),
                //            ControlNo = csv.GetField<string>("ControlNo"),
                //            ExpiryDate = csv.GetField<string>("ExpiryDate"),
                //            Status = csv.GetField<string>("Status"),
                //        };
                //        rows.Add(r);
                //    }
                //    ViewBag.TotalProcessCard = await _dataService.UploadAwayaCard(model.CardId, rows);
                //}
                return View();
            }
            else
            {
                return await Index();
            }

        }
    }
}
