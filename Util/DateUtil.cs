using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectJobs.Util
{
    public static class DateUtil
    {
        public static DateTime getTimeStampToDateTime(double timeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            
            return dateTime;
        }
    }
}
