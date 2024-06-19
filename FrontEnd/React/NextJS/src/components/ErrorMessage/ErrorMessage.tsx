import styles from "./ErrorMessage.module.css";

type Props = {
  messages: string[];
};

export function ErrorMessage({ messages }: Props) {
  return (
    <article className={styles.errorMessage}>
      {messages.map(message => (
        <p>{message}</p>
      ))}
    </article>
  );
}
