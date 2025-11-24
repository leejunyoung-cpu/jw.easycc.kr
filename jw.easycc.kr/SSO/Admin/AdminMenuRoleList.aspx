<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMenuRoleList.aspx.cs" Inherits="SSO.Admin.AdminMenuRoleList" %>
<asp:Content ID="Scriptcontent" ContentPlaceHolderID="headscript" runat="server">
 <script src="/SSO/Admin/Proc/AdminMenuRoleList.js?var=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
 <script type="text/javascript">
     
     $(document).ready(function () {
         if ($("#hidDisplayMode").val() == "Y") {
             fnDefaultAlert($("#ErrMsg").val(), "success");
             $("#hidDisplayMode").val("");
             $("#ErrMsg").val("");
         }
     });

     function fnSearchData() {
         fnMoveToPage(1);
     }

     function fnReloadPageNotice(strMsg) {
         fnCloseCpLayer();
         fnSearchData();
     }

     function InsAdminMenuRole(strTitle, strMenuRoleNo) {
        
         fnOpenRightSubLayer(strTitle, "/SSO/Admin/AdminMenuRoleUpd.aspx?MenuRoleNo=" + strMenuRoleNo, "800px", "600px", "40%");
         
     }

 </script>
  
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="RecordCnt" />
    <asp:HiddenField runat="server" ID="PageNo" />
    <asp:HiddenField runat="server" ID="PageSize" />
    <asp:HiddenField runat="server" id="ErrMsg" />
    <asp:HiddenField runat="server" ID="hidDisplayMode"/>
    <div id="contents">
        <div class="data_list">
            <ul class="action">
                <li class="left">
                    <button type="button" class="btn_b01" onclick="javascript:InsAdminMenuRole('메뉴 역할 등록', '');">역할추가</button>
                </li>
            </ul>
        </div>
        <div class="grid_list">
            <ul class="grid_option">
                <li class="left">
                    <strong id="GridResult" style="display: inline-block;"></strong>
                    <strong id="GridDataInfo" style=" line-height: 25px; font-weight: 500; color: #666666; "></strong>
                </li>
                <li class="right">
                </li>
            </ul>

            <div id="AdminMenuRoleListGrid" style="width:100%; height:600px;"></div>
            <div id="page"></div>
        </div>
	</div>
</asp:Content>
