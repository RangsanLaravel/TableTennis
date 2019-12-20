
function setDatePicker(ctlID) {
    $('input[id$=' + ctlID + ']').datepicker({
        format: "dd/mm/yyyy",
        language: "th-th",
        isBuddhist: true,
        todayHighlight: true,
        autoclose: true,
    });
}

function setDatePickerFromTo(ctlFromID, ctlToID) {
    setDatePicker(ctlFromID);
    setDatePicker(ctlToID);
    $('input[id$=' + ctlFromID + ']').on('changeDate', function (selected) {
        if ($(this).val() == '') {
            $('input[id$=' + ctlToID + ']').datepicker('setStartDate', null);
        } else {
            var startDate = new Date(selected.date.valueOf());
            $('input[id$=' + ctlToID + ']').datepicker('setStartDate', startDate);
        }
    }).data('datepicker');

    $('input[id$=' + ctlToID + ']').on('changeDate', function (selected) {
        if ($(this).val() == '') {
            $('input[id$=' + ctlFromID + ']').datepicker('setEndDate', null);
        } else {
            var endDate = new Date(selected.date.valueOf());
            $('input[id$=' + ctlFromID + ']').datepicker('setEndDate', endDate);
        }
    }).data('datepicker');
}

function settooltip() {
    $('[data-original-title]').tooltip();
    $('[title]').tooltip();
}
