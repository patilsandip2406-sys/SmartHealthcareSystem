import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:5000", // Replace with your API base URL
  headers: {
    "Content-Type": "application/json",
  },
});

export default apiClient;