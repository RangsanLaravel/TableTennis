var urlTour = {};
$(document).ready(function () {
    getUrl();

    $('#adPy').on('click', function () {
        adpy();
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

function py(e) {
    $('.divLoading').show();
    $('#pl_name' + e).text('');
    $('#pl_gender' + e).text('');
    $('#pl_birth' + e).text('');
    $('#pl_age' + e).text('');
    var Id = $('#pl_code' + e).val();
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
                    $('#pl_name' + e).text(response.data.FIRST_NAME + ' ' + response.data.LAST_NAME);
                    $('#pl_gender' + e).text(response.data.GENDER_DESC);
                    $('#pl_birth' + e).text(response.data.BIRTH_DATE_STR);
                    $('#pl_age' + e).text(response.data.AGE);
                } else {
                    alert('ไม่พบนักกีฬา');
                }
                
            } else {
                alert(response.result.ErrorMassage);
                $('#pl_code' + e).val('');
            }

            $('.divLoading').hide();
        }
    });
}

function adpy() {
    debugger
    var py = []
    for (var i = 0; i < $('input.pyCode').serializeArray().length; i++) {
        if ($('input.pyCode').serializeArray()[i].value) {
            if ($('#pl_code' + i).val()) {
                py.push({
                    TOUR_MAP_ID: $('#even :selected').val()
                    , PLAYER_ID: $('#pl_code' + i).val()
                    , MANAGER_ID: $('#manager_id').val()
                    , CREATE_BY: $('#manager_code').val()
                    , CATEGORY_ID: $('#category_id').val()
                    , GROUP_NAME: $('#teamName').val()
                });
            }            
        }
    }

    if (py.length != 0) {
        if (py.length >=2 && py.length <= $('input.pyCode').serializeArray().length) {
            $.ajax({
                type: 'POST',
                url: urlTour.AddTourRegisters,
                data: { dataObj: py },
                success: function (response) {
                    $('#myModal').modal('toggle');
                    getTourRegister();                    
                    movePy();
                }
            });
        } else {            
            alert('คุณใส่ผู้เล่นไม่ครบ');
        }    
    }
    else {
        alert('ไม่มีข้อมูล');
    }
}

function movePy() {
    for (var i = 0; i < $('input.pyCode').serializeArray().length; i++) {
        $('#pl_name' + i).text('');
        $('#pl_gender' + i).text('');
        $('#pl_birth' + i).text('');
        $('#pl_age' + i).text('');
        $('#pl_code' + i).val('');
    }
}

function getTourRegister() {
    $('.divLoading').show();
    debugger
    $.ajax({
        type: 'POST',
        url: urlTour.GetTeamRegister,
        data: { managerId: $('#manager_id').val(), catId: $('#category_id').val(), tourId: $('#tourId').val() },
        success: function (response) {
            $('#tResult').html(response);
            debugger
            $('.divLoading').hide();
        }
    });
}

function dlTeam(tRef) {
    $('.divLoading').show();
    $.ajax({
        type: 'POST',
        url: urlTour.RemoveTeam,
        data: { team_ref: tRef },
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