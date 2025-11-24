using CommonLibrary.CommonModel;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using CommonLibrary.Session;
using Newtonsoft.Json;
using CommonLibrary.DBModel;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using DbConCommon;

//===============================================================
// FileName       : AshxBaseHandler.cs
// Description    : Ashx 페이지에서 기본으로 상속받는 클래스
// Author         : ljy@jwccs.com, 2024-07-11
// Modify History : Just Created.
//================================================================
namespace CommonLibrary.CommonModule
{
    public class AshxBaseHandler : IHttpHandler, IRequiresSessionState
    {
        private const string RESPONSE_CHARSET_UTF8 = "utf-8";
        protected const string ERROR_JSON_STRING = "[{{\"RetCode\":{0},\"ErrMsg\":\"{1}\"}}]";
        public const string RESPONSE_CONTENTTYPE_JSON = "application/json";

        protected HttpContext objContext = null;
        protected HttpResponse objResponse = null;
        protected HttpRequest objRequest = null;
        protected ResponseMap objResMap = null; //Ashx전역 ResMap

        protected string strMethod = string.Empty; //요청 메소드 명

        private bool IsCheckLogin { get; set; } //로그인 체크 여부
        private bool IsCheckAuth { get; set; } //메뉴권한 체크 여부
        public bool IsHandlerStop { get; set; }

        #region 로그인 정보
        protected SiteSession objSes { get; private set; }
        #endregion

        #region 메뉴 접근권한
        private string strMenuLink = string.Empty;                           //페이지 링크
        protected Dictionary<string, MenuAuthType> objMethodAuthList = null; //메소드 별 접근권한을 담는 Dictionary
        #endregion

