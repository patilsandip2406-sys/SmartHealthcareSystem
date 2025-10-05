import React, { useEffect, useState } from "react";
import { getNotifications, markAsRead } from "../services/notificationservice";
import "../stylesheets/TableStyles.css";

interface Notification {
  id: number;
  message: string;
  status: string;
  createdAt: string;
}


let isEmptyList = false;

const NotificationsList: React.FC = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);

  useEffect(() => {
    loadNotifications();

    const handleRefresh = () => {
        loadNotifications();
    };

    window.addEventListener("refreshNotifications", handleRefresh);

    return () => {
        window.removeEventListener("refreshNotifications", handleRefresh);
    };
  }, []);

  const loadNotifications = async () => {
    const data = await getNotifications();
    if (data.length === 0) isEmptyList = true;
    else isEmptyList = false;
    setNotifications(data);
  };

  const handleMarkAsRead = async (id: number) => {
    await markAsRead(id);
    loadNotifications(); // refresh list
  };

   
  return (
    <div className="p-4" style={{ padding: "0 20px" }}>
      <h3 className="text-xl font-bold mb-4">Notifications List</h3>
      <table>
        <thead>
          <tr>
            <th className="px-4 py-2">ID</th>
            <th className="px-4 py-4">Message</th>
            <th className="px-4 py-4">Status</th>
            <th className="px-4 py-4">Created At</th>
            <th className="px-4 py-2">Actions</th>
            </tr>
        </thead>
        <tbody>
        {  (isEmptyList) ? <tr><td>No notifications available.</td></tr> :
            notifications.map((n) => (
            <tr key={n.id} className={n.status === "Unread" ? "bg-yellow-100" : "bg-white"}>
              <td className="border px-4 py-2">{n.id}</td>
              <td className="border px-4 py-4">{n.message}</td>
                <td className="border px-4 py-4">{n.status}</td>
                <td className="border px-4 py-4">{new Date(n.createdAt).toLocaleString()}</td>
                <td className="border px-4 py-2">
                    {n.status === "Unread" && (
                    <button
                      onClick={() => handleMarkAsRead(n.id)}
                      className="px-3 py-1 bg-blue-600 text-white rounded"
                    >
                      Mark as Read
                    </button>
                    )}
                </td>
            </tr>
            ))}
        </tbody>
        </table>
      {/*<ul className="space-y-3">
        
        {   (isEmptyList) ? <li>No notifications available.</li> :
            notifications.map((n) => (
          <li
            key={n.id}
            className={`p-3 rounded-lg shadow ${
              n.status === "Unread" ? "bg-yellow-100" : "bg-gray-100"
            }`}
          >
            <div className="flex justify-between items-center">
              <span>{n.message}</span>
              {n.status === "Unread" && (
                <button
                  onClick={() => handleMarkAsRead(n.id)}
                  className="px-3 py-1 bg-blue-600 text-white rounded"
                >
                  Mark as Read
                </button>
              )}
            </div>
            <small className="text-gray-500">{n.status}</small>
          </li>
        ))}
      </ul>*/}
    </div>
  );
};

export default NotificationsList;
