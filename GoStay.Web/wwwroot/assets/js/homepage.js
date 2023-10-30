$(document).ready(function () {
    function bincarouselvemaybay() {
        $('#homepage-carousel-vemaybay').owlCarousel({
            loop: true,
            navClass: [
                'hide',
                'hide'
            ],
            dots: true,
            margin: 30,
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                },
                1200: {
                    items: 4
                }
            }
        });
    }
    function getTicketHomePage() {
        
        var homepagevemaybay = document.querySelector("#homepage-carousel-vemaybay");
        if (homepagevemaybay) {
            $('#homepage-carousel-vemaybay').html('Loading...');
            if (sessionStorage && sessionStorage.getItem("homepage_topvemaybay")) {
                $('#homepage-carousel-vemaybay').html(sessionStorage.getItem("homepage_topvemaybay"));
                bincarouselvemaybay();
            }
            else {
                $.ajax({
                    type: "GET",
                    url: "/Tickets/GetTicketHomePage",
                    data: {
                        IsHomepage: 10
                    },
                    success: function (data) {

                        $('#homepage-carousel-vemaybay').html(data);
                        sessionStorage.setItem("homepage_topvemaybay", data);
                        bincarouselvemaybay();

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log("An error occured. getTicketHomePage" + xhr + ajaxOptions + thrownError);
                    }
                });
            }
        }
        
    };
    function homepageGetTopHotels(IdProvince) {
        var Url = '/Home/GetHotelHomePage';
        $.ajax({
            type: "GET",
            url: Url,
            data: {
                IdProvince: IdProvince
            },
            // datatype: "json",
            cache: false,
            async: false,
            success: function (data) {


                $('#tabproduct').html(data);
                var swiper = new Swiper(".SwiperHotelHome", {
                    slidesPerView: 1,
                    spaceBetween: 30,
                    loop: true,
                    pagination: {
                        el: ".swiper-pagination",
                        clickable: true,
                    },
                    navigation: {
                        nextEl: ".swiper-button-next",
                        prevEl: ".swiper-button-prev",
                    },
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {

                alert("An error occured. homepageGetTopHotels");
            }
        })
    }
    function homepageGetTopTours(IdTourStyle) {
        console.log("homepageGetTopTours");
        var Url = '/Home/GetTourHomePage';
        $.ajax({
            type: "GET",
            url: Url,
            data: {
                IdTourStyle: IdTourStyle
            },
            // datatype: "json",
            cache: false,
            async: false,
            success: function (data) {

                $('#tabproduct2').html(data);

                var swiper = new Swiper(".SwiperToursHome", {
                    slidesPerView: 1,
                    spaceBetween: 30,
                    loop: true,
                    pagination: {
                        el: ".swiper-pagination",
                        clickable: true,
                    },
                    navigation: {
                        nextEl: ".swiper-button-next",
                        prevEl: ".swiper-button-prev",
                    },
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("An error occured. homepageGetTopTours");
            }
        })
    }

    if ($('#homepage-carousel-vemaybay')){
        getTicketHomePage();
    }

    if ($('#SwiperHotelHome')) {

        $(".btntopHotel").click(function () {

            $('.btntopHotel').removeClass("active");
            $('.btntopHotel').removeClass("loading");

            $(this).addClass("active");

            homepageGetTopHotels($(this).attr("data-tinhthanh"));
        });

        homepageGetTopHotels(0);
    }  
    
    if ($('#SwiperToursHome')) {

        $(".btntopTours").click(function () {

            $('.btntopTours').removeClass("active");
            $('.btntopTours').removeClass("loading");

            $(this).addClass("active");

            homepageGetTopTours($(this).attr("data-toursstyle"));
        }); 
        homepageGetTopTours(4);
    } 

});

