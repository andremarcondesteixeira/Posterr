import { PublicationEntity } from "@Core/Domain/Entities/types";
import { Publication } from "../Publication";
import styles from "./PublicationsList.module.css";

type Props = {
  publications: PublicationEntity[];
  startRepostAction: (originalPost: PublicationEntity) => void;
}

export function PublicationsList({ publications, startRepostAction }: Props) {
  return (
    <ul className={styles.publicationsList}>
      {publications && publications.map(publication => (
        <li key={publication.id}>
          <Publication publication={publication}>
            {!publication.isRepost && (
              <button className="transparent" type="button" onClick={() => startRepostAction(publication)}>
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
  );
}
