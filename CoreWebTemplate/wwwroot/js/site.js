// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {

    setMenuCss(window.actionValue);

    SetSticky();

    $(window).resize(function () { SetBodyViewHeight(); });

});

function SetSelectTitleEvent() {

    $(".select-title-event").on("click", function () {
        var $this = $(this);

        $("#Id").val($this.data().id);

        $("#Shares").val($this.data().shares);

        $("#Likes").val($this.data().likes);

        $("#select-post-form").submit();

    });

}

function SetSticky() {

    var isImageExpanded = true;

    $(".sticky-container").Sticky({
        onSticky: function () {
  
        },

        onScrollTop: function () {


        }
    });
}

function setMenuCss(actionValue) {
    $(".nav-item." + actionValue).addClass("selected-menu-item")
}

(function ($) {


    $.fn.Sticky = function (options) {

        var settings = $.extend({
            // These are the defaults.
            onSticky: null,
            onScrollTop: null,
            init: null,
            offset: 0,
        }, options);

        var header = $(this);

        var sticky = header.offset().top;

        $(window).scroll(function () {

            var scrollPos = window.pageYOffset;
            if (scrollPos > sticky) {


                $("#home-img").addClass("home-img-shrink");
                header.addClass("sticky box-shadow");

                if (settings.onSticky)
                   settings.onSticky();

            }
            else {

                header.removeClass("box-shadow sticky");
                $("#home-img").removeClass("home-img-shrink");
                if (settings.onScrollTop)
                    settings.onScrollTop()

            }

        });

    }
}(jQuery));