


function swalConfirmDel(title, text, fncYes, fncNo) {
    fncNo = fncNo == null || fncNo == undefined ? function () { } : fncNo;
    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d42424",
        cancelButtonColor: '#b1b1b1',
        confirmButtonText: "OK",
        cancelButtonText: 'No, cancel!',
    }).then(fncYes, fncNo);
}

function swalSuccess(title, text, fncYes) {
    fncYes = fncYes == null || fncYes == undefined ? function () { } : fncYes;
    swal({
        title: title,
        text: text,
        type: "success",
        showCancelButton: false,
        confirmButtonColor: "#3085d6",
        confirmButtonText: "OK",
    }, fncYes);
}

function swalError(title, text, fncYes) {
    fncYes = fncYes == null || fncYes == undefined ? function () { } : fncYes;
    swal({
        title: title,
        text: text,
        type: "error",
        showCancelButton: false,
        confirmButtonColor: "#3085d6",
        confirmButtonText: "OK",

    }, fncYes);
}

function swalWarning(title, text, fncYes) {
    fncYes = fncYes == null || fncYes == undefined ? function () { swal.closeModal() } : fncYes;
    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: false,
        confirmButtonColor: "#3085d6",
        confirmButtonText: "OK",
    }, fncYes);
}

