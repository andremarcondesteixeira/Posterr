"use client";

import { ContainerBand } from "@/components/ContainerBand";
import { Loading } from "@/components/Loading";
import { PublicationsList } from "@/components/PublicationsList";
import { SearchForm } from "@/components/SearchForm";
import { Publication } from "@CoreDomain/Entities/types";
import { SearchPublicationsUseCase } from "@CoreDomain/UseCases";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import styles from "./page.module.css";

export default function SearchPage() {
  const [publications, setPublications] = useState<Publication[]>([]);
  const searchParams = useSearchParams();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const searchTerm = searchParams.get("searchTerm");

    if (!searchTerm) {
      setIsLoading(false);
      return;
    }

    const abortController = new AbortController();
    SearchPublicationsUseCase(searchTerm, abortController.signal).then(result => {
      result.match({
        ok(value) {
          setPublications(value._embedded.publications);
        },
        error(value) {
          console.error(value);
        },
      });
      setIsLoading(false);
    });
    return () => abortController.abort();
  }, []);

  return (
    <>
      <ContainerBand className={styles.containerBand}>
        <h1>Search Results</h1>
        <SearchForm initialValue={searchParams.get("searchTerm")} />
        <Link href={"/"}>Back to feed</Link>
      </ContainerBand>
      {isLoading && <Loading />}
      {!isLoading && publications.length > 0 && (
        <>
          <p className={styles.resultsDescriptionText}>
            {publications.length} results found:
          </p>
          <PublicationsList publications={publications} startRepostAction={() => { }} />
        </>
      )}
      {!isLoading && publications.length === 0 && (
        <p className={styles.resultsDescriptionText}>No results found.</p>
      )}
    </>
  );
}
