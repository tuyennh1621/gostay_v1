const firebaseConfig = {
    apiKey: "AIzaSyCgYLUomIjhs4uA2imn5GZpF3YgyJp8hS8",
    authDomain: "gostay-1ae07.firebaseapp.com",
    projectId: "gostay-1ae07",
    storageBucket: "gostay-1ae07.appspot.com",
    messagingSenderId: "565579751675",
    appId: "1:565579751675:web:08ff3773f89159a438905e",
    measurementId: "G-NK9LLCYW8H"
};
// initializing firebase SDK
firebase.initializeApp(firebaseConfig);

function executeVerified(number, status) {
    $.ajax({
        type: "POST",
        traditional: true,
        data:
        {
            "statusStr": JSON.stringify(status),
            "number": JSON.stringify(number),
        },
        url: "/Home/ExecutePhoneVerified",
        success: function (data) {

            if (data.verified) {
                $('#loginform').modal('hide');
                swal("Thông báo!", "Đăng nhập thành công!", "success").then((value) => {
                    window.location.reload();
                });
            }
            /*
            else if (data.verified) {
                window.location = "/Home/RegisterUserPhone?phoneNumber=" + data.mobileNo;
            }
            */
        },
    })
}

function logout() {
    var uid = getCookie("userId");
    setPersistent(uid, false);
    firebase.auth().signOut();
    window.location = "/Home/Logout";
    document.cookie = "userId=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

function loginbyAccount() {
    document.getElementById('btnloginwemail').classList.add("loading");
    document.getElementById('btnloginwemail').disabled = true;
    var email = $("#email").val();
    var password = $("#password").val();
    $.ajax({
        type: "POST",
        traditional: true,
        data:
        {
            "email": JSON.stringify(email),
            "password": JSON.stringify(password),
        },
        url: "/Home/LoginbyAccount",
        success: function (data) {
            if (!data) {
                alert('Sai tên đăng nhập hoặc mật khẩu');

                document.getElementById('sign-in-button').classList.remove("loading");
                document.getElementById('sign-in-button').disabled = false;
            }
            else {
                window.location = "/Home/Index";
            }
        },
        Error: function (data) {
            alert('Có lỗi khi đăng nhập');

            document.getElementById('btnloginwemail').classList.remove("loading");
            document.getElementById('btnloginwemail').disabled = false;
        }
    })
}

$(document).ready(function () {

    $("#loginwidthPhone").show();
    $("#loginwidthEmail").hide();
    $("#btnloginwidthPhone").hide();

    $("#btnloginwidthEmail").click(function () {
        $("#loginwidthPhone").hide();
        $("#loginwidthEmail").show();

        $("#btnloginwidthPhone").show();
        $("#btnloginwidthEmail").hide();
    });

    //$("#btnloginwidthFacebook").click(function () {
    //    window.location = "/Home/FacebookLogin";
    //    $("#loginwidthPhone").show();
    //    $("#loginwidthEmail").hide();
    //});
    $("#btnloginwidthPhone").click(function () {
        //  window.location = "/Home/LoginWithPhone";
        $("#loginwidthPhone").show();
        $("#loginwidthEmail").hide();

        $("#btnloginwidthPhone").hide();
        $("#btnloginwidthEmail").show();
    });
});