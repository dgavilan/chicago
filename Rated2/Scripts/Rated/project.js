var Project = {
    SaveProject: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/AddProject",
            type: "POST",
            data: data,
        });
        
        jqxhr.done(function (data) {
            var jsonData = $.parseJSON(data);

            // Display entered values to UI
            $("#divProjectName").html(jsonData.ProjectName);
            $("#divProjectDescription").html(jsonData.ProjectDescription);
            $("#ProjectId").val(jsonData.ProjectId);

            $("#modalAddProject").modal('hide');
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    CancelSaveProject: function () {
        window.location = "/"; // NOTE: Redirect user to main page.
    },

    AddDetail: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/AddDetail",
            type: "POST",
            data: data,
        });

        jqxhr.done(function (data) {
            var newDetail = JSON.parse(data);

            var html = "<tr id='tr_" + newDetail.ProjectDetailId + "'>"
                + "<td style='background-color:#f9f9f9;width:5px'>" + newDetail.DetailItemNumber + "</td>"
                + "<td valign='top'>"
                + "<span id='span_projectDetailName_" + newDetail.ProjectDetailId + "' style='font-size:17px; font-weight:bold'>" + newDetail.ProjectDetailName + "</span>"
                + "<br />"
                + "<span id='span_projectDetailDescription_" + newDetail.ProjectDetailId + "'>" + newDetail.ProjectDetailDescription + "</span>"
                + "<br />"
                + "<span id='span_hoursToComplete_" + newDetail.ProjectDetailId + "'>" + newDetail.HoursToComplete + " hours to complete</span>"
                + "<br />"
                + "<br />"
                + "Created: " + newDetail.CreatedDate
                + "<br />"
                + "Modified: " + newDetail.ModifiedDate
                + "</td>"
                + "</tr>"
                + "<tr><td colspan='3' style='height:3px'></td></tr>";

            $("#spanItemDetailCount").html(newDetail.DetailItemNumber);
            $("#tblItems tr:last").after(html);

            alert("Record added");
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    UpdateDetail: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/UpdateDetail",
            type: "PUT",
            data: data,
        });

        jqxhr.done(function () {
            alert("Record updated");
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    DeleteDetail: function (projectId, detailId) {
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/Project/" + projectId + "/Detail/" + detailId,
            type: "DELETE",
        });

        jqxhr.done(function () {
            alert("Record deleted");
            $("#tr_" + detailId).remove();
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    ShowAddDetail: function () {
        Project.ClearDetailModal();

        $("#btnAddToProject").text("Add");
        $("#modalAddItem").modal("show");
    },

    ShowEditDetail: function (projectDetailId, detailName, detailDescription, hoursToComplete) {
        Project.ClearDetailModal();

        $("#ProjectDetailId").val(projectDetailId);
        $("#ProjectDetailName").val(detailName);
        $("#ProjectDetailDescription").val(detailDescription);
        $("#HoursToComplete").val(hoursToComplete);

        
        $("#btnAddToProject").text("Save");
        $("#modalAddItem").modal("show");
    },

    ClearDetailModal: function () {
        $("#ProjectDetailId").val("");
        $("#ProjectDetailName").val("");
        $("#ProjectDetailDescription").val("");
        $("#HoursToComplete").val("");
    },
};

function handleErrors(jqXHR) {
    alert("API ERROR: " + jqXHR.textStatus);
}

$(document).ready(function () {

    var itemDetailCount = 0;

    if(openAddProjectModal){
        $("#modalAddProject").modal('show');
    }

    $("#btnAssignReviewer").click(function () {
        $("#modalAssignReviewer").modal('show');
    });

    $("#btnStartTheProject").click(function () {
        $("#modalStartTheProject").modal('show');
    });

    $("#btnAddToProject").click(function () {

        var projectDetailId = $("#ProjectDetailId").val();
        var itemName = $("#ProjectDetailName").val();
        var itemDescription = $("#ProjectDetailDescription").val();
        var itemTimeToComplete = $("#HoursToComplete").val();

        if (projectDetailId === "") {
            var totalDetailCount = $("#TotalDetailCount").val();
            var nextDetailNumber = +totalDetailCount + 1;
            $("#DetailItemNumber").val(nextDetailNumber);

            var newDetail = Project.AddDetail();

            itemDetailCount += 1;

            //var html = "<tr id='tr_" + projectDetailId + "'>"
            //    + "<td style='background-color:#f9f9f9;width:5px'>" + itemDetailCount + "</td>"
            //    + "<td valign='top'>"
            //    + "<span id='span_projectDetailName_" + projectDetailId + "' style='font-size:17px; font-weight:bold'>" + itemName + "</span>"
            //    + "<br />"
            //    + "<span id='span_projectDetailDescription_" + projectDetailId + "'>" + itemDescription + "</span>"
            //    + "<br />"
            //    + "<span id='span_hoursToComplete_" + projectDetailId + "'>" + itemTimeToComplete + " hours to complete</span>"
            //    + "<br />"
            //    + "<br />"
            //    //+ "Created: " + newDetail.CreatedDate
            //    + "</td>"
            //    + "</tr>"
            //    + "<tr><td colspan='3' style='height:3px'></td></tr>";

            //$("#spanItemDetailCount").html(itemDetailCount);
            //$("#tblItems tr:last").after(html);
        }
        else {
            Project.UpdateDetail();

            $("#span_projectDetailName_" + projectDetailId).html(itemName);
            $("#span_projectDetailDescription_" + projectDetailId).html(itemDescription);
            $("#span_hoursToComplete_" + projectDetailId).html(itemTimeToComplete + " hours to complete");
        }
       
        $("#modalAddItem").modal('hide');
    });
        
});