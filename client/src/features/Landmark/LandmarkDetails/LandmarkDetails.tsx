import { useState, useEffect } from "react";
import { ClassifyLandmarkResponse } from "../models/Landmark";
import "./LandmarkDetails.scss";
import "leaflet/dist/leaflet.css";
import CountryFlag from "react-country-flag";
import { LandmarkImg } from "../../../shared/utils/CloudImg";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import L from "leaflet";
import LinearProgress from "@mui/material/LinearProgress";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import KeyboardBackspaceIcon from "@mui/icons-material/KeyboardBackspace";
import FavoriteIcon from "@mui/icons-material/Favorite";
import SaveLandmark from "../SaveLandmark/SaveLandmark";

interface LandmarkDetailProps {
  landmarkData: ClassifyLandmarkResponse | null;
  onClosed: () => void;
}

const LandmarkDetails: React.FC<LandmarkDetailProps> = ({
  landmarkData,
  onClosed,
}) => {
  const [normalizedScore, setNormalizedScore] = useState(0);
  const [favouriteHover, setFavouriteHover] = useState(false);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const customIcon = new L.Icon({
    iconUrl: "/images/general/marker-icon.png",
    iconSize: [20, 30],
    iconAnchor: [17, 45],
    popupAnchor: [1, -34],
    shadowSize: [50, 64],
    shadowAnchor: [15, 64],
  });

  useEffect(() => {
    if (
      landmarkData &&
      landmarkData.landmark.score !== null &&
      landmarkData.landmark.score !== undefined
    ) {
      const score = landmarkData.landmark.score;
      const maxScore = 1.8;
      setNormalizedScore(((score - 0.7) / (maxScore - 0.7)) * 100);
    }
  }, [landmarkData]);

  if (landmarkData === null || landmarkData === undefined) {
    onClosed();
    return null;
  }

  const getColor = (s: any) => {
    if (s < 30) return "#ff7060df";
    if (s >= 30 && s <= 50) return "rgba(250, 250, 105, 0.775)";
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
          <p className="save">Save</p>{" "}
          {favouriteHover ? (
            <FavoriteIcon className="save-icon" />
          ) : (
            <FavoriteBorderIcon className="save-icon" />
          )}
        </button>
      )}
      <button
        type="button"
        title="back button"
        onClick={onClosed}
        className="back-button"
      >
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
                icon={customIcon}
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
