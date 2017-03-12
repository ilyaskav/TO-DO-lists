$(function () {
    $(document).on({
        mouseenter: function (e) {
            $(this).find('.controls').css('display', 'block');
        },
        mouseleave: function (e) {
            $(this).find('.controls').css('display', 'none');
        }
    }, '.task, .title');

    $('#btn-add-project').on('click', function () {
        redirectToLogin();
    });

    $(document).on('click', '.add-task button', function () {
        redirectToLogin();
    })

    var redirectToLogin = function () {
        toastr.info("This action requires authentication. You will be redirected to login page");
        var timerID = window.setTimeout(function () {
            window.location.href = '/Account/Login';
        }, 3000);
    }
});