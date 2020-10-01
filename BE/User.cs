using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserType Type { get; set; }
        public bool FinishedRegistration { get; set; } = false;
        public int Code { get; set; } = 0;
    }
}
