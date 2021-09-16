'use strict';
const monthNames = ["January", "February", "March", "April", "May", "June",
	"July", "August", "September", "October", "November", "December"
];

var table = $('#GetMonthlyLowestSeller');
var specificDate = new Date().getFullYear();
var initTable1 = function () {
	KTApp.block('#GetMonthlyLowestSeller_crd');
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
		ajax: {
			url: '/EInvoicing//v0/report/AjaxgetMonthlyLowestSeller',
			type: 'POST',
			data: function (post) {
				post.specificDate = specificDate;
			}
		},
		"fnDrawCallback": function () {
			KTApp.unblock('#GetMonthlyLowestSeller_crd');
		},
		columns: [
			{ data: 'month' },
			{ data: 'itemCode' },
			{ data: 'itemDesc' },
			{ data: 'count' },
			{ data: 'totalAmount' },
			{ data: 'totalTax' }
		],
		columnDefs: [
			{
				targets: -6,
				render: function (data, type, full, meta) {
					return monthNames[data-1];
				},
			}
		],
	});
};
jQuery(document).ready(function () {
	$("#LowestspecificDate").val(new Date().getFullYear());
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
	specificDate = ($("#LowestspecificDate").val()) ? $("#LowestspecificDate").val() : '';
	table.DataTable().destroy();
	initTable1();
}
