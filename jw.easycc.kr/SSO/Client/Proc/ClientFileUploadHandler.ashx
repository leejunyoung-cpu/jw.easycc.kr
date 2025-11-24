<%@ WebHandler Language="C#" Class="ClientFileUploadHandler" %>
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
using System.IO;

///================================================================
/// <summary>
/// FileName        : ClientFileUploadHandler.ashx
/// Description     : 고객사 관련 Process Handler
/// Special Logic
///   - Use Session Variables : NONE
///   - Use Server Component  : NONE
///
/// Author          : ljy@jwccs.com, 2024-07-10
/// Modify History  : Just Created.
/// </summary>
///================================================================
public class ClientFileUploadHandler : AshxBaseHandler
{
    //상수 선언
    private const string CurrentMenuLink = "/SSO/Client/ClientIns"; //필수

    // 메소드 리스트
    private const string MethodClientFileUpload         = "ClientFileUpload";
    private const string MethodClientFileDel            = "ClientFileDel";

    ClientService     objClientService   = new ClientService();

    private string strCallType         = string.Empty;
    private int    intPageSize         = 0;
    private int    intPageNo           = 0;
    private string strHidFilePath          = string.Empty;

    ///-------------------------------------------------------
    /// <summary>
    /// 최초실행 메소드 - 해당 메소드의 내용은 변경되지 않음!!
    /// </summary>
    ///-------------------------------------------------------
    public override void ProcessRequest(HttpContext context)
    {
        //# 메소드 별 필요한 메뉴 접근권한 정의
        objMethodAuthList.Add(MethodClientFileUpload,      MenuAuthType.ReadOnly);
        objMethodAuthList.Add(MethodClientFileDel,         MenuAuthType.ReadOnly);

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
            Process(context);
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9401;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientFileUploadHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
        finally
        {
            //3. 결과 출력 - 종료
            WriteJsonResponse("ClientFileUploadHandler");
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
            strHidFilePath       = HttpUtility.UrlDecode(SiteGlobal.GetRequestForm("HidFilePath"));
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9402;
            objResMap.ErrMsg  = CommonConstant.COMMON_EXCEPTION_MESSAGE;

            SiteGlobal.WriteLog("ClientFileUploadHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    ///------------------------------
    /// <summary>
    /// 실행 메소드 처리함수
    /// </summary>
    ///------------------------------
    private void Process(HttpContext context)
    {
        try
        {
            switch (strCallType)
            {
                case MethodClientFileUpload:
                    SetClientFileUpload(context);
                    break;
                case MethodClientFileDel:
                    SetClientFileDel();
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

            SiteGlobal.WriteLog("ClientFileUploadHandler", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                objResMap.RetCode);
        }
    }

    #region Handler Process

    /// <summary>
    /// 파일 업로드
    /// </summary>
    private void SetClientFileUpload(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        string         lo_strExtension      = string.Empty;
        string         lo_strFileName       = string.Empty;
        string         lo_strFileNameNew    = string.Empty;
        string         lo_strFileDir        = string.Empty;
        Random         lo_rnd               = new Random();
        DirectoryInfo  lo_di                = null;
        HttpPostedFile lo_objHttpPostedFile = null;
        var response = new { success = false, message = ""};


        // 파일 업로드
        try
        {
            if (context.Request.Files.Count == 0)
            {
                objResMap.RetCode = 9001;
                objResMap.ErrMsg = "파일이 업로드되지 않았습니다.";
                return;
            }

            lo_objHttpPostedFile = context.Request.Files["file"];

            string fileExtension = Path.GetExtension(lo_objHttpPostedFile.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

            if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
            {
                objResMap.RetCode = 9002;
                objResMap.ErrMsg = "허용되지 않은 파일 형식입니다.";
                return;
            }

            if (lo_objHttpPostedFile.ContentLength > 10 * 1024 * 1024)
            {
                objResMap.RetCode = 9003;
                objResMap.ErrMsg = "파일 크기는 10MB 이하여야 합니다.";
                return;
            }

            lo_strExtension = Path.GetExtension(lo_objHttpPostedFile.FileName).ToLower().Replace(".", "");
            lo_strFileName = Path.GetFileName(lo_objHttpPostedFile.FileName).ToLower();
            lo_strFileNameNew = "O" + DateTime.Now.ToString("yyyyMMddHHmmss") + lo_rnd.Next(1000, 10000) + "." + lo_strExtension;
            lo_strFileDir = SiteGlobal.FILE_SERVER_ROOT + @"Client\Logo" + @"\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM");
            lo_di = new DirectoryInfo(lo_strFileDir);

            if (!lo_di.Exists)
            {
                lo_di.Create();
            }

            if (lo_objHttpPostedFile.ContentLength.Equals(0))
            {
                objResMap.RetCode = 9004;
                objResMap.ErrMsg = "첨부된 파일이 없습니다.";
                return;
            }

            if (lo_objHttpPostedFile.ContentLength > 10 * 1024 * 1024)
            {
                objResMap.RetCode = 9005;
                objResMap.ErrMsg = "파일 크기는 10MB 이하여야 합니다.";
                return;
            }

            if (File.Exists(lo_strFileDir + @"\" + lo_strFileNameNew))
            {
                objResMap.RetCode = 9006;
                objResMap.ErrMsg = "동일한 파일이 존재합니다.";
                return;
            }

            lo_objHttpPostedFile.SaveAs(lo_strFileDir + @"\" + lo_strFileNameNew);

            if (!File.Exists(lo_strFileDir + @"\" + lo_strFileNameNew))
            {
                objResMap.RetCode = 9006;
                objResMap.ErrMsg = "파일 업로드에 실패했습니다.";
                return;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9201;
            objResMap.ErrMsg = lo_ex.ToString();

            SiteGlobal.WriteLog(
                "WebDomesticFileHandler",
                "Exception",
                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                9201);
        }
        finally {
            objResMap.RetCode = 0;
            objResMap.ErrMsg = "파일 업로드 성공.";
            objResMap.Add("FilePath", lo_strFileDir + @"\" + lo_strFileNameNew);
            objResMap.Add("FileName", lo_strFileName);
        }

    }

    private void SetClientFileDel()
    {
        try
        {
            if (File.Exists(strHidFilePath))
            {
                File.Delete(strHidFilePath);
                objResMap.RetCode = 0;
                objResMap.ErrMsg = "파일이 삭제되었습니다.";
                return;
            }
            else
            {
                objResMap.RetCode = 0;
                objResMap.ErrMsg = "파일을 찾을 수 없습니다.";
                return;
            }
        }
        catch (Exception lo_ex)
        {
            objResMap.RetCode = 9301;
            objResMap.ErrMsg = lo_ex.ToString();

            SiteGlobal.WriteLog(
                "WebDomesticFileHandler",
                "Exception",
                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                9201);
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