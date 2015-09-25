var Project = {
    AddNewCompany: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/AddCompany",
            type: "POST",
            data: data,
        });

        jqxhr.done(function (data) {
            var newCompanyName = $("#Name").val();

            // DG: TODO: Add new company created to company drop down and select it
            $("#CompanyId").append($("<option></option>")
                           .attr("value", data)
                           .text(newCompanyName));

            $("#CompanyId").val(data);

            $("#modalAddCompany").modal('hide');
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    AddNewProject: function () {
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

            window.location = "Project/Edit/" + jsonData.ProjectId + "/?ProjectStatusId=102";

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

            var html = "<tr id='tr_" + newDetail.TaskId + "'>"
                + "<td style='background-color:#f9f9f9;width:5px'>" + newDetail.DetailItemNumber + "</td>"
                + "<td valign='top'>"
                + "<span id='span_projectDetailName_" + newDetail.TaskId + "' style='font-size:17px; font-weight:bold'>" + newDetail.Name + "</span>"
                + "<br />"
                + "<span id='span_projectDetailDescription_" + newDetail.TaskId + "'>" + newDetail.Description + "</span>"
                + "<br />"
                + "<span id='span_hoursToComplete_" + newDetail.TaskId + "'>" + newDetail.HoursToComplete + " hours to complete</span>"
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

    ShowEditDetail: function (projectDetailId, detailName, detailDescription, hoursToComplete, reviewInstructions) {
        Project.ClearDetailModal();

        $("#TaskId").val(projectDetailId);
        $("#Name").val(detailName);
        $("#Description").val(detailDescription);
        $("#HoursToComplete").val(hoursToComplete);
        $("#ReviewInstructions").val(reviewInstructions);

        
        $("#btnAddToProject").text("Save");
        $("#modalAddItem").modal("show");
    },

    ClearDetailModal: function () {
        $("#TaskId").val("");
        $("#Name").val("");
        $("#Description").val("");
        $("#HoursToComplete").val("");
        $("#ReviewInstructions").val("");
    },

    AssignReviewer: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/Project/Reviewer",
            type: "POST",
            data: data,
        });

        jqxhr.done(function () {
            alert("Reviewer assigned");
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    AssignDetailReviewer: function (projectDetailId) {
        $("#TaskId").val(projectDetailId);
        $("#modalAssignReviewer").modal('show');
    },

    StartTheProject: function (projectId) {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            //url: "/api/ProjectApi/Project/" + projectId + "/StartTheProject",
            url: "/api/ProjectApi/Project/StartTheProject",
            type: "PUT",
            data: data,
        });

        jqxhr.done(function () {
            alert("Project started");
            $("#modalAddProject").modal('hide');
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    ReviwerAcceptsProject: function (projectId) {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            //url: "/api/ProjectApi/Project/" + projectId + "/ReviewerAccepted",
            url: "/api/ProjectApi/Project/ReviewerAccepted",
            type: "PUT",
            data: data,
        });

        jqxhr.done(function () {
            $("#modalConfirmReviewerAccept").modal("show");
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });

    },

    ConfirmProjectDetailComplete: function (projectDetailId) {
        var projectDetailId = $("#TaskId").val(projectDetailId);
        $("#modalProjectDetail").modal("show");
    },
    MarkProjectDetailAsComplete: function () {
        var projectId = $("#ProjectId").val();
        var projectDetailId = $("#TaskId").val();

        var jqxhr = $.ajax({
            url: "/api/ProjectApi/Project/" + projectId + "/ProjectDetail/" + projectDetailId + "/Complete",
            type: "PUT",
            //data: data,
        });

        jqxhr.done(function () {
            alert("Task marked as complete");
            $("#modalProjectDetail").modal("hide");
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },
    ShowReviewInstructions: function (projectDetailId) {
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/ProjectDetail/" + projectDetailId,
            type: "GET",
        });

        jqxhr.done(function (data) {
            $("#modalReviewInstructionsText").html(data.ReviewInstructions);
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });

        $("#modalReviewInstructions").modal("show");
    },

    ReviewerAcceptsProjectDetail: function (projectDetailId) {
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/ProjectDetail/" + projectDetailId + "/ReviewerAcceptsProjectDetail",
            type: "PUT",
        });

        jqxhr.done(function () {
            alert("Reviewer accepted successfully.");
            window.location.reload(true);
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },

    ReviewerDeclinesProjectDetail: function (projectDetailId) {

    },

    DetailReviewComplete: function (projectDetailId) {
        $("#TaskId").val(projectDetailId);
        $("#modalReviewComplete").modal("show");
    },

    SubmitReview: function () {
        var data = $(":input").serializeArray();
        var jqxhr = $.ajax({
            url: "/api/ProjectApi/ProjectDetail/SubmitReview",
            type: "PUT",
            data: data,
        });

        jqxhr.done(function () {
            alert("Review complete");
            window.location.reload(true);
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },
};

function handleErrors(jqXHR) {
    alert("API ERROR: " + jqXHR.textStatus);
}

$(document).ready(function () {

    var itemDetailCount = 0;

    //if(openAddProjectModal){
    //    $("#modalAddProject").modal('show');
    //}

    $("#btnConfirmCompleteTask").click(function () {

    });

    $("#btnReviewerAcceptsRequest").click(function () {
        var projectId = $("#ProjectId").val();
        Project.ReviwerAcceptsProject(projectId)
    });

    $("#btnOpenModalStartProject").click(function () {
        $("#modalAddProject").modal('show');
    });

    $("#btnAssignReviewer").click(function () {
        $("#modalAssignReviewer").modal('show');
    });

    $("#btnOpenStartProjectModal").click(function () {
        $("#modalStartTheProject").modal('show');
    });

    $("#btnStartProject").click(function () {
        Project.StartTheProject($("#ProjectId").val());
    });

    $("#btnAddToProject").click(function () {

        var projectDetailId = $("#TaskId").val();
        var itemName = $("#Name").val();
        var itemDescription = $("#Description").val();
        var itemTimeToComplete = $("#HoursToComplete").val();

        if (projectDetailId === "") {
            Project.AddDetail();
        }
        else {
            Project.UpdateDetail();
            $("#span_projectDetailName_" + projectDetailId).html(itemName);
            $("#span_projectDetailDescription_" + projectDetailId).html(itemDescription);
            $("#span_hoursToComplete_" + projectDetailId).html(itemTimeToComplete + " hours to complete");
        }
       
        $("#modalAddItem").modal('hide');
    });

    $("#btnOpenEditProject").click(function () {
        $("#modalEditProject").modal("show");
    });

    $("#btnAddNewComapany").click(function () {
        $("#modalAddCompany").modal("show");
    });
    
});