export interface PostDto {
  userId: number;
  userName: string;
  profilePicture: string | null;
  likesCount: number;
  commentsCount: number;
  postId: number;
  postPicture: string;
}
