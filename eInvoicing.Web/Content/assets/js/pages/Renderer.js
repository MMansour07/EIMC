
jQuery(document).ready(function () {
	var data = { SubmittedInvoiceCount: 0, SubmittedCreditCount: 0, SubmittedDebitCount: 0, ReceivedInvoiceCount: 0, ReceivedCreditCount: 0, ReceivedDebitCount: 0 };
	KTApp.block('#Card4');
	KTApp.block('#Card5');
	KTApp.block('#Card6');
    $.ajax({
        url: "/EInvoicing//v0/master/renderer?_date=" + ModifyDate(new Date()),
        type: "get", //send it through get method
        data: {},
		success: function (response) {
			var data = response.data;
			if (!data) {
				Swal.fire({
					title: "Ooops!",
					text: "Something went wrong. Please check connection and try again later.",
					icon: "error",
					buttonsStyling: false,
					confirmButtonText: "Try Again",
					showCancelButton: true,
					cancelButtonText: "Settings",
					customClass: {
						confirmButton: "btn btn-danger",
						cancelButton: "btn btn-default"
					}
				}).then(function (result) {
					if (result.value) {
						window.location.href = "/EInvoicing//v0";
						KTUtil.scrollTop();
					} else if (result.dismiss === "cancel") {
						window.location.href = "/EInvoicing//v0/appsetting";
					}
				});
			}
			else
			{
				initTable1(data.goodsModel.map(o => [o.itemCode, o.itemDesc, o.count, o.totalAmount, o.totalTax]));
				$("#ReceivedValidDocumentsCount1").html('Monthly Received Valid Documents: (' + data.ReceivedValidDocumentsCount + ')');
				$("#ReceivedValidDocumentsCount2").html(data.ReceivedValidDocumentsCount);
				$("#ReceivedInvoiceTotalAmount").html(data.ReceivedInvoiceTotalAmount + ' EGP');
				$("#ReceivedInvoiceCount").html('('+data.ReceivedInvoiceCount+')');
				$("#ReceivedInvoiceTotalTax").html(data.ReceivedInvoiceTotalTax + ' EGP');
				$("#ReceivedCreditCount").html('(' + data.ReceivedCreditCount + ')');
				$("#ReceivedCreditTotalAmount").html(data.ReceivedCreditTotalAmount + ' EGP');
				$("#ReceivedCreditTotalTax").html(data.ReceivedCreditTotalTax + ' EGP');
				$("#ReceivedDebitCount").html('(' + data.ReceivedDebitCount + ')');
				$("#ReceivedDebitTotalAmount").html(data.ReceivedDebitTotalAmount + ' EGP');
				//$("#ReceivedDebitCount").html(data.ReceivedDebitCount);
				$("#ReceivedDebitTotalTax").html(data.ReceivedDebitTotalTax + ' EGP');
				$("#ReceivedValidDocumentsCountPercentage").css('width', data.ReceivedValidDocumentsCountPercentage + '%');
				$("#ReceivedInValidDocumentsCountPercentage").css('width', data.ReceivedInValidDocumentsCountPercentage + '%');
				$("#ReceivedCanceledDocumentsCountPercentage").css('width', data.ReceivedCanceledDocumentsCountPercentage + '%');
				$("#ReceivedRejectedDocumentsCountPercentage").css('width', data.ReceivedRejectedDocumentsCountPercentage + '%');
				$("#ReceivedSubmittedDocumentsCountPercentage").css('width', data.ReceivedSubmittedDocumentsCountPercentage + '%');
				$("#ReceivedInValidDocumentsCount").html(data.ReceivedInValidDocumentsCount);
				$("#ReceivedCanceledDocumentsCount").html(data.ReceivedCanceledDocumentsCount);
				$("#ReceivedRejectedDocumentsCount").html(data.ReceivedRejectedDocumentsCount);
				$("#ReceivedSubmittedDocumentsCount").html(data.ReceivedSubmittedDocumentsCount);
				KTApp.unblock('#Card4');
				KTApp.unblock('#Card5');
				KTApp.unblock('#Card6');
			}
            
        },
        error: function (xhr) {
            //Do Something to handle error
			Swal.fire({
				title: "Ooops!",
				text: "Something went wrong. Please check connection and try again later.",
				icon: "error",
				buttonsStyling: false,
				confirmButtonText: "Try Again",
				showCancelButton: true,
				cancelButtonText: "Settings",
				customClass: {
					confirmButton: "btn btn-danger",
					cancelButton: "btn btn-default"
				}
			}).then(function (result) {
				if (result.value) {
					window.location.href = "/EInvoicing//v0";
					KTUtil.scrollTop();
				} else if (result.dismiss === "cancel") {
					window.location.href = "/EInvoicing//v0/appsetting";
				}
			});
        }
    });

	$("#kt_datepicker_2").on('change', function () {
        KTApp.block('#Card4');
        KTApp.block('#Card5');
        KTApp.block('#Card6');
        $.ajax({
            url: "/EInvoicing//v0/master/renderer?_date=" + ModifyDate(new Date($("#kt_datepicker_2").val())),
            type: "get", //send it through get method
            data: {},
			success: function (response) {
				var data = response.data;
				initTable1(data.goodsModel.map(o => [o.itemCode, o.itemDesc, o.count, o.totalAmount, o.totalTax]));
				$("#ReceivedValidDocumentsCount1").html('Monthly Received Valid Documents: ' + data.ReceivedValidDocumentsCount);
				$("#ReceivedValidDocumentsCount2").html(data.ReceivedValidDocumentsCount);
				$("#ReceivedInvoiceTotalAmount").html(data.ReceivedInvoiceTotalAmount + ' EGP');
				$("#ReceivedInvoiceCount").html('(' + data.ReceivedInvoiceCount + ')');
				$("#ReceivedInvoiceTotalTax").html(data.ReceivedInvoiceTotalTax + ' EGP');
				$("#ReceivedCreditCount").html('(' + data.ReceivedCreditCount + ')');
				$("#ReceivedCreditTotalAmount").html(data.ReceivedCreditTotalAmount + ' EGP');
				$("#ReceivedCreditTotalTax").html(data.ReceivedCreditTotalTax + ' EGP');
				$("#ReceivedDebitCount").html('(' + data.ReceivedDebitCount + ')');
				$("#ReceivedDebitTotalAmount").html(data.ReceivedDebitTotalAmount + ' EGP');
				$("#ReceivedDebitTotalTax").html(data.ReceivedDebitTotalTax + ' EGP');
				$("#ReceivedValidDocumentsCountPercentage").css('width', data.ReceivedValidDocumentsCountPercentage + '%');
				$("#ReceivedInValidDocumentsCountPercentage").css('width', data.ReceivedInValidDocumentsCountPercentage + '%');
				$("#ReceivedCanceledDocumentsCountPercentage").css('width', data.ReceivedCanceledDocumentsCountPercentage + '%');
				$("#ReceivedRejectedDocumentsCountPercentage").css('width', data.ReceivedRejectedDocumentsCountPercentage + '%');
				$("#ReceivedSubmittedDocumentsCountPercentage").css('width', data.ReceivedSubmittedDocumentsCountPercentage + '%');
				$("#ReceivedInValidDocumentsCount").html(data.ReceivedInValidDocumentsCount);
				$("#ReceivedCanceledDocumentsCount").html(data.ReceivedCanceledDocumentsCount);
				$("#ReceivedRejectedDocumentsCount").html(data.ReceivedRejectedDocumentsCount);
				$("#ReceivedSubmittedDocumentsCount").html(data.ReceivedSubmittedDocumentsCount);
				KTApp.unblock('#Card4');
				KTApp.unblock('#Card5');
				KTApp.unblock('#Card6');
			},
			error: function (xhr) {
				//Do Something to handle error
				Swal.fire({
					title: "Ooops!",
					text: "Something went wrong. Please check connection and try again later.",
					icon: "error",
					buttonsStyling: false,
					confirmButtonText: "Try Again",
					showCancelButton: true,
					cancelButtonText: "Settings",
					customClass: {
						confirmButton: "btn btn-danger",
						cancelButton: "btn btn-default"
					}
				}).then(function (result) {
					if (result.value) {
						window.location.href = "/EInvoicing//v0";
						KTUtil.scrollTop();
					} else if (result.dismiss === "cancel") {
						window.location.href = "/EInvoicing//v0/appsetting";
					}
				});
			}
        });
    });
});
function ModifyDate(date) {
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year

    // After this construct a string with the above results as below
    return year + "-" + month + "-" + day;
}
var KTApexChartsDemo = function (data) {


	//var _demo12 = function (data) {
	//	const apexChart = "#chart_12";
	//	var options = {
	//		series: [data?.SubmittedInvoiceCount, data?.SubmittedCreditCount, data?.SubmittedDebitCount],
	//		chart: {
	//			width: 380,
	//			type: 'pie',
	//		},
	//		labels: ['Invoice', 'Credit Note', 'Debit Note'],
	//		responsive: [{
	//			breakpoint: 480,
	//			options: {
	//				chart: {
	//					width: 200
	//				},
	//				legend: {
	//					position: 'bottom'
	//				}
	//			}
	//		}],
	//		colors: [primary, success, danger]
	//	};
	//	var chart = new ApexCharts(document.querySelector(apexChart), options);
	//	chart.render();
	//}
	//var _demo13 = function (data) {
	//	const apexChart = "#chart_13";
	//	var options = {
	//		series: [data?.SubmittedValidDocumentsCountPercentage, data?.SubmittedInValidDocumentsCountPercentage, data?.SubmittedCanceledDocumentsCountPercentage, data?.SubmittedRejectedDocumentsCountPercentage, data?.SubmittedDocumentsCountPercentage],
	//		chart: {
	//			height: 287,
	//			type: 'radialBar',
	//		},
	//		plotOptions: {
	//			radialBar: {
	//				dataLabels: {
	//					name: {
	//						fontSize: '22px',
	//					},
	//					value: {
	//						fontSize: '16px',
	//					},
	//					total: {
	//						show: true,
	//						label: 'Total',
	//						formatter: function (w) {
	//							// By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
	//							return data?.SubmittedValidDocumentsCount + data?.SubmittedInValidDocumentsCount + data?.SubmittedCanceledDocumentsCount + data?.SubmittedRejectedDocumentsCount + data?.SubmittedDocumentsCount;
	//						}
	//					}
	//				}
	//			}
	//		},
	//		labels: ['Valid', 'Invalid', 'Cancelled', 'Rejeted', 'Submitted'],
	//		colors: [success, warning, info, danger, primary]
	//	};

	//	var chart = new ApexCharts(document.querySelector(apexChart), options);
	//	chart.render();
	//}

	//var _demo21 = function (data) {
	//	const apexChart = "#chart_21";
	//	var options = {
	//		series: [data?.ReceivedInvoiceCount, data?.ReceivedCreditCount, data?.ReceivedDebitCount],
	//		chart: {
	//			width: 380,
	//			type: 'pie',
	//		},
	//		labels: ['Invoice', 'Credit Note', 'Debit Note'],
	//		responsive: [{
	//			breakpoint: 480,
	//			options: {
	//				chart: {
	//					width: 200
	//				},
	//				legend: {
	//					position: 'bottom'
	//				}
	//			}
	//		}],
	//		colors: [primary, success, danger]
	//	};

	//	var chart = new ApexCharts(document.querySelector(apexChart), options);
	//	chart.render();
	//}

	//var _demo31 = function (data) {
	//	const apexChart = "#chart_31";
	//	var options = {
	//		series: [data?.ReceivedValidDocumentsCountPercentage, data?.ReceivedInValidDocumentsCountPercentage, data?.ReceivedCanceledDocumentsCountPercentage, data?.ReceivedRejectedDocumentsCountPercentage, data?.ReceivedSubmittedDocumentsCountPercentage],
	//		chart: {
	//			height: 287,
	//			type: 'radialBar',
	//		},
	//		plotOptions: {
	//			radialBar: {
	//				dataLabels: {
	//					name: {
	//						fontSize: '22px',
	//					},
	//					value: {
	//						fontSize: '16px',
	//					},
	//					total: {
	//						show: true,
	//						label: 'Total',
	//						formatter: function (w) {
	//							// By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
	//							return data?.ReceivedValidDocumentsCount + data?.ReceivedInValidDocumentsCount + data?.ReceivedCanceledDocumentsCount + data?.ReceivedRejectedDocumentsCount + data?.ReceivedSubmittedDocumentsCount;
	//						}
	//					}
	//				}
	//			}
	//		},
	//		labels: ['Valid', 'Invalid', 'Cancelled', 'Rejeted', 'Received'],
	//		colors: [success, warning, info, danger, primary]
	//	};

	//	var chart = new ApexCharts(document.querySelector(apexChart), options);
	//	chart.render();
	//}

	return {
		// public functions
		init: function (data) {
			//_demo12(data);
			//_demo13(data);
			//_demo21(data);
			//_demo31(data);
		}
	};
}();
var initTable1 = function (source) {
	var table = $('#kt_datatable');
	table.DataTable().destroy();
	table.DataTable({
		responsive: true,
		lengthMenu: [10, 25, 50, 100],
		pageLength: 10,
		language: {
			'lengthMenu': 'Display _MENU_',
		},
		data: source,
		order: [[2, 'desc']],
		autoWidth: true,
		autoFill: true,
		columnDefs: [
			{
				targets: -5,
				width: 200
			},
			{
				targets: -4,
				width: 200
			},
			{
				targets: -3,
				width: 50
			},
			{
				targets: -2,
				width: 50
			},
			{
				targets: -4,
				width: 50
			}
		],
	});
};