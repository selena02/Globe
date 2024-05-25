import { useCallback, useEffect, useRef, useState } from "react";
import paginatedFetchAPI from "../../../shared/utils/paginatedFetchAPI";
import Post from "../../../shared/components/Post/Post";
import Spinner from "../../../shared/components/Spinner/Spinner";
import { PostDto } from "../../../shared/models/Post";
import "./Posts.scss";
import { useDispatch } from "react-redux";
import { removePost } from "../../../state/features/postsSlice";
import { useParams } from "react-router-dom";

const Posts = () => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const [posts, setPosts] = useState<PostDto[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(6);
  const [totalPages, setTotalPages] = useState(0);
  const [hasMore, setHasMore] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const observerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const fetchPosts = async (page: number) => {
      setIsLoading(true);
      setError(null);

      try {
        const endpoint = `posts/user/${id}?pageSize=${pageSize}&pageNumber=${page}`;
        const { data, pagination } = await paginatedFetchAPI<{
          posts: PostDto[];
          pagination: { totalPages: number };
        }>(endpoint);

        setPosts((prevPosts) => [...prevPosts, ...data.posts]);
        setTotalPages(pagination.totalPages);
        setHasMore(page < pagination.totalPages);
      } catch (err: any) {
        setError(err.message || "Something went wrong");
      } finally {
        setIsLoading(false);
      }
    };

    fetchPosts(pageNumber);
  }, [pageNumber, pageSize, id]);

  const handleObserver = useCallback(
    (entries: IntersectionObserverEntry[]) => {
      const entry = entries[0];
      if (entry.isIntersecting && hasMore && !isLoading) {
        setPageNumber((prevPageNumber) => prevPageNumber + 1);
      }
    },
    [hasMore, isLoading]
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
    setPosts((prevPosts) => prevPosts.filter((post) => post.postId !== postId));
    dispatch(removePost(postId));
  };

  return (
    <div className="feed-container">
      <div className="profile-posts-container">
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

export default Posts;
