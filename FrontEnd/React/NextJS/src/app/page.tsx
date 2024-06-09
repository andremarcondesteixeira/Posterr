"use client"

import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { REQUEST_ABORTED } from "@Core/Services/HttpRequestService";
import { FormEvent, useEffect, useRef, useState } from "react";
import styles from "./page.module.css";

const defaultAuthorUsername = process.env["NEXT_PUBLIC_DEFAULT_USERNAME"] as string;

export default function Home() {
  const [newPostContent, setNewPostContent] = useState("");
  const [isFirstPage, setIsFirstPage] = useState(true);
  const [publications, setPublications] = useState<PublicationAPIResource[]>([]);
  const feedEndElementRef = useRef<HTMLParagraphElement | null>(null);

  useEffect(() => {
    const abortController = new AbortController();
    loadNextPublicationsPage(0, abortController.signal, () => {
      setIsFirstPage(false);
    });
    return () => abortController.abort(REQUEST_ABORTED);
  }, []);

  useEffect(() => {
    if (!feedEndElementRef.current || isFirstPage) return;

    const abortController = new AbortController();
    const onIntersect = () => {
      if (publications.length === 0) return;
      const lastSeenPublicationId = publications[publications.length - 1].id;
      loadNextPublicationsPage(lastSeenPublicationId, abortController.signal);
    }
    const intersectionCfg = { rootMargin: '0px 0px 500px 0px' };
    const intersectionObserver = new IntersectionObserver(onIntersect, intersectionCfg);
    intersectionObserver.observe(feedEndElementRef.current!);

    return () => {
      abortController.abort(REQUEST_ABORTED);
      intersectionObserver.unobserve(feedEndElementRef.current!);
    };
  }, [isFirstPage, publications.length]);

  function loadNextPublicationsPage(lastSeenPublicationId: number, abortSignal: AbortSignal, onSuccess?: () => void) {
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
    const response = await ApiEndpoint.publications.POST(defaultAuthorUsername, newPostContent);
    response.match({
      error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      ok: publication => setPublications(prev => [publication, ...prev]),
    });
  }

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
      <p ref={feedEndElementRef}>That's all for today!</p>
    </main>
  );
}
