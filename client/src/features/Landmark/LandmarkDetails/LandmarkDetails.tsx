import { useState } from "react";
import { ClassifyLandmarkResponse } from "../models/Landmark";
import "./LandmarkDetails.scss";
import "leaflet/dist/leaflet.css";
import CountryFlag from "react-country-flag";
import { LandmarkImg } from "../../../shared/utils/CloudImg";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import LinearProgress from "@mui/material/LinearProgress";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import KeyboardBackspaceIcon from "@mui/icons-material/KeyboardBackspace";
import FavoriteIcon from "@mui/icons-material/Favorite";
import SaveLandmark from "../SaveLandmark/SaveLandmark";

interface LadnmarkDetailProps {
  landmarkData: ClassifyLandmarkResponse | null;
  onClosed: () => void;
}

const LandmarkDetails: React.FC<LadnmarkDetailProps> = ({
  landmarkData,
  onClosed,
}) => {
  if (landmarkData === null) {
    onClosed();
    return null;
  }
  const position = {
    lat: landmarkData.locationDetails.latitude ?? 0,
    lng: landmarkData.locationDetails.longitude ?? 0,
  };
  const score = landmarkData.landmark.score;
  const maxScore = 5.5;
  const normalizedScore = (score / maxScore) * 100;
  const [favouriteHover, setFavouriteHover] = useState(false);
  const [isPopupOpen, setIsPopupOpen] = useState(false);

  const getColor = (s: any) => {
    if (s < 30) return "#ff7060df";
    if (s >= 30 && s <= 60) return "rgba(250, 250, 105, 0.775)";
    return "rgb(180, 243, 180)";
  };

  const handleClose = () => {
    setIsPopupOpen(false);
  };

  return (
    <div className="landmark-details-container">
      {landmarkData.canSave && (
        <button
          onMouseEnter={() => setFavouriteHover(true)}
          onMouseLeave={() => setFavouriteHover(false)}
          onClick={() => setIsPopupOpen(true)}
          className="save-button"
        >
          <p>Save</p>{" "}
          {favouriteHover ? (
            <FavoriteIcon className="save-icon" />
          ) : (
            <FavoriteBorderIcon className="save-icon" />
          )}
        </button>
      )}
      <button onClick={onClosed} className="back-button">
        <KeyboardBackspaceIcon />
      </button>
      <h1 className="landmark-name">{landmarkData.landmark.name}</h1>
      <p className="landmark-location">
        {landmarkData.locationDetails.city},{" "}
        {landmarkData.locationDetails.country}{" "}
        {landmarkData.locationDetails.countryCode && (
          <CountryFlag
            countryCode={landmarkData.locationDetails.countryCode}
            svg
            style={{
              width: "1.6em",
              height: "1.6em",
              marginLeft: "0.2em",
            }}
            title={landmarkData.locationDetails.country || ""}
          />
        )}
      </p>
      <div className="progress-bar">
        <LinearProgress
          variant="determinate"
          value={normalizedScore}
          style={{
            height: "2.6em",
            backgroundColor: "#eee",
            borderRadius: "2em",
          }}
          sx={{
            "& .MuiLinearProgress-bar": {
              backgroundColor: getColor(normalizedScore),
            },
          }}
        />
        <div className="progress-bar-score">
          {Math.round(normalizedScore)}% Confidence Score
        </div>
      </div>
      <div className="landmark-image-map-container">
        <div className="landmark-img">
          <LandmarkImg publicId={landmarkData.photoId} />
        </div>
        {landmarkData.locationDetails.latitude &&
          landmarkData.locationDetails.longitude && (
            <MapContainer
              className="map"
              center={[
                landmarkData.locationDetails.latitude,
                landmarkData.locationDetails.longitude,
              ]}
              zoom={13}
            >
              <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />
              <Marker
                position={[
                  landmarkData.locationDetails.latitude,
                  landmarkData.locationDetails.longitude,
                ]}
              >
                <Popup>{landmarkData.landmark.name}</Popup>
              </Marker>
            </MapContainer>
          )}
      </div>
      {isPopupOpen && <SaveLandmark onClosed={handleClose} />}
    </div>
  );
};

export default LandmarkDetails;
