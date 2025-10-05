import React, { useEffect, useState } from "react";
import apiClient from "../api/apiClient";
import DataTable from "./DataTable";
import "../stylesheets/TableStyles.css";

interface Patient { id: number; name: string; dob: string; }

 const columns = ["Id", "Name", "DOB"];

const PatientsList: React.FC = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  useEffect(() => {
    apiClient.get("/patients").then(res => setPatients(res.data)).catch(console.error);
  }, []);
  return (
    <div>
      <h2>Patients</h2><hr/>
       {/*<ul>{patients.map(p => <li key={p.id}>{p.name} (DOB: {p.dob})</li>)}</ul> */}
      <DataTable columns={columns} data={patients} />
    </div>
  );
};

export default PatientsList;
