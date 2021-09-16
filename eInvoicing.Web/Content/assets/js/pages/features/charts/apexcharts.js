"use strict";

// Shared Colors Definition
const primary = '#6993FF';
const success = '#1BC5BD';
const info = '#8950FC';
const warning = '#FFA800';
const danger = '#F64E60';

// Class definition
function generateBubbleData(baseval, count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = Math.floor(Math.random() * (750 - 1 + 1)) + 1;;
      var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;
      var z = Math.floor(Math.random() * (75 - 15 + 1)) + 15;
  
      series.push([x, y, z]);
      baseval += 86400000;
      i++;
    }
    return series;
  }

function generateData(count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
        var x = 'w' + (i + 1).toString();
        var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

        series.push({
            x: x,
            y: y
        });
        i++;
    }
    return series;
}

//var KTApexChartsDemo = function () {


//	var _demo12 = function () {
//		const apexChart = "#chart_12";
//		var options = {
//			series: [parseInt($('#SubmittedInvoiceCount').text().trim()), parseInt($('#SubmittedCreditCount').text().trim()), parseInt($('#SubmittedDebitCount').text().trim())],
//			chart: {
//				width: 380,
//				type: 'pie',
//			},
//			labels: ['Invoice', 'Credit Note', 'Debit Note'],
//			responsive: [{
//				breakpoint: 480,
//				options: {
//					chart: {
//						width: 200
//					},
//					legend: {
//						position: 'bottom'
//					}
//				}
//			}],
//			colors: [primary, success, danger]
//		};
//		var chart = new ApexCharts(document.querySelector(apexChart), options);
//		chart.render();
//	}
//	var _demo13 = function () {
//		const apexChart = "#chart_13";
//		var options = {
//			series: [parseInt($('#SubmittedValidDocumentsCount').html()), parseInt($('#SubmittedInValidDocumentsCount').html()), parseInt($('#SubmittedCanceledDocumentsCount').html()), parseInt($('#SubmittedRejectedDocumentsCount').html()), parseInt($('#SubmittedDocumentsCount').html())],
//			chart: {
//				height: 287,
//				type: 'radialBar',
//			},
//			plotOptions: {
//				radialBar: {
//					dataLabels: {
//						name: {
//							fontSize: '22px',
//						},
//						value: {
//							fontSize: '16px',
//						},
//						total: {
//							show: true,
//							label: 'Total',
//							formatter: function (w) {
//								// By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
//								return parseInt($('#SubmittedValidDocumentsCount').html()) + parseInt($('#SubmittedInValidDocumentsCount').html()) + parseInt($('#SubmittedCanceledDocumentsCount').html()) + parseInt($('#SubmittedRejectedDocumentsCount').html()) + parseInt($('#SubmittedDocumentsCount').html());
//							}
//						}
//					}
//				}
//			},
//			labels: ['Valid', 'Invalid', 'Cancelled', 'Rejeted', 'Submitted'],
//			colors: [success, warning, info, danger, primary]
//		};

//		var chart = new ApexCharts(document.querySelector(apexChart), options);
//		chart.render();
//	}

//	var _demo21 = function () {
//		const apexChart = "#chart_21";
//		var options = {
//			series: [parseInt($('#ReceivedInvoiceCount').text()), parseInt($('#ReceivedCreditCount').text()), parseInt($('#ReceivedDebitCount').text())],
//			chart: {
//				width: 380,
//				type: 'pie',
//			},
//			labels: ['Invoice', 'Credit Note', 'Debit Note'],
//			responsive: [{
//				breakpoint: 480,
//				options: {
//					chart: {
//						width: 200
//					},
//					legend: {
//						position: 'bottom'
//					}
//				}
//			}],
//			colors: [primary, success, danger]
//		};

//		var chart = new ApexCharts(document.querySelector(apexChart), options);
//		chart.render();
//	}

//	var _demo31 = function () {
//		const apexChart = "#chart_31";
//		var options = {
//			series: [parseInt($('#ReceivedValidDocumentsCount').html()), parseInt($('#ReceivedInValidDocumentsCount').html()), parseInt($('#ReceivedCanceledDocumentsCount').html()), parseInt($('#ReceivedRejectedDocumentsCount').html()), parseInt($('#ReceivedSubmittedDocumentsCount').html())],
//			chart: {
//				height: 287,
//				type: 'radialBar',
//			},
//			plotOptions: {
//				radialBar: {
//					dataLabels: {
//						name: {
//							fontSize: '22px',
//						},
//						value: {
//							fontSize: '16px',
//						},
//						total: {
//							show: true,
//							label: 'Total',
//							formatter: function (w) {
//								// By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
//								return parseInt($('#ReceivedValidDocumentsCount').html()) + parseInt($('#ReceivedInValidDocumentsCount').html()) + parseInt($('#ReceivedCanceledDocumentsCount').html()) + parseInt($('#ReceivedRejectedDocumentsCount').html()) + parseInt($('#ReceivedSubmittedDocumentsCount').html());
//							}
//						}
//					}
//				}
//			},
//			labels: ['Valid', 'Invalid', 'Cancelled', 'Rejeted', 'Submitted'],
//			colors: [success, warning, info, danger, primary]
//		};

//		var chart = new ApexCharts(document.querySelector(apexChart), options);
//		chart.render();
//	}

//	return {
//		// public functions
//		init: function () {
//			_demo12();
//			_demo13();
//			_demo21();
//			_demo31();


//		}
//	};
//}();

jQuery(document).ready(function () {
	//KTApexChartsDemo.init();
});
