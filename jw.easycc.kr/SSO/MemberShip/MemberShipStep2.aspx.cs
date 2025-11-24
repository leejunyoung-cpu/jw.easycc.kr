using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

///================================================================
/// <summary>
/// FileName        : Login.cs
/// Description     : 로그인 페이지
/// Author          : ljy@jwccs.com, 2024-07-17
/// Modify History  : Just Created.
/// </summary>
///================================================================
namespace SSO.MemberShip
{
    public partial class MemberShipStep2 : Page
    {
        protected string strReturnUrl   = string.Empty;
        protected string strWebTemplate = string.Empty;

        ///----------------------------------------------------------------------
        /// <summary>
        /// Name          : Page_Load()
        /// Description   : 페이지 로드(포스트백인지 체크)
        /// Special Logic : NONE    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///----------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInitData();
            }
        }

        protected void GetInitData()
        {
            try
            {
                string lo_strAgreeFlag = string.Empty;
                lo_strAgreeFlag = Utils.IsNull(SiteGlobal.GetRequestForm("HidAgreeFlag"), "N");
                if (lo_strAgreeFlag.Equals("N")) {
                    DisplayMode.Value = "Y";
                    ErrMsg.Value = "이용약관에 동의해야 가입이 가능합니다.";
                    return;
                }
                HidAgreeFlag.Value = Utils.IsNull(SiteGlobal.GetRequestForm("HidAgreeFlag"), "N");
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("MemberShipStep2", "Exception"
                                  , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9300);
            }
        }
    }
}