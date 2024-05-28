import React, { useState } from "react";
import Avatar from "../../../shared/components/Avatar/Avatar";
import { UserDto } from "../models/user";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { Link, useNavigate } from "react-router-dom";
import "./User.scss";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

interface UserProps {
  user: UserDto;
  isPilot: boolean;
}

const User: React.FC<UserProps> = ({ user, isPilot }) => {
  const [isGuide, setIsGuide] = useState(user.isGuide);
  const [isLoading, setIsLoading] = useState(false);
  const [isUserDeleteDialogOpen, setIsUserDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();

  const handleRoleChange = async () => {
    if (!isPilot) {
      return;
    }

    try {
      setIsLoading(true);
      await fetchAPI(`pilot/update/role/${user.userId}`, { method: "PUT" });
      setIsGuide((prevIsGuide) => !prevIsGuide);
    } catch (err: any) {
      handleApiErrors(err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!isPilot) {
      return;
    }

    try {
      setIsLoading(true);
      await fetchAPI(`pilot/delete/${user.userId}`, { method: "DELETE" });
      navigate(`/dummy`);
      navigate(`/users`);
      window.location.reload();
    } catch (err: any) {
      handleApiErrors(err);
    } finally {
      setIsLoading(false);
    }
  };
  return (
    <div key={user.userId} className="user-card">
      <Link to={`/profile/${user.userId}`} className="user-left">
        <div className="avatar-user-container">
          <Avatar photoUrl={user.profilePicture} />
        </div>
        <div className="username-and-created-container">
          <h3 className="username">{user.username}</h3>
          <p>Created: {new Date(user.createdAt).toLocaleString()}</p>
        </div>
      </Link>
      <div className="user-middle">
        {isGuide ? (
          <button
            onClick={handleRoleChange}
            type="button"
            title="edit role button"
            className="is-guide-button"
            disabled={!isPilot || isLoading}
          >
            Guide
          </button>
        ) : (
          <button
            onClick={handleRoleChange}
            type="button"
            title="edit role button"
            className="is-traveler-button"
            disabled={!isPilot || isLoading}
          >
            Traveler
          </button>
        )}
      </div>
      {isPilot && (
        <div className="user-right">
          <button
            type="button"
            title="delete button"
            className="delete-button"
            onClick={() => setIsUserDeleteDialogOpen(true)}
            disabled={isLoading}
          >
            Delete
          </button>
        </div>
      )}
      <Dialog
        open={isUserDeleteDialogOpen}
        onClose={() => setIsUserDeleteDialogOpen(false)}
      >
        <DialogTitle className="dialog-title">Delete User</DialogTitle>
        <DialogContent>
          <DialogContentText className="dialog-text">
            Are you sure you want to delete this user?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            className="dialog-button"
            onClick={() => setIsUserDeleteDialogOpen(false)}
            color="primary"
          >
            Cancel
          </Button>
          <Button
            className="dialog-button"
            onClick={handleDelete}
            color="secondary"
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default User;
