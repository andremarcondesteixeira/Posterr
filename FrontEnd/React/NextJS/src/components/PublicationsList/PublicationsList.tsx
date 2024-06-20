import { Publication } from "@Core/Domain/Entities/types";
import { RepostIcon } from "../Icons";
import { PublicationContainer } from "../PublicationContainer";
import styles from "./PublicationsList.module.css";

type Props = {
  publications: Publication[];
  startRepostAction: (originalPost: Publication) => void;
}

export function PublicationsList({ publications, startRepostAction }: Props) {
  return (
    <ul className={styles.publicationsList}>
      {publications && publications.map(publication => (
        <li key={publication.id}>
          <PublicationContainer publication={publication}>
            {!publication.isRepost && (
              <button className="transparent" type="button" onClick={() => startRepostAction(publication)}>
                <RepostIcon />
                Repost
              </button>
            )}
          </PublicationContainer>
        </li>
      ))}
    </ul>
  );
}
