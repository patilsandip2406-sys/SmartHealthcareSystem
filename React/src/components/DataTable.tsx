import React from "react";

interface TableProps {
  columns: string[];
  data: Record<string, any>[];
}

const DataTable: React.FC<TableProps> = ({ columns, data }) => {
  console.log("DataTable Render", { columns, data });
  return (
    <div className="overflow-x-auto shadow-lg rounded-lg">
      <table className="min-w-full border border-gray-200 text-sm text-left">
        <thead className="bg-gray-100">
          <tr>
            {columns.map((col, index) => (
              <th
                key={index}
                className="px-6 py-3 border-b border-gray-200 font-semibold text-gray-700 uppercase tracking-wider"
              >
                {col}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="bg-white">
          {data.map((row, idx) => (
            <tr
              key={idx}
              className="hover:bg-blue-50 transition duration-150 ease-in-out"
            >
              {columns.map((col, i) => (
                <td
                  key={i}
                  className="px-6 py-3 border-b border-gray-200 text-gray-600"
                >
                  {row[col.toLowerCase()]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DataTable;
