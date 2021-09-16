"use strict";
// Class definition
var Result;
var datatable;
var KTDatatableRecordSelectionDemo = function () {
    var options = {
        // datasource definition
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'GET',
                    url: '/EInvoicing//v0/document/items/' + (getParameterByName("id") == null ? GetURLParameter() : getParameterByName("id"))
                },
            },
            pageSize: 10,
            saveState: { cookie: false }
        },

        // layout definition
        layout: {
            scroll: true, // enable/disable datatable scroll both horizontal and
            footer: false // display/hide footer
        },

        // column sorting
        sortable: true,
        pagination: true,

        // columns definition
        columns: [
            {
                field: 'internalCode',
                title: 'Internal Code'
            },
            {
                field: 'itemCode',
                title: 'Item Code',
                width: 120,
            },
            {
                field: 'itemType',
                title: 'Item Type',
            },
            {
                field: '',
                title: 'Quantity/Unit Type',
                width: 150,
                // callback function support for column rendering
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + row.quantity + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">Piece</span>';
                }
            },
            {

                field: 'unitValue',
                title: 'Unit Price (EGP)/(Foreign)',
                width: 200,
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + row.unitValue.amountEGP + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + row.unitValue.amountSold+ '</span>';
                }
            },
            {
                field: 'salesTotal',
                title: 'Sales Amount (EGP)/(Foreign)',
                width: 200,
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + row.salesTotal + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">0.000000</span>';
                }
            },
            {
                field: 'totalTax',
                title: 'Total Tax',
                width: 100
            },
            {
                field: 'valueDifference',
                title: 'Value Difference (EGP)'
            },
            {
                field: 'netTotal',
                title: 'Net Total (EGP)/(Foreign)',
                width: 150,
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + row.netTotal + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">0.00000</span>';
                }
            },
            {
                field: 'totalTaxableFees',
                title: 'Total Taxable Fees (EGP)',
                width: 100
            },
            {
                field: 'receiver.name',
                title: 'Total (EGP)/Items Discount',
                width: 150,
                template: function (row) {
                    return '<span class="navi-text" style= "float:left; clear:left;">' + row.total + '</span>\
                            <span class="navi-text" style= "float:left; clear:left;">' + row.itemsDiscount + '</span>';
                }
            },
            {
                field: 'Actions',
                title: 'More',
                width: 100,
                sortable: false,
                overflow: 'visible',
                textAlign: 'left',
                autoHide: false,
                template: function (row) {
                    return '<button data-record-id="' + row.internalId + '" class="btn btn-sm btn-clean" title="View records">\
		                      <i class="flaticon2-document"></i>Taxes</button>\'';
                },
            }],
    };
    // basic demo
    var localSelectorDemo = function () {
        // enable extension
        options.extensions = {
            // boolean or object (extension options)
            checkbox: true,
        };
        options.search = {
            input: $('#kt_datatable_search_query'),
            key: 'generalSearch'
        };
        datatable = $('#kt_datatable').KTDatatable(options);

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();


        datatable.on('click', '[data-record-id]', function () {
            Result = datatable.rows().data().KTDatatable.dataSet.toArray();
            initSubDatatable($(this).data('record-id'));
            $('#kt_datatable_modal').modal('show');
        });
    };
    return {
        // public functions
        init: function () {
            localSelectorDemo();
            //serverSelectorDemo();
        },
    };
}();

jQuery(document).ready(function () {
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
    return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear() + " " + strTime;
}
var initSubDatatable = function (id) {
    var el = $('#kt_datatable_sub');
    var options_sub = {
        data: {
            type: 'local',
            source: Result.find(x => x.internalId == id)?.taxableItems,
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
                field: 'taxType',
                title: 'Tax Type'
            },
            {
                field: 'rate',
                title: 'Rate'
            },
            {
                field: 'amount',
                title: 'Amount (EGP)'
            },
            {
                field: 'subType',
                title: 'Sub Type'
            },
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
function GetURLParameter() {
    var sPageURL = window.location.href;
    var indexOfLastSlash = sPageURL.lastIndexOf("/");

    if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
        return sPageURL.substring(indexOfLastSlash + 1);
    else
        return 0;
}
function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}