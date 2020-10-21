using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace htmlPdf1.Controllers
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
            string html = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"demo.html"));
            string csspath = @"D:\Project\Experiment\ITextSharp\htmlPdf1\htmlPdf1\style1.css";
            // byte[] buffer = ConvertHtmlToPdf(html, csspath);
            // byte[] buffer = htmlTOPdf(html);
            byte[] buffer = GenerateHTML(html);

            return File(buffer, "application/pdf", "myFirstPDF.pdf");
        }

        //private byte[] parseHtml(String html)
        //{
        //    ByteArrayOutputStream baos = new ByteArrayOutputStream();
        //    // step 1
        //    Document document = new Document();
        //    // step 2
        //    PdfWriter writer = PdfWriter.getInstance(document, baos);
        //    // step 3
        //    document.open();
        //    // step 4
        //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document,
        //            new FileInputStream(html));
        //    // step 5
        //    document.close();
        //    // return the bytes of the PDF
        //    return baos.toByteArray();
        //}
        private byte[] ConvertHtmlToPdf(string html, string cssPath)
        {
            Document document = new Document(PageSize.A1);

            byte[] pdfBytes;
            using (var ms = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.CloseStream = false;
                document.Open();
                HtmlPipelineContext htmlPipelineContext = new HtmlPipelineContext(null);
                htmlPipelineContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssResolver.AddCssFile(cssPath, true);
                IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlPipelineContext, new PdfWriterPipeline(document, writer)));
                XMLWorker xmlWorker = new XMLWorker(pipeline, true);
                XMLParser xmlParser = new XMLParser(xmlWorker);

                xmlParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(html)));
                // document.NewPage();
                document.Close();
                pdfBytes = ms.GetBuffer();
            }

            return pdfBytes;
        }

        [Obsolete]
        private byte[] htmlTOPdf(string html)
        {
            byte[] pdfBytes;
            // string html = "<div style='background-color:yellow'>Demo !!</div>";
            MemoryStream msOutput = new MemoryStream();
            TextReader reader = new StringReader(html);
            // step 1: creation of a document-object
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);

            // step 2:
            // we create a writer that listens to the document
            // and directs a XML-stream to a file
            PdfWriter writer = PdfWriter.GetInstance(document, msOutput);

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(document);

            // step 4: we open document and start the worker on the document
            document.Open();
            worker.StartDocument();

            // step 5: parse the html into the document
            worker.Parse(reader);

            // step 6: close the document and the worker
            worker.EndDocument();
            worker.Close();
            document.Close();
            pdfBytes = msOutput.ToArray();
            return pdfBytes;
        }

        private byte[] GenerateHTML(string html)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string newHtml = "<div style='border:1px solid red'><input type='checkbox' id='id1' name='nm1' />Corrected</div>";
                newHtml = newHtml + "<input type='text' id='id2' name='nm2' value='ABCDEF' />";
                newHtml = newHtml + "<div style='background:yellow;border:2px solid Black;' >ddsfsdfds fdsfdsfdsfdsf </div>";
                StringReader sr = new StringReader(newHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return stream.ToArray();
            }
        }

        private byte[] WriteHTML()
        {

            return null;
        }

    }
}
