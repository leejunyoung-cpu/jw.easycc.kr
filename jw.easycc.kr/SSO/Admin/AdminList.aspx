<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminList.aspx.cs" Inherits="SSO.Admin.AdminList" %>

<asp:Content ID="Scriptcontent" ContentPlaceHolderID="headscript" Runat="Server">
    <script src="/SSO/Admin/Proc/AdminList.js?var=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            fnSetInitData();

            $("#SearchType").on("change", function () {
                fnSetInitData();
            });

            $("#ListSearch").on("keydown", function(event) {
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

            $("#BtnInsAdmin").on("click", function () {
                fnInsAdmin("");
                return;
            });

            //---------------------------------------------------------------------------------
            //---- Export Excel 버튼 이벤트
            //---------------------------------------------------------------------------------
            $("#BtnSaveExcel").on("click", function () {
                var objParam = {
                    CallType: "AdminListExcel",
                    GradeCode: $("#GradeCode").val(),
                    UseFlag: $("#UseFlag").val(),
                    SearchType: $("#SearchType").val(),
                    ListSearch: $("#ListSearch").val(),
                };

                $.fileDownload("/SSO/Admin/Proc/AdminHandler.ashx", {
                    httpMethod: "POST",
                    data: objParam,
                    prepareCallback: function () {
                        UTILJS.Ajax.fnAjaxBlock();
                    },
                    successCallback: function (url) {
                        $.unblockUI();
                        fnDefaultAlert("엑셀을 다운로드 하였습니다.", "success");
                    },
                    failCallback: function (html, url) {
                        $.unblockUI();
                        fnDefaultAlert("나중에 다시 시도해 주세요.");
                    }
                });
            });
        });

        function fnSetInitData() {
            if ($("#SearchType option:selected").val() == "") {
                $("#ListSearch").val("");
                $("#ListSearch").attr("disabled", true);
            }
            else {
                $("#ListSearch").attr("disabled", false);
            }
        }

        function fnAjaxSaveExcel(objData) {

            if (objData[0].RetCode !== 0) {
                fnDefaultAlert("나중에 다시 시도해 주세요.(" + objData[0].ErrMsg + ")");
            }

            return;
        }

        function fnReloadPageNotice(strMsg) {
            fnCloseCpLayer();
            fnDefaultAlert(strMsg, "info");
            $("#PageNo").val("1");
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="RecordCnt" />
    <asp:HiddenField runat="server" ID="PageNo" />
    <asp:HiddenField runat="server" ID="PageSize" />
    <asp:HiddenField runat="server" ID="HidGradeCode" />

    <div id="contents">
        <div class="data_list">
            <div class="search">
                <asp:DropDownList runat="server" ID="GradeCode" class="type_01" AutoPostBack="false"></asp:DropDownList>
                <asp:DropDownList runat="server" ID="UseFlag" class="type_01" AutoPostBack="false"></asp:DropDownList>
                <asp:DropDownList runat="server" ID="SearchType" class="type_01" AutoPostBack="false"></asp:DropDownList>
                <asp:TextBox runat="server" ID="ListSearch" class="type_01" AutoPostBack="false" />
                <button type="button" runat="server" ID="BtnListSearch" class="btn_s01">조회</button>
            </div>  

            <ul class="action">
                <li class="left">
                    <button type="button" runat="server" ID="BtnInsAdmin" class="btn_s01 blue">관리자등록</button>
                </li>
                <li class="right">
                    
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

            <div id="AdminListGrid" style="width:100%; height:600px;"></div>
            <div id="page"></div>
        </div>
	</div>
</asp:Content>
