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

        $('#pending_fromDate, #fromDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            templates: arrows,
            todayBtn: "linked",
            format: "dd-M-yyyy",
            clearBtn: true,
            todayHighlight: true,
            autoclose: true,
            //endDate: new Date($('#pending_toDate').val())
        });

        // input group layout 
        $('#pending_toDate, #toDate_validate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            templates: arrows,
            todayBtn: "linked",
            format: "dd-M-yyyy",
            clearBtn: true,
            todayHighlight: true,
            autoclose: true,
            //startDate: new Date($("#pending_fromDate").val())
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