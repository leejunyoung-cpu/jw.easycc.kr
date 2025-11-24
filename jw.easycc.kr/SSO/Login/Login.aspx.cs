using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using System;
using System.Web.UI;

namespace SSO.Login
{
    public partial class Login : Page
    {
        protected string strReturnUrl = string.Empty;
        protected string strWebTemplate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // iframe 안에 로그인 창이 구동된 경우, top window로 redirect 한다.
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                                "if(window != window.top) {window.top.location = '/';}", true);

            Page.ClientScript.GetPostBackEventReference(this, "");

            if (!IsPostBack)
            {
                GetInitData();
            }
        }

        protected void GetInitData()
        {
            try
            {
                strWebTemplate = "default";

                strReturnUrl = Request.QueryString["returnurl"];
                if (string.IsNullOrWhiteSpace(strReturnUrl))
                {
                    strReturnUrl = CommonConstant.MAIN_PAGE_1;
                }

                returnurl.Value = strReturnUrl;
                errmsg.Value = Request.QueryString["errmsg"];
                asd.Value = Utils.GetEncrypt("Server=172.27.0.21,1433;Database=EASYCC_AGENT;User Id=dbjwccs;Password=wpdlejqmffbccs1019!@;Pooling=true;Min Pool Size=10;Max Pool Size=300;Connect Timeout=5;Load Balance Timeout=300;Application Name=CustomsAgent-Web");
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("Login", "Exception"
                                  , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9300);
            }
        }
    }
}