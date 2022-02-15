"use strict";
var fromDate;
var toDate;
var active;
var codeName;
var itemCode;
var codeDescription;
var status;

var datatable;

var KTDatatableRecordSelectionDemo = function () {
    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '/v1/code/ajax_SearchMyEGSCodeUsageRequests',
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
            pageSize: 5,
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
            field: 'active',
            title: 'Active',
            sortable: false,
            width: 100,
            template: function (row) {
                var active = {
                    true: { 'title': 'Yes', 'class': 'label-success' },
                    false: { 'title': 'No', 'class': 'label-primary' },
                };
                return '<span class="label label-lg font-weight-bold ' + active[row.active].class + ' label-inline">' + active[row.active].title + '</span>';
            }
        },
        {
            field: 'codeTypeName',
            title: 'Code Type',
            sortable: false,
            width: 100
        },
        {
            field: 'itemCode',
            title: 'Item Code',
            sortable: false,
            width: 220,
            template: function (row) {
                return "<a href='/v1/document/details/' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.itemCode + "</a>";
            },
        },
        {
            field: 'codeNamePrimaryLang',
            title: 'Name (English)/Name (Arabic)',
            sortable: false,
            width: 220,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.codeNamePrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.codeNameSecondaryLang + '</span>';
            }
        },
        {
            field: 'descriptionPrimaryLang',
            title: 'Description (English)/Description (Arabic)',
            sortable: false,
            width: 220,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.descriptionPrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.descriptionSecondaryLang + '</span>';
            }
        },
        {
            field: 'activeFrom',
            title: 'Active From',
            sortable: 'desc',
            template: function (row) {
                var temp = convertToJavaScriptDate(new Date(row.activeFrom)).split(" ");
                return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
            }
        },
        {
            field: 'activeTo',
            title: 'Active To',
            sortable: false,
            template: function (row) {
                if (row.activeTo != null && row.activeTo != "") {
                    var temp = convertToJavaScriptDate(new Date(row.activeTo)).split(" ");
                    return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                }
                else {
                    return '<span class="navi-text" style= "float:left; clear:left;"></span>';
                }
                
            }
        },
        {
            field: 'parentItemCode',
            title: 'GPC Linked Item',
            sortable: false,
            width: 200,
        },
        {
            field: 'parentCodeNamePrimaryLang',
            title: 'GPC Name (English)/GPC Name (Arabic)',
            sortable: false,
            width: 200,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNamePrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNameSecondaryLang + '</span>';
            }
        }]
    };
    var localSelectorDemo = function () {

        options.data.source.read.params =
        {
            fromDate: ModifyDate(fromDate),
            toDate: ModifyDate(toDate),
            active: active,
            codeName: codeName,
            itemCode: itemCode,
            codeDescription: codeDescription,
            status: status
        }
        datatable = $('#kt_datatable2').KTDatatable(options);
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
    KTDatatableRecordSelectionDemo.init();

    $("#_find").on('click', function () {
        searchData();
    });
    $("#_clear").on('click', function () {
        clearFilterationData();
    });
    $("#tab_2").on('click', function () {
        datatable.destroy();
        KTDatatableRecordSelectionDemo2.init();
    });
    $("#tab_1").on('click', function () {
        datatable.destroy();
        KTDatatableRecordSelectionDemo.init();
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
    return dt.getDate() + "-" + monthNames[(dt.getMonth())] + "-" + dt.getFullYear() + " " + strTime;
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
    fromDate = $("#pending_fromDate").val();
    toDate = ($("#pending_toDate").val());
    active = ($("#active").val());
    codeName = ($("#codeName").val());
    itemCode = ($("#itemCode").val());
    codeDescription = ($("#codeDescription").val());
    status = ($("#status").val());

    datatable.destroy();
    if ($("#kt_tab_pane_2").hasClass('active'))
        KTDatatableRecordSelectionDemo2.init();
    else
        KTDatatableRecordSelectionDemo.init();
}

function clearFilterationData() {
    $("#pending_fromDate").val("");
    $("#pending_toDate").val("");
    $("#active").val("")
    $("#codeName").val("");
    $("#itemCode").val("");
    $("#codeDescription").val("");
    $("#status").val("");

    searchData();
}
var KTDatatableRecordSelectionDemo2 = function () {
    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '/v1/code/ajax_SearchMyEGSCodeUsageRequests',
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
            pageSize: 5,
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
        columns: [{
            field: 'status',
            title: 'Status',
            sortable: false,
            width: 100,
            template: function (row) {
                var status = {
                    submitted: { 'title': 'Submitted', 'class': 'label-primary' },
                    approved: { 'title': 'Approved', 'class': 'label-success' },
                    rejected: { 'title': 'Rejected', 'class': 'label-danger' }
                };
                return '<span class="label label-lg font-weight-bold ' + status[row.status.toLowerCase()].class + ' label-inline">' + status[row.status.toLowerCase()].title + '</span>';
            }
        },
        {
            field: 'codeTypeName',
            title: 'Code Type',
            sortable: false,
            width: 100
        },
        {
            field: 'itemCode',
            title: 'Item Code',
            sortable: false,
            width: 220,
            template: function (row) {
                return "<a href='/v1/document/details/' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.itemCode + "</a>";
            },
        },
        {
            field: 'codeNamePrimaryLang',
            title: 'Name (English)/Name (Arabic)',
            sortable: false,
            width: 220,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.codeNamePrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.codeNameSecondaryLang + '</span>';
            }
        },
        {
            field: 'descriptionPrimaryLang',
            title: 'Description (English)/Description (Arabic)',
            sortable: false,
            width: 220,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.descriptionPrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.descriptionSecondaryLang + '</span>';
            }
        },
        {
            field: 'activeFrom',
            title: 'Active From',
            sortable: 'desc',
            template: function (row) {
                var temp = convertToJavaScriptDate(new Date(row.activeFrom)).split(" ");
                return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
            }
        },
        {
            field: 'activeTo',
            title: 'Active To',
            sortable: false,
            template: function (row) {
                if (row.activeTo != null && row.activeTo != "") {
                    var temp = convertToJavaScriptDate(new Date(row.activeTo)).split(" ");
                    return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                }
                else {
                    return '<span class="navi-text" style= "float:left; clear:left;"></span>';
                }

            }
        },
        {
            field: 'parentItemCode',
            title: 'GPC Linked Item',
            sortable: false,
            width: 220
        },
        {
            field: 'parentCodeNamePrimaryLang',
            title: 'GPC Name (English)/GPC Name (Arabic)',
            sortable: false,
            width: 220,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNamePrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNameSecondaryLang + '</span>';
            }
        }]

    };
    var localSelectorDemo = function () {

        options.data.source.read.params =
        {
            fromDate: ModifyDate(fromDate),
            toDate: ModifyDate(toDate),
            active: active,
            codeName: codeName,
            itemCode: itemCode,
            codeDescription: codeDescription,
            status: status
        }
        datatable = $('#kt_datatable').KTDatatable(options);
    };
    return {
        // public functions
        init: function () {
            localStorage.clear();
            localSelectorDemo();
        },
    };
}();


