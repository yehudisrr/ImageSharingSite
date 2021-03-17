$(() => {
    $(".view-image").submit(function (e) {
        const id = $('#id').val();
        const passwordEntered = $('#password-entered').val();
        const password = $('#password').val();
        if (passwordEntered != password) {
            e.preventDefault();
            $('#alertDiv').text('Invalid Password!');
        }
    });
});