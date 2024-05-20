import React, { useState, useCallback } from "react";
import { useForm, SubmitHandler } from "react-hook-form";
import { useDropzone } from "react-dropzone";
import { PostDto } from "../../models/Post";
import "./UploadPost.scss";
import { handleApiErrors } from "../../utils/displayApiErrors";
import { Close } from "@mui/icons-material";

interface UploadPostProps {
  onPostUploaded: (post: PostDto) => void;
  onClose: () => void;
}

interface UploadPostFormValues {
  content: string;
  postImage: FileList;
}

const API_URL = "https://localhost:7063/api";

const UploadPost: React.FC<UploadPostProps> = ({ onPostUploaded, onClose }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UploadPostFormValues>();
  const [isUploading, setIsUploading] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);

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

  const onSubmit: SubmitHandler<UploadPostFormValues> = async (data) => {
    if (isUploading || !file) return;

    setIsUploading(true);

    const formData = new FormData();
    formData.append("Content", data.content);
    formData.append("PostImage", file);

    const token = localStorage.getItem("token");
    const headers: HeadersInit = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    try {
      const response = await fetch(`${API_URL}/posts/upload`, {
        method: "POST",
        body: formData,
        headers,
      });

      const postResponse = await response.json();

      if (!response.ok) {
        throw new Error(postResponse.message || "Failed to upload post");
      }

      onPostUploaded(postResponse.post);
    } catch (err) {
      handleApiErrors(err);
    } finally {
      setIsUploading(false);
    }
  };

  return (
    <div className="upload-post-container">
      <form onSubmit={handleSubmit(onSubmit)} className="upload-post-form">
        <button type="button" className="close-button" onClick={onClose}>
          <Close className="close-icon" />
        </button>
        <h1>Upload Post</h1>
        <div className="form-group">
          <label htmlFor="content">Content*</label>
          <textarea
            {...register("content", {
              required: "Content is required",
              maxLength: {
                value: 500,
                message: "Content must be under 500 characters",
              },
            })}
            disabled={isUploading}
            className={`upload-post-input ${
              errors.content ? "is-invalid" : ""
            }`}
          />
          <p className="error">
            {errors.content ? errors.content.message : ""}
          </p>
        </div>
        <label htmlFor="image">Image*</label>
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
          {errors.postImage ? errors.postImage.message : ""}
        </p>

        <button
          type="submit"
          disabled={isUploading || !file || !errors.content}
          className="upload-post-button"
        >
          {isUploading ? "Uploading..." : "Post"}
        </button>
      </form>
    </div>
  );
};

export default UploadPost;
