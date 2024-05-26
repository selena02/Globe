export interface ProfileUser {
  id: string;
  userName: string;
  fullName: string;
  email: string;
  profilePicture: string | null;
  location: string | null;
  bio: string | null;
  followersCount: number;
  followingCount: number;
}

export interface EditUserDto {
  bio: string | null;
  profilePicture: File | null;
}
