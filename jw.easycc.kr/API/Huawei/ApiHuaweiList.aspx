<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApiHuaweiList.aspx.cs" Inherits="API.Huawei.ApiHuaweiList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headscript" runat="server">
    <script src="/API/Huawei/Proc/ApiHuaweiList.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script>
        $(document).ready(function () {

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="RecordCnt" />
    <asp:HiddenField runat="server" ID="PageNo" />
    <asp:HiddenField runat="server" ID="PageSize" />

    <div id="contents">
        <div class="data_list">
            <div class="search">
                <asp:TextBox runat="server" ID="DateFrom" class="type_01 date" ReadOnly="true" AutoPostBack="false" />
                <asp:TextBox runat="server" ID="DateTo" class="type_01 date" ReadOnly="true" AutoPostBack="false" />
                <button type="button" runat="server" ID="BtnListSearch" class="btn_s01">조회</button>
            </div>  

            <ul class="action">
                <li class="left">
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

            <div id="ApiHuaweiListGrid" style="width:100%; height:600px;"></div>
            <div id="page"></div>
        </div>
    </div>
</asp:Content>
