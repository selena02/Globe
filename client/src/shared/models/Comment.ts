export interface CommentDto {
  commentId: number;
  userId: number;
  userName: string;
  profilePicture: string | null;
  content: string;
  likesCount: number;
  createdAt: Date;
  isLikedByCurrentUser: boolean;
  canDelete: boolean;
}
