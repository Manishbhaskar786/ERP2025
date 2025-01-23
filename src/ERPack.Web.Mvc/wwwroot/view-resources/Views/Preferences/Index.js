(function ($) {
    let _preferenceService = abp.services.app.preference,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#PreferencesTable');

    let _$preferencesTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _preferenceService.getAll,
            inputFilter: function () {
                return $('#PreferencesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$preferencesTable.draw(false)
            }
        ],
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'idType'
            },
            {
                targets: 1,
                data: 'nameIdentifier'
            },
            {
                targets: 2,
                data: 'fixedName'
            },
            {
                targets: 3,
                data: 'monthSelection'
            },
            {
                targets: 4,
                data: 'yearSelection'
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-preference" data-preference-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-preference" data-preference-id="${row.id}" data-preference-name="${row.idType}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.delete-preference', function () {
        let preferenceId = $(this).attr("data-preference-id");
        let preferenceName = $(this).attr('data-preference-name');

        deletePreference(preferenceId, preferenceName);
    });

    function deletePreference(preferenceId, preferenceName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                preferenceName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _preferenceService.delete({
                        id: preferenceId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$preferencesTable.ajax.reload();
                    }).fail(function (jqXHR, textStatus) {
                    });;
                }
            }
        );
    }

    $(document).on('click', '.edit-preference', function (e) {
        let preferenceId = $(this).attr("data-preference-id");
        e.preventDefault();
        window.location.href = '/Preferences/AddPreference?preferenceId=' + preferenceId;
    });

    $('.btn-search').on('click', (e) => {
        _$preferencesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$preferencesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
