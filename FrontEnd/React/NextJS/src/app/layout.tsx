import type { Metadata } from "next";
import { Header } from "../components/Header";
import { DefaultAuthorUsernameContextProvider } from "./DefaultAuthorUsernameContext";
import "./globals.css";

export const metadata: Metadata = {
  title: "Posterr",
  description: "A social network",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head>
        <link rel="preconnect" href="https://fonts.googleapis.com" />
        <link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin="" />
        <link href="https://fonts.googleapis.com/css2?family=Bebas+Neue&family=Titillium+Web:ital,wght@0,200;0,300;0,400;0,600;0,700;0,900;1,200;1,300;1,400;1,600;1,700&family=Tulpen+One&display=swap" rel="stylesheet" />
      </head>
      <body>
        <DefaultAuthorUsernameContextProvider>
          <Header />
          <main>
            {children}
          </main>
        </DefaultAuthorUsernameContextProvider>
      </body>
    </html>
  );
}
