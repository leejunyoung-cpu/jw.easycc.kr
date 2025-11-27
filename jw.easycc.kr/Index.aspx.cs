using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace easycc.co.kr
{
    public partial class Index : PageBase
    {
        protected string strLeftMenu_B_List = string.Empty;
        protected string strDocumentTitle = string.Empty;
        protected string strLogoImage = string.Empty;
        protected string strSiteMap = string.Empty;
        protected int intGradeCode = 0;
        AdminService objAdminDBServices = new AdminService();
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
            try
            {
                // iframe 안에 로그인 창이 구동된 경우, top window로 redirect 한다.
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                                    "if(window != window.top) {window.top.location = '/';}", true);

                GetLeftMenuGroup();
                intGradeCode = objSes.GradeCode;
                HidGradeCode.Value = objSes.GradeCode.ToString();
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("Index", "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9310);
            }
        }

        protected void GetPostBackData()
        {
            if (SiteGlobal.GetRequestForm("__EVENTTARGET").Equals("logout"))
            {
                objSes.goLogout();
                objSes.GoLogin("");
            }
        }

        protected void GetLeftMenuGroup()
        {
            ServiceResult<ResAdminLeftMenuAllList> lo_objResAdminLeftMenuAllList = null;
            StringBuilder lo_sb_b = null;
            int nIndexNo = 0;
            string lo_text = string.Empty;
            string lo_strMenuName = string.Empty;

            Dictionary<int, string> menuGroupKind = new Dictionary<int, string>()
            {
                { 4, "서비스" },
                { 3, "API 오더" },
                { 2, "고객사" },
                { 1, "시스템관리" }
            };

            Dictionary<int, string> menuGroupList = new Dictionary<int, string>();
            bool bFound = false;

            try
            {
                lo_objResAdminLeftMenuAllList = objAdminDBServices.GetAdminLeftMenuAllList(objSes.AdminID);
                lo_sb_b = new StringBuilder();

                if (lo_objResAdminLeftMenuAllList.result.ErrorCode.IsFail())
                {
                    return;
                }

                for (int i = 0; i < menuGroupKind.Count; i++)
                {
                    for (int j = 0; j < lo_objResAdminLeftMenuAllList.data.list.Count; j++)
                    {
                        if (menuGroupKind.Keys.ElementAt(i).Equals(lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupKind))
                        {
                            lo_text += menuGroupKind.Values.ElementAt(i) + ",";
                        }
                    }
                }

                for (int i = 0; i < menuGroupKind.Count; i++)
                {
                    for (int y = 0; y < lo_text.TrimEnd(',').Split(',').Distinct().ToArray().Length; y++)
                    {

                        if (menuGroupKind.Values.ElementAt(i).Equals(lo_text.TrimEnd(',').Split(',').Distinct().ToArray()[y]))
                        {
                            menuGroupList.Clear();
                            for (int j = 0; j < lo_objResAdminLeftMenuAllList.data.list.Count; j++)
                            {
                                if (!lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupKind.Equals(menuGroupKind.Keys.ElementAt(i)))
                                {
                                    continue;
                                }
                                else
                                {
                                    lo_text += menuGroupKind.Values.ElementAt(i) + ",";
                                }

                                bFound = false;
                                for (int k = 0; k < menuGroupList.Count; k++)
                                {
                                    if (lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupNo.Equals(menuGroupList.Keys.ElementAt(k)) &&
                                       lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupName.Equals(menuGroupList.Values.ElementAt(k)))
                                    {
                                        bFound = true;
                                        break;
                                    }
                                }

                                if (bFound.Equals(false))
                                {
                                    menuGroupList.Add(lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupNo, lo_objResAdminLeftMenuAllList.data.list[j].MenuGroupName);
                                }
                            }

                            /*메뉴*/
                            lo_sb_b.AppendLine("<li class=\"m\"><span>" + menuGroupKind.Values.ElementAt(i) + "</span>");
                            lo_sb_b.AppendLine("<ul class=\"sub_depth1\">");

                            for (int j = 0; j < menuGroupList.Count; j++)
                            {
                                /*메뉴*/
                                lo_sb_b.AppendLine("<li>");
                                lo_sb_b.AppendLine("<dl>");
                                lo_sb_b.AppendLine("<dt>" + HttpUtility.JavaScriptStringEncode(menuGroupList.Values.ElementAt(j)) + "</dt>");
                                lo_sb_b.AppendLine("<dd>");
                                lo_sb_b.AppendLine("<ul class=\"menu" + menuGroupList.Keys.ElementAt(j) + "\">");
                                nIndexNo = 0;
                                for (int k = 0; k < lo_objResAdminLeftMenuAllList.data.list.Count; k++)
                                {
                                    if (lo_objResAdminLeftMenuAllList.data.list[k].MenuGroupNo.Equals(menuGroupList.Keys.ElementAt(j)) &&
                                       lo_objResAdminLeftMenuAllList.data.list[k].MenuGroupName.Equals(menuGroupList.Values.ElementAt(j)))
                                    {
                                        lo_strMenuName = HttpUtility.JavaScriptStringEncode(lo_objResAdminLeftMenuAllList.data.list[k].MenuName);
                                        lo_strMenuName = lo_strMenuName.Replace("\\u0026", "&amp;");

                                        lo_sb_b.AppendLine("<li><a href=\"javascript:fnGoPage('" + menuGroupKind.Values.ElementAt(i) + "','" + HttpUtility.JavaScriptStringEncode(lo_objResAdminLeftMenuAllList.data.list[k].MenuGroupName) + "', '" + lo_strMenuName + "','" + lo_objResAdminLeftMenuAllList.data.list[k].MenuLink + "','" + lo_objResAdminLeftMenuAllList.data.list[k].MenuGroupNo + "', " + nIndexNo + ");\">" + lo_strMenuName + "</a></li>");
                                        nIndexNo++;
                                    }
                                }

                                lo_sb_b.AppendLine("</ul>");
                                lo_sb_b.AppendLine("</dd>");
                                lo_sb_b.AppendLine("</dl>");
                                lo_sb_b.AppendLine("</li>");
                            }

                            lo_sb_b.AppendLine("</ul>");
                            lo_sb_b.AppendLine("</li>");
                        }
                    }
                }
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("Index", "Exception"
                    , "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace, 9312);
            }

            strLeftMenu_B_List += lo_sb_b.ToString();
        }
    }
}