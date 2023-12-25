$(document).ready(function () {
    $('#customerInfo').submit(function (event) {
        event.preventDefault(); // Ngăn chặn việc gửi form một cách mặc định

        // Lấy dữ liệu từ form
        var formData = {
            tenKH: $('#TenKH').val(),
            sdt: $('#Sdt').val(),
            diaChi: $('#DiaChi').val(),
            email: $('#Email').val()
        };

        // Gửi dữ liệu lên server
        $.ajax({
            type: 'POST',
            url:'@Url.Action("ThongTin", "TrangChu")',  // Thay đổi đường dẫn tới endpoint xử lý form trên server
            data: formData,
            success: function (response) {
                // Xử lý kết quả từ server sau khi gửi dữ liệu thành công
                console.log(response);
                // Hiển thị thông báo hoặc thực hiện các thao tác khác sau khi gửi form thành công
            },
            error: function (error) {
                // Xử lý khi có lỗi xảy ra trong quá trình gửi dữ liệu
                console.error(error);
                // Hiển thị thông báo hoặc thực hiện các thao tác khác khi gửi form gặp lỗi
            }
        });
    });
});
