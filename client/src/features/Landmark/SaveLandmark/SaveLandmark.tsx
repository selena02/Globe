import { useForm, Controller } from "react-hook-form";
import { TextField, Rating, Typography } from "@mui/material";
import "./SaveLandmark.scss";

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

  const onSubmit = (data: any) => {
    // Here you would handle the API request
    console.log(data);
  };

  return (
    <form className="save-landmark-form" onSubmit={handleSubmit(onSubmit)}>
      <Typography variant="h6">Save Landmark Review</Typography>
      <Controller
        name="review"
        control={control}
        rules={{
          required: "Review is required",
          maxLength: {
            value: 500,
            message: "Review cannot be more than 500 characters",
          },
        }}
        render={({ field }) => (
          <TextField
            {...field}
            label="Review"
            multiline
            rows={4}
            fullWidth
            variant="outlined"
            error={Boolean(errors.review)}
            helperText={errors.review ? errors.review.message : ""}
          />
        )}
      />
      <Controller
        name="rating"
        control={control}
        rules={{ required: "Rating is required", min: 1, max: 5 }}
        render={({ field }) => (
          <div>
            <Typography component="legend">Rating</Typography>
            <Rating
              {...field}
              value={Number(field.value)}
              onChange={(_, value) => field.onChange(value)}
            />
          </div>
        )}
      />
      <button type="submit">Save Landmark</button>
    </form>
  );
};

export default SaveLandmark;
