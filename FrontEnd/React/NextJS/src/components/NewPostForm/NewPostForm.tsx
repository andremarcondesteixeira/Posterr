import { DefaultAuthorUsernameContext } from "@/app/DefaultAuthorUsernameContext";
import { ApiEndpoint, PublicationAPIResource } from "@Core/Services/ApiEndpointsService";
import { Dispatch, FormEvent, SetStateAction, useContext, useState } from "react";
import styles from "./NewPostForm.module.css";

type Props = {
  setPublications: Dispatch<SetStateAction<PublicationAPIResource[]>>;
};

export function NewPostForm({ setPublications }: Props) {
  const [newPostContent, setNewPostContent] = useState("");
  const { defaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);

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
    <form className={styles.newPostForm} onSubmit={tryCreateNewPost}>
      <textarea placeholder="What are your thoughts?" value={newPostContent} onChange={(event) => setNewPostContent(event.target.value)} />
      <button className="primary">Publish</button>
    </form>
  );
}
