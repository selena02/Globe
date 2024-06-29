import { Close, Delete } from "@mui/icons-material";
import Spinner from "../../../../shared/components/Spinner/Spinner";
import { handleApiErrors } from "../../../../shared/utils/displayApiErrors";
import fetchAPI from "../../../../shared/utils/fetchAPI";
import { FullLandmarkDto } from "../../models/landmarks";
import "./FullLandmark.scss";
import { useEffect, useState } from "react";
import { FullPostImg } from "../../../../shared/utils/CloudImg";
import CountryFlag from "react-country-flag";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Rating,
} from "@mui/material";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";

interface FullLandmarkProps {
  landmarkId: number;
  onClose: () => void;
  onLandmarkDeleted: (postId: number) => void;
}

const FullLandmark: React.FC<FullLandmarkProps> = ({
  landmarkId,
  onClose,
  onLandmarkDeleted,
}) => {
  const [showPopup, setShowPopup] = useState(false);
  const [landmark, setLandmark] = useState<FullLandmarkDto | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const customIcon = new L.Icon({
    iconUrl: "/images/general/marker-icon.png",
    iconSize: [20, 30],
    iconAnchor: [17, 45],
    popupAnchor: [1, -34],
    shadowSize: [50, 64],
    shadowAnchor: [15, 64],
  });

  useEffect(() => {
    const fetchLandmark = async () => {
      setIsLoading(true);
      try {
        const endpoint = `landmark/${landmarkId}`;
        const data = await fetchAPI<FullLandmarkDto>(endpoint, {
          method: "GET",
        });
        setLandmark(data);
      } catch (err: any) {
        handleApiErrors(err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchLandmark();
  }, [landmarkId]);

  const confirmDeleteLandmark = async () => {
    if (landmark == null) {
      return;
    }
    try {
      const endpoint = `landmark/${landmark.landmarkId}`;
      await fetchAPI(endpoint, { method: "DELETE" });
      onLandmarkDeleted(landmark.landmarkId);
    } catch (err: any) {
      handleApiErrors(err);
    } finally {
      setIsDeleteDialogOpen(false);
    }
  };

  if (isLoading) {
    return (
      <div className="spinner-container">
        <Spinner />
      </div>
    );
  }
  if (landmark == null) {
    return null;
  }

  return (
    <div className="full-landmark-container">
      <div className="landmark-popup">
        <button
          title="close button"
          type="button"
          onClick={onClose}
          className="close-btn"
        >
          <Close className="icon" />
        </button>
        <div className="landmark-content">
          <div className="landmark-image">
            <FullPostImg publicId={landmark.publicId} />
          </div>
          <div className="landmark-details">
            {landmark.canDelete && (
              <button
                title="delete button"
                type="button"
                className="delete-btn"
                onClick={() => setIsDeleteDialogOpen(true)}
              >
                <Delete className="icon" />
              </button>
            )}
            <div className="details-upper">
              <h1 className="name">{landmark.locationName}</h1>
              <p className="landmark-location">
                {landmark.city}, {landmark.country}{" "}
                {landmark.countryCode && (
                  <CountryFlag
                    countryCode={landmark.countryCode}
                    svg
                    style={{
                      width: "1.4em",
                      height: "1.4em",
                      marginLeft: "0.2em",
                    }}
                    title={landmark.country || ""}
                  />
                )}
              </p>
              <p className="review">
                {landmark.review ? landmark.review : "No review"}
              </p>

              <div className="rating">
                <Rating
                  value={landmark.rating}
                  precision={0.5}
                  readOnly
                  sx={{ fontSize: 35 }}
                />
              </div>
              <p className="visitend-date">
                Visited on:{" "}
                {new Date(landmark.visitedOn).toLocaleDateString("en-US", {
                  year: "numeric",
                  month: "long",
                  day: "numeric",
                })}
              </p>
            </div>

            {landmark.latitude && landmark.longitude && (
              <MapContainer
                className="map"
                center={[landmark.latitude, landmark.longitude]}
                zoom={13}
              >
                <TileLayer
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <Marker
                  icon={customIcon}
                  position={[landmark.latitude, landmark.longitude]}
                >
                  <Popup>{landmark.locationName}</Popup>
                </Marker>
              </MapContainer>
            )}
          </div>
        </div>
      </div>
      <Dialog
        open={isDeleteDialogOpen}
        onClose={() => setIsDeleteDialogOpen(false)}
      >
        <DialogTitle className="dialog-title">Delete Landmark</DialogTitle>
        <DialogContent>
          <DialogContentText className="dialog-text">
            Are you sure you want to delete this landmark?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            className="dialog-button"
            onClick={() => setIsDeleteDialogOpen(false)}
            color="primary"
          >
            Cancel
          </Button>
          <Button
            className="dialog-button"
            onClick={confirmDeleteLandmark}
            color="secondary"
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default FullLandmark;
