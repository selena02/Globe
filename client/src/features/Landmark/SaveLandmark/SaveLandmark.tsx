import { useForm, Controller, set } from "react-hook-form";
import { TextField, Rating } from "@mui/material";
import "./SaveLandmark.scss";
import { Close } from "@mui/icons-material";
import { useState } from "react";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { RootState } from "../../../state/store";

interface SaveLandmarkProps {
  onClosed: () => void;
}
const SaveLandmark: React.FC<SaveLandmarkProps> = ({ onClosed }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      review: "",
      rating: 1,
    },
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const currentUser: any = useSelector((state: RootState) => state.auth.user);

  const onSubmit = async (data: any) => {
    if (isLoading) return;
    if (!currentUser) {
      navigate("/account/login");
      return;
    }
    setIsLoading(true);
    try {
      const response = await fetchAPI("landmark/save", {
        method: "POST",
        body: data,
      });
      navigate(`/profile/${currentUser.id}/landmarks`);
    } catch (error: any) {
      handleApiErrors(error);
    } finally {
      setIsLoading(false);
    }
  };

  const onClose = () => {
    onClosed();
  };

  return (
    <div className="save-landmark-container">
      <form className="save-landmark-form" onSubmit={handleSubmit(onSubmit)}>
        <button
          type="button"
          disabled={isLoading}
          className="close-save-button"
          onClick={onClose}
        >
          <Close className="close-icon" />
        </button>
        <h1 className="form-title">Save Landmark</h1>
        <p>Review</p>
        <Controller
          name="review"
          control={control}
          rules={{
            maxLength: {
              value: 500,
              message: "Review cannot be more than 500 characters",
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              multiline
              rows={4}
              fullWidth
              variant="outlined"
              error={Boolean(errors.review)}
              helperText={errors.review ? errors.review.message : ""}
              sx={{
                "& .MuiInputBase-input": {
                  fontSize: "1.4em",
                },
                "& .MuiOutlinedInput-root": {
                  "&.Mui-focused fieldset": {
                    borderColor: "#c03ff3",
                  },
                },
              }}
            />
          )}
        />
        <Controller
          name="rating"
          control={control}
          rules={{ required: "Rating is required", min: 1, max: 5 }}
          render={({ field }) => (
            <div>
              <p className="rating">Rating</p>
              <Rating
                {...field}
                value={Number(field.value)}
                onChange={(_, value) => field.onChange(value)}
                sx={{ fontSize: 35, color: "#f368e0" }}
              />
            </div>
          )}
        />
        <button
          disabled={isLoading}
          className="save-landmark-button"
          type="submit"
        >
          {isLoading ? "Saving Landmark.." : "Save Landmark"}
        </button>
      </form>
    </div>
  );
};

export default SaveLandmark;
