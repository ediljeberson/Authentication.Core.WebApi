using System;
using System.Collections.Generic;

#nullable disable

namespace Authentication.Core.WebApi.Models
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
