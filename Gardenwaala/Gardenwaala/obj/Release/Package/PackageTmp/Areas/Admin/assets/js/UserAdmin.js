$(document).ready(function () {
    //fadeout the Delete message div
    $('#divDel').fadeOut(5000);
    //initially hide status div
    $('#divStatus').hide();

    //hide status dropdown if no checkbox is selected, otherwise show it
    $('#tbl').on('change', 'input#Ids', function () //dynamic event
    {
        var cnt = $('input#Ids:checked').length;
        if (cnt > 0) {
            $('#divStatus').show();
            $('#head').hide();
        }
        else {
            $('#divStatus').hide();
            $('#head').show();
        }

    });

    $('#SelectAll').change(function () {
        var status = this.checked;
        $('input#Ids').each(function () {
            this.checked = status;
        });
        var cnt = $('input#Ids:checked').length;
        if (cnt > 0 && status == true) {
            $('#divStatus').show();
            $('#head').hide();
        }
        else {
            $('#divStatus').hide();
            $('#head').show();
        }

    });

    $('#tbl').on('change', 'input#Ids', function () {
        if ($(this).prop('checked') == false)
            $('#SelectAll').prop('checked', false);
        if ($('input#Ids:checked').length == $('input#Ids').length) {
            $("#SelectAll").prop('checked', true);
        }
    });


    //status dropdown validation using jQuery
    $("#frmStatus").validate({
        errorClass: 'field-validation-error',
        rules: {
            Status: {
                required: true
            }
        },
        messages: {
            Status: {
                required: "Select Status"
            }
        }
    });

    $('#btnGo').click(function () {
        var table = $('#tbl').DataTable();
        var info = table.page.info();
        var order = table.order();
        var Ids = new Array();

        $('#skip').val(info.start);
        $('#pgSize').val(info.length);
        $('#sortColumn').val(order[0][0]);
        $('#sortDir').val(order[0][1]);
        $('input#Ids:checked').each(function () {
            Ids.push($(this).val());
        });
        $('#chkIds').val(Ids);
    });

    
});