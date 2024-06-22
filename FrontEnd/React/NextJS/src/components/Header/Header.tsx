"use client"

import { ListUsersUseCase } from "@Core/Domain/UseCases";
import { AuthorAPIResource } from "@Core/Services/ApiEndpointsService";
import { PosterrAPIErrorResponse } from "@Core/Services/PosterrAPIErrorResponse";
import { useContext, useEffect, useState } from "react";
import { DefaultAuthorUsernameContext } from "../../app/DefaultAuthorUsernameContext";
import { GitHubIcon, LinkedInIcon } from "../Icons";
import styles from "./Header.module.css";

export function Header() {
  const [users, setUsers] = useState<AuthorAPIResource[]>([]);
  const { defaultAuthorUsername, setDefaultAuthorUsername } = useContext(DefaultAuthorUsernameContext);

  useEffect(() => {
    ListUsersUseCase().then(result => {
      result.match({
        ok: users => setUsers(users._embedded.users),
        error(error) {
          if (error instanceof PosterrAPIErrorResponse) {
            alert(error.message);
          }
        }
      });
    });
  }, []);

  return (
    <>
      <header className={styles.header}>
        <p className={styles.socialLinks}>
          Made by André Teixeira
          <a href="https://www.linkedin.com/in/andremarcondesteixeira/?locale=en_US" target="_blank" title="LinkedIn">
            <LinkedInIcon />
          </a>
          <a href="https://github.com/andremarcondesteixeira" target="_blank" title="GitHub">
            <GitHubIcon />
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
