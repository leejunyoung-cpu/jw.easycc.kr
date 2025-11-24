using System;
using System.Web.UI;
using CommonLibrary.CommonModule;

namespace logiman.cargomanager.co.kr
{
    public partial class PopupMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

        }

        protected void GetInitData()
        {
            p_popup.Value = SiteGlobal.GetRequestForm("popup");

            if (!p_popup.Value.Equals("Y"))
            {
                divPOPUP_TITLE_LAYER.Style.Add("margin-top", "10px");
            }
            else
            {
                divPOPUP_TITLE_LAYER.Style.Add("margin-top", "10px");
                if (!string.IsNullOrWhiteSpace(SiteGlobal.GetRequestForm("title")))
                {
                    lblPOPUP_TITLE.Text = SiteGlobal.GetRequestForm("title");
                }
            }
        }
    }
}