$(document).ready(function () {
    if ($("#hidDisplayMode").val() == "Y") {
        if (parent) {
            parent.fnReloadPageNotice($("#hidErrMsg").val());
        }
        else if (opener) {
            opener.fnReloadPageNotice($("#hidErrMsg").val());
        }
    }

    $("#ExpireYMD").datepicker({
        changeMonth: true,
        changeYear: true,
        monthNamesShort: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
        dayNamesMin: ['일', '월', '화', '수', '목', '금', '토'],
        monthNames: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
        dateFormat: "yy-mm-dd"
    });

    fnSetInitData();
});

function fnSetInitData() {
    if ($("#hidMode").val() === "insert") {
        $("#lblMode").html("등록");
        $("#ClientCodeTr").hide();
    }
    else {
        $("#lblMode").html("수정");
        $("#CorpNo").attr("readonly", true);
        $("#CorpNo").css("background-color", "#e7e7e7");
        $("#CorpNo").addClass("readonly");
        $("#CorpName").show();
        $("#CorpNoBtn").hide();

        $("#AdminID").attr("readonly", true);
        $("#AdminID").css("background-color", "#e7e7e7");
        $("#AdminID").addClass("readonly");
        $("#MobileNo").attr("readonly", true);
        $("#MobileNo").css("background-color", "#e7e7e7");
        $("#MobileNo").addClass("readonly");
        $("#AdminIDBtn").hide();
        $("#GradeTable").show();
        
        if (Number($("#GradeCode").val()) >= 7) {
            $("#GradeCode option:not(:selected)").attr("disabled", true);
            $("#GradeCode").css("background-color", "#e7e7e7");
            $("#ClientName").attr("readonly", true);
            $("#ClientCodes").hide();
            $("#ClientCode").show();
        }
    }

    $("#GradeCode").on("change", function () {
        if (Number($("#GradeCode").val()) <= 2) {
            $("#ClientCodeTr").hide();
        } else if (Number($("#GradeCode").val()) >= 7) {
            $("#ClientCodes").hide();
            $("#ClientCode").show();
            $("#ClientCodeTr").show();
        } else {
            $("#ClientCodes").show();
            $("#ClientCode").hide();
            $("#ClientCodeTr").show();
        }
    });

    $("#AdminID").keypress(function (e) {
        $("#hidAdminIDFlag").val("");
        if (e.keyCode === 13) {
            fnChkAdminID(1);
            return false;
        }
    });
}

function fnCloseThisLayer() {
    parent.fnClosePopUpLayer();
}

function fnInsAdmin() {
    var strConfMsg = "";
    var strAccessCenterCode = "";
    var strCallType;
    var arrCenterCode = $("input[name='ClientCodes']");
    var arrCenterCode = $('#ClientCodes input:checkbox:checked').map(function () { return this.value; }).get().join(",");
    
    if (Number($("#GradeCode").val()) >= 7) {
        if (Number($("#GradeCode").val()) >= 7) {
            if ($("#ClientCode").val() === "") {
                fnDefaultAlertFocus("고객사를 선택하세요.", "ClientCode", "warning");
                return;
            }
        } else {
            if (arrCenterCode === "") {
                fnDefaultAlert("고객사를 선택하세요.", "warning");
                return;
            }
        }
    }

    if (!$("#GradeCode").val()) {
        fnDefaultAlertFocus("사용자 등급을 선택하세요.", "GradeCode", "warning");
        return;
    }

    if (!$("#AdminID").val() ) {
        fnDefaultAlertFocus("아이디를 입력하세요.", "AdminID", "warning");
        return;
    }

    if ($("#hidMode").val() === "insert") {
        if (!UTILJS.Util.fnValidId($("#AdminID").val())) {
            fnDefaultAlertFocus("아이디는 영문자 또는 숫자 조합 6~20자로 입력하세요.", "AdminID", "warning");
            return;
        }

        if ($("#hidAdminIDFlag").val() === "") {
            fnDefaultAlert("아이디 중복 확인이 필요합니다.", "warning");
            return;
        }
    }

    if (!$("#AdminName").val()) {
        fnDefaultAlertFocus("이름을 입력하세요.", "AdminName", "warning");
        return;
    }

    if (!$("#MobileNo").val()) {
        fnDefaultAlertFocus("휴대폰 번호를 입력하세요.", "MobileNo", "warning");
        return;
    }

    if (!UTILJS.Util.fnCellNoChk($("#MobileNo").val())) {
        fnDefaultAlertFocus("휴대폰번호가 올바르지 않습니다.", "MobileNo", "warning");
        return;
    }

    if ($("#Email").val()) {
        if (!UTILJS.Util.fnValidEmail($("#Email").val())) {
            fnDefaultAlertFocus("이메일이 올바르지 않습니다.", "Email", "warning");
            return;
        }
    } else {
        fnDefaultAlertFocus("이메일은 필수입력 항목입니다.", "Email", "warning");
        return;
    }

    if (!$("#BtoBYN").prop("checked") && !$("#BtoCYN").prop("checked")) {
        fnDefaultAlert("서비스 유형을 체크해주세요.(BtoB 또는 BtoC)", "warning");
        return;
    }

    if ($("#PrivateAvailFlag").val() === "") {
        fnDefaultAlertFocus("개인정보 동의여부를 선택하세요.", "PrivateAvailFlag", "warning");
        return;
    }

    if ($("#PrivateAvailFlag").val() !== "Y") {
        $("#UseFlag").val("D");
        strConfMsg = "개인정보 미동의시 계정을 사용할 수 없습니다.<br>";
    }

    if (!$("#UseFlag").val()) {
        fnDefaultAlertFocus("계정 사용 여부를 선택하세요.", "UseFlag", "warning");
        return;
    }

    if ($("#hidMode").val() === "update") {
        strCallType = "AdminUpdate";
        strConfMsg += "수정하시겠습니까?";
    }
    else {
        strCallType = "AdminInsert";
        strConfMsg += "등록하시겠습니까?";
    }

    //Confirm
    var objFnParam = { CallType: strCallType };
    fnDefaultConfirm(strConfMsg, "fnInsAdminProc", objFnParam);

    return;
}

