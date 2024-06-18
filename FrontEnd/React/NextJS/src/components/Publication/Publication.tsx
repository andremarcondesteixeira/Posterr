import type { PublicationEntity } from "@Core/Domain/Entities/types";
import { ReactNode } from "react";
import styles from "./Publication.module.css";

type Props = {
  publication: PublicationEntity;
  children?: ReactNode;
}

export function Publication({ publication, children }: Props) {
  return (
    <article className={`${styles.publication} ${publication.isRepost && styles.isRepost}`}>
      <span className={styles.publicationAuthor}>{publication.authorUsername}</span>
      <span className={styles.publicationPublicationDate}>{new Date(publication.publicationDate).toLocaleString()}</span>
      <span className={styles.publicationContent}>{publication.content}</span>
      {publication.isRepost && (
        <div className={styles.publication}>
          <span className={styles.publicationAuthor}>{publication.originalPostAuthorUsername}</span>
          <span className={styles.publicationPublicationDate}>{new Date(publication.originalPostPublicationDate!).toLocaleString()}</span>
          <span className={styles.publicationContent}>{publication.originalPostContent}</span>
        </div>
      )}
      {children && (
        <>
          <hr />
          <section className={styles.publicationActions}>
            {children}
          </section>
        </>
      )}
    </article>
  );
}
