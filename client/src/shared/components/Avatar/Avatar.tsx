import React from "react";
import "./Avatar.scss";
import PersonIcon from "@mui/icons-material/Person";
import { ProfileImg } from "../../utils/CloudImg";

interface AvatarProps {
  photoUrl: string | null;
}

const Avatar: React.FC<AvatarProps> = ({ photoUrl }) => {
  return (
    <div className="avatar-container">
      <div className="avatar">
        {photoUrl ? (
          <ProfileImg publicId={photoUrl} />
        ) : (
          <PersonIcon className="person-icon" />
        )}
      </div>
    </div>
  );
};

export default Avatar;
