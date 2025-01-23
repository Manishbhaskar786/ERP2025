(function ($) {
    var _materialService = abp.services.app.material,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#MaterialsTable');

    var _$materialsTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _materialService.getAll,
            inputFilter: function () {
                return $('#MaterialsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$materialsTable.draw(false)
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
                sortable: false,
                defaultContent: '',
                checkboxes: {
                    selectRow: true
                }
            },
            {
                targets: 1,
                data: 'displayName'
            },
            {
                targets: 2,
                data: 'itemCode'
            },
            {
                targets: 3,
                data: 'itemTypeName'
            },
            {
                targets: 4,
                data: 'departmentName'
            },
            {
                targets: 5,
                data: 'buyingUnit'
            },
            {
                targets: 6,
                data: 'sellingUnit'
            },
            {
                targets: 7,
                data: 'buyingPrice'
            },
            {
                targets: 8,
                data: 'sellingPrice'
            },
            {
                targets: 9,
                data: 'hsn'
            },
            {
                targets: 10,
                data: 'cgst'
            },
            {
                targets: 11,
                data: 'sgst'
            },
            {
                targets: 12,
                data: 'igst'
            },
            {
                targets: 13,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-material" data-material-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-material" data-material-id="${row.id}" data-material-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.delete-material', function () {
        var materialId = $(this).attr("data-material-id");
        var materialName = $(this).attr('data-material-name');

        deleteMaterial(materialId, materialName);
    });

    function deleteMaterial(materialId, materialName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                materialName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _materialService.delete({
                        id: materialId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$materialsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-material', function (e) {
        var materialId = $(this).attr("data-material-id");
        e.preventDefault();
        window.location.href = '/Materials/AddMaterial?materialId=' + materialId;
    });

    $(document).on('change ', '.selectAll input[type="checkbox"]', function (e) {
        if (this.checked == true) {
            $('#bulkActions').show();
        }
        else if (this.checked == false) {
            $('#bulkActions').hide();
        }
    });

    $('.btn-search').on('click', (e) => {
        _$materialsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$materialsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);

function fnCreatePdf() {
    event.preventDefault();
    var tabledata = $("#MaterialsTable").html();
    let obj = {};
    obj.Name = "MaterialList";
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
    var tabledata = $("#MaterialsTable").html();
    let obj = {};
    obj.Name = "MaterialList";
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

