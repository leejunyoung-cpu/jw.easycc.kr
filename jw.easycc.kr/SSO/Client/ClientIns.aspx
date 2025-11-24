<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="ClientIns.aspx.cs" Inherits="SSO.Client.ClientIns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headscript" runat="server">
    <script src="/SSO/Client/Proc/ClientIns.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script src="/js/lib/jquery/jquery.fileupload.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="DisplayMode" />
    <asp:HiddenField runat="server" ID="ErrMsg" />
    <asp:HiddenField runat="server" ID="HidMode" />
    <asp:HiddenField runat="server" ID="CorpCheck" />
    <asp:HiddenField runat="server" ID="ClientCode" />
    <asp:HiddenField runat="server" ID="HidFilePath" />
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
                        <th style="width:150px"><span style="color:#f00">*</span> 사업자번호</th>
                        <td class="lft">
                            <asp:TextBox runat="server" ID="CorpNo" Width="233px" class="type_01 OnlyNumber" MaxLength="10" PlaceHolder="사업자번호('-'제외)" Title="사업자번호('-'제외)"></asp:TextBox>
                            <button type="button" class="btn_b01 gray" runat="server" id="CorpNoBtn" onclick="fnChkCorpNo();">중복확인</button>
                            <button type="button" class="btn_b01 gray" runat="server" id="CorpNoBtnReturn" onclick="fnClientReset();" style="display:none;">다시 입력</button>
                            <br />
                            <span class="id_pass" style="color:#006cb7; display:none;">등록가능한 고객사입니다.</span>
                            <span class="id_fail" style="color:#f15929; display:none;">이미 등록된 고객사입니다.</span>
                        </td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 고객사 고유번호</th>
                        <td class="lft"><asp:TextBox runat="server" ID="SiteID" class="type_02" MaxLength="14" placeholder="고객사 고유번호(ex : AB0F)" title="고객사 고유번호(ex : AB0F)"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 고객사명</th>
                        <td class="lft"><asp:TextBox runat="server" ID="ClientName" class="type_01" MaxLength="30" placeholder="고객사명" title="고객사명"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 대표자명</th>
                        <td class="lft"><asp:TextBox runat="server" ID="ClientCeoName" class="type_01" MaxLength="30" placeholder="대표자명" title="대표자명"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 서비스 유형</th>
                        <td class="lft">
                            <asp:CheckBox runat="server" ID="BtoBYN" Text="<span></span> B2B"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="BtoCYN" Text="<span></span> B2C"/>
                        </td>
                    </tr>
                    <tr>
                        <th>고객사 이메일</th>
                        <td class="lft"><asp:TextBox runat="server" ID="Email" class="type_02" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>전화번호</th>
                        <td class="lft"><asp:TextBox runat="server" ID="ClientTel" class="type_02 OnlyNumber" MaxLength="20"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>주소</th>
                        <td class="lft"><asp:TextBox runat="server" ID="Addr" class="type_02" style="width:100%;" MaxLength="256"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>로고 이미지<br /><span>(사이즈 : 160 X 43)</span></th>
                        <td class="lft">
                            <asp:Image runat="server" ID="ClientLogoFilePath" style="width:160px;"/>
                            <asp:FileUpload runat="server" ID="ClientLogoFile" CssClass="type_03" style="vertical-align: super;"/> 
                            <button type="button" id="BtnLogoDel" class="btn_03" style="display:none; vertical-align:top;" onclick="fnLogoUploadDelConfirm();">삭제</button>
                        </td>
                    </tr>
                    <tr>
                        <th>고객사 유형</th>
                        <td class="lft">
                            <asp:DropDownList runat="server" ID="ClientType" CssClass="type_01"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th><span style="color:#f00">*</span> 사용 여부</th>
                        <td class="lft"><asp:DropDownList runat="server" ID="UseFlag" Width="120px" CssClass="type_01"></asp:DropDownList></td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="text-align:center;margin-top:10px">
            <ul><li><button type="button" class="btn_b01" onclick="javascript:fnInsClient();"><asp:Label runat="server" ID="lblMode" style="color:#fff;font-weight:200;font-size:15px">저장</asp:Label></button></li></ul>
        </div>
    </div>
</div>
</asp:Content>
