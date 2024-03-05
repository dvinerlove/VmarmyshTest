using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using VmarmyshTest.Filters;
using VmarmyshTest.Models;

namespace VmarmyshTest.Controllers
{
    [ApiController]
    [Tags("User/Tree")]
    [Route("Api/User/Tree")]
    public class TreeController : AppControllerBase
    {
        private readonly ILogger<TreeController> _logger;
        private readonly AppDbContext _context;

        public TreeController(ILogger<TreeController> logger, AppDbContext context)
        {
            _logger = logger;

            _context = context;
        }

        [HttpPost("Get")]
        [ProducesResponseType(200, Type = typeof(Models.Node))]
        public IActionResult Get([BindRequired] string? treeName)
        {
            this.CheckRequiredFields(MethodBase.GetCurrentMethod());

            var tree = _context.LoadEntireTree(treeName);

            _context.Nodes.Update(tree);
            _context.SaveChanges();

            return Ok(tree);
        }
    }
}