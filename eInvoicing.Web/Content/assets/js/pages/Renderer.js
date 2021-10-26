
jQuery(document).ready(function () {
	var data = { SubmittedInvoiceCount: 0, SubmittedCreditCount: 0, SubmittedDebitCount: 0, ReceivedInvoiceCount: 0, ReceivedCreditCount: 0, ReceivedDebitCount: 0 };
	KTApp.block('#Card4');
	KTApp.block('#Card5');
	KTApp.block('#Card6');
    $.ajax({
        url: "/v0/master/renderer?_date=" + ModifyDate(new Date()),
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
						window.location.href = "/ETA.Hub/einvoicing/v0";
						KTUtil.scrollTop();
					} else if (result.dismiss === "cancel") {
						window.location.href = "/v0/appsetting";
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
					window.location.href = "/ETA.Hub/einvoicing/v0";
					KTUtil.scrollTop();
				} else if (result.dismiss === "cancel") {
					window.location.href = "/v0/appsetting";
				}
			});
        }
    });

	$("#kt_datepicker_2").on('change', function () {
        KTApp.block('#Card4');
        KTApp.block('#Card5');
        KTApp.block('#Card6');
        $.ajax({
            url: "/v0/master/renderer?_date=" + ModifyDate(new Date($("#kt_datepicker_2").val())),
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
						window.location.href = "/ETA.Hub/einvoicing/v0";
						KTUtil.scrollTop();
					} else if (result.dismiss === "cancel") {
						window.location.href = "/v0/appsetting";
					}
				});
			}
        });
    });
});
function ModifyDate(date) {
    var day = date.getDate();       
    var month = date.getMonth() + 1;  
    var year = date.getFullYear();

    // After this construct a string with the above results as below
    return year + "-" + month + "-" + day;
}
var KTApexChartsDemo = function (data) {
	return {
		// public functions
		init: function (data) {
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