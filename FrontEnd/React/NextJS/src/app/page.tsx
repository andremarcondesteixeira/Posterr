"use client"

import { ContainerBand } from "@/components/ContainerBand";
import { Loading } from "@/components/Loading";
import { NewPublicationForm } from "@/components/NewPublicationForm";
import { PublicationsList } from "@/components/PublicationsList";
import { SearchForm } from "@/components/SearchForm";
import { Publication } from "@CoreDomain/Entities/types";
import { ListPublicationsUseCase } from "@CoreDomain/UseCases";
import { PosterrAPIErrorResponse, PublicationAPIResource } from "@CoreTypes";
import { useEffect, useRef, useState } from "react";
import styles from "./page.module.css";

export default function Home() {
  const [publications, setPublications] = useState<PublicationAPIResource[]>([]);
  const [originalPostForRepost, setOriginalPostForRepost] = useState<Publication | null>(null);
  const feedEndElementRef = useRef<HTMLParagraphElement | null>(null);

  useEffect(() => {
    const abortController = new AbortController();
    loadPublications(0, abortController.signal);

    return () => {
      if (!abortController.signal.aborted) {
        abortController.abort();
      }
    }
  }, []);

  useEffect(() => {
    if (!feedEndElementRef.current || publications.length === 0) return;

    const noMorePostsToBeLoaded = publications.length > 0 && publications[publications.length - 1].id === 1;
    if (noMorePostsToBeLoaded) {
      feedEndElementRef.current.innerText = "That's all for today!";
      return;
    }

    const currentRef = feedEndElementRef.current;
    const abortController = new AbortController();
    let isLoading = false;

    const onIntersect: IntersectionObserverCallback = entries => {
      if (!entries[0].isIntersecting || isLoading) return;
      isLoading = true;
      const lastSeenPublicationId = publications[publications.length - 1].id;
      loadPublications(lastSeenPublicationId, abortController.signal, () => {
        isLoading = false;
      });
    };
    const intersectionObserver = new IntersectionObserver(onIntersect);
    intersectionObserver.observe(currentRef);

    return () => {
      abortController.abort();
      intersectionObserver.unobserve(currentRef);
    };
  }, [publications]);

  async function loadPublications(lastSeenPublicationId: number, abortSignal: AbortSignal, onSuccess?: () => void) {
    const response = await ListPublicationsUseCase(lastSeenPublicationId, abortSignal);

    response.match({
      ok(response) {
        setPublications(prev => [...prev, ...response._embedded.publications]);
        onSuccess?.();
      },
      error(error) {
        // At this point, things should not go wrong if everything is wired correctly.
        // Since this is only a test, I'll just alert the error if something happens
        if (typeof error === "string") {
          alert(error);
        } else if (error instanceof PosterrAPIErrorResponse) {
          alert(`${error.cause.title}\n${error.cause.detail}`);
        }
      }
    });
  }

  async function startRepost(originalPost: Publication) {
    window.scrollTo({
      behavior: "smooth",
      top: 0,
    });
    document.getElementById('newPublicationContent')?.focus({
      preventScroll: true,
    });
    setOriginalPostForRepost(originalPost);
  }

  return (
    <section className={styles.main}>
      <ContainerBand>
        <NewPublicationForm
          setPublications={setPublications}
          originalPost={originalPostForRepost}
          setOriginalPost={setOriginalPostForRepost}
          cancelRepostAction={() => setOriginalPostForRepost(null)}
        />
      </ContainerBand>
      <div className={styles.additionalFeedActions}>
        <SearchForm />
      </div>
      <PublicationsList publications={publications} startRepostAction={startRepost} />
      <Loading ref={feedEndElementRef} />
    </section>
  );
}
