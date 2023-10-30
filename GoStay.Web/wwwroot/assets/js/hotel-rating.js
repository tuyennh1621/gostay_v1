
    $(document).ready(function () {
        let totalPoint = $(".total-point");
        let totalName = $(".total-name");
        let btnAjax = $('.btn-ajax');
        let modalDanhGia = $('#modal-danhgia');
        $(".js-range-slider").ionRangeSlider(
            {
                skin: "big",
                min: 1,
                max: 10,
                from: 5,
                step: 0.5,
                hide_min_max: true,
                onChange: function (data) {
                    console.log(data);

                }
            });


        $(".js-range-slider").change(function () {
            let nameRating = $(this).prop('name');
            let test = $(this).attr('id')
            let pointRating = $(this).val();
            let divTypeRange = $(`.type-rating-${nameRating}`)
            console.log('test ' + test);
            console.log('nameRating ' + nameRating);
            console.log('pointRating ' + pointRating);
            divTypeRange.text(' : ' + getTotalNameWithPoint(pointRating))
            totalPoint.text(getTotalPoint());
            totalName.text(getTotalNameWithPoint(getTotalPoint()))
        });


        // AJAX
        btnAjax.click(function () {
            console.log($('.content-body').val());
          let rating = {
                HotelId: $('#hotelId').val(),
                LocationScore: $('#range-vitri').val(),
                ValueScore: $('#range-giaca').val(),
                ServiceScore: $('#range-phucvu').val(),
                CleanlinessScore: $('#range-vesinh').val(),
                RoomsScore: $('#range-tiennghi').val(),
                Description: $('.content-body').val()
            }
            var request = $.ajax({
                url: '/HotelDetail/UserReview',
                method: "POST",
                data: rating,
            });
           request.done(function (msg) {
                console.log(msg);
                $('#write_reviews').modal('hide');
                $('.modal-danhgia-body').text(msg.value)
                modalDanhGia.modal('show');
           });

            request.fail(function (jqXHR, textStatus) {
                $('#write_reviews').modal('hide');
                alert("Request failed: " + textStatus);

            });

        });


        // funtion
        const getTotalNameWithPoint = (point) => {
            let name = "Kém";
            if (point >= 3 && point < 7) name = "Trung Bình";
            else if (point >= 7 && point < 9) name = "Tốt";
            else if (point >= 9 && point <= 10) name = "Tuyệt vời";
            return name;
        };
        const getTotalPoint = () => {
            let average = 0;
            let stt = 0;
            $(".js-range-slider").each(function (index) {
                average = average + parseFloat($(this).val());
                stt++;
            });
            console.log(average);
            console.log(stt);
            return average / stt;
        }

      // loadmore
      $('.reviews_listing').loadMoreResults({
        tag: {
          name: 'div',
          'class': 'user-listing'
        },
        displayedItems: 10,
        showItems: 10,
        button: {
          'text': 'Xem thêm ',
          'class':'loadmore'
          }
      });

    });

