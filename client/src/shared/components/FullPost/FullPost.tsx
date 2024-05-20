import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { handleApiErrors } from "../../utils/displayApiErrors";
import paginatedFetchAPI from "../../utils/paginatedFetchAPI";
import fetchAPI from "../../utils/fetchAPI";
import Spinner from "../Spinner/Spinner";
import { Close, Delete, Edit } from "@mui/icons-material";
import { FullPostImg } from "../../utils/CloudImg";
import Avatar from "../Avatar/Avatar";
import { redirect } from "react-router-dom";
import FavoriteIcon from "@mui/icons-material/Favorite";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
} from "@mui/material";
import Comment from "../Comment/Comment";
import "./FullPost.scss";
import { CommentDto } from "../../models/Comment";
import { FullPostDto } from "../../models/Post";
import { RootState } from "../../../state/store";
import { useSelector } from "react-redux";

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
  const navigate = useNavigate();
  const [post, setPost] = useState<FullPostDto | null>(null);
  const [comments, setComments] = useState<CommentDto[]>([]);
  const [page, setPage] = useState(1);
  const [hasMoreComments, setHasMoreComments] = useState(true);
  const [loading, setLoading] = useState(true);
  const [loadingMoreComments, setLoadingMoreComments] = useState(false);
  const [error, setError] = useState(null);
  const [liked, setLiked] = useState(false);
  const [likesCount, setLikesCount] = useState(0);
  const [isLikeLoading, setIsLikeLoading] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [commentToDelete, setCommentToDelete] = useState<CommentDto | null>(
    null
  );
  const [isCommentDeleteDialogOpen, setIsCommentDeleteDialogOpen] =
    useState(false);
  const [newComment, setNewComment] = useState("");
  const [isCommentUploading, setIsCommentUploading] = useState(false);
  const isLoggedIn = useSelector((state: RootState) => state.auth.isLoggedIn);

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

  useEffect(() => {
    fetchComments();
  }, [page]);

  const fetchComments = async () => {
    setLoadingMoreComments(true);
    try {
      const { data, pagination } = await paginatedFetchAPI<{
        comments: CommentDto[];
        pagination: { totalPages: number };
      }>(`comments/post/${postId}?pageSize=3&pageNumber=${page}`, {
        method: "GET",
      });
      setComments((prevComments) => [...prevComments, ...data.comments]);
      setHasMoreComments(page < pagination.totalPages);
    } catch (err) {
      handleApiErrors(err);
      setHasMoreComments(false);
    } finally {
      setLoadingMoreComments(false);
    }
  };

  const handleLoadMoreComments = () => {
    if (hasMoreComments) {
      setPage((prevPage) => prevPage + 1);
    }
  };

  const handleLike = async () => {
    if (!isLoggedIn) {
      navigate("/account/login");
      return;
    }
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
    if (!isLoggedIn) {
      navigate("/account/login");
      return;
    }
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

  const handleCommentDeleted = (commentId: number) => {
    setComments((prevComments) =>
      prevComments.filter((comment) => comment.commentId !== commentId)
    );
  };

  const openCommentDeleteDialog = (comment: CommentDto) => {
    if (!isLoggedIn) {
      navigate("/account/login");
      return;
    }
    setCommentToDelete(comment);
    setIsCommentDeleteDialogOpen(true);
  };

  const confirmDeleteComment = async () => {
    if (commentToDelete) {
      try {
        await fetchAPI(`comments/${commentToDelete.commentId}`, {
          method: "DELETE",
        });
        handleCommentDeleted(commentToDelete.commentId);
      } catch (err) {
        handleApiErrors(err);
      } finally {
        setIsCommentDeleteDialogOpen(false);
      }
    }
  };

  const handleCommentUpload = async () => {
    if (!isLoggedIn) {
      navigate("/account/login");
      return;
    }
    if (isCommentUploading || newComment.trim() === "") {
      return;
    }
    setIsCommentUploading(true);
    try {
      const commentResponse = await fetchAPI<{ comment: CommentDto }>(
        `comments/upload`,
        {
          method: "POST",
          body: { PostId: postId, Content: newComment },
        }
      );
      setComments((prevComments) => [...prevComments, commentResponse.comment]);
      setNewComment("");
    } catch (err) {
      handleApiErrors(err);
    } finally {
      setIsCommentUploading(false);
    }
  };

  if (loading) {
    return (
      <div className="spinner-container">
        <Spinner />
      </div>
    );
  }

  if (error || post == null) {
    return null;
  }

  return (
    <div className="post-popup-container">
      <div className="post-popup">
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

              {/* {post.isOwner && (
                <button className="edit-btn">
                  <Edit className="icon" />
                </button>
              )} */}
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
                <ChatBubbleOutlineIcon className="counts-icon" />
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
                <ChatBubbleOutlineIcon className="icons" />
                <p className="post-count">Comment</p>
              </button>
            </div>

            <div className="comments-section">
              {loadingMoreComments && (
                <span className="comment-spinner">
                  <Spinner />
                </span>
              )}
              {hasMoreComments && !loadingMoreComments && (
                <button
                  onClick={handleLoadMoreComments}
                  className="load-more-btn"
                >
                  Load More Comments
                </button>
              )}
              {comments.map((comment) => (
                <Comment
                  key={comment.commentId}
                  comment={comment}
                  onCommentDeleted={handleCommentDeleted}
                  onOpenDeleteDialog={openCommentDeleteDialog}
                />
              ))}
            </div>
            <div className="upload-comment-container">
              <input
                type="text"
                placeholder={
                  isCommentUploading ? "Loading..." : "Write a comment..."
                }
                className="comment-input"
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                disabled={isCommentUploading}
              />
              <button
                className="upload-comment-btn"
                onClick={handleCommentUpload}
                disabled={isCommentUploading}
              >
                Post
              </button>
            </div>
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

      <Dialog
        open={isCommentDeleteDialogOpen}
        onClose={() => setIsCommentDeleteDialogOpen(false)}
      >
        <DialogTitle className="dialog-title">Delete Comment</DialogTitle>
        <DialogContent>
          <DialogContentText className="dialog-text">
            Are you sure you want to delete this comment?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            className="dialog-button"
            onClick={() => setIsCommentDeleteDialogOpen(false)}
            color="primary"
          >
            Cancel
          </Button>
          <Button
            className="dialog-button"
            onClick={confirmDeleteComment}
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
