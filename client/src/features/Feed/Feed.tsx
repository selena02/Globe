import "./Feed.scss";
import React, { useCallback, useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import { PostsState, fetchPosts } from "../../state/features/postsSlice";
import { PostDto } from "../../shared/models/Post";
import type { AppDispatch, RootState } from "../../state/store";
import Post from "../../shared/components/Post/Post";

const Feed = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { posts, pageNumber, hasMore, isLoading } = useSelector(
    (state: RootState) => state.posts
  );
  const observerRef = useRef<HTMLDivElement | null>(null);

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

  return (
    <div className="feed-container">
      <div className="posts-container">
        {posts.map((post) => (
          <Post key={post.postId} post={post} />
        ))}
      </div>
      <div ref={observerRef} style={{ height: "20px" }} />
      {isLoading && <p>Loading...</p>}
    </div>
  );
};

export default Feed;
