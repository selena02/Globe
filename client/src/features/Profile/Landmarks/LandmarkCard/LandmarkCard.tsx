import React, { useState } from "react";
import { Rating } from "@mui/material";
import { PostImg } from "../../../../shared/utils/CloudImg";
import { LandmarkCardDto } from "../../models/landmarks";
import "./LandmarkCard.scss";

interface LandmarkCardProps {
  landmark: LandmarkCardDto;
}

const LandmarkCard: React.FC<LandmarkCardProps> = ({ landmark }) => {
  const [showPopup, setShowPopup] = useState(false);
  const formattedDate = new Date(landmark.visitedOn).toLocaleDateString(
    "en-US",
    {
      year: "numeric",
      month: "long",
      day: "numeric",
    }
  );

  const handlePictureClick = () => {
    setShowPopup(true);
  };

  const handleClosePopup = () => {
    setShowPopup(false);
  };

  return (
    <div className="landmark-card">
      <div onClick={handlePictureClick} className="landmark-img">
        <PostImg publicId={landmark.landmarkPictureId} />
        <h2 className="landmark-name">{landmark.locationName}</h2>
      </div>
      <div className="landmark-footer">
        <div className="footer-left">
          <Rating value={landmark.rating} precision={0.5} readOnly />
        </div>
        <div className="footer-right">{formattedDate}</div>
      </div>
    </div>
  );
};

export default LandmarkCard;
