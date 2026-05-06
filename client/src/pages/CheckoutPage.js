import React, { useState } from 'react';
import gateway from '../api/gateway';

const CheckoutPage = () => {
    const [order, setOrder] = useState({
        userName: 'pedro', // Na razie na sztywno
        firstName: '', lastName: '', emailAddress: '',
        addressLine: '', country: '',
        cardName: '', cardNumber: '', expiration: '', CVV: '',
        paymentMethod: 1
    });

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            // Strzał do Ocelota na endpoint Basket Checkout
            await gateway.post('/Basket', order);
            alert("Zamówienie wysłane! Sprawdź logi Ordering.API");
        } catch (err) {
            console.error("Błąd Checkoutu:", err);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Dane wysyłki</h2>
            <input placeholder="Imię" onChange={e => setOrder({...order, firstName: e.target.value})} />
            <input placeholder="Nazwisko" onChange={e => setOrder({...order, lastName: e.target.value})} />
            
            <h2>Płatność</h2>
            <input placeholder="Numer karty" onChange={e => setOrder({...order, cardNumber: e.target.value})} />
            <input placeholder="CVV" onChange={e => setOrder({...order, CVV: e.target.value})} />
            
            <button type="submit">Złóż zamówienie</button>
        </form>
    );
};

export default CheckoutPage;