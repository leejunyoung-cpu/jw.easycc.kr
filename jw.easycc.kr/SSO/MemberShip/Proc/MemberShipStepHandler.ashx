<%@ WebHandler Language="C#" Class="MemberShipStepHandler" %>
using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Web;

///================================================================
/// <summary>
/// FileName        : MemberShipStepHandler.ashx
/// Description     : 상하차지 관련
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2024-07-17
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class MemberShipStepHandler : AshxBaseHandler
{
    ClientService       objClientService        = new ClientService();
    AdminService        objAdminService         = new AdminService();
    AdminMenuService    objAdminMenuService         = new AdminMenuService();

    private string      strCallType         = string.Empty;
    private int         intPageSize         = 0;
    private int         intPageNo           = 0;

    private string      strClientCode           = string.Empty;
    private string      strCorpNo               = string.Empty;
    private string      strClientName           = string.Empty;
    private string      strAdminID              = string.Empty;
    private string      strAdminName            = string.Empty;
    private string      strTelNo                = string.Empty;
    private string      strMobileNo             = string.Empty;
    private string      strEmail                = string.Empty;
    private string      strAdminPwd             = string.Empty;
    private string      strPrivateAvailFlag     = string.Empty;

    #region IHttpHandler Members
    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
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
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipStepHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("MemberShipStepHandler");
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
            strCallType         = Utils.IsNull(SiteGlobal.GetRequestForm("CallType"), "");
            strClientCode       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientCode"), "0");
            strCorpNo           = Utils.IsNull(SiteGlobal.GetRequestForm("CorpNo"), "");
            strClientName       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientName"), "");
            strAdminID          = Utils.IsNull(SiteGlobal.GetRequestForm("AdminID"), "");
            strAdminName        = Utils.IsNull(SiteGlobal.GetRequestForm("AdminName"), "");
            strTelNo            = Utils.IsNull(SiteGlobal.GetRequestForm("TelNo"), "");
            strMobileNo         = Utils.IsNull(SiteGlobal.GetRequestForm("MobileNo"), "");
            strEmail            = Utils.IsNull(SiteGlobal.GetRequestForm("Email"), "");
            strAdminPwd         = Utils.IsNull(SiteGlobal.GetRequestForm("AdminPwd"), "");
            strPrivateAvailFlag         = Utils.IsNull(SiteGlobal.GetRequestForm("PrivateAvailFlag"), "");
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipStepHandler", "Exception",
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
                case "ClientCorpNoChk":
                    GetClientCorpNoChk();
                    break;
                case "AdminIDCheck":
                    GetAdminIDCheckCheck();
                    break;
                case "CheckAdminPwdPolicy":
                    CheckAdminPwdPolicy();
                    break;
                case "AdminInsert":
                    GetAdminInsert();
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

            SiteGlobal.WriteLog("MemberShipStepHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region IHttpHandler Process

    /// <summary>
    /// 고객사 사업자 중복 체크
    /// </summary>
    protected void GetClientCorpNoChk()
    {
        ReqClientList                lo_objReqClientList = null;
        ServiceResult<ResClientList> lo_objResClientList = null;

        if (string.IsNullOrWhiteSpace(strCorpNo))
        {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg  = "필요한 값이 없습니다.";
            return;
        }

        // 중복 사업자 체크
        try
        {
            lo_objReqClientList = new ReqClientList
            {
                CorpNo           = strCorpNo,
                PageSize         = intPageSize,
                PageNo           = intPageNo
            };

            lo_objResClientList   = objClientService.GetClientList(lo_objReqClientList);

            if (lo_objResClientList.data.RecordCnt.Equals(0))
            {
                objResMap.RetCode = 9003;
                objResMap.ErrMsg = "등록된 고객사정보가 없습니다.<br>가입을 원하실 경우 EasyCC 커텍트 관리자에게 문의해주세요.";
                return;
            }
            else {
                objResMap.Add("ClientCeoName",   lo_objResClientList.data.list[0].ClientCeoName);
                objResMap.Add("ClientName",   lo_objResClientList.data.list[0].ClientName);
                objResMap.Add("ClientCode",   lo_objResClientList.data.list[0].ClientCode);
                objResMap.Add("CorpNo",   lo_objResClientList.data.list[0].CorpNo);
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipStepHandler", "Exception",
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
                AdminID  = strAdminID,
                SesGradeCode = 1,
                PageSize = 1,
                PageNo   = 1
            };

            lo_objResAdminList = objAdminService.GetAdminList(lo_objReqAdminList);
            objResMap.RetCode = lo_objResAdminList.result.ErrorCode;
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
                objResMap.ErrMsg  = "이미 등록된 아이디입니다.";
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipInfoHandler", "Exception",
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
            lo_intRetVal = PasswordManager.ValidatePasswordPolicy(strAdminPwd, out lo_strErrMsg);
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

    protected void GetAdminInsert() {
        AdminViewModel              lo_objReqInsAdmin  = null;
        AdminMenuAccessViewModel    lo_objReqAdminMenuAccessViewModel  = null;
        ServiceResult<bool>         lo_objResInsAdmin  = null;
        BCrypt                      lo_objBCrypt            = new BCrypt();

        try
        {
            if (string.IsNullOrWhiteSpace(strClientCode) || strClientCode.Equals("0"))
            {
                objResMap.RetCode = 9001;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            if (string.IsNullOrWhiteSpace(strAdminID))
            {
                objResMap.RetCode = 9003;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            if (string.IsNullOrWhiteSpace(strAdminName))
            {
                objResMap.RetCode = 9005;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            if (string.IsNullOrWhiteSpace(strMobileNo))
            {
                objResMap.RetCode = 9006;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            if (string.IsNullOrWhiteSpace(strEmail))
            {
                objResMap.RetCode = 9007;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            if (string.IsNullOrWhiteSpace(strAdminPwd))
            {
                objResMap.RetCode = 9008;
                objResMap.ErrMsg  = "필요한 값이 없습니다.";
                return;
            }

            lo_objReqInsAdmin = new AdminViewModel
            {
                AdminID          = strAdminID,
                AdminPWD         = lo_objBCrypt.HashPassword(strAdminPwd, lo_objBCrypt.GenerateSaltByRandom()),
                MobileNo         = strMobileNo,
                AdminName        = strAdminName,
                GradeCode        = 7,
                TelNo            = strTelNo,
                Email            = strEmail,
                PrivateAvailFlag = strPrivateAvailFlag,
                AccessClientCode = strClientCode,
                UseFlag          = "D",
                RegAdminID       = strAdminID,
            };

            lo_objResInsAdmin = objAdminService.InsAdmin(lo_objReqInsAdmin);
            objResMap.RetCode = lo_objResInsAdmin.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResInsAdmin.result.ErrorMsg;
            }

            if (objResMap.RetCode.Equals(0)) {
                lo_objReqAdminMenuAccessViewModel = new AdminMenuAccessViewModel
                {
                    AdminID = strAdminID,
                    AccessTypeCode = 2,
                    AddMenuList = SiteGlobal.MEMBERSHIP_MENU_ROLE_NO.ToString(),
                    AllAuthCode = "1"

                };
                lo_objResInsAdmin = objAdminMenuService.InsMenuAccessAdmin(lo_objReqAdminMenuAccessViewModel);
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9604;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("MemberShipStepHandler", "Exception",
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