using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.Infrastructure
{
    public class LicenceManager
    {
        public Boolean CheckLicenceExpiryDate()
        {
            //read licence file
            //if no file, then fail.

            //decrypt file content
            //if decryption failed, then failed.
            //if Content failed, then failed.

            //read creation time
            //if file creationtime lower than now, then failed.
            //read expirydate
            //if expirydate higher than now, then failed.
            return true;
        }
    }

    public class LicenceFileContent
    {
        public DateTime CreationTime { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
