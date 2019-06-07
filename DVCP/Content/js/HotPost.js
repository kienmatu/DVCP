$(function () {
    $("#lstPost").DataTable();
});
var amodal = $('#addModal');
var delmodal = $('#deleteModal');
var emodal = $('#editModal');
var idde;
var idfix;
var deleteConfirm = function (id, title) {
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/deleteHotPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không xóa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
});
$('#addHot').click(function () {
    $('#IDPost').val(null);
    amodal.modal('show');

});
$('#addBtn').click(function () {
    var valprio = $('#newPri').val();
    var prio = 100;
    var idadd = $('#IDPost').val();
    if (valprio != "") {
        prio = valprio;
    }
    amodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/addhotPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idadd, priority: prio }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1500);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không tạo được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
    clearDat();

});
var editPost = function (id, prior, title) {

    emodal.find('.modal-title').text(title);
    $('#Txtpriority').val(prior);
    emodal.modal('show');
    idfix = id;
}
$('#editBtn').click(function () {
    var newpri = $('#Txtpriority').val();
    emodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/editHotPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idfix, priority: newpri }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1500);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không sửa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
    //$('#CateNameEdit').val(null);
});
$('#IDPost').change(function () {
    checkPost()
});
$('#btnCheck').click(function () {
    checkPost()
});
var idz;
var checkPost = function () {
    idz = $('#IDPost').val();
    $.ajax({
        type: "POST",
        url: '/Admin/checkPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idz }),
        dataType: "json",
        success: function (recData) {
            if (recData.valid == true) {
                $('#postName').text(recData.Message);
                $('#postName').removeClass('text-danger');
                $('#postName').addClass('text-primary');
                $('#addBtn').removeAttr('disabled');
            }
            else if (recData.valid == false) {
                $('#postName').text(recData.Message);
                $('#postName').removeClass('text-primary');
                $('#postName').addClass('text-danger');
                $('#addBtn').prop("disabled", true);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/><br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
}
var clearDat = function () {
    $('#IDPost').val(null);
    $('#postName').text("ID không hợp lệ");
    $('#postName').removeClass('text-primary');
    $('#postName').addClass('text-danger');
}
$('.cleardt').click(function () {
    clearDat();
});