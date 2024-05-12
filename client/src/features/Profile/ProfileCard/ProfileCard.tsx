import "./ProfileCard.scss";
import { ProfileUser } from "../models/profileUser";
import Avatar from "../../../shared/components/Avatar/Avatar";
import { useSelector } from "react-redux";
import { RootState } from "../../../state/store";
import EditUser from "../EditUser/EditUser";

interface ProfileCardProps {
  user: ProfileUser | null;
}

const ProfileCard = ({ user }: ProfileCardProps) => {
  const currentUser: any = useSelector((state: RootState) => state.auth.user);
  let isCurrentUser: boolean = false;

  if (!user) {
    return <div>Loading...</div>;
  }

  if (currentUser !== null) {
    isCurrentUser = user.id === currentUser.id;
  }

  return (
    <div className="profile-card">
      {isCurrentUser && <EditUser />}
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
      </div>
    </div>
  );
};

export default ProfileCard;
