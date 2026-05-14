import axios from 'axios';

const gateway = axios.create({
    baseURL: 'http://localhost:8010', // Port Twojego Ocelota
    headers: {
    'Content-Type': 'application/json'
  }
});

export default gateway;