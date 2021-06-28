function userEdit() {
    var edit = document.getElementById("editUserInfo");
    edit.classList.toggle("block_visibility_active");
    document.getElementById("controlUserEdit").classList.toggle("block_visibility_active");
    var parent = edit.parentElement;
    var call = document.getElementsByClassName("user-info");
    var i;
    for (i = 0; i < call.length; i++) {
        call[i].toggleAttribute("disabled");
        call[i].classList.toggle("form-control-active");
    }
    call = parent.getElementsByClassName("hidden");
    for (i = 0; i < call.length; i++) {
        call[i].classList.toggle("visible");
    }
    call = document.getElementById("profileControls").getElementsByTagName("button");
    for (i = 0; i < call.length; i++) {
        call[i].classList.toggle("disabled");
    }
}

function changeProfileView(e) {
    var i;
    var tag = e.id.replace("Button","");
    var tagBlock = document.getElementById(tag + "Block");
    var call = e.parentElement.getElementsByTagName("button");
    for (i = 0; i < call.length; i++) {
        ["no-hover", "btn-fill-secondary", "icon_color_secondary"].map(cls => call[i].classList.toggle(cls, false));
        ["btn-outline-primary", "icon_color_primary"].map(cls => call[i].classList.toggle(cls, true));
    }
    ["no-hover", "btn-fill-secondary", "icon_color_secondary"].map(cls => e.classList.toggle(cls, true));
    e.classList.toggle("btn-outline-primary", false);
    call = document.getElementsByClassName("profileContentBlock");
    for (i = 0; i < call.length; i++) {
        if (tag !== call[i].id.replace("Block","")) {
            call[i].classList.toggle("block_visibility_active", false);
            call[i].classList.toggle("block_padding_none", true);
        }
    }
    tagBlock.classList.toggle("block_visibility_active", true);
    tagBlock.classList.toggle("block_padding_none", false);
}

function searchFunction(input) {
    var filter, txtValue, elementsCollection, i;
    filter = input.value.toUpperCase();
    elementsCollection = document.getElementsByClassName("contentSearchTitle");
    for (i = 0; i < elementsCollection.length; i++) {
        var currentElement = elementsCollection[i];
        txtValue = currentElement.innerText || currentElement.textContent;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            currentElement.parentElement.style.display = "";
        }
        else {
            currentElement.parentElement.style.display = "none";
        }
    }
}

function calcHeight(value) {
    let numberOfLineBreaks = (value.match(/\n/g) || []).length;
    // min-height + lines x line-height + padding + border
    let newHeight = 20 + numberOfLineBreaks * 20 + 12 + 2;
    return newHeight;
}

let textarea = document.querySelector(".resize-ta");
if (textarea !== null) {
    textarea.addEventListener("keyup", () => {
        textarea.style.height = calcHeight(textarea.value) + "px";
    });
}

function requiredField(field) {
    if (field.value === '') {
        field.setCustomValidity('Это обязательное поле!');
        return true;
    }
}
