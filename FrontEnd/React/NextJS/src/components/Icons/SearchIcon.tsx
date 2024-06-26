type Props = {
  color?: string;
}

export function SearchIcon({ color }: Props) {
  return (
    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
      <path
        stroke={color || "#000"}
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeWidth="1.5"
        d="M9 4a5 5 0 0 1 5 5m.659 5.655L19 19M17 9A8 8 0 1 1 1 9a8 8 0 0 1 16 0z"
      />
    </svg>
  );
}
