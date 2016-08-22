// variables for hiding the header
var headerHidden = false;
var headerHeight = $("#topBanner").innerHeight();

// hide top banner button
function hideHeader() {
    if (headerHidden) {
        $("#topBanner").animate({ "height": headerHeight + "px" }, 500, function () { $("#hideHeader").html("Hide Header"); });
    }
    else {
        $("#topBanner").animate({ "height": "32px", scrollTop: headerHeight + "px" }, 500, function () { $("#hideHeader").html("Show Header"); });
    }
    headerHidden = !headerHidden;
}

$(document).ready(function () {
    
    // sometimes the measurements are off because the page hasn't fully loaded
    // putting them in the .ready makes sure the measurements are good.
    headerHidden = false;
    headerHeight = $("#topBanner").innerHeight();

    /////////////////////////////////////////
    // Hide Header on on scroll down
    /////////////////////////////////////////

    var didScroll;
    var lastScrollTop = 0;
    var delta = 10;
    var navbarHeight = $('#topBanner').outerHeight();

    var menuShowing = false;


    //--------------------------------------------------------------
    //--------------------------------------------------------------

    function hasScrolled() {
        var st = $(this).scrollTop();

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down - hide
            $('#topBanner').animate({ "margin-top": -$('#topBanner').height() }, 300);

        } else {
            $('#topBanner').animate({ "margin-top": "0" }, 300);
        }

        lastScrollTop = st;
    }

    //--------------------------------------------------------------
    function getAspectRatio() {
        tempWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        tempHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;

        var ratioObject = {
            width: tempWidth,
            height: tempHeight,
            aspectRatio: tempWidth / tempHeight
        }

        return ratioObject;
    }

    //--------------------------------------------------------------

    function displayCorrectMenu() {
        if (getAspectRatio().aspectRatio < 0.9 || getAspectRatio().width < 750) {
            // portrait
            $("#topBanner").hide();
            $("#mobileTopBanner").show();
            $("#sideMenu").show();
        }
        else {
            // landscape
            $("#topBanner").show();
            $("#mobileTopBanner").hide();
            $("#sideMenu").hide();
        }
    }

    //--------------------------------------------------------------

    function hideSideMenu() {
        $("#mobileTopBanner").animate({ "left": "0" }, 300);
        $("#mobileSideMenu").animate({ "left": "-80%" }, 300);
    }

    //--------------------------------------------------------------

    function showSideMenu() {
        $("#mobileTopBanner").animate({ "left": "80%" }, 300);
        $("#mobileSideMenu").animate({ "left": "0" }, 300);
    }
    //--------------------------------------------------------------
    //--------------------------------------------------------------


    // run once on load
    displayCorrectMenu();

    // show correct menu on resize or if you turn your phone to portrait/landscape
    $(window).resize(function () {
        displayCorrectMenu();
        width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    });

    // hide top menu when scrolling
    $(window).scroll(function (event) {
        didScroll = true;

    });

    // only hide or show if you're not scrolling super slowly
    setInterval(function () {
        if (didScroll) {
            hasScrolled();
            didScroll = false;
        }
    }, 200);





    

    $("#mobileTopBanner img").click(function () {
        if (menuShowing) {
            hideSideMenu();
            menuShowing = false;
        }
        else {
            showSideMenu();
            menuShowing = true;
        }

        $("#mobileSideMenu ul #mobileSearchBar").animate(
            {
                "margin-left": "-100%",
                "opacity": 0
            },
            {
                duration: 400,
                easing: "linear"
            }
        );
        
    });

    $("#mobileSideMenu .back").click(function () {
        hideSideMenu();
        menuShowing = false;
    });

    $("#mobileSideMenu ul #mobileMenuSearchButton").click(function () {
        $("#mobileSearchBar form #search").focus();
        $("#mobileSideMenu ul #mobileSearchBar").animate(
            {
                "margin-left": "0%",
                "opacity": 1
            },
            {
                duration: 400,
                easing: "linear"
            }
        );
    });
});