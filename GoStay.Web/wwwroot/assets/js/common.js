$(document).ready(function () {
    $(".newsletter .newsletter-form .btn").attr("type", "button");
    $(".newsletter .newsletter-form .mail-valid").hide();
    $(".newsletter .newsletter-form .btn").click(function () {
        var email = $(".newsletter .newsletter-form input[name=EMAIL]").val();
        preCheckEmail(email);
    })
})

function preCheckEmail(input) {

    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (input.match(validRegex)) {
        debugger
        $(".newsletter .newsletter-form .mail-valid").hide();
        subpriceEmail(input);
        $(".newsletter .newsletter-form input[name=EMAIL]").val('')
        //var local = localStorage.getItem("emailLocalStorage");
        //if (local == null || local == '' || local == undefined) {
        //    localStorage.setItem("emailLocalStorage", input);
        //    subpriceEmail(input);
        //} else if (local == input) {
        //    alert("Email đã tồn tại");
        //} else {
        //    subpriceEmail(input);
        //}
    } else {
        $(".newsletter .newsletter-form .mail-valid").show();
        return false;
    }
}

function preCheckEmail2(id) {
    debugger;
    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    var abc = $("#" + id).val();
    if (abc.match(validRegex)) {
        debugger
        $(".newsletter .newsletter-form .mail-valid").hide();
        subpriceEmail(input);
        $(".newsletter .newsletter-form input[name=EMAIL]").val('')
        //var local = localStorage.getItem("emailLocalStorage");
        //if (local == null || local == '' || local == undefined) {
        //    localStorage.setItem("emailLocalStorage", input);
        //    subpriceEmail(input);
        //} else if (local == input) {
        //    alert("Email đã tồn tại");
        //} else {
        //    subpriceEmail(input);
        //}
    } else {
        $(".newsletter .newsletter-form .mail-valid").show();
        return false;
    }
}
function subpriceEmail(email) {
    debugger
    $.ajax({
        type: "POST",
        url: "/Admin/FireBaseHome/SupriceEmail",
        data:
        {
            email : email
        },
        beforeSend: function () {

            showSearchLoader();
        },
        success: function (data) {
            if (data.ErrorCode == 0) {
                swal("success", data.ErrorMess, "success");
            } else {
                swal("warning", data.ErrorMess, "warning");
                
            }
            hideSearchLoader();
        },
        error: function (e) {
            swal("error", e, "error");
        }
    })
}

