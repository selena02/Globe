export interface LandmarkDto {
  error: string | null;
  name: string;
  score: number;
}

export interface LocationDetailsDto {
  city: string | null;
  country: string | null;
  errors: string | null;
  latitude: number | null;
  longitude: number | null;
}

export interface ClassifyLandmarkResponse {
  canSave: boolean;
  landmark: LandmarkDto;
  locationDetails: LocationDetailsDto;
  photoId: string;
}
