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
        {posts && posts.map(post => (
          <li key={`${post.postId}-${post.repostId ?? 'original'}`}>
            <span>ID: {post.postId}</span>
            <span>Author: {post.authorUsername}</span>
            <span>Published on {post.publicationDate.toLocaleDateString()}</span>
            <span>{post.content}</span>
          </li>
        ))}
        {errorLoadingPosts && (
          <span>There was an error when loading the posts: {errorLoadingPosts.message}</span>
        )}
      </ul>
    </main>
  );
}
