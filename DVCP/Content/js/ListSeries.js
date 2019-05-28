$(function () {
    $("#lstPost").DataTable();
});

$('#addBtn').click(function () {
    var newModal = $('#newModal');
    $('#srname').val(null);
    newModal.modal('show');
    $('#confirmaddBtn').click(function () {
        var sname = $('#srname').val();
        newModal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/addSerie',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ name: sname }),
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
    });
});
var deleteConfirm = function (id, title) {
    var delmodal = $('#deleteModal');
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');

    $('#deleteBtn').click(function () {
        delmodal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/DeleteSeries',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: id }),
            dataType: "json",
            success: function (recData) {
                var notify = $.notify('<strong>Thành công</strong><br />' + recData.Message + '<br />', {
                    type: 'pastel-info',
                    allow_dismiss: false,
                    timer: 1000,
                });
                setTimeout(function () {
                    window.location.reload();
                }, 2000);

            },
            error: function () {
                var notify = $.notify('<strong>Lỗi</strong><br />Không xóa được<br />', {
                    type: 'pastel-warning',
                    allow_dismiss: false,
                });
            }
        });
    });
}
var EditSr = function (id, title) {
    var editModal = $('#editModal');
    $('#srname2').val(title);
    editModal.modal('show');
    $('#confirmeditBtn').click(function () {
        var sname2 = $('#srname2').val();
        $('#srname2').val(null);
        editModal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/editSerie',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: id, name: sname2 }),
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
    });
}