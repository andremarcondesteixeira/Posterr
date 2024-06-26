import { ForwardedRef, forwardRef } from "react";
import { LoadingIcon } from "../Icons";
import styles from "./Loading.module.css";

export const Loading = forwardRef((_, ref: ForwardedRef<HTMLParagraphElement>) => {
  return (
    <p className={styles.loading} ref={ref}>
      <LoadingIcon />
      Loading
    </p>
  );
});
