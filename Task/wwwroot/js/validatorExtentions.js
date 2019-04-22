// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var defaultRangeValidator = $.validator.methods.range;
$.validator.methods.range = function(value, element, param) {
    if(element.type === 'checkbox') {
        // if it's a checkbox return true if it is checked
        return element.checked;
    } else {
        // otherwise run the default validation function
        return defaultRangeValidator.call(this, value, element, param);
    }
}

var validateSectorsOnCheck = false; // for lazy validation
var requiredSectors = 1;
var sectors = $('.nested-select input[type="checkbox"]');
var errorMessage = 'Select at least 1 sector';
var errorElement = $('span[data-valmsg-for="Sectors"]');

// function to validate the required number of sectors
function validateSectors() {
    var selectedSectors = sectors.filter(':checked').length;
    var isValid = selectedSectors >= requiredSectors;
    if (!isValid) {
        errorElement.addClass('field-validation-error').removeClass('field-validation-valid').text(errorMessage);
    } else {
        errorElement.addClass('field-validation-valid').removeClass('field-validation-error').text('');
    }
    return (isValid);
}

$('form').submit(function () {
    validateSectorsOnCheck = true;
    if (!validateSectors()) {
        return false; // prevent submit
    }
});
$('#validate').on('click', 'input', function () {
    if (validateSectorsOnCheck) {
        validateSectors();
    }
})