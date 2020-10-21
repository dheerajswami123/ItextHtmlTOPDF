using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.StyledXmlParser.Css.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HTMLPDF2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            string html = System.IO.File.ReadAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"abc.html"));
            string csspath = @"D:\Project\Experiment\ITextSharp\htmlPdf1\htmlPdf1\style1.css";
            // byte[] buffer = ConvertHtmlToPdf(html, csspath);
            // byte[] buffer = htmlTOPdf(html);
          byte[] buffer =  GenerateHTML(html);

            return File(buffer, "application/pdf", "myFirstPDF.pdf");


        }
        public byte[] GenerateHTML(string html)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);//@"D:\Docs\demp.pdf"
                PdfDocument pdfDoc = new PdfDocument(writer);

                // Set the result to be tagged
                pdfDoc.SetTagged();
                pdfDoc.SetDefaultPageSize(PageSize.A4);

                ConverterProperties converterProperties = new ConverterProperties();

                // Set media device description details
                MediaDeviceDescription mediaDescription = new MediaDeviceDescription(MediaType.SCREEN);
                //mediaDescription.SetWidth(screenWidth);
                converterProperties.SetMediaDeviceDescription(mediaDescription);

                FontProvider fp = new DefaultFontProvider();

                // Register external font directory
                ////fp.AddDirectory(resourceLoc);

                ////converterProperties.SetFontProvider(fp);
                // Base URI is required to resolve the path to source files
                ////converterProperties.SetBaseUri(resourceLoc);

                // Create acroforms from text and button input fields
                converterProperties.SetCreateAcroForm(true);

                // HtmlConverter.ConvertToPdf(html, pdfDoc, converterProperties);
                HtmlConverter.ConvertToPdf(html, pdfDoc,converterProperties);
               
                pdfDoc.Close();
                return ms.ToArray();
            }
            
        }

    }
}
