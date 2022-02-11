"use strict";
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var fromDate = new Date(y, m, 1);
var toDate = new Date();
var Result;
var datatable;
var KTDatatableRecordSelectionDemo = function () {

   var options = {
        // datasource definition
        data: {
           type: 'remote',
           source: {
                read: {
                   method: 'POST',
                   url: '/v1/document/ajax_pending',
                   map: function (raw) {
                        // 
                        // sample data mapping
                        var dataSet = raw;
                        if (typeof raw.data !== 'undefined') {
                            dataSet = raw.data;
                        }
                        return dataSet;
                   },
                   timeout: 1000000
                },
            },
           pageSize: 10,
           serverPaging: true,
           serverFiltering: true,
           serverSorting: true,
           saveState: { cookie: false }
        },
        // layout definition
        layout: {
            scroll: true,
            height: 800, // enable/disable datatable scroll both horizontal and vertical
            footer: false // display/hide footer
        },
        // column sorting
        sortable: true,
        pagination: true,
        // columns definition
        columns: [
            {
                field: 'recordID',
                title: '#',
                sortable: false,
                width: 35,
                selector: {
                    class: ''
                },
                textAlign: 'center',
            },
            {
                field: 'status',
                title: 'Status',
                width:80,
                // callback function support for column rendering
                template: function (row) {
                    var status = {
                        new:    { 'title': 'New', 'class': 'label-light-primary' },
                        failed: { 'title': 'Failed', 'class': ' label-light-warning' },
                        updated:{ 'title': 'Updated', 'class': ' label-light-info' }
                    };
                    return '<span class="label label-lg font-weight-bold' + status[row.status.toLowerCase()].class + ' label-inline">' + status[row.status.toLowerCase()].title + '</span>';
                }
            },
            {
                field: 'internalID',
                title: 'Internal Id',
                name: "internalID",
                sortable: false,
                width: 135,
                template: function (row) {
                    return "<a href='#' onclick='ViewDocument_NewTab(\"" + row.internalID + "\")' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.internalID + "</a>";
            },
            },
            {
                field: 'documentType',
                title: 'Type/Version',
                // callback function support for column rendering
                template: function (row) {
                    var documentType = {
                        i: { 'title': 'Invoice'},
                        c: { 'title': 'Credit Note'},
                        d: { 'title': 'Debit Note'},
                    };
                    return '<span class="navi-text" style= "float:left; clear:left;">'+ documentType[row.documentType.toLowerCase()].title + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + row.documentTypeVersion + '</span>';
                }
            },
            {

                field: 'dateTimeIssued',
                title: 'Issued Date',
                template: function (row) {
                    var temp = convertToJavaScriptDate(new Date(parseInt(row.dateTimeIssued.substr(6)))).split(" ");
                    return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + temp[1] +' '+ temp[2] + '</span>';
                }
            },
            {
                field: 'receiver.name',
                title: 'Receiver Name/Id',
                width: 170,
                sortable: false,
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + ((row.receiver.name) ? row.receiver.name : 'NA') + '</span>\
                            <span class="navi-text" style= "float:left; clear:left; color:#02bda1;">' + ((row.receiver.id) ? row.receiver.id : 'NA') + '</span>';
                }
            },
            {
                field: 'totalSalesAmount',
                title: 'Sales Amount (EGP)'
            },
            {
                field: 'totalItemsDiscountAmount',
                title: 'Discount (EGP)'
            },
            {
                field: 'totalAmount',
                title: 'Total Amount (EGP)'
            },
            {
                field: 'taxAmount',
                title: 'Tax (EGP)'
            },
            {
                field: 'receiver.type',
                title: '#Line',
                template: function (row) {
                    return row.invoiceLines.length;
                }
            },
            {
                field: 'receiver.address',
                title: '#Tax Type',
                template: function (row) {
                    return row.taxTotals.length;
                }
            },
            {
                field: 'Actions',
                title: 'Actions',
                width: 125,
                sortable: false,
                overflow: 'visible',
                textAlign: 'left',
                autoHide: false,
                template: function (row) {
                    if (row.isInternallyCreated) {
                        if (row.status.toLowerCase() == "failed") {
                            return "\
                            <div class='dropdown dropdown-inline'>\
                                <a href='javascript:;' class='btn btn-sm btn-clean btn-icon mr-2' data-toggle='dropdown'>\
                                    <span class='svg-icon svg-icon-md'>\
                                        <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'>\
                                            <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
                                                <rect x='0' y='0' width='24' height='24'/>\
                                                <path d='M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z' fill='#000000'/>\
                                            </g>\
                                        </svg>\
                                    </span>\
                                </a>\
                            <div class='dropdown-menu dropdown-menu-sm dropdown-menu-right'>\
                                <ul class='navi flex-column navi-hover py-2'>\
                                    <li class='navi-header font-weight-bolder text-uppercase font-size-xs text-primary pb-2'>\
                                        Choose an action:\
                                    </li>\
                                    <li class='navi-item'>\
                                        <a onclick='ViewDocument(\"" + row.internalID + "\")' class='navi-link submitdoc'>\
                                            <span class='navi-icon'><i class='la la-eye'></i></span>\
                                            <span class='navi-text'>View</span>\
                                        </a>\
                                    </li>\
                                    <li class='navi-item'>\
                                        <a onclick='SubmitDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                            <span class='navi-icon'><i class='la la-paper-plane-o'></i></span>\
                                            <span class='navi-text'>Submit</span>\
                                        </a>\
                                    </li>\
                                    <li class='navi-item'>\
                                        <a onclick='EditDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                            <span class='navi-icon'><i class='la la-edit'></i></span>\
                                            <span class='navi-text'>Edit</span>\
                                        </a>\
                                    </li>\
                                </ul>\
                            </div>\
                    </div>\
                        <button data-record-id='" + row.internalID + "' class='btn btn-sm btn-clean submitdoc' title = 'View records'>\
                        <i class='flaticon-close'></i> Why?\
		                </button >";
                        }
                        else {
                            return "\
                    <div class='dropdown dropdown-inline'>\
                        <a href='javascript:;' class='btn btn-sm btn-clean btn-icon mr-2' data-toggle='dropdown'>\
                            <span class='svg-icon svg-icon-md'>\
                                <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'>\
                                    <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
                                        <rect x='0' y='0' width='24' height='24'/>\
                                        <path d='M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z' fill='#000000'/>\
                                    </g>\
                                </svg>\
                            </span>\
                        </a>\
                        <div class='dropdown-menu dropdown-menu-sm dropdown-menu-right'>\
                            <ul class='navi flex-column navi-hover py-2'>\
                                <li class='navi-header font-weight-bolder text-uppercase font-size-xs text-primary pb-2'>\
                                    Choose an action:\
                                </li>\
                                <li class='navi-item'>\
                                    <a onclick='ViewDocument(\"" + row.internalID + "\")' class='navi-link submitdoc'>\
                                        <span class='navi-icon'><i class='la la-eye'></i></span>\
                                        <span class='navi-text'>View</span>\
                                    </a>\
                                </li>\
                                <li class='navi-item'>\
                                    <a onclick='SubmitDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                        <span class='navi-icon'><i class='la la-paper-plane-o'></i></span>\
                                        <span class='navi-text'>Submit</span>\
                                    </a>\
                                </li>\
                                <li class='navi-item'>\
                                        <a onclick='EditDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                            <span class='navi-icon'><i class='la la-edit'></i></span>\
                                            <span class='navi-text'>Edit</span>\
                                        </a>\
                               </li>\
                            </ul>\
                        </div>\
                    </div>";
                        }
                    }
                    else
                    {
                        if (row.status.toLowerCase() == "failed") {
                            return "\
                                    <div class='dropdown dropdown-inline'>\
                                        <a href='javascript:;' class='btn btn-sm btn-clean btn-icon mr-2' data-toggle='dropdown'>\
                                            <span class='svg-icon svg-icon-md'>\
                                                <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'>\
                                                    <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
                                                        <rect x='0' y='0' width='24' height='24'/>\
                                                        <path d='M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z' fill='#000000'/>\
                                                    </g>\
                                                </svg>\
                                            </span>\
                                        </a>\
                                    <div class='dropdown-menu dropdown-menu-sm dropdown-menu-right'>\
                                        <ul class='navi flex-column navi-hover py-2'>\
                                            <li class='navi-header font-weight-bolder text-uppercase font-size-xs text-primary pb-2'>\
                                                Choose an action:\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='ViewDocument(\"" + row.internalID + "\")' class='navi-link submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-eye'></i></span>\
                                                    <span class='navi-text'>View</span>\
                                                </a>\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='SubmitDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-paper-plane-o'></i></span>\
                                                    <span class='navi-text'>Submit</span>\
                                                </a>\
                                            </li>\
                                        </ul>\
                                    </div>\
                                </div>\
                        <button data-record-id='" + row.internalID + "' class='btn btn-sm btn-clean submitdoc' title = 'View records'>\
                        <i class='flaticon-close'></i> Why?\
		                </button >";
                        }
                        else
                        {
                            return "\
                                <div class='dropdown dropdown-inline'>\
                                    <a href='javascript:;' class='btn btn-sm btn-clean btn-icon mr-2' data-toggle='dropdown'>\
                                        <span class='svg-icon svg-icon-md'>\
                                            <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'>\
                                                <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>\
                                                    <rect x='0' y='0' width='24' height='24'/>\
                                                    <path d='M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z' fill='#000000'/>\
                                                </g>\
                                            </svg>\
                                        </span>\
                                    </a>\
                                    <div class='dropdown-menu dropdown-menu-sm dropdown-menu-right'>\
                                        <ul class='navi flex-column navi-hover py-2'>\
                                            <li class='navi-header font-weight-bolder text-uppercase font-size-xs text-primary pb-2'>\
                                                Choose an action:\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='ViewDocument(\"" + row.internalID + "\")' class='navi-link submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-eye'></i></span>\
                                                    <span class='navi-text'>View</span>\
                                                </a>\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='SubmitDocument(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-paper-plane-o'></i></span>\
                                                    <span class='navi-text'>Submit</span>\
                                                </a>\
                                            </li>\
                                        </ul>\
                                    </div>\
                                </div>";
                        }
                    }
                    
                },
            }],
    };
    // basic demo
    var localSelectorDemo = function () {

        options.data.source.read.params =
        {
            fromDate: ModifyDate(fromDate),
            toDate: ModifyDate(toDate)
        }
        // enable extension
        options.extensions = {
            // boolean or object (extension options)
            checkbox: true,
        };
        options.search = {
            input: $('#kt_datatable_search_query'),
            delay: 1000,
            key: 'generalSearch'
        };
        datatable = $('#kt_datatable').KTDatatable(options);
        
        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val(), 'status');
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();

        datatable.on('datatable-on-check datatable-on-uncheck',function (e) {
                var checkedNodes = datatable.rows('.datatable-row-active').nodes();
                var count = checkedNodes.length; 
                $('#kt_datatable_selected_records').html(count);
                $('#kt_datatable_fetch_modal').html("Send Top " + count + " <i class='far fa-arrow-alt-circle-right'></i>");

                if (count !== options.data.pageSize && count !== 1 && 1 > count > options.data.pageSize) {
                    $("#kt_datatable_send").hide(); 
                    $("#kt_datatable_sendAll").hide();
                }
                else if (count === 1 || count < options.data.pageSize)
                {
                    $("#kt_datatable_sendAll").hide();
                    $("#kt_datatable_fetch_modal").hide();
                    $("#kt_datatable_send").show(); 
                }
                else {
                    $("#kt_datatable_send").hide(); 
                    $("#kt_datatable_sendAll").show();
                    $("#kt_datatable_fetch_modal").show();
                }
                if (count > 0)
                {
                    $('#kt_datatable_group_action_form').collapse('show');
                }
                else
                {
                    $('#kt_datatable_group_action_form').collapse('hide');
                }
        });
        datatable.on('click', '[data-record-id]', function () {
            localStorage.clear();
            Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
            initSubDatatable($(this).data('record-id'));
            $('#kt_datatable_modal').modal('show');
        });

        
    };
    
    return {
        // public functions
        init: function () {
            localStorage.clear();
            localSelectorDemo();
        },
    };
}();
jQuery(document).ready(function () {
    sessionStorage.removeItem("PendingDocs");
    $("#pending_fromDate").val(((fromDate.getDate() > 9) ? fromDate.getDate() : ('0' + fromDate.getDate())) + '-' + monthNames[((fromDate.getMonth() > 8) ? (fromDate.getMonth()) : ((fromDate.getMonth())))] + '-' + fromDate.getFullYear());
    $("#pending_toDate").val(((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '-' + monthNames[((toDate.getMonth() > 8) ? (toDate.getMonth()) : ((toDate.getMonth())))] + '-' + toDate.getFullYear());
    fromDate = ($("#pending_fromDate").val()) ? $("#pending_fromDate").val() : '';
    toDate = ($("#pending_toDate").val()) ? $("#pending_toDate").val() : '';
    KTDatatableRecordSelectionDemo.init();

    $('#kt_datatable_fetch_modal').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });
        var ids = datatable.rows('.datatable-row-active').
            nodes().
            find('.checkbox > [type="checkbox"]').
            map(function (i, chk) {
                return $(chk).val();
            }).toArray();
        var btn = KTUtil.getById("kt_datatable_fetch_modal");
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Sending...");
        Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
        // Ajax Call to Submit documnets Web Contoller
        var FilteredDocuments = Result.filter(doc => ids.indexOf(doc.internalID) != -1);
        $.post('/v1/documentsubmission/submit', { obj: FilteredDocuments },
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData.status == "1") {
                    Swal.fire({
                        title: 'Process has been executed successfully! with the below results.',
                        html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Submitted Documents: [' + returnedData.data.acceptedDocuments.length + ']</span>\
                                       <span class="navi-text" style= "float:left; clear:left;">Failed Documents: ['+ returnedData.data.rejectedDocuments.length + ']</span>',
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                    datatable.reload();
                    LoadDraft();
                    LoadSent();
                }
                else if (returnedData.status == "2") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "ETA & Signer Integration Failure, Please check logs.",
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
                else if (returnedData.status == "3") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "Internal Integration Failure due to the following: " + returnedData.message,
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
                else if (returnedData.status == "5") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text:  returnedData.message,
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
                else if (returnedData.status == "401") {
                    Swal.fire({
                        title: "Your license has expired. You can no longer use this product.",
                        text:  "To purchase a new license of this product, Contact the product owner.",
                        icon: "info",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
                else  {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "Inner Exception Failure due to the following: " + returnedData.message,
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
            }).fail(function () {
                KTUtil.btnRelease(btn);
                KTApp.unblockPage();
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: "Internal Server Error: " + returnedData.message,
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
            });
    });

    $('#kt_datatable_send').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });
        var ids = datatable.rows('.datatable-row-active').nodes().find('.checkbox > [type="checkbox"]').map(function (i, chk) {
            return $(chk).val();
        }).toArray();
        var btn = KTUtil.getById("kt_datatable_send");
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Sending...");
        Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
        // Ajax Call to Submit documnets Web Contoller
        var FilteredDocuments = Result.filter(doc => ids.indexOf(doc.internalID) != -1);
        $.post('/v1/documentsubmission/submit', { obj: FilteredDocuments }, function (returnedData) {
            KTUtil.btnRelease(btn);
            $('#kt_datatable_group_action_form').collapse('hide');
            KTApp.unblockPage();
            if (returnedData.status == "1") {
                Swal.fire({
                    title: 'Process has been executed successfully! with the below results.',
                    html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Submitted Documents: [' + returnedData.data.acceptedDocuments.length + ']</span>\
                                   <span class="navi-text" style= "float:left; clear:left;">Failed Documents: ['+ returnedData.data.rejectedDocuments.length + ']</span>',
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
                datatable.reload();
                LoadDraft();
                LoadSent();
            }
            else if (returnedData.status == "2") {
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: "ETA & Signer Integration Failure, Please check logs.",
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
            else if (returnedData.status == "3") {
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: "Internal Integration Failure due to the following: " + returnedData.message,
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
            else if (returnedData.status == "5") {
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: returnedData.message,
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
            else if (returnedData.status == "401") {
                Swal.fire({
                    title: "Your license has expired. You can no longer use this product.",
                    text: "To purchase a new license of this product, Contact the product owner.",
                    icon: "info",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
            }
            else {
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: "Inner Exception due to the following: " + returnedData.message,
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
        }).fail(function () {
            KTUtil.btnRelease(btn);
            KTApp.unblockPage();
            Swal.fire({
                title: "Sorry, something went wrong, please try again.",
                text: "Internal Server Error: " + returnedData.message,
                icon: "error",
                buttonsStyling: false,
                confirmButtonText: "Ok, got it!",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
        });
    });

    // Send All Documents in one shot
    $('#kt_datatable_sendAll').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });
        var btn = KTUtil.getById("kt_datatable_sendAll");
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-primary pl-15", "Sending...");
        // Ajax Call to Submit documnets Web Contoller
        $.post('/v1/documentsubmission/auto_submit',
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData.status == "1") {
                    Swal.fire({
                        title: 'Process has been executed successfully! with the below results.',
                        html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Submitted Documents: [' + returnedData.data.acceptedDocuments.length + ']</span>\
                                   <span class="navi-text" style= "float:left; clear:left;">Failed Documents: ['+ returnedData.data.rejectedDocuments.length + ']</span>',
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                    datatable.reload();
                    LoadDraft();
                    LoadSent();
                }
                else if (returnedData.status == "2") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "ETA & Signer Integration Failure, Please check logs.",
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
                else if (returnedData.status == "3") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "Internal Integration Failure due to the following: " + returnedData.message,
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
                else if (returnedData.status == "5") {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: returnedData.message,
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
                else if (returnedData.status == "401") {
                    Swal.fire({
                        title: "Your license has expired. You can no longer use this product.",
                        text: "To purchase a new license of this product, Please contact the product owner.",
                        icon: "info",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "Inner Exception due to the following: " + returnedData.message,
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
            }).fail(function () {
                KTUtil.btnRelease(btn);
                KTApp.unblockPage();
                Swal.fire({
                    title: "Sorry, something went wrong, please try again.",
                    text: "Internal Server Error: " + returnedData.message,
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
            });
    });

    $("#_find").on('click', function () {
        searchData();
    });

    $("#_sync").on('click', function () {
        syncData();
    });

    //$('._do').on('click', function () { alert('11') });
    //$('._do').on('click', function () { alert('11') });
});
const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];
function convertToJavaScriptDate(value) {
    var dt = value;
    var hours = dt.getHours();
    var minutes = dt.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return dt.getDate() + "-" + monthNames[(dt.getMonth())] + "-" + dt.getFullYear() + " "+ strTime;
}
var func = function (input)
{
    alert(input);
    $('#cover-spin').show(0);
    Result = datatable.rows().data().KTDatatable.dataSet.where(p => p.internalID == id).map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    // Ajax Call to Submit documnets Web Contoller
    var FilteredDocuments = Result.filter(doc => ids.indexOf(doc.internalID) != -1);
    $.post('/v1/documentsubmission/submit', { obj: FilteredDocuments },
        function (returnedData) {
            datatable.reload();
            $('#cover-spin').hide(0);
        }).fail(function () {
            $('#cover-spin').hide(0);
        });
    return true;
}
function ModifyDate(date) {
    if (date) {
        date = date.toString().split("-");
        // After this construct a string with the above results as below
        return date[2] + "-" + ((monthNames.indexOf(date[1]) < 9) ? ("0" + (parseInt(monthNames.indexOf(date[1])) + 1)) : (parseInt(monthNames.indexOf(date[1])) + 1)) + "-" + date[0];
    }
    else
        return null;

}
function searchData() {
    fromDate = ($("#pending_fromDate").val()) ? $("#pending_fromDate").val() : '';
    toDate = ($("#pending_toDate").val()) ? $("#pending_toDate").val() : '';
    datatable.destroy();
    //localStorage.clear();
    KTDatatableRecordSelectionDemo.init();
}

function SubmitDocument(DocumentId) {
    KTApp.blockPage({
        overlayColor: '#000000',
        state: 'primary',
        message: 'Please wait as this may take a few seconds'
    });
    Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    // Ajax Call to Submit documnets Web Contoller
    var FilteredDocuments = Result.filter(doc => doc.internalID == DocumentId);
    $.post('/v1/documentsubmission/submit', { obj: FilteredDocuments }, function (returnedData) {
        KTApp.unblockPage();
        if (returnedData.status == "1") {
            Swal.fire({
                title: 'Process has been executed successfully! with the below results.',
                html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Submitted Documents: [' + returnedData.data.acceptedDocuments.length + ']</span>\
                                   <span class="navi-text" style= "float:left; clear:left;">Failed Documents: ['+ returnedData.data.rejectedDocuments.length + ']</span>',
                icon: "success",
                buttonsStyling: false,
                confirmButtonText: "Ok, got it!",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
            datatable.reload();
            LoadDraft();
            LoadSent();
        }
        else if (returnedData.status == "2") {
            Swal.fire({
                title: "Sorry, something went wrong, please try again.",
                text: "ETA & Signer Integration Failure, Please check logs.",
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
        else if (returnedData.status == "3") {
            Swal.fire({
                title: "Sorry, something went wrong, please try again.",
                text: "Internal Integration Failure due to the following: " + returnedData.message,
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
        else if (returnedData.status == "5") {
            Swal.fire({
                title: "Sorry, something went wrong, please try again.",
                text: returnedData.message,
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
        else if (returnedData.status == "401") {
            Swal.fire({
                title: "Your license has expired. You can no longer use this product.",
                text: "To purchase a new license of this product, Contact the product owner.",
                icon: "info",
                buttonsStyling: false,
                confirmButtonText: "Ok, got it!",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
        }
        else {
            Swal.fire({
                title: "Sorry, something went wrong, please try again.",
                text: "Inner Exception due to the following: " + returnedData.message,
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
    }).fail(function () {
        KTUtil.btnRelease(btn);
        KTApp.unblockPage();
        Swal.fire({
            title: "Sorry, something went wrong, please try again.",
            text: "Internal Server Error: " + returnedData.message,
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    });
}

function ViewDocument(DocumentId) {
    var AllDocs = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    var TargetedDoc = AllDocs.filter(doc => doc.internalID == DocumentId);
    sessionStorage.setItem("PendingDocs", JSON.stringify(TargetedDoc));
    window.location.href = "/v1/document/details/" + DocumentId;
}

function ViewDocument_NewTab(DocumentId) {
    var AllDocs = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    var TargetedDoc = AllDocs.filter(doc => doc.internalID == DocumentId);
    sessionStorage.setItem("PendingDocs", JSON.stringify(TargetedDoc));
    window.open("/v1/document/details/" + DocumentId, "_self");
    //window.location.href = "/v1/document/details/" + DocumentId;
}

var initSubDatatable = function (id) {
    var el = $('#kt_datatable_sub');
    var options_sub = {
        data: {
            type: 'local',
            source: Result.find(x => x.internalID == id)?.errors,
            pageSize: 5,
        },
        // layout definition
        layout: {
            theme: 'default',
            scroll: true,
            footer: false,
        },
        sortable: true,
        pagination: true,
        columns: [
            {
                field: 'id',
                title: '#',
                sortable: false,
                width: 200,
                type: 'number',
                textAlign: 'left'
            },
            {
                field: 'target',
                title: 'Target',
                sortable: false,
                width: 200,
                textAlign: 'left'
            },
            {
                field: 'propertyPath',
                title: 'Property Path',
                sortable: false,
                width: 200,
                textAlign: 'left'
            },
            {
                field: 'message',
                title: 'Message',
                width: 200,
                textAlign: 'left'
            },
            {
                field: 'CreatedOn',
                title: 'Created On',
                width: 200,
                textAlign: 'left',
                template: function (row) {
                    if (row.CreatedOn) {
                        var temp = convertToJavaScriptDate(new Date(parseInt(row.CreatedOn.substr(6)))).split(" ");
                        return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                                <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                    }
                    else {
                        return '<span class="navi-text" style= "float:left; clear:left;">NA</span>\
                                <span class="navi-text" style= "float:left; clear:left;">NA</span>';
                    }

                }
            }
        ],
    }
    var datatable = el.KTDatatable(options_sub);
    var modal = datatable.closest('.modal');


    // Fix datatable layout after modal shown
    datatable.hide();
    modal.on('shown.bs.modal', function () {
        var modalContent = $(this).find('.modal-content');
        datatable.spinnerCallback(true, modalContent);
        datatable.spinnerCallback(false, modalContent);
    }).on('hidden.bs.modal', function () {
        el.KTDatatable('destroy');
    });

    datatable.on('datatable-on-layout-updated', function () {
        datatable.show();
        datatable.redraw();
    });

    // Fix datatable layout after modal shown
    //datatable.hide();
    //var alreadyReloaded = false;
    //modal.on('shown.bs.modal', function () {
    //    // 
    //    if (!alreadyReloaded) {
    //        var modalContent = $(this).find('.modal-content');
    //        datatable.spinnerCallback(true, modalContent);

    //        datatable.reload();

    //        datatable.on('datatable-on-layout-updated', function () {
    //            datatable.show();
    //            datatable.spinnerCallback(false, modalContent);
    //            datatable.redraw();
    //        });

    //        alreadyReloaded = true;
    //    }
    //});
};


function EditDocument(InternalId) {
    //var AllDocs = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    //var TargetedDoc = AllDocs.filter(doc => doc.internalID == DocumentId);
    //sessionStorage.setItem("PendingDocs", JSON.stringify(TargetedDoc));
    window.location.href = "/v1/document/edit_document?InternalId=" + InternalId;
}

function syncData() {
    KTApp.blockPage({
        overlayColor: '#000000',
        state: 'primary',
        message: 'Please wait as this may take a few seconds'
    });
    $.ajax({
        url: "/v1/master/SyncCustomerDocumentsByCurrentloggedinOrg",
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (response) {
            if (response.success) {
                KTApp.unblockPage();
                Swal.fire({
                    text: 'Documents have been synced successfully, and all is up to date.',
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    location.reload();
                });
            }
            else {
                Swal.fire({
                    title: "Sorry, something went wrong!",
                    text:  "It might happened becuase procedure doesn't exist, please make sure and try again.",
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