// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


document.addEventListener("DOMContentLoaded", function () {
    const qtyInput = document.getElementById('product-quantity');
    const hiddenQty = document.getElementById('hidden-quantity');
    const increaseBtn = document.getElementById('increase-qty');
    const decreaseBtn = document.getElementById('decrease-qty');

    if (increaseBtn && decreaseBtn && qtyInput) {
        increaseBtn.addEventListener('click', () => {
            qtyInput.value = parseInt(qtyInput.value) + 1;
            hiddenQty.value = qtyInput.value;
        });

        decreaseBtn.addEventListener('click', () => {
            if (parseInt(qtyInput.value) > 1) {
                qtyInput.value = parseInt(qtyInput.value) - 1;
                hiddenQty.value = qtyInput.value;
            }
        });
    }
});

// ---  NAVIGATION ---
document.addEventListener("DOMContentLoaded", function () {

    const tabLinks = document.querySelectorAll('a[data-bs-toggle="tab"]');
    if (tabLinks.length > 0) {
        tabLinks.forEach(link => {
            link.addEventListener('click', function (e) {
                e.preventDefault();

                document.querySelectorAll('.profile-nav-item').forEach(li => li.classList.remove('active'));
                this.parentElement.classList.add('active');

                document.querySelectorAll('.tab-pane').forEach(pane => pane.classList.remove('show', 'active'));
                const targetId = this.getAttribute('href');
                const targetPane = document.querySelector(targetId);
                if (targetPane) {
                    targetPane.classList.add('show', 'active');
                }
            });
        });
    }

    //--- TOGGLE PASSWORD - CHANGE PASSWORD FORM ---
    const toggleButtons = document.querySelectorAll(".toggle-password");
    if (toggleButtons.length > 0) {
        toggleButtons.forEach(btn => {
            btn.addEventListener("click", function () {
                const targetId = this.getAttribute("data-target");
                const input = document.getElementById(targetId);
                const icon = this.querySelector("i");

                if (input && input.type === "password") {
                    input.type = "text";
                    icon.classList.replace("fa-eye", "fa-eye-slash");
                } else if (input) {
                    input.type = "password";
                    icon.classList.replace("fa-eye-slash", "fa-eye");
                }
            });
        });
    }

    const passError = document.querySelector("#password-tab .text-danger");
    if (passError && passError.innerText.trim() !== "") {
        const passTabBtn = document.querySelector('a[href="#password-tab"]');
        if (passTabBtn) passTabBtn.click();
    }
});

/* --- sorting categories --- */
function filter() {
    const catElement = document.getElementById("catFilter");
    const sortElement = document.getElementById("sortFilter");

    if (catElement && sortElement) {
        const cat = catElement.value;
        const sort = sortElement.value;

        window.location.href = `/Store?category=${encodeURIComponent(cat)}&sort=${sort}`;
    }
}