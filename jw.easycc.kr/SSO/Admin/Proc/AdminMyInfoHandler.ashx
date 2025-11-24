<%@ WebHandler Language="C#" Class="AdminMyInfoHandler" %>
using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Web;
using Newtonsoft.Json;
///================================================================
/// <summary>
/// FileName        : AdminMyInfoHandler.ashx
/// Description     : 내정보 관련 Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljyh@jwccs.com, 2024-07-17
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class AdminMyInfoHandler : AshxBaseHandler
{
    //상수 선언
    private const string CurrentMenuLink = "/SSO/Admin/AdminMyInfo"; //필수

    // 메소드 리스트
    private const string MethodAdminMyInfoUpdate    = "AdminMyInfoUpdate";
    private const string MethodUpdAdminPwd          = "UpdAdminPwd";
    private const string MethodCheckAdminPwdPolicy  = "CheckAdminPwdPolicy";

    AdminService objAdminService            = new AdminService();
    ClientService objClientService          = new ClientService();

    private string strCallType      = string.Empty;

    private string strAdminID          = string.Empty;
    private string strMobileNo         = string.Empty;
    private string strDeptName         = string.Empty;
    private string strTelNo            = string.Empty;
    private string strNetwork24DDID    = string.Empty;
    private string strNetworkHMMID     = string.Empty;
    private string strNetworkOneCallID = string.Empty;
    private string strNetworkHmadangID = string.Empty;
    private string strEmail            = string.Empty;
    private string strUseFlag          = string.Empty;
    private string strAdminResetPwd    = string.Empty;
    private string strAdminPosition    = string.Empty;
    private string strHidAuthCode      = string.Empty;
    private string strSmsAuthNo        = string.Empty;
    private string strHidEncCode       = string.Empty;
    private string strOrgAdminPwd      = string.Empty;
    private string strCenterCode       = string.Empty;
    private string strClientCode       = string.Empty;
    private string strSeqNo            = string.Empty;
    private string strClientName       = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodAdminMyInfoUpdate,      MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodUpdAdminPwd,            MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodCheckAdminPwdPolicy,    MenuAuthType.ReadWrite);

        //# 호출 페이지 링크 지정
        SetMenuLink(CurrentMenuLink);

        base.ProcessRequest(context);
        if(base.IsHandlerStop.Equals(true))
        {
            return;
        }

        try
        {
            strCallType = SiteGlobal.GetRequestForm("CallType");

            //1.Request
            GetData();
            if (!objResMap.RetCode.Equals(0))
            {
                return;
            }

            //2.처리
            Process();
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9401;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminMenuHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("AdminMenuHandler");
        }
    }

    ///------------------------------
    /// <summary>
    /// 파라미터 데이터 설정
    /// </summary>
    ///------------------------------
    private void GetData()
    {
        try
        {
            // AdminMyInfo List
            strAdminID          = SiteGlobal.GetRequestForm("AdminID");
            strMobileNo         = SiteGlobal.GetRequestForm("MobileNo");
            strDeptName         = Utils.IsNull(SiteGlobal.GetRequestForm("DeptName"),         "");
            strTelNo            = Utils.IsNull(SiteGlobal.GetRequestForm("TelNo"),            "");
            strNetwork24DDID    = Utils.IsNull(SiteGlobal.GetRequestForm("Network24DDID"),    "");
            strNetworkHMMID     = Utils.IsNull(SiteGlobal.GetRequestForm("NetworkHMMID"),     "");
            strNetworkOneCallID = Utils.IsNull(SiteGlobal.GetRequestForm("NetworkOneCallID"), "");
            strNetworkHmadangID = Utils.IsNull(SiteGlobal.GetRequestForm("NetworkHmadangID"), "");
            strEmail            = Utils.IsNull(SiteGlobal.GetRequestForm("Email"),            "");
            strUseFlag          = SiteGlobal.GetRequestForm("UseFlag");
            strAdminResetPwd    = Utils.IsNull(SiteGlobal.GetRequestForm("AdminResetPwd"), "");
            strAdminPosition    = Utils.IsNull(SiteGlobal.GetRequestForm("AdminPosition"), "");
            strHidAuthCode      = Utils.IsNull(SiteGlobal.GetRequestForm("HidAuthCode"),   "");
            strSmsAuthNo        = Utils.IsNull(SiteGlobal.GetRequestForm("SmsAuthNo"),     "");
            strHidEncCode       = Utils.IsNull(SiteGlobal.GetRequestForm("HidEncCode"),    "");
            strOrgAdminPwd      = Utils.IsNull(SiteGlobal.GetRequestForm("OrgAdminPwd"),   "");
            strCenterCode       = Utils.IsNull(SiteGlobal.GetRequestForm("CenterCode"),    "0");
            strClientCode       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientCode"),    "0");
            strSeqNo            = Utils.IsNull(SiteGlobal.GetRequestForm("SeqNo"),         "0");
            strClientName       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientName"),    "");
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminMyInfoHandler", "Exception",
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
                case MethodAdminMyInfoUpdate:
                    UpdAdminMyInfo();
                    break;
                case MethodUpdAdminPwd:
                    UpdAdminPwd();
                    break;
                case MethodCheckAdminPwdPolicy:
                    CheckAdminPwdPolicy();
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
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminMyInfoHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region Handler Process


    protected void UpdAdminMyInfo()
    {
        AdminViewModel      lo_objReqUpdAdminMyInfo = null;
        ServiceResult<bool> lo_objResUpdAdminMyInfo = null;

        try
        {
            lo_objReqUpdAdminMyInfo = new AdminViewModel
            {
                AdminID          = strAdminID,
                TelNo            = strTelNo.Replace("-", ""),
                Email            = strEmail,
                DeptName         = strDeptName,
                UseFlag          = "Y"
            };

            lo_objResUpdAdminMyInfo = objAdminService.UpdAdminMyInfo(lo_objReqUpdAdminMyInfo);
            objResMap.RetCode       = lo_objResUpdAdminMyInfo.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResUpdAdminMyInfo.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminMyInfoHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    protected void UpdAdminPwd()
    {

        string              lo_strEncAdminResetPwd  = string.Empty;
        BCrypt              lo_objBCrypt            = new BCrypt();
        ServiceResult<bool> lo_objResDelAdminMyInfo = null;

        if (!lo_objBCrypt.CheckPassword(strOrgAdminPwd, strHidEncCode))
        {
            objResMap.RetCode = 9599;
            objResMap.ErrMsg = "현재 비밀번호가 일치하지 않습니다.";
            return;
        }

        try
        {
            lo_strEncAdminResetPwd = lo_objBCrypt.HashPassword(strAdminResetPwd, lo_objBCrypt.GenerateSaltByRandom());
            lo_objResDelAdminMyInfo = objAdminService.UpdAdminPwd(strAdminID, lo_strEncAdminResetPwd, strAdminID, 1);
            objResMap.RetCode       = lo_objResDelAdminMyInfo.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResDelAdminMyInfo.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.RetCode = 9404;

            SiteGlobal.WriteLog("AdminMyInfoHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    protected void CheckAdminPwdPolicy()
    {
        int      lo_intRetVal           = 0;
        string   lo_strErrMsg           = string.Empty;

        try
        {
            // Step 1 : 인증정보 체크
            lo_intRetVal = PasswordManager.ValidatePasswordPolicy(strAdminResetPwd, out lo_strErrMsg);
            if (lo_intRetVal.IsFail())
            {
                objResMap.RetCode = lo_intRetVal;
                objResMap.ErrMsg  = lo_strErrMsg;
                return;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9403;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipInfoHandler", "Exception",
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