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
			'print',
			'copyHtml5',
			'excelHtml5',
			'csvHtml5',
			'pdfHtml5',
		],
		lengthMenu: [5, 10, 25, 50],
		pageLength: 10,
		language: {
			'lengthMenu': 'Display _MENU_',
		},
		searchDelay: 500,
		serverSide: true,
		order: [
			[2, "desc"]
		],
		ajax: {
			url: '/EInvoicing//v0/report/AjaxTopGoodsUsage',
			type: 'POST',
			data: function (post) {
				post.fromDate = ModifyDate(fromDate);
				post.toDate = ModifyDate(toDate);
			}
		},
		columns: [
			{ data: "itemCode", "name": "itemCode" },
			{ data: "itemDesc", "name": "itemDesc" },
			{ data: "count", "name": "count" },
			{ data: "totalAmount", "name": "totalAmount" },
			{ data: "totalTax", "name": "totalTax" }
		],
		"fnDrawCallback": function () {
			KTApp.unblock('#SubmittedDocumentsStats_crd');
		}
	});
};
jQuery(document).ready(function () {
	$("#fromDate").val(((fromDate.getMonth() > 8) ? (fromDate.getMonth() + 1) : ('0' + (fromDate.getMonth() + 1))) + '/' + ((fromDate.getDate() > 9) ? fromDate.getDate() : ('0' + fromDate.getDate())) + '/' + fromDate.getFullYear());
	$("#toDate").val(((toDate.getMonth() > 8) ? (toDate.getMonth() + 1) : ('0' + (toDate.getMonth() + 1))) + '/' + ((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '/' + toDate.getFullYear());
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
	return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + strTime;
}
function ModifyDate(date) {
	if (date) {
		date = date.toString().split("/");
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
