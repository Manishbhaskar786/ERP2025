(function () {
    $('#ReturnUrlHash').val(location.hash);

    let _accountService = abp.services.app.account;
    let _$form = $('#LoginForm');

    _$form.submit(function (e) {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }

        abp.ui.setBusy(
            $('body'),

            abp.ajax({
                contentType: 'application/x-www-form-urlencoded',
                url: _$form.attr('action'),
                data: _$form.serialize()
            })
        );
    });
;
    var subdomain = window.location.hostname.split('.')[0];

    let tenancyName;
    if (/[^a-zA-Z0-9]/.test(subdomain)) {
        tenancyName = null;
    }
    else if (subdomain == "erpack" || subdomain == "localhost" || subdomain == "www") {
        tenancyName = null;
    }
    else {
        tenancyName = subdomain;
    }

    if (!tenancyName) {
        abp.multiTenancy.setTenantIdCookie(null);
        return;
    }
    

    _accountService.isTenantAvailable({
        tenancyName: tenancyName
    }).done(function (result) {
        switch (result.state) {
            case 1: //Available
                abp.multiTenancy.setTenantIdCookie(result.tenantId);
                return;
            case 2: //InActive
                abp.message.warn(abp.utils.formatString(abp.localization
                    .localize("TenantIsNotActive", "ERPack"),
                    tenancyName));
                break;
            case 3: //NotFound
                abp.message.warn(abp.utils.formatString(abp.localization
                    .localize("ThereIsNoTenantDefinedWithName{0}", "ERPack"),
                    tenancyName));
                break;
        }
    });
})();
