using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GD
{
    public class IncorrectAccountInfoException : Exception
    {
        public IncorrectAccountInfoException() : base("The account credentials you have provided are incorrect.") { }
    }
    public class GDAccount
    {
        public int AccountID { get; }
        public int UserID { get; }
        public string Username { get; }
        public string Password { get; }
        public string GJP { get; }

        public GDAccount(string userName, string password)
        {
            Username = userName;
            Password = password;
            GJP = GJPTools.Encode(Password);

            GDLoginResponse response = API.Login(Username, Password);
            AccountID = response.AccountId;
            UserID = response.UserId;
        }
    }

    public class GDLoginResponse : GDServerResponse
    {
        public int AccountId { get; }
        public int UserId { get; }
        public GDLoginResponse(GDServerResponse response) : base(response)
        {
            if (response.Status == Status.Error)
                throw new IncorrectAccountInfoException();

            // shitty hack but that's what you get when you try to generalize robtop code
            AccountId = Convert.ToInt32(Data.Keys.First());
            UserId = GetInt(AccountId);
            // man couldn't keep his server response formats consistent smh
        }
    }
}
