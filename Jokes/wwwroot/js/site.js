// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#RandomButton").on("click", function () {
    $.ajax({
        url: "/jokesapi/get-random-return-html",
        success: function (data) {
            $("#RandomResults").html(data);
        }
    });
});

$("#SearchText").on("keyup", function (event) {
    if (event.keyCode === 13) {
        $("#SearchButton").click();
    }
});


$("#SearchButton").on("click", function () {

    $.ajax({
        url: "/jokesapi/search-return-html?term=" + $.trim($("#SearchText").val()),
        data: $.trim($("#SearchText").val()),
        success: function (data) {
            $("#SearchResults").html(data);
        }
    });
});