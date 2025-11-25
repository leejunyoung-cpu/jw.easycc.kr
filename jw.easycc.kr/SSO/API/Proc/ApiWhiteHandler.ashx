<%@ WebHandler Language="C#" Class="ApiWhiteHandler" %>
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
/// FileName        : ApiWhiteHandler.ashx
/// Description     : 허용 IP 등록
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2025-11-25
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class ApiWhiteHandler : AshxBaseHandler
{
    //상수 선언
    private const string CurrentMenuLink = "/SSO/API/ApiWhiteList"; //필수

    // 메소드 리스트
    private const string MethodApiWhiteList      = "ApiWhiteList";
    private const string MethodApiWhiteUpd      = "ApiWhiteUpd";
    private const string MethodApiWhiteIns      = "ApiWhiteIns";
    private const string MethodApiWhiteDel      = "ApiWhiteDel";

    ApiService        objApiService      = new ApiService();

    private string strCallType         = string.Empty;
    private int    intPageSize         = 0;
    private int    intPageNo           = 0;

    private string strSeqNo         = string.Empty;
    private string strServiceCode   = string.Empty;
    private string strCompany       = string.Empty;
    private string strIsActive      = string.Empty;
    private string strServiceUrl    = string.Empty;
    private string strIpAddress     = string.Empty;
    private string strIpNote        = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodApiWhiteList,      MenuAuthType.ReadOnly);
        objMethodAuthList.Add(MethodApiWhiteUpd,       MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodApiWhiteIns,       MenuAuthType.ReadWrite);
        objMethodAuthList.Add(MethodApiWhiteDel,       MenuAuthType.ReadWrite);

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
            strSeqNo                = Utils.IsNull(SiteGlobal.GetRequestForm("SeqNo"), "0");
            strServiceCode          = Utils.IsNull(SiteGlobal.GetRequestForm("ServiceCode"), "");
            strCompany              = Utils.IsNull(SiteGlobal.GetRequestForm("Company"), "");
            strIsActive             = Utils.IsNull(SiteGlobal.GetRequestForm("IsActive"), "");
            strServiceUrl           = Utils.IsNull(SiteGlobal.GetRequestForm("ServiceUrl"), "");
            strIpAddress            = Utils.IsNull(SiteGlobal.GetRequestForm("IpAddress"), "");
            strIpNote               = Utils.IsNull(SiteGlobal.GetRequestForm("IpNote"), "");

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
                case MethodApiWhiteList:
                    GetApiWhiteList();
                    break;
                case MethodApiWhiteUpd:
                    SetApiWhiteUpd();
                    break;
                case MethodApiWhiteIns:
                    SetApiWhiteInsert();
                    break;
                case MethodApiWhiteDel:
                    SetApiWhiteDel();
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

    protected void GetApiWhiteList()
    {
        ReqApiIpWhiteList                lo_objReqApiIpWhiteList = null;
        ServiceResult<ResApiIpWhiteList> lo_objResApiIpWhiteList = null;

        try
        {
            lo_objReqApiIpWhiteList = new ReqApiIpWhiteList
            {
                SeqNo               = strSeqNo.ToInt(),
                ServiceCode         = strServiceCode,
                Company             = strCompany,
                IsActive            = strIsActive,
                ServiceUrl          = strServiceUrl,
                PageSize            = intPageSize,
                PageNo              = intPageNo
            };

            lo_objResApiIpWhiteList    = objApiService.GetApiIpWhiteList(lo_objReqApiIpWhiteList);
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

    protected void SetApiWhiteUpd()
    {
        ApiIpWhiteViewModel     lo_objReqApiIpWhiteViewModel    = null;
        ServiceResult<bool>     lo_objResult                    = null;
        string                  lo_strClientCode                = string.Empty;
        string                  lo_strFileName                  = string.Empty;

        if (string.IsNullOrEmpty(strSeqNo) || strSeqNo.Equals("0")) {
            objResMap.RetCode = 9001;
            objResMap.ErrMsg = "필요한 값이 없습니다.(일련번호)";
            return;
        }

        if (string.IsNullOrEmpty(strServiceCode)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "필요한 값이 없습니다.(서비스코드)";
            return;
        }

        if (string.IsNullOrEmpty(strCompany)) {
            objResMap.RetCode = 9003;
            objResMap.ErrMsg = "필요한 값이 없습니다.(업체코드)";
            return;
        }

        if (string.IsNullOrEmpty(strIpAddress)) {
            objResMap.RetCode = 9004;
            objResMap.ErrMsg = "필요한 값이 없습니다.(IP주소)";
            return;
        }

        if (string.IsNullOrEmpty(strIsActive)) {
            objResMap.RetCode = 9004;
            objResMap.ErrMsg = "필요한 값이 없습니다.(활성화여부)";
            return;
        }

        try
        {
            lo_objReqApiIpWhiteViewModel = new ApiIpWhiteViewModel() {
                SeqNo           = strSeqNo.ToInt(),
                ServiceCode     = strServiceCode,
                Company         = strCompany,
                IpAddress       = strIpAddress,
                IsActive        = strIsActive,
                ServiceUrl      = strServiceUrl,
                IpNote          = strIpNote
            };

            lo_objResult        = objApiService.SetApiIpWhiteUpd(lo_objReqApiIpWhiteViewModel);
            objResMap.RetCode   = lo_objResult.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResult.result.ErrorMsg;
                return;
            }

            objResMap.Add("SeqNo", strSeqNo);
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9405;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally {

        }
    }

    protected void SetApiWhiteInsert()
    {
        ApiIpWhiteViewModel                 lo_objApiIpWhiteViewModel   = null;
        ServiceResult<ApiIpWhiteViewModel>  lo_objResult            = null;

        if (string.IsNullOrEmpty(strSeqNo) || strSeqNo.Equals("0")) {
            objResMap.RetCode = 9001;
            objResMap.ErrMsg = "필요한 값이 없습니다.(일련번호)";
            return;
        }

        if (string.IsNullOrEmpty(strServiceCode)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "필요한 값이 없습니다.(서비스코드)";
            return;
        }

        if (string.IsNullOrEmpty(strCompany)) {
            objResMap.RetCode = 9003;
            objResMap.ErrMsg = "필요한 값이 없습니다.(업체코드)";
            return;
        }

        if (string.IsNullOrEmpty(strIpAddress)) {
            objResMap.RetCode = 9004;
            objResMap.ErrMsg = "필요한 값이 없습니다.(IP주소)";
            return;
        }

        try
        {
            lo_objApiIpWhiteViewModel = new ApiIpWhiteViewModel() {
                ServiceCode     = strServiceCode,
                Company         = strCompany,
                IpAddress       = strIpAddress,
                ServiceUrl      = strServiceUrl,
                IpNote          = strIpNote
            };

            lo_objResult        = objApiService.SetApiIpWhiteIns(lo_objApiIpWhiteViewModel);
            objResMap.RetCode   = lo_objResult.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResult.result.ErrorMsg;
                return;
            }

            objResMap.Add("SeqNo", lo_objResult.data.SeqNo);
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9405;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ApiWhiteHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally {

        }
    }

    protected void SetApiWhiteDel()
    {
        ApiIpWhiteViewModel                 lo_objApiIpWhiteViewModel   = null;
        ServiceResult<bool>                 lo_objResult                = null;

        if (string.IsNullOrEmpty(strSeqNo) || strSeqNo.Equals("0")) {
            objResMap.RetCode = 9001;
            objResMap.ErrMsg = "필요한 값이 없습니다.(일련번호)";
            return;
        }
        try
        {
            lo_objApiIpWhiteViewModel = new ApiIpWhiteViewModel()
            {
                SeqNo = strSeqNo.ToInt()
            };

            lo_objResult = objApiService.SetApiIpWhiteDel(lo_objApiIpWhiteViewModel);
            
            objResMap.RetCode   = lo_objResult.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResult.result.ErrorMsg;
                return;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9406;
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