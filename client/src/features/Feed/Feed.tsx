import { useState, useEffect } from "react";
import { ApiError } from "../../shared/models/ApiError";
import { PostDto } from "../../shared/models/Post";
import fetchAPI from "../../shared/utils/fetchAPI";

const Feed = () => {
  const [posts, setPosts] = useState<PostDto[]>([]);
  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);

  useEffect(() => {
    fetchPosts();
  }, [pageNumber, pageSize]);

  const fetchPosts = async () => {
    try {
      const endpoint = `posts?pageSize=${pageSize}&pageNumber=${pageNumber}`;
      const data = await fetchAPI<{
        posts: PostDto[];
        pagination: { totalPages: number };
      }>(endpoint, { method: "GET" });
      console.log(data);
      setPosts(data.posts);
    } catch (error) {
      console.error("Failed to fetch posts:", error);
      if (error instanceof ApiError) {
      }
    }
  };

  return (
    <div>
      <button
        onClick={() => setPageNumber(pageNumber - 1)}
        disabled={pageNumber === 1}
      >
        Previous
      </button>
      <button onClick={() => setPageNumber(pageNumber + 1)}>Next</button>
    </div>
  );
};

export default Feed;
