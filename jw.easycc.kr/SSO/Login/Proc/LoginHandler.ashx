<%@ WebHandler Language="C#" Class="LoginHandler" %>
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.Extensions;
using CommonLibrary.DBModel;
using CommonLibrary.CommonModel;
using CommonLibrary.DBServices;
using Newtonsoft.Json;
using System;
using System.Web;
using CommonLibrary.Session;
using DbConCommon;
///================================================================
/// <summary>
/// FileName        : LoginHandler.ashx
/// Description     : Get Login Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Copyright ⓒ 2024 by JWCCS Inc. All rights reserved.
/// Author          : ljy@jwccs.com, 2024-06-19
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class LoginHandler : AshxBaseHandler
{

    private string strCallType   = string.Empty;

    private string strAdminID    = string.Empty;
    private string strAdminPwd   = string.Empty;
    private string strOtpCode    = string.Empty;
    private string strOtpFlag    = string.Empty;
    private string strCorpNo     = string.Empty;
    private string strMemberType = string.Empty;
    private string strMobileNo   = string.Empty;

    // 아이디 찾기
    private string strIdMobileNo    = string.Empty;
    private string strIdAdminCorpNo = string.Empty;
    private string strIdAdminName   = string.Empty;


    // 본인인증 / 인증체크
    private string strEncSMSAuthNum = string.Empty;
    private string strAuthNumber    = string.Empty;
    private string strAuthInfo      = string.Empty;
    private string strAdminResetPwd = string.Empty;

    //구글 QR 불러오기
    private string strAdminAuthNumber = string.Empty;
    private string strAuthAdminID     = string.Empty;

    AdminService     objAdminDBServices   = new AdminService();

    /// <summary>
    /// You will need to configure this handler in the Web.config file of your 
    /// web and register it with IIS before being able to use it. For more information
    /// see the following link: https://go.microsoft.com/?linkid=8101007
    /// </summary>
    #region IHttpHandler Members

    public override void ProcessRequest(HttpContext context)
    {
        //NOTICE:로그인 체크가 필요없는 핸들러인 경우 호출 - 반드시 base.ProcessRequest 구문상단에서 호출해야 함
        IgnoreCheckSession();

        //0.초기화 및 세션티켓 검증
        //# 부모 클래스의 함수 호출
        base.ProcessRequest(context);
        if(base.IsHandlerStop.Equals(true))
        {
            return;
        }

        try
        {
            //1.Request
            GetData();
            if (objResMap.RetCode.IsFail())
            {
                return;
            }

            //2.처리
            Process();
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9401;
            objResMap.ErrMsg  = "예기치 않은 오류가 발생하였습니다.";

            SiteGlobal.WriteLog("LoginHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("LoginHandler");
        }
    }

    #endregion

    ///------------------------------
    /// <summary>
    /// 파라미터 데이터 설정
    /// </summary>
    ///------------------------------
    private void GetData()
    {
        try
        {
            strCallType = SiteGlobal.GetRequestForm("CallType");

            // 아이디 찾기
            strIdMobileNo    = Utils.IsNull(SiteGlobal.GetRequestForm("IdMobileNo"), "");
            strIdAdminCorpNo = Utils.IsNull(SiteGlobal.GetRequestForm("IdAdminCorpNo"), "");
            strIdAdminName   = Utils.IsNull(SiteGlobal.GetRequestForm("IdAdminName"), "");

            // 로그인 체크
            strAdminID  = Utils.IsNull(SiteGlobal.GetRequestForm("AdminID"), "");
            strAdminPwd = Utils.IsNull(SiteGlobal.GetRequestForm("AdminPwd"), "");

        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = "예기치 않은 오류가 발생하였습니다.";

            SiteGlobal.WriteLog("LoginHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    ///------------------------------
    /// <summary>
    /// 실행 메소드 처리함수
    /// </summary>
    ///------------------------------
    private void Process()
    {
        try
        {
            switch (strCallType)
            {
                case "chkLogin":
                    ChkAdminLogin();
                    break;
                default:
                    objResMap.RetCode = 9500;
                    objResMap.ErrMsg  = "Wrong Method" + strCallType;
                    break;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9403;
            objResMap.ErrMsg  = "예기치 않은 오류가 발생하였습니다.";

            SiteGlobal.WriteLog("LoginHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region IHttpHandler Process
    protected void ChkAdminLogin()
    {
        string lo_strErrMsg          = string.Empty;
        string lo_strLastLoginNotice = string.Empty;
        int    lo_intRetVal          = 99;
        bool   lo_intIsValid         = false; //결과 값
        int    lo_intGradeCode       = 0;

        ReqAdminList                lo_objReqAdminList = null;
        ServiceResult<ResAdminList> lo_objResAdminList = null;

        if (string.IsNullOrEmpty(strAdminID)) {
            objResMap.RetCode = 9001;
            objResMap.ErrMsg = "아이디를 입력해주세요.";
            return;
        }

        if (string.IsNullOrEmpty(strAdminPwd)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "아이디를 입력해주세요.";
            return;
        }

        try
        {
            // 관리자 비밀번호 체크
            objResMap.RetCode = PasswordManager.CheckEnteredPassword(strAdminID, strAdminPwd, out lo_intGradeCode, out lo_strErrMsg);
            if (objResMap.RetCode.IsFail())
            {
                // 비밀번호가 틀린 경우, 관리자 로그인 실패 로그 등록
                if (objResMap.RetCode.Equals(ErrorHandler.COMMON_LIB_ERR_22009))
                {
                    objAdminDBServices.InsAdminLoginFail(strAdminID, lo_strErrMsg);
                }

                objResMap.ErrMsg = lo_strErrMsg;
                return;
            }else{
                SetAdminLogin();
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = "예기치 않은 오류가 발생하였습니다.";

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    protected void SetAdminLogin()
    {
        string lo_strErrMsg          = string.Empty;
        string lo_strLastLoginNotice = string.Empty;
        int    lo_intRetVal          = 99;
        bool   lo_intIsValid         = false; //결과 값

        ServiceResult<AdminSessionInfo> lo_objResInsAdminSession = null;

        try
        {
            // 세션 생성
            lo_objResInsAdminSession = objAdminDBServices.InsAdminSession(strAdminID, SiteGlobal.GetRemoteAddr());

            objResMap.RetCode     = lo_objResInsAdminSession.result.ErrorCode;
            if (objResMap.RetCode.IsSuccess())
            {
                SiteSession lo_objSes = new SiteSession();
                ConnectGlobal lo_objDB = null;
                lo_objSes.SiteSessionSub4NoSession(ref lo_objDB, lo_objResInsAdminSession.data.SessionKey);
                lo_strLastLoginNotice = $"\n최종 접속일시 : {lo_objResInsAdminSession.data.LastLoginDate:yyyy-MM-dd HH:mm:ss}"
                                       + $"<br/>최종 접속 IP : {lo_objResInsAdminSession.data.LastLoginIP}"
                                       + $"<br/>계정 만료일 : {Utils.ConvertDateFormat(lo_objSes.ExpireYmd)}"
                                       + $"<br/><br/>안녕하세요. {lo_objSes.AdminName}님은 현재 EasyCC 시스템을 이용하고 있습니다.";
                objResMap.Add("LastLoginNotice", lo_strLastLoginNotice);
            }
            else
            {
                objResMap.ErrMsg = lo_objResInsAdminSession.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9405;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("LoginHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }
    #endregion


    ///--------------------------------------------
    /// <summary>
    /// 페이지 기본 Json 응답 출력
    /// </summary>
    ///--------------------------------------------
    public override void WriteJsonResponse(string strLogFileName)
    {
        try
        {
            base.WriteJsonResponse(strLogFileName);
        }
        catch
        {
        }
    }
}