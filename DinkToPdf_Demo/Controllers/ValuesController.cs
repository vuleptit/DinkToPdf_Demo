using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DinkToPdf_Demo.Controllers
{
    [Route("api/htmlConverter/toPdf")]
    [ApiController]
    public class PdfConverter : ControllerBase
    {
        private IConverter _converter;

        public PdfConverter(IConverter converter)
        {
            this._converter = converter;
        }

        // GET api/values
        [HttpGet]
        [Route("pdfConverter")]
        public async Task<string> CreatePdfAsync()
        {
            try
            {
                /// Cài đặt hiển thị cho pdf khi được generate
                var globalSetting = new GlobalSettings
                {
                    ColorMode = ColorMode.Color, //Tạo file màu hoặc đen trắng: Color: màu giống như html, Gray: đen trắng
                    Orientation = Orientation.Portrait, // Chiều đặt của giấy: Portrait: dọc, Landscape: ngang
                    PaperSize = PaperKind.A4, //Kiểu giấy
                    Margins = new MarginSettings { Top = 1, Left = 1, Unit = Unit.Millimeters}, // Căn lề
                    DocumentTitle = @"Pdf minh họa", // Title file
                    Out = @"D:\PDFCreator\Employee_Report.pdf" // Out ra file - cần tạo ra file chứa trước
                };


                /// Lấy dữ liệu - html của website cần chuyển sang pdf
                var ObjectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = await getWebsiteContent(@"https://www.youtube.com/watch?v=tt-blq_YuwE&list=RDyJytt8T2naw&index=15")
                };

                var doc = new HtmlToPdfDocument
                {
                    GlobalSettings = globalSetting,
                    Objects = { ObjectSettings }
                };

                this._converter.Convert(doc);

                return "Tạo pdf thành công";
            }
            catch (Exception ex)
            {
                return "Có lỗi xảy ra: " + ex;
            }
        }

        /// <summary>
        /// Function for crawling data from an uri and Return a String
        /// </summary>
        /// <param name="url">Url of the website needing crawl</param>
        /// <returns>string: the html source of the website</returns>
        public async Task<string> getWebsiteContent(string url)
        {
            using(var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using(var client = new HttpClient(httpClientHandler))
                {
                    string webContentString = await client.GetStringAsync(url);
                    return webContentString;
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
