import { MouseEventHandler } from "react";
import styles from "./SortingOrderSelector.module.css";

type Props = {
  onChange: (sortingOrder: string) => void;
}

export function SortingOrderSelector({ onChange }: Props) {
  const changeSortingOrder: MouseEventHandler<HTMLInputElement> = (event) => {
    onChange((event.target as HTMLInputElement).value);
  }

  return (
    <div className={styles.sortingOrderSelector}>
      Sort by:
      <label>
        <input type="radio" name="sortingOrder" value="newest" onClick={changeSortingOrder} />
        Newest
      </label>
      <label>
        <input type="radio" name="sortingOrder" value="trending" onClick={changeSortingOrder} />
        Trending
      </label>
    </div>
  );
}
