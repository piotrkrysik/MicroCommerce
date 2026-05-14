import { useState } from 'react';
import gateway from '@/api/gateway';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function CheckoutPage() {
    const [order, setOrder] = useState({
        userName: 'pedro', firstName: '', lastName: '', emailAddress: '',
        addressLine: '', country: '', cardName: '', cardNumber: '', 
        expiration: '', CVV: '', paymentMethod: 1
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            // UPEWNIJ SIĘ, ŻE TO JEST WŁAŚCIWY ENDPOINT (np. /Basket/Checkout)
            await gateway.post('/Basket/Checkout', order);
            alert("Zamówienie wysłane pomyślnie!");
            // Możesz tu dodać automatyczne przekierowanie do zamówień:
            // navigate('/orders');
        } catch (err) {
            console.error("Błąd Checkoutu:", err);
        }
    };

    return (
        <div className="p-8 flex justify-center">
            <Card className="w-full max-w-2xl">
                <CardHeader><CardTitle>Dane do zamówienia</CardTitle></CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div className="grid grid-cols-2 gap-4">
                            <Input placeholder="Imię" onChange={e => setOrder({...order, firstName: e.target.value})} />
                            <Input placeholder="Nazwisko" onChange={e => setOrder({...order, lastName: e.target.value})} />
                        </div>
                        <Input placeholder="Adres" onChange={e => setOrder({...order, addressLine: e.target.value})} />
                        <div className="grid grid-cols-2 gap-4 border-t pt-4">
                            <Input placeholder="Numer karty" onChange={e => setOrder({...order, cardNumber: e.target.value})} />
                            <Input placeholder="CVV" onChange={e => setOrder({...order, CVV: e.target.value})} />
                        </div>
                        <Button type="submit" className="w-full text-lg py-6">Złóż zamówienie</Button>
                    </form>
                </CardContent>
            </Card>
        </div>
    );
}