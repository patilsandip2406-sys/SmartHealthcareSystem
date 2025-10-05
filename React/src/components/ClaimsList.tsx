import React, { useEffect, useState } from "react";
import apiClient from "../api/apiClient";
import "../stylesheets/TableStyles.css";
import DataTable from "./DataTable";

interface Claim { id: number; name: string; amount: number; diagnosis: string; procedure: string; status: string; }

 const columns = ["Id", "Name", "Amount", "Diagnosis", "Procedure", "Status"];
 const ClaimsList: React.FC = () => {
  const [claims, setClaims] = useState<Claim[]>([]);
  useEffect(() => {
    apiClient.get("/claims").then(res => setClaims(res.data)).catch(console.error);
  }, []);
  return (
      <div>
        <h2>Claims</h2><hr/>
        {/* <ul>{claims.map(c => <li key={c.id}>Patient: {c.patientId}, Amount: {c.amount}, Status: {c.status}</li>)}</ul> */}
      
        <DataTable columns={columns} data={claims} />
    </div>
  );
};

export default ClaimsList;
