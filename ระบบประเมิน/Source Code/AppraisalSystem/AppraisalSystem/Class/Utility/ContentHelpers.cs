using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace AppraisalSystem.Utility
{
    public class ContentHelpers
    {
        public ContentHelpers()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string MD5Encode(string input)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }

        /*************************************** TEXT HELPERS **************************************/

        public static bool Isnull(object Value)
        {
            if ((Value == null))
                return true;

            if ((object.ReferenceEquals(Value, DBNull.Value)))
                return true;

            if ((Value.ToString().Trim() == ""))
                return true;

            return false;
        }

        public static bool IsNotnull(object Value)
        {
            return !Isnull(Value);
        }

        public static String GetEncryptMD5(String password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
        }

        public static bool IsEmail(object Value)
        {
            if (Isnull(Value))
                return false;
            else
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex regEx = new Regex(strRegex);
                return regEx.IsMatch(Value.ToString());
            }
        }

        public static bool IsNumeric(object Value)
        {
            if (IsNotnull(Value))
            {
                Regex regEx = new Regex("(^[0-9]*$)");
                return regEx.IsMatch(Value.ToString());
            }
            else
                return false;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string ConvertTHDateFormat(object date, bool isShowTime)
        {
            if (date == DBNull.Value && date == null)
                return " - ";
            else
            {
                if (isShowTime)
                    return Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH"));
                else
                    return Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            }
        }

        public static string ConvertENDateFormat(object date, bool isShowTime)
        {
            if (date == DBNull.Value && date == null)
                return " - ";
            else
            {
                if (isShowTime)
                    return Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                else
                    return Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy", new CultureInfo("en-US"));
            }
        }

        public static string ConvertNullToEmpty(object data)
        {
            string result = "";
            if (!Isnull(data))
            {
                result = data.ToString().Trim();
            }
            return result;
        }

        public static int ConvertNullToInt(object data)
        {
            int result = 0;
            if (!Isnull(data))
            {
                result = Convert.ToInt32(data);
            }
            return result;
        }

        public static string DisplayCurrency(object currency)
        {
            if (currency == DBNull.Value)
                return "";
            else
                return Convert.ToDouble(currency).ToString("#,##0.00");
        }

        public static string Encode(string str)
        {
            if (IsNotnull(str))
            {
                byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(encbuff);
            }
            return "";
        }

        public static string Decode(string str)
        {
            if (IsNotnull(str))
            {
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            return "";
        }

        public static string getShortParam(object obj, int width)
        {
            string shortTopic = string.Empty;
            int start = 0;
            int end = width;
            if (obj == null && string.IsNullOrEmpty(obj.ToString()))
                return shortTopic;
            else
            {
                // cut html tag
                string objDesc = obj.ToString();
                for (int i = start; i < objDesc.Length; i++)
                {
                    if (objDesc.IndexOf("<") > -1)
                    {
                        int matchSearch1 = objDesc.IndexOf("<");
                        int matchSearch2 = objDesc.IndexOf(">") + 1;
                        objDesc = objDesc.Remove(matchSearch1, matchSearch2 - matchSearch1);
                    }
                }
                // cut short desc
                if (objDesc.Length > end)
                    shortTopic = objDesc.Substring(start, end) + "...";
                else
                    shortTopic = objDesc;

                return shortTopic;
            }
        }

        /************************************* IMAGE HELPERS **************************************/

        public static bool checkImgType(HttpPostedFile img)
        {
            bool isValid = true;
            string ext = System.IO.Path.GetExtension(img.FileName);
            string[] arrExtAccept = { ".jpg", ".JPG", ".png", ".PNG", ".gif", ".GIF" };

            if (Array.IndexOf(arrExtAccept, ext) < 0)
                isValid = false;

            return isValid;
        }

        public static string UploadImage(FileUpload Fileupload, string currentFileName, string path, bool isOriginalpicOnly, int? width, int? height)
        {
            string strReturnVal = null;
            string LogicalPath = System.Web.HttpContext.Current.Server.MapPath(path);

            if (Fileupload.HasFile && Fileupload != null && Fileupload.FileName != "")
            {
                //If you want pic full size & thumnail, rename 2 function 
                // SaveFromSteam is thumnail
                // SaveAs is pic full size

                string newFileName = "img_" + Guid.NewGuid().ToString();
                string originalFileType = System.IO.Path.GetExtension(Fileupload.FileName);
                string originalFilePath = LogicalPath + @"/" + newFileName + originalFileType;

                if (!isOriginalpicOnly)
                {
                    string thumnailFileType = "_n" + System.IO.Path.GetExtension(Fileupload.FileName);
                    string thumnailFilePath = LogicalPath + @"/" + newFileName + thumnailFileType;

                    byte[] thumImg;
                    thumImg = ImageUtility.SaveFromStream(width, height, Fileupload, newFileName + thumnailFileType, thumnailFilePath);
                }

                Fileupload.SaveAs(originalFilePath);
                strReturnVal = newFileName;
            }
            else
            {
                if (currentFileName != null)
                {
                    strReturnVal = currentFileName.ToString();
                }
            }

            return strReturnVal;
        }

        /********************************** NOTIFICATION HELPERS ***********************************/

        public static string getAlertBox(DataInfo.AlertStatusId msgType, string msg)
        {
            string alertBox = string.Empty;
            string classAlert = string.Empty;
            string textHeading = string.Empty;

            if (string.IsNullOrEmpty(msg))
                msg = "Alert message is NULL!";

            switch (msgType)
            {
                case DataInfo.AlertStatusId.COMPLETE: classAlert = "alert alert-success";
                    break;
                case DataInfo.AlertStatusId.ERROR: classAlert = "alert alert-danger";
                    break;
            }

            alertBox = "<div class='" + classAlert + "'>";
            alertBox += "<p>" + msg + "</p>";
            alertBox += "</div>";

            return alertBox;
        }

        public static string checkTrueData(object isTrueData)
        {
            string imgPath = "";
            if (bool.Parse(isTrueData.ToString()))
                imgPath = "<img src='Img/ico_true2.png' class='unitPng' />";
            return imgPath;
        }

        public static void setCookies(string name, string value)
        {
            /*HttpCookieCollection cookie = new HttpCookieCollection();
            HttpContext.Current.Response.Cookies["SearchCookies"][name] = value;

            HttpContext.Current.Response.Cookies.Add(cookie);*/

        }

        /********************************** REPLACE ***********************************/

        public static string replaceText(string value, string replaceTxt)
        {
            string data = "";
            if (IsNotnull(value))
            {
                data = value.Replace(replaceTxt, "");   
            }
            return data;
        }

        public static string replaceNewText(string value, string replaceTxt, string newReplaceTxt)
        {
            string data = "";
            if (IsNotnull(value))
            {
                data = value.Replace(replaceTxt, newReplaceTxt);
            }
            return data;
        }

        public static Double replaceMoneyToDouble(string textMoney)
        {
            string result = "";
            if (IsNotnull(textMoney))
            {
                result = replaceText(textMoney, ",");
            }
            return Convert.ToDouble(result);
        }
    }
}