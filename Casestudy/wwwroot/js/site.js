$(function () {

    if ($("#register_popup") !== undefined) {
        $('#register_popup').modal('show');
    }

    if ($("#login_popup") !== undefined) {
        $('#login_popup').modal('show');
    }


    // details click - to load popup on catalogue
    $("a.btn-default").on("click", (e) => {
        let Id = e.target.dataset.id;
        let data = JSON.parse(e.target.dataset.details); // it's a string need an object
        $("#results").text("");
        CopyToModal(Id, data);
    });

    

    $('.nav-tabs a').on('show.bs.tab', function (e) {
        if ($(e.relatedTarget).text() === 'Demographic') { // tab 1
            $('#Firstname').valid()
            $('#Lastname').valid()
            $('#Age').valid()
            $('#CreditcardType').valid()
            if ($('#Firstname').valid() === false || $('#Lastname').valid() === false|| $('#Age').valid() === false ||  $('#CreditcardType').valid() === false )
            {
                return false; // suppress click
            }
        }
        if ($(e.relatedTarget).text() === 'Address') { // tab 2
            $('#Address1').valid()
            $('#City').valid()
            $('#Region').valid()
            $('#Country').valid()
            $('#Mailcode').valid()
            if ($('#Address1').valid() === false || $('#City').valid() === false || $('#Region').valid() === false || $('#Country').valid() === false || $('#Mailcode').valid() === false) {
                return false; // suppress click
            }
        }
        if ($(e.relatedTarget).text() === 'Account') { // tab 3
            $('#Email').valid()
            $('#Password').valid()
            $('#RepeatPassword').valid()
            if ($('#Email').valid() === false || $('#Password').valid() === false || $('#RepeatPassword').valid() === false) {
                return false; // suppress click
            }
        }
    }); // show bootstrap tab

    // display message if modal still loaded
    if ($('#detailsId') !== undefined)
        if ($('#detailsId').val().length > 0) {
        var data = $('#modalbtn' + $('#detailsId').val()).data('details');
        CopyToModal($('#detailsId').val(), data);
        $('#details_popup').modal('show');
        }

    $("#jsondatartns").on("click", function (e) {
        busySignal("/Data/Json");
    });

    // for partial view
    $(document).on('submit', '#brandsForm', function () {
        let $theForm = $(this);
        // manually trigger validation
        $.post('/Brand/SelectProduct', $theForm.serialize())
            .done(response => $('#results').text(response))
        return false;
    });


});
let CopyToModal = (id, data) => {
    $("#qty").val("0");
    $("#Description").text(data.Description);
    $("#ProductName").text(data.ProductName);
    $("#Price").text("$" + data.CostPrice);
    $("#detailsGraphic").attr("src", "/images/" + data.GraphicName);
    $("#detailsId").val(id);
}

let busySignal = (url) => {
    let busyImg = $("<img/>", { src: "/images/wait.gif" });
    $("#busy").empty();
    $("#busy").append(busyImg);
    window.location.href = url;
}