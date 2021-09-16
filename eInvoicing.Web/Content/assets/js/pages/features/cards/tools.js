"use strict";

var KTCardTools = function () {
    // Toastr
    var initToastr = function() {
        toastr.options.showDuration = 1000;
    }


    // Demo 2
    var demo2 = function () {
        // This card is lazy initialized using data-card="true" attribute. You can access to the card object as shown below and override its behavior
        var card = new KTCard('kt_card_2');
    }

    // Demo 3
    var demo3 = function () {
        // This card is lazy initialized using data-card="true" attribute. You can access to the card object as shown below and override its behavior
        var card = new KTCard('kt_card_3');
    }
    // Demo 4
    var demo4 = function () {
        // This card is lazy initialized using data-card="true" attribute. You can access to the card object as shown below and override its behavior
        var card = new KTCard('kt_card_4');

    }

    // Demo 5
    var demo5 = function () {
        // This card is lazy initialized using data-card="true" attribute. You can access to the card object as shown below and override its behavior
        var card = new KTCard('kt_card_5');

       
    }
    // Demo 6
    var demo6 = function () {
        // This card is lazy initialized using data-card="true" attribute. You can access to the card object as shown below and override its behavior
        var card = new KTCard('kt_card_6');

        
    }

    

    return {
        //main function to initiate the module
        init: function () {
            initToastr();

            // init demos
            //demo1();
            demo2();
            demo3();
            demo4();
            demo5();
            demo6();

        }
    };
}();

jQuery(document).ready(function() {
    KTCardTools.init();
});
