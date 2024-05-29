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
import { FollowerDto } from "../models/followUser";
import LikedUsers from "../../../shared/components/LikedUsers/LikedUsers";
import { Shield } from "@mui/icons-material";

interface ProfileCardProps {
  user: ProfileUser | null;
}

interface GetFollowStatusResponse {
  isFollowing: boolean;
}

const ProfileCard = ({ user }: ProfileCardProps) => {
  const currentUser: any = useSelector((state: RootState) => state.auth.user);
  const [isFollowing, setIsFollowing] = useState(false);
  const [loadingStatus, setLoadingStatus] = useState(false);
  const [loadingFollows, setLoadingFollows] = useState(false);
  const [followers, setFollowers] = useState<FollowerDto[]>([]);
  const [following, setFollowing] = useState<FollowerDto[]>([]);
  const [isFollowerUsersOpen, setIsFollowerUsersOpen] = useState(false);
  const [isFollowingUsersOpen, setIsFollowingUsersOpen] = useState(false);
  const isGuide = currentUser?.roles.includes("Guide");
  const [profilePicture, setProfilePicture] = useState<string | null>(
    user?.profilePicture ?? null
  );
  const [deletingPicture, setDeletingPicture] = useState(false);
  const isCurrentUser = currentUser && user?.id === currentUser.id;

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

  const fetchFollowers = async () => {
    if (!user) {
      return;
    }

    if (user.followersCount === 0) {
      return;
    }

    setLoadingFollows(true);
    try {
      const response = await fetchAPI<{ followers: FollowerDto[] }>(
        `users/${user.id}/followers`
      );
      setFollowers(response.followers);
      setIsFollowerUsersOpen(true);
    } catch (error: any) {
      handleApiErrors(error);
    } finally {
      setLoadingFollows(false);
    }
  };

  const fetchFollowing = async () => {
    if (!user) {
      return;
    }

    if (user.followingCount === 0) {
      return;
    }
    if (isFollowerUsersOpen) {
      setIsFollowerUsersOpen(false);
      return;
    }

    setLoadingFollows(true);
    try {
      const response = await fetchAPI<{ following: FollowerDto[] }>(
        `users/${user.id}/following`
      );
      setFollowing(response.following);
      setIsFollowingUsersOpen(true);
    } catch (error: any) {
      handleApiErrors(error);
    } finally {
      setLoadingFollows(false);
    }
  };

  const handleCloseFollowingUsers = () => {
    setIsFollowingUsersOpen(false);
  };

  const handleCloseFollowerUsers = () => {
    setIsFollowerUsersOpen(false);
  };

  const handleModeratePicture = async () => {
    try {
      setDeletingPicture(true);
      await fetchAPI(`guide/delete/picture/${user?.id}`, { method: "DELETE" });
      setProfilePicture(null);
    } catch (error: any) {
      handleApiErrors(error);
    } finally {
      setDeletingPicture(false);
    }
  };

  if (!user) {
    return <Spinner></Spinner>;
  }

  return (
    <div className="profile-card">
      <div className="avatar-picture">
        <Avatar photoUrl={profilePicture} />
        {isGuide && profilePicture && !isCurrentUser && (
          <button
            onClick={handleModeratePicture}
            type="button"
            title="moderate picture"
            className="moderate-picture"
            disabled={deletingPicture}
          >
            <Shield className="moderate-icon" />
          </button>
        )}
      </div>

      <div className="profile-details">
        <h2 className="profile-name">{user.fullName}</h2>
        <p className="profile-username">@{user.userName}</p>
        {user.location && <p className="profile-location">{user.location}</p>}
        <div className="profile-social">
          <button
            onClick={fetchFollowers}
            disabled={loadingFollows}
            className="profile-followers"
          >
            Followers: {user.followersCount}
          </button>
          <button
            onClick={fetchFollowing}
            disabled={loadingFollows}
            className="profile-following"
          >
            Following: {user.followingCount}
          </button>
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
      <LikedUsers
        likedUsers={followers}
        isOpen={isFollowerUsersOpen}
        onClose={handleCloseFollowerUsers}
      />
      <LikedUsers
        likedUsers={following}
        isOpen={isFollowingUsersOpen}
        onClose={handleCloseFollowingUsers}
      />
    </div>
  );
};

export default ProfileCard;
