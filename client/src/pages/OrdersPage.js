import React, { useEffect, useState } from 'react';
import gateway from '../api/gateway';

const OrdersPage = () => {
    const [orders, setOrders] = useState([]);
    const userName = "pedro";

    useEffect(() => {
        gateway.get(`/Order/${userName}`)
            .then(res => setOrders(res.data))
            .catch(err => console.error("Błąd zamówień:", err));
    }, []);

    return (
        <div>
            <h1>Twoje Zamówienia</h1>
            {orders.length === 0 ? <p>Brak zamówień w bazie.</p> : (
                <table border="1">
                    <thead>
                        <tr>
                            <th>ID Zamówienia</th>
                            <th>Data</th>
                            <th>Suma</th>
                            <th>Adres</th>
                        </tr>
                    </thead>
                    <tbody>
                        {orders.map(order => (
                            <tr key={order.id}>
                                <td>{order.id}</td>
                                <td>{order.userName}</td> {/* Możesz dodać datę jeśli masz w modelu */}
                                <td>{order.totalPrice} PLN</td>
                                <td>{order.addressLine}, {order.country}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default OrdersPage;