using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using System;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary.CommonModel;
using CommonLibrary.Extensions;

namespace SSO.Client
{
    public partial class ClientIns : PageBase
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
            string lo_strClientCode = Utils.IsNull(SiteGlobal.GetRequestForm("ClientCode"), "0");

            HidMode.Value = lo_strHidMode;

            CommonDDLB.USE_FLAG_DDLB(UseFlag);

            ClientType.Items.Clear();
            ClientType.Items.Add(new ListItem("고객사", "1"));
            ClientType.Items.Add(new ListItem("대행사", "2"));
            ClientType.Items.Add(new ListItem("운송주선사", "3"));

            if (lo_strHidMode.Equals("Update") && !lo_strClientCode.Equals("0")) {
                DisplayData(lo_strClientCode);
            }
        }

        protected void DisplayData(string lo_strClientCode)
        {
            ReqClientList                   lo_objReqClientList = null;
            ServiceResult<ResClientList>    lo_objResClientList = null;
            ClientService                   lo_objClientService = null;
            string lo_strFilePath                               = string.Empty;
            try 
            {
                lo_objClientService = new ClientService();

                lo_objReqClientList = new ReqClientList() { 
                    ClientCode = lo_strClientCode.ToInt()
                };

                lo_objResClientList = lo_objClientService.GetClientList(lo_objReqClientList);
                if (lo_objResClientList.result.ErrorCode.IsFail() || !lo_objResClientList.data.RecordCnt.Equals(1))
                {
                    DisplayMode.Value = "Y";
                    ErrMsg.Value = "고객사 정보를 조회하지 못했습니다.";
                    return;
                }
                CorpNoBtn.Visible = false;
                CorpNoBtnReturn.Visible = false;
                CorpNo.ReadOnly = true;
                SiteID.ReadOnly = true;
                CorpCheck.Value = "Y";

                ClientCode.Value    = Utils.GetEncrypt(lo_objResClientList.data.list[0].ClientCode.ToString());
                CorpNo.Text         = lo_objResClientList.data.list[0].CorpNo;
                SiteID.Text         = lo_objResClientList.data.list[0].SiteID;
                ClientName.Text     = lo_objResClientList.data.list[0].ClientName;
                ClientCeoName.Text  = lo_objResClientList.data.list[0].ClientCeoName;
                Email.Text          = lo_objResClientList.data.list[0].Email;
                ClientTel.Text      = lo_objResClientList.data.list[0].ClientTel;
                UseFlag.Text        = lo_objResClientList.data.list[0].UseFlag;
                Addr.Text           = lo_objResClientList.data.list[0].Addr;
                ClientType.SelectedValue    = lo_objResClientList.data.list[0].ClientType.ToString();
                BtoBYN.Checked              = lo_objResClientList.data.list[0].BtoBYN.Equals("Y") ? true : false;
                BtoCYN.Checked              = lo_objResClientList.data.list[0].BtoCYN.Equals("Y") ? true : false;
                lo_strFilePath              = lo_objResClientList.data.list[0].ClientLogoFile;
                
                if (!string.IsNullOrEmpty(lo_strFilePath))
                {
                    HidFilePath.Value = lo_strFilePath;
                    ClientLogoFilePath.ImageUrl = "/UploadFile/" + lo_strFilePath.Substring(lo_strFilePath.IndexOf("Client"));
                }
                else {
                    ClientLogoFilePath.Visible = false;
                }
                

            }
            catch(Exception lo_ex) {
                SiteGlobal.WriteLog("ClientIns", "Exception",
                                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " +
                                    lo_ex.StackTrace, 9100);
            }
            finally {
                
            }
        }
    }
}