import { FullPostDto } from "../../models/Post";
import { handleApiErrors } from "../../utils/displayApiErrors";
import fetchAPI from "../../utils/fetchAPI";
import "./FullPost.scss";
import { useEffect, useState } from "react";
import Spinner from "../Spinner/Spinner";
import { Close, Delete, Edit } from "@mui/icons-material";
import { FullPostImg } from "../../utils/CloudImg";
import Avatar from "../Avatar/Avatar";
import { redirect } from "react-router-dom";
import FavoriteIcon from "@mui/icons-material/Favorite";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import CommentIcon from "@mui/icons-material/Comment";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
} from "@mui/material";

interface FullPostProps {
  postId: number;
  onClose: () => void;
  onPostDeleted: (postId: number) => void;
}

const FullPost: React.FC<FullPostProps> = ({
  postId,
  onClose,
  onPostDeleted,
}) => {
  const [post, setPost] = useState<FullPostDto | null>(null);
  const [comments, setComments] = useState([]);
  const [page, setPage] = useState(1);
  const [hasMoreComments, setHasMoreComments] = useState(true);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [liked, setLiked] = useState(false);
  const [likesCount, setLikesCount] = useState(0);
  const [isLikeLoading, setIsLikeLoading] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);

  useEffect(() => {
    setLoading(true);
    const fetchPost = async () => {
      try {
        const postResponse = await fetchAPI<FullPostDto>(`posts/${postId}`, {
          method: "GET",
        });
        setPost(postResponse);
        setLiked(postResponse.isLiked);
        setLikesCount(postResponse.likesCount);
      } catch (err) {
        handleApiErrors(err);
        redirect("/feed");
      } finally {
        setLoading(false);
      }
    };

    fetchPost();
  }, [postId]);

  const handleLike = async () => {
    if (isLikeLoading) {
      return;
    }
    setIsLikeLoading(true);
    setLiked((prevLiked) => !prevLiked);
    try {
      if (liked) {
        await fetchAPI(`posts/unlike/${postId}`, {
          method: "DELETE",
        });
        setLiked(false);
        setLikesCount((prevCount) => prevCount - 1);
      } else {
        await fetchAPI(`posts/like/${postId}`, {
          method: "POST",
        });
        setLiked(true);
        setLikesCount((prevCount) => prevCount + 1);
      }
    } catch (err) {
      handleApiErrors(err);
    } finally {
      setIsLikeLoading(false);
    }
  };

  const handleDelete = async () => {
    try {
      await fetchAPI(`posts/delete/${postId}`, {
        method: "DELETE",
      });
      onPostDeleted(postId);
      onClose();
    } catch (err) {
      handleApiErrors(err);
    }
  };

  if (error || post == null) {
    return null;
  }

  return (
    <div className="post-popup-container">
      <div className="post-popup">
        {loading && <Spinner />}
        <button onClick={onClose} className="close-btn">
          <Close className="icon" />
        </button>
        <div className="post-content">
          <div className="post-image">
            <FullPostImg publicId={post.postPicture} />
          </div>
          <div className="post-details">
            <div className="post-header">
              <div className="post-user">
                <Avatar photoUrl={post.profilePicture} />
                <div className="user-info">
                  <h2>{post.userName}</h2>
                  <p>{new Date(post.createdAt).toLocaleString()}</p>
                </div>
              </div>

              {post.isOwner && (
                <button className="edit-btn">
                  <Edit className="icon" />
                </button>
              )}
              {post.canDelete && (
                <button
                  className="delete-btn"
                  onClick={() => setIsDeleteDialogOpen(true)}
                >
                  <Delete className="icon" />
                </button>
              )}
            </div>
            <p className="post-text-content">{post.content}</p>
            <div className="like-comment-counts">
              <div className="counts">
                <FavoriteIcon className="counts-icon" />
                <span className="counts-icon-text">{likesCount}</span>
              </div>
              <div className="counts">
                <CommentIcon className="counts-icon" />
                <span className="counts-icon-text">{post.commentsCount}</span>
              </div>
            </div>
            <div className="post-bottom post-counts-borders">
              <button
                type="button"
                className="post-actions"
                onClick={handleLike}
              >
                {liked ? (
                  <FavoriteIcon className="icons" />
                ) : (
                  <FavoriteBorderIcon className="icons" />
                )}
                <p className="post-count">Like</p>
              </button>
              <button type="button" className="post-actions">
                <CommentIcon className="icons" />
                <p className="post-count">Comment</p>
              </button>
            </div>

            <div className="comments-section"></div>
          </div>
        </div>
      </div>

      <Dialog
        open={isDeleteDialogOpen}
        onClose={() => setIsDeleteDialogOpen(false)}
      >
        <DialogTitle className="dialog-title">Delete Post</DialogTitle>
        <DialogContent>
          <DialogContentText className="dialog-text">
            Are you sure you want to delete this post?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            className="dialog-button"
            onClick={() => setIsDeleteDialogOpen(false)}
            color="primary"
          >
            Cancel
          </Button>
          <Button
            className="dialog-button"
            onClick={handleDelete}
            color="secondary"
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default FullPost;
