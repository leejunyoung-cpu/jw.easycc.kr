using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace easycc.co.kr
{
    public partial class DashBoard : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInitData();
            }
            else
            {
                GetPostBackData();
            }
        }

        protected void GetInitData()
        {
            
        }
        protected void GetPostBackData()
        {
            string lo_strEventTarget = SiteGlobal.GetRequestForm("__EVENTTARGET");
            string lo_strEventArgument = SiteGlobal.GetRequestForm("__EVENTARGUMENT");

            try
            {
                
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}