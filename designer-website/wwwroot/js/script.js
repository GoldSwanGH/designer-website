// var menuCall = document.getElementsByClassName("menu_expand");
// var i;
//
// for (i = 0; i < menuCall.length; i++) {
//     menuCall[i].addEventListener("click", function () {
//         document.getElementsByClassName("main__menu").item(0).classList.toggle("block_visibility_active");
//         document.getElementsByClassName("main__menu").item(0).classList.toggle("block_margin-top_normal");
//     });
// }

$("input").keydown(function(event) {
    if (event.keyCode == 13) {
        event.preventDefault();
    }
});

var anchors = [];
var currentAnchor = -1;
var isAnimating  = false;
$(function(){
    function updateAnchors() {
        anchors = [];
        $('.anchor').each(function(i, element){
            $('body').addClass('site_scroll_disable');
            anchors.push( $(element).offset().top );
        });
    }
    $('.anchor').on('mousewheel', function(e) {
        e.stopPropagation();
        if( isAnimating ) {
            return false;
        }
        isAnimating  = true;
        if( e.originalEvent.wheelDelta >= 0 ) {
            currentAnchor--;
        }else{
            currentAnchor++;
        }
        if( currentAnchor > (anchors.length - 1)
            || currentAnchor < 0 ) {
            currentAnchor = 0;
        }
        isAnimating  = true;
        $('html, body').animate({
            scrollTop: parseInt( anchors[currentAnchor] )
        }, 500, 'swing', function(){
            isAnimating  = false;
        });
    })
    updateAnchors();
});