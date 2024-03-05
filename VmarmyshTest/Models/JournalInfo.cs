using System;

namespace VmarmyshTest.Models
{
    public class JournalInfo
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTimeOffset.Now.DateTime;
    }

}
