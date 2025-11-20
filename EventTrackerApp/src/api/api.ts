import axios, { type AxiosInstance } from "axios";

const api: AxiosInstance = axios.create({
  baseURL: "https://localhost:7026/api",
  withCredentials: true,
});

export default api;
