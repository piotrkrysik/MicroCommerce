import React, { useEffect, useState } from 'react';
import gateway from '../api/gateway';
import { useNavigate } from 'react-router-dom';

const BasketPage = () => {
    const [basket, setBasket] = useState(null);
    const userName = "pedro"; // Stały użytkownik dla testów
    const navigate = useNavigate();

    useEffect(() => {
        gateway.get(`/Basket/${userName}`)
            .then(res => setBasket(res.data))
            .catch(err => console.error("Błąd koszyka:", err));
    }, []);

    if (!basket) return <div>Ładowanie koszyka...</div>;

    return (
        <div>
            <h1>Twój Koszyk (Użytkownik: {basket.userName})</h1>
            {basket.items.length > 0 ? (
                <>
                    <ul>
                        {basket.items.map(item => (
                            <li key={item.productId}>
                                {item.productName} - {item.price} PLN (Sztuk: {item.quantity})
                            </li>
                        ))}
                    </ul>
                    <h3>Suma: {basket.totalPrice} PLN</h3>
                    <button onClick={() => navigate('/checkout')}>Przejdź do płatności</button>
                </>
            ) : <p>Koszyk jest pusty</p>}
        </div>
    );
};

export default BasketPage;