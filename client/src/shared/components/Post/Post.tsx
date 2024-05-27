import "./Post.scss";
import React, { useState } from "react";
import FavoriteIcon from "@mui/icons-material/Favorite";
import CommentIcon from "@mui/icons-material/Comment";
import { PostDto } from "../../../shared/models/Post";
import Avatar from "../Avatar/Avatar";
import { PostImg } from "../../utils/CloudImg";
import { Link } from "react-router-dom";
import FullPost from "../FullPost/FullPost";

interface PostProps {
  post: PostDto;
  onPostDeleted: (postId: number) => void;
}

const Post: React.FC<PostProps> = ({ post, onPostDeleted }) => {
  const [showPopup, setShowPopup] = useState(false);

  const handlePictureClick = () => {
    setShowPopup(true);
  };

  const handleClosePopup = () => {
    setShowPopup(false);
  };

  return (
    <div className="post-container">
      <div className="post-img" onClick={handlePictureClick}>
        <PostImg publicId={post.postPicture} />
        <div className="post-footer">
          <Link to={`/profile/${post.userId}`} className="footer-left">
            <Avatar photoUrl={post.profilePicture} />
            <div className="user-name">{post.userName}</div>
          </Link>
          <div className="footer-right">
            <div className="icon-container">
              <FavoriteIcon className="icon" />
              <span className="icon-text">{post.likesCount}</span>
            </div>
            <div className="icon-container">
              <CommentIcon className="icon" />
              <span className="icon-text">{post.commentsCount}</span>
            </div>
          </div>
        </div>
      </div>
      {showPopup && (
        <FullPost
          postId={post.postId}
          onClose={handleClosePopup}
          onPostDeleted={onPostDeleted}
        />
      )}
    </div>
  );
};

export default Post;
