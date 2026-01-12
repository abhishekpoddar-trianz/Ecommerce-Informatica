// Ecommerce Informatica - Site JavaScript

// Document Ready
document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    initializeTooltips();

    // Initialize popovers
    initializePopovers();

    // Auto-hide alerts after 5 seconds
    autoHideAlerts();

    // Confirm delete actions
    initializeDeleteConfirmations();

    // Form validation enhancements
    enhanceFormValidation();

    // Search input focus
    focusSearchInput();

    // Calculate subtotals
    initializeSubtotalCalculation();
});

// Initialize Bootstrap tooltips
function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize Bootstrap popovers
function initializePopovers() {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
}

// Auto-hide alerts after 5 seconds
function autoHideAlerts() {
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
}

// Confirm delete actions
function initializeDeleteConfirmations() {
    const deleteButtons = document.querySelectorAll('button[type="submit"]');
    deleteButtons.forEach(function (button) {
        if (button.textContent.includes('Delete') || button.textContent.includes('Confirm')) {
            button.addEventListener('click', function (e) {
                if (!confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
                    e.preventDefault();
                    return false;
                }
            });
        }
    });
}

// Enhance form validation
function enhanceFormValidation() {
    const forms = document.querySelectorAll('form');
    forms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
}

// Focus search input on page load
function focusSearchInput() {
    const searchInput = document.querySelector('input[name="searchString"]');
    if (searchInput && window.location.search.indexOf('searchString') === -1) {
        searchInput.focus();
    }
}

// Calculate subtotals for invoice details
function initializeSubtotalCalculation() {
    const quantityInputs = document.querySelectorAll('input[name*="Quantity"]');
    const priceInputs = document.querySelectorAll('input[name*="UnitPrice"]');

    function calculateSubtotal() {
        quantityInputs.forEach(function (qtyInput, index) {
            const priceInput = priceInputs[index];
            const subtotalInput = document.querySelector('input[name*="Subtotal"]');

            if (qtyInput && priceInput && subtotalInput) {
                qtyInput.addEventListener('input', updateSubtotal);
                priceInput.addEventListener('input', updateSubtotal);

                function updateSubtotal() {
                    const quantity = parseFloat(qtyInput.value) || 0;
                    const price = parseFloat(priceInput.value) || 0;
                    const subtotal = quantity * price;
                    subtotalInput.value = subtotal.toFixed(2);
                }
            }
        });
    }

    if (quantityInputs.length > 0 && priceInputs.length > 0) {
        calculateSubtotal();
    }
}

// Format currency inputs
function formatCurrencyInput(input) {
    input.addEventListener('blur', function () {
        let value = parseFloat(this.value);
        if (!isNaN(value)) {
            this.value = value.toFixed(2);
        }
    });
}

// Apply currency formatting to all currency inputs
document.querySelectorAll('input[type="number"][step="0.01"]').forEach(function (input) {
    formatCurrencyInput(input);
});

// Loading spinner utility
function showLoadingSpinner() {
    const spinner = document.createElement('div');
    spinner.id = 'loadingSpinner';
    spinner.className = 'position-fixed top-50 start-50 translate-middle';
    spinner.innerHTML = `
        <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
            <span class="visually-hidden">Loading...</span>
        </div>
    `;
    document.body.appendChild(spinner);
}

function hideLoadingSpinner() {
    const spinner = document.getElementById('loadingSpinner');
    if (spinner) {
        spinner.remove();
    }
}

// Export utility functions
window.EcommerceInformatica = {
    showLoadingSpinner: showLoadingSpinner,
    hideLoadingSpinner: hideLoadingSpinner,
    formatCurrencyInput: formatCurrencyInput
};

// Smooth scroll for anchor links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            e.preventDefault();
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Print functionality
function printPage() {
    window.print();
}

// Export data to CSV (simple implementation)
function exportTableToCSV(tableId, filename) {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    for (let i = 0; i < rows.length; i++) {
        const row = [];
        const cols = rows[i].querySelectorAll('td, th');

        for (let j = 0; j < cols.length; j++) {
            row.push(cols[j].innerText);
        }

        csv.push(row.join(','));
    }

    downloadCSV(csv.join('\n'), filename);
}

function downloadCSV(csv, filename) {
    const csvFile = new Blob([csv], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}
