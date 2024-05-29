import { useCallback, useState, useEffect } from "react";
import "./EditUser.scss";
import { useDropzone } from "react-dropzone";
import { SubmitHandler, useForm } from "react-hook-form";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import { Close } from "@mui/icons-material";
import { useNavigate, useParams } from "react-router-dom";

const API_URL = import.meta.env.VITE_API_URL;

interface UploadEditFormValues {
  bio: string | null;
  profilePicture: FileList | null;
}

const EditUser = () => {
  const {
    register,
    handleSubmit,
    setError,
    formState: { errors, isValid, isSubmitting },
    watch,
  } = useForm<UploadEditFormValues>({
    mode: "onChange",
  });

  const [open, setOpen] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [isUploading, setIsUploading] = useState(false);
  const [isFormValid, setIsFormValid] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams();

  const bio = watch("bio");

  useEffect(() => {
    setIsFormValid(Boolean(bio || file));
  }, [bio, file]);

  const openPopup = () => {
    setOpen(true);
  };

  const onClose = () => {
    setOpen(false);
  };

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

  const onSubmit: SubmitHandler<UploadEditFormValues> = async (data) => {
    if (isUploading) return;

    setIsUploading(true);

    const formData = new FormData();
    formData.append("Bio", data.bio ?? "");
    if (file) {
      formData.append("ProfilePicture", file);
    }

    const token = localStorage.getItem("token");
    const headers: HeadersInit = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(`${API_URL}/users/edit`, {
        method: "PUT",
        body: formData,
        headers,
      });

      const postResponse = await response.json();

      if (!response.ok) {
        throw new Error(postResponse.message || "Failed to upload post");
      }

      if (file) {
        setFile(null);
        setPreview(null);
      }
      onClose();
      navigate(`/dummy`);
      navigate(`/profile/${id}`);
      window.location.reload();
    } catch (err) {
      handleApiErrors(err);
    } finally {
      setIsUploading(false);
    }
  };

  return (
    <div>
      <button onClick={openPopup} type="button" className="edit-user-button">
        Edit Profile
      </button>
      {open && (
        <div className="edit-user-form-popup">
          <form onSubmit={handleSubmit(onSubmit)} className="edit-form">
            <button
              title="close button"
              type="button"
              className="close-button"
              onClick={onClose}
            >
              <Close className="close-icon" />
            </button>
            <h1>Edit Profile</h1>
            <div className="form-group">
              <label className="bio-label" htmlFor="bio">
                Bio
              </label>
              <textarea
                id="bio"
                {...register("bio", {
                  maxLength: {
                    value: 200,
                    message: "Bio must be under 200 characters",
                  },
                })}
                disabled={isUploading}
                className="edit-input"
              />
              <p className="error">{errors.bio ? errors.bio.message : ""}</p>
            </div>
            <label htmlFor="image">Profile Picture</label>
            <div {...getRootProps({ className: "dropzone" })}>
              <input {...getInputProps()} />
              {isDragActive ? (
                <p>Drop the files here ...</p>
              ) : (
                <p>Upload your image</p>
              )}
              {preview && (
                <img src={preview} alt="Preview" className="preview-image" />
              )}
            </div>
            <p className="error">
              {errors.profilePicture ? errors.profilePicture.message : ""}
            </p>

            <button
              type="submit"
              disabled={isUploading || isSubmitting || !isFormValid}
              className="edit-button"
            >
              {isUploading ? "Loading..." : "Edit Profile"}
            </button>
          </form>
        </div>
      )}
    </div>
  );
};

export default EditUser;
