document.addEventListener('DOMContentLoaded', function () {
    const list = document.querySelectorAll('.list .item');

    list.forEach(item => {
        item.addEventListener('click', function (event) {
            if (event.target.classList.contains('add')) {
                let selectedSizeInput = item.querySelector('input[type=radio]:checked');
                if (selectedSizeInput) {
                    let product = {
                        IdTU: item.getAttribute('data-key'),
                        Size: selectedSizeInput.value,
                        TenTU: item.querySelector('.title').textContent,
                        Gia: selectedSizeInput.dataset.price,
                        SoLuong: parseInt(item.querySelector('.count').value)
                    };

                    addToCart(product);
                } else {
                    alert('Vui lòng chọn size trước khi thêm vào giỏ hàng.');
                }
            } else if (event.target.classList.contains('remove')) {
                const keyToRemove = item.getAttribute('data-key');
                const sizeToRemove = item.querySelector('input[type=radio]:checked').value;
                Remove(keyToRemove, sizeToRemove);
            }
        });
    });

    function addToCart(product) {
        let cartItems = document.querySelectorAll('.cart .item');
        let existingItem = null;

        cartItems.forEach(cartItem => {
            if (
                cartItem.getAttribute('data-key') === product.IdTU &&
                cartItem.getAttribute('data-size') === product.Size
            ) {
                existingItem = cartItem;
            }
        });

        if (existingItem) {
            // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng và thông tin chi tiết sản phẩm
            let currentQuantity = parseInt(existingItem.querySelector('.count').value);
            let newQuantity = currentQuantity + product.SoLuong;
            existingItem.querySelector('.count').value = newQuantity;
            // Cập nhật thông tin chi tiết sản phẩm tại đây nếu cần
        } else {
            // Nếu sản phẩm chưa tồn tại trong giỏ hàng, thêm sản phẩm mới
            let itemNew = item.cloneNode(true);
            let selectedSizeInput = itemNew.querySelector('input[type=radio]:checked');

            itemNew.querySelectorAll('input[type=radio]').forEach(input => {
                input.closest('label').style.display = 'none';
            });

            itemNew.querySelector('input[type=radio]').removeAttribute('disabled');
            itemNew.querySelector('input[type=number]').value = product.SoLuong; // Cập nhật số lượng vào sản phẩm mới
            itemNew.querySelector('.add').classList.add('disabled');

            itemNew.setAttribute('data-size', product.Size);

            let selectedSizeInfo = selectedSizeInput.nextSibling.nodeValue.trim();
            itemNew.querySelector('.sizes').innerHTML = selectedSizeInfo;

            let checkIsset = false;

            let listCart = document.querySelectorAll('.cart .item');
            listCart.forEach(cart => {
                if (
                    cart.getAttribute('data-key') === itemNew.getAttribute('data-key') &&
                    cart.getAttribute('data-size') === itemNew.getAttribute('data-size')
                ) {
                    checkIsset = true;
                    cart.classList.add('danger');
                    setTimeout(function () {
                        cart.classList.remove('danger');
                    }, 1000);
                }
            });

            if (!checkIsset) {
                document.querySelector('.listCart').appendChild(itemNew);
            }
        }
    }

    function Remove(key, size) {
        const cartItems = document.querySelectorAll('.cart .item');

        cartItems.forEach(cartItem => {
            const cartKey = cartItem.getAttribute('data-key');
            const cartSize = cartItem.getAttribute('data-size');

            if (cartKey === key && cartSize === size) {
                cartItem.remove();
            }
        });
    }
});
