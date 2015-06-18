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

        jqxhr.done(function () {
            //$("#modalAddProject").modal('hide');
        })
        jqxhr.fail(function (jqXHR, textStatus, errorThrown) { handleErrors(jqXHR) })
        jqxhr.always(function () { });
    },
};

function handleErrors(jqXHR) {
    alert(jqXHR.textStatus);
}

$(document).ready(function () {

    var itemDetailCount = 0;

    if(openAddProjectModal){
        $("#modalAddProject").modal('show');
    }

    $("#btnAddItem").click(function () {
        $("#modalAddItem").modal('show');
    });

    $("#btnAssignReviewer").click(function () {
        $("#modalAssignReviewer").modal('show');
    });

    $("#btnStartTheProject").click(function () {
        $("#modalStartTheProject").modal('show');
    });

    $("#btnAddToProject").click(function () {
        Project.AddDetail();

        var itemName = $("#ProjectDetailName").val();
        var itemDescription = $("#ProjectDetailDescription").val();
        var itemTimeToComplete = $("#TimeToComplete").val();
        //var itemDetails = $("#Details").val();

        itemDetailCount += 1;

        var html = "<tr>"
            + "<td style='background-color:#f9f9f9;width:5px'>" + itemDetailCount + "</td>"
            + "<td valign='top'>"
            + "<h4>" + itemName + "<button class='btn btn-link'>Edit</button>|<button class='btn btn-link'>Delete</button></h4>"
            + "<span>" + itemTimeToComplete + " to complete</span>"
            //+ "<br /><i>" + itemDescription + "</i>"
            + "<br /> <b>Details:</b> <br />" + itemDescription
            + "</td>"
            + "<td nowrap></td>"
            + "</tr>"
            + "<tr><td colspan='3' style='height:3px'></td></tr>";

        $("#spanItemDetailCount").html(itemDetailCount);
        $("#tblItems tr:last").after(html);
        $("#modalAddItem").modal('hide');
    });
        
});