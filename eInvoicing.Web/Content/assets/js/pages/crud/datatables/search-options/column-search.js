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
				title: "Submitted Document Stats from date [ " + $("#fromDate").val() + " ] to date [ " + $("#toDate").val() + " ]"
			},
			'copyHtml5',
			{
				orientation: 'portrait',
				pageSize: 'LEGAL',
				extend: 'excel',
				footer: true,
				title: "Submitted Document Stats from date [ " + $("#fromDate").val() + " ] to date [ " + $("#toDate").val() + " ]"
			},
			//{
			//	orientation: 'portrait',
			//	pageSize: 'LEGAL',
			//	extend: 'csv',
			//	footer: true,
			//	title: "Submitted Document Stats from date [ " + $("#fromDate").val() + " ] to date [ " + $("#toDate").val() + " ]",
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
				title: "Submitted Document from [ " + $("#fromDate").val() + " ] to [ " + $("#toDate").val() + " ]",
				customize: function (doc) {
					doc.content[1].layout = "Borders";
					//$body = $(win.document.body);
					//$body.find('h1').css('text-align', 'center');
				}
			}
		],
		lengthMenu: [5, 10, 25, 50],
		pageLength: 10,
		language: {
			'lengthMenu': 'Display _MENU_',
		},
		searchDelay: 500,
		serverSide: true,
		order: [
			[7, "desc"]
		],
		ajax: {
			url: '/EInvoicing//v0/report/AjaxGetSubmittedDocumentsStats',
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
				'[' + pageTotal1 + ']'
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(2).footer()).html(
				'[' + pageTotal2 + ']'
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(3).footer()).html(
				'[' + pageTotal3 + ']'
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(4).footer()).html(
				'[' + pageTotal4 + ']'
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(5).footer()).html(
				'[' + pageTotal5 + ']'
				//'$' + pageTotal + ' ( $' + total + ' total)'
			);
			$(api.column(6).footer()).html(
				'[' + pageTotal6 + ']'
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
