type Props = {
  color?: string;
}

export function UploadIcon({ color }: Props) {
  return (
    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 19.5 18.5">
      <path
        stroke={color || "#000"}
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeWidth="1.5"
        d="M4.75 6.75v-1a5 5 0 0 1 10 0v1a4 4 0 0 1 4 4c0 1.48-.804 2.808-2 3.5m-12-7.5a4 4 0 0 0-4 4c0 1.48.804 2.808 2 3.5m2-7.5a4 4 0 0 1 1.24.196M9.75 8.75v9m0-9 3 3m-3-3-3 3"
      />
    </svg>
  );
}
