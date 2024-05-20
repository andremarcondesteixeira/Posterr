import { afterAll, afterEach, describe, expect, test, vi } from "vitest";
import { usePostsPagination } from "../usePostsPagination";
import * as useHttpRequestHook from "../useHttpRequest";
import { Publication } from "@Core/Domain/Entities/Publication";

describe("usePostsPage", () => {
  const spy = vi.spyOn(useHttpRequestHook, "useHttpRequest");

  afterEach(() => {
    spy.mockReset();
  });

  afterAll(() => {
    spy.mockRestore();
  });

  test("Given request is in loading state, then return loading state", () => {
    spy.mockReturnValueOnce({
      data: undefined,
      error: undefined,
      isLoading: true,
    });
    const result = usePostsPagination(1);
    expect(result.isLoadingPosts).toBe(true);
    expect(result.posts).not.toBeDefined();
    expect(result.errorLoadingPosts).not.toBeDefined();
  });

  test("Given request was successful, then posts should be there", () => {
    spy.mockReturnValueOnce({
      data: [
        {
          postId: 1,
          authorUsername: "author 1",
          publicationDate: new Date(2024, 4, 20, 12, 30, 30),
          content: "post 1",
        },
        {
          postId: 2,
          authorUsername: "author 2",
          publicationDate: new Date(2024, 3, 10, 12),
          content: "post 2",
          repostId: 3,
          reposterUsername: "reposter",
          repostDate: new Date(2024, 3, 11, 12),
        }
      ] as Publication[],
      error: undefined,
      isLoading: false,
    });

    const result = usePostsPagination(1);
    expect(result.isLoadingPosts).toBe(false);
    expect(result.posts?.length).toBe(2);
    expect(result.posts?.[0].postId).toBe(1);
    expect(result.posts?.[0].authorUsername).toBe("author 1");
    expect(result.posts?.[0].publicationDate).toEqual(new Date(2024, 4, 20, 12, 30, 30));
    expect(result.posts?.[0].content).toBe("post 1");
    expect(result.posts?.[1].postId).toBe(2);
    expect(result.posts?.[1].authorUsername).toBe("author 2");
    expect(result.posts?.[1].publicationDate).toEqual(new Date(2024, 3, 10, 12));
    expect(result.posts?.[1].content).toBe("post 2");
    expect(result.posts?.[1].repostId).toBe(3);
    expect(result.posts?.[1].reposterUsername).toBe("reposter");
    expect(result.posts?.[1].repostDate).toEqual(new Date(2024, 3, 11, 12));
    expect(result.errorLoadingPosts).not.toBeDefined();
  });

  test("Given request was not successful, then make the error available", () => {
    spy.mockReturnValueOnce({
      data: undefined,
      error: new Error("error"),
      isLoading: false,
    });

    const result = usePostsPagination(1);

    expect(result.isLoadingPosts).toBe(false);
    expect(result.posts).not.toBeDefined();
    expect(result.errorLoadingPosts?.message).toBe("error");
  });

  test.each([
    0,
    -1,
    null,
    undefined,
    "foo",
    1.23,
  ])("Given page number is not an integer bigger than or equal to 1, then return error state", (pageNumber) => {
    expect(() => {
      usePostsPagination(pageNumber as unknown as number);
    }).toThrowError(new Error("Page number must be a positive integer"));
  });
});
