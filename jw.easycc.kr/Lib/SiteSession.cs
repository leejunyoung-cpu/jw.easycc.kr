using System;
using CommonLibrary.DBServices;
using System.Web;
using CommonLibrary.CommonModule;
using CommonLibrary.Constants;
using System.Web.UI;
using Newtonsoft.Json;
using System.Text;
using CommonLibrary.CommonModel;
using DbConCommon;
using CommonLibrary.DBModel;
using CommonLibrary.Extensions;
using System.Security.Policy;

//===============================================================
// FileName       : SiteSession.cs
// Description    : 세션 Class
// Author         : ljy@jwccs.com, 2024-06-24
// Modify History : Just Created.
//================================================================
namespace CommonLibrary.Session
{
    public class SiteSession
    {
        #region Define Variables
        SiteSession_Member objSesMember = new SiteSession_Member();
        private string strDomainName;
        AdminService objAdminDBServices = new AdminService();
        #endregion

        public SiteSession()
        {
            strDomainName = SiteGlobal.GetHttpHost(HttpContext.Current.Request);
        }

        public SiteSession(Page p)
        {
            strDomainName = SiteGlobal.GetHttpHost(HttpContext.Current.Request);
            string lo_strSessionKey = string.Empty;

            GetSessionCookie(out lo_strSessionKey);
            
            if (string.IsNullOrWhiteSpace(lo_strSessionKey))
            {
                GoLogin("");
            }
            else
            {
                SiteSessionSub(lo_strSessionKey);
            }
        }

        private void SiteSessionSub(string strSessionKey)
        {
            ConnectGlobal lo_objDas = null;
            string lo_strErrMsg = string.Empty;

            VerifySession(ref lo_objDas, strSessionKey, out lo_strErrMsg);
            if (false == IsLogin)
            {
                GoLogin(lo_strErrMsg);
            }
            else
            {
                CreateSessionCookie(strSessionKey);
            }
        }

        public int SiteSessionSub4NoSession(ref ConnectGlobal objDas, string strSessionKey)
        {
            int lo_intRetVal = 0;
            string lo_strErrMsg = string.Empty;

            goLogout();

            VerifySession(ref objDas, strSessionKey, out lo_strErrMsg);
            if (false == IsLogin)
            {
                lo_intRetVal = 9999;
            }

            CreateSessionCookie(strSessionKey);

            return lo_intRetVal;
        }

