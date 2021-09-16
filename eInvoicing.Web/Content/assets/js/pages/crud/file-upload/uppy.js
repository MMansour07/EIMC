"use strict";


// Class definition
var KTUppy = function () {
    const Tus = Uppy.Tus;
    const ProgressBar = Uppy.ProgressBar;
    const StatusBar = Uppy.StatusBar;
    const FileInput = Uppy.FileInput;
    const Informer = Uppy.Informer;
    var IscalledFirst = false;

    // to get uppy companions working, please refer to the official documentation here: https://uppy.io/docs/companion/
    const Dashboard = Uppy.Dashboard;
    const Dropbox = Uppy.Dropbox;
    const GoogleDrive = Uppy.GoogleDrive;
    const Webcam = Uppy.Webcam;

    // Private functions
    var initUppy1 = function () {
        var id = '#kt_uppy_1';
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
                        //$(".uppy-Dashboard-close").click();
                        $.ajax({
                            url: '/master/UploadLicense',
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
    }

    return {
        // public functions
        init: function () {
            initUppy1();
        }
    };
}();

KTUtil.ready(function () {
    KTUppy.init();
});
