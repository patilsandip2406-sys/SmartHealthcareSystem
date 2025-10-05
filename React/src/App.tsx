import React, { useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import Sidebar from "./components/Sidebar";
import Dashboard from "./pages/Dashboard";
import Users from "./pages/Users";
import Patients from "./pages/Patients";
import Claims from "./pages/Claims";
import Notifications from "./pages/Notifications";
import PatientSummary from "./pages/PatientSum";
import RagPatientSummary from "./pages/RagPatientSummary";
import ClaimsSummary from "./pages/ClaimsSummary";

import "./App.css";
import PatientSummaryNotes from "./components/PatientSumNotes";

const App: React.FC = () => {
  /*useEffect(() => { startConnection(); }, []);*/
  return (
    <Router>
      <div className="app-layout">
        <Header />
        <div className="content-layout">
          <Sidebar />
          <main className="content">
            <Routes>
              <Route path="/" element={<Dashboard />} />
              <Route path="/users" element={<Users />} />
              <Route path="/patients" element={<Patients />} />
              <Route path="/claims" element={<Claims />} />
              <Route path="/Claimssummary" element={<ClaimsSummary />} />
              <Route path="/notifications" element={<Notifications />} />
              <Route path="/patientsummary" element={<PatientSummary />} />
              <Route path="/ragpatientsummary" element={<RagPatientSummary />} />
              <Route path="/patientsummarynotes" element={<PatientSummaryNotes />} />
            </Routes>
          </main>
        </div>
      </div>
    </Router>
  );
};

export default App;
