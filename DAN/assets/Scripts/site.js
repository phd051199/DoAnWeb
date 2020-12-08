$(document).ready(function () {
    loadNewProduct(0);
    loadMostView(0);
    vk_slideshow(4000);
});
$(function () {
    $(this).scroll(function () {
        if ($(this).scrollTop() >= 100) {
            $('#up-arrow').css("display", "block");
        } else {
            $('#up-arrow').css("display", "none");
        }
    });
    $('#up-arrow').click(function () {
        $('body,html').animate({
            scrollTop: 0
        });
    });
    $('#select-mate option').click(function () {
        location.href = $('#select-mate').val();
    });
    $(document).on('click', '.close, #over', function () {
        closeCart();
    });
});
function showCart() {
    if (window.innerWidth >= 700) {
        $.ajax({
            url: "/Cart/ShowCart",
            success: function (result) {
                closeCart();
                $('body').append('<div id="over">');
                $('body').append(result);
            }
        });
    } else {
        location.href = '/Cart';
    }
}
function addCart(id, soLuong) {
    $.ajax({
        url: "/Cart/AddCart",
        data: { itemId: id, soLuong: soLuong },
        success: function (result) {
        }
    });
}
function removeCart(id) {
    $.ajax({
        url: "/Cart/RemoveCart",
        data: { itemId: id },
        success: function (result) {
            showCart();
        }
    });
}
function closeCart() {
    $('div#over').remove();
    $('div.cartPartial').remove();
}
function loadNewProduct(i) {
    $.ajax({
        url: "/Home/NewProduct",
        data: { page: i },
        success: function (result) {
            $("#new-product").html(result);
        }
    });
}


function loadSearchView(i) {
    $.ajax({
        url: "/Home/SearchProduct",
        data: { page: i },
        success: function (result) {
            $("#product").html(result);
        }
    });
}
function loadMostView(i) {
    $.ajax({
        url: "/Home/MostView",
        data: { page: i },
        success: function (result) {
            $("#most-view").html(result);
        }
    });
}
function loadCategory(i) {
    $.ajax({
        url: "/Category",
        data: { itemId: $('div.title').attr("id"), page: i },
        success: function (result) {
            $("#catgory").html(result);
        }
    });
}

function widegetStatus(slides) {
    slides.each(function (index) {
        if ($(this).attr('class') == 'current')
            $('#controlNav a').removeClass('active').eq(index).addClass('active');
    });
}
function slideshow(prev) {
    var slides = $('#slideshow li');
    var currElem = slides.filter('.current');
    var lastElem = slides.filter(':last');
    var firstElem = slides.filter(':first');
    // Xác định phần tử kế tiếp là prev hay next
    var nextElem = (prev) ? currElem.prev() : currElem.next();
    if (prev) {
        if (firstElem.attr('class') == 'current') nextElem = lastElem;
    } else if (lastElem.attr('class') == 'current') nextElem = firstElem;
    fadeElem(currElem, nextElem);
    widegetStatus(slides);
}
function wideget(index) {
    var slides = $('#slideshow li');
    var currElem = slides.filter('.current');
    var nextElem = slides.eq(index);
    fadeElem(currElem, nextElem);
    widegetStatus(slides);
}
function fadeElem(currElem, nextElem) {
    currElem.removeClass('current').find('img').css({ 'z-index': '50' }).end().find('p').css({ 'z-index': '50' });
    nextElem.addClass('current').find('img').css({ 'opacity': '0', 'z-index': '100' }).animate({ opacity: 1 }, 700, function () {
        currElem.find('img').css({ 'z-index': '0' });
    }).end().find('p').css({ 'height': '0', 'z-index': '100' }).animate({ height: 50 }, 500, function () {
        currElem.find('p').css({ 'z-index': '0' });
    });
}
function vk_slideshow(time) {
    var idset = setInterval('slideshow()', time);
    var liarr = $('#slideshow ul li');
    var controlNav = $('#controlNav');
    var str = '';
    for (var i = 0; i < liarr.length; i++) {
        str += '<a></a>';
    }
    controlNav.append(str);
    controlNav.children().each(function (index) {
        $(this).click(function () {
            wideget(index); clearInterval(idset);
            idset = setInterval('slideshow()', time);
        });
    }).eq(0).addClass('active');
    $('#next').click(function () {
        slideshow(); clearInterval(idset);
        idset = setInterval('slideshow()', time);
    });
    $('#prev').click(function () {
        slideshow(true); clearInterval(idset);
        idset = setInterval('slideshow()', time);
    });
}