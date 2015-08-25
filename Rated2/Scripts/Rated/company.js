var Company = {
    GetCompanyForUser: function () {
        var jqxhr = $.ajax({
            url: "/api/CompanyApi/GetCompanyForUser",
            type: "GET",
        });
        
        jqxhr.done(function (data) {
            for (var i = 0; i <= data.length - 1; i++) {
                $("#CompanyId")
                    .append($("<option></option>")
                    .attr("value", data[i].CompanyId)
                    .text(data[i].Name));
            }
        });

        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    GetCompanyForUserIfNeeded: function () {
        if ($("#CompanyId option").size() == 1) {
            Company.GetCompanyForUser();
        }
    },

};

function handleErrors(jqXHR) {
    alert("API ERROR: " + jqXHR.textStatus);
}

$(document).ready(function () {

});