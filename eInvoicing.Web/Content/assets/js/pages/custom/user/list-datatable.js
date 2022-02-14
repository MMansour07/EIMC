"use strict";

// Class definition
var datatable;
var _frmvalidation;
var KTAppsUsersListDatatable = function () {
    // Private functions

    // basic demo
    var _demo = function () {

        datatable = $('#kt_datatable').KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        method: "GET",
                        url: "/v1/user/getusers",
                    },
                },
                //pageSize: 10, // display 10 records per page
            },
            // layout definition
            layout: {
                scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
            },
            // column sorting
            sortable: true,
            //pagination: true,
            search: {
                input: $('#kt_subheader_search_form'),
                delay: 400,
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: '',
                    title: '#',
                    width: 50,
                    template: function (row, index) {
                        return index + 1;
                    }
                },
                {
                    field: 'FirstName',
                    title: 'Full Name',
                    width: 200,
                    template: function (row) {
                        return '<div class="d-flex align-items-center">\
                                <div class="symbol symbol-40 flex-shrink-0">\
                                    <div class="symbol-label">\
                                       <span class="symbol symbol-lg-35 symbol-25 symbol-primary" >\
								       <span class="symbol-label font-size-h5 font-weight-bold">'+ row.FirstName[0].toUpperCase() + '</span></span>\
                                    </div>\
                                </div>\
                                <div class="ml-2">\
                                <div class="text-dark-75 font-weight-bold line-height-sm"> ' + row.FirstName + ' ' + row.LastName + '</div>\
                                    <a class="font-size-sm text-dark-50 text-hover-primary">' + row.Title + '</a>\
                                </div>\
                            </div>';

                    }
                },
                {
                    field: 'UserName',
                    title: 'Username',
                    width: 120
                },
                {
                    field: 'Email',
                    title: 'Email',
                    width: 220
                },
                {
                    field: 'PhoneNumber',
                    title: 'Phone'
                },
                {
                    field: 'Roles',
                    title: 'Roles',
                    template: function (row) {
                        var output = '';
                        if (row.Roles.length > 0) {
                            for (var i = 0; i < row.Roles.length; i++) {
                                output += '<span class=" mr-1 mb-1 label label-lg font-weight-bold label-inline">' + row.Roles[i].Name + '</span>';
                            }
                        }
                        return output;
                    },
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return "\
	                        <a onclick='editUser(\"" + row.Id + "\")' class='btn btn-icon btn-light btn-hover-info btn-sm mx-3' title='Edit'>\
	                        <span class='svg-icon svg-icon-md svg-icon-info'>\
							<svg xmlns = 'http://www.w3.org/2000/svg' xmlns: xlink = 'http://www.w3.org/1999/xlink' width = '24px' height = '24px' viewBox = '0 0 24 24' version = '1.1' >\
							<g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
								<rect x='0' y='0' width='24' height='24'></rect>\
								<path d='M12.2674799,18.2323597 L12.0084872,5.45852451 C12.0004303,5.06114792 12.1504154,4.6768183 12.4255037,4.38993949 L15.0030167,1.70195304 L17.5910752,4.40093695 C17.8599071,4.6812911 18.0095067,5.05499603 18.0083938,5.44341307 L17.9718262,18.2062508 C17.9694575,19.0329966 17.2985816,19.701953 16.4718324,19.701953 L13.7671717,19.701953 C12.9505952,19.701953 12.2840328,19.0487684 12.2674799,18.2323597 Z' fill='#000000' fill-rule='nonzero' transform='translate(14.701953, 10.701953) rotate(-135.000000) translate(-14.701953, -10.701953) '></path>\
								<path d='M12.9,2 C13.4522847,2 13.9,2.44771525 13.9,3 C13.9,3.55228475 13.4522847,4 12.9,4 L6,4 C4.8954305,4 4,4.8954305 4,6 L4,18 C4,19.1045695 4.8954305,20 6,20 L18,20 C19.1045695,20 20,19.1045695 20,18 L20,13 C20,12.4477153 20.4477153,12 21,12 C21.5522847,12 22,12.4477153 22,13 L22,18 C22,20.209139 20.209139,22 18,22 L6,22 C3.790861,22 2,20.209139 2,18 L2,6 C2,3.790861 3.790861,2 6,2 L12.9,2 Z' fill='#000000' fill-rule='nonzero' opacity='0.3'></path>\
							</g>\
							</svg>\
	                        </a>\
	                        <a  onclick='deleteUser(\"" + row.Id + "\", \"" + row.UserName + "\")' class='btn btn-sm btn-light btn-hover-danger btn-icon' title='Delete'>\
								<span class='svg-icon svg-icon-md svg-icon-danger'>\
									<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'>\
										<g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
											<rect x='0' y='0' width='24' height='24'/>\
											<path d='M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z' fill='#000000' fill-rule='nonzero'/>\
											<path d='M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z' fill='#000000' opacity='0.3'/>\
										</g>\
									</svg>\
								</span>\
	                        </a>";
                    },
                }],
        });
        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Status');
        });

        $('#kt_datatable_search_type').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Type');
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();
    };

    return {
        // public functions
        init: function () {
            _demo();
        },
    };
}();

