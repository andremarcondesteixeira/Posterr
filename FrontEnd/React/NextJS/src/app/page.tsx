"use client"

import type { Publication } from "@Core/Domain/Entities/types";
import { ApiEndpoint } from "@Core/Services/ApiEndpointsService";
import { FormEvent, useEffect, useState } from "react";
import styles from "./page.module.css";

const defaultAuthorUsername = process.env["NEXT_PUBLIC_DEFAULT_USERNAME"] as string;

export default function Home() {
  const [newPostContent, setNewPostContent] = useState("");
  const [publications, setPublications] = useState<Publication[]>([]);

  useEffect(() => {
    ApiEndpoint.publications.GET(1).then(response => {
      response.match({
        ok: response => setPublications(response._embedded.publications),
        error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      });
    });
  }, []);

  const tryCreateNewPost = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const response = await ApiEndpoint.publications.POST({
      authorUsername: defaultAuthorUsername,
      content: newPostContent
    });
    response.match({
      error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      ok: publication => setPublications(prev => [publication, ...prev]),
    });
  };

  return (
    <main className={styles.main}>
      <form method="post" action="/api/posts" className={styles.newPostForm} onSubmit={tryCreateNewPost}>
        <textarea placeholder="What are your thoughts?" value={newPostContent} onChange={(event) => setNewPostContent(event.target.value)} />
        <button>Publish</button>
      </form>
      <ul className={styles.publicationsList}>
        {publications && publications.map(post => (
          <li key={post.id}>
            <article className={styles.post}>
              <span className={styles.postAuthor}>{post.authorUsername}</span>
              <span className={styles.postPublicationDate}>{new Date(post.publicationDate).toLocaleString()}</span>
              <span className={styles.postContent}>{post.content}</span>
              {post.isRepost && (
                <div className={styles.post}>
                  <span className={styles.postAuthor}>{post.originalPostAuthorUsername}</span>
                  <span className={styles.postPublicationDate}>{new Date(post.originalPostPublicationDate!).toLocaleString()}</span>
                  <span className={styles.postContent}>{post.originalPostContent}</span>
                </div>
              )}
            </article>
          </li>
        ))}
      </ul>
    </main>
  );
}
