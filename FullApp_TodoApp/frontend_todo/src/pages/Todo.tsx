import React from "react";
import { List, ListItem, ListItemText, Divider } from "@mui/material";

const users = [
  { name: "Alice Johnson", email: "alice@example.com" },
  { name: "Bob Smith", email: "bob@example.com" },
  { name: "Carol Williams", email: "carol@example.com" },
];

const Todo = () => {
  return (
    <List>
      {users.map((user, index) => (
        <div key={index}>
          <ListItem>
            <ListItemText primary={user.name} secondary={user.email} />
          </ListItem>
          {index < users.length - 1 && <Divider />}
        </div>
      ))}
    </List>
  );
};

export default Todo;
