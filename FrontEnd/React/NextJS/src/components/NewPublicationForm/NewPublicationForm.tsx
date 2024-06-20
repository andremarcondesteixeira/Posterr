import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { PublicationEntity } from "@Core/Domain/Entities/types";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { Dispatch, FormEvent, SetStateAction, useContext, useLayoutEffect, useRef, useState } from "react";
import { ErrorMessage } from "../ErrorMessage";
import { CancelIcon, LoadingIcon } from "../Icons";
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
                <CancelIcon />
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
      {isLoading && (
        <p className={styles.loading}>
          <LoadingIcon />
          Please wait
        </p>
      )}
    </form>
  );
}
