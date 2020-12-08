$(document).ready(function () {
    loadOrder(0);
    loadProduct(0);
    loadSale(0);
});
function loadOrder(i) {
    $.ajax({
        url: "/System/Manage/GetOrder",
        data: { page: i },
        success: function (result) {
            $("#order").html(result);
        }
    });
}

function loadProduct(i) {
    $.ajax({
        url: "/System/Manage/GetProduct",
        data: { page: i },
        success: function (result) {
            $("#product").html(result);
        }
    });
}

function loadSale(i) {
    $.ajax({
        url: "/System/Manage/Sale",
        data: { page: i },
        success: function (result) {
            $("#sale").html(result);
        }
    });
}


function loadCategory(i) {
    $.ajax({
        url: "/System/Manage/Category",
        data: { page: i },
        success: function (result) {
            $("#category").html(result);
        }
    });
}

//function loadProduct(i) {
//    $.ajax({
//        url: "/System/Manage/GetProduct",
//        data: { page: i },
//        success: function (result) {
//            $("#product").html(result);
//        }
//    });
//}
function deleteCat(id, name) {
    var html = '<div class="alert alert-danger">Xóa <b>' + name + '</b> và tất cả sản phẩm, hình ảnh có trong <b>'+name+'</b>?</div>';
    html += '<a href="EditCategory/' + id + '/delete"><button class="btn btn-danger">Xóa</button></a><button class="btn btn-primary" onclick="close("result");">Hủy</button>';
    $('#result').html(html);
}
function deleteProduct(id, name) {
    var html = '<div class="alert alert-danger">Xóa sản phẩm <b>' + name + '</b> và toàn bộ hình ảnh?</div>';
    html += '<a href="EditProduct/' + id + '/delete"><button class="btn btn-danger">Xóa</button></a><button class="btn btn-primary" onclick="close("result");">Hủy</button>';
    $('#result').html(html);
}
function close(id) {
    $('#' + id).html("");
}