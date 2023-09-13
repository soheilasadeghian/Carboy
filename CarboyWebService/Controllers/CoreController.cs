using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarBoyWebservice.Controllers
{
    public class CoreController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> GetCore()
        {
            byte[] data = await Request.Content.ReadAsByteArrayAsync();
            MBProtoLib.Core.UserAuth core = new MBProtoLib.Core.UserAuth(MBProto.Contractors.constructors,
                System.Configuration.ConfigurationManager.ConnectionStrings["CarBoyDBConnectionString"].ConnectionString, "");

            try
            {
                var response = core.Receive(data);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(response);
                return result;
            }
            catch (MBProto.Exceptions.AuthException ext)
            {
                return Request.CreateResponse(ext.Serialize<HttpStatusCode>(), ext.Serialize<string>());
            }
            catch (MBProtoLib.Exceptions.AuthException ext)
            {
                return Request.CreateResponse(ext.Serialize<HttpStatusCode>(), ext.Serialize<string>());
            }
            catch
            {
                var ext = new MBProtoLib.Exceptions.AuthException(new MBProtoLib.Exceptions.AuthException.InternalServerError());
                return Request.CreateResponse(ext.Serialize<HttpStatusCode>(), ext.Serialize<string>());
            }
        }
    }
}
