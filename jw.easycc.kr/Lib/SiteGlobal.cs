using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace CommonLibrary.CommonModule
{
    public class SiteGlobal
    {

        public static string AES2_ENC_KEY_VALUE = string.Empty;
        public static string AES2_ENC_IV_VALUE = string.Empty;
        private const int VALUE_MAX = 4000;
        // 서버 환경 변수
        public static string EASYCC_DOMAIN = string.Empty;
        public static string EASYCC_REG_DOMAIN = string.Empty;
        public static string EASYCC_API_DOMAIN = string.Empty;
        public static string FILE_DOMAIN = string.Empty;
        public static string FILE_SERVER_ROOT = string.Empty;
        public static string FILE_OCR_SERVER_ROOT = string.Empty;
        public static string FILE_URL = string.Empty;
        public static string DOMAINNAME = string.Empty;
        public static string APP_HOST_IP = string.Empty;
        public static bool B_LOG_WRITE = false;
        public static int MEMBERSHIP_MENU_ROLE_NO; //회원가입시 적용될 고객사 담당자 메뉴 역할 번호
        public static string SERVER_TITLE = string.Empty; //회원가입시 적용될 고객사 담당자 메뉴 역할 번호
        public static string MAIL_SERVER = string.Empty;
        public static int MAIL_PORTNO = 0;
        public static string OpenApiKey = string.Empty;
        public static string GMAIL_USER = "ljy@jwccs.com";
        public static string GMAIL_APP_PASSWORD = "lbef hmer mcsv aziz";
        // 알림톡 템플릿 변수
        public static string userID = "jwccsair";
        public static string apikey = "rvcrha1z5imu5yaxgp93neqw6wp1ex6i";
        public static string senderkey = "e12f4591387b353f94fd15f333ed93a08c651251";
        public static string M_HOST_SERVER_IP = string.Empty;       // 서버IP 변수

        //Connection
        public static string ConnectionString;
        public static bool OpenAiBypassCache = true;

        static SiteGlobal()
        {
            DOMAINNAME = CommonUtils.Utils.GetRegistryValue(CommonConstant.WEBSERVICE_DOMAIN_NAME).ToString();
            AES2_ENC_KEY_VALUE = CommonUtils.Utils.GetRegistryValue(CommonConstant.AES_ENC_KEY_NAME).ToString();
            AES2_ENC_IV_VALUE = CommonUtils.Utils.GetRegistryValue(CommonConstant.AES_ENC_IV_NAME).ToString();

            // 서비스 환경에 따른 변수값 선언
            switch (DOMAINNAME.ToLower())
            {
                case CommonConstant.LOCALDEV_DOMAIN:
                    APP_HOST_IP = "127.0.0.1";
                    EASYCC_DOMAIN = "http://localjw.easycc.kr";
                    FILE_DOMAIN = "http://localjwfile.easycc.kr";
                    FILE_SERVER_ROOT = @"D:\ProjectFiles\Jwfile/";
                    FILE_OCR_SERVER_ROOT = @"D:\ProjectFiles\localOCRJwfile/";
                    ConnectionString = Utils.GetDecrypt("ea8FKChMSDOtUv2Wj6qLOry+dOqeY5QWW1SQ8RQc33dFf7oArVTg4WXjoPrjUBd0VTklGWcXl1ZF9Muy9x9yYJIp9hnGY+uUr19Ck6I335WcAQXOrLPnKGDkuyZ94FvZFn2H8i50p8Cyk1gbvtKJRw0yjypd1bNckqEhnfCxgbbtIqEMryrX4PzGBnC9kfxW+czwISYnJxzU2O0lque/WJj4wY4LBaKCZHRax6J5DvCMAhDtVEES/t1Y/PAQxmUjbX+eoU6Mp62+fglWu5HYnGOOwgw4pU3gB+1T6QDCaTwZTt4++kSdi+aNcNJIXwMdF9Oe/hETjbdB00Gp4w==");
                    FILE_URL = "http://localjw.easycc.kr/UploadFile";
                    B_LOG_WRITE = true;
                    EASYCC_REG_DOMAIN = "localjw.easycc.kr";
                    MEMBERSHIP_MENU_ROLE_NO = 7;
                    SERVER_TITLE = "[LOCAL 서버]";
                    M_HOST_SERVER_IP = "106.254.149.195";
                    MAIL_SERVER = "211.251.239.223";
                    MAIL_PORTNO = 587;
                    break;
                case CommonConstant.TEST_DOMAIN:
                    APP_HOST_IP = "127.0.0.1";
                    EASYCC_DOMAIN = "https://testjw.easycc.kr";
                    FILE_DOMAIN = "http://testjwtfile.easycc.kr";
                    FILE_SERVER_ROOT = @"E:\ProjectFiles\Jwfile/";
                    FILE_OCR_SERVER_ROOT = @"E:\ProjectFiles\OCRJwfile/";
                    FILE_URL = "https://testjw.easycc.kr/UploadFile";
                    ConnectionString = Utils.GetDecrypt("ea8FKChMSDOtUv2Wj6qLOry+dOqeY5QWW1SQ8RQc33dFf7oArVTg4WXjoPrjUBd0VTklGWcXl1ZF9Muy9x9yYJIp9hnGY+uUr19Ck6I335WcAQXOrLPnKGDkuyZ94FvZFn2H8i50p8Cyk1gbvtKJRw0yjypd1bNckqEhnfCxgbbtIqEMryrX4PzGBnC9kfxW+czwISYnJxzU2O0lque/WJj4wY4LBaKCZHRax6J5DvCMAhDtVEES/t1Y/PAQxmUjbX+eoU6Mp62+fglWu5HYnGOOwgw4pU3gB+1T6QDCaTwZTt4++kSdi+aNcNJIXwMdF9Oe/hETjbdB00Gp4w==");
                    B_LOG_WRITE = true;
                    EASYCC_REG_DOMAIN = "testjw.easycc.kr";
                    EASYCC_API_DOMAIN = "https://testapi.easycc.kr/api";
                    SERVER_TITLE = "[TEST 서버]";
                    M_HOST_SERVER_IP = "211.251.239.223";
                    MAIL_SERVER = "127.0.0.1";
                    MAIL_PORTNO = 25;
                    break;
                case CommonConstant.REAL_DOMAIN:
                    APP_HOST_IP = "127.0.0.1";
                    EASYCC_DOMAIN = "https://jw.easycc.kr";
                    FILE_DOMAIN = "https://jwtfile.easycc.kr";
                    FILE_SERVER_ROOT = @"E:\ProjectFiles\JwFile/";
                    FILE_OCR_SERVER_ROOT = @"E:\ProjectFiles\OCRJwFile/";
                    FILE_URL = "https://jw.easycc.kr/UploadFile";
                    ConnectionString = Utils.GetDecrypt("VozFS+bBNxdnKKB5djQgB6aZG6rLD41iAZyDBI9dR4YG0dclWBnbjcExaw0yH2xOqH+aF+Z673Jcn0H3lnF92y2mCLRbBETSqvZzTDhOEeqMankMHHGOggD7zMp3yziAqTIIoqdj5rGIxDGtLxn16afpynTSUxrxaZ+ylmm/1XA1rny2uPHZJvnMga9pY5fQvGXfmX9KTxXeNYFcAlrnjVuht14i9BvzDDE7fdZgIGxDQH/ISNE8Gw0GvLkAnD1gArtYUU1GbQLs6IFkj8x7fUgbFSpC6208vz0/abAVLpxSXB8+cO3G7nuRb2WnWGXg2KhWsD4Inz4=");
                    B_LOG_WRITE = false;
                    EASYCC_REG_DOMAIN = "jw.easycc.kr";
                    EASYCC_API_DOMAIN = "https://api.easycc.kr/api";
                    M_HOST_SERVER_IP = "210.106.105.77";
                    MAIL_SERVER = "127.0.0.1";
                    MAIL_PORTNO = 25;
                    break;
            }
        }

        /*
        //strLogFileName : 파일명(/AdminLog 밑에 파일명_yyyyMMdd.txt 형식으로 파일 생성
        //strLogPreFix : 파일명 뒤 Prefix [Prefix : [IPAddr , FilePath]] 형식으로 생성
        //strLogData : 로그
        //intRetVal : 에러코드
        //intSendMail : 메일전송 여부(0 : 미전송, 1:전송)
        */
        public static void WriteLog(string strLogFileName, string strLogPreFix, string strLogData, int intRetVal = 9000, int intSendMail = 1)
        {
            try
            {
                GlobalContext.Properties["LogPath"] = CommonConstant.DEFAULT_FILE_LOGPATH + strLogFileName;       // 웹사이트 경로에서부터 지정
                GlobalContext.Properties["LogPreFix"] = strLogPreFix + " : [" + SiteGlobal.DOMAINNAME + " , " + HttpContext.Current.Request.FilePath + "]";   // web.config에서 이용 가능함.

                log4net.Config.XmlConfigurator.Configure();
                ILog objLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
                objLog.Info("\r\n\t[RetVal : " + intRetVal + "] " + "[Http Referer : " + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + "] " + strLogData);

                strLogData = strLogData + "\nUserHostAddrss : " + HttpContext.Current.Request.UserHostAddress;

                //5000번대 에러는 메일을 보내지 않는다.
                if (intRetVal >= 5000 && intRetVal < 6000)
                {
                    intSendMail = 0;
                }

                if (intSendMail.Equals(1))
                {
                    string strSubject = "[어드민 에러][" + GlobalContext.Properties["LogPreFix"] + "][RetVal : " + intRetVal + "]";

                    foreach (var adminToEmail in CommonConstant.ADMIN_TO_EMAIL)
                    {
                        SendMail(CommonConstant.ADMIN_FROM_EMAIL, adminToEmail, strSubject, "[Http Referer : " + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + "] " + strLogData, false, "");
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static bool SendMail(string from, string to, string subject, string body, bool isHtml, string strAttach)
        {
            bool lo_result;
            try
            {
                MailMessage message = new MailMessage(from, to, subject, body) { IsBodyHtml = isHtml };

                if (!String.IsNullOrWhiteSpace(strAttach))
                {
                    message.Attachments.Add(new Attachment(strAttach));
                }

                lo_result = SendMail(message);

            }
            catch (Exception lo_ex)
            {
                HttpContext.Current.Response.Write(lo_ex.ToString());
                lo_result = false;

                WriteLog("SiteGlobal", "Exception"
                       , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9002);
            }

            return lo_result;
        }

        public static bool SendMail(MailMessage message)
        {
            bool lo_result = true;

            try
            {
                if (MAIL_PORTNO.Equals(25))
                {
                    SmtpClient client = new SmtpClient(SiteGlobal.MAIL_SERVER, SiteGlobal.MAIL_PORTNO)
                    {
                        Credentials = CredentialCache.DefaultNetworkCredentials
                    };
                    client.Send(message);
                }
                else if (MAIL_PORTNO.Equals(587))
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("ljy@jwccs.com", "lbef hmer mcsv aziz")
   ,
                        EnableSsl = true
   ,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("<br />" + e);
                lo_result = false;
            }
            finally
            {
                message.Dispose();
            }

            return lo_result;
        }

        // ======= 유틸 =======

        private static IEnumerable<string> SplitAddresses(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) yield break;

            // 콤마/세미콜론 구분자 모두 허용
            var tokens = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var t in tokens)
            {
                var email = SanitizeEmail(t);
                if (!string.IsNullOrEmpty(email))
                    yield return email;
            }
        }

        private static string SanitizeEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // 헤더 인젝션 방지: 개행/캐리지리턴 제거
            var s = input.Replace("\r", "").Replace("\n", "").Trim();

            // "Name <email@ex.com>" 같은 형태가 들어올 수 있다면,
            // MailAddress 파서를 잠시 사용해 순수 주소만 뽑는다.
            try
            {
                var ma = new MailAddress(s);
                return ma.Address; // display name 제거, 순수 이메일만 반환
            }
            catch
            {
                // 이미 "a@ex.com,b@ex.com" 같은 혼합 문자열은 SplitAddresses에서 다시 분리되므로
                // 여기서 실패하면 빈 문자열 반환
                return string.Empty;
            }
        }

        public static string GetRequestForm(string name, bool isReplace = true)
        {
            string[] lo_arrVars;
            int lo_intCount;

            foreach (string var in HttpContext.Current.Request.QueryString)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    lo_arrVars = var.Split('$');
                    lo_intCount = lo_arrVars.Length;
                    if (lo_arrVars[lo_intCount - 1].Equals(name))
                    {
                        return isReplace ? GetValue(HttpContext.Current.Request.QueryString[var]) : HttpContext.Current.Request.QueryString[var];
                    }
                }
            }

            foreach (string var in HttpContext.Current.Request.Form)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    lo_arrVars = var.Split('$');
                    lo_intCount = lo_arrVars.Length;
                    if (lo_arrVars[lo_intCount - 1].Equals(name))
                    {
                        return isReplace ? GetValue(HttpContext.Current.Request.Form[var]) : HttpContext.Current.Request.Form[var];
                    }
                }
            }

            return "";
        }

        public static string GetAllRequestForm()
        {
            string[] lo_arrVars;
            string lo_strAllVars = string.Empty;

            foreach (string var in HttpContext.Current.Request.QueryString)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    if (var.StartsWith("__"))
                    {
                        continue;
                    }

                    lo_arrVars = var.Split('$');

                    if (string.IsNullOrWhiteSpace(lo_strAllVars))
                    {
                        lo_strAllVars += "?";
                    }
                    else
                    {
                        lo_strAllVars += "&";
                    }

                    if (!string.IsNullOrWhiteSpace(GetValue(HttpContext.Current.Request.QueryString[var])))
                    {
                        lo_strAllVars += lo_arrVars[lo_arrVars.Length - 1] + "=" + GetValue(HttpContext.Current.Request.QueryString[var]);
                    }
                }
            }

            foreach (string var in HttpContext.Current.Request.Form)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    if (var.StartsWith("__"))
                    {
                        continue;
                    }

                    lo_arrVars = var.Split('$');

                    if (string.IsNullOrWhiteSpace(lo_strAllVars))
                    {
                        lo_strAllVars += "?";
                    }
                    else
                    {
                        lo_strAllVars += "&";
                    }

                    if (!string.IsNullOrWhiteSpace(GetValue(HttpContext.Current.Request.Form[var])))
                    {
                        lo_strAllVars += lo_arrVars[lo_arrVars.Length - 1] + "=" + GetValue(HttpContext.Current.Request.Form[var]);
                    }
                }
            }

            return lo_strAllVars;
        }

        /// <summary>
        /// Filtering GET/POST parameter
        /// </summary>
        /// <param name="strVal">String value</param>
        /// <param name="blnHtmlDecode">Option to set whether Html-decoding</param>
        /// <param name="blnFilter">Option to set whether filtering SQL injection</param>
        /// <return>Filtered value</return>
        public static string GetValue(string strVal, bool blnHtmlDecode = false, bool blnFilter = true)
        {
            string lo_strRet = string.Empty;
            //string lo_strRet = null;

            try
            {
                if (!string.IsNullOrEmpty(strVal))
                {
                    if (blnHtmlDecode)
                    {
                        strVal = HttpUtility.HtmlDecode(strVal);
                    }

                    if (blnFilter)
                    {
                        strVal = strVal.Replace("'", "");
                        strVal = strVal.Replace("\"", "");
                        strVal = strVal.Replace("\\", "");
                        strVal = strVal.Replace("#", "");
                        strVal = strVal.Replace("||", "");

                        strVal = strVal.Replace(">", "");
                        strVal = strVal.Replace("--", "");
                        strVal = strVal.Replace("/*", "");
                        strVal = strVal.Replace("*/", "");
                        strVal = strVal.Replace("xp_", "");
                    }

                    lo_strRet = strVal;
                }
            }
            catch (Exception)
            {
                return lo_strRet;
            }

            return lo_strRet;
        }

        public static string GetHttpHost(HttpRequest httpRequest)
        {
            return httpRequest.ServerVariables.Get("HTTP_HOST");
        }

        public static string GetHttpProtocol()
        {
            string lo_strVal;
            string lo_strRetVal;

            lo_strVal = HttpContext.Current.Request.ServerVariables.Get("HTTPS");
            if (lo_strVal == "off" || lo_strVal == null || lo_strVal.Equals(""))
            {
                lo_strVal = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_PROTO");
                lo_strRetVal = !string.IsNullOrWhiteSpace(lo_strVal) ? lo_strVal : "http";
            }
            else
            {
                lo_strRetVal = "https";
            }

            return lo_strRetVal;
        }

        public static void WriteInformation(string strLogFileName, string strLogPreFix, string strLogData, bool bWrite = true)
        {
            try
            {
                if (!bWrite) return;

                GlobalContext.Properties["LogPath"] = CommonConstant.DEFAULT_FILE_INFO_LOGPATH + strLogFileName;       // 웹사이트 경로에서부터 지정
                GlobalContext.Properties["LogPreFix"] = strLogPreFix + " : [" + SiteGlobal.DOMAINNAME + " , " + HttpContext.Current.Request.FilePath + "]";   // web.config에서 이용 가능함.

                log4net.Config.XmlConfigurator.Configure();
                ILog objLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
                objLog.Info("\r\n\t " + strLogData);

                strLogData = strLogData + "\nUserHostAddrss : " + HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static string GetRemoteAddr()
        {
            string lo_strRemoteAddr = !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (lo_strRemoteAddr.Contains(","))
            {
                lo_strRemoteAddr = lo_strRemoteAddr.Split(',').First().Trim();
            }

            return lo_strRemoteAddr;
        }


        private static string Trunc(string s, int max = VALUE_MAX)
        {
            if (string.IsNullOrEmpty(s)) return s;
            s = s.Trim();
            return (s.Length <= max) ? s : s.Substring(0, max);
        }
        public static string GetAllRequestLog()
        {
            var dict = new Dictionary<string, object>();

            void Process(NameValueCollection nvc)
            {
                foreach (string rawKey in nvc)
                {
                    if (string.IsNullOrWhiteSpace(rawKey)) continue;
                    if (rawKey.StartsWith("__") || rawKey.StartsWith("hid_LAYER_")) continue;

                    var parts = rawKey.Split('$');
                    var key = parts[parts.Length - 1];

                    var val = GetValue(nvc[rawKey]);
                    if (string.IsNullOrWhiteSpace(val)) continue;

                    var secret = ConvSecreFiledData(key, val);
                    secret = Trunc(secret); // 긴 값 컷(필요 시)

                    if (!dict.ContainsKey(key))
                        dict.Add(key, secret);
                }
            }

            Process(HttpContext.Current.Request.QueryString);
            Process(HttpContext.Current.Request.Form);

            // Formatting은 기본 None 이므로 생략
            var json = JsonConvert.SerializeObject(dict, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }
        /*
        public static string GetAllRequestLog()
        {
            string[] lo_arrVars;

            Dictionary<String, Object> lo_objDic = new Dictionary<string, object>();
            JavaScriptSerializer lo_objJsonSerializer = new JavaScriptSerializer();

            foreach (string var in HttpContext.Current.Request.QueryString)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    if (var.StartsWith("__") || var.StartsWith("hid_LAYER_"))
                    {
                        continue;
                    }

                    lo_arrVars = var.Split('$');

                    if (!string.IsNullOrWhiteSpace(GetValue(HttpContext.Current.Request.QueryString[var])))
                    {
                        string lo_strSecretData = ConvSecreFiledData(lo_arrVars[lo_arrVars.Length - 1], GetValue(HttpContext.Current.Request.QueryString[var]));
                        lo_objDic.Add(lo_arrVars[lo_arrVars.Length - 1], lo_strSecretData);
                    }
                }
            }

            foreach (string var in HttpContext.Current.Request.Form)
            {
                if (!string.IsNullOrWhiteSpace(var))
                {
                    if (var.StartsWith("__") || var.StartsWith("hid_LAYER_"))
                    {
                        continue;
                    }

                    lo_arrVars = var.Split('$');

                    if (!string.IsNullOrWhiteSpace(GetValue(HttpContext.Current.Request.Form[var])))
                    {
                        string lo_strSecretData = ConvSecreFiledData(lo_arrVars[lo_arrVars.Length - 1], GetValue(HttpContext.Current.Request.Form[var]));
                        if (!lo_objDic.ContainsKey(lo_arrVars[lo_arrVars.Length - 1]))
                        {
                            lo_objDic.Add(lo_arrVars[lo_arrVars.Length - 1], lo_strSecretData);
                        }
                    }
                }
            }

            return lo_objJsonSerializer.Serialize(lo_objDic);
        }
        */
        public static string ConvSecreFiledData(string strKey, string strValue)
        {
            string lo_strSecretData = string.Empty;
            bool lo_bSecretFlag = false;
            int lo_intSecretLen = 0;

            try
            {
                lo_bSecretFlag = false;
                for (int nLoop = 0; nLoop < CommonConstant.SECRETFIELD.Length; nLoop++)
                {
                    if (strKey.ToLower().Trim().Equals(CommonConstant.SECRETFIELD[nLoop].Split(':')[0].Trim()))
                    {
                        lo_bSecretFlag = true;
                        lo_intSecretLen = Convert.ToInt32(CommonConstant.SECRETFIELD[nLoop].Split(':')[1]);
                        break;
                    }
                }

                if (lo_bSecretFlag)
                {
                    int lo_intShowLen = strValue.Length - lo_intSecretLen;
                    if (lo_intShowLen < 0)
                    {
                        lo_intShowLen = 0;
                    }

                    lo_strSecretData = strValue.Substring(0, lo_intShowLen);
                    lo_strSecretData = lo_strSecretData.PadRight(strValue.Length, '*');
                }
                else
                {
                    lo_strSecretData = strValue;
                }
            }
            catch
            {
                lo_strSecretData = strValue;
            }

            return lo_strSecretData;
        }

        //유니패스 API 호출 XML->Json 파싱
        public static string GetUnipassInfoApi(string strApiUrl)
        {
            string results = string.Empty;
            string jsonString = string.Empty;
            string lo_strResult = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(strApiUrl);
                request.Method = "GET";
                request.Timeout = System.Threading.Timeout.Infinite;
                HttpWebResponse response;

                using (response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    results = reader.ReadToEnd();
                    // XML 문자열을 XmlDocument로 파싱
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(results);

                    XDocument doc = XDocument.Parse(results);
                    jsonString = JsonConvert.SerializeXNode(doc);

                    JObject obj = JObject.Parse(jsonString);

                    lo_strResult = JsonConvert.SerializeObject(obj);
                }

            }
            catch (WebException lo_ex)
            {
                WriteLog("SiteGlobal", "Exception", "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9901);
            }

            return lo_strResult;
        }
    }
}