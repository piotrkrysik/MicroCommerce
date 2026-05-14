import { useEffect, useState } from 'react';
import gateway from '@/api/gateway';
import { useNavigate } from 'react-router-dom';
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

export default function BasketPage() {
    const [basket, setBasket] = useState<any>(null);
    const navigate = useNavigate();

    useEffect(() => {
        gateway.get(`/Basket/pedro`).then(res => setBasket(res.data));
    }, []);

    if (!basket) return <div className="p-10">Ładowanie...</div>;

    return (
        <div className="p-8 max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold mb-6">Twój Koszyk</h1>
            <div className="border rounded-lg p-4 bg-white shadow-sm">
                <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHead>Produkt</TableHead>
                            <TableHead className="text-center">Sztuk</TableHead>
                            <TableHead className="text-right">Cena</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {basket.items.map((item: any) => (
                            <TableRow key={item.productId}>
                                <TableCell>{item.productName}</TableCell>
                                <TableCell className="text-center">{item.quantity}</TableCell>
                                <TableCell className="text-right">{item.price} PLN</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
                <div className="mt-6 flex justify-between items-center">
                    <span className="text-xl font-bold">Suma: {basket.totalPrice} PLN</span>
                    <Button onClick={() => navigate('/checkout')} size="lg">Kupuję i płacę</Button>
                </div>
            </div>
        </div>
    );
}