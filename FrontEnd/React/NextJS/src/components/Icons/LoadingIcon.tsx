export function LoadingIcon() {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      xmlnsXlink="http://www.w3.org/1999/xlink"
      viewBox="0 0 100 100"
      preserveAspectRatio="xMidYMid"
      style={{
        background: "transparent",
        display: "block",
        height: "4rem",
        shapeRendering: "auto",
      }}
    >
      <circle strokeWidth="2" stroke="#e90c59" fill="none" r="0" cy="50" cx="50">
        <animate begin="0s" calcMode="spline" keySplines="0 0.2 0.8 1" keyTimes="0;1" values="0;40" dur="1s" repeatCount="indefinite" attributeName="r"></animate>
        <animate begin="0s" calcMode="spline" keySplines="0.2 0 0.8 1" keyTimes="0;1" values="1;0" dur="1s" repeatCount="indefinite" attributeName="opacity"></animate>
      </circle>
      <circle strokeWidth="2" stroke="#46dff0" fill="none" r="0" cy="50" cx="50">
        <animate begin="-0.5s" calcMode="spline" keySplines="0 0.2 0.8 1" keyTimes="0;1" values="0;40" dur="1s" repeatCount="indefinite" attributeName="r"></animate>
        <animate begin="-0.5s" calcMode="spline" keySplines="0.2 0 0.8 1" keyTimes="0;1" values="1;0" dur="1s" repeatCount="indefinite" attributeName="opacity"></animate>
      </circle>
    </svg>
  );
}
