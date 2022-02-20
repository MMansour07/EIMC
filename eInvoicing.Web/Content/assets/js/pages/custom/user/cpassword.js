jQuery(document).ready(function () {
    var form = KTUtil.getById('CpasswordForm');
    _frmvalidation = FormValidation.formValidation(
        document.getElementById('CpasswordForm'),
        {
            form,
            fields: {
                OldPassword: {
                    validators: {
                        notEmpty: {
                            message: 'Old Password is required'
                        }
                    }
                },
                NewPassword: {
                    validators: {
                        notEmpty: {
                            message: 'New Password is required'
                        }
                    }
                },
                ConfirmPassword: {
                    validators: {
                        identical: {
                            compare: function () {
                                return form.querySelector('[name="NewPassword"]').value;
                            },
                            message: 'The password and its confirm are not the same'
                        }
                    }
                }

            },
            plugins: {
                trigger: new FormValidation.plugins.Trigger(),
                bootstrap: new FormValidation.plugins.Bootstrap(),
            }
        }
    );
    $("#CpasswordSubmit").click(function (e) {
        _frmvalidation.validate().then(function (status) {
            if (status == 'Valid') {
                e.preventDefault();
                KTApp.blockPage();
                var valdata = $("#CpasswordForm").serialize();
                $.ajax({
                    url: "/efatorty/v1/account/changepassword",
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: valdata,
                    success: function (response) {
                        KTApp.unblockPage();
                        if (response.success) {
                            Swal.fire({
                                //title: "Done",
                                text: "Your password has been changed successfully, please try sign in.",
                                icon: "success",
                                buttonsStyling: false,
                                confirmButtonText: "Ok, got it!",
                                customClass: {
                                    confirmButton: "btn font-weight-bold btn-light-primary"
                                }
                            }).then(function () {
                                document.getElementById('logoutForm').submit()
                            });
                        }
                        else
                        {
                            Swal.fire({
                                //title: "Sorry, something went wrong, please try again.",
                                text:  "Sorry, something went wrong, please try again.",
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
                    }
                });
            }
            else {
                return;
            }
        });
    });
});