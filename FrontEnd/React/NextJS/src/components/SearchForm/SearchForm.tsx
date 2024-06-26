"use client";

import { useEffect, useState } from "react";
import { SearchIcon } from "../Icons";
import styles from "./SearchForm.module.css";

type Props = {
  initialValue?: string | null;
}

export function SearchForm({ initialValue }: Props) {
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    setSearchTerm(initialValue || "");
  }, []);

  return (
    <form action="/search" method="GET" className={styles.searchForm}>
      <input
        type="text"
        placeholder="Search"
        name="searchTerm"
        value={searchTerm}
        onChange={event => setSearchTerm(event.target.value)}
        required
      />
      <button type="submit" className="primary">
        <SearchIcon color="white" />
        Search
      </button>
    </form>
  );
}
