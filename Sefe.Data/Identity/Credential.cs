using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Data.Identity
{
    public class Credential
    {
        public int Id { get; set; }
        public int RefId { get; set; }
        public byte UserType { get; set; }
        public int? RoleId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool CanAccess { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