function editUser(id) {
    KTApp.blockPage();
    $.ajax({
        url: "/v1/user/editpartial?id=" + id,
        type: "GET",
        data: {},
        success: function (response) {
            $("#userEdit").html(response);
            $("#editBtn").click();
            KTApp.unblockPage();
        },
        error: function (xhr) {
        }
    });
}

jQuery(document).ready(function () {
    KTAppsUsersListDatatable.init();

    $("#userSubmission").click(function (e) {
        _frmvalidation.validate().then(function (status) {
            if (status == 'Valid') {
                e.preventDefault();
                $("#closeModal").click();
                KTApp.blockPage();
                var valdata = $("#_frm").serialize();
                $.ajax({
                    url: "/v1/user/createuser",
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: valdata,
                    success: function (response) {
                        KTApp.unblockPage();
                        if (response.success) {

                            datatable.reload();
                            KTUtil.scrollTop();
                            toastr.options = {
                                "closeButton": false,
                                "debug": false,
                                "newestOnTop": true,
                                "progressBar": false,
                                "positionClass": "toast-top-center",
                                "preventDuplicates": false,
                                "onclick": null,
                                "showDuration": "300",
                                "hideDuration": "1000",
                                "timeOut": "5000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };
                            toastr.success("Data has been saved successfully!");
                        }
                        else {
                            if (response.message == "400") {
                                toastr.options = {
                                    "closeButton": false,
                                    "debug": false,
                                    "newestOnTop": true,
                                    "progressBar": false,
                                    "positionClass": "toast-top-center",
                                    "preventDuplicates": false,
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "1000",
                                    "timeOut": "5000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };
                                toastr.error("That email is already registered!");
                            }
                            else {
                                toastr.options = {
                                    "closeButton": false,
                                    "debug": false,
                                    "newestOnTop": true,
                                    "progressBar": false,
                                    "positionClass": "toast-top-center",
                                    "preventDuplicates": false,
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "1000",
                                    "timeOut": "5000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };
                                toastr.error("Something went wrong!");
                            }
                        }

                    }
                });
            }
            else {
                return;
            }
        });
    });
    $("#userEdition").click(function (e) {
        _editFrm.validate().then(function (status) {
            if (status == 'Valid') {
                e.preventDefault();
                $("#closeModal2").click();
                KTApp.blockPage();
                var valdata = $("#_editFrm").serialize();
                $.ajax({
                    url: "/v1/user/edituser",
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: valdata,
                    success: function (response) {
                        KTApp.unblockPage();
                        if (response.success) {
                            datatable.reload();
                            KTUtil.scrollTop();
                            toastr.options = {
                                "closeButton": false,
                                "debug": false,
                                "newestOnTop": true,
                                "progressBar": false,
                                "positionClass": "toast-top-center",
                                "preventDuplicates": false,
                                "onclick": null,
                                "showDuration": "300",
                                "hideDuration": "1000",
                                "timeOut": "5000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };
                            toastr.success("Data has been saved successfully!");
                        }
                        else {
                            toastr.options = {
                                "closeButton": false,
                                "debug": false,
                                "newestOnTop": true,
                                "progressBar": false,
                                "positionClass": "toast-top-center",
                                "preventDuplicates": false,
                                "onclick": null,
                                "showDuration": "300",
                                "hideDuration": "1000",
                                "timeOut": "5000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };

                            toastr.error("Something went wrong!");
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