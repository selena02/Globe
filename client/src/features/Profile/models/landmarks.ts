export interface LandmarkCardDto {
  landmarkId: number;
  locationName: string;
  landmarkPictureId: string;
  rating: number;
  visitedOn: Date;
}

export interface FullLandmarkDto {
  landmarkId: number;
  locationName: string;
  visitedOn: Date;
  review: string | null;
  publicId: string;
  rating: number;
  longitude: number | null;
  latitude: number | null;
  country: string | null;
  city: string | null;
  countryCode: string | null;
  canDelete: boolean;
}
