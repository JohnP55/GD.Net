using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GDAPI
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

            GDLoginResponse response = Login();
            AccountID = response.AccountId;
            UserID = response.UserId;
        }

        public GDLoginResponse Login()
        {
            string url = "accounts/loginGJAccount.php";
            string parameters = $"udid=doesntmatterlolxd&userName={Username}&password={Password}&sID=6969696969";
            GDLoginResponse response = new GDLoginResponse(GDHTTP.Post(url, parameters, GDHTTP.ACCOUNT_SECRET, ','));

            return response;
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
            AccountId = Data.Keys.First();
            UserId = GetInt(AccountId);
            // man couldn't keep his server response formats consistent smh
        }
    }
}
