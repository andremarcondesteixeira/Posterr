import type { PublicationEntity } from "@Core/Domain/Entities/types";
import styles from "./Publication.module.css";

type Props = {
  publication: PublicationEntity;
  onClickRepost: () => void;
}

export function Publication({ publication, onClickRepost }: Props) {
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
      {!publication.isRepost && (
        <>
          <hr />
          <section className={styles.publicationActions}>
            <button className="transparent" type="button" onClick={onClickRepost}>
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                <path d="M5 4a2 2 0 0 0-2 2v6H0l4 4 4-4H5V6h7l2-2H5zm10 4h-3l4-4 4 4h-3v6a2 2 0 0 1-2 2H6l2-2h7V8z" />
              </svg>
              Repost
            </button>
          </section>
        </>
      )}
    </article>
  );
}
