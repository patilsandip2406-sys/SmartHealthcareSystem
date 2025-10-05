import React, { useEffect, useState } from "react";
import apiClient from "../api/apiClient";
import "../stylesheets/TableStyles.css";
import DataTable from "./DataTable";

interface User { id: number; name: string; email: string; }
 const columns = ["Id", "Name", "Email"];
const UsersList: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  useEffect(() => {
    apiClient.get("/users").then(res => setUsers(res.data)).catch(console.error);
  }, []);
  return (
    <div>
      <h2>Users</h2><hr/>
      {/*<ul>{users.map(u => <li key={u.id}>{u.name} ({u.email})</li>)}</ul>*/}
       <DataTable columns={columns} data={users} />
    </div>
  );
};

export default UsersList;