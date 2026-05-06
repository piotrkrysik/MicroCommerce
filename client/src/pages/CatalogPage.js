import React, { useEffect, useState } from 'react';
import gateway from '../api/gateway';

const CatalogPage = () => {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        gateway.get('/Catalog')
            .then(res => setProducts(res.data))
            .catch(err => console.error("Błąd katalogu:", err));
    }, []);

    return (
        <div>
            <h1>Katalog Produktów</h1>
            <ul>
                {products.map(p => (
                    <li key={p.id}>{p.name} - {p.price} PLN</li>
                ))}
            </ul>
        </div>
    );
};

export default CatalogPage;