<%@ WebHandler Language="C#" Class="AdminHandler" %>
using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Data;
using System.IO;
using System.Web;
using Newtonsoft.Json;

///================================================================
/// <summary>
/// FileName        : AdminHandler.ashx
/// Description     : 어드민 관련 Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2024-07-10
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class AdminHandler : AshxBaseHandler
{
    //상수 선언
    private const string CurrentMenuLink = "/SSO/Admin/AdminList"; //필수

    // 메소드 리스트
    private const string MethodAdminList       = "AdminList";
    private const string MethodAdminListExcel  = "AdminListExcel";
    private const string MethodAdminInsert     = "AdminInsert";
    private const string MethodAdminUpdate     = "AdminUpdate";
    private const string MethodAdminLoginReset = "AdminLoginReset";
    private const string MethodAdminSendPwd    = "AdminSendPwd";
    private const string MethodAdminIDCheck    = "AdminIDCheck";
    AdminService     objAdminService   = new AdminService();

    private string strCallType         = string.Empty;
    private int    intPageSize         = 0;
    private int    intPageNo           = 0;
    private int    intGradeCode        = 0;
    private string strUseFlag          = string.Empty;
    private string strSearchType       = string.Empty;
    private string strListSearch       = string.Empty;
    private string strAdminID          = string.Empty;
    private string strAdminPwd         = string.Empty;
    private string strMobileNo         = string.Empty;
    private string strAdminName        = string.Empty;
    private string strTelNo            = string.Empty;
    private string strEmail            = string.Empty;
    private string strLastLoginDate    = string.Empty;
    private string strLastLoginIP      = string.Empty;
    private string strJoinYMD          = string.Empty;
    private string strExpireYMD        = string.Empty;
    private string strPwdUpdDate       = string.Empty;
    private string strClientCode       = string.Empty;
    private string strAccessClientCode = string.Empty;
    private string strAdminResetPwd    = string.Empty;
    private string strPrivateAvailFlag = string.Empty;
    private string strDeptName         = string.Empty;
    private string strBtoBYN           = string.Empty;
    private string strBtoCYN           = string.Empty;
    private string strAdminAreaType    = string.Empty;
    private string strGradeCode        = string.Empty;
    private string strUpsCsType        = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodAdminList,       MenuAuthType.ReadOnly);
        objMethodAuthList.Add(MethodAdminListExcel,  MenuAuthType.All);
        objMethodAuthList.Add(MethodAdminInsert,     MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodAdminUpdate,     MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodAdminLoginReset, MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodAdminSendPwd,    MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodAdminIDCheck,    MenuAuthType.ReadWrite);

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
            intPageSize = Utils.IsNull(SiteGlobal.GetRequestForm("PageSize"), "0").ToInt();
            intPageNo   = Utils.IsNull(SiteGlobal.GetRequestForm("PageNo"), "0").ToInt();

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

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("AdminHandler");
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
            // Admin List
            intGradeCode        = Utils.IsNull(SiteGlobal.GetRequestForm("GradeCode"), "0").ToInt();
            strUseFlag          = SiteGlobal.GetRequestForm("UseFlag");
            strClientCode       = SiteGlobal.GetRequestForm("ClientCode");
            strAccessClientCode = SiteGlobal.GetRequestForm("AccessClientCode");
            strSearchType       = SiteGlobal.GetRequestForm("SearchType");
            strListSearch       = SiteGlobal.GetRequestForm("ListSearch");
            strAdminID          = SiteGlobal.GetRequestForm("AdminID");
            strAdminPwd         = SiteGlobal.GetRequestForm("AdminPwd");
            strMobileNo         = SiteGlobal.GetRequestForm("MobileNo");
            strAdminName        = SiteGlobal.GetRequestForm("AdminName");
            strTelNo            = SiteGlobal.GetRequestForm("TelNo");
            strEmail            = SiteGlobal.GetRequestForm("Email");
            strLastLoginDate    = SiteGlobal.GetRequestForm("LastLoginDate");
            strLastLoginIP      = SiteGlobal.GetRequestForm("LastLoginIP");
            strJoinYMD          = SiteGlobal.GetRequestForm("JoinYMD");
            strExpireYMD        = SiteGlobal.GetRequestForm("ExpireYMD");
            strPwdUpdDate       = SiteGlobal.GetRequestForm("PwdUpdDate");
            strAdminResetPwd    = Utils.IsNull(SiteGlobal.GetRequestForm("AdminResetPwd"),    string.Empty);
            strPrivateAvailFlag = Utils.IsNull(SiteGlobal.GetRequestForm("PrivateAvailFlag"), "N");
            strDeptName         = Utils.IsNull(SiteGlobal.GetRequestForm("DeptName"), "");
            strBtoBYN           = Utils.IsNull(SiteGlobal.GetRequestForm("BtoBYN"), "N");
            strBtoCYN           = Utils.IsNull(SiteGlobal.GetRequestForm("BtoCYN"), "N");
            strAdminAreaType    = Utils.IsNull(SiteGlobal.GetRequestForm("AdminAreaType"), "99");
            strGradeCode        = Utils.IsNull(SiteGlobal.GetRequestForm("GradeCode"), "0");
            strUpsCsType        = Utils.IsNull(SiteGlobal.GetRequestForm("UpsCsType"), "0");
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
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
                case MethodAdminList:
                    GetAdminList();
                    break;
                case MethodAdminSendPwd:
                    SendPwdAdmin();
                    break;
                case MethodAdminLoginReset:
                    ResetLoginAdmin();
                    break;
                case MethodAdminUpdate:
                    UpdAdmin();
                    break;
                case MethodAdminInsert:
                    InsAdmin();
                    break;
                case MethodAdminIDCheck:
                    GetAdminIDCheckCheck();
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

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region Handler Process
    protected void GetAdminList()
    {
        string lo_strAdminID   = string.Empty;
        string lo_strAdminName = string.Empty;
        string lo_strMobileNo  = string.Empty;

        ReqAdminList                lo_objReqAdminList = null;
        ServiceResult<ResAdminList> lo_objResAdminList = null;

        try
        {
            switch (strSearchType)
            {
                case "AdminID":
                    lo_strAdminID = strListSearch;
                    break;
                case "AdminName":
                    lo_strAdminName = strListSearch;
                    break;
                case "MobileNo":
                    lo_strMobileNo = strListSearch;
                    break;
            }

            lo_objReqAdminList = new ReqAdminList
            {
                AdminID          = lo_strAdminID,
                MobileNo         = lo_strMobileNo,
                AdminName        = lo_strAdminName,
                AccessClientCode = objSes.AccessClientCode,
                UseFlag          = strUseFlag,
                GradeCode        = strGradeCode.ToInt(),
                SesGradeCode     = objSes.GradeCode,
                PageSize         = intPageSize,
                PageNo           = intPageNo
            };

            lo_objResAdminList    = objAdminService.GetAdminList(lo_objReqAdminList);
            objResMap.strResponse = "[" + JsonConvert.SerializeObject(lo_objResAdminList) + "]";
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    /// <summary>
    /// 신규 비밀번호 전송
    /// </summary>
    protected void SendPwdAdmin()
    {
        bool lo_result = false;
        string              lo_strEncAdminResetPwd = string.Empty;
        int                 lo_intRetVal           = 0;
        ServiceResult<bool> lo_objUpdAdminPwd      = null;
        BCrypt              lo_objBCrypt           = new BCrypt();
        string              lo_strEmailTitle      = string.Empty;
        string              lo_strEmailContents      = string.Empty;

        if (string.IsNullOrWhiteSpace(strAdminID))
        {
            objResMap.RetCode = 9903;
            objResMap.ErrMsg  = "필요한 값이 없습니다.";
            return;
        }

        try
        {
            if (string.IsNullOrEmpty(strEmail))
            {
                objResMap.RetCode = 9551;
                objResMap.ErrMsg  = "휴대폰 번호가 없습니다.";
                return;
            }

            strAdminResetPwd = Guid.NewGuid().ToString().Right(8).Replace("-", "");

            lo_strEncAdminResetPwd = lo_objBCrypt.HashPassword(strAdminResetPwd, lo_objBCrypt.GenerateSaltByRandom());

            lo_objUpdAdminPwd = objAdminService.UpdAdminPwd(strAdminID, lo_strEncAdminResetPwd, objSes.AdminID, 2);
            lo_intRetVal         = lo_objUpdAdminPwd.result.ErrorCode;
            if (lo_intRetVal.IsFail())
            {
                objResMap.RetCode = lo_intRetVal;
                objResMap.ErrMsg  = lo_objUpdAdminPwd.result.ErrorMsg;
                return;
            }

            lo_strEmailTitle = $"[EasyCC 커넥트] {strAdminID}님의 임시패스워드 입니다.";
            lo_strEmailContents = $"[EasyCC 커넥트] {strAdminID}님의 임시패스워드는 '{strAdminResetPwd}' 입니다.\n";
            lo_strEmailContents += $"시스템 접속 후 내정보에서 패스워드를 꼭 변경하세요.";

            lo_result = SiteGlobal.SendMail(CommonConstant.ADMIN_FROM_EMAIL, strEmail, lo_strEmailTitle, lo_strEmailContents, false, "");

            if (lo_result)
            {
                objResMap.RetCode = lo_intRetVal;
                objResMap.ErrMsg = strEmail + " 메일로 임시패스워드를 발송하였습니다.";
            }
            else { 
                objResMap.RetCode = 9005;
                objResMap.ErrMsg = "메일 발송에 실패하였습니다.";
            }

        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    /// <summary>
    /// 접속 초기화
    /// </summary>
    protected void ResetLoginAdmin()
    {
        int                 lo_intRegReqType         = 0;
        ServiceResult<bool> lo_objResResetLoginAdmin = null;

        if (string.IsNullOrWhiteSpace(strAdminID))
        {
            objResMap.RetCode = 9903;
            objResMap.ErrMsg  = "필요한 값이 없습니다.";
            return;
        }

        try
        {
            lo_objResResetLoginAdmin = objAdminService.ResetLoginAdmin(strAdminID);
            objResMap.RetCode        = lo_objResResetLoginAdmin.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResResetLoginAdmin.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }
    
    protected void InsAdmin()
    {
        int                 lo_intRegReqType   = 0;
        AdminViewModel      lo_objReqInsAdmin  = null;
        ServiceResult<bool> lo_objResInsAdmin  = null;

        try
        {
            lo_objReqInsAdmin = new AdminViewModel
            {
                AdminID          = strAdminID,
                AdminPWD         = strAdminPwd,
                MobileNo         = strMobileNo,
                AdminName        = strAdminName,
                GradeCode        = intGradeCode,
                Email            = strEmail,
                DeptName         = strDeptName,
                TelNo            = strTelNo,
                ExpireYMD        = strExpireYMD.Replace("-", ""),
                BtoBYN           = strBtoBYN,
                BtoCYN           = strBtoCYN,
                AdminAreaType    = strAdminAreaType.ToInt(),
                UpsCsType        = strUpsCsType.ToInt(),
                PwdUpdDate       = strPwdUpdDate,
                AccessClientCode = strAccessClientCode,
                UseFlag          = strUseFlag,
                RegAdminID       = objSes.AdminID,
                PrivateAvailFlag = strPrivateAvailFlag
            };

            lo_objResInsAdmin = objAdminService.InsAdmin(lo_objReqInsAdmin);
            objResMap.RetCode = lo_objResInsAdmin.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResInsAdmin.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    protected void UpdAdmin()
    {
        int                 lo_intRegReqType   = 0;
        AdminViewModel      lo_objReqUpdAdmin  = null;
        ServiceResult<bool> lo_objResUpdAdmin  = null;

        try
        {
            switch (intGradeCode)
            {
                case 6:
                    lo_intRegReqType   = 2;
                    break;
                default:
                    lo_intRegReqType   = 1;
                    break;
            }

            lo_objReqUpdAdmin = new AdminViewModel
            {
                AdminID          = strAdminID,
                MobileNo         = strMobileNo,
                AdminName        = strAdminName,
                GradeCode        = intGradeCode,
                TelNo            = strTelNo,
                DeptName         = strDeptName,
                Email            = strEmail,
                ExpireYMD        = strExpireYMD.Replace("-", ""),
                BtoBYN           = strBtoBYN,
                BtoCYN           = strBtoCYN,
                AdminAreaType    = strAdminAreaType.ToInt(),
                UpsCsType        = strUpsCsType.ToInt(),
                AccessClientCode = strAccessClientCode,
                UseFlag          = strUseFlag,
                UpdAdminID       = objSes.AdminID,
                PrivateAvailFlag = strPrivateAvailFlag
            };

            lo_objResUpdAdmin = objAdminService.UpdAdmin(lo_objReqUpdAdmin);
            objResMap.RetCode = lo_objResUpdAdmin.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResUpdAdmin.result.ErrorMsg;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }
    
    protected void GetAdminIDCheckCheck()
    {
        ReqAdminList                lo_objReqAdminList = null;
        ServiceResult<ResAdminList> lo_objResAdminList = null;

        try
        {
            lo_objReqAdminList = new ReqAdminList
            {
                AdminID      = strAdminID,
                MobileNo     = strMobileNo,
                SesGradeCode = 1,
                PageSize     = 1,
                PageNo       = 1
            };

            lo_objResAdminList = objAdminService.GetAdminList(lo_objReqAdminList);
            objResMap.RetCode  = lo_objResAdminList.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResAdminList.result.ErrorMsg;
                return;
            }

            if (lo_objResAdminList.data.RecordCnt.Equals(0))
            {
                objResMap.RetCode = 0;
                objResMap.ErrMsg  = "등록가능";
            }
            else
            {
                objResMap.RetCode = 9460;
                objResMap.ErrMsg  = lo_objResAdminList.data.list[0].AdminID;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("AdminHandler", "Exception",
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