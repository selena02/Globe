import "./Post.scss";
import React from "react";
import FavoriteIcon from "@mui/icons-material/Favorite";
import CommentIcon from "@mui/icons-material/Comment";
import { PostDto } from "../../../shared/models/Post";
import Avatar from "../Avatar/Avatar";
import { PostImg } from "../../utils/CloudImg";

interface PostProps {
  post: PostDto;
}

const Post: React.FC<PostProps> = ({ post }) => {
  return (
    <div className="post-container">
      <div className="post-img">
        <PostImg publicId={post.postPicture} />
      </div>

      <div className="post-footer">
        <Avatar photoUrl={post.profilePicture} />
        <div className="user-name">{post.userName}</div>
        <div className="icon-container">
          <FavoriteIcon className="icon" color="error" />
          <span className="icon-text">{post.likesCount}</span>
        </div>
        <div className="icon-container">
          <CommentIcon className="icon" color="primary" />
          <span className="icon-text">{post.commentsCount}</span>
        </div>
      </div>
    </div>
  );
};

export default Post;
