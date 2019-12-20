
var urlTour = {};
$(document).ready(function () {
    getUrl();
    $('#player_code').change(function () {
        $('#name').val('');
        $('#gender').val('');
        $('#birth_date').val('');
        $('#age').val('');
        getPlayer();
    });

    $('#addPlay').on('click', function () {

        addTPlayer();
        $('#name').val('');
        $('#gender').val('');
        $('#birth_date').val('');
        $('#age').val('');
        $('#player_code').val('');
    });

});

function getUrl() {
    $.ajax({
        type: 'GET',
        url: '/TableTennis/Tournament/UrlTour',
        success: function (res) {
            debugger
            urlTour = res;
            getTourRegister();
        }
    })
}

function addTPlayer() {

    var obj = {
        TOUR_MAP_ID: $('#even :selected').val()
        , PLAYER_ID: $('#player_code').val()
        , MANAGER_ID: $('#manager_id').val()
        , CREATE_BY: $('#manager_code').val()
        , CATEGORY_ID: $('#category_id').val()
    };

    $.ajax({
        type: 'POST',
        url: urlTour.AddTourRegister,
        data: { dataObj: obj },
        success: function (response) {
            if (response.result.Successed)
                getTourRegister();
            else
                alert(response.result.ErrorMassage);
        }
    });
}

function getTourRegister() {
    $('.divLoading').show();
    $('#conTable table').remove();
    var managerId = $('#manager_id').val();
    var tourId = $('#tourId').val();
    debugger
    $.ajax({
        type: 'POST',
        url: urlTour.GetTourRegister,
        data: { managerId: managerId, tourId: tourId },
        success: function (response) {
            $('#conTable').append(response);
            debugger
            $('.divLoading').hide();
        }
    });
}

function getPlayer() {
    $('.divLoading').show();
    var Id = $('#player_code').val();
    var managerId = $('#manager_id').val();
    var tourId = $('#tourId').val();
    var catId = $('#category_id').val();
    var evenId = $('#even :selected').val();
    $.ajax({
        type: 'POST',
        url: urlTour.Player,
        data: { id: Id, managerId: managerId, tourId: tourId, catId: catId, evenId: evenId },
        success: function (response) {
            if (response.result.Successed) {
                if (response.data) {
                    $('#name').val(response.data.FIRST_NAME + ' ' + response.data.LAST_NAME);
                    $('#gender').val(response.data.GENDER_DESC);
                    $('#birth_date').val(response.data.BIRTH_YEAR);
                    $('#age').val(response.data.AGE);
                    $('#addPlay').prop("disabled", false);
                } else {
                    $('#player_code').val('');
                    $('#addPlay').prop("disabled", true);
                    alert("ไม่พบผู้เล่น");                    
                }                
            } else {
                $('#player_code').val('');
                $('#addPlay').prop("disabled", true);
                alert(response.result.ErrorMassage);
            }
            $('.divLoading').hide();
        }
    });
}

function addPlayer() {

}

function dlPlayer(e) {
    $('.divLoading').show();
    $.ajax({
        type: 'POST',
        url: urlTour.RemovePlayer,
        data: { regId: e },
        success: function (response) {
            debugger
            if (response.result.Successed) {
                alert("success");
                getTourRegister();
            }
            else {
                alert(response.result.ErrorMassage);
            }
                

            $('.divLoading').hide();
        }
    });
}