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
                   url: '/efatorty/v1/document/ajax_invalidandfailed',
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
       layout: {
           scroll: true,
           height: 800,
           footer: false
       },
        sortable: true,
        pagination: true,
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
                template: function (row) {
                    var status = {
                        invalid: { 'title': 'Invalid', 'class': 'label-danger' },
                        failed:  { 'title': 'Failed',  'class': 'label-warning'},
                    };
                    return '<span class="label label-lg font-weight-bold ' + status[row.status.toLowerCase()].class + ' label-inline">' + status[row.status.toLowerCase()].title + '</span>';
                }
            },
            {
                field: 'internalID',
                title: 'Internal Id',
                name: "internalID",
                sortable: false,
                width: 200,
                template: function (row) {
                    return "<a href='/efatorty/v1/document/details/" + row.internalID +"' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.internalID + "</a>";
            },
            },
            {
                field: 'documentType',
                title: 'Type/Version',
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
                sortable: 'desc',
                width: 125,
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
                                        <a href='/efatorty/v1/document/details/"+ row.internalID +"' class='navi-link submitdoc'>\
                                            <span class='navi-icon'><i class='la la-eye'></i></span>\
                                            <span class='navi-text'>View</span>\
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
                                    <a href='/efatorty/v1/document/details/"+ row.internalID +"' class='navi-link submitdoc'>\
                                        <span class='navi-icon'><i class='la la-eye'></i></span>\
                                        <span class='navi-text'>View</span>\
                                    </a>\
                                </li>\
                                <li class='navi-item'>\
                                            <a href='#' onclick='UpdateDocumentByInternalId(\"" + row.internalID + "\")' class='navi-link'>\
                                                <span class='navi-icon'><i class='la la-undo-alt'></i></span>\
                                                <span class='navi-text'>Recall</span>\
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
                                                <a href='/efatorty/v1/document/details/"+ row.internalID +"' class='navi-link submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-eye'></i></span>\
                                                    <span class='navi-text'>View</span>\
                                                </a>\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='Resync(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-refresh'></i></span>\
                                                    <span class='navi-text'>Resync</span>\
                                                </a>\
                                            </li>\
                                        </ul>\
                                    </div>\
                                </div>";
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
                                                <a href='/efatorty/v1/document/details/"+ row.internalID+"' class='navi-link submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-eye'></i></span>\
                                                    <span class='navi-text'>View</span>\
                                                </a>\
                                            </li>\
                                            <li class='navi-item'>\
                                                <a onclick='Resync(\"" + row.internalID + "\")' class='navi-link _do submitdoc'>\
                                                    <span class='navi-icon'><i class='la la-refresh'></i></span>\
                                                    <span class='navi-text'>Resync</span>\
                                                </a>\
                                            </li>\
                                         <li class='navi-item'>\
                                                        <a href='#' onclick='UpdateDocumentByInternalId(\"" + row.internalID + "\")' class='navi-link'>\
                                                            <span class='navi-icon'><i class='la la-undo-alt'></i></span>\
                                                            <span class='navi-text'>Recall</span>\
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
        options.extensions = {
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
            $('#kt_datatable_fetch_modal').html("Resync Top " + count + " <i class='flaticon-refresh'></i>");
                $('#kt_datatable_fetch_modal_recallAll').html("Recall Top " + count + " <i class='flaticon2-refresh-arrow'></i>");

                if (count !== options.data.pageSize && count !== 1 && 1 > count > options.data.pageSize) {
                    $("#kt_datatable_sync").hide();
                    $("#kt_datatable_syncAll").hide();
                    $("#kt_datatable_Recall").hide();
                    $("#kt_datatable_recallAll").hide();
                }
                else if (count === 1 || count < options.data.pageSize)
                {
                    $("#kt_datatable_syncAll").hide();
                    $("#kt_datatable_fetch_modal").hide();
                    $("#kt_datatable_recallAll").hide();
                    $("#kt_datatable_fetch_modal_recallAll").hide();
                    $("#kt_datatable_sync").show(); 
                    $("#kt_datatable_Recall").show(); 
                }
                else {
                    $("#kt_datatable_sync").hide(); 
                    $("#kt_datatable_Recall").hide(); 
                    $("#kt_datatable_syncAll").show();
                    $("#kt_datatable_recallAll").show();
                    $("#kt_datatable_fetch_modal").show();
                    $("#kt_datatable_fetch_modal_recallAll").show();
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
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Resyncing...");

        $.post('/efatorty/v1/document/ResyncDocuments', { DocumentIds: ids },
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData != -1) {
                    Swal.fire({
                        title: 'The documents have been resynced successfully with the below results.',
                        html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Resynced Documents:  [' + returnedData + ']</span>\
                               <span class="navi-text" style= "float:left; clear:left;">Non-exist Documents: ['+ (ids.length - returnedData) + ']</span>',
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
                    LoadInvalidandFailed();
                }
                else  {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text:  "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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

    $('#kt_datatable_sync').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });

        var ids = datatable.rows('.datatable-row-active').nodes().find('.checkbox > [type="checkbox"]').map(function (i, chk) {
            return $(chk).val();
        }).toArray();

        var btn = KTUtil.getById("kt_datatable_sync");

        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Resyncing...");

        //Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
        // Ajax Call to Submit documnets Web Contoller
        //var FilteredDocuments = Result.filter(doc => ids.indexOf(doc.internalID) != -1);

        $.post('/efatorty/v1/document/ResyncDocuments', { DocumentIds: ids },
            function (returnedData) {
            KTUtil.btnRelease(btn);
            $('#kt_datatable_group_action_form').collapse('hide');
            KTApp.unblockPage();
                if (returnedData != -1) {
                    Swal.fire({
                        title: 'The documents have been resynced successfully with the below results.',
                        html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Resynced Documents:  [' + returnedData + ']</span>\
                               <span class="navi-text" style= "float:left; clear:left;">Non-exist Documents: ['+ (ids.length - returnedData) + ']</span>',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                text: "No connection could be made because the target machine actively refused it",
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

    $('#kt_datatable_syncAll').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });

        var btn = KTUtil.getById("kt_datatable_syncAll");

        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-primary pl-15", "Resyncing...");
        // Ajax Call to Submit documnets Web Contoller

        $.post('/efatorty/v1/document/ResyncAllDocuments',
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData != -1) {
                    Swal.fire({
                        title: 'The documents have been resynced successfully with the below results.',
                        html: '<span class="navi-text mb-1" style= "float:left; clear:left;">Resynced Documents:  [' + returnedData + ']</span>\
                               <span class="navi-text" style= "float:left; clear:left;">Non-exist Documents: ['+ (ids.length - returnedData) + ']</span>',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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


    $('#kt_datatable_fetch_modal_recallAll').on('click', function (e) {
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


        var btn = KTUtil.getById("kt_datatable_fetch_modal_recallAll");
        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Recalling...");

        $.post('/efatorty/v1/document/RecallDocuments', { DocumentIds: ids },
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData) {
                    Swal.fire({
                        title: 'The documents have been recalled successfully.',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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

    $('#kt_datatable_Recall').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });

        var ids = datatable.rows('.datatable-row-active').nodes().find('.checkbox > [type="checkbox"]').map(function (i, chk) {
            return $(chk).val();
        }).toArray();

        var btn = KTUtil.getById("kt_datatable_Recall");

        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15", "Recalling...");


        $.post('/efatorty/v1/document/RecallDocuments', { DocumentIds: ids },
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData) {
                    Swal.fire({
                        text: 'The documents have been recalled successfully.',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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

    $('#kt_datatable_recallAll').on('click', function (e) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });

        var btn = KTUtil.getById("kt_datatable_recallAll");

        KTUtil.btnWait(btn, "spinner spinner-left spinner-light-primary pl-15", "Recalling...");
        // Ajax Call to Submit documnets Web Contoller

        $.post('/efatorty/v1/document/RecallAllDocuments',
            function (returnedData) {
                KTUtil.btnRelease(btn);
                $('#kt_datatable_group_action_form').collapse('hide');
                KTApp.unblockPage();
                if (returnedData) {
                    Swal.fire({
                        text: 'The documents have been recalled successfully.',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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
    KTDatatableRecordSelectionDemo.init();
}

function EditDocument(InternalId) {
    //var AllDocs = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    //var TargetedDoc = AllDocs.filter(doc => doc.internalID == DocumentId);
    //sessionStorage.setItem("PendingDocs", JSON.stringify(TargetedDoc));
    window.location.href = "/efatorty/v1/document/edit_document?InternalId=" + InternalId;
}

function UpdateDocumentByInternalId(InternalId) {
    KTApp.blockPage({
        overlayColor: '#000000',
        state: 'primary'
    });
    $.ajax({
        url: "/efatorty/v1/document/UpdateDocumentByInternalId?InternalId=" + InternalId,
        type: "get", //send it through get method
        data: {},
        success: function (response) {
            datatable.reload();
            KTApp.unblockPage();
            if (response.status == "Success") {
                Swal.fire({
                    text: 'Document has been recalled successfully, Now you can send it agian.',
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                    window.location.href = "/efatorty/v1/document/pending";
                });
            }
            else {
                Swal.fire({
                    text: "Sorry, something went wrong, please try again.",
                    //text: "Internal Server Error: " + result.message,
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

        },
        error: function (xhr) {
            KTApp.unblockPage();
            //Do Something to handle error
            Swal.fire({
                text: "Sorry, something went wrong, please try again.",
                //text: "Internal Server Error: " + res,
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
}

function Resync(id) {
        KTApp.blockPage({
            overlayColor: '#000000',
            state: 'primary',
            message: 'Please wait as this may take a few seconds'
        });

        $.post('/efatorty/v1/document/ResyncDocuments', { DocumentIds: [id] },
            function (returnedData) {
                KTApp.unblockPage();
                if (returnedData != -1) {
                    Swal.fire({
                        text: 'The document has been resynced successfully.',
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
                    LoadInvalidandFailed();
                }
                else {
                    Swal.fire({
                        title: "Sorry, something went wrong, please try again.",
                        text: "No connection could be made because the target machine actively refused it",
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
                    text: "No connection could be made because the target machine actively refused it",
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