(function ($) {
    let l = abp.localization.getSource('ERPack'),
        _$form = $('#preferenceCreateForm');

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        let preference = new FormData($('form')[0]);

        $.ajax({
            type: "POST",
            url: "/Preferences/AddPreference",
            data: preference,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success == true) {
                    if (response.result.msg == "EXISTS") {
                        abp.notify.info(l('Already Exists'));
                    }
                    else {
                        abp.notify.success(l('SavedSuccessfully'));
                        $('#preferenceCreateForm')[0].reset();
                        window.location.href = '/Preferences/Index';
                    }
                }
            },
            // TODO document why this block is empty
            error: function () {

            }
        });
    });
})(jQuery);