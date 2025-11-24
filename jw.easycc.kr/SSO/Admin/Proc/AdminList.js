// 그리드
var GridID = "#AdminListGrid";
var GridData = [];
$(document).ready(function () {
    fnCallGridData(GridID);
    // '데이터가 없습니다' 메시지를 담을 요소 생성
    $(GridID).after('<div id="noDataMessage">조회된 데이터가 없습니다</div>');
});

var columns = [
    {
        id: "MenuRoleNo",
        name: "일련번호",
        field: "MenuRoleNo",
        width: 0,
        visible: false
    },
    {
        id: "ClientName",
        name: "담당 고객사",
        field: "ClientName",
        width: 200,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "GradeName",
        name: "사용자 등급",
        field: "GradeName",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "AdminAreaTypeM",
        name: "근무지",
        field: "AdminAreaTypeM",
        width: 80,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "AdminID",
        name: "아이디",
        field: "AdminID",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "AdminName",
        name: "이름",
        field: "AdminName",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "MobileNo",
        name: "휴대폰",
        field: "MobileNo",
        width: 150,
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
        name: "가입일시",
        field: "RegDate",
        width: 150,
        cssClass: "slick-cell-tc",
        visible: true
    },
    {
        id: "BtnMenuAccess",
        name: "메뉴사용권한",
        field: "BtnMenuAccess",
        formatter: actionButtonMenuAuth,
        width: 100,
        visible: true
    },
    {
        id: "BtnSendResetPwd",
        name: "신규 비밀번호",
        field: "BtnSendResetPwd",
        formatter: actionButtonNewPassWord,
        width: 100,
        visible: true
    },
    {
        id: "BtnResetAdmin",
        name: "접속 초기화",
        field: "BtnResetAdmin",
        formatter: actionButtonResetAdmin,
        width: 100,
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

//메뉴사용권란
function actionButtonMenuAuth(row, cell, value, columnDef, dataContext) {
    if (Number($("#HidGradeCode").val()) === 1 || Number($("#HidGradeCode").val()) === 2) {
        if (dataContext.GradeCode !== 1) {
            return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnMenuAccess('" + dataContext.AdminID + "')\">조회</button>";
        } else {
            return "-";
        }
    } else {
        return "-";
    }
}

/*접속 초기화*/
function actionButtonResetAdmin(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnResetAdmin('" + dataContext.AdminID + "')\">Reset</button>";
}

/*신규 비밀번호 전송 */
function actionButtonNewPassWord(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnSendResetPassword('" + dataContext.AdminID + "', '" + dataContext.Email + "')\">전송</button>";
}

/*조회*/
function actionButtonFormatter(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnInsAdmin('" + dataContext.AdminID + "')\">조회</button>";
}



/**
 * /
 * 그리드 데이터 호출
 */
function fnCallGridData(strGID) {
    $("#divLoadingImage").show();
    var strHandlerURL = "/SSO/Admin/Proc/AdminHandler.ashx";
    var strCallBackFunc = "fnGridSuccResult";

    var objParam = {
        CallType: "AdminList",
        ClientCode: $("#ClientCode").val(),
        GradeCode: $("#GradeCode").val(),
        UseFlag: $("#UseFlag").val(),
        SearchType: $("#SearchType").val(),
        ListSearch: $("#ListSearch").val(),
        PageNo: $("#PageNo").val(),
        PageSize: $("#PageSize").val()
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

        // 데이터가 없을 경우 '데이터가 없습니다' 메시지 표시
        if (objRes[0].data.RecordCnt === 0) {
            $("#noDataMessage").show();
        } else {
            $("#noDataMessage").hide();
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

/*버튼 이벤트*/
//메뉴권한성정
function fnMenuAccess(strAdminID) {
    fnOpenRightSubLayer("관리자 메뉴 권한 설정", "/SSO/Admin/AdminMenuAccessUpd.aspx?AdminID=" + strAdminID, "1024px", "700px", "50%");
}

/*신규 비밀번호 발급*/
function fnSendResetPassword(strAdminID, strEmail) {
    if (strEmail === "" || typeof strEmail === "undefined") {
        fnDefaultAlert("메일이 등록되어있지 않습니다.<br>(등록 된 이메일로 임시 비밀번호가 전송됩니다.)");
        return;
    }
    var strConfMsg = "해당 계정의 이메일로 비밀번호를 전송하시겠습니까?";
    var objFnParam = {
        AdminID : strAdminID,
        Email   : strEmail
    }

    fnDefaultConfirm(strConfMsg, "fnSendResetPasswordProc", objFnParam);
}

function fnSendResetPasswordProc(objParam) {
    var strHandlerURL = "/SSO/Admin/Proc/AdminHandler.ashx";
    var strCallBackFunc = "fnAjaxSendPwdSuccResult";

    var objFnParam = {
        CallType: "AdminSendPwd",
        AdminID: objParam.AdminID,
        Email: objParam.Email
    }
    UTILJS.Ajax.fnHandlerRequest(objFnParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxSendPwdSuccResult(objData) {

    if (objData[0].RetCode !== 0) {
        fnDefaultAlert("신규 비밀번호 전송을 실패하였습니다. 나중에 다시 시도해 주세요.(" + objData[0].ErrMsg + ")");
    } else {
        fnDefaultAlert(objData[0].ErrMsg, "success");
    }

    return;
}

function fnResetAdmin(strAdminID) {
    var strConfMsg = "로그인 접속을 초기화 하시겠습니까?";
    var objFnParam = {
        AdminID: strAdminID
    }

    fnDefaultConfirm(strConfMsg, "fnResetAdminProc", objFnParam);
    return;
}

function fnResetAdminProc(objParam) {
    var strHandlerURL = "/SSO/Admin/Proc/AdminHandler.ashx";
    var strCallBackFunc = "fnAjaxResetAdmin";
    var objFnParam = {
        CallType: "AdminLoginReset",
        AdminID: objParam.AdminID
    }

    UTILJS.Ajax.fnHandlerRequest(objFnParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxResetAdmin(objData) {

    if (objData[0].RetCode !== 0) {
        fnDefaultAlert("초기화를 실패하였습니다. 나중에 다시 시도해 주세요.(" + objData[0].ErrMsg + ")");
    } else {
        fnDefaultAlert("로그인 접속이 초기화 되었습니다.", "success");
    }

    return;
}
/**
 * 사용자 조회
 */
function fnInsAdmin(strAdminID) {
    var title = "회원 등록";
    if (typeof strAdminID !== "undefined" && strAdminID !== "") {
        title = "회원 수정";
    }

    fnOpenRightSubLayer(title, "/SSO/Admin/AdminIns.aspx?AdminID=" + strAdminID, "1024px", "700px", "50%");
}