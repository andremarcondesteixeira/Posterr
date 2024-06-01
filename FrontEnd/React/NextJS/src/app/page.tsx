"use client"

import { Publication } from "@Core/Domain/Entities/Publication";
import { ApiEndpoint } from "@Core/Services/ApiEndpointsService";
import { FormEvent, useEffect, useState } from "react";
import styles from "./page.module.css";

const username = process.env["NEXT_PUBLIC_DEFAULT_USERNAME"] as string;

export default function Home() {
  const [newPostContent, setNewPostContent] = useState("");
  const [publications, setPublications] = useState<Publication[]>([]);

  useEffect(() => {
    ApiEndpoint.publications.GET(1).then(response => {
      response.match({
        ok: (okValue) => {
          setPublications(okValue._embedded.publications);
        },
        error: (errorValue) => {
          alert(errorValue.message);
        },
      });
    });
  }, []);

  const tryCreateNewPost = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const response = ApiEndpoint.posts.POST({ username, content: newPostContent });
  };

  return (
    <main className={styles.main}>
      <form method="post" action="/api/posts" className={styles.newPostForm} onSubmit={tryCreateNewPost}>
        <textarea placeholder="What are your thoughts?" value={newPostContent} onChange={(event) => setNewPostContent(event.target.value)} />
        <button>Publish</button>
      </form>
      <ul className={styles.publicationsList}>
        {publications && publications.map(post => (
          <li key={`${post.postId}-${post.repostAuthorUsername ?? 'original'}`}>
            <article className={styles.post}>
              <span className={styles.postAuthor}>{post.postAuthorUsername}</span>
              <span className={styles.postPublicationDate}>{new Date(post.postPublicationDate).toLocaleString()}</span>
              <span className={styles.postContent}>{post.postContent}</span>
            </article>
          </li>
        ))}
      </ul>
    </main>
  );
}
