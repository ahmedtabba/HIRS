
var UploadedFile;
var UploadedFilePath;
function additionalDetail() {
    return {
        filename: window.UploadedFile,
        filePath: window.UploadedFilePath
    }
}
function onSelectFileCustom(e) {
    debugger;
    // Array with information about the uploaded files
    var files = e.files;
    var isValied = true;
    var errorMessagelbl = $(`#${e.sender.name}errorMsgTxt`);
    errorMessagelbl.html('');
    if (files.length > 1) {
        isValied = false;
        alert(OnlyOneFileIsPermited);
        e.preventDefault();
    }
    else {
        if (files[0].size > 5242880) {
            isValied = false;
            //alert('@commonResources.txtFileSizeError');
            errorMessagelbl.append(maxMBErrorMsg)
            e.preventDefault();
        }

        if (files[0].extension == ".exe") {
            isValied = false;
            //alert('@commonResources.txtFileExtensionError');
            errorMessagelbl.append(FileTypeNotPermitedErrorMsg)
            e.preventDefault();
        }
    }
    if (!isValied)
        document.getElementById(`${e.sender.name}errorMessage`).style.display = "block";
    else
        document.getElementById(`${e.sender.name}errorMessage`).style.display = "none";

}
function OnEditEvidence(e) {
    //$(".k-edit-form-container").parent().width(420).data("kendoWindow").center();
    if (!e.model.isNew()) {
        // Disable the editor of the "id" column when editing data items
        e.container.data("kendoWindow").title('Update Attachment');
        e.container.find(".k-button.k-grid-update").text('Save'); //update button
        e.model.dirty = true;
    }
    else {
        e.container.data("kendoWindow").title('Add Attachment');
        e.container.find(".k-button.k-grid-update").text('Add New'); //update button

    }
    e.container.find(".k-button.k-grid-cancel").text('Cancel'); //cancel button

}
function onSuccessUploadFile(e) {
    UploadedFile = getFileInfo(e);
    UploadedFilePath = e.response.filePath;
}




function onSelectFile(e) {
    debugger;
    // Array with information about the uploaded files
    var files = e.files;
    var isValied = true;
    var errorMessagelbl = $("#errorMsgTxt");
    errorMessagelbl.html('');
    if (files.length > 1) {
        isValied = false;
        alert(OnlyOneFileIsPermited);
        e.preventDefault();
    }
    else {
        if (files[0].size > 5242880) {
            isValied = false;
            //alert('@commonResources.txtFileSizeError');
            errorMessagelbl.append(maxMBErrorMsg);
            e.preventDefault();
        }

        if (files[0].extension == ".exe") {
            isValied = false;
            //alert('@commonResources.txtFileExtensionError');
            errorMessagelbl.append(FileTypeNotPermitedErrorMsg);
            e.preventDefault();
        }
    }
    if (!isValied)
        document.getElementById('errorMessage').style.display = "block";
    else
        document.getElementById('errorMessage').style.display = "none";

}



function getFileInfo(e) {
    return $.map(e.files, function (file) {
        var info = file.name;

        return info;
    }).join(", ");
}

function getFilePath(e) {
    return $.map(e.files, function (file) {
        var info = file.path;

        return info;
    }).join(", ");
}

function onUploadFile(e) {
    this.options.async.saveUrl = getResource('FileUploadPath');
}