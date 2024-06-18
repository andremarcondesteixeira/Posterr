import { PublicationEntity } from "@Core/Domain/Entities/types";
import { RepostIcon } from "../Icons";
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
                <RepostIcon />
                Repost
              </button>
            )}
          </Publication>
        </li>
      ))}
    </ul>
  );
}
