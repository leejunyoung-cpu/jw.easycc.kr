<%@ WebHandler Language="C#" Class="ApiHuaweiHandler" %>
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
/// FileName        : ApiHuaweiHandler.ashx
/// Description     : 고객사 관련 Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2024-07-10
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class ApiHuaweiHandler : AshxBaseHandler
{
    private const string CurrentMenuLink = "/API/Huawei/ApiHuaweiList"; //필수

    // 메소드 리스트
    private const string MethodApiApiHuaweiList      = "ApiHuaweiList";

    ApiService        objApiService      = new ApiService();

    private string strCallType         = string.Empty;
    private int    intPageSize         = 0;
    private int    intPageNo           = 0;

    private string strDateFrom        = string.Empty;
    private string strDateTo        = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodApiApiHuaweiList,      MenuAuthType.ReadOnly);

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

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("ApiWhiteHandler");
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
            strDateFrom             = Utils.IsNull(SiteGlobal.GetRequestForm("DateFrom").Replace("-", ""), "");
            strDateTo               = Utils.IsNull(SiteGlobal.GetRequestForm("DateTo").Replace("-", ""), "");
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
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
                case MethodApiApiHuaweiList:
                    GetApiApiHuaweiList();
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

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region Handler Process

    protected void GetApiApiHuaweiList()
    {
        ReqApiHuaweiList                lo_objReqApiIpWhiteList = null;
        ServiceResult<ResApiHuaweiList> lo_objResApiIpWhiteList = null;

        try
        {
            lo_objReqApiIpWhiteList = new ReqApiHuaweiList
            {
                DateFrom            = strDateFrom,
                DateTo              = strDateTo,
                PageSize            = intPageSize,
                PageNo              = intPageNo
            };

            lo_objResApiIpWhiteList    = objApiService.GetApiApiHuaweiList(lo_objReqApiIpWhiteList);
            objResMap.strResponse = "[" + JsonConvert.SerializeObject(lo_objResApiIpWhiteList) + "]";
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
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