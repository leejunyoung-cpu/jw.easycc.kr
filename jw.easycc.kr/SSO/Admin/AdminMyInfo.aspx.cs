using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.DBServices;
using CommonLibrary.DBModel;
using CommonLibrary.Extensions;
using System;

namespace SSO.Admin
{
    public partial class AdminMyInfo : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            _pageAccessType = PageAccessType.ReadWrite;
        }

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

            DisplayData();
        }

        protected void GetInitData()
        {
            GradeCode.Value = objSes.GradeCode.ToString();
        }

        protected void GetPostBackData()
        {
            string lo_strEventTarget = SiteGlobal.GetRequestForm("__EVENTTARGET");

            if (lo_strEventTarget.Equals("logout"))
            {
                objSes.goLogout();
                objSes.GoLogin("");
            }
        }
        protected void DisplayData()
        {
            ReqAdminList lo_objReqAdminList = null;
            ServiceResult<ResAdminList> lo_objResAdminList = null;
            AdminService lo_objAdminService = null;

            try
            {
                lo_objAdminService = new AdminService();

                lo_objReqAdminList = new ReqAdminList
                {
                    AdminID          = objSes.AdminID,
                    SesGradeCode     = objSes.GradeCode,
                    AccessClientCode = objSes.AccessClientCode
                };

                lo_objResAdminList = lo_objAdminService.GetAdminList(lo_objReqAdminList);
                if (lo_objResAdminList.result.ErrorCode.IsFail() || !lo_objResAdminList.data.RecordCnt.Equals(1))
                {
                    hidDisplayMode.Value = "Y";
                    hidErrMsg.Value = "사용자 정보를 조회하지 못했습니다.";
                    return;
                }

                AdminID.Text           = lo_objResAdminList.data.list[0].AdminID;
                HidEncCode.Value       = lo_objResAdminList.data.list[0].AdminPWD; 
                MobileNo.Text          = lo_objResAdminList.data.list[0].MobileNo;
                AdminName.Text         = lo_objResAdminList.data.list[0].AdminName;
                Email.Text             = lo_objResAdminList.data.list[0].Email;
                TelNo.Text             = lo_objResAdminList.data.list[0].TelNo;
                DeptName.Text          = lo_objResAdminList.data.list[0].DeptName;
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("AdminMyInfo", "Exception",
                                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " +
                                    lo_ex.StackTrace, 9100);
            }
        }
    }
}