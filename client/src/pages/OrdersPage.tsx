import { useEffect, useState } from 'react';
import gateway from '@/api/gateway';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

export default function OrdersPage() {
    const [orders, setOrders] = useState<any[]>([]);
    const userName = "pedro";

    useEffect(() => {
        gateway.get(`/Order/${userName}`)
            .then(res => setOrders(res.data))
            .catch(err => console.error("Błąd zamówień:", err));
    }, []);

    return (
        <div className="p-8 max-w-6xl mx-auto">
            <h1 className="text-3xl font-bold mb-8">Twoje Zamówienia</h1>
            
            <Card>
                <CardHeader>
                    <CardTitle>Historia zakupów</CardTitle>
                </CardHeader>
                <CardContent>
                    {orders.length === 0 ? (
                        <div className="text-center py-10 text-muted-foreground">
                            Brak zamówień w bazie danych.
                        </div>
                    ) : (
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead className="w-[200px]">ID Zamówienia</TableHead>
                                    <TableHead>Użytkownik</TableHead>
                                    <TableHead>Adres dostawy</TableHead>
                                    <TableHead className="text-right">Suma</TableHead>
                                    <TableHead className="text-center">Status</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {orders.map((order) => (
                                    <TableRow key={order.id}>
                                        <TableCell className="font-mono text-xs text-slate-500">
                                            {order.id}
                                        </TableCell>
                                        <TableCell className="font-medium">{order.userName}</TableCell>
                                        <TableCell>
                                            <div className="text-sm">
                                                <p>{order.addressLine}</p>
                                                <p className="text-xs text-muted-foreground">{order.country}</p>
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-right font-bold">
                                            {order.totalPrice} PLN
                                        </TableCell>
                                        <TableCell className="text-center">
                                            <Badge variant="outline" className="bg-green-50 text-green-700 border-green-200">
                                                Zrealizowane
                                            </Badge>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    )}
                </CardContent>
            </Card>
        </div>
    );
}