        protected AshxBaseHandler()
        {
            try
            {
                HttpContext.Current.Response.ContentType = RESPONSE_CONTENTTYPE_JSON;
                HttpContext.Current.Response.Charset = RESPONSE_CHARSET_UTF8;

                objResMap = new ResponseMap();
                objMethodAuthList = new Dictionary<string, MenuAuthType>();

                IsCheckLogin = true;
                IsCheckAuth = true;
                IsHandlerStop = false;
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog(
                    "AshxBaseHandler",
                    "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                    -20001);

                HttpContext.Current.Response.Status = "400 Bad request";
                HttpContext.Current.Response.StatusCode = 400;
                HttpContext.Current.Response.Write(string.Format(ERROR_JSON_STRING, -20001, CommonConstant.HTTP_STATUS_CODE_999_MESSAGE));
                HttpContext.Current.Response.End();
            }
        }
        ~AshxBaseHandler()
        {
            objResMap = null;
            objMethodAuthList = null;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 페이지 링크를 지정한다.(필수)
        /// </summary>
        protected void SetMenuLink(string strCurrMenuLink)
        {
            strMenuLink = strCurrMenuLink;
        }

        //오버라이드 가능하도록 virtual 메소드로 정의
        public virtual void ProcessRequest(HttpContext context)
        {
            int lo_intRetVal = -9999;
            ConnectGlobal lo_objDB = null;

            Init(context);

            try
            {
                //로그인 검증
                if (IsCheckLogin)
                {
                    lo_objDB = new ConnectGlobal();

                    lo_intRetVal = CheckSession(ref lo_objDB);

                    if (lo_intRetVal.IsFail())
                    {
                        WriteJsonResponse("AshxBaseHandler", 9999, "로그인 세션이 만료되었습니다.<br>다시 로그인해 주세요.");
                        IsHandlerStop = true;
                        return;
                    }

                    //메뉴접근 권한 확인
                    if (IsCheckAuth)
                    {
                        //메소드 유효성 검증 - 메뉴 접근권한 확인 시에만 검증한다.
                        if (string.IsNullOrEmpty(strMethod) || !objMethodAuthList.ContainsKey(strMethod))
                        {
                            WriteJsonResponse("AshxBaseHandler", 9999, string.Format("잘못된 메서드({0}) 호출입니다.", strMethod));
                            IsHandlerStop = true;
                            return;
                        }

                        CheckAdminMenu(ref lo_objDB);
                        if (IsHandlerStop.Equals(true))
                        {
                            return;
                        }

                        // 메뉴 호출 로그를 DB에 기록한다.
                        string lo_strCallParam = SiteGlobal.GetAllRequestLog();
                        AdminService lo_objAdminService = new AdminService();

                        lo_strCallParam = lo_strCallParam != null && lo_strCallParam.Length > 4000 ? lo_strCallParam.Substring(0, 3999) : lo_strCallParam;
                        lo_strCallParam = lo_strCallParam.Replace((char)3, '[');
                        lo_strCallParam = lo_strCallParam.Replace((char)2, ']');

                        lo_objAdminService.InsAdminLog(ref lo_objDB, objSes.AdminID, 0, strMenuLink, objMethodAuthList[strMethod].GetHashCode(), 2, lo_strCallParam);
                    }
                }
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("AshxBaseHandler", "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9312);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }
            }

            //if (objResponse.StatusCode == 400 || objResponse.StatusCode == 401)
            //{
            //    objResponse.End();
            //}
        }

        ///--------------------------------------------
        /// <summary>
        /// 페이지 기본 Json 응답 출력
        /// </summary>
        ///--------------------------------------------
        private void WriteJsonResponse(string strLogName, int nErrorCode, string strErrorMsg)
        {
            string lo_strResponse = string.Empty;
            string lo_strErrMsg = string.Empty;
            ErrorResponse response = new ErrorResponse();

            try
            {
                response.result.ErrorCode = nErrorCode;
                response.result.ErrorMsg = strErrorMsg;

                lo_strResponse = "[" + JsonConvert.SerializeObject(response) + "]";
            }
            catch (Exception lo_ex)
            {
                objResMap.RetCode = -21003;
                objResMap.ErrMsg = CommonConstant.COMMON_EXCEPTION_MESSAGE;
                lo_strErrMsg = "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace;
                lo_strResponse = string.Format(ERROR_JSON_STRING, objResMap.RetCode, objResMap.ErrMsg);
            }
            finally
            {
                // 출력
                objResponse.Write(lo_strResponse);

                // intRetVal 이 마이너스인 경우는 익셉션 에러임
                if (response.result.ErrorCode < 0)
                {
                    // 익셉션 발생시 처리 - File Logging & Send Mail
                    SiteGlobal.WriteLog(strLogName, "Exception", lo_strErrMsg, objResMap.RetCode);
                }
            }
        }

        ///--------------------------------------------
        /// <summary>
        /// 페이지 기본 Json 응답 출력
        /// </summary>
        ///--------------------------------------------
        public virtual void WriteJsonResponse(string strLogFileName)
        {
            string lo_strResponse = string.Empty;
            string lo_strErrMsg = string.Empty;

            try
            {
                //toJsonString() 제어 로직 - strResponse 판단.
                // - Not Null : strResponse 를 리턴
                // - Null     : intRetVal(ErrCode), strErrMsg(ErrMsg)를 Json String 으로 시리얼라이즈하여 리턴                
                lo_strResponse = objResMap.ToJsonString();
            }
            catch (Exception lo_ex)
            {
                objResMap.RetCode = -21003;
                objResMap.ErrMsg = CommonConstant.COMMON_EXCEPTION_MESSAGE;
                lo_strErrMsg = "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace;
                lo_strResponse = string.Format(ERROR_JSON_STRING, objResMap.RetCode, objResMap.ErrMsg);
            }
            finally
            {
                // 출력
                objResponse.Write(lo_strResponse);

                // intRetVal 이 마이너스인 경우는 익셉션 에러임
                if (objResMap.RetCode < 0)
                {
                    SiteGlobal.WriteLog(strLogFileName, "Exception", lo_strErrMsg, objResMap.RetCode);
                }
            }
        }

        /// <summary>
        /// 로그인 체크가 필요없는 경우 호출한다
        /// </summary>
        protected void IgnoreCheckSession()
        {
            IsCheckLogin = false;
        }

        /// <summary>
        /// 메뉴권한 체크가 필요없는 경우 호출한다
        /// </summary>
        protected void IgnoreCheckMenuAuth()
        {
            IsCheckAuth = false;
        }
        protected void Init(HttpContext context)
        {
            objContext = context;

            objRequest = context.Request;
            objResponse = context.Response;
            strMethod = objRequest.Params["CallType"];
        }

        /// <summary>
        /// 로그인 세션 체크 후 실패시 실패 응답
        /// </summary>
        private int CheckSession(ref ConnectGlobal objDas)
        {
            string lo_strJson = string.Empty;
            string lo_strSessionKey = string.Empty;
            int lo_intRetVal = 0;

            //세션 생성
            objSes = new SiteSession();
            objSes.GetSessionCookie(out lo_strSessionKey);
            if (objSes.IsLogin.Equals(false))
            {
                lo_intRetVal = objSes.SiteSessionSub4NoSession(ref objDas, lo_strSessionKey);
            }

            return lo_intRetVal;
        }

        /// <summary>
        /// 로그인 세션키가 쿠키게 존재하는지 체크
        /// </summary>
        private bool CheckSessionKeyCookie()
        {
            string lo_strSessionKey = string.Empty;

            //세션 생성
            objSes = new SiteSession();
            objSes.GetSessionCookie(out lo_strSessionKey);
            if (!string.IsNullOrWhiteSpace(lo_strSessionKey))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 메뉴 접근 권한을 확인
        /// </summary>
        private void CheckAdminMenu(ref ConnectGlobal objDas)
        {
            MenuAuthType lo_menuAuth;

            AdminService objAdminService = new AdminService();
            ServiceResult<AdminMenuChk> lo_objResAdminMenuChk = null;

            try
            {
                lo_objResAdminMenuChk = objAdminService.CheckAdminMenu(ref objDas, objSes.AdminID, objSes.GradeCode, strMenuLink);

                if (lo_objResAdminMenuChk.result.ErrorCode.IsSuccess())
                {
                    switch (lo_objResAdminMenuChk.data.AuthCode)
                    {
                        case 1:
                            lo_menuAuth = MenuAuthType.All;
                            break;
                        case 2:
                            lo_menuAuth = MenuAuthType.ReadWrite;
                            break;
                        case 3:
                            lo_menuAuth = MenuAuthType.ReadOnly;
                            break;
                        default:
                            lo_menuAuth = MenuAuthType.None;
                            break;
                    }
                }
                else
                {
                    lo_objResAdminMenuChk.result.ErrorCode = 0; //각 핸들러 안에서 메소드 별 설정된 권한으로 체크하도록 성공 리턴
                    lo_menuAuth = MenuAuthType.None;
                }

                //요청 메소드의 접근권한 확인
                if (objMethodAuthList[strMethod].GetHashCode() < lo_menuAuth.GetHashCode())
                {
                    objResMap.RetCode = 28001;
                    objResMap.ErrMsg = "메뉴 사용 권한이 없습니다.";
                }
            }
            catch (Exception lo_ex)
            {
                objResMap.RetCode = -20100;
                objResMap.ErrMsg = CommonConstant.HTTP_STATUS_CODE_999_MESSAGE;

                SiteGlobal.WriteLog(
                    "AshxBaseHandler",
                    "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                    lo_objResAdminMenuChk.result.ErrorCode);
            }
            finally
            {
                if (lo_objResAdminMenuChk.result.ErrorCode.IsFail())
                {
                    WriteJsonResponse("AshxBaseHandler", lo_objResAdminMenuChk.result.ErrorCode, lo_objResAdminMenuChk.result.ErrorMsg);
                    IsHandlerStop = true;
                }
            }
        }
    }
}