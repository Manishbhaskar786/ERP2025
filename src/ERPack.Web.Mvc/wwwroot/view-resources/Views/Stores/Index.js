(function ($) {
    let _storeService = abp.services.app.store,
        l = abp.localization.getSource('ERPack'),
        _$modal = $('#StoreCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#StoresTable');

    let _$storesTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _storeService.getAll,
            inputFilter: function () {
                return $('#StoresSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$storesTable.ajax.reload(null, false); },
            }
        ],
        scrollX: !0,
        select: {
            style: 'multi'
        },
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '',
                checkboxes: {
                    selectRow: true
                },
                sortable: false
            },
            {
                targets: 1,
                data: 'storeName'
            },
            {
                targets: 2,
                data: 'storeLocation'
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-store" data-store-id="${row.id}" data-toggle="modal" data-target="#StoreEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-store" data-store-id="${row.id}" data-store-name="${row.storeName}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.find('.save-button').click(function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        let store = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);

        _storeService
            .create(store)
            .done(function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info(l('SavedSuccessfully'));
                _$storesTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-store', function () {
        let storeId = $(this).attr('data-store-id');
        let storeName = $(this).attr('data-store-name');

        deleteStore(storeId, storeName);
    });

    $(document).on('click', '.edit-store', function (e) {
        let storeId = $(this).attr('data-store-id');

        abp.ajax({
            url: abp.appPath + 'Stores/EditModal?storeId=' + storeId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#StoreEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    abp.event.on('store.edited', (data) => {
        _$storesTable.ajax.reload();
    });

    function deleteStore(storeId, storeName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                storeName
            ),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _storeService
                        .delete({
                            id: storeId
                        })
                        .done(() => {
                            abp.notify.info(l('SuccessfullyDeleted'));
                            _$storesTable.ajax.reload();
                        });
                }
            }
        );
    }

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$storesTable.ajax.reload();
    });

    $('.btn-clear').on('click', (e) => {
        $('input[name=Keyword]').val('');
        $('input[name=IsActive][value=""]').prop('checked', true);
        _$storesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$storesTable.ajax.reload();
            return false;
        }
    });

    $(document).on('change ', '.selectAll input[type="checkbox"]', function (e) {
        if (this.checked == true) {
            $('#bulkActions').show();
        }
        else if (this.checked == false) {
            $('#bulkActions').hide();
        }
    });

    function fadeit() {
        $(".modal-backdrop").hide();
    }
    // Use jQuery's setInterval
    setInterval(fadeit, 3000);

})(jQuery);

function fnCreatePdf() {
    event.preventDefault();
    var tabledata = $("#StoresTable").html();
    let obj = {};
    obj.Name = "StoreList";
    obj.Html = tabledata;
    let data = JSON.stringify(obj);
    $.ajax({
        type: "POST",
        url: "/Common/GeneratePdf",
        data: data,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.success == true) {
                window.open(response.result.data, "_blank");
            }
        },
        error: function () {

        }
    });
}

function fnCreateCSV() {
    event.preventDefault();
    var tabledata = $("#StoresTable").html();
    let obj = {};
    obj.Name = "StoreList";
    obj.Html = tabledata;
    let data = JSON.stringify(obj);
    $.ajax({
        type: "POST",
        url: "/Common/GenerateExcel",
        data: data,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.success == true) {
                window.open(response.result.data, "_blank");
            }
        },
        error: function () {

        }
    });
}
