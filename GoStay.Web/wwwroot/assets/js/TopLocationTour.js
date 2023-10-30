$(document).ready(function () {
    function TotalTourLocationJs() {
        
        $.ajax({
            type: "GET",
            url: "/Tours/TotalTourLocation",
            data:
            {
            },
            success: function (data) {
                $('#TotalTourLocation').html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                
            }
        })

    };

    TotalTourLocationJs();
   
})