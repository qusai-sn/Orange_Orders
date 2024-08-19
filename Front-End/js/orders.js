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

    fetch(`${apiUrl}/api/orderlists`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, description })
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

// Load Order Lists function
function loadOrderLists() {
    fetch(`${apiUrl}/api/OrderLists`)
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to load order lists');
        }
        return response.json();
    })
    .then(orderLists => {
        const orderListContainer = document.getElementById('order-list-container');
        const orderListSelect = document.getElementById('order-list-select');
        orderListContainer.innerHTML = '';
        orderListSelect.innerHTML = '';

        orderLists.forEach(orderList => {
            const li = document.createElement('li');
            li.textContent = orderList.name;
            orderListContainer.appendChild(li);

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

    fetch(`${apiUrl}/api/orderlists/${orderListId}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ orderName: name, description, price })
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
