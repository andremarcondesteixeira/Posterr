"use client"

import { NewPublicationForm } from "@/components/NewPublicationForm";
import { Publication } from "@/components/Publication";
import type { PublicationEntity } from "@Core/Domain/Entities/types";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { REQUEST_ABORTED } from "@Core/Services/HttpRequestService";
import { useEffect, useRef, useState } from "react";
import styles from "./page.module.css";

export default function Home() {
  const [publications, setPublications] = useState<PublicationAPIResource[]>([]);
  const [originalPostForRepost, setOriginalPostForRepost] = useState<PublicationEntity | null>(null);
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

  async function startRepost(originalPost: PublicationEntity) {
    setOriginalPostForRepost(originalPost);
  }

  return (
    <main className={styles.main}>
      <NewPublicationForm setPublications={setPublications} originalPost={originalPostForRepost} />
      <ul className={styles.publicationsList}>
        {publications && publications.map(publication => (
          <li key={publication.id}>
            <Publication publication={publication}>
              {!publication.isRepost && (
                <button className="transparent" type="button" onClick={() => startRepost(publication)}>
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                    <path d="M5 4a2 2 0 0 0-2 2v6H0l4 4 4-4H5V6h7l2-2H5zm10 4h-3l4-4 4 4h-3v6a2 2 0 0 1-2 2H6l2-2h7V8z" />
                  </svg>
                  Repost
                </button>
              )}
            </Publication>
          </li>
        ))}
      </ul>
      <p ref={feedEndElementRef}>That's all for today!</p>
    </main>
  );
}
