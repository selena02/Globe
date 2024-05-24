import React, { useCallback, useEffect, useRef, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import {
  fetchPosts,
  removePost,
  addPost,
} from "../../state/features/postsSlice";
import type { AppDispatch, RootState } from "../../state/store";
import Post from "../../shared/components/Post/Post";
import Spinner from "../../shared/components/Spinner/Spinner";
import UploadPost from "./UploadPost/UploadPost";
import { PostDto } from "../../shared/models/Post";
import "./Feed.scss";

const Feed = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { posts, pageNumber, hasMore, isLoading, error } = useSelector(
    (state: RootState) => state.posts
  );
  const isLoggedIn = useSelector((state: RootState) => state.auth.isLoggedIn);
  const observerRef = useRef<HTMLDivElement | null>(null);

  const [isUploadPostVisible, setIsUploadPostVisible] = useState(false);

  useEffect(() => {
    if (pageNumber === 1 && posts.length === 0) {
      dispatch(fetchPosts(pageNumber));
    }
  }, [dispatch, pageNumber, posts.length]);

  const handleObserver = useCallback(
    (entries: IntersectionObserverEntry[]) => {
      const entry = entries[0];
      if (entry.isIntersecting && hasMore && !isLoading) {
        dispatch(fetchPosts(pageNumber + 1));
      }
    },
    [dispatch, hasMore, isLoading, pageNumber]
  );

  useEffect(() => {
    const observer = new IntersectionObserver(handleObserver);
    if (observerRef.current) {
      observer.observe(observerRef.current);
    }

    return () => {
      if (observerRef.current) {
        observer.unobserve(observerRef.current);
      }
    };
  }, [handleObserver]);

  const handlePostDeleted = (postId: number) => {
    dispatch(removePost(postId));
  };

  const handlePostUploaded = (post: PostDto) => {
    dispatch(addPost(post));
    setIsUploadPostVisible(false);
  };

  return (
    <div className="feed-container">
      {isLoggedIn && (
        <button
          className="show-upload-post-button"
          onClick={() => setIsUploadPostVisible(true)}
        >
          Upload Post
        </button>
      )}
      {isUploadPostVisible && (
        <div className="upload-post-modal">
          <UploadPost
            onPostUploaded={handlePostUploaded}
            onClose={() => setIsUploadPostVisible(false)}
          />
        </div>
      )}
      <div className="posts-container">
        {posts.map((post) => (
          <Post
            key={post.postId}
            post={post}
            onPostDeleted={handlePostDeleted}
          />
        ))}
      </div>
      <div ref={observerRef} style={{ height: "20px" }} />
      <div id="loading-or-error">
        {isLoading && !error && <Spinner />}
        {error && <div className="error-message">{error + " :("}</div>}
      </div>
    </div>
  );
};

export default Feed;
