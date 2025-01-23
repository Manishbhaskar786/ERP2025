(function ($) {
    let _tenantService = abp.services.app.tenant,
        l = abp.localization.getSource('ERPack'),
        _$form = $('#tenantEditForm');

    $.validator.addMethod("validatePAN", function (value, element) {
        return this.optional(element) || /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/.test(value);
    }, "Please enter a valid PAN Card Number.");

    $.validator.addMethod("validateGST", function (value, element) {
        return this.optional(element) || /^(?:[0-9]{2})(?:[A-Z]{4})(?:\d{4})(?:[A-Z]{1})(?:Z)(?:[0-9A-Z]{1})$/.test(value);
    }, "Please enter a valid GST Number.");
    $.validator.addMethod("validateAccountNumber", function (value, element) {
        return this.optional(element) || /^\d+$/.test(value);
    }, "Account Number must be numeric.");

    _$form.validate({
        rules: {
            PANNumber: {
                validatePAN: true
            },
            GSTNumber: {
                validateGST: true
            },
            AccountNumber: {
                validateAccountNumber: true
            }
        },
        messages: {
            PANNumber: {
                validatePAN: "Please enter a valid PAN Card Number."
            },
            GSTNumber: {
                validateGST: "Please enter a valid GST Number."
            },
            AccountNumber: {
                validateAccountNumber: "Account Number must be numeric."
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        let tenant = new FormData($('form')[0]);
        tenant.append('Logo', $("#hdnLogoFile").val());

        abp.ui.setBusy(_$form);
        $.ajax({
            type: "POST",
            url: "/Tenants/Update",
            data: tenant,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success == true) {
                    abp.ui.clearBusy(_$form);
                    abp.notify.info(l('SavedSuccessfully')); 
                    window.location = '/Tenants/EditHostInfo';
                }
            },
            error: function () {

            }
        });

    });

})(jQuery);