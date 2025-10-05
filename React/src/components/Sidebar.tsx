import React from "react";
import { Link } from "react-router-dom";
import "./Sidebar.css";

const Sidebar: React.FC = () => {
  return (
    <aside className="sidebar">
      <nav>
        <ul>
          <li><Link to="/">Dashboard</Link></li>
          <li><Link to="/patients">Patients</Link></li>
          <li><Link to="/Users">Users</Link></li>
          <li><Link to="/Claims">Claims</Link></li>
          <li><Link to="/Claimssummary">Claims Summary</Link></li>
          <li><Link to="/notifications">Notifications</Link></li>
          <li><Link to="/patientsummary">Patient Summary</Link></li> 
          <li><Link to="/ragpatientsummary">Rag Patient Summary</Link></li> 
          <li><Link to="/patientsummarynotes">Patient Notes</Link></li>
        </ul>
      </nav>
    </aside>
  );
};

export default Sidebar;
