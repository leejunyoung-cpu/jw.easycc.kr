using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using System;
using System.Web.UI.WebControls;

namespace SSO.API
{
    public partial class ApiWhiteIns : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInitData();
            }
        }

        protected void GetInitData()
        {
            string lo_strHidMode = SiteGlobal.GetRequestForm("HidMode");
            string lo_strSeqNo = Utils.IsNull(SiteGlobal.GetRequestForm("SeqNo"), "0");

            HidMode.Value = lo_strHidMode;
            SeqNo.Value = lo_strSeqNo;

            IsActive.Items.Clear();
            IsActive.Items.Add(new ListItem("활성화 여부", ""));
            IsActive.Items.Add(new ListItem("활성화", "Y"));
            IsActive.Items.Add(new ListItem("비활성화", "N"));
        }
    }
}