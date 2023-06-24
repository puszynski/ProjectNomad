using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerAccount
{
    public interface IPlayerAccountModule
    {
        void CreateAccount();
        void LogIn();
    }
}
