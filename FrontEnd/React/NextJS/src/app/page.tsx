"use client"

import { ApiEndpoint, AuthorAPIResource, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { REQUEST_ABORTED } from "@Core/Services/HttpRequestService";
import { FormEvent, useEffect, useRef, useState } from "react";
import styles from "./page.module.css";

export default function Home() {
  const [newPostContent, setNewPostContent] = useState("");
  const [publications, setPublications] = useState<PublicationAPIResource[]>([]);
  const [users, setUsers] = useState<AuthorAPIResource[]>([]);
  const [selectedAuthorUsername, setSelectedAuthorUsername] = useState<string | null>(null);
  const feedEndElementRef = useRef<HTMLParagraphElement | null>(null);

  useEffect(() => {
    const abortController = new AbortController();
    loadPublications(0, true, abortController.signal);
    return () => abortController.abort(REQUEST_ABORTED);
  }, []);

  useEffect(() => {
    if (!feedEndElementRef.current || publications.length === 0) return;

    const abortController = new AbortController();
    let isLoading = false;
    const onIntersect: IntersectionObserverCallback = entries => {
      if (!entries[0].isIntersecting || isLoading) return;
      isLoading = true;
      const lastSeenPublicationId = publications[publications.length - 1].id;
      loadPublications(lastSeenPublicationId, false, abortController.signal, () => {
        isLoading = false;
      });
    };
    const intersectionObserver = new IntersectionObserver(onIntersect);
    intersectionObserver.observe(feedEndElementRef.current!);

    return () => {
      abortController.abort(REQUEST_ABORTED);
      intersectionObserver.unobserve(feedEndElementRef.current!);
    };
  }, [publications.length]);

  useEffect(() => {
    ApiEndpoint.users.GET().then(response => {
      response.match({
        ok: users => setUsers(users._embedded.users),
        error: error => alert(error.message),
      });
    });
  }, []);

  function loadPublications(lastSeenPublicationId: number, isFirstPage: boolean, abortSignal: AbortSignal, onSuccess?: () => void) {
    ApiEndpoint.publications.GET(lastSeenPublicationId, isFirstPage, abortSignal).then(response => {
      response.match({
        ok(response) {
          setPublications(prev => [...prev, ...response._embedded.publications]);
          onSuccess?.();
        },
        error(error) {
          if (error.cause.title !== REQUEST_ABORTED) {
            alert(`${error.cause.title}\n${error.cause.detail}`);
          }
        }
      });
    });
  }

  async function tryCreateNewPost(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!selectedAuthorUsername) {
      alert("Please select the author for the new post");
      return;
    }

    const response = await ApiEndpoint.publications.POST(selectedAuthorUsername, newPostContent);
    response.match({
      error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      ok: publication => setPublications(prev => [publication, ...prev]),
    });
  }

  return (
    <main className={styles.main}>
      <form className={styles.newPostForm} onSubmit={tryCreateNewPost}>
        <textarea placeholder="What are your thoughts?" value={newPostContent} onChange={(event) => setNewPostContent(event.target.value)} />
        <label className={styles.newPostAuthor}>
          Author
          <select onChange={event => setSelectedAuthorUsername(event.target.value)}>
            <option>--- select ---</option>
            {users.map(user => (
              <option value={user.username} key={user.username}>{user.username}</option>
            ))}
          </select>
        </label>
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
      <p ref={feedEndElementRef}>That's all for today!</p>
    </main>
  );
}
