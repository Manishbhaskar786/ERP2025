(function ($) {
    let l = abp.localization.getSource('ERPack'),
        _$form = $('#vendorCreateForm');


    $.validator.addMethod("noAlphanumeric", function (value, element) {
        return this.optional(element) || /^[A-Za-z]+$/.test(value);
    }, "Only alphabetic characters are allowed.");
   
    _$form.validate({
        rules: {
            email: true,
            BankName: {
              //  required: true,
                noAlphanumeric: true // Custom validation rule
            },
            AccountNumber: {
              //  required: true,
                noAlphanumeric: true // Custom validation rule
            },
            Branch: {
              //  required: true,
                noAlphanumeric: true // Custom validation rule
            },
            IFSC: {
             //   required: true,
                noAlphanumeric: true // Custom validation rule
            }
        },
        messages: {
            email: 'enter a valid email address.',
            BankName: {
             //   required: 'Please enter the bank name.',
                noAlphanumeric: 'Only alphabetic characters are allowed.'
            },
            AccountNumber: {
            //    required: 'Please enter the account number.',
                noAlphanumeric: 'Only alphabetic characters are allowed.'
            },
            Branch: {
             //   required: 'Please enter the branch name.',
                noAlphanumeric: 'Only alphabetic characters are allowed.'
            },
            IFSC: {
            //    required: 'Please enter the IFSC code.',
                noAlphanumeric: 'Only alphabetic characters are allowed.'
            }
        }
    });


    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        let vendor = new FormData($('form')[0]);

        $.ajax({
            type: "POST",
            url: "/Vendors/AddVendor",
            data: vendor,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success == true) {
                    abp.notify.info(l('SavedSuccessfully'));
                    _$form[0].reset();
                    window.location = '/Vendors';
                }
            },
            // TODO document why this block is empty
            error: function () {

            }
        });
    });
})(jQuery);

function restrictNonNumeric(event) {
    // Allow: backspace, delete, tab, escape, enter, and arrow keys
    if ([46, 8, 9, 27, 13, 37, 38, 39, 40].indexOf(event.keyCode) !== -1 ||
        // Allow: Ctrl+A, Ctrl+C, Ctrl+V
        (event.ctrlKey === true || event.metaKey === true) &&
        [65, 67, 86].indexOf(event.keyCode) !== -1) {
        return; // Allow these keys
    }

    // Ensure that the key is a number (0-9)
    if (event.key < '0' || event.key > '9') {
        event.preventDefault(); // Prevent the key press
    }
}

$("#VendorName").blur(function () {
    let name = this.value;
    let idType = "VendorId"

    if (name && name.length > 2 && $("#Id").val() == '0') {
        $.ajax({
            url: `/Common/GetIdByPreference?idType=${idType}&name=${name}`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                $("#VendorCode").val(data.result.id);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error("Error:", status, error);
            }
        });
    }
});