
(function () {
    "use strict";


    var wrapper = $('<div/>').css({ height: 0, width: 0, 'overflow': 'hidden' });

    var fileInput = $(':file').wrap(wrapper);

    // When your file input changes, update the text for your button
    fileInput.change(function () {
        $this = $(this);
        // If the selection is empty, reset it
        if ($this.val().length == 0) {
            $('#file').text("Your Text to Choose a File Here!");
        } else {
            $('#file').text($this.val());
        }
    })

    // When your fake button is clicked, simulate a click of the file button
    $('#file').click(function () {
        fileInput.click();
    }).show();

    $(".appSearch").click(function () {
        $("#fromParaSearch").submit();
    });

    

    $("#txtInputRoomSearch").click(function () {
        $("#result_search_room3").show();
    });

    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

    function binCachedKeyWord(KeyWord, input, control) {
        var _sgs = document.querySelector("#" + control);
        if (!_sgs)
            return;
        document.getElementById(control).innerHTML = "";
        if (localStorage.getItem(KeyWord)) {
            var strCached = localStorage.getItem(KeyWord);
            if (strCached.indexOf(',') > 0) {
                var arrayCached = strCached.split(',');
                arrayCached.forEach(element =>
                    document.getElementById(control).innerHTML += "<li data-key=\"" + element + "\"><a class=\"w-100\">\"" + element + "\"</a><i class=\"fa-solid fa-trash-can pl-10 remove\"></i></li>"
                );

            }
            else {
                document.getElementById(control).innerHTML += "<li data-key=\"" + strCached + "\"><a class=\"w-100\">\"" + strCached + "\"</a><i class=\"fa-solid fa-trash-can pl-10 remove\"></i></li>"
            }
        }
        else {
            $("#no-result" + KeyWord).css('display', 'flex');
            $("#" + control).hide();
        }

        $("." + control + " li a").click(function () {

            var textSearch = $(this).parent().attr("data-key");

            $("#" + input).val(textSearch);
            if (input == "txtInputHotelSearch") {
                delay(function () {
                    binSuggestSearch(textSearch);
                }, 100);
            }
            else {
                delay(function () {
                    binSuggestSearchTour(textSearch);
                }, 100);
            }
            
        });


        $("." + control +" .remove").click(function () {
            var paa = $(this).parent();
            removeCachedKeyWord(paa.attr("data-key"), KeyWord);
            paa.fadeOut(50);
        });
    }


    function removeCachedKeyWord(KeyWord, listKeyWord) {
        if (typeof (Storage) !== "undefined") {

            if (localStorage.getItem(listKeyWord)) {
                var strCached = localStorage.getItem(listKeyWord);

                if (strCached.indexOf(',') <= 0) {
                    localStorage.removeItem(listKeyWord);
                }
                else {
                    var arrayCached = strCached.split(',');

                    const index = arrayCached.indexOf(KeyWord);
                    if (index > -1) {
                        arrayCached.splice(index, 1); 
                    }
                    localStorage.setItem(listKeyWord, arrayCached.toString());
                }
                
            }
           

        }
    }
    function AddCachedKeyWord(KeyWord, key) {
        if (typeof (Storage) !== "undefined") {

            if (!localStorage.getItem(key)) {
                localStorage.setItem(key, KeyWord);
                return;
            }
            var strCached = localStorage.getItem(key);

            var arrayCached = strCached.split(',');

            const index = arrayCached.indexOf(KeyWord);
            if (index > -1) {
                arrayCached.splice(index, 1);
            }
            arrayCached.unshift(KeyWord);
            localStorage.setItem(key, arrayCached.toString());
            
        }
    }
    function binActionSelectSuggest(){
        $("#ResultSuggest li").click(function () {
            var textSearch = $(this).attr("data-text");

            $("#txtInputHotelSearch").val(textSearch);

            AddCachedKeyWord(textSearch, "CachedKeyWord");

            $("#result_search_hotel").hide();

            bincalender();

            $("#result_search_date").show();

            $("#txtStyleSearch").val($(this).attr("data-style"));

             
            // Get First Li tag
            // var first_li = $("#ResultSuggest li").first();

            var sty = $(this).attr("data-style")
            if (sty == "1") {
                $("input[name='IdTinhthanh']").val($(this).attr("data-id"));
            }
            else if (sty == "2") {
                $("input[name='IdTinhthanh']").val($(this).attr("data-idtt"));
                $("#txtparaQuans").val($(this).attr("data-id"));
                $("#txtparaQuans").prop("checked", true);
            }
            else if (sty == "3") {
                $("input[name='IdTinhthanh']").val($(this).attr("data-idtt"));
                $("checkbox[name='Quans']").val($(this).attr("data-idqh"));
                $("checkbox[name='Quans']").prop("checked", true);
                $("input[name='IdPhuong']").val($(this).attr("data-id"));

            }
            else if (sty == "4") {
                $("input[name='idHotel']").val($(this).attr("data-id"));
            }
        });
    }
    var __nearlat = 0;
    var __nearlon = 0;
    function binSuggestSearch(textSearch) {

        $(".no-result").hide();
        textSearch = textSearch.trim();
        if (textSearch.length < 3 && textSearch !='.')
            return;
        $("#result_search_hotel").show();
        $("#mainsgsresult").addClass('loading2');
        $.ajax({
            type: "GET",
            traditional: true,
            data:
            {
                "keyword": textSearch,
                "lat": __nearlat,
                "lon": __nearlon,
            },
            url: "/Hotels/SuggestSearch",
        }).done(function (result) {
            
            $('#ResultSuggest').html(result);
            binActionSelectSuggest();
            $(".sgsCachedKeyWord").hide();
            $(".btnClearKW").click(function () {
                $("#txtInputHotelSearch").val('');
                $('#ResultSuggest').html('');

                $("input[name='IdTinhthanh']").val('0');
                $("input[name='IdPhuong']").val('0');
                $("input[name='idHotel']").val('0');

                $("#txtInputHotelSearch").focus();
                $(".sgsCachedKeyWord").show();

                localStorage.removeItem("CachedResultSuggest");

                binCachedKeyWord("CachedKeyWord", "txtInputHotelSearch","sgsCachedKeyWord");
            });
            $("#mainsgsresult").removeClass('loading2');

            AddCachedKeyWord(textSearch,"CachedKeyWord");

            localStorage.setItem("CachedResultSuggest", result);
        });

    }
    function binSuggestSearchTour(textSearch) {
        $(".no-result").hide();
        textSearch = textSearch.trim();
        if (textSearch.length < 3)
            return;
        $("#result_search_tour").show();
        //$("#mainsgsresult").addClass('loading2');
        $("#toursgresult").addClass('loading2'); 

        $.ajax({
            type: "GET",
            traditional: true,
            data:
            {
                "keyword": textSearch,
            },
            url: "/Tours/SuggestTour",
        }).done(function (result) {

            $('#TourResultSuggest').html(result);
            binActionSelectSuggestTour();
            $(".sgsCachedKeyWordTours").hide();
            $(".btnClearTours").click(function () {
                $("#txtInputTourSearch").val('');
                $('#TourResultSuggest').html('');

                $("#txtInputTourSearch").focus();
                $(".sgsCachedKeyWordTours").show();

                localStorage.removeItem("CachedResultSuggest2");

                binCachedKeyWord("CachedKeyWordTour", "txtInputTourSearch", "sgsCachedKeyWordTours");
            });
            $("#toursgresult").removeClass('loading2');

            AddCachedKeyWord(textSearch,"CachedKeyWordTour");

            localStorage.setItem("CachedResultSuggest2", result);
        });
    }
    function binActionSelectSuggestTour() {
        $("#TourResultSuggest li").click(function () {
            var textSearch = $(this).attr("data-text");

            $("#txtInputTourSearch").val(textSearch);

            AddCachedKeyWord(textSearch,"CachedKeyWordTour");

            $("#result_search_tour").hide();

            bincalender();

            $("#result_search_date").show();

            $("#txtStyleSearch").val($(this).attr("data-style"));

            var sty = $(this).attr("data-style")
            if (sty == "1") {
                $("input[name='IdTinhthanh']").val($(this).attr("data-id"));
            }
            else if (sty == "2") {
                $("input[name='IdTinhthanh']").val($(this).attr("data-id"));
            }
        });
    }
    $('#txtInputHotelSearch').keyup(function () {
        var textSearch = this.value;

        delay(function () {
            binSuggestSearch(textSearch);
        }, 500);
    }); 


    $('#btnnearme').click(function () {
        $("#txtInputHotelSearch").val('.');


        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(SavePosition);
        }

        if (typeof (Storage) !== "undefined" || __nearlat == 0) {
            if (localStorage.getItem('lat')) {
                __nearlat = parseFloat(localStorage.getItem('lat'));
            }
            if (localStorage.getItem('lon')) {
                __nearlon = parseFloat(localStorage.getItem('lon'));
            }
        }

        if (__nearlat == 0 || __nearlon == 0) {
            alert('Bạn cần cho phép định vị để tìm khách sạn gần bạn');
            return;
        }

        binSuggestSearch('.');
    });

    $('#txtInputTourSearch').keyup(function () {
        var textSearch = this.value;

        delay(function () {
            binSuggestSearchTour(textSearch);
        }, 500);
    });

    $("#ul_room_c li").click(function () {
        $(this).addClass('active').siblings().removeClass('active');

        binparaRooms_Adults_Chils($(this).attr("data-NumRoom"), $(this).attr("data-NumAdults"), $(this).attr("data-NumChils"));

        function binparaRooms_Adults_Chils(rooms, adults, chils) {
            $("#txtNumRoom").val(rooms);
            $("#txtNumAdults").val(adults);
            $("#txtNumChils").val(chils);

            $("#lbnumRoom").text(rooms);
            $("#lbnumadu").text(adults);
            $("#lbnumchils").text(chils);

            $("#txtInputRoomSearch").val(rooms + " Phòng, " + adults + " Người lớn, " + chils+ " Trẻ em."); 
        }
    });

    bincalender();
    function bincalender() {

        $('.InputDate').pickadate({
            selectMonths: true,
            selectYears: 30,
            editable: true,
            format: 'dd/mm/yyyy'
        });

        var from_$input = $('.InputDate_from').pickadate({
            min: Date.now(),
            todayHighlight: true,
            daysOfWeekDisabled: "0,2,3,4,5,6",
            format: 'dd/mm/yyyy'
        }), from_picker = from_$input.pickadate('picker')

        if (!from_picker)
            return;

        var to_$input = $('.InputDate_to').pickadate({
            min: Date.now(),
            todayHighlight: true,
            format: 'dd/mm/yyyy'
        }), to_picker = to_$input.pickadate('picker')

        if (!to_picker)
            return;


        // Check if there’s a “from” or “to” date to start with.
        if (from_picker.get('value')) {
            to_picker.set('min', from_picker.get('select'))
        }
        if (to_picker.get('value')) {
       //     from_picker.set('max', to_picker.get('select'))
        }

        // When something is selected, update the “from” and “to” limits.
        from_picker.on('set', function (event) {
            if (event.select) {
                to_picker.set('min', from_picker.get('select'))
            }
            else if ('clear' in event) {
                to_picker.set('min', Date.now())
            }
        })
        to_picker.on('set', function (event) {
            if (event.select) {
       //         from_picker.set('max', to_picker.get('select'))
            }
            else if ('clear' in event) {
                from_picker.set('max', false)
            }
        })

    }
    var swiper = new Swiper(".mySwiper", {
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
    $('#main-carousel-topIndex').owlCarousel({
        loop: false,
        dots: true,
        margin: 30,
        nav: true,
   //     onChanged: callback,
        responsive: {
            0: {
                items: 1
            },
            600: {
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
        autoplay: true,
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

    //function callback(event) {
    //    var swiper = new Swiper(".mySwiper", {
    //        pagination: {
    //            el: ".swiper-pagination",
    //            clickable: true,
    //        }
    //    });
    //}
    $("#fromParaSearch").submit(function (e) {
        //e.preventDefault();
        document.getElementById('btnSubmitSearch').classList.add("loading");
        document.getElementById('btnSubmitSearch').disabled = true;
    });
    $("#form_UpdateUserInfo").submit(function (e) {

        e.preventDefault();
        var _submit = $(this).find(":submit");
        _submit.addClass('loading');
        _submit.prop('disabled', true);
        var formAction = $(this).attr("action");
        var user = {
            UserId: $('#UserId').val(),
            UserName: $('#txtFulname').val(),
            Email: $('#txtEmail').val(),
            MobileNo: $('#txtMobile').val(),
            Address: $('#txtAddress').val(),
            Password: $('#txtPass').val(),
            Picture: $('#Picture').val()
        };

        $.ajax({
            type: "POST",
            traditional: true,
            data:
            {
                "userJson": JSON.stringify(user),
            },
            url: formAction,
        }).done(function (result) {
            _submit.removeClass('loading');
            _submit.prop('disabled', false);
            alert(result.message);
            
        });

    });

    function showSearchLoader() {
        $("#search_loader").css("display", "block");
    }
    function hideSearchLoader() {
        $("#search_loader").css("display", "none");
    }
    function loginFb(user) {
        swal("Thông báo!", "Đăng nhập thành công!", "success").then((value) => {
            $.ajax({
                type: "POST",
                data:
                {
                    "jsonUser": JSON.stringify(user),
                },
                url: "/Home/loginWidthFireBase",
                success: function (data) {
                    window.location.reload(true);
                },
            });
        });
    }
    function toggleSignInGG() {
        if (!firebase.auth().currentUser) {

            var provider = new firebase.auth.GoogleAuthProvider();

            provider.addScope('profile');
            provider.addScope('email');

            firebase.auth().signInWithPopup(provider).then(function (result) {

                var user = result.user.providerData[0];
                if (user == null) {
                    user = result.user
                }
                if (user.email != null) {
                    loginFb(user);
                }
                

            }).catch(function (error) {
                var errorCode = error.code;
                var errorMessage = error.message;
                var email = error.email;
                var credential = error.credential;
                alert(errorMessage);
            });


            //    provider.addScope('https://www.googleapis.com/auth/plus.login');
            //firebase.auth().signInWithRedirect(provider);
        } else {
            onSignOutClick();
        }
        document.getElementById('btnloginwidthGoogle').classList.add("loading");
        document.getElementById('btnloginwidthGoogle').disabled = true;

    }

    function toggleSignInFB() {
        if (!firebase.auth().currentUser) {

            var provider = new firebase.auth.FacebookAuthProvider();
            provider.addScope('email');

            firebase.auth().signInWithPopup(provider).then(function (result) {
                var user = result.user.providerData[0];
                if (user == null) {
                    user = result.user
                }
                if (user.email != null) {
                    loginFb(user);
                }
                else {
                    if (confirm("Đăng nhập qua Facebook thất bại. Bạn có muốn đăng nhập qua Google?")) {
                        toggleSignInGG();
                    }
                    else {
                        var errorCode = error.code;
                        var errorMessage = error.message;
                        var email = error.email;
                        var credential = error.credential;
                        alert('Đăng nhập thất bại!' + errorCode);

                    }
                }

            }).catch(function (error) {
                var errorCode = error.code;
                var errorMessage = error.message;
                var email = error.email;
                var credential = error.credential;
                alert(errorMessage);
            });


          //  firebase.auth().signInWithRedirect(provider);

        } else {
            onSignOutClick();
        }
        document.getElementById('btnloginwidthFacebook').classList.add("loading");
        document.getElementById('btnloginwidthFacebook').disabled = true;
    }

    function onSignInSubmit(e) {

        e.preventDefault();

        if (document.getElementById('phone-number').value.length < 9) {
            alert('Bạn cần nhập đầy đủ số điện thoại đăng nhập.');
            document.getElementById('phone-number').focus();
            return;
        }

        document.getElementById('sign-in-button').classList.add("loading");
        document.getElementById('sign-in-button').disabled = true;

        
        if (isCaptchaOK() && isPhoneNumberValid()) {
            window.signingIn = true;
            updateSignInButtonUI();
            var phoneNumber = getPhoneNumberFromUserInput();
            var appVerifier = window.recaptchaVerifier;
            firebase.auth().signInWithPhoneNumber(phoneNumber, appVerifier)
                .then(function (confirmationResult) {
                    // SMS sent. Prompt user to type the code from the message, then sign the
                    // user in with confirmationResult.confirm(code).
                    window.confirmationResult = confirmationResult;
                    window.signingIn = false;
                    updateVerificationCodeFormUI();
                    updateSignInButtonUI();
                    updateVerifyCodeButtonUI();
                    updateSignInFormUI();
                }).catch(function (error) {
                    // Error; SMS not sent
                    console.error('Error during signInWithPhoneNumber', error);
                    window.alert('Error during signInWithPhoneNumber:\n\n'
                        + error.code + '\n\n' + error.message);
                    window.signingIn = false;
                    updateSignInFormUI();
                    updateSignInButtonUI();

                    document.getElementById('sign-in-button').classList.remove("loading");
                    document.getElementById('sign-in-button').disabled = false;
                });
        }
    }

    function onVerifyCodeSubmit(e) {

        e.preventDefault();

        document.getElementById('verify-code-button').classList.add("loading");
        document.getElementById('verify-code-button').disabled = true;

        if (!!getCodeFromUserInput()) {
            window.verifyingCode = true;
            updateVerifyCodeButtonUI();
            var code = getCodeFromUserInput();
            confirmationResult.confirm(code).then(function (result) {
                // User signed in successfully.
                executeVerified(result.user.phoneNumber, "true");
                window.verifyingCode = false;
                window.confirmationResult = null;
                updateVerificationCodeFormUI();
                

            }).catch(function (error) {
                // User couldn't sign in (bad verification code?)
                console.error('Error while checking the verification code', error);
                window.alert('Error while checking the verification code:\n\n'
                    + error.code + '\n\n' + error.message);
                window.verifyingCode = false;
                updateSignInButtonUI();
                updateVerifyCodeButtonUI();

                document.getElementById('verify-code-button').classList.remove("loading");
                document.getElementById('verify-code-button').disabled = false;
            });
        }
    }

    /**
     * Cancels the verification code input.
     */
    function cancelVerification(e) {
        e.preventDefault();
        window.confirmationResult = null;
        updateVerificationCodeFormUI();
        updateSignInFormUI();
    }

    /**
     * Signs out the user when the sign-out button is clicked.
     */
    function onSignOutClick() {
        //var temp = getCookie("userId");
        //setPersistent(uid, false)
        console.log('signOut firebase');
        firebase.auth().signOut();
        
    }
    function logout() {
        onSignOutClick();
        window.location = "/Home/Logout";
    }
    /**
     * Reads the verification code from the user input.
     */
    function getCodeFromUserInput() {
        var code = "";
        $("div#enter-pin :input").each(function () {
            var input = $(this);
            code += input.val();
        });
        document.getElementById('verify-code-button').disabled = code.length < 6;
        return code;
    }

    /**
     * Reads the phone number from the user input.
     */
    function getPhoneNumberFromUserInput() {
        var p = '+84' + document.getElementById('phone-number').value.replace('+84', '');
        p = p.replace('+840', '+84');
        return p;
    }

    /**
     * Returns true if the phone number is valid.
     */
    function isPhoneNumberValid() {
        var pattern = /^\+[0-9\s\-\(\)]+$/;

        var phoneNumber = getPhoneNumberFromUserInput();
        var isvalid = phoneNumber.search(pattern) !== -1;
        return isvalid;
    }

    /**
     * Returns true if the ReCaptcha is in an OK state.
     */
    function isCaptchaOK() {
        if (typeof grecaptcha !== 'undefined'
            && typeof window.recaptchaWidgetId !== 'undefined') {
            var recaptchaResponse = grecaptcha.getResponse(window.recaptchaWidgetId);
            return recaptchaResponse !== '';
        }
        return false;
    }

    /**
     * Re-initializes the ReCaptacha widget.
     */
    function resetReCaptcha() {
        if (typeof grecaptcha !== 'undefined'
            && typeof window.recaptchaWidgetId !== 'undefined') {
            grecaptcha.reset(window.recaptchaWidgetId);
        }
    }

    /**
     * Updates the Sign-in button state depending on ReCAptcha and form values state.
     */
    function updateSignInButtonUI() {

        document.getElementById('recaptcha-container').style.display = 'block';

        document.getElementById('sign-in-button').disabled =
            !isCaptchaOK()
            || !isPhoneNumberValid()
            || !!window.signingIn;
    }

    /**
     * Updates the Verify-code button state depending on form values state.
     */
    function updateVerifyCodeButtonUI() {
        document.getElementById('verify-code-button').disabled =
            !!window.verifyingCode
            || !getCodeFromUserInput();
    }

    /**
     * Updates the state of the Sign-in form.
     */
    function updateSignInFormUI() {
        if (firebase.auth().currentUser || window.confirmationResult) {
            document.getElementById('sign-in-form').style.display = 'none';
        } else {
            resetReCaptcha();
            document.getElementById('sign-in-form').style.display = 'block';
        }
    }

    /**
     * Updates the state of the Verify code form.
     */
    function updateVerificationCodeFormUI() {
        if (!firebase.auth().currentUser && window.confirmationResult) {
            document.getElementById('verification-code-form').style.display = 'block';
        } else {
            document.getElementById('verification-code-form').style.display = 'none';
        }
    }

    /**
     * Updates the state of the Sign out button.
     */
    function updateSignOutButtonUI() {
        if (firebase.auth().currentUser) {
            document.getElementById('sign-out-button').style.display = 'block';
        } else {
            document.getElementById('sign-out-button').style.display = 'none';
        }
    }

    /**
     * Updates the Signed in user status panel.
     */
    function updateSignedInUserStatusUI() {
        var user = firebase.auth().currentUser;
        if (user) {
            document.getElementById('sign-in-status').textContent = 'Signed in';
            document.getElementById('account-details').textContent = JSON.stringify(user, null, '  ');
        } else {
            document.getElementById('sign-in-status').textContent = 'Signed out';
            document.getElementById('account-details').textContent = 'null';
        }
    }
    
    function initApp() {
        if (window.sessionStorage.getItem('pending')) {

            window.sessionStorage.removeItem('pending');

            firebase.auth().getRedirectResult().then(function (result) {
                if (result.user) {
                    window.sessionStorage.setItem('pending', 1);

                    console.log('credential' + result.user);

                    swal("Thông báo!", "Đăng nhập thành công!", "success").then((value) => {
                        showSearchLoader();
                        $.ajax({
                            type: "POST",
                            data:
                            {
                                "jsonUser": JSON.stringify(result),
                            },
                            url: "/Home/loginWidthFireBase",
                            success: function (data) {
                                window.location.reload(true);
                            },
                        });
                    });
                }
                else {
                    console.log('result.user is null');
                }
                hideSearchLoader();
            }).catch(function (error) {
                onSignOutClick();
                var errorCode = error.code;
                var errorMessage = error.message;
                var email = error.email;
                var credential = error.credential;
                if (errorCode === 'auth/account-exists-with-different-credential') {
                    alert("Thông báo!", "You have already signed up with a different auth provider for that email." + errorMessage);
                } else {
                    alert("Thông báo!" + errorCode + errorMessage);
                }
            });

        }

        
        // Listening for auth state changes.
        firebase.auth().onAuthStateChanged(function (user) {
            
            var toastElements = document.querySelectorAll('.toast')
            for (const toastEl of toastElements) {
                new bootstrap.Toast(toastEl)
            }
            var toastElementswellcome = document.getElementById('toast-chat-wellcome');
            var toast = bootstrap.Toast.getInstance(toastElementswellcome);

            var toastElementschat = document.getElementById('toast-chat');
            var toastchat = bootstrap.Toast.getInstance(toastElementschat);

            document.getElementById('btnShowToast').addEventListener('click', function () {
                toast.show();
                toastElementswellcome.style.zIndex = "99";
            })
            document.getElementById('btnloginwidthGoogle').addEventListener('click', toggleSignInGG);
            document.getElementById('btnloginwidthFacebook').addEventListener('click', toggleSignInFB);
            document.getElementById('sign-in-form').addEventListener('submit', onSignInSubmit);
            document.getElementById('phone-number').addEventListener('keyup', updateSignInButtonUI);
            document.getElementById('phone-number').addEventListener('change', updateSignInButtonUI);

            document.getElementById('enter-pin').addEventListener("keyup", function (e) { // e = event object
                if (e.target) {
                    getCodeFromUserInput()
                }
            });
            document.getElementById('enter-pin').addEventListener("change", function (e) { // e = event object
                if (e.target) {
                    getCodeFromUserInput()
                }
            });

            document.getElementById('verification-code-form').addEventListener('submit', onVerifyCodeSubmit);

            window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container', {
                'size': 'normal',
                'callback': function (response) {
                    updateSignInButtonUI();
                },
                'expired-callback': function () {
                    updateSignInButtonUI();
                }
            });
            recaptchaVerifier.render().then(function (widgetId) {
                window.recaptchaWidgetId = widgetId;
            });

            if (user) {
                // User is signed in.
                var displayName = user.displayName;
                var email = user.email;
                var phone = user.phoneNumber;
                var emailVerified = user.emailVerified;
                var photoURL = user.photoURL;

                var isAnonymous = user.isAnonymous;

                var uid = user.uid;
                var providerData = user.providerData;
                var temp = getCookie("userId");
                if (temp != uid) {
                    setPersistent(uid, true)
                }
                document.cookie = "userId=" + uid;
                setUID(uid);
                $('#ulQS li').click(function (e) {
                    var _strobj = $(this).find("a").attr("data-message")
                    toast.hide();
                    toastchat.show();
                    toastElementschat.style.zIndex = "99";
                });

            } else {
                console.log('Chua login');
                $('#ulQS li').click(function (e) {
                    var _strobj = $(this).find("a").attr("data-message")
                    toast.hide();
                    $('#loginform').modal('show');
                });
                
            }
            
        });
    }

    function LoadDroplistDVHC(_obj, _style, _control) {
        $.ajax({
            type: "GET",
            url: "Admin/HotelHome/binDropdownAjax",
            data: { obj: _obj, style: _style },
            cache: true,
            async: false,
            success: function (data) {
                $('#' + _control).html(data);
                $('#' + _control).select2({
                    allowClear: true
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
            //    alert("An error occured. Please try again!" + xhr.errorCode);
            }
        })
    }

    $(".listchuyenbay").select2(
        {
            data:
                [
                    {
                        id: 10,
                        text: 'Miền bắc',
                        children:
                            [
                                { id: 'HNA', text: 'Hà Nội (HNA)' },
                                { id: 'HPH ', text: 'Hải Phòng (HPH)' },
                            ]
                    },
                    {
                        id: 20,
                        text: 'Miền trung',
                        children:
                            [
                                { id: 'DAD', text: 'Đà Nẵng (DAD)' },
                                { id: 'DLI', text: 'Đà Lạt (DLI)' },
                                { id: 'CXR', text: 'Nha Trang (CXR)' },
                                { id: 'UIH', text: 'Quy Nhơn (UIH)' },
                                { id: 'HUI ', text: 'Huế (HUI)' },
                                { id: 'VDH', text: 'Đồng Hới (VDH)' },
                                { id: 'TBB', text: 'Tuy Hoà (TBB)' },
                                { id: 'VII ', text: 'TP Vinh (VII)' },
                            ]
                    },
                    {
                        id: 30,
                        text: 'Miền nam',
                        children:
                        [
                                { id: 'SGA', text: 'TP HCM (SGA)' },
                                { id: 'PQC', text: 'Phú Quốc (PQC)' },
                                { id: 'VCS', text: 'Côn Đảo (VCS)' },
                        ]
                    }
                    
                ]
        });

    function SavePosition(position) {

        __nearlat = position.coords.latitude;
        __nearlon = position.coords.longitude;
        if (typeof (Storage) !== "undefined") {
            localStorage.setItem('lat', position.coords.latitude);
            localStorage.setItem('lon', position.coords.longitude);
        }
    }

    window.onload = function () {
        initApp();

        if (typeof (Storage) !== "undefined") {
            if (localStorage.getItem("CachedResultSuggest")) {
                $("#ResultSuggest").html(localStorage.getItem("CachedResultSuggest"));
                $(".btnClearKW").click(function () {
                    $("#txtInputHotelSearch").val('');
                    $('#ResultSuggest').html('');


                    $("input[name='IdTinhthanh']").val('0');
                    $("input[name='IdPhuong']").val('0');
                    $("input[name='idHotel']").val('0');


                    $("#txtInputHotelSearch").focus();
                    $(".sgsCachedKeyWord").show();
                    localStorage.removeItem("CachedResultSuggest");
                });
                $(".sgsCachedKeyWord").hide();
                binActionSelectSuggestTour();
            }

            if (localStorage.getItem("CachedResultSuggest2")) {

                $("#TourResultSuggest").html(localStorage.getItem("CachedResultSuggest2"));

                $(".btnClearTours").click(function () {
                    $("#txtInputTourSearch").val('');
                    $('#TourResultSuggest').html('');

                    $("#txtInputTourSearch").focus();
                    $(".sgsCachedKeyWordTours").show();
                    localStorage.removeItem("CachedResultSuggest2");
                });
                $(".sgsCachedKeyWordTours").hide();
                binActionSelectSuggest();
            }

            binCachedKeyWord("CachedKeyWord", "txtInputHotelSearch","sgsCachedKeyWord");
            binCachedKeyWord("CachedKeyWordTour", "txtInputTourSearch", "sgsCachedKeyWordTours");
        }
    }

    let navbarToggler = document.querySelector(".mobile-menu-btn");
    navbarToggler.addEventListener('click', function () {
        navbarToggler.classList.toggle("active");
    });

    window.onscroll = function () {
        //var header_navbar = document.querySelector(".navbar-area");

        //var sticky = header_navbar.offsetTop;

        //if (window.pageYOffset > sticky) {
        //    header_navbar.classList.add("sticky");
        //} else {
        //    header_navbar.classList.remove("sticky");
        //}

        var detail_navbar = document.querySelector(".detail-hotel-title");
        if (detail_navbar) {
            var sticky = detail_navbar.offsetTop;

            if (window.pageYOffset > sticky) {
                detail_navbar.classList.add("sticky");
            } else {
                detail_navbar.classList.remove("sticky");
            }
        }

        // show or hide the back-top-top button
        var backToTo = document.querySelector(".scroll-top")
        if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
            backToTo.style.display = "flex";
        } else {
            backToTo.style.display = "none";
        }

    };
   
    new WOW().init();

    var imgDefer = document.getElementsByTagName('img');
    for (var i = 0; i < imgDefer.length; i++) {
        if (imgDefer[i].getAttribute('data-src')) {
            imgDefer[i].setAttribute('src', imgDefer[i].getAttribute('data-src'));
        }
    };
    var picklef = document.querySelector("#sidebar_hotelsOrders");
    if (picklef) {
        var a = new StickySidebar('#sidebar_hotelsOrders', {
            topSpacing: 200,
            bottomSpacing: 20,
            containerSelector: '#containerhotelbooking',
            innerWrapperSelector: '.sidebar__inner',
        });
    }
    var sidebar_tour = document.querySelector("#sidebar_tour");
    if (sidebar_tour) {
        var a = new StickySidebar('#sidebar_tour', {
            topSpacing: 200,
            bottomSpacing: 20,
            containerSelector: '#lefcontentTour',
            innerWrapperSelector: '.sidebar__inner',
        });
    }
   

})();


