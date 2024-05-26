import { useEffect, useState } from "react";
import Spinner from "../../../shared/components/Spinner/Spinner";
import { LandmarkCardDto } from "../models/landmarks";
import { useParams } from "react-router-dom";
import fetchAPI from "../../../shared/utils/fetchAPI";
import "./Landmarks.scss";
import LandmarkCard from "./LandmarkCard/LandmarkCard";

const Landmarks = () => {
  const [landmarks, setLandmarks] = useState<LandmarkCardDto[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { id } = useParams();
  console.log(id);

  useEffect(() => {
    const fetchLandmarks = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const endpoint = `landmark/user/${id}`;
        const data = await fetchAPI<{ landmarks: LandmarkCardDto[] }>(
          endpoint,
          {
            method: "GET",
          }
        );
        setLandmarks([...data.landmarks]);
      } catch (err: any) {
        setError(err.message || "Something went wrong");
      } finally {
        setIsLoading(false);
      }
    };

    fetchLandmarks();
  }, [id]);

  const handleDeleteLandmark = (landmarkId: number) => {
    setLandmarks((prevLandmarks) =>
      prevLandmarks.filter((landmark) => landmark.landmarkId !== landmarkId)
    );
  };

  return (
    <div className="landmarks-section-container">
      <div className="landmarks-container">
        {landmarks.map((landmark) => (
          <LandmarkCard
            key={landmark.landmarkId}
            landmark={landmark}
            onLandmarkDeleted={handleDeleteLandmark}
          />
        ))}
      </div>
      <div id="loading-or-error">
        {isLoading && !error && <Spinner />}
        {error && <div className="error-message">{error + " :("}</div>}
      </div>
    </div>
  );
};

export default Landmarks;
