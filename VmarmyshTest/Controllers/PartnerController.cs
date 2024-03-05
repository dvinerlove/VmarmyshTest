using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VmarmyshTest.Models;

namespace VmarmyshTest.Controllers
{
    [ApiController]
    [Tags("User/Partner")]
    [Route("Api/User/Partner")]
    public class PartnerController : ControllerBase
    {
        [HttpPost("RememberMe")]
        public IActionResult RememberMe([BindRequired] string code)
        {
            return Ok();
        }
    }
}