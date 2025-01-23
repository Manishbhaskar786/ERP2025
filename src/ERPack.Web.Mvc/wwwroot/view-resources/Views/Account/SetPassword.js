(function ($) {
    var _userService = abp.services.app.user,
        l = abp.localization.getSource('ERPack'),
        _$form = $('#SetPassword');

    $.validator.addMethod("regex", function (value, element, regexpr) {
        return regexpr.test(value);
    }, l("PasswordsMustBeAtLeast8CharactersContainLowercaseUppercaseNumber"));

    _$form.validate({
        rules: {
            NewPassword: {
                regex: /(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/
            },
            ConfirmNewPassword: {
                equalTo: "#NewPassword"
            }
        },
        messages: {
            ConfirmNewPassword: {
                equalTo: l("PasswordsDoNotMatch")
            }
        }
    });

    function save() {
        if (!_$form.valid()) {
            return;
        }
        
        var setPasswordDto = _$form.serializeFormToObject();
        
        abp.ui.setBusy(_$form);
        var skipClearBusy = false;
        _userService.setPassword(setPasswordDto).done(success => {            
            if (success) {
                skipClearBusy = true;
                abp.notify.info(l('SavedSuccessfully'));
                setTimeout(() => {
                    window.location.href = "/Tenants/Edit";
                }, 1200);
            }
        }).always(function () {
            if (!skipClearBusy) {
                abp.ui.clearBusy(_$form);
            }
        });
    }

    _$form.submit(function (e) {
        e.preventDefault();
        save();
    });
})(jQuery);