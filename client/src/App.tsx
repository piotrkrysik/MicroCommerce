import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import CatalogPage from './pages/CatalogPage';
import BasketPage from './pages/BasketPage';
import CheckoutPage from './pages/CheckoutPage';
import OrdersPage from './pages/OrdersPage';

function App() {
  return (
    <Router>
      <nav className="p-4 bg-white border-b flex gap-6">
        <Link to="/" className="font-bold hover:text-primary text-slate-800">Sklep</Link>
        <Link to="/basket" className="hover:text-primary text-slate-600">Koszyk</Link>
        <Link to="/orders" className="hover:text-primary text-slate-600">Moje zamówienia</Link>
      </nav>

      <Routes>
        <Route path="/" element={<CatalogPage />} />
        <Route path="/basket" element={<BasketPage />} />
        <Route path="/checkout" element={<CheckoutPage />} />
        <Route path="/orders" element={<OrdersPage />} />
      </Routes>
    </Router>
  );
}

export default App;