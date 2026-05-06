import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import CatalogPage from './pages/CatalogPage';
import BasketPage from './pages/BasketPage';
import CheckoutPage from './pages/CheckoutPage';
import OrdersPage from './pages/OrdersPage';

function App() {
 return (
    <Router>
      <nav>
        <Link to="/"> 🏠 Katalog </Link> | 
        <Link to="/basket"> 🛒 Koszyk </Link> | 
        <Link to="/orders"> 📦 Moje Zamówienia </Link>
      </nav>

      <Routes>
        <Route path="/" element={<CatalogPage />} />
        <Route path="/basket" element={<BasketPage />} />
        <Route path="/orders" element={<OrdersPage />} />
        <Route path="/checkout" element={<CheckoutPage />} />
      </Routes>
    </Router>
  );
}

export default App;
