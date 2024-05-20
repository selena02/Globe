import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import paginatedFetchAPI from "../../shared/utils/paginatedFetchAPI";
import { PostDto } from "../../shared/models/Post";

interface FetchPostsResponse {
  posts: PostDto[];
  pageNumber: number;
  totalPages: number;
}

export interface PostsState {
  posts: PostDto[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  isLoading: boolean;
  hasMore: boolean;
  error: string | null;
}

const initialState: PostsState = {
  posts: [],
  pageNumber: 1,
  pageSize: 10,
  totalPages: 0,
  isLoading: false,
  hasMore: true,
  error: null,
};

export const fetchPosts = createAsyncThunk<FetchPostsResponse, number>(
  "posts/fetchPosts",
  async (pageNumber, { getState, rejectWithValue }) => {
    try {
      const state = getState() as { posts: PostsState };
      const endpoint = `posts?pageSize=${state.posts.pageSize}&pageNumber=${pageNumber}`;
      const { data, pagination } = await paginatedFetchAPI<{
        posts: PostDto[];
        pagination: { totalPages: number };
      }>(endpoint);
      return {
        posts: data.posts,
        pageNumber,
        totalPages: pagination.totalPages,
      };
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

const postsSlice = createSlice({
  name: "posts",
  initialState,
  reducers: {
    resetPosts: (state) => initialState,
    removePost: (state, action: PayloadAction<number>) => {
      state.posts = state.posts.filter(
        (post) => post.postId !== action.payload
      );
    },
    addPost: (state, action: PayloadAction<PostDto>) => {
      state.posts = [action.payload, ...state.posts];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchPosts.pending, (state) => {
        state.isLoading = true;
      })
      .addCase(
        fetchPosts.fulfilled,
        (state, action: PayloadAction<FetchPostsResponse>) => {
          state.posts = [...state.posts, ...action.payload.posts];
          state.pageNumber = action.payload.pageNumber;
          state.totalPages = action.payload.totalPages;
          state.hasMore = action.payload.pageNumber < action.payload.totalPages;
          state.isLoading = false;
        }
      )
      .addCase(fetchPosts.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      });
  },
});

export const { resetPosts, removePost, addPost } = postsSlice.actions;
export default postsSlice.reducer;
