$(function () {
    $("#lstPost2").DataTable();
});
var newModal = $('#addModal');
var delmodal = $('#deleteModal');
var editModal = $('#editModal');

$('#addTag').click(function () {
    $('#TagName').val(null);
    newModal.modal('show');
});
$('#ConfirmAdd').click(function () {
    var sname = $('#TagName').val();
    
    if (sname == "") {
        alert("Hãy nhập đủ thông tin");
    }
    else {
        newModal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/NewTag',
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
            error: function (recData) {
                var notify = $.notify('<strong>Lỗi</strong><br/>' + recData.Message + '<br />', {
                    type: 'pastel-warning',
                    allow_dismiss: false,
                });
            }
        });
    }

});
var ide;
var EditTag = function (id, name) {
    ide = id;
    $('#TagNameEdit').val(name);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var sname2 = $('#TagNameEdit').val();
    if (sname2 == "") {
        alert("Hãy nhập đủ thông tin");
    }
    else {
        editModal.modal('hide');
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateTag',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, name: sname2}),
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
    }

});
////
var idde;
var deleteConfirm = function (id, title) {
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteTag',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
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
