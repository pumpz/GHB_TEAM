using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppraisalSystem.Utility
{
    public class DataInfo
    {
        public enum AlertStatusId : int
        {
            INFO = 0,
            ERROR = 1,
            WARNING = 2,
            COMPLETE = 3
        }

        public enum StatusTypeId : int
        {
            ACTIVE = 1,
            INACTIVE = 2,
            CLOSE = 3
        }

        public enum AdminTypeId : int
        {
            ADMIN = 1,
            USER = 2
        }

        public enum AdminPermId : int
        {
            INSERT = 1,
            EDIT = 2,
            DEL = 3
        }
    }
}