<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="ApiWhiteIns.aspx.cs" Inherits="SSO.API.ApiWhiteIns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headscript" runat="server">
    <script src="/SSO/API/Proc/ApiWhiteIns.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script src="/js/lib/jquery/jquery.fileupload.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="DisplayMode" />
    <asp:HiddenField runat="server" ID="ErrMsg" />
    <asp:HiddenField runat="server" ID="HidMode" />
    <asp:HiddenField runat="server" ID="SeqNo" />
    <div id="iframe_wrap">
    <div runat="server" id="POPUP_VIEW">
        <div class="popup_control">
            <div>
                <table class="tblst1">
                    <colgroup>
                        <col style="width:180px"/> 
                        <col style="width:auto;"/> 
                    </colgroup>
                    <tr>
                        <th><span style="color:#f00">*</span> 서비스코드</th>
                        <td class="lft"><asp:TextBox runat="server" ID="ServiceCode" class="type_02" MaxLength="50" placeholder="서비스 코드" title="서비스 코드"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 업체코드</th>
                        <td class="lft"><asp:TextBox runat="server" ID="Company" class="type_01" MaxLength="50" placeholder="업체코드" title="업체코드"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> IP주소</th>
                        <td class="lft"><asp:TextBox runat="server" ID="IpAddress" class="type_01" MaxLength="45" placeholder="대표자명" title="대표자명"></asp:TextBox></td>
                    </tr>
                    <tr class="active">
                        <th><span style="color:#f00">*</span> 활성화 여부</th>
                        <td class="lft"><asp:DropDownList runat="server" ID="IsActive" Width="120px" CssClass="type_01"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th>서비스URL</th>
                        <td class="lft"><asp:TextBox runat="server" ID="ServiceUrl" class="type_02" MaxLength="100" placeholder="서비스URL"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>메모</th>
                        <td class="lft"><asp:TextBox runat="server" ID="IpNote" class="type_02" MaxLength="100" placeholder="메모"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="text-align:center;margin-top:10px">
            <ul><li><button type="button" class="btn_b01" onclick="javascript:fnInsApiWhiteConfirm();"><asp:Label runat="server" ID="lblMode" style="color:#fff;font-weight:200;font-size:15px">저장</asp:Label></button></li></ul>
        </div>
    </div>
</div>
</asp:Content>
