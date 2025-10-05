import React, { useState } from "react";
import axios from "axios";
import apiClient from "../api/apiClient";

export const PatientSummaryNotes: React.FC = () => {
  const [pid, setPid] = useState<number>(1);
  const [query, setQuery] = useState<string>("latest health status");
  const [summary, setSummary] = useState<string>("");

  const fetchSummary = async () => {
    const res = await apiClient.get(`/ai/genai/ragPatientNotes/${pid}?query=${encodeURIComponent(query)}`)
      .then(res => setSummary(res.data.answer)).catch(console.error);;
    ;
  };

  return (
    <div>
      <h2>Patient Summary (GenAI)</h2>
      <input type="number" value={pid} onChange={e => setPid(Number(e.target.value))} />
      <input type="text" value={query} onChange={e => setQuery(e.target.value)} />
      <button onClick={fetchSummary}>Get Summary</button>
      <pre>{summary}</pre>
    </div>
  );
};
export default PatientSummaryNotes;