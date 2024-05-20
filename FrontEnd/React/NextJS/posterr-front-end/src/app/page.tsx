"use client"

import Image from "next/image";
import styles from "./page.module.css";
import { usePostsRequest } from "@/hooks/usePostsRequest";

export default function Home() {
  const { isLoading, posts, error } = usePostsRequest(1);

  return (
    <main>
      <h1>Home</h1>
      <ul>
        {posts.map(post => (
          <li key={`${post.postId}-${post.repostId ?? 'original'}`}>
            {post.publicationDate.toLocaleDateString()} - {post.authorUsername} - {post.content}
          </li>
        ))}
      </ul>
    </main>
  );
}
