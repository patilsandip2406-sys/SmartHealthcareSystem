import React, { useState } from "react";

const ClaimsSummary: React.FC = () => {
  const [summary, setSummary] = useState("");
  const [loading, setLoading] = useState(false);

  const fetchSummary = async () => {
    setLoading(true);
    const res = await fetch("https://localhost:5000/claims/1/summary"); 
    console.log("Received Result:", res);
    const data = await res.json();
    console.log("Received data:", data.answer);
    setSummary(data.choices ? data.choices[0].message.content : JSON.stringify(data.answer));
    setLoading(false);
  };

  return (
    <div className="p-6">
      <h1 className="text-xl font-bold mb-4">Claim Summary</h1>
      <button
        onClick={fetchSummary}
        className="bg-blue-600 text-white px-4 py-2 rounded"
      >
        {loading ? "Loading..." : "Get Summary"}
      </button>
      {summary && (
        <div className="mt-4 p-4 bg-gray-100 rounded shadow">
          <p>{summary}</p>
        </div>
      )}
    </div>
  );
};

export default ClaimsSummary;
