"use client"

import { NewPublicationForm } from "@/components/NewPublicationForm";
import { PublicationsList } from "@/components/PublicationsList";
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
      <NewPublicationForm
        setPublications={setPublications}
        originalPost={originalPostForRepost}
        cancelRepostAction={() => setOriginalPostForRepost(null)}
      />
      <PublicationsList publications={publications} startRepostAction={startRepost} />
      <p ref={feedEndElementRef}>That's all for today!</p>
    </main>
  );
}
