"use strict";

// Class definition
var KTUppy = function () {
    var IscalledFirst = false;
    const Dashboard = Uppy.Dashboard;
    const Dropbox = Uppy.Dropbox;
    const GoogleDrive = Uppy.GoogleDrive;
    const Webcam = Uppy.Webcam;
    const Tus = Uppy.Tus;
    const ProgressBar = Uppy.ProgressBar;
    const StatusBar = Uppy.StatusBar;
    const FileInput = Uppy.FileInput;
    const Informer = Uppy.Informer;


    // Private functions
    var initUppy1 = function () {
        var id = '#kt_uppy_1';
        /*if ($(id).length) {*/
        var options = {
            proudlyDisplayPoweredByUppy: false,
            target: id + ' .uppy-dashboard',
            inline: false,
            replaceTargetContent: true,
            showProgressDetails: true,
            note: 'Max file size is 1MB and max number of files is 1.',
            height: 470,
            metaFields: [
                { id: 'name', name: 'Name', placeholder: 'file name' },
                { id: 'caption', name: 'Caption', placeholder: 'describe what the image is about' }
            ],
            browserBackButtonClose: true,
            trigger: id + ' .uppy-btn'
        }
        var uppyDashboard = Uppy.Core({
            autoProceed: true,
            restrictions: {
                maxFileSize: 1000000, // 1mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.lic']
            }
        });
        uppyDashboard.on('complete', function (file) {
            if (IscalledFirst) {
                if (window.FormData !== undefined) {
                    // Create FormData object  
                    var fileData = new FormData();
                    $.each(file.successful, function (index, value) {
                        var sizeLabel = "bytes";
                        var filesize = value.size;
                        if (filesize > 1024) {
                            filesize = filesize / 1024;
                            sizeLabel = "kb";
                            if (filesize > 1024) {
                                filesize = filesize / 1024;
                                sizeLabel = "MB";
                            }
                        }
                        fileData.append(value.name, value.data);
                    });
                    // Adding one more key to FormData object  
                    fileData.append('username', 'sa');
                    $(".uppy-Dashboard-close").click();
                    KTApp.blockPage({
                        overlayColor: '#000000',
                        state: 'primary'
                    });
                    $.ajax({
                        url: '/v1/master/UploadLicense',
                        type: "POST",
                        contentType: false, // Not to set any content header  
                        processData: false, // Not to process data  
                        data: fileData,
                        success: function (result) {
                            KTApp.unblockPage();
                            if (result.success) {
                                $(".uppy-Dashboard-Item-action--remove").click();
                                Swal.fire({
                                    title: 'File has been uploaded successfully!',
                                    icon: "success",
                                    buttonsStyling: false,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(function () {
                                    KTUtil.scrollTop();
                                });
                            }
                            else {
                                Swal.fire({
                                    title: "Sorry, something went wrong, please try again.",
                                    text: "Internal Server Error: " + result.message,
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(function () {
                                    KTUtil.scrollTop();
                                });
                            }
                        },
                        error: function (err) {
                            KTApp.unblockPage();
                            alert(err.statusText);
                        }
                    });
                }
                IscalledFirst = false;
            }
            else { IscalledFirst = true; }
        });
        uppyDashboard.use(Dashboard, options);
        uppyDashboard.use(GoogleDrive, { target: Dashboard, companionUrl: 'https://companion.uppy.io' });
        uppyDashboard.use(Dropbox, { target: Dashboard, companionUrl: 'https://companion.uppy.io' });
        uppyDashboard.use(Webcam, { target: Dashboard });
        //}
    }
    var initUppy6 = function () {
       
        var id = '#kt_uppy_6';
        var options = {
            proudlyDisplayPoweredByUppy: false,
            target: id + ' .uppy-dashboard',
            inline: false,
            replaceTargetContent: true,
            showProgressDetails: true,
            note: 'Max file size is 1MB and max number of files is 1.',
            height: 470,
            metaFields: [
                { id: 'name', name: 'Name', placeholder: 'file name' },
                { id: 'caption', name: 'Caption', placeholder: 'describe what the image is about' }
            ],
            browserBackButtonClose: true,
            trigger: id + ' .uppy-btn'
        }
        var uppyDashboard = Uppy.Core({
            autoProceed: true,
            restrictions: {
                maxFileSize: 1000000, // 1mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.xlsx', '.csv']
            }
        });
        uppyDashboard.on('complete', function (file) {
            if (IscalledFirst) {
                if (window.FormData !== undefined) {
                    // Create FormData object  
                    var fileData = new FormData();
                    $.each(file.successful, function (index, value) {
                         
                        var sizeLabel = "bytes";
                        var filesize = value.size;
                        if (filesize > 1024) {
                            filesize = filesize / 1024;
                            sizeLabel = "kb";
                            if (filesize > 1024) {
                                filesize = filesize / 1024;
                                sizeLabel = "MB";
                            }
                        }
                        fileData.append(value.name, value.data);
                    });
                    // Adding one more key to FormData object  
                    fileData.append('username', 'sa');
                    $(".uppy-Dashboard-close").click();
                    KTApp.blockPage({
                        overlayColor: '#000000',
                        state: 'primary'
                    });
                    $.ajax({
                        url: '/v1/document/UploadFile',
                        type: "POST",
                        contentType: false, // Not to set any content header  
                        processData: false, // Not to process data  
                        data: fileData,
                        success: function (result) {
                            KTApp.unblockPage();
                            if (result.success) {
                                if (result.IsInserted || result.IsUpdated) {
                                    $(".uppy-Dashboard-Item-action--remove").click();
                                }
                                Swal.fire({
                                    text: result.IsInserted || result.IsUpdated ? 'Data has been saved successfully!' : 'Data already exist!',
                                    html:
                                        result.IsInserted && result.IsUpdated ?
                                            '<span class="navi-text mb-1" style= "float:left; clear:left;">Documents: [Inserted (' + result.InsertedDocumentsCount + '), Updated (' + result.UpdatedDocumentsCount + ')]</span>\
										 <span class="navi-text" style= "float:left; clear:left;">Lines : [Inserted ('+ result.InsertedInvoiceLinesCount + '), Updated (' + result.UpdatedInvoiceLinesCount + ')]</span>\
									     <span class="navi-text" style= "float:left; clear:left;">Taxable Items : [Inserted ('+ result.InsertedTaxableItemsCount + '), Updated (' + result.UpdatedTaxableItemsCount + ')]</span>' : result.IsInserted ?
                                                '<span class="navi-text mb-1" style= "float:left; clear:left;">Documents: [' + result.InsertedDocumentsCount + ']</span>\
										 <span class="navi-text" style= "float:left; clear:left;">Lines : ['+ result.InsertedInvoiceLinesCount + ']</span>\
									     <span class="navi-text" style= "float:left; clear:left;">Taxable Items : ['+ result.InsertedTaxableItemsCount + ']</span>' : result.IsUpdated ?
                                                    '<span class="navi-text mb-1" style= "float:left; clear:left;">Documents: [' + result.UpdatedDocumentsCount + ']</span>\
										 <span class="navi-text" style= "float:left; clear:left;">Lines : ['+ result.UpdatedInvoiceLinesCount + ']</span>\
									     <span class="navi-text" style= "float:left; clear:left;">Taxable Items : ['+ result.UpdatedTaxableItemsCount + ']</span>' : '',
                                    icon: result.IsInserted || result.IsUpdated ? "success" : "warning",
                                    buttonsStyling: false,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(function () {
                                    KTUtil.scrollTop();
                                });
                            }
                            else {
                                Swal.fire({
                                    title: "Sorry, something went wrong, please try again.",
                                    text: "Internal Server Error: " + result.message,
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(function () {
                                    KTUtil.scrollTop();
                                });
                            }
                        },
                        error: function (err) {
                            KTApp.unblockPage();
                            alert(err.statusText);
                        }
                    });
                }
                IscalledFirst = false;
            }
            else { IscalledFirst = true; }
        });
        uppyDashboard.use(Dashboard, options);
        uppyDashboard.use(GoogleDrive, { target: Dashboard, companionUrl: 'https://companion.uppy.io' });
        uppyDashboard.use(Dropbox, { target: Dashboard, companionUrl: 'https://companion.uppy.io' });
        uppyDashboard.use(Webcam, { target: Dashboard });
    }
    var initUppy5 = function () {
        // Uppy variables
        // For more info refer: https://uppy.io/
        var elemId = 'kt_uppy_5';
        var id = '#' + elemId;
        var $statusBar = $(id + ' .uppy-status');
        var $uploadedList = $(id + ' .uppy-list');
        var timeout;

        var uppyMin = Uppy.Core({
            debug: true,
            autoProceed: true,
            showProgressDetails: true,
            restrictions: {
                maxFileSize: 1000000, // 1mb
                maxNumberOfFiles: 1,
                minNumberOfFiles: 1,
                allowedFileTypes: ['.lic']
            }
        });

        uppyMin.use(FileInput, { target: id + ' .uppy-wrapper', pretty: false });
        uppyMin.use(Informer, { target: id + ' .uppy-informer' });

        // demo file upload server
        //uppyMin.use(Tus, { endpoint: 'https://master.tus.io/files/' });
        uppyMin.use(StatusBar, {
            target: id + ' .uppy-status',
            hideUploadButton: true,
            hideAfterFinish: false
        });

        $(id + ' .uppy-FileInput-input').addClass('uppy-input-control').attr('id', elemId + '_input_control');
        $(id + ' .uppy-FileInput-container').append('<label class="uppy-input-label btn btn-light-primary btn-sm btn-bold" for="' + (elemId + '_input_control') + '">Attach files</label>');

        var $fileLabel = $(id + ' .uppy-input-label');

        uppyMin.on('upload', function (data) {
            $fileLabel.text("Uploading...");
            $statusBar.addClass('uppy-status-ongoing');
            $statusBar.removeClass('uppy-status-hidden');
            clearTimeout(timeout);
        });

        uppyMin.on('complete', function (file) {
            if (window.FormData !== undefined) {
                // Create FormData object  
                var _data = new FormData();
                $.each(file.successful, function (index, value) {
                     
                    var sizeLabel = "bytes";
                    var filesize = value.size;
                    if (filesize > 1024) {
                        filesize = filesize / 1024;
                        sizeLabel = "kb";

                        if (filesize > 1024) {
                            filesize = filesize / 1024;
                            sizeLabel = "MB";
                        }
                    }
                    var uploadListHtml = '<div class="uppy-list-item" data-id="' + value.id + '"><div class="uppy-list-label">' + value.name + ' (' + Math.round(filesize, 2) + ' ' + sizeLabel + ')</div><span class="uppy-list-remove" data-id="' + value.id + '"><i class="flaticon2-cancel-music"></i></span></div>';
                    $uploadedList.append(uploadListHtml);
                    _data.append(value.name, value.data);
                });
                //Adding one more key to FormData object  
                _data.append('username', 'sa');
                $('.uppy-list-remove').on("click", function () { $fileLabel.text("Attach files"); });
                KTApp.blockPage({
                    overlayColor: '#000000',
                    state: 'primary'
                });
                $.ajax({
                    url: '/v1/taxpayer/token',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: _data,
                    success: function (result) {
                        KTApp.unblockPage();
                        if (result.success) {
                            $(".uppy-Dashboard-Item-action--remove").click();
                            Swal.fire({
                                text: 'File has been uploaded successfully!',
                                icon: "success",
                                buttonsStyling: false,
                                confirmButtonText: "Ok, got it!",
                                customClass: {
                                    confirmButton: "btn font-weight-bold btn-light-primary"
                                }
                            }).then(function () {
                                KTUtil.scrollTop();
                            });
                            resetForm();
                        }
                        else {
                            Swal.fire({
                                title: "Sorry, something went wrong, please try again.",
                                text: "Internal Server Error: " + result.message,
                                icon: "error",
                                buttonsStyling: false,
                                confirmButtonText: "Ok, got it!",
                                customClass: {
                                    confirmButton: "btn font-weight-bold btn-light-primary"
                                }
                            }).then(function () {
                                KTUtil.scrollTop();
                            });
                        }
                    },
                    error: function (err) {
                        KTApp.unblockPage();
                        alert(err.statusText);
                    }
                });
            }
            $fileLabel.text("Uploaded!");
            $statusBar.addClass('uppy-status-hidden');
            $statusBar.removeClass('uppy-status-ongoing');
        });

        

        $(document).on('click', id + ' .uppy-list .uppy-list-remove', function () {
            var itemId = $(this).attr('data-id');
            uppyMin.removeFile(itemId);
            $(id + ' .uppy-list-item[data-id="' + itemId + '"').remove();
        });
    }
    return {
        // public functions
        init: function () {
            if (window.location.pathname.split("/")[3]?.toString().toLowerCase() === "taxpayer_details")
            {
                initUppy5();
            }
            initUppy6();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
     
    if (window.location.pathname.split("/")[3]?.toString().toLowerCase() === "taxpayer_details") {
        resetForm();
    }
});
function resetForm()
{
    $.ajax({
        url: "/v1/taxpayer/ajaxtaxpayerdetails",
        type: "get",
        data: {},
        success: function (response) {
            var data = response.data;
            $("#CompanyEn").html(data.TaxPayerNameEn);
            $("#CompanyAr").html(data.TaxPayerNameAr);
            $("#ERP").html(data.ERPName);
            $("#ERPAr").html(data.ERPAr);
            $("#LicenseType").html(data.LicenseType);
            $("#RegistrationNumber").html(data.RegistrationNumber);
            $("#CreatedBy").html(data.CreatedBy);
            $("#CreationDate").html(convertToJavaScriptDate_FileUpload(data.LicenseCreationDate));
            $("#ExpirationDate").html(convertToJavaScriptDate_FileUpload(data.LicenseExpirationDate));
            $("#ClientSecretExpDate").html(convertToJavaScriptDate_FileUpload(data.ClientSecretExpirationDate));
            $("#RegistrationDate").html(convertToJavaScriptDate_FileUpload(data.RegistrationDate));
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
                    window.location.href = "/v1/taxpayer/taxpayer_details";
                }
            });
        }
    });
}



function convertToJavaScriptDate_FileUpload(value) {
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    if (value)
    {
        var dt = new Date(parseInt(value?.substr(6)));
        return dt.getDate() + "-" + monthNames[(dt.getMonth())] + "-" + dt.getFullYear();
    }
    return "";
}