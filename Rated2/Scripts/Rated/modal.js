
$(document).ready(function () {
    $('#modalAddProject').on('shown.bs.modal', function (e) {
        Company.GetCompanyForUserIfNeeded();
    })
});