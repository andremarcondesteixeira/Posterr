"use client"

import { ReactNode, createContext, useState } from "react";

export const DefaultAuthorUsernameContext = createContext<{
  defaultAuthorUsername: string;
  setDefaultAuthorUsername: (u: string) => void;
}>({
  defaultAuthorUsername: "",
  setDefaultAuthorUsername: () => {},
});

export function DefaultAuthorUsernameContextProvider({ children }: {
  children: ReactNode;
}) {
  const [defaultAuthorUsername, setDefaultAuthorUsername] = useState<string>("");

  return (
    <DefaultAuthorUsernameContext.Provider value={{ defaultAuthorUsername, setDefaultAuthorUsername }}>
      {children}
    </DefaultAuthorUsernameContext.Provider>
  );
}
