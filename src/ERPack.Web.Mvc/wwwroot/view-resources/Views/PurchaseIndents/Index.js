(function ($) {
    var _purchaseIndentsService = abp.services.app.purchaseIndent,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#PurchaseIndentsTable');

    var _$purchaseIndentsTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _purchaseIndentsService.getAll,
            inputFilter: function () {
                return $('#PurchaseIndentsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$purchaseIndentsTable.ajax.reload(null,false) },
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
                sortable: false
            },
            {
                targets: 1,
                data: 'itemCode'
            },
            {
                targets: 2,
                data: 'materialName'
            },
            {
                targets: 3,
                data: 'quantity'
            },
            {
                targets: 4,
                data: 'requiredBy'
            },
            {
                targets: 5,
                data: 'remark'
            },
            {
                targets: 6,
                data: 'requestedByUser'
            },
            {
                targets: 7,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-purchaseindent" data-purchaseindent-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-purchaseindent" data-purchaseindent-id="${row.id}" data-purchaseindent-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $('.btn-search').on('click', (e) => {
        _$purchaseIndentsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$purchaseIndentsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
