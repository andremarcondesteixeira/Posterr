"use client"

import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { REQUEST_ABORTED } from "@Core/Services/HttpRequestService";
import { FormEvent, useContext, useEffect, useRef, useState } from "react";
import { DefaultAuthorUsernameContext } from "./DefaultAuthorUsernameContext";
import styles from "./page.module.css";

export default function Home() {
  const [newPostContent, setNewPostContent] = useState("");
  const [publications, setPublications] = useState<PublicationAPIResource[]>([]);
  const feedEndElementRef = useRef<HTMLParagraphElement | null>(null);
  const { defaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);

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

    if (!defaultAuthorUsername) {
      alert("Please select the author for the new post");
      return;
    }

    const response = await ApiEndpoint.publications.POST(defaultAuthorUsername, newPostContent);
    response.match({
      error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      ok: publication => setPublications(prev => [publication, ...prev]),
    });
  }

  async function startRepost() {

  }

  return (
    <main className={styles.main}>
      <form className={styles.newPostForm} onSubmit={tryCreateNewPost}>
        <textarea placeholder="What are your thoughts?" value={newPostContent} onChange={(event) => setNewPostContent(event.target.value)} />
        <button className="primary">Publish</button>
      </form>
      <ul className={styles.publicationsList}>
        {publications && publications.map(post => (
          <li key={post.id}>
            <article className={`${styles.post} ${post.isRepost && styles.repost}`}>
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
              {!post.isRepost && (
                <>
                  <hr />
                  <section className={styles.postActions}>
                    <button className="transparent" type="button" onClick={startRepost}>
                      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                        <path d="M5 4a2 2 0 0 0-2 2v6H0l4 4 4-4H5V6h7l2-2H5zm10 4h-3l4-4 4 4h-3v6a2 2 0 0 1-2 2H6l2-2h7V8z"/>
                      </svg>
                      Repost
                    </button>
                  </section>
                </>
              )}
            </article>
          </li>
        ))}
      </ul>
      <p ref={feedEndElementRef}>That's all for today!</p>
    </main>
  );
}
