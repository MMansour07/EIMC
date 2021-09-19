// Class definition

var KTBootstrapDatepicker = function () {
    var today = new Date();
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    var startDate = new Date(today.getFullYear(), 6, 1);
    var endDate = new Date(today.getFullYear(), 6, 31);
    $("#kt_datepicker_2").val(monthNames[today.getMonth()] + " " + today.getFullYear());
    var arrows;
    if (KTUtil.isRTL()) {
        arrows = {
            leftArrow: '<i class="la la-angle-right"></i>',
            rightArrow: '<i class="la la-angle-left"></i>'
        }
    } else {
        arrows = {
            leftArrow: '<i class="la la-angle-left"></i>',
            rightArrow: '<i class="la la-angle-right"></i>'
        }
    }
    
    // Private functions
    var demos = function () {
        // minimum setup
        $('#kt_datepicker_1, #kt_datepicker_1_validate').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            orientation: "bottom left",
            templates: arrows,
            
        });

        // minimum setup for modal demo
        $('#kt_datepicker_1_modal').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            orientation: "bottom left",
            templates: arrows
        });

        // input group layout 
        $('#kt_datepicker_2, #kt_datepicker_2_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            templates: arrows,
            format: "MM yyyy",
            minViewMode: 1,
            autoclose: true
        });

        // input group layout for modal demo
        $('#kt_datepicker_2_modal').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            orientation: "bottom left",
            templates: arrows
        });


        // input group layout 
        $('#fromDate, #fromDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            templates: arrows,
            todayBtn: "linked",
            format: "dd-M-yyyy",
            clearBtn: true,
            todayHighlight: true,
            autoclose: true
        });

        // input group layout 
        $('#toDate, #toDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            templates: arrows,
            todayBtn: "linked",
            format: "dd-M-yyyy",
            clearBtn: true,
            todayHighlight: true,
            autoclose: true
        });

        $('#specificDate, #specificDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            format: "yyyy",
            templates: arrows,
            autoclose: true,
            viewMode: "years",
            minViewMode: "years"
        });
        $('#LowestspecificDate, #specificDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            format: "yyyy",
            templates: arrows,
            autoclose: true,
            viewMode: "years",
            minViewMode: "years"
        });

        // enable clear button 
        $('#kt_datepicker_3, #kt_datepicker_3_validate').datepicker({
            rtl: KTUtil.isRTL(),
            todayBtn: "linked",
            clearBtn: true,
            todayHighlight: true,
            templates: arrows
        });

        // enable clear button for modal demo
        $('#kt_datepicker_3_modal').datepicker({
            rtl: KTUtil.isRTL(),
            todayBtn: "linked",
            clearBtn: true,
            todayHighlight: true,
            templates: arrows
        });

        // orientation 
        $('#kt_datepicker_4_1').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "top left",
            todayHighlight: true,
            templates: arrows
        });

        $('#kt_datepicker_4_2').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "top right",
            todayHighlight: true,
            templates: arrows
        });

        $('#kt_datepicker_4_3').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            todayHighlight: true,
            templates: arrows
        });

        $('#kt_datepicker_4_4').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom right",
            todayHighlight: true,
            templates: arrows
        });

        // range picker
        $('#kt_datepicker_5').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            templates: arrows
        });

         // inline picker
        $('#kt_datepicker_6').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            templates: arrows
        });
    }

    return {
        // public functions
        init: function() {
            demos(); 
        }
    };
}();

jQuery(document).ready(function() {    
    KTBootstrapDatepicker.init();
});
function toMMDDYYYY(date) {
    var datePart = date.toString().split("/");
    var MMDDYYYY = [datePart[1], datePart[0], datePart[2]].join('/');
    return MMDDYYYY
}