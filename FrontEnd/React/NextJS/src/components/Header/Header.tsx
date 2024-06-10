"use client"

import { ApiEndpoint, AuthorAPIResource } from "@Core/Services/ApiEndpointsService";
import { useContext, useEffect, useState } from "react";
import { DefaultAuthorUsernameContext } from "../../app/DefaultAuthorUsernameContext";
import styles from "./Header.module.css";

export function Header() {
  const [users, setUsers] = useState<AuthorAPIResource[]>([]);
  const { defaultAuthorUsername, setDefaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);

  useEffect(() => {
    ApiEndpoint.users.GET().then(response => {
      response.match({
        ok: users => setUsers(users._embedded.users),
        error: error => alert(error.message),
      });
    });
  }, []);

  return (
    <>
      <header className={styles.header}>
        <p className={styles.socialLinks}>
          Made by André Teixeira
          <a href="https://www.linkedin.com/in/andremarcondesteixeira/?locale=en_US" target="_blank" title="LinkedIn">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 12 12">
              <path fill="#0a66c2" d="M10.225 10.225H8.447V7.44c0-.664-.012-1.519-.925-1.519-.926 0-1.068.724-1.068 1.47v2.834H4.676V4.498h1.707v.783h.024c.348-.594.996-.95 1.684-.925 1.802 0 2.135 1.185 2.135 2.728l-.001 3.14ZM2.67 3.715a1.037 1.037 0 0 1-1.032-1.031c0-.566.466-1.032 1.032-1.032.566 0 1.031.466 1.032 1.032 0 .566-.466 1.032-1.032 1.032zm.889 6.51h-1.78V4.498h1.78zM11.11 0H.885A.88.88 0 0 0 0 .866v10.268A.88.88 0 0 0 .885 12h10.226a.882.882 0 0 0 .889-.866V.865a.88.88 0 0 0-.889-.864z" />
            </svg>
          </a>
          <a href="https://github.com/andremarcondesteixeira" target="_blank" title="GitHub">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
              <path fill="#000" fill-rule="evenodd" d="M10 0c5.523 0 10 4.59 10 10.253 0 4.529-2.862 8.371-6.833 9.728-.507.101-.687-.219-.687-.492 0-.338.012-1.442.012-2.814 0-.956-.32-1.58-.679-1.898 2.227-.254 4.567-1.121 4.567-5.059 0-1.12-.388-2.034-1.03-2.752.104-.259.447-1.302-.098-2.714 0 0-.838-.275-2.747 1.051A9.396 9.396 0 0 0 10 4.958a9.375 9.375 0 0 0-2.503.345C5.586 3.977 4.746 4.252 4.746 4.252c-.543 1.412-.2 2.455-.097 2.714-.639.718-1.03 1.632-1.03 2.752 0 3.928 2.335 4.808 4.556 5.067-.286.256-.545.708-.635 1.371-.57.262-2.018.715-2.91-.852 0 0-.529-.985-1.533-1.057 0 0-.975-.013-.068.623 0 0 .655.315 1.11 1.5 0 0 .587 1.83 3.369 1.21.005.857.014 1.665.014 1.909 0 .271-.184.588-.683.493C2.865 18.627 0 14.783 0 10.253 0 4.59 4.478 0 10 0" />
            </svg>
          </a>
        </p>
        <h1>POSTERR</h1>
        <label className={styles.defaultAuthorUsernameSelector}>
          Default Author:
          <select onChange={event => setDefaultAuthorUsername(event.target.value)} defaultValue={defaultAuthorUsername}>
            <option value="">--- select ---</option>
            {users.map(user => (
              <option value={user.username} key={user.username}>{user.username}</option>
            ))}
          </select>
        </label>
      </header>
    </>
  );
}