function titleChange(input) {
    let str = input;
    if (str != '') str = str.trim();
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`| |{|}|\||\\/g, "-");
    str = str.replace(/ + /g, " ");
    str = str.trim();
    return str;
}
function replaceStrIndex(string, index, replacement) {
    if (index >= string.length) {
        return string.valueOf();
    }
    return string.substring(0, index) + replacement;
};
function pushStateUrl(current, id, newUrl) {
    var index = current.indexOf(id);
    var url = id + "/" + newUrl;
    url = replaceStrIndex(current, index, url)
    window.history.pushState("", "", url);
}
function checkUrlTitle() {
    var newsTitle = $("#url-title").attr("data-slug");
    var newsId = $("#url-title").attr("data-id");
    var currentUrl = window.location.pathname;
    if (!currentUrl.includes(newsTitle)) {
        pushStateUrl(currentUrl, newsId, newsTitle)
    }
    return 0;
}
function replaceRouter() {
    var res = 0;
    $('.format-router').each(function () {
        res++;
        let attrHref = $(this).attr("href");
        let index = attrHref.indexOf("/--");
        let router = attrHref.substring(index + 3, attrHref.length);
        router = titleChange(router);
        let mapRouter = replaceStrIndex(attrHref, index + 1, router);
        $(this).attr("href", mapRouter);
    });
    console.log("res:.... ", res);
}

var provinceDataJson = [{
        id: 5,
        url: "/ha-noi",
    }, {
        id: 6,
        url: "/tp-hcm",
    }, {
        id: 8,
        url: "/dak-lak",
    }, {
        id: 9,
        url: "/an-giang",
    }, {
        id: 10,
        url: "/binh-duong",
    }, {
        id: 11,
        url: "/bac-lieu",
    }, {
        id: 12,
        url: "/bac-ninh",
    }, {
        id: 13,
        url: "/can-tho",
    }, {
        id: 14,
        url: "/ha-giang",
    }, {
        id: 16,
        url: "/hung-yen",
    }, {
        id: 17,
        url: "/kontum",
    }, {
        id: 18,
        url: "/lang-son",
    }, {
        id: 20,
        url: "/nam-dinh",
    }, {
        id: 22,
        url: "/quang-binh",
    }, {
        id: 23,
        url: "/quang-ninh",
    }, {
        id: 24,
        url: "/son-la",
    }, {
        id: 25,
        url: "/thai-nguyen",
    }, {
        id: 26,
        url: "/tien-giang",
    }, {
        id: 27,
        url: "/vinh-long",
    }, {
        id: 28,
        url: "/da-nang",
    }, {
        id: 29,
        url: "/dong-thap",
    }, {
        id: 30,
        url: "/binh-dinh",
    }, {
        id: 31,
        url: "/binh-thuan",
    }, {
        id: 32,
        url: "/bac-giang",
    }, {
        id: 33,
        url: "/ca-mau",
    }, {
        id: 34,
        url: "/gia-lai",
    }, {
        id: 35,
        url: "/ha-nam",
    }, {
        id: 36,
        url: "/hoa-binh",
    }, {
        id: 37,
        url: "/hai-phong",
    }, {
        id: 38,
        url: "/kien-giang",
    }, {
        id: 39,
        url: "/lam-dong",
    }, {
        id: 40,
        url: "/long-an",
    }, {
        id: 41,
        url: "/ninh-binh",
    }, {
        id: 42,
        url: "/phu-yen",
    }, {
        id: 43,
        url: "/quang-ngai",
    }, {
        id: 44,
        url: "/soc-trang",
    }, {
        id: 45,
        url: "/thai-binh",
    }, {
        id: 46,
        url: "/thanh-hoa",
    }, {
        id: 47,
        url: "/tuyen-quang",
    }, {
        id: 48,
        url: "/yen-bai",
    }, {
        id: 49,
        url: "/dong-nai",
    }, {
        id: 50,
        url: "/ba-ria-vung-tau",
    }, {
        id: 51,
        url: "/binh-phuoc",
    }, {
        id: 52,
        url: "/bac-kan",
    }, {
        id: 53,
        url: "/ben-tre",
    }, {
        id: 54,
        url: "/cao-bang",
    }, {
        id: 55,
        url: "/ha-tinh",
    }, {
        id: 56,
        url: "/hai-duong",
    }, {
        id: 57,
        url: "/khanh-hoa",
    }, {
        id: 58,
        url: "/lao-cai",
    }, {
        id: 59,
        url: "/lai-chau",
    }, {
        id: 60,
        url: "/nghe-an",
    }, {
        id: 61,
        url: "/phu-tho",
    }, {
        id: 62,
        url: "/quang-nam",
    }, {
        id: 63,
        url: "/quang-tri",
    }, {
        id: 64,
        url: "/tay-ninh",
    }, {
        id: 65,
        url: "/hue",
    }, {
        id: 66,
        url: "/tra-vinh",
    }, {
        id: 67,
        url: "/vinh-phuc",
    }, {
        id: 68,
        url: "/ninh-thuan",
    }, {
        id: 0,
        url: "/viet-nam",
    }, {
        id: 70,
        url: "/hau-giang",
    }, {
        id: 71,
        url: "/dien-bien",
    }, {
        id: 72,
        url: "/dak-nong",
    }

]
function setUrlProvince(page) {
    var currentUrl = window.location.search;
    if (currentUrl.includes("IdTinhthanh")) {
        var indexEnd = currentUrl.indexOf("&");
        var indexStart = currentUrl.indexOf("=");
        indexEnd = (indexEnd > 0) ? indexEnd : currentUrl.length;
        var idProvince = currentUrl.substring(indexStart + 1, indexEnd);
        idProvince = parseInt(idProvince);
        var result = provinceDataJson.find(item => item.id == idProvince);
        result = result.url;
        var urlTemp = currentUrl.substring(indexEnd + 1, currentUrl.length);
        if (urlTemp == null || urlTemp == undefined || urlTemp == "") {
            resultUrl = "/" + page + result;
        }
        else {
            resultUrl = "/" + page + result + "?" + urlTemp;
        }
        window.history.pushState("", "", resultUrl)
    } else return 0;
}

function changePrice() {

    var max = parseInt($("#rangTo").val());
    var min = parseInt($("#rangFrom").val());
    if (max > min) {
        $("#maxPrice").text(formatMoney(max));
        $("#minPrice").text(formatMoney(min));
    } else {
        $("#maxPrice").text(formatMoney(min));
        $("#minPrice").text(formatMoney(max));
    }
};
function formatMoney(amount, decimalCount = 0, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 0 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign +
            (j ? i.substr(0, j) + thousands : '') +
            i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) +
            (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    }
    catch (e) {
    }
};
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function setUID(uId) {
    $(".dropdown-menu li .set-uid").each(function () {
        let tempHref = $(this).attr("href");
        tempHref = tempHref + "?uId=" + uId + "__" + localStorage.getItem('MachineId');
        $(this).attr("href", tempHref);
        console.log("test 1");
    })
}
function getMachineId() {

    let machineId = localStorage.getItem('MachineId');

    if (!machineId) {
        machineId = crypto.randomUUID();
        localStorage.setItem('MachineId', machineId);
    }

    return machineId;
}
getMachineId();
function setPersistent(uid, auth) {

    var deviceId = localStorage.getItem('MachineId');
    var isFirstLogin = false;
    if (deviceId) {
        deviceId = getMachineId();
        isFirstLogin = true;
    }
    debugger;
    console.log("testr : .. ", isFirstLogin)
    $.ajax({
        type: "POST",
        url: "/Admin/FireBaseHome/SavePersistent",
        data:
        {
            tableName: "Persistent",
            uid: uid,
            isLogin: auth,
            deviceId: deviceId,
            isFirstLogin: isFirstLogin
        },
        beforeSend: function () {

        },
        success: function (data) {
            console.log(data)
        },
        error: function (e) {
            swal("error", data.ErrorMess, "error");
        }
    })
}
function showPopupLogin() {
    var pLogin = document.getElementById('loginFormHome');
    pLogin.click();
}

function setLocalStorage(name, val) {
    window.localStorage.setItem(name, val);
}

function orderSuccess() {
    debugger;

    var UserIdorderSuccess = getCookie("userId");
    if (UserIdorderSuccess == null || UserIdorderSuccess == undefined || UserIdorderSuccess == "") return 0;
    var CodeorderSuccess = $('.item-success-order-code').attr("data-item").replace(",", "-");
    var IdorderSuccess = UserIdorderSuccess + CodeorderSuccess;

    var LinkorderSuccess = $('.item-success-order-id').attr("data-item");
    var EmailorderSuccess = localStorage.getItem("emailOrder");
    var TotalPriceorderSuccess = $('.item-success-order-price').attr("data-item");
    var DateorderSuccess = $('.item-success-order-date').attr("data-item");
    var MethodorderSuccess = $('.item-success-order-method').attr("data-item");
    var PrepayorderSuccess = $('.item-success-order-prepay').attr("data-item");
    var ImageorderSuccess = $('.item-success-order-image').attr("data-item");
    var HotelNameorderSuccess = $('.item-success-order-hotel').attr("data-item");
    var HotelAddressorderSuccess = $('.item-success-order-hoteladdress').attr("data-item");
    var listRoom = [];

    $('#accordionPanelsStayOpenExample .accordion-item').each(function () { 
        let room = {
            roomName: $(this).attr("data-name"),
            roomDesc: $(this).attr("data-desc"),
            roomCheckin: $(this).attr("data-checkin"),
            roomCheckout: $(this).attr("data-checkout"),
            roomNight: $(this).attr("data-night"),
            roomImage: $(this).attr("data-image")
        };
        listRoom.push(room);
    })
    var DataRoomOrder = JSON.stringify(listRoom);

    $.ajax({
        type: "POST",
        url: "/Admin/FireBaseHome/CreateOrder",
        data: {
            Id: IdorderSuccess,
            Link: LinkorderSuccess,
            Code: CodeorderSuccess,
            TotalPrice: TotalPriceorderSuccess,
            DateCreate: DateorderSuccess,
            UserId: UserIdorderSuccess,
            Email: EmailorderSuccess,
            Method: MethodorderSuccess,
            PrePay: PrepayorderSuccess,
            Image: ImageorderSuccess,
            DataRoomOrder: DataRoomOrder,
            HotelName: HotelNameorderSuccess,
            HotelAddress: HotelAddressorderSuccess,
        },
        cache: false,
        async: false,
        beforeSend: function () {

            showSearchLoader();
        },
        success: function (data) {

            hideSearchLoader();
            console.log("orderSuccess:... ", data);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            hideSearchLoader();
            alert("An error occured. Please try again!");
        }
    })
}

function showGroupPayment() {
    var option = $('#typePaymentGroup').val();
    var met = localStorage.getItem("method");
    console.log("option:... ", option);
    var method = $('.booking-payment-parent input[name="flexRadio"]:checked').val();
    if (option == 4) {
        $('input[name="flexRadio"]').each(function () {
            $(this).attr("disabled", "disabled");
        })
        $('#codpay').removeAttr("disabled");
        $('#codpay').click();

    } else {
        $('input[name="flexRadio"]').each(function () {
            $(this).removeAttr("disabled");
            if ($(this).val() == met && method == 4) {
                $(this).click();
            }
        })
        $('#codpay').attr("disabled", "disabled");
    }
    //$('input[name="flexRadio"]').each(function () {
    //    console.log($(this).val());
    //    if ($(this).attr("data-typepay") < option) {
    //        $(this).attr("disabled", "disabled");
            
    //    }
    //    else $(this).removeAttr("disabled");
    //    if (method > option) {
    //        $("#momopay").click();
    //    }
       
    //})
}
function setPaymentMethod() {
    var groupMethod = $('#typePaymentGroup').val();
    var method = $('.booking-payment-parent input[name="flexRadio"]:checked').val();
    localStorage.setItem("groupMethod", groupMethod);
    localStorage.setItem("method", method);
    localStorage.setItem("emailOrder", $('.set-email-order').val());
}
function setPaymentMethodDefault() {
    $('#typePaymentGroup').val(localStorage.getItem("groupMethod"));
    var met = localStorage.getItem("method");
    $('input[name="flexRadio"]').each(function () {
        console.log($(this).val());
        if ($(this).attr("data-typepay") > localStorage.getItem("groupMethod")) $(this).attr("disabled", "disabled");
        else $(this).removeAttr("disabled");
        if ($(this).val() == met) {
            $(this).click();
            $(this).parents("li.ratio-").addClass("ratio-active");
        }
    })
    $('.booking-payment-parent input[name="flexRadio"]:checked').val();
}