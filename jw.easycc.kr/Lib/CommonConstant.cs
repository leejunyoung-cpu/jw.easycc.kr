using System.Web;

namespace CommonLibrary.Constants
{
    public class CommonConstant
    {
        public static string SITE_TITLE             = ":::: EasyCC Connect ::::";
        public const string ADMIN_FROM_EMAIL        = "JwccsAdmin@jwccs.com";
        public const string UPS_FROM_EMAIL          = "UPS@jwccs.com";
        public const string UPS_B2C_FROM_EMAIL      = "upsb2c@jwccs.com";
        //public static string[] ADMIN_TO_EMAIL       = { "jjy@jwccs.co.kr" };
        public static string[] ADMIN_TO_EMAIL       = { "ljy@jwccs.com", "jjy@jwccs.co.kr" };

        // 일반변수
        public static string LOGIN_PAGE                 = "/SSO/Login/Login.aspx";
        public static string MAIN_PAGE_1                = "/Index";
        public static string MAIN_PAGE_2                = "/Index.aspx";
        public static string PAGENAVIGATION_LIST        = "20";
        public static string GRID_PAGENAVIGATION_LIST   = "3000";
        public static string[] SECRETFIELD              = { "card_no:8", "expire_date:2", "batch_key:8" }; // 로그에 남기지 않을 REQUEST 필드명(:Hidden 길이)

        // 사이트의 도메인 정보 관리 변수
        public const string WEBSERVICE_REGISTRY     = "EasyCCJwDomain";
        public const string WEBSERVICE_REGISTRY_KEY = "jw.easycc.kr";
        public const string WEBSERVICE_DOMAIN_NAME  = "DomainName";
        public const string LOCALDEV_DOMAIN         = "localjw.easycc.kr";
        public const string TEST_DOMAIN             = "testjw.easycc.kr";
        public const string REAL_DOMAIN             = "jw.easycc.kr";
        public const string AES_ENC_KEY_NAME        = "AesEncKey";
        public const string AES_ENC_IV_NAME         = "AesEncIV";
        public const string COOKIE_NAME             = "EasyCCADM";

        public const int    DB_SUCCESS_CODE                    = 0;
        public const int    DB_RET_VAL_CODE                    = 999;
        public const string DB_ERR_MESSAGE                     = "DAS error occurred.";
        public const int    DB_DATA_ACCESS_SERVICE_ERR_CODE    = 998;
        public const string DB_DATA_ACCESS_SERVICE_ERR_MESSAGE = "An unexpected error occurred during accessing data.";

        // 서비스 점검 
        public const string SERVICE_STOP_FLAG       = "ServiceStopFlag";
        public const string SERVICE_STOP_ALLOWIP    = "ServiceStopAllowIP";
        public const string SERVICE_STOP_TITLE      = "ServiceStopTitle";
        public const string SERVICE_STOP_CONTENT    = "ServiceStopContent";

        public const int HTTP_STATUS_CODE_999 = 999;
        public const string HTTP_STATUS_CODE_999_MESSAGE = "예기치 않은 오류가 발생하였습니다.";
        public const string COMMON_EXCEPTION_MESSAGE = "예기치 않은 오류가 발생하였습니다.";

        public const int HTTP_STATUS_CODE_401 = 401;
        public const string HTTP_STATUS_CODE_401_MESSAGE = "You do not have permission. Please access the correct path.";

        public const int HTTP_STATUS_CODE_403 = 403;
        public const string HTTP_STATUS_CODE_403_MESSAGE = "An exception error occurred.";

        public const int HTTP_STATUS_CODE_404 = 404;
        public const string HTTP_STATUS_CODE_404_MESSAGE = "Page Not Found Error(The wrong approach.)";

        public const int HTTP_STATUS_CODE_500 = 500;
        public const string HTTP_STATUS_CODE_500_MESSAGE = "An error occurred while processing.";

        public const int HTTP_STATUS_CODE_503 = 503;
        public const string HTTP_STATUS_CODE_503_MESSAGE = "HTTP Specific error occurred.";

        public static readonly string DEFAULT_FILE_LOGPATH      = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + "..\\logfiles\\EasyCCLog\\";
        public static readonly string DEFAULT_FILE_INFO_LOGPATH = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + "..\\logfiles\\EasyCCInfoLog\\";
    }
}