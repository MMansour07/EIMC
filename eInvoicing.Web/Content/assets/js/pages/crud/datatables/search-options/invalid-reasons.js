'use strict';
var table = $('#kt_datatable10');
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var fromDate = new Date(y, m, 1);
var toDate = new Date();
var initTable1 = function () {
    KTApp.block('#InvalidReasons_crd');
    // begin first table
    table.DataTable({
        responsive: true,
        dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
			<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,
        buttons: [
            {
                orientation: 'portrait',
                extend: 'print',
                footer: true,
                title: "Invalid Document Reasons from " + $("#fromDate").val() + " to " + $("#toDate").val() + ""
            },
            'copyHtml5',
            {
                orientation: 'portrait',
                pageSize: 'LEGAL',
                extend: 'excel',
                footer: true,
                title: "Invalid Document Reasons from " + $("#fromDate").val() + " to " + $("#toDate").val() + ""
            },
            //{
            //	orientation: 'portrait',
            //	pageSize: 'LEGAL',
            //	extend: 'csv',
            //	footer: true,
            //	title: "Invalid Document Reasons from " + $("#fromDate").val() + " to " + $("#toDate").val() + ""
            //	//customize: function (win) {
            //	//	$body = $(win.document.body);
            //	//	$body.find('h1').css('text-align', 'center');
            //	//}
            //},
            {
                orientation: 'portrait',
                pageSize: 'LEGAL',
                extend: 'pdfHtml5',
                footer: true,
                title: "Invalid Document Reasons from " + $("#fromDate").val() + " to " + $("#toDate").val() + "",
                customize: function (doc) {
                    doc.content[1].layout = "Borders";
                    doc.content.splice(0, 0, {
                        margin: [0, 0, 0, 50],
                        alignment: 'left',
                        image: 'data:image/png;base64,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'
                    });
                }
            }
        ],
        lengthMenu: [10, 15, 25, 50],
        pageLength: 15,
        language: {
            'lengthMenu': 'Display _MENU_',
        },
        searchDelay: 500,
        serverSide: true,
        order: [
            [2, "desc"]
        ],
        ajax: {
            url: '/eimc.hub/v1/report/AjaxInvalidReasons',
            type: 'POST',
            data: function (post) {
                post.fromDate = ModifyDate(fromDate);
                post.toDate = ModifyDate(toDate);
            }
        },
        columns: [
            { data: "DocumentId", "name": "DocumentId" },
            { data: "DateTimeIssued", "name": "DateTimeIssued" },
            { data: "DateTimeReceived", "name": "DateTimeReceived" },
            { data: "ValidationSteps", "name": "ValidationSteps" },
            { data: "Errors", "name": "Errors" },
            { data: "InnerErrors", "name": "InnerErrors" },
            { data: "TotalSalesAmount", "name": "TotalSalesAmount" },
            { data: "NetAmount", "name": "NetAmount" },
            { data: "TotalAmount", "name": "TotalAmount" },
        ],
        "fnDrawCallback": function () {
            KTApp.unblock('#InvalidReasons_crd');
        },
        columnDefs: [
            {
                targets: -7,
                render: function (data, type, full, meta) {
                    var temp = convertToJavaScriptDate(new Date(parseInt(data.substr(6))));
                    return '<span class="navi-text">' + temp + '</span>';
                },
            },
            {
                targets: -8,
                render: function (data, type, full, meta) {
                    var temp = convertToJavaScriptDate(new Date(parseInt(data.substr(6))));
                    return '<span class="navi-text">' + temp + '</span>';
                },
            },
        ],
    });
};
const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];
jQuery(document).ready(function () {
    $("#fromDate").val(((fromDate.getDate() > 9) ? fromDate.getDate() : ('0' + fromDate.getDate())) + '-' + monthNames[((fromDate.getMonth() > 8) ? (fromDate.getMonth()) : ((fromDate.getMonth())))] + '-' + fromDate.getFullYear());
    $("#toDate").val(((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '-' + monthNames[((toDate.getMonth() > 8) ? (toDate.getMonth()) : ((toDate.getMonth())))] + '-' + toDate.getFullYear());
    fromDate = ($("#fromDate").val()) ? $("#fromDate").val() : '';
    toDate = ($("#toDate").val()) ? $("#toDate").val() : '';
    initTable1();
    $("#_find").on('click', function () {
        searchData();
    });
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
    return dt.getDate() + "-" + monthNames[(dt.getMonth())] + "-" + dt.getFullYear() + " " + strTime;
}
function ModifyDate(date) {
    if (date) {
        date = date.toString().split("-");
        // After this construct a string with the above results as below
        //return date[2] + "-" + date[0] + "-" + date[1];
        return date[2] + "-" + ((monthNames.indexOf(date[1]) < 9) ? ("0" + (parseInt(monthNames.indexOf(date[1])) + 1)) : (parseInt(monthNames.indexOf(date[1])) + 1)) + "-" + date[0];
    }
    else
        return null;

}
function searchData() {
    fromDate = ($("#fromDate").val()) ? $("#fromDate").val() : '';
    toDate = ($("#toDate").val()) ? $("#toDate").val() : '';
    table.DataTable().destroy();
    initTable1();
}
