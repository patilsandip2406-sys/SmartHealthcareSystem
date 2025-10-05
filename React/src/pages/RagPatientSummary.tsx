import React, { useState } from "react";
import axios from "axios";

const RagPatientSummary: React.FC = () => {
  const [patientId, setPatientId] = useState<number | "">("");
  const [loading, setLoading] = useState(false);
  const [summary, setSummary] = useState<string | null>(null);
  const [usedDocs, setUsedDocs] = useState<any[]>([]);

  const fetchSummary = async () => {
    if (!patientId) return;
    setLoading(true);
    try {
      // Call via Ocelot: /ai/genai/patient-summary/{id} or direct RagService: /api/genai/patient-summary/{id}
      const res = await axios.get(`http://localhost:5000/ai/genai/ragPatientSummary/${patientId}`);
      setSummary(res.data.summary || res.data.response || res.data);
      setUsedDocs(res.data.usedDocs ?? res.data.UsedDocs ?? []);
    } catch (err) {
      console.error(err);
      setSummary("Error fetching summary");
    }
    setLoading(false);
  };

  return (
    <div style={{ padding: 20 }}>
      <h2>Patient Summary (RAG)</h2>
      <div style={{ display: "flex", gap: 8, marginBottom: 12 }}>
        <input
          type="number"
          value={patientId}
          onChange={(e) => setPatientId(e.target.value === "" ? "" : Number(e.target.value))}
          placeholder="Patient ID"
        />
        <button onClick={fetchSummary} disabled={loading}>
          {loading ? "Loading..." : "Get Summary"}
        </button>
      </div>

      {summary && (
        <div style={{ border: "1px solid #ddd", padding: 12, marginTop: 12 }}>
          <h3>AI Summary</h3>
          <pre style={{ whiteSpace: "pre-wrap" }}>{summary}</pre>
        </div>
      )}

      {usedDocs.length > 0 && (
        <div style={{ marginTop: 12 }}>
          <h4>Used Documents</h4>
          <ul>
            {usedDocs.map((d: any, i: number) => (
              <li key={i}><strong>Score:</strong> {d.score} — {d.document.content?.slice(0,200)}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default RagPatientSummary;
