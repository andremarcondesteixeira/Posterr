"use client"

import { usePostsPagination } from "@/hooks/usePostsPagination";

export default function Home() {
  const { isLoadingPosts, posts, errorLoadingPosts } = usePostsPagination(1);

  return (
    <main>
      <h1>Posterr</h1>
      <ul>
        {isLoadingPosts && (
          <span>Loading posts...</span>
        )}
        {posts && posts._embedded.publications.map(post => (
          <li key={`${post.postId}-${post.repostAuthorUsername ?? 'original'}`}>
            <span>ID: {post.postId}</span>
            <span>Author: {post.postAuthorUsername}</span>
            <span>Published on {new Date(post.postPublicationDate).toLocaleDateString()}</span>
            <span>{post.postContent}</span>
          </li>
        ))}
        {errorLoadingPosts && (
          <span>There was an error when loading the posts: {errorLoadingPosts.message}</span>
        )}
      </ul>
    </main>
  );
}
