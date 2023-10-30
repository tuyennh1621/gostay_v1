$(document).ready(function () {
    let currentPage = 0;
    function firstLoadKetQuaTKVeMayBay(pageindex, pagesize) {
        var startPoint = $('#StartPoint').val();
        var endPoint = $('#EndPoint').val();
        var departureDate = $('#DepartureDate').val();
        var returnDate = $('#ReturnDate').val();
        var adult = $('#adult').val();
        var children = $('#children').val();
        var infant = $('#infant').val();
        var priceMin = $('#rangFrom').val();
        var priceMax = $('#rangTo').val();

        var checkDirection = document.getElementsByName('Direction[]');
        var direction = "";
        for (var i = 0, n = checkDirection.length; i < n; i++) {
            if (checkDirection[i].checked) {
                direction += "," + checkDirection[i].value;
            }
        }
        if (direction) direction = direction.substring(1);

        var checkAirline = document.getElementsByName('Airline[]');
        var airline = "";
        for (var i = 0, n = checkAirline.length; i < n; i++) {
            if (checkAirline[i].checked) {
                airline += "," + checkAirline[i].value;
            }
        }
        if (airline) airline = airline.substring(1);

        var checkStartTime = document.getElementsByName('StartTime[]');
        var startTime = "";
        for (var i = 0, n = checkStartTime.length; i < n; i++) {
            if (checkStartTime[i].checked) {
                startTime += "," + checkStartTime[i].value;
            }
        }
        if (startTime) startTime = startTime.substring(1);

        var checkClassTk = document.getElementsByName('ClassTk[]');
        var classTk = "";
        for (var i = 0, n = checkClassTk.length; i < n; i++) {
            if (checkClassTk[i].checked) {
                classTk += "," + checkClassTk[i].value;
            }
        }
        if (classTk) classTk = classTk.substring(1);

        var checkTypeFlight = document.getElementsByName('TypeFlight[]');
        var typeFlight = "";
        for (var i = 0, n = checkTypeFlight.length; i < n; i++) {
            if (checkTypeFlight[i].checked) {
                typeFlight += "," + checkTypeFlight[i].value;
            }
        }
        if (typeFlight) typeFlight = typeFlight.substring(1);
        if (currentPage == 0) {
            $('#showKetQuaTKVeMayBay').html('<div class=\"text-center\"><div class=\"spinner-border text-primary\" role=\"status\"></div><div>Đang tải dữ liệu...</div></div>');
        }
        $.ajax({
            type: "GET",
            url: "/Tickets/ketqua",
            data:
            {
                StartPoint: startPoint,
                EndPoint: endPoint,
                DepartureDate: departureDate,
                ReturnDate: returnDate,
                Adult: adult,
                Children: children,
                Infant: infant,
                ClientVia : "Web",
                FilterType:0,
                PriceMin: priceMin,
                PriceMax: priceMax,
                Direction: direction,
                Airline: airline,
                StartTime: startTime,
                ClassTk: classTk,
                TypeFlight: typeFlight,
                PageIndex: pageindex,
                PageSize: pagesize
            },
            success: function (data) {
                if (currentPage == 0) {
                    $('#showKetQuaTKVeMayBay').html(data);
                }
                else {
                    $(data).hide().appendTo("#showKetQuaTKVeMayBay").fadeIn(1000);
                }
                currentPage = pageindex + 1;

                txtcurenPageSize = $("#txtTotalItem").val();
                
                if (pagesize * currentPage < txtcurenPageSize) {
                    $(".loadmore").css("display", "block");
                }
                else {
                    $(".loadmore").css("display", "none");
                }
                
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Tìm vé không thành công, hãy thử thay đổi ngày khởi hành và tìm lại.');
            }
        })

    };

    $("#btnSearchVeMB").click(function () {
        currentPage = 0;
        firstLoadKetQuaTKVeMayBay(0,10);
    });

    firstLoadKetQuaTKVeMayBay(0, 10);

    $(".loadmore").click(function () {
        firstLoadKetQuaTKVeMayBay(currentPage,10);
    });
})