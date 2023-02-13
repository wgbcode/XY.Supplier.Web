using System;

namespace XY.Supplier.Web.Models.Common
{
    public class Signin_Model
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string ImageCode { get; set; }
        public bool IsRemember { get; set; }
        public string Qr { get; set; }
        public string Guid { get; set; }
    }

    public class XWZN_Signin_Qr_Model<T>
    {
        public bool Success { get; set; }
        public int ResultCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class XWZN_Signin_Token
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }

    public class XWZN_UserInfo
    {
        public string Realname { get; set; }
        public string Nickname { get; set; }
        public string HeadImage { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public class XWZN_BaseInfo_Para
    {
        public string PassportId { get; set; }
    }
}
