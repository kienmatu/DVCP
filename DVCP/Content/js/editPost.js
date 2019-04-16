$("#post_type").change(function () {
    var type = this.value;
    if (type == 2) {
        $('#imganno').show();
    }
    else {
        $('#imganno').hide();
    }
});
$('#Status').change(function () {
    var x = $('#Status').prop("checked");
    if (x == true) {
        $('#showstatus').text(' Đăng ngay');
        $('#Status').val('true');
    }
    else if (x == false) {
        $('#showstatus').text(' Đăng nháp (không hiển thị)');
        $('#Status').val('false');
    }
});
$('#changeAvatar').change(function () {
    var x = $('#changeAvatar').prop("checked");
    if (x == true) {
        $('#changeAvtText').text(' Thay đổi');
        $('#changeAvatar').val('true');
        $('#avatarPost').show();
        $('#avtpreview').show();
    }
    else if (x == false) {
        $('#changeAvtText').text(' Giữ nguyên');
        $('#changeAvatar').val('false');
        $('#avatarPost').hide();
        //$('#avtpreview').hide();
        $("#avatarFile").val(null);
    }
});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#avtpreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#avatarFile").change(function () {
    readURL(this);
});