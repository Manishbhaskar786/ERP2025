(function ($) {
    var _departmentService = abp.services.app.department,    
        l = abp.localization.getSource('ERPack'),
        _$modal = $('#departmentCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#DepartmentTable');

    var _$departmentTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _departmentService.getAll,
            inputFilter: function () {
                return $('#DepartmentSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$departmentTable.ajax.reload(null, false); }
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
                sortable: false,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                data: 'deptName'
            },
            {
                targets: 2,
                data: 'description'
            },
            {
                targets: 3,
                data: null,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-department" data-department-id="${row.id}" data-toggle="modal" data-target="#DepartmentEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
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
        var department = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);

        _departmentService
            .create(department)
            .done(function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info(l('SavedSuccessfully'));
                _$departmentTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.edit-department', function (e) {
        var departmentId = $(this).attr('data-department-id');

        abp.ajax({
            url: abp.appPath + 'Department/EditModal?departmentId=' + departmentId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#DepartmentEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    abp.event.on('department.edited', (data) => {
        _$departmentTable.ajax.reload();
    });
     

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$departmentTable.ajax.reload();
    });

    $('.btn-clear').on('click', (e) => {
        $('input[name=Keyword]').val('');
        $('input[name=IsActive][value=""]').prop('checked', true);
        _$departmentTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$departmentTable.ajax.reload();
            return false;
        }
    });
    setInterval(fadeit, 2000);
})(jQuery);

function fadeit() {
    $(".modal-backdrop").hide();
}
