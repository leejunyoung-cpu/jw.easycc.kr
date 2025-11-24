// 그리드
var GridID = "#AdminMenuRoleListGrid";
var GridData = [];

$(document).ready(function () {
    fnCallGridData(GridID);
});


var columns = [
    {
        id: "MenuRoleNo",
        name: "일련번호",
        field: "MenuRoleNo",
        width: 0,
        visible : false
    },
    {
        id: "MenuRoleName",
        name: "메뉴역할명",
        field: "MenuRoleName",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "UseFlag",
        name: "사용여부",
        field: "UseFlag",
        width: 150,
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
        id: "UpdDate",
        name: "수정일시",
        field: "UpdDate",
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
    },
    {
        id: "BtnDelAdmin",
        name: "삭제",
        field: "BtnDelAdmin",
        formatter: actionDelAdminMenuRole,
        width: 100,
        visible: true
    }
];

/**
 * 버튼 이벤트
 */
function actionButtonFormatter(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"InsAdminMenuRole('메뉴 역할 수정','" + dataContext.MenuRoleNo + "')\">조회</button>";
}

function actionDelAdminMenuRole(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01 gray\" data-row=\"" + row + "\" onclick=\"fnDelAdminMenuRole('" + dataContext.MenuRoleNo + "')\">삭제</button>";
}

var options = {
    editable: true,
    enableAddRow: false,
    enableCellNavigation: true,
    asyncEditorLoading: false,
    rowHeight: 35
};

/**
 * /
 * 그리드 데이터 호출
 */
function fnCallGridData(strGID) {
    $("#divLoadingImage").show();
    var strHandlerURL = "/SSO/Admin/Proc/AdminMenuRoleHandler.ashx";
    var strCallBackFunc = "fnGridSuccResult";

    var objParam = {
        CallType: "AdminMenuRoleList",
        PageNo: $("#PageNo").val()
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
        // 가시성이 true인 컬럼만 필터링
        var visibleColumns = columns.filter(column => column.visible);
        Grid = new Slick.Grid(GridID, objRes[0].data.list, visibleColumns, options);
    }
    $("#divLoadingImage").hide();
}
function fnDelAdminMenuRole(strMenuRoleNo) {
    var strConfMsg;

    strConfMsg = "삭제한 이후에는 복원할 수 없습니다.\n메뉴 역할을 삭제하시겠습니까?";
    //Confirm
    var fnParam = strMenuRoleNo;
    fnDefaultConfirm(strConfMsg, "fnDelAdminMenuRoleProc", fnParam);

    return;
}

function fnDelAdminMenuRoleProc(ojbParam) {
    var strHandlerURL = "/SSO/Admin/Proc/AdminMenuRoleHandler.ashx";
    var strCallBackFunc = "fnAjaxDelAdminMenuRole";
    
    let objParam = {
        CallType: "AdminMenuRoleDelete",
        MenuRoleNo: ojbParam,
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);

}

function fnAjaxDelAdminMenuRole(data) {
    if (data[0].RetCode !== 0) {
        fnDefaultAlert("나중에 다시 시도해 주세요.(" + data[0].ErrMsg + ")");
    } else {     
        fnDefaultAlert("삭제 성공하였습니다");
        fnMoveToPage(1);
    }
}
