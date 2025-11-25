// 그리드
var GridID = "#ApiWhiteListGrid";
var GridData = [];
var GridSet;

$(document).ready(function () {
    fnCallGridData(GridID);
});

var columns = [
    {
        id: "SeqNo",
        name: "일련번호",
        field: "SeqNo",
        width: 0,
        visible: false
    },
    {
        id: "ServiceCode",
        name: "서비스코드",
        field: "ServiceCode",
        width: 200,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "Company",
        name: "업체코드",
        field: "Company",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "IpAddress",
        name: "IP주소",
        field: "IpAddress",
        width: 200,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "IsActive",
        name: "활성화",
        field: "IsActive",
        width: 100,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "ServiceUrl",
        name: "서비스URL",
        field: "ServiceUrl",
        width: 200,
        visible: true
    },
    {
        id: "IpNote",
        name: "설명",
        field: "IpNote",
        width: 200,
        visible: true
    },
    {
        id: "BtnInsAdmin",
        name: "조회",
        field: "BtnInsAdmin",
        formatter: actionButtonFormatter,
        width: 100,
        visible: true
    },
    {
        id: "BtnDel",
        name: "삭제",
        field: "BtnDel",
        formatter: fnApiWhiteDelBtn,
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
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnApiWhiteIns('" + dataContext.SeqNo + "')\">조회</button>";
}

function fnApiWhiteDelBtn(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_03\" data-row=\"" + row + "\" onclick=\"fnApiWhiteDelComfirm('" + dataContext.SeqNo + "')\">삭제</button>";
}
/**
 * /
 * 그리드 데이터 호출
 */
function fnCallGridData(strGID) {
    $("#divLoadingImage").show();
    var strHandlerURL = "/SSO/API/Proc/ApiWhiteHandler.ashx";
    var strCallBackFunc = "fnGridSuccResult";

    var objParam = {
        CallType        : "ApiWhiteList",
        ServiceCode     : $("#ServiceCode").val(),
        Company         : $("#Company").val(),
        ClientCeoName   : $("#ClientCeoName").val(),
        IsActive        : $("#IsActive").val(),
        ServiceUrl      : $("#ServiceUrl").val(),
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
        GridData = objRes[0].data.List;
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
function fnApiWhiteIns(ClientCode) {
    var title = "허용 IP 등록";
    var Mode = "Insert";
    if (typeof ClientCode !== "undefined" && ClientCode !== "") {
        title = "허용 IP 수정";
        Mode = "Update";
    }

    fnOpenRightSubLayer(title, "/SSO/API/ApiWhiteIns.aspx?HidMode=" + Mode +"&SeqNo=" + ClientCode, "1024px", "700px", "50%");
}

function fnApiWhiteDelComfirm(SeqNo) {
    fnDefaultConfirm("해당 허용 IP를 삭제하시겠습니까?", "fnApiWhiteDel", SeqNo);
    return;
}

function fnApiWhiteDel(SeqNo) {
    var strHandlerURL = "/SSO/API/Proc/ApiWhiteHandler.ashx";
    var strCallBackFunc = "fnDelApiWhiteResult";

    var objParam = {
        CallType: "ApiWhiteDel",
        SeqNo: SeqNo
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnDelApiWhiteResult(data) {
    if (data) {
        if (data[0].RetCode !== 0) {
            fnDefaultAlert("나중에 다시 시도해 주세요.<br />(" + objData[0].ErrMsg + ")", "warning");
            return;
        } else {
            fnDefaultAlert("삭제되었습니다.", "success", "fnCallGridData", "");
            return;
        }
    }
}