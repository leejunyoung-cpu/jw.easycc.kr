$(document).ready(function () {
    //----------------------------------------------------------------------------------
    //-- 회원 로그인 이벤트
    //----------------------------------------------------------------------------------
    $("#btnLogin").on("click", function () {
        event.preventDefault();
        fnGoLogin();
    });

    if ($.cookie("LoginId") != "") {
        $("#AdminID").val($.cookie("LoginId"));
        $('#SaveId').prop('checked', true);
        $("#password").focus();
    }
});


function fnGoLogin() {
    if ($("#AdminID").val() === "") {
        fnDefaultAlertFocus("아이디를 입력해주세요.", "AdminID");
        return;
    }

    if ($("#AdminPwd").val() === "") {
        fnDefaultAlertFocus("비밀번호를 입력해주세요.", "AdminPwd");
        return;
    }
    
    var strHandlerURL = "/SSO/Login/Proc/LoginHandler.ashx";
    var strCallBackFunc = "fnAjaxLoginSuccResult";

    var objParam = {
        CallType: "chkLogin",
        AdminID: $("#AdminID").val(),
        AdminPwd: $("#AdminPwd").val()
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxLoginSuccResult(data) {
    if (data[0].RetCode === 0) {
        fnDefaultAlert(data[0].LastLoginNotice, "success", "fnRootPageReplace", $("#returnurl").val());
        if ($('#SaveId').is(':checked')) {
            $.cookie('LoginId', $("#AdminID").val(), { expires: 365, path: '/' });
        } else {
            $.cookie('LoginId', "", { expires: 365, path: '/' });
        }
    } else {
        fnDefaultAlert("로그인을 실패하였습니다. 나중에 다시 시도해 주세요.<br/>(" + data[0].ErrMsg + ")");
    }
    return;
}