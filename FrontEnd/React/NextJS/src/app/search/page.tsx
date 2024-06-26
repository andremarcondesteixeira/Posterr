"use client";

import { ContainerBand } from "@/components/ContainerBand";
import { Loading } from "@/components/Loading";
import { PublicationsList } from "@/components/PublicationsList";
import { SearchForm } from "@/components/SearchForm";
import { Publication } from "@CoreDomain/Entities/types";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { useState } from "react";
import styles from "./page.module.css";

export default function SearchPage() {
  const [publications, setPublications] = useState<Publication[]>([]);
  const searchParams = useSearchParams();
  const [isLoading, setIsLoading] = useState(true);

  return (
    <>
      <ContainerBand className={styles.containerBand}>
        <h1>Search Results</h1>
        <SearchForm initialValue={searchParams.get("searchTerm")} />
        <Link href={"/"}>Back to feed</Link>
      </ContainerBand>
      {isLoading && <Loading />}
      {!isLoading && publications.length > 0 && (
        <PublicationsList publications={publications} startRepostAction={() => { }} />
      )}
      {!isLoading && publications.length === 0 && (
        <p style={{ textAlign: "center" }}>No results found.</p>
      )}
    </>
  );
}
