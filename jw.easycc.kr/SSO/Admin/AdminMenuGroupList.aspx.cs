using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Data;
using System.IO;

namespace SSO.Admin
{
    public partial class AdminMenuGroupList : PageBase
    {
        public string[] arrImageName;

        protected void Page_Init(object sender, EventArgs e)
        {
            _pageAccessType = PageAccessType.ReadOnly;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInitData();
               
            }
            DisplayData();
        }
       

        protected void GetInitData()
        {
            
        }

        protected void DisplayData()
        {
            ReqAdminMenuGroupList                   lo_objReqAdminMenuGroupList = null;
            ServiceResult<ResAdminMenuGroupList>    lo_objRResAdminMenuGroupList = null;
            AdminMenuService                        lo_objAdminMenuService = null;
            DataTable                               lo_objDt = null;
            try
            {
                lo_objAdminMenuService      = new AdminMenuService();
                lo_objReqAdminMenuGroupList = new ReqAdminMenuGroupList
                {
                    MenuGroupNo =null
                };

                lo_objRResAdminMenuGroupList = lo_objAdminMenuService.GetAdminMenuGroupInfo(lo_objReqAdminMenuGroupList);


                if (lo_objRResAdminMenuGroupList.result.ErrorCode.IsFail())
                {
                    hidDisplayMode.Value = "Y";
                    hidErrMsg.Value = "메뉴 그룹 정보를 읽지 못했습니다.";
                    return;
                }

                RecordCnt.Value = lo_objRResAdminMenuGroupList.data.RecordCnt.ToString();

                lo_objDt = lo_objRResAdminMenuGroupList.data.list.GetConvertListToDataTable();
                repList.DataSource = lo_objDt;
                repList.DataBind();

            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("AdminMenuUpd", "Exception",
                                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " +
                                    lo_ex.StackTrace, 9100);
            }
        }
    }
}