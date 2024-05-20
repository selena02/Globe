export interface PostDto {
  userId: number;
  userName: string;
  profilePicture: string | null;
  likesCount: number;
  commentsCount: number;
  postId: number;
  postPicture: string;
}

export interface FullPostDto {
  postId: number;
  content: string;
  postPicture: string;
  createdAt: Date;
  likesCount: number;
  commentsCount: number;
  userId: number;
  userName: string;
  profilePicture: string | null;
  isLiked: boolean;
  isOwner: boolean;
  canDelete: boolean;
}

export interface UploadPostDto {
  content: string;
  postImage: FileList;
}

export interface LikedUserDto {
  userId: number;
  username: string;
  profilePicture: string | null;
}
