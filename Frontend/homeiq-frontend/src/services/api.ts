// src/services/api.ts
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5033/api", // sau alt URL al backendului
  headers: {
    "Content-Type": "application/json"
  }
});

export default api;