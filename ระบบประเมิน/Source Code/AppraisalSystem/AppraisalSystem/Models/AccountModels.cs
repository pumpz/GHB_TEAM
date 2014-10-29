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
        [Display(Name = "อีเมลล์")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "เบอร์โทรศัพท์")]
        public string Phone { get; set; }
    }

    public class UserModel
    {
        [Display(Name = "รหัสผู้ใช้งาน")]
        public int UserID { get; set; }
        [Display(Name = "ชื่อผู้ใช้งาน")]
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
        public int DeleteFlag { get; set; }
        public string CitizenID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? Last_Login { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
        public DateTime? Delete_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_By { get; set; }
        public string Delete_By { get; set; }
    }
    public class RoleModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
       
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
        Hashtable CreateUser(RegisterModel register, string createBy);
        Boolean UpdateUser(RegisterModel register, string updateBy);
        Boolean DeleteUser(int userId, string delBy);
        Boolean ChangePassword(string userName, string oldPassword, string newPassword, string updateBy);
        Boolean LockUser(int userId, Boolean isLock, string updateBy);
        Boolean LogOut(string userName);
        List<UserModel> GetUsers(string keyword);
        UserModel GetUsersByID(int id);
        List<RoleModel> GetAllRole();
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

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public Hashtable CreateUser(RegisterModel register, String createBy)
        {
            if (String.IsNullOrEmpty(register.UserName)) throw new ArgumentException("Value cannot be null or empty.", "username");
            if (String.IsNullOrEmpty(register.Email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(register.CitizenID)) throw new ArgumentException("Value cannot be null or empty.", "citizen");
            if (String.IsNullOrEmpty(createBy)) throw new ArgumentException("Value cannot be null or empty.", "createBy");
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
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

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_INS_USERS, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_user_name", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_password", MySqlDbType.VarChar).Value = ContentHelpers.Isnull(register.Password) ? 
                                                                                        ContentHelpers.MD5Hash(Resources.ConfigResource.PASSWORD_DEFAULT) : 
                                                                                        ContentHelpers.MD5Hash(register.Password);
                        cmd.Parameters.Add("p_roleid", MySqlDbType.VarChar).Value = register.RoleID;
                        cmd.Parameters.Add("p_citizenid", MySqlDbType.VarChar).Value = register.CitizenID;
                        cmd.Parameters.Add("p_name", MySqlDbType.VarChar).Value = register.Name;
                        cmd.Parameters.Add("p_email", MySqlDbType.VarChar).Value = register.Email;
                        cmd.Parameters.Add("p_phone", MySqlDbType.VarChar).Value = register.Phone;
                        cmd.Parameters.Add("p_create_by", MySqlDbType.VarChar).Value = createBy;

                        cmd.Parameters.Add(new MySqlParameter("oMessage", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("oUserID", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                        cmd.ExecuteScalar();
                        //
                        int userId = cmd.Parameters["oUserID"].Value == System.DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters["oUserID"].Value);
                        if (userId > 0)
                        {
                            tran.Commit();
                            process = true;
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
            result["Status"] = process;
            result["Message"] = msg;
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean UpdateUser(RegisterModel register, String updateBy)
        {
            if (String.IsNullOrEmpty(register.UserName)) throw new ArgumentException("Value cannot be null or empty.", "username");
            if (String.IsNullOrEmpty(register.Email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(register.CitizenID)) throw new ArgumentException("Value cannot be null or empty.", "citizen");
            if (String.IsNullOrEmpty(updateBy)) throw new ArgumentException("Value cannot be null or empty.", "updateBy");
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_UPD_USERS, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_user_name", MySqlDbType.VarChar).Value = register.UserName;
                        cmd.Parameters.Add("p_password", MySqlDbType.VarChar).Value = ContentHelpers.Isnull(register.Password) ?
                                                                                        ContentHelpers.MD5Hash(Resources.ConfigResource.PASSWORD_DEFAULT) :
                                                                                        ContentHelpers.MD5Hash(register.Password);
                        cmd.Parameters.Add("p_roleid", MySqlDbType.VarChar).Value = register.RoleID;
                        cmd.Parameters.Add("p_citizenid", MySqlDbType.VarChar).Value = register.CitizenID;
                        cmd.Parameters.Add("p_name", MySqlDbType.VarChar).Value = register.Name;
                        cmd.Parameters.Add("p_email", MySqlDbType.VarChar).Value = register.Email;
                        cmd.Parameters.Add("p_phone", MySqlDbType.VarChar).Value = register.Phone;
                        cmd.Parameters.Add("p_update_by", MySqlDbType.VarChar).Value = updateBy;

                        int excute = cmd.ExecuteNonQuery();
                        //
                        if (excute > 0)
                        {
                            tran.Commit();
                            process = true;
                        }
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean DeleteUser(int userId, string delBy)
        {
            if (userId <= 0) throw new ArgumentException("Value cannot be null or empty.", "userId");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_DEL_USERS, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_user_id", MySqlDbType.Int32).Value = userId;
                        cmd.Parameters.Add("p_delete_by", MySqlDbType.VarChar).Value = delBy;

                        int excute = cmd.ExecuteNonQuery();
                        //
                        if (excute > 0)
                        {
                            tran.Commit();
                            process = true;
                        }
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean ChangePassword(string userName, string oldPassword, string newPassword, string updateBy)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
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

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_UPD_USERS_CHANGE_PWD, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("iUserName", MySqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("iOldPassword", MySqlDbType.VarChar).Value = oldPassword;
                        cmd.Parameters.Add("iNewPassword", MySqlDbType.VarChar).Value = newPassword;
                        cmd.Parameters.Add("iUpdateBy", MySqlDbType.VarChar).Value = updateBy;

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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean LockUser(int userId, Boolean isLock, string updateBy)
        {
            if (userId <= 0) throw new ArgumentException("Value cannot be null or empty.", "userId");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_UPD_USERS_LOCK, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("iUserID", MySqlDbType.Int32).Value = userId;
                        if (isLock)
                        {
                            cmd.Parameters.Add("iUnlock", MySqlDbType.Int32).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("iUnlock", MySqlDbType.Int32).Value = 1;
                        }
                        cmd.Parameters.Add("iUpdateBy", MySqlDbType.VarChar).Value = updateBy;

                        int excute = cmd.ExecuteNonQuery();
                        //
                        if (excute > 0)
                        {
                            tran.Commit();
                            process = true;
                        }
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

        [DataObjectMethod(DataObjectMethodType.Update)]
        public Boolean LogOut(string userName)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            bool process = false;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    tran = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.USP_UPD_USERS_LOGOUT, conn, tran))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("iUserName", MySqlDbType.VarChar).Value = userName;

                        int excute = cmd.ExecuteNonQuery();
                        //
                        if (excute > 0)
                        {
                            tran.Commit();
                            process = true;
                        }
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

        [DataObjectMethod(DataObjectMethodType.Fill)]
        public List<UserModel> GetUsers(string keyword)
        {
            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            List<UserModel> UserItemList = null;
            DateTime? Nullable = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_USERS, conn))
                    {
                        cmd.CommandText += string.Format(" WHERE DELETE_FLAG =  0 ");
                        if (ContentHelpers.IsNotnull(keyword))
                        {
                            cmd.CommandText += string.Format(" AND USER_NAME LIKE '%{0}%' OR CITIZEN_ID LIKE '%{0}%' OR NAME LIKE '%{0}%'", keyword);
                        }
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                UserItemList = new List<UserModel>();
                                while (dr.Read())
                                {
                                    UserModel UserItem = new UserModel();
                                    UserItem.UserID = dr["USER_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["USER_ID"]);
                                    UserItem.UserName = dr["USER_NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["USER_NAME"]);
                                    UserItem.RoleID = dr["ROLE_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ROLE_ID"]);
                                    UserItem.RoleCode = dr["ROLE_CODE"] == System.DBNull.Value ? "" : Convert.ToString(dr["ROLE_CODE"]);
                                    UserItem.RoleName = dr["ROLE_NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["ROLE_NAME"]);
                                    UserItem.CitizenID = dr["CITIZEN_ID"] == System.DBNull.Value ? "" : Convert.ToString(dr["CITIZEN_ID"]);
                                    UserItem.Name = dr["NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["NAME"]);
                                    UserItem.Email = dr["EMAIL"] == System.DBNull.Value ? "" : Convert.ToString(dr["EMAIL"]);
                                    UserItem.Phone = dr["PHONE"] == System.DBNull.Value ? "" : Convert.ToString(dr["PHONE"]);
                                    UserItem.Last_Login = dr["LAST_LOGIN"] == System.DBNull.Value ? Nullable : Convert.ToDateTime(dr["LAST_LOGIN"]);
                                    UserItem.DeleteFlag = dr["DELETE_FLAG"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["DELETE_FLAG"]);
                                    UserItem.Create_Date = dr["CREATE_DATE"] == System.DBNull.Value ? Nullable : Convert.ToDateTime(dr["CREATE_DATE"]);
                                    UserItem.Update_Date = dr["UPDATE_DATE"] == System.DBNull.Value ? Nullable : Convert.ToDateTime(dr["UPDATE_DATE"]);
                                    UserItem.Delete_Date = dr["DELETE_DATE"] == System.DBNull.Value ? Nullable : Convert.ToDateTime(dr["DELETE_DATE"]);
                                    UserItem.Create_By = dr["CREATE_BY"] == System.DBNull.Value ? "" : Convert.ToString(dr["CREATE_BY"]);
                                    UserItem.Update_By = dr["UPDATE_BY"] == System.DBNull.Value ? "" : Convert.ToString(dr["UPDATE_BY"]);
                                    UserItem.Delete_By = dr["DELETE_BY"] == System.DBNull.Value ? "" : Convert.ToString(dr["DELETE_BY"]);
                                    UserItem.Status = dr["STATUS"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["STATUS"]);
                                    UserItemList.Add(UserItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
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
            return UserItemList;
        }

        [DataObjectMethod(DataObjectMethodType.Fill)]
        public UserModel GetUsersByID(int id)
        {
            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            UserModel UserItem = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_USERS, conn))
                    {
                        cmd.CommandText += string.Format(" WHERE USER_ID = {0}", id);
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    UserItem = new UserModel();
                                    UserItem.UserID = dr["USER_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["USER_ID"]);
                                    UserItem.UserName = dr["USER_NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["USER_NAME"]);
                                    UserItem.RoleID = dr["ROLE_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ROLE_ID"]);
                                    UserItem.RoleCode = dr["ROLE_CODE"] == System.DBNull.Value ? "" : Convert.ToString(dr["ROLE_CODE"]);
                                    UserItem.RoleName = dr["ROLE_NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["ROLE_NAME"]);
                                    UserItem.CitizenID = dr["CITIZEN_ID"] == System.DBNull.Value ? "" : Convert.ToString(dr["CITIZEN_ID"]);
                                    UserItem.Name = dr["NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["NAME"]);
                                    UserItem.Email = dr["EMAIL"] == System.DBNull.Value ? "" : Convert.ToString(dr["EMAIL"]);
                                    UserItem.Phone = dr["PHONE"] == System.DBNull.Value ? "" : Convert.ToString(dr["PHONE"]);
                                    UserItem.DeleteFlag = dr["DELETE_FLAG"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["DELETE_FLAG"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
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
            return UserItem;
        }
        [DataObjectMethod(DataObjectMethodType.Fill)]
        public List<RoleModel> GetAllRole()
        {
            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            MySqlConnection conn = null;
            List<RoleModel> roles = new List<RoleModel>();
            RoleModel role = null;
            try
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(Resources.SQLResource.VIEW_ROLES, conn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    role = new RoleModel();
                                    role.RoleID = dr["ROLE_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ROLE_ID"]);
                                    role.RoleName = dr["ROLE_NAME"] == System.DBNull.Value ? "" : Convert.ToString(dr["ROLE_NAME"]);
                                    roles.Add(role);
                                   
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ms)
            {
                throw new Exception("MySqlException: " + ms.Message);
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
            return roles;
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut(string userName);
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            HttpContext.Current.Session["UserName"] = ContentHelpers.Encode(userName);
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut(string userName)
        {
            FormsAuthentication.SignOut();
            IMembershipService MembershipService = new AccountMembershipService();
            if (!MembershipService.LogOut(userName))
            {
                throw new Exception("Logout unsuccess");
            }
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
            return (valueAsString == null || valueAsString.Length >= _minCharacters);
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
