import { useNavigate, useParams } from "react-router-dom";
import "./Profile.scss";
import { useEffect, useState } from "react";
import fetchAPI from "../../shared/utils/fetchAPI";
import { ProfileUser } from "./models/profileUser";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import ProfileCard from "./ProfileCard/ProfileCard";

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
    </div>
  );
};

export default Profile;
