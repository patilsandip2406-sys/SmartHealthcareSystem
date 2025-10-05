import React, { useEffect, useState } from "react";
import { startConnection, sendMessage } from "../services/notificationservice";
import "./NotificationDemo.css";

const NotificationDemo: React.FC = () => {
  const [user, setUser] = useState("ReactUser");
  const [message, setMessage] = useState("");
  const [connected, setConnected] = useState(false);

  useEffect(() => {
  const initConnection = async () => {
    await startConnection();
    setConnected(true); // Only mark connected after start completes
  };
  initConnection();
}, []);

  const handleSend = async () => {
    if (!connected) {
    alert("SignalR not connected yet!");
    return;
  }
    await sendMessage(user, message);
    setMessage("");

      // 🔊 Fire event to refresh list
    window.dispatchEvent(new Event("refreshNotifications"));
  };
  

  return (
    <div className="p-4" style={{ padding: "0 20px" }}>
      <h3>Send Notification</h3>
      <input
        type="text"
        value={message}
        placeholder="Enter a message"
        onChange={(e) => setMessage(e.target.value)}
      />
      <button onClick={handleSend}>Send</button>
    </div>
  );
};

export default NotificationDemo;
