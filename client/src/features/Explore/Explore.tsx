import "./Explore.scss";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import "leaflet/dist/leaflet.css";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { useEffect, useState } from "react";
import fetchAPI from "../../shared/utils/fetchAPI";
import { LandmarkCoordinatesDto } from "./models/landmarkCoordinates";
import L from "leaflet";
import Spinner from "../../shared/components/Spinner/Spinner";

const Explore = () => {
  const [coordinates, setCoordinates] = useState<LandmarkCoordinatesDto[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const customIcon = new L.Icon({
    iconUrl: "/images/general/marker-icon.png",
    iconSize: [20, 30],
    iconAnchor: [17, 45],
    popupAnchor: [1, -34],
    shadowSize: [50, 64],
    shadowAnchor: [15, 64],
  });

  useEffect(() => {
    const fetchCoordinates = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const endpoint = `landmark/coordinates`;
        const data = await fetchAPI<{ coordinates: LandmarkCoordinatesDto[] }>(
          endpoint,
          {
            method: "GET",
          }
        );
        setCoordinates([...data.coordinates]);
      } catch (err: any) {
        setError(err.message || "Something went wrong");
        handleApiErrors(error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCoordinates();
  }, []);

  return (
    <div className="explore-container">
      <div className="title-sutitle-container">
        <h1 className="explore-title">Footprints</h1>
        <p className="explore-subtitle">Trace Our Steps Across the Globe</p>
      </div>
      <MapContainer className="explore-map" center={[0, 0]} zoom={2}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {coordinates.map((coord, index) => (
          <Marker
            key={index}
            position={[coord.latitude, coord.longitude]}
            icon={customIcon}
          >
            <Popup>{coord.locationName}</Popup>
          </Marker>
        ))}
      </MapContainer>
      {isLoading && !error && <Spinner />}
    </div>
  );
};

export default Explore;
