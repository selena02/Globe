import { useCallback, useState } from "react";
import "./Landmark.scss";
import { SubmitHandler, useForm } from "react-hook-form";
import { useDropzone } from "react-dropzone";
import { useSelector } from "react-redux";
import { RootState } from "../../state/store";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import LoadingScreen from "../../shared/components/LoadingScreen/LoadingScreen";
import { ClassifyLandmarkResponse } from "./models/Landmark";
import LandmarkDetails from "./LandmarkDetails/LandmarkDetails";

interface UploadLandmarkFormValues {
  landmarkImage: FileList;
}

const API_URL = "https://localhost:7063/api";

const Landmark = () => {
  const {
    handleSubmit,
    setError,
    formState: { errors, isValid, isSubmitting },
  } = useForm<UploadLandmarkFormValues>({
    mode: "onChange",
  });
  const [isUploading, setIsUploading] = useState(false);
  const [landmarkData, setLandmarkData] =
    useState<ClassifyLandmarkResponse | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const isLoggedIn = useSelector((state: RootState) => state.auth.isLoggedIn);
  const navigate = useNavigate();

  const onDrop = useCallback((acceptedFiles: File[]) => {
    const file = acceptedFiles[0];
    setFile(file);
    setPreview(URL.createObjectURL(file));
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: {
      "image/jpeg": [".jpeg", ".jpg"],
      "image/png": [".png"],
    },
    maxFiles: 1,
  });

  const onSubmit: SubmitHandler<UploadLandmarkFormValues> = async (data) => {
    if (isUploading) return;

    if (!isLoggedIn) {
      navigate("/account/login");
      return;
    }

    if (!file) {
      setError("landmarkImage", {
        type: "manual",
        message: "Image is required",
      });
      return;
    }
    setIsUploading(true);
    const formData = new FormData();
    formData.append("LandmarkImage", file as File);

    const token = localStorage.getItem("token");
    const headers: HeadersInit = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(`${API_URL}/landmark`, {
        method: "POST",
        body: formData,
        headers,
      });

      if (!response.ok) {
        const data = await response.json();
        throw new Error(data.message);
      }

      const data: ClassifyLandmarkResponse = await response.json();
      setLandmarkData(data);
    } catch (err) {
      handleApiErrors(err);
    } finally {
      setIsUploading(false);
      setFile(null);
      setPreview(null);
    }
  };

  const handleClose = () => {
    setLandmarkData(null);
  };

  return (
    <div className="landmark-container">
      {!landmarkData && (
        <form onSubmit={handleSubmit(onSubmit)} className="landmark-form">
          <h1 className="landmark-title">Landmark Detector</h1>
          <p className="landmark-subtitle">
            Capture a Photo, Identify It - AI Powered Landmark Recognition
          </p>
          <div {...getRootProps({ className: "dropzone" })}>
            <input {...getInputProps()} />
            {isDragActive ? (
              <p>Drop the files here ...</p>
            ) : (
              <p>Upload Your Landmark</p>
            )}
            {preview && (
              <img src={preview} alt="Preview" className="preview-image" />
            )}
          </div>

          <button
            type="submit"
            disabled={isUploading || isSubmitting || !isValid || !file}
            className="landmark-button"
          >
            {isUploading ? "Uploading..." : "Detect Landmark"}
          </button>
        </form>
      )}
      {isUploading && <LoadingScreen />}
      {landmarkData && (
        <LandmarkDetails landmarkData={landmarkData} onClosed={handleClose} />
      )}
    </div>
  );
};

export default Landmark;
