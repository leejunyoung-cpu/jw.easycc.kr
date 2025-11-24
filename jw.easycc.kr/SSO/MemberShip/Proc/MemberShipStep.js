$(document).ready(function () {
    $("input[ID=AdminPwd]").keyup(function (event) {
        if ($("#AdminPwd").val() === "") {
            $("p.info_text").css("color", "#E23D77").text("비밀번호는 영문,숫자,특수문자(!@$%^*만 허용)를 혼합 사용하여 8~16자까지");
            $("#SecurityFlag").val("");
            return;
        }
    });

    $('input[ID=AdminPwd]').focusout(function () {
        if ($("#AdminPwd").val() !== "") {
            var strHandlerURL = "/SSO/MemberShip/Proc/MemberShipStepHandler.ashx";
            var strCallBackFunc = "fnAjaxPwdPolicySuccResult";

            var objParam = {
                CallType: "CheckAdminPwdPolicy",
                AdminPwd: $("#AdminPwd").val()
            }

            UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", false);
        }
    });

});


function fnAjaxPwdPolicySuccResult(data) {
    if (data[0].RetCode !== 0) {
        $("p.password_text").css("color", "#E23D77").text(data[0].ErrMsg);
        $("#SecurityFlag").val("");
        $("#AdminPwd").focus();
    } else {
        $("p.password_text").css("color", "#0095F6").text("안전한 비밀번호입니다.");
        $("#SecurityFlag").val("Y");
    }
}

//사업자번호 체크
function fnCropNoChk() {

    if (!$("#CorpNo").val()) {
        fnDefaultAlertFocus("사업자번호를 입력하세요.", "CorpNo", "warning");
        return false;;
    }

    if (!UTILJS.Util.fnBizNoChk($("#CorpNo").val())) {
        fnDefaultAlertFocus("사업자번호가 올바르지 않습니다.", "CorpNo", "warning");
        return false;;
    }

    var strHandlerURL = "/SSO/MemberShip/Proc/MemberShipStepHandler.ashx";
    var strCallBackFunc = "fnChkCorpNoSuccResult";
    var strFailCallBackFunc = "fnChkCorpNoFailResult";

    var objParam = {
        CallType: "ClientCorpNoChk",
        CorpNo: $("#CorpNo").val()
    }

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, strFailCallBackFunc, "", true);
}

function fnChkCorpNoSuccResult(objRes) {
    if (objRes) {
        if (objRes[0].RetCode !== 0) {
            fnChkCorpNoFailResult(objRes);
            return false;;
        }

        $("#CorpNo").attr("readonly", true);
        $("#CorpNo").val(objRes[0].CorpNo);
        $("#BtnCorpNoChk").hide();
        $("#BtnCorpNoReChk").show();
        $("div.form_tb p.cnts").hide();
        $("div.form_tb p.cnts").html("");
        $("#ClientCeoName").val(objRes[0].ClientCeoName);
        $("#ClientName").val(objRes[0].ClientName);
        $("#ClientCode").val(objRes[0].ClientCode);
        return;
    } else {
        fnChkCorpNoFailResult(objRes);
    }
}

function fnChkCorpNoFailResult(objRes) {
    $("div.form_tb p.cnts").show();
    $("div.form_tb p.cnts").html(objRes[0].ErrMsg);
}

function fnCropNoChkReset() {
    $("#CorpNo").attr("readonly", false);
    $("#HidCorpNoChk").val("");
    $("#CorpNo").val("");
    $("#ClientCode").val("");
    $("#ClientName").val("");
    $("#ClientCeoName").val("");
    $("#BtnCorpNoChk").show();
    $("#BtnCorpNoReChk").hide();
}

