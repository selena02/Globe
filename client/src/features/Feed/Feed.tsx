import "./Feed.scss";
import { useCallback, useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import { fetchPosts, removePost } from "../../state/features/postsSlice";
import type { AppDispatch, RootState } from "../../state/store";
import Post from "../../shared/components/Post/Post";
import Spinner from "../../shared/components/Spinner/Spinner";

const Feed = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { posts, pageNumber, hasMore, isLoading, error } = useSelector(
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

  const handlePostDeleted = (postId: number) => {
    dispatch(removePost(postId));
  };

  return (
    <div className="feed-container">
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
