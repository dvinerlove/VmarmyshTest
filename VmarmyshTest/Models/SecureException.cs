using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace VmarmyshTest.Models
{
    public class SecureException : Exception
    {
        public SecureException() : base()
        {

        }

        public SecureException(string message) : base(message)
        {
        }

        public SecureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}