import { useEffect, useState } from 'react';
import gateway from '@/api/gateway';
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { ShoppingCart } from "lucide-react";

export default function CatalogPage() {
    const [products, setProducts] = useState<any[]>([]);

    useEffect(() => {
        gateway.get('/Catalog').then(res => setProducts(res.data));
    }, []);

    const handleAddToCart = async (product: any) => {
        try {
            // Domyślny, pusty koszyk
            let currentBasket: any = { userName: "pedro", items: [] };

            // 1. Bezpieczna próba pobrania koszyka
            try {
                const res = await gateway.get('/Basket/pedro');
                if (res.data && res.data.items) {
                    currentBasket = res.data;
                }
            } catch (getErr: any) {
                // Jeśli dostaniemy 404, ignorujemy to (koszyk jest po prostu pusty w Redisie)
                // Jeśli to inny błąd (np. 500), to go logujemy
                if (getErr.response?.status !== 404) {
                    console.warn("Błąd pobierania koszyka inny niż 404:", getErr);
                }
            }
            
            // 2. Aktualizacja przedmiotów
            let updatedItems = currentBasket.items;
            const existingItem = updatedItems.find((i: any) => i.productId === product.id);

            if (existingItem) {
                existingItem.quantity += 1;
            } else {
                updatedItems.push({
                    quantity: 1,
                    price: product.price,
                    productId: product.id,
                    productName: product.name
                });
            }

            // Wyliczamy sumę (choć zazwyczaj backend i tak robi to sam)
            const newTotal = updatedItems.reduce((sum: number, item: any) => sum + (item.price * item.quantity), 0);

            // 3. Wysyłamy zaktualizowany koszyk (POST)
            await gateway.post('/Basket', {
                userName: "pedro",
                items: updatedItems,
                totalPrice: newTotal
            });

            alert(`Dodałeś ${product.name} do koszyka!`);
            
        } catch (err) {
            console.error("Błąd zapisu do koszyka:", err);
            alert("Nie udało się dodać produktu. Sprawdź F12 -> Network.");
        }
    };

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-8">Katalog Produktów</h1>
            <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
                {products.map(p => (
                    <Card key={p.id}>
                        <CardHeader>
                            <CardTitle>{p.name}</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="h-40 bg-slate-100 rounded mb-4 flex items-center justify-center text-slate-400">
                                <img 
                                    src={p.imageFile} 
                                    alt={p.name} 
                                    className="w-full h-full object-contain transition-transform hover:scale-105"
                                    onError={(e) => {
                                    // Jeśli link nie działa, pokaże placeholder
                                    (e.target as HTMLImageElement).src = 'https://placehold.co/400x300?text=Brak+Zdjecia';
                                }}
                              />
                            </div>
                            <p className="text-2xl font-bold">{p.price} PLN</p>
                        </CardContent>
                        <CardFooter>
                            <Button className="w-full" onClick={() => handleAddToCart(p)}>
                                <ShoppingCart className="mr-2 h-4 w-4" /> Dodaj
                            </Button>
                        </CardFooter>
                    </Card>
                ))}
            </div>
        </div>
    );
}