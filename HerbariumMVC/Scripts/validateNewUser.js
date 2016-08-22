function validateNewUser(form) {
    var errors = [];

    if (form.userName.value == "" || form.userName.value == null) {
        errors.push("Username required");
    }

    if (form.fName.value == "" || form.fName.value == null) {
        errors.push("First Name required");
    }

    if (form.lName.value == "" || form.lName.value == null) {
        errors.push("Last Name required");
    }


    if (errors.length > 0) {
        $("#errors").html("");
        for (var i = 0; i < errors.length; i++) {
            $("#errors").append(errors[i] + "<br />");
            console.log(errors[i]);
        }
        return false;
    }
    else {
        return true;
    }
}