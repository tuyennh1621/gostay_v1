$(document).ready(function () {

    $('#main-carousel-topIndex').owlCarousel({
        loop: true,
        dots: true,
        margin: 30,
        nav: true,
        onChanged: callback,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 2
            },
            600: {
                items: 2
            },
            800: {
                items: 3
            },
            1000: {
                items: 4
            },
            1200: {
                items: 5
            }
        }
    });

    $('#main-carousel-centerIndex').owlCarousel({
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

    $('#main-carousel-promotionIndex').owlCarousel({
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
                items: 1
            },
            1000: {
                items: 3
            },
            1200: {
                items: 3
            }
        }
    });

    $('#main-carousel-locationIndex').owlCarousel({
        loop: true,
        navClass: [
            'hide',
            'hide'
        ],
        dots: true,
        margin: 30,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            1000: {
                items: 5
            },
            1200: {
                items: 6
            }
        }
    });

    $('#main-carousel-detailTour').owlCarousel({
        stagePadding: 100,
        loop: true,
        dots: true,
        margin: 10,
        navClass: [
            'hide',
            'hide'
        ],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            },
            1400: {
                items: 3
            }
        }
    });

    $('#main-carousel-listingPlane').owlCarousel({
        loop: true,
        dots: false,
        margin: 30,
        nav: true,
        onChanged: callback,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            800: {
                items: 2
            },
            990: {
                items: 3
            },
            1000: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });
    $('#main-carousel-listingPlane2').owlCarousel({
        loop: true,
        dots: false,
        margin: 30,
        nav: true,
        onChanged: callback,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            800: {
                items: 2
            },
            990: {
                items: 3
            },
            1000: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });
    $('#main-carousel-breaking-news').owlCarousel({
        loop: true,
        dots: false,
        margin: 30,
        navClass: [
            'hide',
            'hide'
        ], responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            800: {
                items: 2
            },
            990: {
                items: 3
            },
            1000: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });

    var owl = $('#main-carousel-breaking-news');
    owl.owlCarousel({
        items: 4,
        loop: true,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 1000,
        autoplayHoverPause: true
    });


    function callback(event) {
        var swiper = new Swiper(".mySwiper", {
            slidesPerView: 1,
            spaceBetween: 30,
            loop: true,
            pagination: {
                el: ".swiper-pagination",
                clickable: true,
            }
        });

    };
})