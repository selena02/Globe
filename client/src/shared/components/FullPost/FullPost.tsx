import { FullPostDto } from "../../models/Post";
import { handleApiErrors } from "../../utils/displayApiErrors";
import fetchAPI from "../../utils/fetchAPI";
import "./FullPost.scss";
import { useEffect, useState } from "react";
import Spinner from "../Spinner/Spinner";
import { Close, Delete, Edit } from "@mui/icons-material";
import { FullPostImg } from "../../utils/CloudImg";
import Avatar from "../Avatar/Avatar";

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

  useEffect(() => {
    setLoading(true);
    const fetchPost = async () => {
      try {
        const postResponse = await fetchAPI<FullPostDto>(`posts/${postId}`, {
          method: "GET",
        });
        setPost(postResponse);
        console.log(postResponse);
      } catch (err) {
        handleApiErrors(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPost();
  }, [postId]);

  if (loading) return <Spinner />;
  if (error || post == null)
    return <p>Error loading post. Please try again.</p>;

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
              <Avatar photoUrl={post.profilePicture} />
              <div className="user-info">
                <h2>{post.userName}</h2>
                <p>{new Date(post.createdAt).toLocaleString()}</p>
              </div>
              {post.isOwner && (
                <button className="edit-btn">
                  <Edit />
                </button>
              )}
              {post.canDelete && (
                <button className="delete-btn">
                  <Delete />
                </button>
              )}
            </div>
            <p>{post.content}</p>
            <div className="comments-section">
              <h3>Comments ({post.commentsCount})</h3>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FullPost;