function fnInsAdminProc(objParam) {
    var strHandlerURL = "/SSO/Admin/Proc/AdminHandler.ashx";
    var strCallBackFunc = "fnAjaxInsAdmin";

    var objParam = {
        CallType            : objParam.CallType,
        AdminID             : $("#AdminID").val(),
        MobileNo            : $("#MobileNo").val(),
        AdminName           : $("#AdminName").val(),
        GradeCode           : $("#GradeCode").val(),
        DeptName            : $("#DeptName").val(),
        TelNo               : $("#TelNo").val(),
        Email               : $("#Email").val(),
        BtoBYN              : $("#BtoBYN").prop("checked") ? "Y" : "N",
        BtoCYN              : $("#BtoCYN").prop("checked") ? "Y" : "N",
        AdminAreaType       : $("#AdminAreaType").val(),
        UpsCsType           : $("#UpsCsType").val(),
        AccessClientCode    : Number($("#GradeCode").val()) >= 7 ? $("#ClientCode").val() : $('#ClientCodes input:checkbox:checked').map(function () { return this.value; }).get().join(","),
        ExpireYMD           : $("#ExpireYMD").val(),
        UseFlag             : $("#UseFlag").val(),
        PrivateAvailFlag    : $("#PrivateAvailFlag").val()
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxInsAdmin(data) {
    if (data[0].RetCode !== 0) {
        fnDefaultAlert("나중에 다시 시도해 주세요.(" + data[0].ErrMsg + ")");
        return;
    } else {
        fnDefaultAlert("요청을 성공하였습니다.", "success", "fnAjaxInsAdminComplete", { AdminID: $("#AdminID").val() });
        parent.fnCallGridData("#AdminListGrid");
        return;
    }
}

function fnAjaxInsAdminComplete(objParam) {
    
    if (typeof objParam.AdminID === "undefined") {
        return false;
    }

    document.location.replace("/SSO/Admin/AdminIns?AdminID=" + objParam.AdminID);
}

function fnAjaxChkCorpNo(data) {

    if (data[0].RetCode === 0) {
        //가입가능
        $("span.corpno_fail").hide();
        $("span.corpno_fail").text("");
        $("span.corpno_pass").show();
        $("#CorpName").show();
        $("#CorpName").val(data[0].CorpName);
        $("#hidCorpNoFlag").val("Y");
        $("#divLoadingImage").hide();
        $("#CorpNo").attr("readonly", true);
        $("#CorpNoBtn").hide();
        $("#CorpNoBtnReturn").show();
    } else {
        //가입 불가능
        $("span.corpno_fail").show();
        $("span.corpno_fail").text(data[0].ErrMsg);
        $("span.corpno_pass").hide();
        $("#CorpName").hide();
        $("#hidCorpNoFlag").val("");
        $("#divLoadingImage").hide();
        $("#CorpNo").attr("readonly", false);
        $("#CorpNoBtn").show();
        $("#CorpNoBtnReturn").hide();
    }

    return;
}

//아이디 중복체크
function fnChkAdminID(type) {
    if (type === 2) {
        $("#AdminID").attr("readonly", false);
        $("#AdminID").val("");
        $("#AdminID").focus();
        $("#hidAdminIDFlag").val("");
        $("#AdminIDBtn").show();
        $("#AdminIDBtnReturn").hide();
        $("span.id_fail").hide();
        $("span.id_pass").hide();
        return;
    }

    if (!$("#AdminID").val()) {
        fnDefaultAlertFocus("아이디를 입력해주세요.", "AdminID", "warning");
        return;
    }

    if (!UTILJS.Util.fnValidId($("#AdminID").val())) {
        fnDefaultAlertFocus("아이디는 영문자 또는 숫자 조합 6~20자로 입력하세요.", "AdminID", "warning");
        return;
    }

    var strHandlerURL = "/SSO/Admin/Proc/AdminHandler.ashx";
    var strCallBackFunc = "fnAjaxChkAdminID";

    var objParam = {
        CallType: "AdminIDCheck",
        AdminID: $("#AdminID").val(),
        MemberType: type
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxChkAdminID(data) {

    if (data[0].RetCode === 0) {
        //가입가능
        $("span.id_fail").hide();
        $("span.id_pass").show();
        $("#hidAdminIDFlag").val("Y");
        $("#divLoadingImage").hide();
        $("#AdminID").attr("readonly", true);
        $("#AdminID").addClass("readonly");
        $("#AdminIDBtn").hide();
        $("#AdminIDBtnReturn").show();
    } else {
        //가입 불가능
        $("span.id_fail").show();
        $("span.id_pass").hide();
        $("#hidAdminIDFlag").val("");
        $("#divLoadingImage").hide();
    }

    return;
}

function fnClientReset() {
    $("#ClientName").val("");
    $("#ClientName").attr("readonly", false);
    $("#ClientName").focus();
    $("#ClientCorpNo").val("");
    $("#ClientResetBtn").hide();
}