import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { Publication } from "@Core/Domain/Entities/types";
import { CreateNewPostUseCase, CreateNewRepostUseCase } from "@Core/Domain/UseCases";
import config from "@Core/Domain/config.json";
import { PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { PosterrAPIErrorResponse } from "@Core/Services/PosterrAPIErrorResponse";
import { Dispatch, FormEvent, SetStateAction, useContext, useLayoutEffect, useRef, useState } from "react";
import { ErrorMessage } from "../ErrorMessage";
import { CancelIcon, LoadingIcon } from "../Icons";
import { PublicationContainer } from "../PublicationContainer";
import styles from "./NewPublicationForm.module.css";

type Props = {
  cancelRepostAction: () => void;
  originalPost: Publication | null;
  setOriginalPost: Dispatch<SetStateAction<Publication | null>>;
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
        CreateNewRepostUseCase(defaultAuthorUsername, newPostContent, originalPost.id) :
        CreateNewPostUseCase(defaultAuthorUsername, newPostContent)
    );

    setIsLoading(false);

    response.match({
      error(error) {
        if (error instanceof PosterrAPIErrorResponse) {
          setErrorMessages([error.cause.title, error.cause.detail]);
        } else if(typeof error === "string") {
          setErrorMessages([error]);
        }
      },
      ok(publication) {
        setPublications(prev => [publication, ...prev]);
        setOriginalPost(null);
        setNewPostContent("")
      },
    });
  }

  return (
    <form className={styles.newPublicationForm} onSubmit={createNewPublication} id="newPublicationForm">
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
        <output
          name="contentCharacterCount"
          htmlFor="newPublicationContent"
          form="newPublicationForm"
          className={styles.contentCharacterCount}
          aria-invalid={newPostContent.length > config.maxPublicationContentLength}
        >
          {newPostContent.length} / {config.maxPublicationContentLength}
        </output>
        {originalPost && (
          <PublicationContainer publication={originalPost}>
            {!isLoading && (
              <button type="button" className="transparent" onClick={cancelRepostAction} disabled={isLoading}>
                <CancelIcon />
                Cancel repost
              </button>
            )}
          </PublicationContainer>
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
