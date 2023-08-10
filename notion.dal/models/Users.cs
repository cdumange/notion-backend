using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace notion.dal.models
{
    public class Users
    {
        public Guid ID { get; set; }
        public string Email { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}