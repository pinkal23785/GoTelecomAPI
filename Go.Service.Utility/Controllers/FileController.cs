using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Go.Service.Utility.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [HttpGet("ConvertHtmlToPdf")]
        public async Task<IActionResult> ConvertHtmlToPDF(string URI)
        {
            try
            {
                string data = "";
                using (WebClient client = new WebClient())
                    data = client.DownloadString(URI);

                string pdfDest = "output.pdf";
                MemoryStream s = new MemoryStream();
                var wr = new PdfWriter(s);
                ConverterProperties properties = new ConverterProperties();
                FontProvider fontProvider = new DefaultFontProvider(false, false, false);

                FontProgram fontProgram = FontProgramFactory.CreateFont("NotoSans-Regular.ttf");
                fontProvider.AddFont(fontProgram);

                FontProgram fontArabicProgram = FontProgramFactory.CreateFont("NotoNaskhArabic-Regular.ttf");
                fontProvider.AddFont(fontArabicProgram);


                properties.SetFontProvider(fontProvider);
                HtmlConverter.ConvertToPdf(data, wr, properties);

                return File(s.ToArray(), "application/pdf");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + "" + ex.InnerException);
            }
            //return Ok();
        }

    }

}

