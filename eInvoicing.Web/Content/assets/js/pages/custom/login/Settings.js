"use strict";

// Class Initialization
jQuery(document).ready(function () {
    $('#settings_btn').on('click', function (e) {
        e.preventDefault();
        var btn = KTUtil.getById("kt_login_signin_submit");
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Signing...");
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var preview = document.getElementById("settings");
                preview.setAttribute('update', 'appseting');
                preview.submit();
            } else {
                KTUtil.btnRelease(btn);
                swal.fire({
                    text: "Sorry, looks like there are some errors detected, please try again.",
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
            }
        });

    });
});
