// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $(".addBookToCart").on("click", function () {
        let priceId = $(this).data("priceid");
        $.ajax({
            url: '/api/shoppingcart',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ stripePriceId: priceId }),
            success: function (response) {
                console.log("Item added to cart successfully.");
                // OShow the message to the user
            },
            error: function (xhr, status, error) {
                console.error("Error, this should be visible to the user");
                // show the error to the user
            }
        });
    });
});
