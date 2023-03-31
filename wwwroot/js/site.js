// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function CountryCreateModalForm() {
    $.ajax(
        {
            url: "/country/CreateModalForm",
            type: 'get',
            success: function (response) {
                $("#DivCreateDialog").html(response);
                ShowCreateModalForm();
            }
        });
}
function CityCreateModalForm() {
    var lstCountryCtrl = document.getElementById('lstCountryId');
    var countryId = lstCountryCtrl.options[lstCountryCtrl.selectedIndex].value;
    $.ajax(
        {
            url: "/city/CreateModalForm?countryid=" +countryId,
            type: 'get',
            success: function (response) {
                $("#DivCreateDialog").html(response);
                ShowCreateModalForm();
            }
        });
}
function FillCities(lstCountryCtrl, lstCityId) {
    var lstCities = $("#" + lstCityId);
    lstCities.empty();

    lstCities.append($('<option/>',
        {
            value: null,
            text: "Select City"
        }));

    var selectedCountry = lstCountryCtrl.options[lstCountryCtrl.selectedIndex].value;

    if (selectedCountry != null && selectedCountry != '') {
        $.getJSON('/Customer/GetCitiesByCountry', { countryId: selectedCountry }, function (cities) {
            if (cities != null && !jQuery.isEmptyObject(cities)) {
                $.each(cities, function (index, city) {
                    lstCities.append($('<option/>',
                        {
                            value: city.value,
                            text: city.text
                        }));
                });
            };
        });
    }
    return;
}
$(".custom-file-input").on("change", function () {

    var fileName = $(this).val().split("\\").pop();

    document.getElementById('PreviewPhoto').src = window.URL.createObjectURL(this.files[0]);

    document.getElementById('PhotoUrl').value = fileName;

});
function ShowCreateModalForm() {
    $("#DivCreateDialogHolder").modal('show');
    return;
}

function SubmitModalForm() {
    var btnSubmit = document.getElementById('btnSubmit');
    btnSubmit.click();
}

function refreshCountryList() {
    var btnBack = document.getElementById('dupBackbtn');
    btnBack.click();
    FillCountries("lstCountryId");
}
function refreshCityList() {
    var btnBack = document.getElementById('dupBackbtn');
    btnBack.click();
    var lstCountryCtrl = document.getElementById('lstCountryId');
    FillCities(lstCountryCtrl, "lstCity");
}
function FillCountries(lstCountryId) {
    var lstCountries = $("#" + lstCountryId);
    lstCountries.empty();

    lstCountries.append($('<option/>',
        {
            value: null,
            text: "Select Country"
        }));
    $.getJSON('/country/GetCountries', function (countries) {
        if (countries != null && !jQuery.isEmptyObject(countries)) {
            $.each(countries, function (index, country) {
                lstCountries.append($('<option/>',
                    {
                        value: country.value,
                        text: country.text
                    }));
            });
        };
    });
    return;
}