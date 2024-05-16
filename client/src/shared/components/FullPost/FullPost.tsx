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
import { set } from "react-hook-form";

const FullPost = ({
  postId,
  onClose,
}: {
  postId: number;
  onClose: () => void;
}) => {
  const [post, setPost] = useState<FullPostDto | null>(null);
  const [comments, setComments] = useState([]);
  const [page, setPage] = useState(1);
  const [hasMoreComments, setHasMoreComments] = useState(true);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [liked, setLiked] = useState(false);
  const [likesCount, setLikesCount] = useState(0);

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
                <button className="delete-btn">
                  <Delete className="icon" />
                </button>
              )}
            </div>
            <p className="post-text-content">{post.content}</p>
            <div className="like-comment-counts">
              <div className="counts">
                <FavoriteIcon className="counts-icon" />
                <span className="counts-icon-text">{post.likesCount}</span>
              </div>
              <div className="counts">
                <CommentIcon className="counts-icon" />
                <span className="counts-icon-text">{post.commentsCount}</span>
              </div>
            </div>
            <div className="post-bottom post-counts-borders">
              <button type="button" className="post-actions">
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
    </div>
  );
};

export default FullPost;
