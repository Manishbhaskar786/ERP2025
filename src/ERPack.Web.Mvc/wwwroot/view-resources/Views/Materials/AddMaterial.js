(function ($) {
    var l = abp.localization.getSource('ERPack'),
        _$modalUnit = $('#UnitCreateModal'),
        _$form = $('#materialAddForm');

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        let material = new FormData($('form')[0]);
        $.ajax({
            type: "POST",
            url: "/Materials/AddMaterial",
            data: material,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success == true) {
                    window.location.href = '/Materials';
                    abp.notify.info(l('SavedSuccessfully'));
                    $('#materialAddForm')[0].reset();
                }
            },
            error: function () {
            }
        });
    });

    $('#saveUnit').on('click', (e) => {
        let unitName = $("#UnitName").val();

        $.ajax({
            type: "POST",
            url: "/Common/AddUnit?name=" + unitName,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.result.msg == "OK") {
                    let optionHTML = `
                                      <option value="${data.result.id}">
                                                     ${unitName}
                                      </option>`;
                    $('#BuyingUnitId').append(optionHTML);
                    $('#SellingUnitId').append(optionHTML);
                    abp.notify.info(l('Saved Successfully'));
                    _$modalUnit.modal('hide');
                }
            },
            error: function () {



            }
        });
    });
})(jQuery);



$("#DisplayName").blur(function () {
    let name = this.value;
    let idType = "MaterialId"



    if (name && name.length > 2 && $("#Id").val() == '0') {
        $.ajax({
            url: `/Common/GetIdByPreference?idType=${idType}&name=${name}`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                $("#ItemCode").val(data.result.id);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error("Error:", status, error);
            }
        });
    }
});



$(document).ready(function () {
    $('#hsnInput').on('input', function () {
        // Use a regular expression to match allowed characters
        const validPattern = /^[a-zA-Z0-9.]*$/;



        // Get the current value
        const inputValue = $(this).val();



        // If the value does not match the pattern, remove the last character
        if (!validPattern.test(inputValue)) {
            $(this).val(inputValue.slice(0, -1));
        }
    });



    $('#DisplayName').on('input', function () {
        // Regular expression to allow only alphabetic characters
        const validPattern = /^[a-zA-Z0-9 ]*$/;



        // Get the current value
        const inputValue = $(this).val();



        // If the value does not match the pattern, remove the last character
        if (!validPattern.test(inputValue)) {
            $(this).val(inputValue.slice(0, -1));
        }
    });



    function fadeit() {
        $(".modal-backdrop").hide();
    }
    // Use jQuery's setInterval
    setInterval(fadeit, 3000);



});