$(function () {
    $("#lstPost").DataTable();
});
var delmodal = $('#deleteModal');
var idx;
var deleteConfirm = function (id, title) {
    idx = id; 
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');
   
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteConfirmed',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idx }),
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
//$('.slider').click(function () {
//    var prev = $(this).prev();
//    alert(prev.prop('checked'));
//});
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