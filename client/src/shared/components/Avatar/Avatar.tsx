import React from "react";
import "./Avatar.scss";
import PersonIcon from "@mui/icons-material/Person";

interface AvatarProps {
  photoUrl: string | null;
}

const Avatar: React.FC<AvatarProps> = ({ photoUrl }) => {
  const onError = (event: React.SyntheticEvent<HTMLImageElement, Event>) => {
    event.currentTarget.src = "";
    event.currentTarget.onerror = null;
  };

  return (
    <div className="avatar-container">
      <div className="avatar">
        {photoUrl ? (
          <img src={photoUrl} alt="Avatar Photo" />
        ) : (
          <PersonIcon className="person-icon" />
        )}
      </div>
    </div>
  );
};

export default Avatar;
