using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using AppraisalSystem.Utility;

namespace AppraisalSystem.Models
{

    #region Models

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสเก่า")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสใหม่")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสใหม่")]
        [Compare("NewPassword", ErrorMessage = "รหัสใช้งานและรหัสยืนยันใช้งานไม่เหมือนกัน!!")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessage = "กรุณากรอก รหัสพนักงาน / ชื่อผู้ใช้")]
        [Display(Name = "ชื่อสำหรับเข้าใช้งาน")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "กรุณากรอก รหัสสำหรับเข้าใช้งานระบบ")]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผู้ใช้งาน")]
        public string Password { get; set; }

        [Display(Name = "จดจำรหัสเข้าใช้งาน?")]
        public bool RememberMe { get; set; }
    }


    public class RegisterModel
    {
        [Required]
        [Display(Name = "ชื่อสำหรับเข้าใช้งาน")]
        public string UserName { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผู้ใช้งาน")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสผู้ใช้งาน")]
        [Compare("Password", ErrorMessage = "รหัสใช้งานและรหัสยืนยันใช้งานไม่เหมือนกัน!!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "สิทธิ์ในการเข้าถึงระบบ")]
        public int RoleID { get; set; }

        [Required]
        [Display(Name = "สถานะ")]
        public int Status { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "เลขบัตรประชาชน")]
        public string CitizenID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ชื่อ-นามสกุล")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "อีเมลย์")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "เบอร์โทรศัพท์")]
        public string Phone { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        Hashtable ValidateUser(string userName, string password);
        Boolean CreateUser(RegisterModel register, String createBy);
        Boolean ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings
                ["ConnectionString"].ConnectionString;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public Hashtable ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            Hashtable result = new Hashtable();
            bool process = false;
            string msg = "";
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    using (cmd = new MySqlCommand(Resources.SQLResource.USP_GET_USERS_LOGIN, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("iUsername", MySqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("iPassword", MySqlDbType.VarChar).Value = ContentHelpers.MD5Hash(password);
                        cmd.Parameters.Add(new MySqlParameter("oMessage", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("oUserID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                        cmd.ExecuteScalar();
                        
                        int userId = cmd.Parameters["oUserID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oUserID"].Value);
                        if (userId > 0)
                        {
                            using (cmd = new MySqlCommand(Resources.SQLResource.USP_GET_USERS_PERMISSION, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("iUsername", MySqlDbType.VarChar).Value = userName;
                                cmd.Parameters.Add(new MySqlParameter("oMessage", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new MySqlParameter("oRoleCode", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                                cmd.ExecuteScalar();

                                string roleCode = cmd.Parameters["oRoleCode"].Value == System.DBNull.Value ? "" :
                                                        Convert.ToString(cmd.Parameters["oRoleCode"].Value);
                                if (ContentHelpers.IsNotnull(roleCode))
                                {
                                    result["RoleCode"] = roleCode;
                                    process = true;
                                }
                            }
                        }
                        msg = Convert.ToString(cmd.Parameters["oMessage"].Value);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            result["Status"] = process;
            result["Message"] = msg;
            return result;
        }

        public Boolean CreateUser(RegisterModel register, String createBy)
        {
            if (String.IsNullOrEmpty(register.UserName)) throw new ArgumentException("Value cannot be null or empty.", "username");
            if (String.IsNullOrEmpty(register.Email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(register.CitizenID)) throw new ArgumentException("Value cannot be null or empty.", "citizen");
            if (String.IsNullOrEmpty(createBy)) throw new ArgumentException("Value cannot be null or empty.", "createBy");
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            string msg = "";
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_INS_USERS, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_user_name", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_password", MySqlDbType.VarChar).Value = ContentHelpers.Isnull(register.Password) ? 
                                                                                        ContentHelpers.MD5Hash(Resources.ConfigResource.PASSWORD_DEFAULT) : 
                                                                                        ContentHelpers.MD5Hash(register.Password);
                        cmd.Parameters.Add("p_roleid", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_citizenid", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_name", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_email", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_phone", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_create_by", MySqlDbType.VarChar).Value = register.UserName;

                        cmd.Parameters.Add(new MySqlParameter("oMessage", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("oUserID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oUserID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oUserID"].Value);
                        if (userId > 0)
                        {
                            tran.Commit();
                        }
                        msg = Convert.ToString(cmd.Parameters["oMessage"].Value);
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return process;
        }

        public Boolean ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                //MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                //return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
            return false;
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            HttpContext.Current.Session["UserName"] = userName;
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]{
                new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
            };
        }
    }
    #endregion

}
