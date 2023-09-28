using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using Valghalla.MessageReceiver.Helpers;
using Valghalla.MessageReceiver.Requests.DigitalPost;

namespace Valghalla.MessageReceiver.Controllers
{
    [ApiController]
    [Route("api/digitalpost")]

    public class DigitalPostController : ControllerBase
    {
        //private readonly ISender sender;
        private readonly ILogger logger;
        private readonly XmlSerializer xmlOutputSerializer;

        //public DigitalPostController(ISender sender, ILogger logger)
        public DigitalPostController(ILogger<DigitalPostController> logger)
        {
            this.logger = logger;
            xmlOutputSerializer = new XmlSerializer(typeof(ModtagBeskedOutput));
      
            //this.sender = sender;
        }

        [HttpPost("digitalpostmessagestatus")]
        [Consumes("application/xml")]
        public ContentResult DigitalPostMessageStatus([FromBody] ModtagBeskedInput value)
        {
            logger.Log(LogLevel.Information, "DigitalPostMessageStatus: Received message!");
            logger.Log(LogLevel.Information, $"DigitalPostMessageStatus: Request beskedId: {value.Haendelsesbesked.BeskedId.UUIDIdentifikator}");

            try
            { 
                PKO_PostStatus message = XmlHelper.DecodePostMessageBase64(value.Haendelsesbesked.Beskeddata.Base64.Value);

                switch (message.TransaktionsStatusKode.ToLower())
                {
                    case "fejlet":
                        logger.Log(LogLevel.Information, $"DigitalPostMessageStatus: There was an error when sending the message: {message.TransaktionsStatusKode}");
                        break;
                    case "afleveret digital post":
                        logger.Log(LogLevel.Information, $"DigitalPostMessageStatus: Message is recieved to the enduser: {message.TransaktionsStatusKode}");
                        break;
                        ;
                    case "modtaget digital post":
                        logger.Log(LogLevel.Information, $"DigitalPostMessageStatus: Message is recieved by Digital post: {message.TransaktionsStatusKode}");
                        break;
                    default:
                        logger.Log(LogLevel.Information, "DigitalPostMessageStatus: Could not see message status. Please check code:");
                        break;
                }
            
                ModtagBeskedOutput returnObject = new()
                {
                    StandardRetur = new StandardRetur()
                    {
                    StatusKode = 20,
                    FejlbeskedTekst = "Ok"
                    }
                };

                string svarxml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                                    <ns2:ModtagBeskedOutput xmlns=""urn:oio:sagdok:3.0.0"" xmlns:ns2=""urn:oio:sts:1.0.0"">
                                     <StandardRetur>
                                      <StatusKode>20</StatusKode>
                                      <FejlbeskedTekst>Ok</FejlbeskedTekst>
                                     </StandardRetur>
                                    </ns2:ModtagBeskedOutput>";

                //Create our own namespaces for the output
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add(string.Empty, string.Empty);
                

                var content = string.Empty;
                using (StringWriter textWriter = new())
                {
                    xmlOutputSerializer.Serialize(textWriter, returnObject, ns);
                    content = textWriter.ToString();
                }

                ContentResult contentResult = new()
                {
                    Content = svarxml,
                    ContentType = "application/xml",
                    StatusCode = 200
                };

                logger.Log(LogLevel.Information, $"DigitalPostMessageStatus: Request return answer: {contentResult.Content.ToString()}");


                return contentResult;
            }
            catch(Exception e)
            {
                logger.Log(LogLevel.Critical, $"DigitalPostMessageStatus: Something went wrong in the DigitalPostMessageStatus endpoint : {e.Message}");
                return null;
            }

        }

        [HttpGet("testcontent")]
        public ContentResult TestContent()
        {
            ModtagBeskedOutput returnObject = new()
            {
                StandardRetur = new StandardRetur()
                {
                    StatusKode = 20,
                    FejlbeskedTekst = "Ok"
                }
            };

            var content = string.Empty;
            using (StringWriter textWriter = new())
            {
                xmlOutputSerializer.Serialize(textWriter, returnObject);
                content = textWriter.ToString();
            }
             
            ContentResult contentResult = new()
            {
                Content = content,
                ContentType = "application/xml",
                StatusCode = 200
            };

            return contentResult;
        }

        [HttpGet("testdata")]
        public IActionResult TestData()
        {
            logger.Log(LogLevel.Information, "DigitalPostMessageTestData!");
            var returnList = new List<string>()
            {
                "Item1",
                "Item2",
                "Item3",
                "Item4",
                "Item5",
                "Item6",
                "Item7",
            };

            return Ok(returnList);
        }

       

    }
}
