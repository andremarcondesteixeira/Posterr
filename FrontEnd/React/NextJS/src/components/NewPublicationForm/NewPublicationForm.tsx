import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { PublicationEntity } from "@Core/Domain/Entities/types";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { Dispatch, FormEvent, SetStateAction, useContext, useLayoutEffect, useRef, useState } from "react";
import styles from "./NewPublicationForm.module.css";

type Props = {
  setPublications: Dispatch<SetStateAction<PublicationAPIResource[]>>;
  originalPost: PublicationEntity | null;
};

export function NewPublicationForm({ setPublications, originalPost }: Props) {
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
          ref={textareaRef}
        />
        {originalPost && (
          <article className={styles.originalPost}>
            <span className={styles.originalPostAuthor}>{originalPost.authorUsername}</span>
            <span className={styles.originalPostPublicationDate}>{new Date(originalPost.publicationDate).toLocaleString()}</span>
            <span className={styles.originalPostContent}>{originalPost.content}</span>
          </article>
        )}
      </section>
      <button className="primary">Publish</button>
    </form>
  );
}
