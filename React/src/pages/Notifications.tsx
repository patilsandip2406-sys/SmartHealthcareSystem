import React from "react";
import NotificationDemo from "../components/NotificationDemo";
import NotificationsList from "../components/NotificationsList";

const Notifications: React.FC = () => {
  return (
    <div>
      <h2>Notifications</h2> <hr/>
      <NotificationDemo />    
      <NotificationsList />
    </div>
  );
};

export default Notifications;
