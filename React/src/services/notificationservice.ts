import * as signalR from "@microsoft/signalr";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import axios from "axios";

let connection: signalR.HubConnection | null = null;
let isConnected = false;

console.log("SignalR state:", connection);

export const startConnection = async () => {
    if (connection && connection.state === signalR.HubConnectionState.Connected) {
    return connection;
  }
  
  console.log("Starting SignalR connection...");
  
  connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5004/notifications", {
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,    
        withCredentials: true, // Required if using cookies/auth
    })
    .withAutomaticReconnect()
    .build();

  console.log("Connections! " + connection.baseUrl, connection.state, connection.connectionId);

  connection.on("ReceiveMessage", (user: string, message: string) => {
   // alert(`${user}: ${message}`);
    
    console.log("Message received:", user, message);
  });

  connection.onclose(() => {
    console.log("Connection closed");
    isConnected = false;
  });
  
  connection.onreconnecting((error) => {
    console.warn("SignalR reconnecting:", error);
  });

  connection.onreconnected((connectionId) => {
    console.log("SignalR reconnected with ID:", connectionId);
  });  

  try {
    console.log("Before, StartAsync");
    await connection.start();
    console.log("SignalR Connected.");
    isConnected = true;
    //alert(`${isConnected}`);
  } catch (err) {
    alert(`${err}`);
    console.error("SignalR Connection Error: ", err);
  }
};

export const sendMessage = async (user: string, message: string) => {
    if (!connection) {
    console.warn("Connection object not initialized");
    return;
  }
   if (connection.state !== signalR.HubConnectionState.Connected) {
    console.warn("Cannot send: connection not connected");
    return;
  }

  if (connection && isConnected) {
    console.log("SignalR state:", connection.state);
    await connection.invoke("SendMessage", user, message);
    getNotifications(); // refresh list
  }  
};

const API_BASE = "https://localhost:5004/api/notifications";

export async function getNotifications() {
    const response = await axios.get(API_BASE);
    return response.data;
}

export async function markAsRead(id: number) {
    const response = await axios.post(`${API_BASE}/${id}/mark-read`);
    return response.data;
}
