import { CloseIcon } from "../Icons";
import styles from "./ErrorMessage.module.css";

type Props = {
  messages: string[];
  onClickClose: () => void;
};

export function ErrorMessage({ messages, onClickClose }: Props) {
  return (
    <article className={styles.errorMessage}>
      <button className={`transparent ${styles.closeButton}`} onClick={onClickClose} title="close">
        <CloseIcon />
      </button>
      {messages.map(message => (
        <p key={message}>{message}</p>
      ))}
    </article>
  );
}
