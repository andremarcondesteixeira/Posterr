"use client"

import { usePostsPagination } from "@/hooks/usePostsPagination";
import styles from "./page.module.css";

export default function Home() {
  const { isLoadingPosts, posts, errorLoadingPosts } = usePostsPagination(1);

  return (
    <main className={styles.main}>
      <ul className={styles.publicationsList}>
        {isLoadingPosts && (
          <span>Loading posts...</span>
        )}
        {posts && posts._embedded.publications.map(post => (
          <li key={`${post.postId}-${post.repostAuthorUsername ?? 'original'}`}>
            <article className={styles.post}>
              <span className={styles.postAuthor}>{post.postAuthorUsername}</span>
              <span className={styles.postPublicationDate}>{new Date(post.postPublicationDate).toLocaleString()}</span>
              <span className={styles.postContent}>{post.postContent}</span>
            </article>
          </li>
        ))}
        {errorLoadingPosts && (
          <span>There was an error when loading the posts: {errorLoadingPosts.message}</span>
        )}
      </ul>
    </main>
  );
}
