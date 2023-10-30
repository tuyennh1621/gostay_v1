function calldatve(Adult, Child, Infant, Datasession, Flightsession, Classtk) {

    let adult = "";
    let child = "";
    let infant = "";
    let baggare = "";
    let email = document.getElementById("contactEmail")?.value.trim();
    let phone = document.getElementById("contactPhone")?.value.trim();
    let name = document.getElementById("contactName")?.value.trim();
    let address = document.getElementById("contactAddress")?.value.trim();


    for (var i = 1; i <= Adult; i++) {
        var nameA = document.getElementById("NameA_" + i).value;
        adult += nameA;
        var gender = document.getElementById("GenderA_" + i).value;
        adult += '-' + gender;
        var dofb = document.getElementById("DateofbirthA_" + i).value;
        if (i < Adult) {
            adult += '-' + dofb + ',';
        }
        else {
            adult += '-' + dofb;
        }

    }

    for (var i = 1; i <= Child; i++) {
        var nameC = document.getElementById("NameC_" + i).value;
        child += nameC;
        var gender = document.getElementById("GenderC_" + i).value;
        child += '-' + gender;
        var dofb = document.getElementById("DateofbirthC_" + i).value;
        if (i < Child) {
            child += '-' + dofb + ',';
        }
        else {
            child += '-' + dofb;
        }
    }
    for (var i = 1; i <= Infant; i++) {
        var NameI = document.getElementById("NameI_" + i).value;
        infant += NameI;
        var gender = document.getElementById("GenderI_" + i).value;
        infant += '-' + gender;
        var dofb = document.getElementById("DateofbirthI_" + i).value;
        if (i < Infant) {
            infant += '-' + dofb + ',';
        }
        else {
            infant += '-' + dofb;
        }
    }
    //var length = document.getElementById("baggareLength").value;
    //baggare += length;
    //var width = document.getElementById("baggareWidth").value;
    //baggare += '-' + width;
    //var high = document.getElementById("baggareHigh").value;
    //baggare += '-' + high;

    window.location.href = 'CreateOrderTicket?Adult=' + adult + '&Child=' + child +
        '&Infant=' + infant + '&Baggare=' + baggare + '&DataSession=' + Datasession + '&FlightSession=' + Flightsession +
        '&ClassTk=' + Classtk + '&Email=' + email + '&Phone=' + phone + '&Name=' + name + '&Address=' + address;
};

$("#BookingFlight").submit(function (e) {
    e.preventDefault();
    $('#BookingFlight').validate();
    if ($('#BookingFlight').valid()) {
        calldatve($(this).attr('data-Adult'), $(this).attr('data-Child'), $(this).attr('data-Infant'), $(this).attr('data-DataSession'), $(this).attr('data-Flightsession'), $(this).attr('data-Classtk'))
    }
    
});

$("#BookingFlight").validate({

    rules: {
        contactName: "required",
        contactEmail: {
            required: true,
            CheckMail2: true
        },
        contactPhone: {
            required: true,
            phoneVI2: true
        },
    },
    errorElement: "em",
    errorPlacement: function (error, element) {
        // Add the `help-block` class to the error element
        error.addClass("help-block");

        if (element.prop("type") === "checkbox") {
            error.insertAfter(element.parent("label"));
        } else {
            error.insertAfter(element);
        }
    },
    highlight: function (element, errorClass, validClass) {

    },
    unhighlight: function (element, errorClass, validClass) {

    }
});

//jQuery.validator.addMethod("phoneVI", function (phone_number, element) {
//    phone_number = phone_number.replace(/\s+/g, "");
//    return this.optional(element) || phone_number.length > 9 && phone_number.match(/(84|0[3|5|7|8|9])+([0-9]{8})\b/);
//}, "Số điên thoại không hợp lệ");

//jQuery.validator.addMethod("CheckMail", function (email) {
//    email = email.replace(/\s+/g, "");
//    return email.match(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
//}, "Email không hợp lệ");

jQuery.validator.addMethod('CheckMail2', function (email, element) {
    email = email.replace(/\s+/g, "");
    return email.match(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);

}, function (params, element) {
    return $(element).data('msg-email');
});

jQuery.validator.addMethod('phoneVI2', function (phone_number, element) {
    phone_number = phone_number.replace(/\s+/g, "");
    return this.optional(element) || (phone_number.length > 8 && phone_number.length < 12) && phone_number.match(/(84|0[3|5|7|8|9])+([0-9]{8})\b/);

}, function (params, element) {
    return $(element).data('msg-number');
});

$("#signupForm").validate({
    rules: {
        UserName: "required",
        Email: {
            required: true,
            CheckMail2: true
        },
        Mobile: {
            required: true,
            phoneVI2: true,
        },
    },
    errorElement: "em",
    errorPlacement: function (error, element) {
        // Add the `help-block` class to the error element
        error.addClass("help-block");

        if (element.prop("type") === "checkbox") {
            error.insertAfter(element.parent("label"));
        } else {
            error.insertAfter(element);
        }
    },
    highlight: function (element, errorClass, validClass) {
        //  $(element).parents(".col-sm-5").addClass("has-error").removeClass("has-success");
    },
    unhighlight: function (element, errorClass, validClass) {
        // $(element).parents(".col-sm-5").addClass("has-success").removeClass("has-error");
    }
});

