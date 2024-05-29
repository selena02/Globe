import React, { useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate, useParams } from "react-router-dom";
import fetchAPI from "../../shared/utils/fetchAPI";
import { ProfileUser } from "./models/profileUser";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import ProfileCard from "./ProfileCard/ProfileCard";
import "./Profile.scss";
import Spinner from "../../shared/components/Spinner/Spinner";
import { Shield } from "@mui/icons-material";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../../state/store";
import { setLogout } from "../../state/features/authSlice";

const Profile = () => {
  const { id } = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [user, setUser] = useState<ProfileUser | null>(null);
  const [bio, setBio] = useState<string | null>(null);
  const [deleteBio, setDeleteBio] = useState<boolean>(false);
  const currentUser: any = useSelector((state: RootState) => state.auth.user);
  const [isUserDeleteDialogOpen, setIsUserDeleteDialogOpen] = useState(false);
  const isGuide = currentUser?.roles.includes("Guide");
  const dispatch = useDispatch();

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const userData = await fetchAPI<ProfileUser>(`users/${id}`, {
          method: "GET",
        });
        setUser(userData);
        setBio(userData.bio);
      } catch (error) {
        handleApiErrors(error);
        navigate("/");
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleModerateBio = async () => {
    try {
      setDeleteBio(true);
      await fetchAPI(`guide/delete/bio/${id}`, { method: "DELETE" });
      setBio(null);
    } catch (error) {
      handleApiErrors(error);
    } finally {
      setDeleteBio(false);
    }
  };

  const handleDeleteAccount = async () => {
    try {
      await fetchAPI(`users/delete`, { method: "DELETE" });
      localStorage.clear();
      dispatch(setLogout());
      navigate("/account/login");
    } catch (error) {
      handleApiErrors(error);
    }
  };

  if (isLoading) {
    return (
      <div className="spinner-profile-container">
        <Spinner />
      </div>
    );
  }

  return (
    <div id="profile-page">
      {user && currentUser && user.id === currentUser.id && (
        <button
          type="button"
          title="delete account"
          className="delete-account-button"
          onClick={() => setIsUserDeleteDialogOpen(true)}
        >
          Delete Account
        </button>
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
            onClick={handleDeleteAccount}
            color="secondary"
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
      <ProfileCard user={user} />
      {user && bio && (
        <p className="profile-bio">
          <span className="about">About Me:</span>
          <span>{bio}</span>
          {isGuide && bio && (
            <button
              onClick={handleModerateBio}
              type="button"
              title="moderate bio"
              className="moderate-bio"
              disabled={deleteBio}
            >
              <Shield className="moderate-bio-icon" />
            </button>
          )}
        </p>
      )}

      <nav className="profile-nav">
        <NavLink
          to="posts"
          className={({ isActive }) =>
            isActive ? "profile-link active" : "profile-link"
          }
        >
          Posts
        </NavLink>

        <NavLink
          to="landmarks"
          className={({ isActive }) =>
            isActive ? "profile-link active" : "profile-link"
          }
        >
          Landmarks
        </NavLink>
      </nav>

      <Outlet />
    </div>
  );
};

export default Profile;
