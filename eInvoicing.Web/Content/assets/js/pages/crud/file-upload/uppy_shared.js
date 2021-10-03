"use strict";
// Class definition
var KTUppy = function () {
    var IscalledFirst = false;
    const Dashboard = Uppy.Dashboard;
    const Dropbox = Uppy.Dropbox;
    const GoogleDrive = Uppy.GoogleDrive;
    const Webcam = Uppy.Webcam;

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
                        $.ajax({
                            url: '/EInvoicing/v0/master/UploadLicense',
                            type: "POST",
                            contentType: false, // Not to set any content header  
                            processData: false, // Not to process data  
                            data: fileData,
                            success: function (result) {

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
                    $.ajax({
                        url: '/EInvoicing/v0/document/UploadFile',
                        type: "POST",
                        contentType: false, // Not to set any content header  
                        processData: false, // Not to process data  
                        data: fileData,
                        success: function (result) {

                            if (result.success) {
                                if (result.IsInserted || result.IsUpdated) {
                                    $(".uppy-Dashboard-Item-action--remove").click();
                                }
                                Swal.fire({
                                    title: result.IsInserted || result.IsUpdated ? 'Data has been saved successfully!' : 'Data already exist!',
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

    return {
        // public functions
        init: function () {
            initUppy6();
            initUppy1();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
});
