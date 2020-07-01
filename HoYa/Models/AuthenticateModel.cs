namespace HoYa.Models
{
    /// <summary>
    /// 登入
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 國際移動設備識別碼
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 權杖
    /// </summary>
    public class LoginReturnModel
    {
      

        /// <summary>
        /// 值
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 使用者序號
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 到期秒數
        /// </summary>
        public string ExpiresIn { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string End { get; set; }

        /// <summary>
        /// 使用者角色名稱
        /// </summary>
        public string RoleId { get; set; }
    }

    public class RegisterModel
    {
     
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }

       

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
    }

    public class ChangePasswordModel
    {
        /// <summary>
        /// 新密碼
        /// </summary>
        public string NewPassword { get; set; }
    }

    public class ResetPasswordResponseModel
    {
        /// <summary>
        /// 過度密碼寄至
        /// </summary>
        public string TempPasswordSendTo { get; set; }

        /// <summary>
        /// 過度密碼
        /// </summary>
        public string TempPassword { get; set; }
    }
}