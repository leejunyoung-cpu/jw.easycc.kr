<%@ WebHandler Language="C#" Class="DashBoardHandler" %>
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
/// FileName        : DashBoardHandler.ashx
/// Description     : 대시보드 관련
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2025-03-04
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class DashBoardHandler : AshxBaseHandler
{
    DashBoardService    objDashBoardService         = new DashBoardService();

    private string      strCallType         = string.Empty;
    private int         intPageSize         = 0;
    private int         intPageNo           = 0;

    private string      strReqYMD           = string.Empty;
    private string      strReqTime          = string.Empty;
    private string      strReqUserName      = string.Empty;
    private string      strReqUserTelNo     = string.Empty;
    private string      strReqUserEmail     = string.Empty;
    private string      strReqDetail        = string.Empty;
    private string      strReqType          = string.Empty;
    private string      strReqGubun         = string.Empty;

    #region IHttpHandler Members
    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        IgnoreCheckMenuAuth();
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

            SiteGlobal.WriteLog("DashBoardHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("DashBoardHandler");
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
            strReqYMD           = Utils.IsNull(SiteGlobal.GetRequestForm("ReqYMD").Replace("-", ""), "");
            strReqTime          = Utils.IsNull(SiteGlobal.GetRequestForm("ReqTime").Replace(":", ""), "");
            strReqUserName      = Utils.IsNull(SiteGlobal.GetRequestForm("ReqUserName"), "");
            strReqUserTelNo     = Utils.IsNull(SiteGlobal.GetRequestForm("ReqUserTelNo").Replace("-", ""), "");
            strReqUserEmail     = Utils.IsNull(SiteGlobal.GetRequestForm("ReqUserEmail"), "");
            strReqDetail        = Utils.IsNull(SiteGlobal.GetRequestForm("ReqDetail"), "");
            strReqType          = Utils.IsNull(SiteGlobal.GetRequestForm("ReqType"), "0");
            strReqGubun         = Utils.IsNull(SiteGlobal.GetRequestForm("ReqGubun"), "0");
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("DashBoardHandler", "Exception",
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
                case "ServiceReqBoardIns":
                    SetServiceReqInsert();
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

            SiteGlobal.WriteLog("DashBoardHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region IHttpHandler Process

    protected void SetServiceReqInsert() {
        ServiceReqBoardViewModel    lo_objReqServiceReqBoard    = null;
        ServiceResult<bool>         lo_objResServiceReqBoard    = null;

        try
        {
            if (string.IsNullOrWhiteSpace(strReqYMD))
            {
                objResMap.RetCode = 9001;
                objResMap.ErrMsg  = "필요한 값이 없습니다.(요청일)";
                return;
            }

            if (string.IsNullOrWhiteSpace(strReqYMD))
            {
                objResMap.RetCode = 9002;
                objResMap.ErrMsg  = "필요한 값이 없습니다.(요청시간)";
                return;
            }

            if (string.IsNullOrWhiteSpace(strReqUserName))
            {
                objResMap.RetCode = 9003;
                objResMap.ErrMsg  = "필요한 값이 없습니다.(요청담당자)";
                return;
            }

            if (string.IsNullOrWhiteSpace(strReqUserTelNo))
            {
                objResMap.RetCode = 9004;
                objResMap.ErrMsg  = "필요한 값이 없습니다.(연락처)";
                return;
            }

            if (string.IsNullOrWhiteSpace(strReqDetail))
            {
                objResMap.RetCode = 9005;
                objResMap.ErrMsg  = "필요한 값이 없습니다.(의뢰내용)";
                return;
            }

            if (string.IsNullOrWhiteSpace(objSes.AccessClientCode))
            {
                objResMap.RetCode = 9006;
                objResMap.ErrMsg  = "계정에 연결된 고객사가 없어 등록 할 수 없습니다.<br>담당자에게 문의하세요.";
                return;
            }

            lo_objReqServiceReqBoard = new ServiceReqBoardViewModel
            {
                AccessClientCode = objSes.AccessClientCode,
                ReqYMD           = strReqYMD,
                ReqTime          = strReqTime,
                ReqUserName      = strReqUserName,
                ReqUserTelNo     = strReqUserTelNo,

                ReqUserEmail     = strReqUserEmail,
                ReqType          = strReqType.ToInt(),
                ReqGubun         = strReqGubun.ToInt(),
                ReqDetail        = strReqDetail,
                RegAdminID       = objSes.AdminID
            };

            lo_objResServiceReqBoard = objDashBoardService.InsServiceReqBoard(lo_objReqServiceReqBoard);
            objResMap.RetCode = lo_objResServiceReqBoard.result.ErrorCode;
            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResServiceReqBoard.result.ErrorMsg;
            }

        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9604;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("DashBoardHandler", "Exception",
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