function fnAdminIDChk() {
    if ($("#AdminID").val() === "") {
        fnDefaultAlertFocus("아이디를 입력해주세요", "AdminID", "warning");
        return;
    }

    if (!UTILJS.Util.fnValidId($("#AdminID").val())) {
        fnDefaultAlertFocus("아이디는 영문자 또는 숫자 조합 6~20자로 입력하세요.", "AdminID", "warning");
        return;
    }

    var strHandlerURL = "/SSO/MemberShip/Proc/MemberShipStepHandler.ashx";
    var strCallBackFunc = "fnAjaxChkAdminID";

    var objParam = {
        CallType: "AdminIDCheck",
        AdminID: $("#AdminID").val()
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxChkAdminID(data) {
    if (data[0].RetCode === 0) {
        //가입가능
        $("#hidAdminIDFlag").val("Y");
        $("#AdminID").attr("readonly", true);
        $("#AdminIDBtn").hide();
        $("#AdminIDBtnReturn").show();
        fnDefaultAlertFocus("등록 가능한 아이디입니다.", "AdminName", "warning");
    } else {
        //가입 불가능
        fnDefaultAlertFocus(data[0].ErrMsg, "AdminID", "warning");
        $("#hidAdminIDFlag").val("");
    }
    $("#divLoadingImage").hide();
    return;
}

function fnAdminIDChkReset() {
    $("#hidAdminIDFlag").val("");
    $("#AdminID").attr("readonly", false);
    $("#AdminIDBtn").show();
    $("#AdminIDBtnReturn").hide();
}

function fnAdminRequestIns() {
    if (fnAdminValid()) {
        fnDefaultConfirm("회원가입을 진행하시겠습니까?", "fnAdminIns", "");
        return;
    }
}

function fnAdminIns() {
    var strHandlerURL = "/SSO/MemberShip/Proc/MemberShipStepHandler.ashx";
    var strCallBackFunc = "fnAjaxChkAdminIns";

    var objParam = {
        CallType: "AdminInsert",
        AdminID: $("#AdminID").val(),
        AdminName: $("#AdminName").val(),
        TelNo: $("#TelNo").val(),
        MobileNo: $("#MobileNo").val(),
        Email: $("#Email").val(),
        AdminPwd: $("#AdminPwd").val(),
        PrivateAvailFlag: $("#HidAgreeFlag").val(),
        ClientCode: $("#ClientCode").val()
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxChkAdminIns(data) {
    if (data[0].RetCode != 0) {
        fnDefaultAlert(data[0].ErrMsg);
        return false;
    } else {
        fnDefaultAlert("가입이 완료되었습니다.(가입 승인대기)<br>가입 승인은 고객사 관리자에게 문의하여<br>승인받으십시오.", "success", "fnLoginPage", "");
        return;
    }
}

function fnLoginPage() {
    location.href = "/SSO/Login/Login";
    return;
}

function fnAdminValid() {
    if ($("#ClientCode").val() === "") {
        fnDefaultAlert("고객사 사업자번호 조회가 진행되지 않았습니다.")
    }

    if ($("#AdminID").val() === "") {
        fnDefaultAlertFocus("아이디를 입력해주세요.", "AdminID", "warning");
        return false;
    }

    if ($("#hidAdminIDFlag").val() != "Y") {
        fnDefaultAlertFocus("아이디 중복확인이 필요합니다.", "AdminID", "warning");
        return false;
    }

    if ($("#AdminName").val() === "") {
        fnDefaultAlertFocus("이름을 입력해주세요.", "AdminID", "warning");
        return false;
    }

    if ($("#MobileNo").val() === "") {
        fnDefaultAlertFocus("휴대폰번호를 입력하세요.", "MobileNo", "warning");
        return false;
    } else {
        if (!UTILJS.Util.fnCellNoChk($("#MobileNo").val())) {
            fnDefaultAlertFocus("휴대폰번호 형식이 올바르지않습니다.", "MobileNo", "warning");
            return false;
        }
    }
    

    if ($("#Email").val() === "") {
        fnDefaultAlertFocus("Email을 입력해주세요.", "Email", "warning");
        return false;
    }

    if ($("#AdminPwd").val() === "") {
        fnDefaultAlertFocus("비밀번호를 입력해주세요.", "AdminPwd", "warning");
        return false;
    }

    if ($("#AdminPwdConfirm").val() === "") {
        fnDefaultAlertFocus("비밀번호 확인을 입력해주세요.", "AdminPwdConfirm", "warning");
        return false;
    }

    if ($("#AdminPwd").val() != $("#AdminPwdConfirm").val()) {
        fnDefaultAlertFocus("입력하신 비밀번호가 다릅니다.", "AdminPwdConfirm", "warning");
        return false;
    }

    if ($("#SecurityFlag").val() != "Y") {
        fnDefaultAlertFocus("비밀번호가 안전하지 않습니다.<br>다시 입력해주세요.", "AdminPwd", "warning");
        return false;
    }
    return true;
}