        private void VerifySession(ref ConnectGlobal objDas, string strSessionKey, out string strErrMsg)
        {
            IsLogin = false;
            strErrMsg = string.Empty;
            ServiceResult<AdminSessionInfo> lo_objResAdminSessionInfo = null;
            string strRemoteAddr = SiteGlobal.GetRemoteAddr();

            if (string.IsNullOrWhiteSpace(strSessionKey))
            {
                return;
            }

            try
            {
                if (SiteGlobal.GetRequestForm("PdfFlag").Equals("Y") && !string.IsNullOrWhiteSpace(SiteGlobal.GetRequestForm("UserAddr")))
                {
                    strRemoteAddr = SiteGlobal.GetRequestForm("UserAddr");
                }

                // 1. 어드민 세션 정보 등록
                lo_objResAdminSessionInfo = GetAdminSession(ref objDas, strSessionKey, strRemoteAddr);
                if (lo_objResAdminSessionInfo.result.ErrorCode.IsSuccess())
                {
                    IsLogin = true;
                    SessionKey = strSessionKey;
                    AdminID = lo_objResAdminSessionInfo.data.AdminID;
                    MobileNo = lo_objResAdminSessionInfo.data.MobileNo;
                    AdminName = lo_objResAdminSessionInfo.data.AdminName;
                    Email = lo_objResAdminSessionInfo.data.Email;
                    AccessClientCode = lo_objResAdminSessionInfo.data.AccessClientCode;
                    GradeCode = lo_objResAdminSessionInfo.data.GradeCode;
                    GradeName = lo_objResAdminSessionInfo.data.GradeName;
                    LastLoginDate = lo_objResAdminSessionInfo.data.LastLoginDate;
                    LastLoginIP = lo_objResAdminSessionInfo.data.LastLoginIP;
                    PwdUpdDate = lo_objResAdminSessionInfo.data.PwdUpdDate;
                    ExpireYmd = lo_objResAdminSessionInfo.data.ExpireYmd;
                    PrivateAvailFlag = lo_objResAdminSessionInfo.data.PrivateAvailFlag;

                    SessionCreateDate = DateTime.Now;
                }
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("SiteSession", "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9100);
            }
        }

        public ServiceResult<AdminSessionInfo> GetAdminSession(ref ConnectGlobal objDas, string strSessionKey, string strAccessIP)
        {
            ServiceResult<AdminSessionInfo> lo_objResAdminSessionInfo = null;

            try
            {
                lo_objResAdminSessionInfo = objAdminDBServices.GetAdminSession(ref objDas, strSessionKey, strAccessIP);
            }
            catch (Exception e)
            {
                lo_objResAdminSessionInfo.result.ErrorCode = 9103;
                lo_objResAdminSessionInfo.result.ErrorMsg = "System error(fail to verifying session)" + e.Message;
                SiteGlobal.WriteLog("SiteSession", "Exception", lo_objResAdminSessionInfo.result.ErrorMsg, lo_objResAdminSessionInfo.result.ErrorCode);
            }

            return lo_objResAdminSessionInfo;
        }

        public void CreateSessionCookie(string strSessionKey)
        {
            ExpireSessionCookie();

            string lo_strEncrypted = CommonUtils.Utils.GetEncrypt(strSessionKey + (Char)2 + JsonConvert.SerializeObject(objSesMember));

            HttpCookie objCookie = new HttpCookie(CommonConstant.COOKIE_NAME)
            {
                Value = lo_strEncrypted,
                Domain = strDomainName,
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        private void ExpireSessionCookie()
        {
            HttpCookie lo_objCookie = new HttpCookie(CommonConstant.COOKIE_NAME)
            {
                Value = null,
                Domain = strDomainName,
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true
            };


            HttpContext.Current.Response.Cookies.Add(lo_objCookie);
        }

        public void GetSessionCookie(out string strSessionKey)
        {
            string[] arrSessionKey;

            HttpCookie lo_objCookie = null;

            strSessionKey = string.Empty;

            lo_objCookie = HttpContext.Current.Request.Cookies[CommonConstant.COOKIE_NAME];
                   
            if (null == lo_objCookie || string.IsNullOrWhiteSpace(lo_objCookie.Value))
            {
                return;
            }

            arrSessionKey = CommonUtils.Utils.GetDecrypt(lo_objCookie.Value).Split((Char)2);

            if (arrSessionKey.Length.Equals(2))
            {
                objSesMember = JsonConvert.DeserializeObject<SiteSession_Member>(arrSessionKey[1]);
                if (objSesMember.SessionCreateDate == null || objSesMember.SessionCreateDate < DateTime.Now.AddHours(-6))
                {
                    objSesMember.IsLogin = false;
                }
            }
            
            strSessionKey = arrSessionKey[0];

        }

        public void GoLogin(string strErrMsg)
        {
            string lo_strProtocol = SiteGlobal.GetHttpProtocol();
            string lo_strHost = HttpContext.Current.Request.ServerVariables.Get("HTTP_HOST");
            string lo_strRetUrl = CommonConstant.LOGIN_PAGE + "?returnurl=" + lo_strProtocol + "://" + lo_strHost;

            if (!string.IsNullOrWhiteSpace(strErrMsg))
            {
                lo_strRetUrl += "&errmsg=" + strErrMsg;
            }

            // 리다이렉트 URL 에는 줄바꿈이 있으면 안된다.
            lo_strRetUrl = lo_strRetUrl.Replace("\r", "").Replace("\n", "");


            HttpContext.Current.Response.Redirect(lo_strRetUrl, true);

            StringBuilder sb = new StringBuilder();
            sb.Append("\n   <!DOCTYPE html>");
            sb.Append("\n   <script type=\"text/javascript\">");
            sb.Append("\n   parent.location.href='" + lo_strRetUrl + "';");
            sb.Append("\n   </script>");

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        public void goLogout()
        {
            //string lo_strSessionKey = string.Empty;
            //GetSessionCookie(out lo_strSessionKey);

            ExpireSessionCookie();
        }

        public bool IsLogin
        {
            get { return objSesMember.IsLogin; }
            set { objSesMember.IsLogin = value; }
        }

        public int AuthCode
        {
            get { return objSesMember.AuthCode; }
            set { objSesMember.AuthCode = value; }
        }

        public string AdminID
        {
            get { return objSesMember.AdminID; }
            set { objSesMember.AdminID = value; }
        }
        public string AdminName
        {
            get { return objSesMember.AdminName; }
            set { objSesMember.AdminName = value; }
        }
        public string MobileNo
        {
            get { return objSesMember.MobileNo; }
            set { objSesMember.MobileNo = value; }
        }
        public string Email
        {
            get { return objSesMember.Email; }
            set { objSesMember.Email = value; }
        }
        public string LastLoginDate
        {
            get { return objSesMember.LastLoginDate; }
            set { objSesMember.LastLoginDate = value; }
        }
        public string LastLoginIP
        {
            get { return objSesMember.LastLoginIP; }
            set { objSesMember.LastLoginIP = value; }
        }
        public string PwdUpdDate
        {
            get { return objSesMember.PwdUpdDate; }
            set { objSesMember.PwdUpdDate = value; }
        }
        public string ExpireYmd
        {
            get { return objSesMember.ExpireYmd; }
            set { objSesMember.ExpireYmd = value; }
        }
        public string PrivateAvailFlag
        {
            get { return objSesMember.PrivateAvailFlag; }
            set { objSesMember.PrivateAvailFlag = value; }
        }
        public string SiteID
        {
            get { return objSesMember.SiteID; }
            set { objSesMember.SiteID = value; }
        }
        public int ClientCode
        {
            get { return objSesMember.ClientCode; }
            set { objSesMember.ClientCode = value; }
        }

        public string AccessClientCode
        {
            get { return objSesMember.AccessClientCode; }
            set { objSesMember.AccessClientCode = value; }
        }

        public int GradeCode
        {
            get { return objSesMember.GradeCode; }
            set { objSesMember.GradeCode = value; }
        }
        public string GradeName
        {
            get { return objSesMember.GradeName; }
            set { objSesMember.GradeName = value; }
        }
        
        public string SessionKey
        {
            get { return objSesMember.SessionKey; }
            set { objSesMember.SessionKey = value; }
        }
        public DateTime SessionCreateDate
        {
            get { return objSesMember.SessionCreateDate; }
            set { objSesMember.SessionCreateDate = value; }
        }
    }

    public class SiteSession_Member
    {
        public bool     IsLogin      { get; set; }
        public int      AuthCode     { get; set; }
        public string   AdminID      { get; set; } = string.Empty;
        public string   AdminName    { get; set; } = string.Empty;
        public string   SiteID       { get; set; } = string.Empty;
        public int      ClientCode   { get; set; }
        public string   AccessClientCode   { get; set; } = string.Empty;
        public int      GradeCode    { get; set; }
        public string GradeName     { get; set; } = string.Empty;
        public string SessionKey    { get; set; } = string.Empty;
        public string MobileNo      { get; set; } = string.Empty;
        public string Email         { get; set; } = string.Empty;
        public string LastLoginDate { get; set; } = string.Empty;
        public string LastLoginIP   { get; set; } = string.Empty;
        public string PwdUpdDate    { get; set; } = string.Empty;
        public string ExpireYmd     { get; set; } = string.Empty;
        public string PrivateAvailFlag { get; set; } = string.Empty;
        public DateTime SessionCreateDate { get; set; }
    }
}