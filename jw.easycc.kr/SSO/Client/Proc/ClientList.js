// 그리드
var GridID = "#ClientListGrid";
var GridData = [];
var GridSet;

$(document).ready(function () {
    fnCallGridData(GridID);
});

var columns = [
    {
        id: "ClientCode",
        name: "고객사코드",
        field: "ClientCode",
        width: 0,
        visible: false
    },
    {
        id: "SiteID",
        name: "고객사고유번호",
        field: "SiteID",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "ClientName",
        name: "고객사명",
        field: "ClientName",
        width: 250,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "ClientCeoName",
        name: "대표자명",
        field: "ClientCeoName",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "CorpNo",
        name: "사업자번호",
        field: "CorpNo",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "ClientTel",
        name: "대표전화번호",
        field: "ClientTel",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "BtoBYN",
        name: "B2B",
        field: "BtoBYN",
        width: 80,
        visible: true
    },
    {
        id: "BtoCYN",
        name: "B2C",
        field: "BtoCYN",
        width: 80,
        visible: true
    },
    {
        id: "UseFlagM",
        name: "사용여부",
        field: "UseFlagM",
        width: 80,
        visible: true
    },
    {
        id: "RegDate",
        name: "등록일시",
        field: "RegDate",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "RegAdminID",
        name: "등록아이디",
        field: "RegAdminID",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "UpdDate",
        name: "수정일시",
        field: "UpdDate",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "UpdAdminID",
        name: "수정아이디",
        field: "UpdAdminID",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "BtnInsAdmin",
        name: "조회",
        field: "BtnInsAdmin",
        formatter: actionButtonFormatter,
        width: 100,
        visible: true
    }
];

/*그리드 옵션 */
var options = {
    editable: true,
    enableAddRow: false,
    enableCellNavigation: true,
    asyncEditorLoading: false,
    rowHeight: 35
};

/**
 * 버튼 이벤트
 */

/*조회*/
function actionButtonFormatter(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnClientIns('" + dataContext.ClientCode + "')\">조회</button>";
}

/**
 * /
 * 그리드 데이터 호출
 */
function fnCallGridData(strGID) {
    $("#divLoadingImage").show();
    var strHandlerURL = "/SSO/Client/Proc/ClientHandler.ashx";
    var strCallBackFunc = "fnGridSuccResult";

    var objParam = {
        CallType        : "ClientList",
        UseFlag         : $("#UseFlag").val(),
        ClientName      : $("#ClientName").val(),
        ClientCeoName   : $("#ClientCeoName").val(),
        CorpNo          : $("#CorpNo").val(),
        PageNo          : $("#PageNo").val(),
        PageSize        : $("#PageSize").val()
    };
    
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, false, "", "", true);
}


function fnGridSuccResult(objRes) {
    if (objRes) {
        if (objRes[0].result.ErrorCode !== 0) {
            $("#RecordCnt").val(0);
            $("#GridResult").html("");
            $("#divLoadingImage").hide();
            fnDefaultAlert(objRes[0].result.ErrorMsg, "warning");
            return false;
        }

        // 페이징
        fnCreatePagingNavigator();
        $("#RecordCnt").val(objRes[0].data.RecordCnt);
        $("#GridResult").html("[" + objRes[0].data.RecordCnt + "건]");
        // visible이 true인 컬럼만 필터링
        var visibleColumns = columns.filter(column => column.visible);
        GridData = objRes[0].data.list;
        GridSet = new Slick.Grid(GridID, GridData, visibleColumns, options);
        fnGirdDbClick(GridSet, GridData);
    }
    $("#divLoadingImage").hide();
}

/*더블클릭*/
function fnGirdDbClick(Grid, Data) {
    Grid.onDblClick.subscribe(function (e, args) {
        var cell = Grid.getCellFromEvent(e);
        var item = Data[cell.row];
        fnClientIns(item.ClientCode);
    });
}

/*버튼 이벤트*/
/**
 * 고객사 등록
 */
function fnClientIns(ClientCode) {
    var title = "고객사 등록";
    var Mode = "Insert";
    if (typeof ClientCode !== "undefined" && ClientCode !== "") {
        title = "고객사 수정";
        Mode = "Update";
    }

    fnOpenRightSubLayer(title, "/SSO/Client/ClientIns.aspx?HidMode=" + Mode +"&ClientCode=" + ClientCode, "1024px", "700px", "50%");
}

/*엑셀다운로드*/
function fnClientListExcel() {
    fnGridExcelDownload("고객사_리스트", GridData, columns);
}