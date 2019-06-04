var amodal = $('#addModal');
$(function () {
    $("#lstPost").DataTable();

});
var deleteConfirm = function (id, title, idSerie) {
    var delmodal = $('#deleteModal');
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');
    $('#deleteBtn').click(function () {
        delmodal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/RemoveFromSerie',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: id, seriid: idSerie }),
            dataType: "json",
            success: function (recData) {
                var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                    type: 'pastel-info',
                    allow_dismiss: false,
                    timer: 1000,
                });
                setTimeout(function () {
                    window.location.reload();
                }, 2000);

            },
            error: function () {
                var notify = $.notify('<strong>Lỗi</strong><br/>Không xóa được<br />', {
                    type: 'pastel-warning',
                    allow_dismiss: false,
                });
            }
        });
    });
}
var changeStt = function (xthis) {
    var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
        url: '/Admin/changeStatus',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: xid, state: st }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
        },
        error: function () { alert('An error occured'); }
    });
}
$('#addBtn').click(function () {
    $('#IDPost').val(null);
    amodal.modal('show');
});
$('#addPostBtn').click(function () {
    var idpost = $('#IDPost').val();
    amodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/AddToSerie',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idpost, seriid: serieid }),
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
            var notify = $.notify('<strong>Lỗi</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });

});
$('#IDPost').change(function () {
    checkPost()
});
$('#btnCheck').click(function () {
    checkPost()
});
var checkPost = function () {
    var idz = $('#IDPost').val();
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
                $('#addPostBtn').removeAttr('disabled');
            }
            if (recData.valid == false) {
                $('#postName').text(recData.Message);
                $('#postName').removeClass('text-primary');
                $('#postName').addClass('text-danger');
                $('#addPostBtn').prop("disabled", true);
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
$("#addModal").on('hide', function () {
    $(':input').val('');
});