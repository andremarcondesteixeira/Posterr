"use client";

import { ContainerBand } from "@/components/ContainerBand";
import { PublicationsList } from "@/components/PublicationsList";
import { SearchForm } from "@/components/SearchForm";
import { useSearchParams } from "next/navigation";
import styles from "./page.module.css";
import Link from "next/link";

export default function SearchPage() {
  const searchParams = useSearchParams();

  return (
    <>
      <ContainerBand className={styles.containerBand}>
        <h1>Search Results</h1>
        <SearchForm initialValue={searchParams.get("searchTerm")} />
        <Link href={"/"}>Back to feed</Link>
      </ContainerBand>
      <PublicationsList publications={[{
        authorUsername: "andre",
        content: "teste",
        id: 1,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      },
      {
        authorUsername: "andre",
        content: "teste",
        id: 2,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      },
      {
        authorUsername: "andre",
        content: "teste",
        id: 3,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      },
      {
        authorUsername: "andre",
        content: "teste",
        id: 4,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      },
      {
        authorUsername: "andre",
        content: "teste",
        id: 5,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      },
      {
        authorUsername: "andre",
        content: "teste",
        id: 6,
        isRepost: false,
        publicationDate: "2024-01-01T00:00:00Z",
      }]} startRepostAction={() => { }} />
    </>
  )
}
