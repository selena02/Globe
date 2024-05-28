export interface UserDto {
  userId: number;
  username: string;
  profilePicture: string | null;
  createdAt: Date;
  isGuide: boolean;
}
