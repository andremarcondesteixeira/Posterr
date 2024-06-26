import { ReactNode } from "react";
import styles from "./ContainerBand.module.css";

type Props = {
  children: ReactNode;
}

export function ContainerBand({ children }: Props) {
  return (
    <div className={styles.containerBand}>{children}</div>
  );
}
