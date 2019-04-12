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
