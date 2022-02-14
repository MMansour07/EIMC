"use strict";
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var fromDate = new Date(y-2, m, 1);
var toDate = new Date();
var Result;
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
    sortable:   true,
    pagination: true,
    columns: [{
            field: 'status',
            title: 'Status',
            width: 100,
            template: function (row) {
                var status = {
                    submitted: { 'title': 'Submitted', 'class': 'label-light-primary' },
                    approved: { 'title': 'Approved', 'class': ' label-light-success' },
                    rejected: { 'title': 'Rejected', 'class': ' label-light-danger' }
                };
                return '<span class="label label-lg font-weight-bold' + status[row.status.toLowerCase()].class + ' label-inline">' + status[row.status.toLowerCase()].title + '</span>';
            }
        },
        {
            field: 'codeTypeName',
            title: 'Code Type',
            width:50
        },
        {
            field: 'itemCode',
            title: 'Item Code',
            width: 170,
            sortable: false,
            template: function (row) {
                return "<a href='/v1/document/details/' class='btn btn-link no-hover' style='padding-left: 0;text-decoration: underline;'>" + row.itemCode + "</a>";
            },
        },
        {
            field: 'codeNamePrimaryLang',
            title: 'Name (English)/Name (Arabic)',
            width: 200,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.codeNamePrimaryLang   + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.codeNameSecondaryLang +'</span>';
            }
        },
        {
            field: 'descriptionPrimaryLang',
            title: 'Description (English)/Description (Arabic)',
            width: 200,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.descriptionPrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.descriptionSecondaryLang + '</span>';
            }
        },
        {
            field: 'parentItemCode',
            title: 'GPC Linked Item',
            width: 170,
        },
        {
            field: 'parentCodeNamePrimaryLang',
            title: 'GPC Name (English)/GPC Name (Arabic)',
            width: 200,
            template: function (row) {
                return '<span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNamePrimaryLang + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + row.parentCodeNameSecondaryLang + '</span>';
            }
        },
        {
            field: 'activeFrom',
            title: 'Active From',
            template: function (row) {
                var temp = convertToJavaScriptDate(new Date(row.activeFrom)).split(" ");
                return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
            }
        },
        {
            field: 'activeTo',
            title: 'Active To',
            template: function (row) {
                var temp = convertToJavaScriptDate(new Date(row.activeTo)).split(" ");
                return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                        <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
            }
        }]   
};
var localSelectorDemo = function () {

    options.data.source.read.params =
    {
        fromDate: ModifyDate(fromDate),
        toDate: ModifyDate(toDate)
    }
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

    //datatable.on('datatable-on-check datatable-on-uncheck',function (e) {
    //        var checkedNodes = datatable.rows('.datatable-row-active').nodes();
    //        var count = checkedNodes.length; 
    //        $('#kt_datatable_selected_records').html(count);
    //        $('#kt_datatable_fetch_modal').html("Send Top " + count + " <i class='far fa-arrow-alt-circle-right'></i>");

    //        if (count !== options.data.pageSize && count !== 1 && 1 > count > options.data.pageSize) {
    //            $("#kt_datatable_send").hide(); 
    //            $("#kt_datatable_sendAll").hide();
    //        }
    //        else if (count === 1 || count < options.data.pageSize)
    //        {
    //            $("#kt_datatable_sendAll").hide();
    //            $("#kt_datatable_fetch_modal").hide();
    //            $("#kt_datatable_send").show(); 
    //        }
    //        else {
    //            $("#kt_datatable_send").hide(); 
    //            $("#kt_datatable_sendAll").show();
    //            $("#kt_datatable_fetch_modal").show();
    //        }
    //        if (count > 0)
    //        {
    //            $('#kt_datatable_group_action_form').collapse('show');
    //        }
    //        else
    //        {
    //            $('#kt_datatable_group_action_form').collapse('hide');
    //        }
    //});
    //datatable.on('click', '[data-record-id]', function () {
    //    localStorage.clear();
    //    Result = datatable.rows().data().KTDatatable.dataSet.map(o => ({ ...o, dateTimeIssued: new Date(parseInt(o.dateTimeIssued.substr(6))).toISOString() }));
    //    initSubDatatable($(this).data('record-id'));
    //    $('#kt_datatable_modal').modal('show');
    //});
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



