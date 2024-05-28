import React, { useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate, useParams } from "react-router-dom";
import fetchAPI from "../../shared/utils/fetchAPI";
import { ProfileUser } from "./models/profileUser";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import ProfileCard from "./ProfileCard/ProfileCard";
import "./Profile.scss";
import { set } from "react-hook-form";
import Spinner from "../../shared/components/Spinner/Spinner";
import { Shield } from "@mui/icons-material";

const Profile = () => {
  const { id } = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [user, setUser] = useState<ProfileUser | null>(null);
  const [bio, setBio] = useState<string | null>(null);
  const [deleteBio, setDeleteBio] = useState<boolean>(false);
  const currentUser = localStorage.getItem("user");

  if (!currentUser) {
    navigate("/account/login");
  }
  const userParsed = JSON.parse(currentUser!);
  const isGuide = userParsed.roles.includes("Guide");

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

  if (isLoading) {
    return (
      <div className="spinner-profile-container">
        <Spinner />
      </div>
    );
  }

  return (
    <div id="profile-page">
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
