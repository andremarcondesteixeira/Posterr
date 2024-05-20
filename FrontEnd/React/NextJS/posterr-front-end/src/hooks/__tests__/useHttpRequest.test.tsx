import { Result } from "@Core/Util/Result";
import { cleanup, render, screen } from "@testing-library/react";
import { afterEach, describe, expect, test } from "vitest";
import { useHttpRequest } from "../useHttpRequest";

describe("useHttpRequest", () => {
  afterEach(() => {
    cleanup();
  });

  test("It should start with loading state", () => {
    render(<Page id="loading" />);
    expect(screen.getByText("is loading")).toBeInTheDocument();
    expect(screen.queryByText("is not loading")).toBeNull();
    expect(screen.queryByText("response")).toBeNull();
    expect(screen.queryByText("error")).toBeNull();
  });

  test("It should quit loading state after response is received", async () => {
    render(<Page id="loaded" />);
    await new Promise(resolve => setTimeout(resolve, 120));
    expect(screen.queryByText("is loading")).toBeNull();
    expect(screen.getByText("is not loading")).toBeInTheDocument();
  });

  test("It should render response when it is received", async () => {
    render(<Page id="response" />);
    await new Promise(resolve => setTimeout(resolve, 120));
    expect(screen.getByText("response")).toBeInTheDocument();
    expect(screen.queryByText("error")).toBeNull();
  });

  test("It should render error when it is received", async () => {
    render(<Page id="error" shouldError={true} />);
    await new Promise(resolve => setTimeout(resolve, 120));
    expect(screen.queryByText("response")).toBeNull();
    expect(screen.getByText("error")).toBeInTheDocument();
  });
});

function Page({ id, shouldError = false }: { id: string, shouldError?: boolean }) {
  const { isLoading, data, error } = useHttpRequest<string>(id, () => new Promise<Result<string>>((resolve, reject) => {
    setTimeout(() => {
      if (shouldError) {
        reject(new Error("error"));
      } else {
        resolve(Result.Ok<string>("response"));
      }
    }, 100);
  }));

  return (
    <main>
      {isLoading && <span>is loading</span>}
      {!isLoading && <span>is not loading</span>}
      {data && <div>{data}</div>}
      {error && <div>{error.message}</div>}
    </main>
  );
}
