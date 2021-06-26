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
    ["ordersBlock", "worksBlock", "userInfoBlock"].map(el => {
        var block = document.getElementById(el);
        if (tag !== el.replace("Block","")) {
            block.classList.toggle("block_visibility_active", false);
            block.classList.toggle("block_padding_none", true);
        }
    });
    tagBlock.classList.toggle("block_visibility_active", true);
    tagBlock.classList.toggle("block_padding_none", false);
}