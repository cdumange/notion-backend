using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace utils
{
    public class JustifiedResponse : JustifiedValue<IList<string>>
    {
        protected JustifiedResponse(bool isError) : base(isError)
        {
        }

        public static JustifiedResponse Default(bool isError = false)
        {
            return (JustifiedResponse)FromValue(null, isError);
        }

        public static JustifiedResponse FromReason(string reason, bool isError = false)
        {
            return (JustifiedResponse)FromValue(new List<string> { reason }, isError);
        }

        public static JustifiedResponse FromReasons(IList<string> reasons, bool isError = false)
        {
            return (JustifiedResponse)FromValue(reasons, isError);
        }
    }
}