import { SortingOrder } from "./SortingOrder";
import styles from "./SortingOrderSelector.module.css";

type Props = {
  onChange: (sortingOrder: SortingOrder) => void;
}

export function SortingOrderSelector({ onChange }: Props) {
  return (
    <div className={styles.sortingOrderSelector}>
      Sort by:
      <label>
        <input type="radio" name="sortingOrder" value={SortingOrder.NEWEST} onClick={() => onChange(SortingOrder.NEWEST)} />
        Newest
      </label>
      <label>
        <input type="radio" name="sortingOrder" value={SortingOrder.TRENDING} onClick={() => onChange(SortingOrder.TRENDING)} />
        Trending
      </label>
    </div>
  );
}
