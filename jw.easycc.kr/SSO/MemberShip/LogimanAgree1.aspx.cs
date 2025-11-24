using CommonLibrary.CommonModule;
using CommonLibrary.Constants;
using System;
using System.Web.UI;

///================================================================
/// <summary>
/// FileName        : Login.cs
/// Description     : 로그인 페이지
/// Author          : ljy@jwccs.com, 2024-07-17
/// Modify History  : Just Created.
/// </summary>
///================================================================
namespace logiman.cargomanager.co.kr
{
    public partial class LogimanAgree1 : Page
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
                
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("LogimanAgree1", "Exception"
                                  , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9300);
            }
        }
    }
}