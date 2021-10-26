"use strict";

// Class definition
// Base elements
var _wizardEl;
var _formEl;
var _wizard;
var _validations = [];
var LineValidations = [];
var counter = 0;
var Lines = [];
var TaxableItems = [];
var editMode = false;
var lineEditMode = false;
var rowIndex;
var lineRowIndex;
var _linefrmvalidation;
var _taxfrmvalidation;
var amount = 0;

var KTWizard4 = function () {
	
	// Private functions
	var initWizard = function () {
		// Initialize form wizard
		_wizard = new KTWizard(_wizardEl, {
			startStep: 1, // initial active step number
			clickableSteps: true  // allow step clicking
		});

		// Validation before going to next page
		_wizard.on('beforeNext', function (wizard) {
			if (wizard.getStep() == 3) {
				if (Lines.length === 0)
				{
					_wizard.stop();
					swal.fire({
						text: "please fill the required fields before going to the next step.",
						icon: "warning",
						buttonsstyling: false,
						confirmbuttontext: "ok, got it!",
						customclass: {
							confirmbutton: "btn font-weight-bold btn-light"
						}
					}).then(function () {
						ktutil.scrolltop();
					});
					return;
				}
            }
			// Don't go to the next step yet
				_wizard.stop();
			// Validate form
				var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step
				if (validator) {
					validator.validate().then(function (status) {
						if (status == 'Valid') {
							_wizard.goNext();
							KTUtil.scrollTop();
						}
						else {
							swal.fire({
								text: "please fill the required fields before going to the next step.",
								icon: "warning",
								buttonsstyling: false,
								confirmbuttontext: "ok, got it!",
								customclass: {
									confirmbutton: "btn font-weight-bold btn-light"
								}
							}).then(function () {
								ktutil.scrolltop();
							});
						}
					});
				}
				else
				{
					_wizard.goNext();
					KTUtil.scrollTop();
				}
		});

		// Change event
		_wizard.on('change', function (wizard) {
			KTUtil.scrollTop();
		});
	}

	var initValidation = function () {
		// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
		// Step 1
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					DocumentType: {
						validators: {
							notEmpty: {
								message: 'Document Type is required'
							}
						}
					},
					DocumentTypeVersion: {
						validators: {
							notEmpty: {
								message: 'Document Type Version is required'
							}
						}
					},
					//InternalId: {
					//	validators: {
					//		notEmpty: {
					//			message: 'Internal Id is required'
					//		}
					//	}
					//}
				},
				
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		));

		// Step 2
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
                fields: {
                    TaxpayerActivityCode: {
                        validators: {
                            notEmpty: {
                                message: 'Taxpayer Activity Code is required'
                            }
                        }
                    },
                    IssuerCountry: {
						validators: {
							notEmpty: {
								message: 'Issuer Country is required'
							}
						}
                    },
                    IssuerGovernorate: {
                        validators: {
                            notEmpty: {
                                message: 'Issuer Governorate is required'
                            }
                        }
                    },
                    IssuerBuildingNumber: {
                        validators: {
                            notEmpty: {
                                message: 'Issuer Building Number is required'
                            }
                        }
                    },
                    IssuerRegionCity: {
                        validators: {
                            notEmpty: {
                                message: 'Issuer City is required'
                            }
                        }
                    },
                    IssuerFloor: {
                        validators: {
                            notEmpty: {
                                message: 'Issuer Floor is required'
                            }
                        }
					},
					IssuerStreet: {
						validators: {
							notEmpty: {
								message: 'Issuer Street is required'
							}
						}
					},
					ReceiverType: {
						validators: {
							notEmpty: {
								message: 'Receiver Type is required'
							}
						}
					},
					ReceiverName: {
						validators: {
							notEmpty: {
								message: 'Receiver Name is required'
							}
						}
					},
					RGN: {
						validators: {
							notEmpty: {
								message: 'Rgisteration Number is required'
							}
						}
					},
					//NID: {
					//	validators: {
					//		notEmpty: {
					//			message: 'National Id is required'
					//		}
					//	}
					//},
					//PID: {
					//	validators: {
					//		notEmpty: {
					//			message: 'Id is required'
					//		}
					//	}
					//},
					ReceiverCountry: {
						validators: {
							notEmpty: {
								message: 'Receiver Country is required'
							}
						}
					},
					ReceiverGovernorate: {
						validators: {
							notEmpty: {
								message: 'Receiver Governorate is required'
							}
						}
					},
					ReceiverBuildingNumber: {
						validators: {
							notEmpty: {
								message: 'Receiver Building Number is required'
							}
						}
					},
					ReceiverRegionCity: {
						validators: {
							notEmpty: {
								message: 'Receiver City is required'
							}
						}
					},
					ReceiverFloor: {
						validators: {
							notEmpty: {
								message: 'Receiver Floor is required'
							}
						}
					},
					ReceiverStreet: {
						validators: {
							notEmpty: {
								message: 'Receiver Street is required'
							}
						}
					},
				},
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
			}
		));

	 _linefrmvalidation = FormValidation.formValidation(
			document.getElementById('line_frm'),
			{
				fields: {
					ItemCode: {
						validators: {
							notEmpty: {
								message: 'Item Code is required'
							}
						}
					},
					Quantity: {
						validators: {
							notEmpty: {
								message: 'Quantity is required'
							}
						}
					},
					UnitType: {
						validators: {
							notEmpty: {
								message: 'Unit Type is required'
							}
						}
					},
					Description: {
						validators: {
							notEmpty: {
								message: 'Description is required'
							}
						}
					},
					AmountEGP: {
						validators: {
							notEmpty: {
								message: ' Unit Price is required'
							}
						}
					},
					CurrencySold: {
						validators: {
							notEmpty: {
								message: 'Currency is required'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap(),
				}
		});

	 _taxfrmvalidation = FormValidation.formValidation(
			document.getElementById('tax_frm'),
			{
				fields: {
					TaxType: {
						validators: {
							notEmpty: {
								message: 'Tax Type is required'
							}
						}
					},
					SubType: {
						validators: {
							notEmpty: {
								message: 'Sub Type is required'
							}
						}
					},
					Rate: {
						validators: {
							notEmpty: {
								message: 'Rate is required'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap(),
				}
			});
	}

	return {
		// public functions
		init: function () {
			_wizardEl = KTUtil.getById('kt_wizard_v4');
			_formEl = KTUtil.getById('kt_form');
			initWizard();
			initValidation();
		}
	};
}();

jQuery(document).ready(function () {
	$("#RGN").hide();
	$("#NID").hide();
	$("#PID").hide();
	KTWizard4.init();
	$("#doc_submit").click(function (e) {
		KTApp.blockPage();
		 
		//var valdata = $("#kt_form").serialize();
		//var InvoiceLines = Lines;
		var obj = { Document: "", InvoiceLines: "" }
		var urlParam = JSON.stringify($("#kt_form").serialize()); 
		var jObject = JSON.parse('{"' + decodeURI(urlParam).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}')
		obj.Document = jObject;
		obj.InvoiceLines = Lines;
		 
		$.ajax({
			url: "/v0/document/ajax_new_document",
			type: "POST",
			dataType: 'json',
			contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
			data: obj,
			success: function (response) {
				 
				KTApp.unblockPage();
                if (response.status.toLowerCase() == "success") {
                    KTUtil.scrollTop();
                    toastr.options = {
                        "closeButton": false,
                        "debug": false,
                        "newestOnTop": true,
                        "progressBar": false,
                        "positionClass": "toast-top-center",
                        "preventDuplicates": false,
                        "onclick": null,
                        "showDuration": "300",
                        "hideDuration": "1000",
                        "timeOut": "5000",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    };
                    toastr.success("Data has been saved successfully!");
                }
                else {
                    toastr.options = {
                        "closeButton": false,
                        "debug": false,
                        "newestOnTop": true,
                        "progressBar": false,
                        "positionClass": "toast-top-center",
                        "preventDuplicates": false,
                        "onclick": null,
                        "showDuration": "300",
                        "hideDuration": "1000",
                        "timeOut": "5000",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    };
                    toastr.error("Something went wrong!");
                }
			}
		});
	});
});

function checkwhenCallBack()
{
	 
	if (counter == parseInt($(".outer").length)) {
		_wizard.goNext();
		KTUtil.scrollTop();
	}
	else {
		swal.fire({
			text: "Please fill the required fields before going to the next step.",
			icon: "warning",
			buttonsstyling: false,
			confirmbuttontext: "ok, got it!",
			customclass: {
				confirmbutton: "btn font-weight-bold btn-light"
			}
		}).then(function () {
			ktutil.scrolltop();
		});
	}
}

function getLines() {
	 
	var Lines = [];
	var len = $(".outer").length;
	//var innerlen = $(".inner").length;
	for (var i = 0; i <= len; i+=2) {
		var innerlen = $($(".outer")[i]).children().find('.inner').length;
		var TaxableItems = [];
		for (var j = 1; j <= innerlen; j+=2) {
			Tax.TaxType = $("select[name='[" + j + "][TaxType]']").val();
			Tax.SubType = $("select[name='[" + j + "][SubType]']").val();
			Tax.Rate = $("input[name='[" + j + "][Rate]']").val();
			Tax.Amount = $("input[name='[" + j + "][Amount]']").val();
			TaxableItems.push(Tax);
		}
		Line.ItemCode = $("input[name='[" + i + "][ItemCode]']").val();
		Line.InternalCode = $("input[name='[" + i + "][InternalCode]']").val();
		Line.InternalId = $("input[name='[" + i + "][InternalId]']").val();
		Line.UnitType = $("select[name='[" + i + "][UnitType]']").val();
		//Line.ItemType = $("input[name='[" + i + "][ItemType]']").val();
		Line.Quantity = $("input[name='[" + i + "][Quantity]']").val();
		Line.SalesTotal = $("input[name='[" + i + "][SalesTotal]']").val();
		Line.Total = $("input[name='[" + i + "][Total]']").val();
		Line.ValueDifference = $("input[name='[" + i + "][ValueDifference]']").val();
		Line.TotalTaxableFees = $("input[name='[" + i + "][TotalTaxableFees]']").val();
		Line.TotalTax = $("input[name='[" + i + "][TotalTax]']").val();
		Line.NetTotal = $("input[name='[" + i +"][NetTotal]']").val();
		Line.ItemsDiscount = $("input[name='[" + i + "][ItemsDiscount]']").val();
		Line.Description = $("textarea[name='[0][Description]']").val();
		Line.CurrencySold = $("select[name='[" + i + "][CurrencySold]']").val();
		Line.AmountEGP = $("input[name='[" + i + "][AmountEGP]']").val();
		Line.AmountSold = $("input[name='[" + i + "][AmountSold]']").val();
		Line.CurrencyExchangeRate = $("input[name='[" + i + "][CurrencyExchangeRate]']").val();
		Line.Rate = $("input[name='[" + i + "][Rate]']").val();
		Line.Amount = $("input[name='[" + i + "][Amount]']").val();
		Line.TaxableItems = TaxableItems;
		Lines.push(Line);
	}
	return Lines;
}