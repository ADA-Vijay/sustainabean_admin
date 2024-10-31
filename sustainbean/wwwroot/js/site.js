
function getApiUrl(){
    return api_url
}




function ValidateField(form_id) {
    $("#" + form_id).find('input.validate, input.validate-email, input.validate-mobile, textarea.validate, select.validate').each(function () {
        const inputField = $(this);
        const value = inputField.hasClass('select2-hidden-accessible') ? inputField.val().join(',').trim() : inputField.val().trim();
        const errorSpan = inputField.hasClass("select2-hidden-accessible") ? inputField.closest(".form-group").find("span.text-danger") : inputField.siblings('span'); // Find the sibling span for error message
        const errorMessage = errorSpan.attr('error-msg'); // Get error message from the error-msg attribute
        if (value === '') {
            errorSpan.text(errorMessage).show(); // Set the error message and display it
            inputField.hasClass('select2-hidden-accessible') ? inputField.next('.select2-container').find('.select2-selection').addClass('is-invalid') : inputField.addClass('is-invalid'); // Add invalid class for styling
        } else {
            errorSpan.text("").hide(); // Hide error message
            inputField.hasClass('select2-hidden-accessible') ? inputField.next('.select2-container').find('.select2-selection').removeClass('is-invalid') : inputField.removeClass('is-invalid'); // Remove invalid class
        }
    });
}
function ValidateEmail(id) {
    const inputField = $("#" + id);
    const value = inputField.val().trim();
    const errorSpan = inputField.siblings('span');
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    // Validate email against the regex
    if (value == "") {
        errorSpan.text("Please enter email").show();
        inputField.addClass('is-invalid');
    }
    else if (!emailRegex.test(value)) {
        errorSpan.text("Please enter valid email format").show(); // Show error message
        inputField.addClass('is-invalid'); // Add invalid class for styling
    } else {
        errorSpan.text("").hide(); // Hide error message
        inputField.removeClass('is-invalid'); // Remove invalid class
    }
}
function ValidateLogo(id) {
    const inputField = $("#" + id);
    const value = inputField.val().trim();
    const errorSpan = inputField.siblings('span');
    const errorMessage = errorSpan.attr('error-msg');

    if (value === '' && inputField.closest('.form-group').find('#img_preview').attr('src') === '') {
        errorSpan.text(errorMessage).show();
        inputField.addClass('is-invalid');
    } else {
        errorSpan.text("").hide();
        inputField.removeClass('is-invalid');
    }
}
function ValidateContactNumber(id) {
    const inputField = $("#" + id);
    const value = inputField.val().trim();
    const errorSpan = inputField.siblings('span');

    // Check if the input value is exactly 10 digits long
    if (value == "") {
        errorSpan.text("Please enter mobile number").show();
        inputField.addClass('is-invalid');
    }
    else if (value.length !== 10) {
        errorSpan.text("Please enter valid 10 digit mobile number").show();
        inputField.addClass('is-invalid');
    } else {
        errorSpan.text("").hide();
        inputField.removeClass('is-invalid'); // Remove invalid class
    }
}
function IsValid(form_id) {
    return $("#" + form_id).find(".is-invalid").length > 0 ? false : true
}

function IsValidDetail() {
    if (
        window.sessionStorage.getItem('user_detail') == null || window.sessionStorage.getItem('token') == null ||
        window.sessionStorage.getItem('refresh_token_expiry') == '' || window.sessionStorage.getItem('refresh_token') == ''

    ) return false;
    else return true;
}