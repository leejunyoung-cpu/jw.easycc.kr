<%@ WebHandler Language="C#" Class="ClientHandler" %>
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
/// FileName        : ClientHandler.ashx
/// Description     : 고객사 관련 Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2024-07-10
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class ClientHandler : AshxBaseHandler
{
    //상수 선언
    private const string CurrentMenuLink = "/SSO/Client/ClientList"; //필수

    // 메소드 리스트
    private const string MethodClientList        = "ClientList";
    private const string MethodClientUpdate      = "ClientUpdate";
    private const string MethodClientInsert      = "ClientInsert";

    ClientService     objClientService   = new ClientService();

    private string strCallType         = string.Empty;
    private int    intPageSize         = 0;
    private int    intPageNo           = 0;
    private string strUseFlag          = string.Empty;
    private string strSearchType       = string.Empty;
    private string strListSearch       = string.Empty;
    private string strClientCode       = string.Empty;
    private string strSiteID           = string.Empty;
    private string strClientName       = string.Empty;
    private string strClientCeoName    = string.Empty;
    private string strCorpNo           = string.Empty;
    private string strEmail            = string.Empty;
    private string strClientTel        = string.Empty;
    private string strAddr             = string.Empty;
    private string strRegAdminID       = string.Empty;
    private string strUpdAdminID       = string.Empty;
    private string strClientType       = string.Empty;
    private string strEncomFlag        = string.Empty;
    private string strBtoBYN           = string.Empty;
    private string strBtoCYN           = string.Empty;
    private string strClientLogoFile   = string.Empty;
    private string strHidFilePath      = string.Empty;
    private string strClientServerIP   = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodClientList,         MenuAuthType.ReadOnly);
        objMethodAuthList.Add(MethodClientUpdate,       MenuAuthType.ReadOnly);
        objMethodAuthList.Add(MethodClientInsert,       MenuAuthType.ReadOnly);

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

            SiteGlobal.WriteLog("ClientHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("ClientHandler");
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
            strSearchType       = SiteGlobal.GetRequestForm("SearchType");
            strListSearch       = SiteGlobal.GetRequestForm("ListSearch");
            strUseFlag          = Utils.IsNull(SiteGlobal.GetRequestForm("UseFlag"), "");
            strSearchType       = Utils.IsNull(SiteGlobal.GetRequestForm("SearchType"), "");
            strListSearch       = Utils.IsNull(SiteGlobal.GetRequestForm("ListSearch"), "");
            strClientCode       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientCode"), "0");
            strSiteID               = Utils.IsNull(SiteGlobal.GetRequestForm("SiteID"), "");
            strClientName           = Utils.IsNull(SiteGlobal.GetRequestForm("ClientName"), "");
            strClientCeoName        = Utils.IsNull(SiteGlobal.GetRequestForm("ClientCeoName"), "");
            strCorpNo               = Utils.IsNull(SiteGlobal.GetRequestForm("CorpNo"), "");
            strEmail                = Utils.IsNull(SiteGlobal.GetRequestForm("Email"), "");
            strClientTel            = Utils.IsNull(SiteGlobal.GetRequestForm("ClientTel"), "");
            strUseFlag              = Utils.IsNull(SiteGlobal.GetRequestForm("UseFlag"), "");
            strRegAdminID           = Utils.IsNull(SiteGlobal.GetRequestForm("RegAdminID"), "");
            strUpdAdminID           = Utils.IsNull(SiteGlobal.GetRequestForm("UpdAdminID"), "");
            strAddr                 = Utils.IsNull(SiteGlobal.GetRequestForm("Addr"), "");
            strClientType           = Utils.IsNull(SiteGlobal.GetRequestForm("ClientType"), "1");
            strEncomFlag            = Utils.IsNull(SiteGlobal.GetRequestForm("EncomFlag"), "Y");
            strBtoBYN               = Utils.IsNull(SiteGlobal.GetRequestForm("BtoBYN"), "N");
            strBtoCYN               = Utils.IsNull(SiteGlobal.GetRequestForm("BtoCYN"), "N");
            strHidFilePath          = Utils.IsNull(SiteGlobal.GetRequestForm("HidFilePath"), "");
            strClientServerIP       = Utils.IsNull(SiteGlobal.GetRequestForm("ClientServerIP"), "");

        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientHandler", "Exception",
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
                case MethodClientList:
                    GetClientList();
                    break;
                case MethodClientUpdate:
                    SetClientUpdate();
                    break;
                case MethodClientInsert:
                    SetClientInsert();
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

            SiteGlobal.WriteLog("ClientHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region Handler Process

    protected void GetClientList()
    {
        ReqClientList                lo_objReqClientList = null;
        ServiceResult<ResClientList> lo_objResClientList = null;

        try
        {
            lo_objReqClientList = new ReqClientList
            {
                ClientCode          = strClientCode.ToInt(),
                ClientName          = strClientName,
                ClientCeoName       = strClientCeoName,
                CorpNo              = strCorpNo,
                AdminID             = objSes.AdminID,
                UseFlag             = strUseFlag,
                PageSize            = intPageSize,
                PageNo              = intPageNo
            };

            lo_objResClientList    = objClientService.GetClientList(lo_objReqClientList);
            objResMap.strResponse = "[" + JsonConvert.SerializeObject(lo_objResClientList) + "]";
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9404;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    protected void SetClientUpdate()
    {
        ClientViewModel     lo_objClientViewModel   = null;
        ServiceResult<bool> lo_objResult            = null;
        string              lo_strClientCode        = string.Empty;
        string              lo_strFileName       = string.Empty;

        if (string.IsNullOrEmpty(strClientCode)) {
            objResMap.RetCode = 9001;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사코드)";
            return;
        }

        if (string.IsNullOrEmpty(strCorpNo)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "필요한 값이 없습니다.(사업자번호)";
            return;
        }

        if (string.IsNullOrEmpty(strSiteID)) {
            objResMap.RetCode = 9003;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사 고유번호)";
            return;
        }

        if (string.IsNullOrEmpty(strClientName)) {
            objResMap.RetCode = 9004;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사명)";
            return;
        }

        if (string.IsNullOrEmpty(strClientCeoName)) {
            objResMap.RetCode = 9005;
            objResMap.ErrMsg = "필요한 값이 없습니다.(대표자명)";
            return;
        }

        if (string.IsNullOrEmpty(strUseFlag)) {
            objResMap.RetCode = 9006;
            objResMap.ErrMsg = "필요한 값이 없습니다.(사용여부)";
            return;
        }

        lo_strFileName = HttpUtility.UrlDecode(strHidFilePath);

        lo_strClientCode = Utils.GetDecrypt(strClientCode);
        try
        {
            lo_objClientViewModel = new ClientViewModel() {
                ClientCode      = lo_strClientCode.ToInt(),
                SiteID          = strSiteID,
                ClientName      = strClientName,
                ClientCeoName   = strClientCeoName,
                Email           = strEmail,
                ClientTel       = strClientTel,
                Addr            = strAddr,
                ClientType      = strClientType.ToInt(),
                EncomFlag       = strEncomFlag,
                BtoBYN          = strBtoBYN,
                BtoCYN          = strBtoCYN,
                ClientLogoFile  = lo_strFileName,
                UseFlag         = strUseFlag,
                UpdAdminID      = objSes.AdminID,
                ClientServerIP  = strClientServerIP
            };

            lo_objResult        = objClientService.UpdClient(lo_objClientViewModel);
            objResMap.RetCode   = lo_objResult.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResult.result.ErrorMsg;
                return;
            }

            objResMap.Add("ClientCode", lo_strClientCode);
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9405;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally {

        }
    }

    protected void SetClientInsert()
    {
        ClientViewModel                 lo_objClientViewModel   = null;
        ServiceResult<ClientViewModel>  lo_objResult            = null;
        string                          lo_strClientCode        = string.Empty;
        string                          lo_strFileName          = string.Empty;
        lo_strFileName                                          = HttpUtility.UrlDecode(strHidFilePath);
        if (string.IsNullOrEmpty(strCorpNo)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "필요한 값이 없습니다.(사업자번호)";
            return;
        }

        if (string.IsNullOrEmpty(strSiteID)) {
            objResMap.RetCode = 9002;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사 고유번호)";
            return;
        }

        if (string.IsNullOrEmpty(strClientName)) {
            objResMap.RetCode = 9003;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사명)";
            return;
        }

        if (string.IsNullOrEmpty(strClientCeoName)) {
            objResMap.RetCode = 9004;
            objResMap.ErrMsg = "필요한 값이 없습니다.(대표자명)";
            return;
        }

        if (string.IsNullOrEmpty(strClientType)) {
            objResMap.RetCode = 9005;
            objResMap.ErrMsg = "필요한 값이 없습니다.(고객사 유형)";
            return;
        }

        if (string.IsNullOrEmpty(strUseFlag)) {
            objResMap.RetCode = 9006;
            objResMap.ErrMsg = "필요한 값이 없습니다.(사용여부)";
            return;
        }

        try
        {
            lo_objClientViewModel = new ClientViewModel() {
                SiteID          = strSiteID,
                ClientName      = strClientName,
                ClientCeoName   = strClientCeoName,
                CorpNo          = strCorpNo,
                Email           = strEmail,
                ClientTel       = strClientTel,
                Addr            = strAddr,
                ClientType      = strClientType.ToInt(),
                EncomFlag       = strEncomFlag,
                BtoBYN          = strBtoBYN,
                BtoCYN          = strBtoCYN,
                ClientLogoFile  = lo_strFileName,
                UseFlag         = strUseFlag,
                RegAdminID      = objSes.AdminID,
                ClientServerIP  = strClientServerIP
            };

            lo_objResult        = objClientService.InsClient(lo_objClientViewModel);
            objResMap.RetCode   = lo_objResult.result.ErrorCode;

            if (objResMap.RetCode.IsFail())
            {
                objResMap.ErrMsg = lo_objResult.result.ErrorMsg;
                return;
            }

            objResMap.Add("ClientCode", lo_objResult.data.ClientCode);
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9405;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally {

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