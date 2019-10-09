$(document).ready(function() {
    $('#tryitForm').bootstrapValidator({
        fields: {
            txtFName: {
                validators: {
                    notEmpty: {
                        message: 'The first name is required and cannot be empty'
                    }
                }
            },
            txtLName: {
                validators: {
                    notEmpty: {
                        message: 'The last name is required and cannot be empty'
                    }
                }
            },          
        },
    });
});
