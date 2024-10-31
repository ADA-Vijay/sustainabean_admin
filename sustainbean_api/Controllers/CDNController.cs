using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sustainbean_api.Models;
using sustainbean_api.Repository;
using System.Buffers.Text;

namespace sustainbean_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CDNController : ControllerBase
    {
        private readonly ICdnRepository _cdn;
        public CDNController(ICdnRepository cdn)
        {
            this._cdn = cdn;
        }

        [HttpPost]
        public IActionResult UploadOnCdn(List<ImgBase64> imgList)
        {
            var res = _cdn.UploadOnCdn(imgList).Result;
            return Ok(res);
        }
    }
}
