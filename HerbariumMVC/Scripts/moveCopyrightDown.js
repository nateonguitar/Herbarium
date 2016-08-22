$(document).ready(function () {
    // force the wrapper to be tall enough to put the copyright at the bottom of 
    // the screen when only a small amount of info is displayed in the wrapper

    // if javascript is turned off then the min-height defaults to 400px from the CSS
    // the default looks ok, but this trick makes it look better
    $("#wrapper")
        .css(
        { //              window's hight     - copyright div's height   - the extra padding that is at the top of the wrapper
            "min-height": window.innerHeight - $("#copyright").height() - $("#wrapper").css("padding-top").replace("px", "")
        }
    );



    // now do the exact same thing if the window is ever resized
    $(window).resize(function () {
        $("#wrapper")
            .css(
            { //              window's hight     - copyright div's height   - the extra padding that is at the top of the wrapper
                "min-height": window.innerHeight - $("#copyright").height() - $("#wrapper").css("padding-top").replace("px", "")
            }
        );
    });

});