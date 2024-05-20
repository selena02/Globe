import React, { useState } from "react";
import Avatar from "../Avatar/Avatar";
import { CommentDto } from "../../models/Comment";
import { Favorite, FavoriteBorder, Delete } from "@mui/icons-material";
import "./Comment.scss";
import fetchAPI from "../../utils/fetchAPI";
import { handleApiErrors } from "../../utils/displayApiErrors";
import { Link } from "react-router-dom";

interface CommentProps {
  comment: CommentDto;
  onCommentDeleted: (commentId: number) => void;
  onOpenDeleteDialog: (comment: CommentDto) => void;
}

const Comment: React.FC<CommentProps> = ({
  comment,
  onCommentDeleted,
  onOpenDeleteDialog,
}) => {
  const [liked, setLiked] = useState(comment.isLikedByCurrentUser);
  const [isLikeLoading, setIsLikeLoading] = useState(false);
  const [likesCount, setLikesCount] = useState(comment.likesCount);

  const handleLike = async () => {
    if (isLikeLoading) {
      return;
    }
    setIsLikeLoading(true);
    setLiked((prevLiked) => !prevLiked);
    try {
      if (liked) {
        setLikesCount((prevCount) => prevCount - 1);
        await fetchAPI(`comments/unlike/${comment.commentId}`, {
          method: "DELETE",
        });
        setLiked(false);
      } else {
        setLikesCount((prevCount) => prevCount + 1);
        await fetchAPI(`comments/like/${comment.commentId}`, {
          method: "POST",
        });
        setLiked(true);
      }
    } catch (err) {
      handleApiErrors(err);
      if (liked) {
        setLikesCount((prevCount) => prevCount + 1);
      } else {
        setLikesCount((prevCount) => prevCount - 1);
      }
    } finally {
      setIsLikeLoading(false);
    }
  };

  const handleDelete = async () => {
    onOpenDeleteDialog(comment);
  };

  return (
    <div className="comment">
      <div className="comment-avatar-container">
        <Avatar photoUrl={comment.profilePicture} />
      </div>

      <div className="comment-content">
        <div className="comment-header">
          <Link className="comment-username" to={`/profile/${comment.userId}`}>
            {comment.userName}
          </Link>
          <span>{new Date(comment.createdAt).toLocaleString()}</span>
        </div>
        <p className="comment-text">{comment.content}</p>
        <div className="comment-actions">
          <div className="comment-likes-container">
            <button onClick={handleLike} className="like-comment-btn">
              {liked ? (
                <Favorite className="comment-icon" />
              ) : (
                <FavoriteBorder className="comment-icon" />
              )}
            </button>
            <button className="comment-likes-count">{likesCount} Likes</button>
          </div>
          {comment.canDelete && (
            <button onClick={handleDelete} className="delete-comment-btn">
              <Delete className="icon" />
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default Comment;
