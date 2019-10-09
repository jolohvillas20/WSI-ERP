$.getScript('http://localhost:60478/Scripts/js1.txt', function () {

    $("#mySel1").multiselect({

    });

    $('.single option').click(function () {
        // only affects options contained within the same optgroup
        // and doesn't include this
        //$(this).siblings().prop('selected', false);
    });

});

$.getScript('http://localhost:60478/Scripts/js1.txt', function () {

    $("#mySel2").multiselect({

    });

    $('.single option').click(function () {
        // only affects options contained within the same optgroup
        // and doesn't include this
        //$(this).siblings().prop('selected', false);
    });

});