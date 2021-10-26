// Class definition
var dest = true;
var KTFormRepeater = function () {

    // Private functions
    var demo1 = function () {
        $('#kt_repeater_1').repeater({
            initEmpty: false,



            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
                var len = $(".outer").length;
                eval(
                    '\_validations.push(FormValidation.formValidation(\
                        _formEl,\
                        {\
                            fields: {\
                                "['+ (2 * parseInt(len) - 2) + '][ItemCode]": {\
                                    validators: {\
                                        notEmpty: {\
                                            message: "Item Code is required"\
                                        },\
                                        digits: {\
                                            message: "The Item Code value is not valid. Only numbers is allowed"\
                                        }\
                                    }\
                                },\
                                "['+ (2 * parseInt(len) - 2) + '][Quantity]": {\
                                            validators: {\
                                                notEmpty: {\
                                                    message: "Item Code is required"\
                                                },\
                                            }\
                                        },\
                                    "['+ (2 * parseInt(len) - 2) + '][UnitType]": {\
                                                        validators: {\
                                                            notEmpty: {\
                                                                message: "Item Code is required"\
                                                            },\
                                                        }\
                                                    },\
                                    "['+ (2 * parseInt(len) - 2) + '][Description]": {\
                                                        validators: {\
                                                            notEmpty: {\
                                                                message: "Item Code is required"\
                                                            },\
                                                        }\
                                                    },\
                                    "['+ (2 * parseInt(len) - 2) + '][AmountEGP]": {\
                                                        validators: {\
                                                            notEmpty: {\
                                                                message: "Item Code is required"\
                                                            },\
                                                        }\
                                                    },\
                                    "['+ (2 * parseInt(len) - 2) + '][CurrencySold]": {\
                                                                    validators: {\
                                                                        notEmpty: {\
                                                                            message: "Item Code is required"\
                                                                        },\
                                                                    }\
                                                                },\
                                    "['+ (2 * parseInt(len) - 2) + '][TaxType]": {\
                                                                    validators: {\
                                                                        notEmpty: {\
                                                                            message: "Item Code is required"\
                                                                        },\
                                                                    }\
                                                                },\
                                                            },\
                                    "['+ (2 * parseInt(len) - 2) + '][SubType]": {\
                                                                    validators: {\
                                                                        notEmpty: {\
                                                                            message: "Item Code is required"\
                                                                        },\
                                                                    }\
                                                                },\
                                    "['+ (2 * parseInt(len) - 2) + '][Rate]": {\
                                                                    validators: {\
                                                                        notEmpty: {\
                                                                            message: "Item Code is required"\
                                                                        },\
                                                                    }\
                                                                },\
                                    "['+ (2 * parseInt(len) - 2) + '][Amount]": {\
                                                                    validators: {\
                                                                        notEmpty: {\
                                                                            message: "Item Code is required"\
                                                                        },\
                                                                    }\
                                                                },\
                            plugins: {\
                                trigger: new FormValidation.plugins.Trigger(),\
                                bootstrap: new FormValidation.plugins.Bootstrap(),\
                            }\
                        }\
                    ));');
            },

            hide: function (deleteElement) {
                if (confirm('Are you sure you want to delete this line?')) {
                    $(this).slideUp(deleteElement);
                    _validations.pop();
                }
            }
        });
    }

    var demo2 = function () {
        $('#kt_repeater_2').subRepeater({
            initEmpty: false,

            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
            },

            hide: function (deleteElement) {
                if (confirm('Are you sure you want to delete this element?')) {
                    $(this).slideUp(deleteElement);
                }
            }
        });
    }


    var demo3 = function () {
        $('#kt_repeater_3').repeater({
            initEmpty: false,

            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
            },

            hide: function (deleteElement) {
                if (confirm('Are you sure you want to delete this element?')) {
                    $(this).slideUp(deleteElement);
                }
            }
        });
    }

    var demo4 = function () {
        $('#kt_repeater_4').repeater({
            initEmpty: false,

            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
            },

            hide: function (deleteElement) {
                $(this).slideUp(deleteElement);
            }
        });
    }

    var demo5 = function () {
        $('#kt_repeater_5').repeater({
            initEmpty: false,

            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
            },

            hide: function (deleteElement) {
                $(this).slideUp(deleteElement);
            }
        });
    }

    var demo6 = function () {
        $('#kt_repeater_6').repeater({
            initEmpty: false,

            defaultValues: {
                'text-input': 'foo'
            },

            show: function () {
                $(this).slideDown();
            },

            hide: function (deleteElement) {
                $(this).slideUp(deleteElement);
            }
        });
    }
    return {
        // public functions
        init: function () {
            demo1();
            demo2();
            //demo3();
            //demo4();
            //demo5();
            //demo6();
        }
    };
}();

jQuery(document).ready(function () {
    KTFormRepeater.init();
});

