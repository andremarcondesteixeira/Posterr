import { SortingOrder } from "@CoreDomain/SortingOrder";
import styles from "./SortingOrderSelector.module.css";

type Props = {
  onChange: (sortingOrder: SortingOrder) => void;
  value: SortingOrder;
};

export function SortingOrderSelector({ onChange, value }: Props) {
  return (
    <div className={styles.sortingOrderSelector}>
      Sort by:
      <label>
        <input
          type="radio"
          name="sortingOrder"
          value={SortingOrder.NEWEST}
          onClick={() => onChange(SortingOrder.NEWEST)}
          checked={value === SortingOrder.NEWEST}
        />
        Newest
      </label>
      <label>
        <input
          type="radio"
          name="sortingOrder"
          value={SortingOrder.TRENDING}
          onClick={() => onChange(SortingOrder.TRENDING)}
          checked={value === SortingOrder.TRENDING}
        />
        Trending
      </label>
    </div>
  );
}
