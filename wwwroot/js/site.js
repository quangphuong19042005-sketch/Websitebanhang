// Shopping Cart AJAX Functions
function addToCart(productId, quantity = 1) {
    $.ajax({
        url: '/ShoppingCart/AddToCart',
        type: 'POST',
        data: { productId: productId, quantity: quantity },
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (response) {
            if (response.success) {
                // Update Badge
                updateCartBadge(response.cartCount);
                
                // Refresh Mini Cart
                refreshMiniCart();
                
                // Show Notification
                Swal.fire({
                    title: 'Thành công!',
                    text: response.message,
                    icon: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                    toast: true,
                    position: 'top-end'
                });
            }
        },
        error: function () {
            Swal.fire({
                title: 'Lỗi!',
                text: 'Không thể thêm sản phẩm vào giỏ hàng.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    });
}

function updateCartBadge(count) {
    const badge = $('#cart-count-badge');
    if (count > 0) {
        badge.text(count).show();
    } else {
        badge.hide();
    }
}

function refreshMiniCart() {
    $.get('/ShoppingCart/GetCartDetails', function (response) {
        const container = $('#mini-cart-container');
        const totalSpan = $('#mini-cart-total');
        
        if (response.items && response.items.length > 0) {
            let html = '';
            // Show only first 5 items
            const displayItems = response.items.slice(0, 5);
            displayItems.forEach(item => {
                const img = item.imageUrl ? (item.imageUrl.startsWith('http') ? item.imageUrl : '/images/' + item.imageUrl) : '/images/default-product.png';
                html += `
                    <div class="d-flex align-items-center p-3 border-bottom mini-cart-item hover-bg-light transition">
                        <img src="${img}" alt="${item.name}" class="rounded object-fit-cover me-3" style="width: 45px; height: 45px; border: 1px solid #eee;">
                        <div class="flex-grow-1 min-w-0">
                            <p class="mb-0 text-truncate fw-bold text-dark small">${item.name}</p>
                            <span class="text-muted small">${item.quantity} x ${formatCurrency(item.price)} đ</span>
                        </div>
                        <div class="text-end ms-2">
                            <span class="text-danger fw-bold small">${formatCurrency(item.price * item.quantity)} đ</span>
                        </div>
                    </div>
                `;
            });
            
            if (response.items.length > 5) {
                html += `
                    <div class="p-2 text-center bg-light-subtle">
                        <span class="small text-muted">Và ${response.items.length - 5} sản phẩm khác...</span>
                    </div>
                `;
            }
            
            container.html(html);
            totalSpan.text(formatCurrency(response.total) + ' đ');
            
            // Ensure bottom action area is visible
            if ($('.mini-cart .bg-white.justify-content-between').length === 0) {
                 location.reload(); // Fallback for structural changes, but usually not needed if already rendered
            }
        } else {
            container.html(`
                <div class="text-center py-5">
                    <div class="opacity-25 mb-3">
                        <i class="fas fa-shopping-basket fa-4x"></i>
                    </div>
                    <p class="text-muted mb-0">Giỏ hàng của bạn đang trống</p>
                </div>
            `);
            $('.mini-cart .bg-white.justify-content-between').remove();
        }
    });
}

function formatCurrency(amount) {
    return amount.toLocaleString('vi-VN');
}

// Initialize Cart Badge on page load
$(document).ready(function () {
    $.get('/ShoppingCart/GetCartCount', function (response) {
        updateCartBadge(response.count);
    });
});
