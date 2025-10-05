import React, { useState } from "react";
import axios from "axios";

const AIChat: React.FC = () => {
  const [input, setInput] = useState("");
  const [history, setHistory] = useState<string[]>([]);

  const send = async () => {
    if (!input.trim()) return;
    setHistory(prev => [...prev, `You: ${input}`]);
    try {
      const res = await axios.post("http://localhost:5000/ai/chat", { message: input });
      const ai = res.data?.response ?? "No response";
      setHistory(prev => [...prev, `AI: ${ai}`]);
    } catch (err) {
      console.error(err);
      setHistory(prev => [...prev, "Error contacting AI"]);
    }
    setInput("");
  };

  return (
    <div style={{ maxWidth: 700, margin: "0 auto", padding: 16 }}>
      <h3>AI Assistant</h3>
      <div style={{ border: "1px solid #ddd", height: 300, overflowY: "auto", padding: 10, background: "#fafafa" }}>
        {history.map((m, i) => <div key={i} style={{ marginBottom: 8 }}>{m}</div>)}
      </div>
      <div style={{ display: "flex", gap: 8, marginTop: 8 }}>
        <input style={{ flex: 1, padding: 8 }} value={input} onChange={e => setInput(e.target.value)} />
        <button onClick={send} style={{ padding: "8px 12px" }}>Send</button>
      </div>
    </div>
  );
};

export default AIChat;
