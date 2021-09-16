"use strict";
// Class definition
var Result;
var options;

var KTDatatableRecordSelectionDemo = function() {
    // basic demo
    var localSelectorDemo = function () {
        options = {
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        method: 'POST',
                        url: '/EInvoicing//v0/document/ajax_submitted',
                        map: function (raw) {
                            // 
                            // sample data mapping
                            var dataSet = raw;
                            if (typeof raw.data !== 'undefined') {
                                dataSet = raw.data;
                            }
                            return dataSet;
                        },
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
                    field: 'status',
                    title: 'Status',
                    // callback function support for column rendering
                    template: function (row) {
                        var status = {
                            Submitted: { 'title': 'Submitted', 'class': 'label-primary' },
                            Valid: { 'title': 'Valid', 'class': ' label-success' },
                            Invalid: { 'title': 'Invalid', 'class': ' label-danger' },
                            Cancelled: { 'title': 'Cancelled', 'class': ' label-info' },
                            Rejected: { 'title': 'Rejected', 'class': ' label-warning' }
                        };
                        return '<span class="label label-lg font-weight-bold' + status[row.status].class + ' label-inline">' + row.status + '</span>';
                    }
                },
                {
                    field: 'internalID',
                    title: 'ID/Internal ID',
                    width:220,
                    template: function (row) {
                        return '<a class="btn btn-link no-hover" href="/EInvoicing//v0/document/raw?uuid=' + row.uuid +'" style="padding-left: 0;text-decoration: underline;">' + row.uuid +'</a>\
                                <span class="navi-text" style= "float:left; clear:left;">' + row.internalID + '</span>';
                    }
                },
                {
                    field: 'documentType',
                    title: 'Type/Version',
                    // callback function support for column rendering
                    template: function (row) {
                        var documentType = {
                            i: { 'title': 'Invoice' },
                            c: { 'title': 'Credit Note' },
                            d: { 'title': 'Debit Note' },
                        };
                        return '<span class="navi-text" style= "float:left; clear:left;">' + documentType[row.documentType.toLowerCase()].title + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + row.documentTypeVersion + '</span>';
                    }
                },
                {

                    field: 'dateTimeIssued',
                    title: 'Issued Date',
                    template: function (row) {
                        var temp = convertToJavaScriptDate(new Date(parseInt(row.dateTimeIssued.substr(6)))).split(" ");
                        return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                    }
                },
                {

                    field: 'dateTimeReceived',
                    title: 'Received Date',
                    template: function (row) {
                        var temp = convertToJavaScriptDate(new Date(parseInt(row.dateTimeReceived.substr(6)))).split(" ");
                        return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                    }
                },
                {
                    field: 'receiver.name',
                    title: 'Receiver/Id',
                    sortable: false,
                    width: 170,
                    template: function (row) {
                        if (row.receiver.id == null || row.receiver.id == "") {
                            return '<span class="navi-text" style= "float:left; clear:left;">' + row.receiver.name + '</span>\
                                <span class="navi-text" style= "float:left; clear:left; color:#0bb783;">NA</span>';
                        }
                        else {
                            return '<span class="navi-text" style= "float:left; clear:left;">' + row.receiver.name + '</span>\
                                <span class="navi-text" style= "float:left; clear:left; color:#0bb783;">' + row.receiver.id + '</span>';
                        }

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
                    field: 'uuid',
                    title: 'Id',
                },
                {
                    field: 'submissionUUID',
                    title: 'submission',
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                    sortable: false,
                    width: 80,
                    overflow: 'visible',
                    textAlign: 'left',
                    autoHide: false,
                    template: function (row) {
                        return '\
                    <div class="dropdown dropdown-inline">\
                        <a href="javascript:;" class="btn btn-sm btn-clean btn-icon mr-2" data-toggle="dropdown">\
                            <span class="svg-icon svg-icon-md">\
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                        <rect x="0" y="0" width="24" height="24"/>\
                                        <path d="M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z" fill="#000000"/>\
                                    </g>\
                                </svg>\
                            </span>\
                        </a>\
                        <div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">\
                            <ul class="navi flex-column navi-hover py-2">\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-file-excel"></i></span>\
                                        <span class="navi-text">Cancel</span>\
                                    </a>\
                                </li>\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-link"></i></span>\
                                        <span class="navi-text">Get Public Link</span>\
                                    </a>\
                                </li>\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-print"></i></span>\
                                        <span class="navi-text">Print</span>\
                                    </a>\
                                </li>\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-plus-circle"></i></span>\
                                        <span class="navi-text">Debit Note</span>\
                                    </a>\
                                </li>\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-minus-circle"></i></span>\
                                        <span class="navi-text">Credit Note</span>\
                                    </a>\
                                </li>\
                                <li class="navi-item">\
                                    <a href="/EInvoicing//v0/document/raw?uuid='+ row.uuid + '  "class="navi-link">\
                                        <span class="navi-icon"><i class="la la-download"></i></span>\
                                        <span class="navi-text">Download</span>\
                                    </a>\
                                </li>\
                            </ul>\
                        </div>\
                    </div>';
                    },
                }],
        };
        options.search = {
            
            input: $('#kt_datatable_search_query'),
            key: 'generalSearch'
           
        };
        var datatable = $('#kt_datatable').KTDatatable(options);

        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'status');
        });
    };
    return {
        // public functions
        init: function() {
            localSelectorDemo();
        },
    };
}();

jQuery(document).ready(function () {
    localStorage.clear();
    KTDatatableRecordSelectionDemo.init();
});
function convertToJavaScriptDate(value) {
    var dt = value;
    var hours = dt.getHours();
    var minutes = dt.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return (dt.getMonth() + 1)+ "/" + dt.getDate() + "/" + dt.getFullYear() + " "+ strTime;
}
var initSubDatatable = function (id) {
    var el = $('#kt_datatable_sub');
    var options_sub = {
        data: {
            type: 'local',
            source: Result.find(x => x.RecordId == id)?.invoiceLines,
            pageSize: 5
        },
        // layout definition
        layout: {
            theme: 'default',
            scroll: true,
            footer: false,
        },
        search: {
            input: el.find('#kt_datatable_search_query_2'),
            key: 'generalSearch'
        },

        sortable: true,
        // columns definition
        columns: [
        {
            field: 'itemCode',
            title: 'Item Code'
        },
        {
            field: 'itemType',
            title: 'item Type'
            },
            {
                field: 'quantity',
                title: 'quantity'
            },
            {
                field: 'unitType',
                title: 'unit Type'
            },
            {
                field: 'netTotal',
                title: 'net Total'
            },
            {
                field: 'salesTotal',
                title: 'sales Total',
                type:'money'
            },
            {
                field: 'totalTaxableFees',
                title: 'total Taxable Fees'
            }
        ],
    }
    var datatable = el.KTDatatable(options_sub);
    
    var modal = datatable.closest('.modal');


    $('#kt_datatable_search_type_2').on('change', function () {
        datatable.search($(this).val().toLowerCase(), 'Type');
    });

    // fix datatable layout after modal shown
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
};