import React, { useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate, useParams } from "react-router-dom";
import fetchAPI from "../../shared/utils/fetchAPI";
import { ProfileUser } from "./models/profileUser";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import ProfileCard from "./ProfileCard/ProfileCard";
import "./Profile.scss";

const Profile = () => {
  const { id } = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const [user, setUser] = useState<ProfileUser | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const userData = await fetchAPI<ProfileUser>(`users/${id}`, {
          method: "GET",
        });
        setUser(userData);
      } catch (error) {
        handleApiErrors(error);
        navigate("/");
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return (
    <div id="profile-page">
      <ProfileCard user={user} />

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
