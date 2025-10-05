import React, { useState } from "react";
import axios from "axios";
import apiClient from "../api/apiClient";

const PatientSummary: React.FC = () => {
  const [patientId, setPatientId] = useState<number | "">("");
  const [summary, setSummary] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchSummary = async () => {
    if (!patientId) return;
    setLoading(true);
    try {
      console.log("Fetching summary for patientId:", patientId);
      const res = await apiClient.get(`/ai/PatientSummary/${patientId}`);
      console.log("Received summary:", res.data);
      setSummary(res.data.summary);
    } catch (err) {
      console.error(err);
      setSummary("Error fetching summary");
    }
    setLoading(false);
  };

  return (
    <div style={{ maxWidth: 800, margin: 16 }}>
      <h3>Patient Summary (GenAI)</h3>
      <div style={{ display: "flex", gap: 8, marginBottom: 8 }}>
        <input
          type="number"
          placeholder="Patient Id"
          value={patientId}
          onChange={e => setPatientId(e.target.value === "" ? "" : Number(e.target.value))}
          style={{ padding: 8 }}
        />
        <button onClick={fetchSummary} disabled={loading} style={{ padding: "8px 12px" }}>
          {loading ? "Summarizing..." : "Get Summary"}
        </button>
      </div>
      {summary && (
        <div style={{ border: "1px solid #ddd", padding: 12, background: "#fff" }}>
          <pre style={{ whiteSpace: "pre-wrap" }}>{summary}</pre>
        </div>
      )}
    </div>
  );
};

export default PatientSummary;
