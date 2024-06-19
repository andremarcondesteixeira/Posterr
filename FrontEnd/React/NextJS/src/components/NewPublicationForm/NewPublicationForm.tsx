import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { PublicationEntity } from "@Core/Domain/Entities/types";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { Dispatch, FormEvent, SetStateAction, useContext, useLayoutEffect, useRef, useState } from "react";
import { ErrorMessage } from "../ErrorMessage";
import { LoadingIcon } from "../Icons";
import { Publication } from "../Publication";
import styles from "./NewPublicationForm.module.css";

type Props = {
  cancelRepostAction: () => void;
  originalPost: PublicationEntity | null;
  setOriginalPost: Dispatch<SetStateAction<PublicationEntity | null>>;
  setPublications: Dispatch<SetStateAction<PublicationAPIResource[]>>;
};

export function NewPublicationForm({ cancelRepostAction, originalPost, setOriginalPost, setPublications }: Props) {
  const textareaRef = useRef<HTMLTextAreaElement | null>(null);
  const { defaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);
  const [newPostContent, setNewPostContent] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessages, setErrorMessages] = useState<string[]>([]);

  useLayoutEffect(() => {
    if (!textareaRef.current) {
      return;
    }

    textareaRef.current.style.height = 'auto';
    textareaRef.current.style.height = (textareaRef.current.scrollHeight) + "px";
  }, [newPostContent]);

  async function createNewPublication(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!defaultAuthorUsername) {
      alert("Please select the author for the new post");
      return;
    }

    setIsLoading(true);
    setErrorMessages([]);

    const response = await (
      originalPost ?
        ApiEndpoint.publications.PATCH(defaultAuthorUsername, newPostContent, originalPost.id) :
        ApiEndpoint.publications.POST(defaultAuthorUsername, newPostContent)
    );

    setIsLoading(false);

    response.match({
      error: error => setErrorMessages([error.cause.title, error.cause.detail]),
      ok: publication => {
        setPublications(prev => [publication, ...prev]);
        setOriginalPost(null);
        setNewPostContent("")
      },
    });
  }

  return (
    <form className={styles.newPublicationForm} onSubmit={createNewPublication}>
      {errorMessages.length > 0 && (
        <ErrorMessage messages={errorMessages} onClickClose={() => setErrorMessages([])} />
      )}
      <section className={styles.publicationContent}>
        <textarea
          id="newPublicationContent"
          placeholder="Share your thoughts"
          value={newPostContent}
          onChange={(event) => setNewPostContent(event.target.value)}
          rows={1}
          style={{ height: "auto" }}
          ref={textareaRef}
          disabled={isLoading}
        />
        {originalPost && (
          <Publication publication={originalPost}>
            {!isLoading && (
              <button type="button" className="transparent" onClick={cancelRepostAction} disabled={isLoading}>
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                  <path d="M10 1a9 9 0 1 0 9 9 9 9 0 0 0-9-9Zm0 16.4a7.4 7.4 0 1 1 7.4-7.4 7.41 7.41 0 0 1-7.4 7.4Zm3.29-12.11L10 8.59l-3.29-3.3-1.42 1.42L8.59 10l-3.3 3.29 1.42 1.42 3.29-3.3 3.29 3.3 1.42-1.42-3.3-3.29 3.3-3.29Z" />
                </svg>
                Cancel repost
              </button>
            )}
          </Publication>
        )}
      </section>
      {!isLoading && (
        <button className="primary">
          Publish
        </button>
      )}
      {isLoading && <LoadingIcon />}
    </form>
  );
}
