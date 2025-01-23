(function ($) {
    var _tenantService = abp.services.app.tenant,
        l = abp.localization.getSource('ERPack'),
        _$modal = $('#TenantCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TenantsTable');

    var _$tenantsTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _tenantService.getAll,
            inputFilter: function () {
                return $('#TenantsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$tenantsTable.ajax.reload(null, false); }
            }
        ],
        scrollX: !0,
        select: {
            style: 'multi'
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '',
                checkboxes: {
                    selectRow: true
                },
                sortable: false,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                data: 'tenancyName'
            },
            {
                targets: 2,
                data: 'name'
            },
            {
                targets: 3,
                data: 'panNumber'
            },
            {
                targets: 4,
                data: 'currency'
            },
            {
                targets: 5,
                data: 'status',
                sortable: false,
                render: (data, type, row) => {
                    var deletionTime = new Date(row.deletionTime);
                    const differenceInDays = Math.floor((Date.now() - deletionTime.getTime()) / (1000 * 60 * 60 * 24));
                    var diffrentOf90Days = 90 - differenceInDays;
                    if (diffrentOf90Days < 0) {
                        diffrentOf90Days = 90
                    }
                    if (data == 1) {
                        return `Active`;
                    } else if (data == 2) {
                        return `InActive`;
                    } else if (data == 3) {
                        return 'Under Deletion ( ' + diffrentOf90Days + ' Left )'
                    } else {
                        return 'Deleted'
                    }
                }
            },
            {
                targets: 6,
                data: null,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (row.status == 3) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-warning restore-tenant" data-tenant-id="${row.id}" data-tenancy-name="${row.name}">`,
                            `       <i class="fas fa-undo"></i> ${l('Restore')}`,
                            '   </button>'
                        ].join('');
                    } else {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-tenant" data-tenant-id="${row.id}" data-toggle="modal" data-target="#TenantEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger delete-tenant" data-tenant-id="${row.id}" data-tenancy-name="${row.name}">`,
                            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                            '   </button>'
                        ].join('');
                    }
                }
            }
        ]
    });

    $.validator.addMethod("tenancyName", function (value, element) {
        return this.optional(element) || /^[a-zA-Z][a-zA-Z0-9_-]*$/.test(value);
    }, "");

    _$form.validate({
        rules: {
            TenancyName: {
                tenancyName: true
            },
        },
        messages: {
            TenancyName: {
                tenancyName: "Name start with a letter and can include numbers, hyphens, and underscores."
            },
        }
    });

    _$form.find('.save-button').click(function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        const isActiveChecked = $('#CreateTenantIsActive').is(':checked');
        $('#IsActive').val(isActiveChecked ? 'true' : 'false');
        var tenant = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);

        _tenantService
            .create(tenant)
            .done(function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info(l('SavedSuccessfully'));
                _$tenantsTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-tenant', function () {
        var tenantId = $(this).attr('data-tenant-id');
        var tenancyName = $(this).attr('data-tenancy-name');

        deleteTenant(tenantId, tenancyName);
    });

    $(document).on('click', '.restore-tenant', function () {
        var tenantId = $(this).attr('data-tenant-id');
        var tenancyName = $(this).attr('data-tenancy-name');

        restorTenant(tenantId, tenancyName);
    });

    $(document).on('click', '.edit-tenant', function (e) {
        var tenantId = $(this).attr('data-tenant-id');

        abp.ajax({
            url: abp.appPath + 'Tenants/EditModal?tenantId=' + tenantId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#TenantEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    abp.event.on('tenant.edited', (data) => {
        _$tenantsTable.ajax.reload();
    });

    function deleteTenant(tenantId, tenancyName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('Deleting this tenant account is a permanent action. The account will be marked "Under Deletion" for 90 days, after which it will be permanently deleted and all data will be unrecoverable. You can restore the account within this 90-day period. An email notification will be sent to both the tenant and the administrator. Proceed with caution.'),
            ),
            abp.utils.formatString(
                l('Delete Tenant : {0}'),
                tenancyName
            ),
            (isConfirmed) => {
                if (isConfirmed) {
                    _tenantService
                        .delete({
                            id: tenantId
                        })
                        .done(() => {
                            abp.notify.info(l('SuccessfullyDeleted'));
                            _$tenantsTable.ajax.reload();
                        });
                }
            }
        );
        const dialogContent = document.querySelector('.swal-text');
        if (dialogContent) {
            dialogContent.innerHTML = '<b>Warning: Deleting this tenant account is a permanent action.</b> The account will be marked "Under Deletion" for 90 days, after which it will be permanently deleted and all data will be unrecoverable. You can restore the account within this 90-day period. An email notification will be sent to both the tenant and the administrator. <span style="color: red;"> Proceed with caution.</span>';
        }
        const confirmButton = document.querySelector('.swal-button--confirm');
        if (confirmButton) {
            confirmButton.textContent = l('Delete'); // Set the confirm button text
        }
    }

    function restorTenant(tenantId, tenancyName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('Warning: You can restore the tenant account within 90 days of deletion. After this period, the account and all associated data will be permanently unrecoverable. An email notification will be sent to both the tenant and the administrator once the account is restored. Proceed with caution'),
            ), abp.utils.formatString(
                l('Restore Tenant : {0}'),
                tenancyName
            ),
            (isConfirmed) => {
                if (isConfirmed) {
                    _tenantService
                        .restorTenant({
                            id: tenantId
                        })
                        .done(() => {
                            abp.notify.info(l('Successfully Restore Tenant'));
                            _$tenantsTable.ajax.reload();
                        });
                }
            }
        );
        const dialogContent = document.querySelector('.swal-text');
        if (dialogContent) {
            dialogContent.innerHTML = '<b>Warning: You can restore the tenant account within 90 days of deletion.</b> After this period, the account and all associated data will be permanently unrecoverable. An email notification will be sent to both the tenant and the administrator once the account is restored. <span style="color: red;"> Proceed with caution.</span>';
        }
        const confirmButton = document.querySelector('.swal-button--confirm');
        if (confirmButton) {
            confirmButton.textContent = l('Restore');
        }
    }

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$tenantsTable.ajax.reload();
    });

    $('.btn-clear').on('click', (e) => {
        $('input[name=Keyword]').val('');
        $('input[name=IsActive][value=""]').prop('checked', true);
        _$tenantsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$tenantsTable.ajax.reload();
            return false;
        }
    });
    setInterval(fadeit, 2000);
})(jQuery);

function fadeit() {
    $(".modal-backdrop").hide();
}
