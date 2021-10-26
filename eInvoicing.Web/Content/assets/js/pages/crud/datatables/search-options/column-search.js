'use strict';
var table = $('#kt_datatable10');
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var fromDate = new Date(y, m, 1);
var toDate = new Date();
var initTable1 = function () {
	KTApp.block('#SubmittedDocumentsStats_crd');
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
				title: "Documents Statistics from " + $("#fromDate").val() + " to " + $("#toDate").val()+ ""
			},
			'copyHtml5',
			{
				orientation: 'portrait',
				pageSize: 'LEGAL',
				extend: 'excel',
				footer: true,
				title: "Documents Statistics from " + $("#fromDate").val() + " to " + $("#toDate").val() + ""
			},
			//{
			//	orientation: 'portrait',
			//	pageSize: 'LEGAL',
			//	extend: 'csv',
			//	footer: true,
			//	title: "Documents Statistics from " + $("#fromDate").val() + " to " + $("#toDate").val() + ""
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
				title: "Documents Statistics from " + $("#fromDate").val() + " to " + $("#toDate").val() + "",
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
		lengthMenu: [5, 10, 25, 50],
		pageLength: 30,
		language: {
			'lengthMenu': 'Display _MENU_',
		},
		searchDelay: 500,
		serverSide: true,
		order: [
			[7, "desc"]
		],
		ajax: {
			url: '/v0/report/AjaxGetSubmittedDocumentsStats',
			type: 'POST',
			data: function (post) {
				post.fromDate = ModifyDate(fromDate);
				post.toDate = ModifyDate(toDate);
			}
		},
		columns: [
			{ data: "issuedOn", "name": "IssuedOn" },
			{ data: "totalCount", "name": "TotalCount" },
			{ data: "validCount", "name": "ValidCount" },
			{ data: "invalidCount", "name": "InvalidCount" },
			{ data: "submittedCount", "name": "SubmittedCount" },
			{ data: "cancelledCount", "name": "CancelledCount" },
			{ data: "rejectedCount", "name": "RejectedCount" },
			{ data: "submittedOn", "name": "SubmittedOn" },
			{ data: "submittedBy", "name": "SubmittedBy" }

		],
		"fnDrawCallback": function () {
			KTApp.unblock('#SubmittedDocumentsStats_crd');
		},
		"footerCallback": function (row, data, start, end, display) {
			var api = this.api(), data;

			// Remove the formatting to get integer data for summation
			var intVal = function (i) {
				return typeof i === 'string' ?
					i.replace(/[\$,]/g, '') * 1 :
					typeof i === 'number' ?
						i : 0;
			};

			// Total over all pages
			var total = api
				.column(1)
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);

			// Total over this page
			var pageTotal1 = api
				.column(1, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);

			var pageTotal2 = api
				.column(2, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);
			var pageTotal3 = api
				.column(3, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);
			var pageTotal4 = api
				.column(4, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);
			var pageTotal5 = api
				.column(5, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);
			var pageTotal6 = api
				.column(6, { page: 'current' })
				.data()
				.reduce(function (a, b) {
					return intVal(a) + intVal(b);
				}, 0);

			// Update footer
			$(api.column(1).footer()).html(
				pageTotal1 
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(2).footer()).html(
				pageTotal2
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(3).footer()).html(
				pageTotal3
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(4).footer()).html(
				pageTotal4
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(5).footer()).html(
				pageTotal5
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(6).footer()).html(
				pageTotal6
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
		},
		columnDefs: [
			{
				targets: -9,
				render: function (data, type, full, meta) {
					var temp = convertToJavaScriptDate(new Date(parseInt(data.substr(6)))).split(" ");
					return '<span class="navi-text">' + temp[0] + '</span>';
				},
			},
			{
				targets: -2,
				render: function (data, type, full, meta) {
					var temp = convertToJavaScriptDate(new Date(parseInt(data.substr(6)))).split(" ");
					return '<span class="navi-text">' + temp[0] + '</span>';
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
		return date[2] + "-" + date[0] + "-" + date[1];
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
