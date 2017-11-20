$(document).ready(function () {

    //customer index - create feedback
    $(".slideToggle").click(function () {
        $(this).next(".slideContent").slideToggle("slow");
    })

    //package display - book
    $(function () {
        var $affixElement = $('div[data-spy="affix"]');
        $affixElement.width($affixElement.parent().width());
    })

    if ($(window).width() > 767) {
                
        $(window).scroll(function () {

        })
    }

    //image map - responsive area
    $('img[usemap]').rwdImageMaps();

})
