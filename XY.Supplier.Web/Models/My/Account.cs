using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XY.Entity.xy_boss;

namespace XY.Supplier.Web.Models.My
{
    public class Account_Model
    {
        public supplier SupplierInfo { get; set; }
    }

    public class Account_Update_Model
    {
        public string Name { get; set; }
    }

}
