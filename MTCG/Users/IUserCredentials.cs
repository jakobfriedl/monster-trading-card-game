using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Users {
    interface IUserCredentials {
        string Username { get; set; }
        string Password { get; set; }
    }
}
