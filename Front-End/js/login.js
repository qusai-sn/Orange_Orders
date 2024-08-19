const apiUrl = 'https://localhost:7247'; // Ensure this is correct

function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    fetch(`${apiUrl}/api/Login`, { // Ensure the endpoint is correct
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    })
    .then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Login failed');
    })
    .then(user => {
        localStorage.setItem('user', JSON.stringify(user));
        window.location.href = 'orders.html';
    })
    .catch(error => alert(error.message));
}
