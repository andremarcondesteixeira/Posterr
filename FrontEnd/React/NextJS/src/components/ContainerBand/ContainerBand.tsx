import { ReactNode } from "react";
import styles from "./ContainerBand.module.css";

type Props = {
  children: ReactNode;
  className?: string;
}

export function ContainerBand({ children, className }: Props) {
  return (
    <div className={`${styles.containerBand} ${className}`}>{children}</div>
  );
}
