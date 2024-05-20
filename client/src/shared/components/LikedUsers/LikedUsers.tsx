import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Typography,
} from "@mui/material";
import Avatar from "../Avatar/Avatar";
import { LikedUserDto } from "../../models/Post";
import { Link } from "react-router-dom";
import "./LikedUsers.scss";

interface LikedUsersProps {
  likedUsers: LikedUserDto[];
  onClose: () => void;
  isOpen: boolean;
}

const LikedUsers: React.FC<LikedUsersProps> = ({
  likedUsers,
  onClose,
  isOpen,
}) => {
  return (
    <Dialog open={isOpen} onClose={onClose}>
      <DialogContent className="liked-users">
        <List>
          {likedUsers.map((user) => (
            <ListItem
              key={user.userId}
              component={Link}
              to={`/profile/${user.userId}`}
              className="liked-user"
            >
              <ListItemAvatar>
                <Avatar photoUrl={user.profilePicture} />
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Typography variant="h6" style={{ fontWeight: "bold" }}>
                    {user.username}
                  </Typography>
                }
              />
            </ListItem>
          ))}
        </List>
      </DialogContent>
    </Dialog>
  );
};

export default LikedUsers;
