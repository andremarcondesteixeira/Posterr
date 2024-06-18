import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { PublicationEntity } from "@Core/Domain/Entities/types";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { Dispatch, FormEvent, SetStateAction, useContext, useLayoutEffect, useRef, useState } from "react";
import { Publication } from "../Publication";
import styles from "./NewPublicationForm.module.css";

type Props = {
  cancelRepostAction: () => void;
  originalPost: PublicationEntity | null;
  setPublications: Dispatch<SetStateAction<PublicationAPIResource[]>>;
};

export function NewPublicationForm({ cancelRepostAction, originalPost, setPublications }: Props) {
  const textareaRef = useRef<HTMLTextAreaElement | null>(null);
  const [newPostContent, setNewPostContent] = useState("");
  const { defaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);

  useLayoutEffect(() => {
    if (!textareaRef.current) {
      return;
    }

    textareaRef.current.style.height = 'auto';
    textareaRef.current.style.height = (textareaRef.current.scrollHeight) + "px";
  }, [newPostContent]);

  async function tryCreateNewPost(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!defaultAuthorUsername) {
      alert("Please select the author for the new post");
      return;
    }

    const response = await ApiEndpoint.publications.POST(defaultAuthorUsername, newPostContent);
    response.match({
      error: error => alert(`${error.cause.title}\n${error.cause.detail}`),
      ok: publication => setPublications(prev => [publication, ...prev]),
    });
  }

  return (
    <form className={styles.newPublicationForm} onSubmit={tryCreateNewPost}>
      <section className={styles.publicationContent}>
        <textarea
          placeholder="Share your thoughts"
          value={newPostContent}
          onChange={(event) => setNewPostContent(event.target.value)}
          rows={1}
          style={{ height: "auto" }}
          ref={textareaRef}
        />
        {originalPost && (
          <Publication publication={originalPost}>
            <button type="button" className="transparent" onClick={cancelRepostAction}>
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                <path d="M10 1a9 9 0 1 0 9 9 9 9 0 0 0-9-9Zm0 16.4a7.4 7.4 0 1 1 7.4-7.4 7.41 7.41 0 0 1-7.4 7.4Zm3.29-12.11L10 8.59l-3.29-3.3-1.42 1.42L8.59 10l-3.3 3.29 1.42 1.42 3.29-3.3 3.29 3.3 1.42-1.42-3.3-3.29 3.3-3.29Z" />
              </svg>
              Cancel repost
            </button>
          </Publication>
        )}
      </section>
      <button className="primary">Publish</button>
    </form>
  );
}
