import { Close } from "@mui/icons-material";
import Spinner from "../../../../shared/components/Spinner/Spinner";
import { handleApiErrors } from "../../../../shared/utils/displayApiErrors";
import fetchAPI from "../../../../shared/utils/fetchAPI";
import { FullLandmarkDto } from "../../models/landmarks";
import "./FullLandmark.scss";
import { useEffect, useState } from "react";
import { FullPostImg } from "../../../../shared/utils/CloudImg";

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
        <button onClick={onClose} className="close-btn">
          <Close className="icon" />
        </button>
        <div className="landmark-content">
          <div className="landmark-image">
            <FullPostImg publicId={landmark.publicId} />
          </div>
          <div className="landmark-details"></div>
        </div>
      </div>
    </div>
  );
};
