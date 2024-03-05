using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using VmarmyshTest.Models;

namespace VmarmyshTest.Controllers
{
    [ApiController]
    [Route("api/user/journal")]
    public class JournalController : AppControllerBase
    {
        private readonly AppDbContext _context;

        public JournalController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("GetRange")]
        [ProducesResponseType(200, Type = typeof(Models.Range<JournalInfo>))]
        public IActionResult GetJournalRange([FromBody] JournalFilter? filter,
                                             [BindRequired] int? skip,
                                             [BindRequired] int? take)
        {
            this.CheckRequiredFields(MethodBase.GetCurrentMethod());

            IQueryable<JournalInfo> infos = _context.JournalInfos;
            if (filter != null)
            {
                if (filter.From != null)
                {
                    infos = infos.Where(x => x.CreatedAt.ToUniversalTime() >= filter.From.Value.ToUniversalTime());
                }

                if (filter.To != null)
                {
                    infos = infos.Where(x => x.CreatedAt.ToUniversalTime() <= filter.To.Value.ToUniversalTime());
                }
                infos = infos.Skip(skip.Value);

            }
            return Ok(new Range<JournalInfo>()
            {
                Skip = skip.Value,
                Count = infos.Count(),
                Items = infos.Take(take.Value).ToList()
            });

        }

        [HttpPost("GetSingle")]
        [ProducesResponseType(200, Type = typeof(Models.Journal))]
        public IActionResult GetSingleJournal([BindRequired] int id)
        {
            this.CheckRequiredFields(MethodBase.GetCurrentMethod());

            return Ok(_context.Events.First(x => x.Id == id));
        }
    }
}
