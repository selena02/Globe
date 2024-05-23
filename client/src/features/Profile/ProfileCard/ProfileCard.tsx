import React, { useEffect, useState } from "react";
import "./ProfileCard.scss";
import { ProfileUser } from "../models/profileUser";
import Avatar from "../../../shared/components/Avatar/Avatar";
import { useSelector } from "react-redux";
import { RootState } from "../../../state/store";
import EditUser from "../EditUser/EditUser";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import Spinner from "../../../shared/components/Spinner/Spinner";

interface ProfileCardProps {
  user: ProfileUser | null;
}

interface GetFollowStatusResponse {
  isFollowing: boolean;
}

const ProfileCard = ({ user }: ProfileCardProps) => {
  const currentUser: any = useSelector((state: RootState) => state.auth.user);
  const [isFollowing, setIsFollowing] = useState(false);
  const [loadingStatus, setLoadingStatus] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFollowStatus = async () => {
      if (user && currentUser && user.id !== currentUser.id) {
        try {
          const response = await fetchAPI<GetFollowStatusResponse>(
            `follows/${user.id}/status`
          );
          setIsFollowing(response.isFollowing);
        } catch (error: any) {
          handleApiErrors(error);
        } finally {
          setLoadingStatus(false);
        }
      } else {
        setLoadingStatus(false);
      }
    };

    fetchFollowStatus();
  }, [user, currentUser]);

  const handleFollowToggle = async () => {
    try {
      const endpoint = `follows/${user?.id}`;
      const method = isFollowing ? "DELETE" : "POST";
      await fetchAPI(endpoint, { method });
      setIsFollowing((prevStatus) => !prevStatus);
    } catch (error: any) {
      handleApiErrors(error);
    }
  };

  if (!user) {
    return <Spinner></Spinner>;
  }

  const isCurrentUser = currentUser && user.id === currentUser.id;

  return (
    <div className="profile-card">
      <div className="avatar-picture">
        <Avatar photoUrl={user.profilePictureUrl} />
      </div>

      <div className="profile-details">
        <h2 className="profile-name">{user.fullName}</h2>
        <p className="profile-username">@{user.userName}</p>
        {user.location && <p className="profile-location">{user.location}</p>}
        {user.bio && <p className="profile-bio">{user.bio}</p>}
        <div className="profile-social">
          <span className="profile-followers">
            Followers: {user.followersCount}
          </span>
          <span className="profile-following">
            Following: {user.followingCount}
          </span>
        </div>
        {isCurrentUser ? (
          <EditUser />
        ) : (
          !loadingStatus && (
            <button className="follow-button" onClick={handleFollowToggle}>
              {isFollowing ? (
                <>
                  Following <span>✓</span>
                </>
              ) : (
                <>
                  Follow <span>+</span>
                </>
              )}
            </button>
          )
        )}
      </div>
    </div>
  );
};

export default ProfileCard;
