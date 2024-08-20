const apiUrl = 'https://localhost:7247'; // Replace with your actual API URL

// Check if user is logged in
if (!localStorage.getItem('user')) {
    window.location.href = 'login.html';
}

// Load Order Lists when the page loads
document.addEventListener('DOMContentLoaded', loadOrderLists);

// Create Order List function
function createOrderList() {
    const name = document.getElementById('order-list-name').value.trim();
    const description = document.getElementById('order-list-description').value.trim();

    if (!name) {
        alert('Order list name is required.');
        return;
    }

    const payload = { name, description };

    fetch(`${apiUrl}/api/orderlists`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to create order list');
        }
        return response.json();
    })
    .then(orderList => {
        alert('Order list created successfully');
        loadOrderLists();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while creating the order list.');
    });
}
function loadOrderLists() {
    fetch(`${apiUrl}/api/orderlists`)
    .then(response => {
        console.log('Response status:', response.status);
        if (!response.ok) {
            throw new Error('Failed to load order lists');
        }
        return response.json();
    })
    .then(orderLists => {
        console.log('Order Lists:', orderLists); // Log the order lists data
        const orderListContainer = document.getElementById('order-list-container');
        const orderListSelect = document.getElementById('order-list-select');
        orderListContainer.innerHTML = '';
        orderListSelect.innerHTML = '';

        orderLists.forEach(orderList => {
            // Create the card for each order list
            const card = document.createElement('div');
            card.className = 'card mask-custom mb-4';

            const cardBody = document.createElement('div');
            cardBody.className = 'card-body p-4 text-white';

            const header = document.createElement('div');
            header.className = 'text-center pt-3 pb-2';
            const img = document.createElement('img');
            img.src = 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-todo-list/check1.webp';
            img.alt = 'Check';
            img.width = 60;
            const title = document.createElement('h2');
            title.className = 'my-4';
            title.textContent = orderList.name;
            header.appendChild(img);
            header.appendChild(title);
            cardBody.appendChild(header);

            const desc = document.createElement('p');
            desc.textContent = orderList.description;
            cardBody.appendChild(desc);

            const table = document.createElement('table');
            table.className = 'table text-white mb-0';

            const thead = document.createElement('thead');
            const trHead = document.createElement('tr');
            trHead.innerHTML = `
                <th scope="col">Order Name</th>
                <th scope="col">Description</th>
                <th scope="col">Price</th>
                <th scope="col">Actions</th>`;
            thead.appendChild(trHead);
            table.appendChild(thead);

            const tbody = document.createElement('tbody');

            orderList.orders.forEach(order => {
                const trBody = document.createElement('tr');
                trBody.className = 'fw-normal';
                trBody.innerHTML = `
                    <td>${order.orderName}</td>
                    <td>${order.description}</td>
                    <td>$${order.price.toFixed(2)}</td>
                    <td>
                        <a href="#!" data-mdb-tooltip-init title="Done"><i class="fas fa-check fa-lg text-success me-3"></i></a>
                        <a href="#!" data-mdb-tooltip-init title="Remove" onclick="deleteOrder(${orderList.orderListId}, ${order.orderId})"><i class="fas fa-trash-alt fa-lg text-warning"></i></a>
                    </td>`;
                tbody.appendChild(trBody);
            });

            table.appendChild(tbody);
            cardBody.appendChild(table);
            card.appendChild(cardBody);
            orderListContainer.appendChild(card);

            // Update the select dropdown
            const option = document.createElement('option');
            option.value = orderList.orderListId;
            option.textContent = orderList.name;
            orderListSelect.appendChild(option);
        });
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while loading the order lists.');
    });
}


// Add Order function
function addOrder() {
    const orderListId = document.getElementById('order-list-select').value;
    const name = document.getElementById('order-name').value.trim();
    const description = document.getElementById('order-description').value.trim();
    const price = parseFloat(document.getElementById('order-price').value);

    if (!orderListId) {
        alert('Please select an order list.');
        return;
    }

    if (!name || isNaN(price)) {
        alert('Order name and valid price are required.');
        return;
    }

    const payload = { orderName: name, description, price };

    fetch(`${apiUrl}/api/orderlists/${orderListId}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to add order');
        }
        return response.json();
    })
    .then(order => {
        alert('Order added successfully');
        loadOrderLists();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while adding the order.');
    });
}

// Delete Order function
function deleteOrder(orderListId, orderId) {
    if (!confirm('Are you sure you want to delete this order?')) {
        return;
    }

    fetch(`${apiUrl}/api/orderlists/${orderListId}/orders/${orderId}`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to delete order');
        }
        alert('Order deleted successfully');
        loadOrderLists();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while deleting the order.');
    });
}

// Delete Order List function
function deleteOrderList(orderListId) {
    if (!confirm('Are you sure you want to delete this order list?')) {
        return;
    }

    fetch(`${apiUrl}/api/orderlists/${orderListId}`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to delete order list');
        }
        alert('Order list deleted successfully');
        loadOrderLists();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while deleting the order list.');
    });
}
