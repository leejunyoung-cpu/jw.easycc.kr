<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientList.aspx.cs" Inherits="SSO.Client.ClientList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headscript" runat="server">
    <script src="/SSO/Client/Proc/ClientList.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script>
        $(document).ready(function () {

            fnSetInitData();

            $("#SearchType").on("change", function () {
                fnSetInitData();
            });

            $("#ListSearch").on("keydown", function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                    fnMoveToPage(1);
                    return;
                }
            });

            $("#BtnListSearch").on("click", function () {
                fnMoveToPage(1);
                return;
            });
        });

        function fnSetInitData() {
            if ($("#SearchType option:selected").val() == "") {
                $("#ListSearch").val("");
                $("#ListSearch").attr("disabled", true);
            }
            else {
                $("#ListSearch").attr("disabled", false);
                $("#ListSearch").focus();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="RecordCnt" />
    <asp:HiddenField runat="server" ID="PageNo" />
    <asp:HiddenField runat="server" ID="PageSize" />

    <div id="contents">
        <div class="data_list">
            <div class="search">
                <asp:DropDownList runat="server" ID="UseFlag" class="type_01" AutoPostBack="false"></asp:DropDownList>
                <asp:TextBox runat="server" ID="ClientName" class="type_01" placeholder="고객사명" AutoPostBack="false" />
                <asp:TextBox runat="server" ID="ClientCeoName" class="type_01" placeholder="대표자명" AutoPostBack="false" />
                <asp:TextBox runat="server" ID="CorpNo" class="type_01" placeholder="사업자번호" AutoPostBack="false" />
                <button type="button" runat="server" ID="BtnListSearch" class="btn_s01">조회</button>
            </div>  

            <ul class="action">
                <li class="left">
                    <button type="button" runat="server" ID="BtnInsAdmin" onclick="fnClientIns();" class="btn_s01 blue">고객사등록</button>
                </li>
                <li class="right">
                    <button type="button" class="btn_02 download" onclick="fnClientListExcel();">엑셀다운</button>
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

            <div id="ClientListGrid" style="width:100%; height:550px;"></div>
            <div id="page"></div>
        </div>
    </div>
</asp:Content>
