"use client"

import { usePostsPagination } from "@/hooks/usePostsPagination";

export default function Home() {
  const { isLoadingPosts, posts, errorLoadingPosts } = usePostsPagination(1);

  return (
    <main>
      <h1>Home</h1>
      <ul>
        {isLoadingPosts && (
          <span>Loading posts...</span>
        )}
        {posts && posts.map(post => (
          <li key={`${post.postId}-${post.repostId ?? 'original'}`}>
            {post.publicationDate.toLocaleDateString()} - {post.authorUsername} - {post.content}
          </li>
        ))}
        {errorLoadingPosts && (
          <span>There was an error when loading the posts: {errorLoadingPosts.message}</span>
        )}
      </ul>
    </main>
  );
}
