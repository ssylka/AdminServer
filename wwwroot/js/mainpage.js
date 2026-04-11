document.addEventListener("DOMContentLoaded", () => { // Else DOM elements may not be available when this script runs


    const selectAll = document.getElementById("selectAll");
    const checkboxes = document.querySelectorAll(".user-checkbox");

    selectAll.addEventListener("change", function () { // Toggle all checkboxes based on "Select All"
        checkboxes.forEach(cb => cb.checked = selectAll.checked);
    });

    function getSelectedUserIds() {  // Get IDs of all selected users
        return Array.from(document.querySelectorAll(".user-checkbox:checked"))
            .map(cb => cb.value);
    }

    document.getElementById("btnDelete").addEventListener("click", async () => {
        const ids = getSelectedUserIds();

        await fetch('/User/DeleteSelected', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(ids)
        });

        location.reload();
    });

    document.getElementById("btnBlock").addEventListener("click", async () => {
        const ids = getSelectedUserIds();

        await fetch('/User/BlockSelected', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(ids)
        });

        location.reload();
    });

    document.getElementById("btnUnblock").addEventListener("click", async () => {
        const ids = getSelectedUserIds();

        await fetch('/User/UnblockSelected', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(ids)
        });

        location.reload();
    });

    document.getElementById("btnDeleteUnverified").addEventListener("click", async () => {

        await fetch('/User/DeleteUnverified', {
            method: 'POST'
        });

        location.reload();
    });

    function updateButtons() { // Enable/disable action buttons based on whether any users are selected
        const hasSelected = getSelectedUserIds().length > 0;

        document.getElementById("btnBlock").disabled = !hasSelected;
        document.getElementById("btnUnblock").disabled = !hasSelected;
        document.getElementById("btnDelete").disabled = !hasSelected;
    }
    document.getElementById("selectAll").addEventListener("change", function () { // for chekbox "Select all"
        const checkboxes = document.querySelectorAll(".user-checkbox");

        checkboxes.forEach(cb => cb.checked = this.checked);

        updateButtons();
    });

    document.querySelectorAll(".user-checkbox").forEach(cb => {
        cb.addEventListener("change", updateButtons);
    });

    document.querySelectorAll(".user-checkbox").forEach(cb => { // disable chekbox "Select all" if any one is not active
        cb.addEventListener("change", () => {
            const all = document.querySelectorAll(".user-checkbox");
            const checked = document.querySelectorAll(".user-checkbox:checked");

            document.getElementById("selectAll").checked = all.length === checked.length;

            updateButtons();
        });
    });

    updateButtons();

});