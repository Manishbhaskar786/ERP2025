(function ($) {
    let _userService = abp.services.app.user,
        l = abp.localization.getSource('ERPack'),
        _$modal = $('#DepartmentCreateModal'),
        _$form = $('#userCreateForm');

    $.validator.addMethod("indianPincode", function (value, element) {
        return this.optional(element) || /^\d{6}$/.test(value);
    }, "");

    $.validator.addMethod("firstName", function (value, element) {
        return this.optional(element) || /^[A-Za-z]+$/.test(value);
    }, "");

    $.validator.addMethod("AadhaarNumber", function (value, element) {
        return this.optional(element) || /^\d{12}$/.test(value);
    }, "");

    $.validator.addMethod("Mobile", function (value, element) {
        const regex = /^\+91\d{10}$/;
        return this.optional(element) || regex.test(value);
    }, "");

    $.validator.addMethod("username", function (value, element) {
        return this.optional(element) || /^[a-zA-Z0-9]+$/.test(value);
    }, "");

    $.validator.addMethod("designation", function (value, element) {
        return this.optional(element) || /^[a-zA-Z\s]+$/.test(value);
    }, "");

    $.validator.addMethod("password", function (value, element) {
        return this.optional(element) || /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$/.test(value);
    }, "");

    $.validator.addMethod("pastDate", function (value, element) {
        const date = new Date(value);
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Set time to midnight for comparison
        return this.optional(element) || date < today;
    }, "");



    document.addEventListener('DOMContentLoaded', function () {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
        var yyyy = today.getFullYear();

        today = yyyy + '-' + mm + '-' + dd;
        document.getElementById('DOB').setAttribute('max', today);
    });

    $('#eyepass').click(function () {

        if ($(this).hasClass('fa-eye-slash')) {

            $(this).removeClass('fa-eye-slash');

            $(this).addClass('fa-eye');

            $('#Password').attr('type', 'text');

        } else {

            $(this).removeClass('fa-eye');

            $(this).addClass('fa-eye-slash');

            $('#Password').attr('type', 'password');
        }
    });

    $('#eyeconpass').click(function () {

        if ($(this).hasClass('fa-eye-slash')) {

            $(this).removeClass('fa-eye-slash');

            $(this).addClass('fa-eye');

            $('#ConfirmPassword').attr('type', 'text');

        } else {

            $(this).removeClass('fa-eye');

            $(this).addClass('fa-eye-slash');

            $('#ConfirmPassword').attr('type', 'password');
        }
    });


    const fileInputs = document.querySelectorAll('.custom-file-input');
    fileInputs.forEach(input => {
        input.addEventListener('change', function () {
            updateFileLabel(input);
        });
    });

    function updateFileLabel(input) {
        const label = document.querySelector(`label[for="${input.id}"]`);
        const files = Array.from(input.files);
        const fileNames = files.map(file => file.name).join(', ');
        label.textContent = fileNames || 'Choose Documents';
    }

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            },
            Pincode: {
                indianPincode: true
            },
            Name: {
                firstName: true
            },
            Surname: {
                firstName: true
            },
            UserName: {
                username: true
            },
            Designation: {
                designation: true
            },
            AdhaarNumber: {
                AadhaarNumber: true
            },
            Gender: "required",
            Mobile: {
                Mobile: true
            },
            DOB: {
                pastDate: true
            },
            Password: {
                password: true
            }
        },
        messages: {
            ConfirmPassword: {
                equalTo: "Confirm password must match the password."
            },
            pincode: {
                indianPincode: "Please enter a valid 6-digit PIN code."
            },
            Name: {
                firstName: "Please enter only characters."
            },
            Surname: {
                firstName: "Please enter only characters."
            },
            UserName: {
                username: "Special character not allowed."
            },
            Designation: {
                designation: "Special character and number not allowed."
            },
            AdhaarNumber: {
                AadhaarNumber: "Please enter a valid 12-digit Aadhaar number."
            },
            Gender: {
                require: "Please select an option."
            },
            Mobile: {
                Mobile: "Please enter a valid phone number."
            },
            DOB: {
                pastDate: "Date must be in the past."
            },
            Password: {
                password: "Password must be 8+ chars with upper, lower, number, and special character."
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        let roleNames = [];
        let _$roleCheckboxes = _$form[0].querySelectorAll("input[name='role']:checked");
        if (_$roleCheckboxes.length == 0) {
            abp.notify.error('Select at least one User Role.');
            return;
        }

        const maxFileSize = 4 * 1024 * 1024; // 4 MB in bytes
        const allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        const filesToValidate = [
            { id: "#ImageFile", name: "Image File" },
            { id: "#AdhaarDocFile", name: "Adhaar Document" },
            { id: "#PANDocFile", name: "PAN Document" }
        ];
        // Validate individual files
        for (const fileInfo of filesToValidate) {
            const fileInput = $(fileInfo.id)[0];
            if (fileInput.files.length > 0) {
                const file = fileInput.files[0];
                if (!allowedExtensions.exec(file.name)) {
                    abp.notify.error(`${fileInfo.name} must be a valid file type (JPEG, JPG, PNG).`);
                    return;
                }
                if (file.size > maxFileSize) {
                    abp.notify.error(`${fileInfo.name} size must not exceed 4 MB.`);
                    return;
                }
            }
        }

        // Validate academic documents
        const academicFiles = $("#AcademicDocsFile")[0].files;
        for (let i = 0; i < academicFiles.length; i++) {
            const file = academicFiles[i];
            if (!allowedExtensions.exec(file.name)) {
                abp.notify.error(`Academic Document ${i + 1} must be a valid file type (JPEG, JPG, PNG).`);
                return;
            }
            if (file.size > maxFileSize) {
                abp.notify.error(`Academic Document ${i + 1} size must not exceed 4 MB.`);
                return;
            }
        }

        // Create a new FormData object
        var formData = new FormData(_$form[0]); // This includes all form fields
        // Append the file inputs to FormData
        formData.append("ImageFile", $("#ImageFile")[0].files[0]);
        formData.append("AdhaarDocFile", $("#AdhaarDocFile")[0].files[0]);
        formData.append("PANDocFile", $("#PANDocFile")[0].files[0]);
        formData.append("IsActive", $('#CreateUserIsActive').is(':checked'));

        $("input[name='role']:checked").each(function () {
            formData.append("role", $(this).val()); // Append each checked role's value
        });
        
        formData.append("Image", $("#hdnImageFile").val());
        formData.append("AdhaarDoc", $("#hdnAdhaarFile").val());
        formData.append("PANDoc", $("#hdnPANFile").val());
        formData.append("AcademicDocs", $("#hdnAcademicFile").val());
        var age = calculateAge($('#DOB').val());
        if (age === null || age < 18) {
            abp.notify.error('User must be at least 18 years old.');
            return;
        }
        abp.ui.setBusy(_$form);

        let userId = $('#Id').val();
        let url = userId == 0 ? "/api/services/app/User/Create" : "/api/services/app/User/Update";
        let type = userId == 0 ? "POST" : "PUT";

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    if (userId == 0)
                        _$form[0].reset();
                    abp.notify.info(l('SavedSuccessfully'));
                    window.location.href = '/Users';
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: ", status, error);
                abp.notify.error(xhr.responseJSON.error.message);
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    });

    function calculateAge(dob) {
        if (!dob) return null;

        const [year, month, day] = dob.split('-').map(Number);
        const birthDate = new Date(year, month - 1, day);
        const today = new Date();

        if (isNaN(birthDate)) return null;

        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        const dayDiff = today.getDate() - birthDate.getDate();

        // Adjust age if the current date is before the birthdate in the current year
        if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
            age--;
        }

        return age;
    }

    $('#saveDepartment').on('click', (e) => {
        let departmentName = $("#DepartmentName").val();
        if (departmentName == null || departmentName == '' || departmentName == undefined) {
            $('#department-name').show();
            return;
        }
        else {
            $('#department-name').hide();
        }
        $.ajax({
            type: "POST",
            url: "/Common/AddDepartment?name=" + departmentName,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.result.msg == "OK") {
                    let optionHTML = `
                                      <option value="${data.result.id}">
                                                     ${departmentName}
                                      </option>`;
                    $('#DepartmentId').append(optionHTML);
                    abp.notify.info(l('SavedSuccessfully'));
                    $('#DepartmentCreateModal').modal('hide');
                }
                else {
                    abp.notify.error(data.result.msg);
                }
            },
            error: function () {
            }
        });
    });

})(jQuery);