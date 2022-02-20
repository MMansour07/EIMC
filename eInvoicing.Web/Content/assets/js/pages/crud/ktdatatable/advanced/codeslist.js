"use strict";
var fromDate;
var toDate;
var active;
var codeName;
var itemCode;
var codeDescription;
var status;
var codeType;

var datatable;

var KTDatatableRecordSelectionDemo = function () {
    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '/efatorty/v1/code/ajax_SearchMyEGSCodeUsageRequests',
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
                return "<a href='/efatorty/v1/document/details/' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.itemCode + "</a>";
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
    $('#add_code').prop('disabled', true);
    $("#searchBtn").children().attr("disabled", "disabled");
    //$('#searchBtn').prop('disabled', true);
    //$("#searchBtn").children().unbind('click');

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

    $("#tab_4").on('click', function () {
        datatable.destroy();
        KTDatatableRecordSelectionDemo3.init();
    });


    $("#searchBtn").on('click', function () {

        if ($("#searchValue").val()?.length > 0) {
            getPublishedCodes();
        }
        else {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": true,
                "progressBar": false,
                "positionClass": "toast-bottom-right",
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
            toastr.warning("You must enter the item code first!");
        }
    
    });
    document.getElementById("searchValue").addEventListener("keyup", myFunction);
    function myFunction() {
        $("#searchValue").removeClass("is-invalid");
        $("#searchValue").removeClass("is-valid");

        if ($("#searchValue").val().length == 0) {
            $("#selectedCodeInfo").css("background-color", "#f9f9f9");
            $('#add_code').prop('disabled', true);
            $("label[id=itemCode]").html("");
            $("label[id=codeName]").html("");
            $("label[id=codeNameAr]").html("");
            $("label[id=description]").html("");
            $("label[id=category]").html("");
            $("label[id=descriptionAR]").html("");
            $("label[id=activity]").html("");
            $("label[id=net_content]").html("");
            $("label[id=brand]").html("");
            $("label[id=net_content_arabic]").html("");
            $("#selectedCodeInfo").css("");
            //getPublishedCodes();
        }
        else {
            $("#searchBtn").children().attr("disabled", "");
        }
    }
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
    codeType = ($("#codeType").val());

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
                    url: '/efatorty/v1/code/ajax_SearchMyEGSCodeUsageRequests',
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
                return "<a href='/efatorty/v1/document/details/' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.itemCode + "</a>";
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

function getPublishedCodes() {
    var btn = KTUtil.getById("spanSearch");
    KTUtil.btnWait(btn, "spinner spinner-left spinner-light-success pl-15");
    $.ajax({
        url: "/efatorty/v1/code/SearchPublishedCodesByKey?searchKey=" + $("#searchKey").val() +"&searchvalue=" + $("#searchValue").val() + "&codeType=" + $("#codeType").val(),
        type: "get", //send it through get method
        data: {},
        success: function (response) {
            if (response.length > 0) {
                $("#searchValue").addClass("is-valid");
                $("label[id=itemCode]").html(response[0].codeLookupValue);
                $("label[id=codeName]").html(response[0].codeNamePrimaryLang);
                $("label[id=codeNameAr]").html(response[0].codeNameSecondaryLang);
                $("label[id=description]").html(response[0].codeDescriptionPrimaryLang);
                $("label[id=category]").html(response[0].CodeCategorization.level1.name + "," + response[0].CodeCategorization.level2.name + "," + response[0].CodeCategorization.level3.name + "," + response[0].CodeCategorization.level4.name);
                $("label[id=descriptionAR]").html(response[0].codeDescriptionSecondaryLang);
                $("label[id=activity]").html(response[0].active ? 'Yes' : 'No' + response[0].activeFrom);
                $("label[id=net_content]").html(response[0].net_content ?? "NA");
                $("label[id=brand]").html(response[0].brand ?? "NA");
                $("label[id=net_content_arabic]").html(response[0].net_content_arabic ?? "NA");
                $("#selectedCodeInfo").css("background-color", "#fff4ec");
                $('#add_code').prop('disabled', false);

            }
            else {
                $("#searchValue").addClass("is-invalid");
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": true,
                    "progressBar": false,
                    "positionClass": "toast-bottom-right",
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
                toastr.info("No results found!");
            }
            KTUtil.btnRelease(btn);
        },
        error: function (xhr) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": true,
                "progressBar": false,
                "positionClass": "toast-bottom-right",
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
    });
}


var KTDatatableRecordSelectionDemo3 = function () {
    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '/efatorty/v1/code/SearchPublishedCodes',
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
                field: 'codeLookupValue',
                title: 'Item Code',
                sortable: false,
                width: 150
            },
            {
                field: 'codeNamePrimaryLang',
                title: 'Code Name (En)',
                sortable: false
            },
            {
                field: 'codeNameSecondaryLang',
                title: 'Code Name (Ar)',
                sortable: false,
            },
            {
                field: 'descriptionPrimaryLang',
                title: 'Description (English)',
                sortable: false,
                width: 220
            }]
    };
    var localSelectorDemo = function () {
        options.data.source.read.params =
        {
            codeName: codeName,
            itemCode: itemCode,
            codeType: codeType??"GS1"
        }
        datatable = $('#kt_datatable3').KTDatatable(options);
    };
    return {
        // public functions
        init: function () {
            localStorage.clear();
            localSelectorDemo();
        },
    };
}();

