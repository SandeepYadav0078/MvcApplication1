$(function () {
    $(".form-validate").validate({
        errorPlacement: function(error, element)
        {
            error.insertAfter(element);
        },
        submitHandler : function(form) {
            tryLogin();
    }

    });
    $(".form-validate-signin").validate({
        errorPlacement: function(error, element)
        {
            error.insertAfter(element);
        }
    });
    $(".form-validate-signup").validate({
        rules: {
            age: {
                range: [0,100]
            }
        },
        errorPlacement: function(error, element)
        {
            error.insertAfter(element);
        }
    });

});