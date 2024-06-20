import { PublicationEntity } from "@Core/Domain/Entities/types";
import * as HttpRequestService from "@Core/Services/HttpRequestService";
import { cleanup, render, screen } from '@testing-library/react';
import { afterAll, afterEach, describe, expect, test, vi } from 'vitest';
import Page from '../page';

describe("Home", () => {
  const HttpRequestServiceSpy = vi.spyOn(HttpRequestService, "makeRequest");

  afterEach(() => {
    HttpRequestServiceSpy.mockReset();
    cleanup();
  });

  afterAll(() => {
    HttpRequestServiceSpy.mockRestore();
  });

  test('It should render', () => {
    HttpRequestServiceSpy.mockReturnValueOnce({
      isLoadingPosts: true,
      posts: undefined,
    });
    render(<Page />);
    expect(screen.getByRole('heading', { level: 1, name: 'Posterr' })).toBeDefined();
  });

  test("It should display a loading state", () => {
    HttpRequestServiceSpy.mockReturnValueOnce({
      errorLoadingPosts: undefined,
      isLoadingPosts: true,
      posts: undefined,
    });
    render(<Page />);
    expect(screen.getByText('Loading posts...')).toBeInTheDocument();
  });

  test("It should display loaded posts", () => {
    HttpRequestServiceSpy.mockReturnValueOnce({
      errorLoadingPosts: undefined,
      isLoadingPosts: false,
      posts: [
        {
          postId: 1,
          authorUsername: "author",
          publicationDate: new Date(2024, 4, 20, 15, 27),
          content: "the content",
        }
      ] as PublicationEntity[],
    });
    render(<Page />);
    expect(screen.queryByText('Loading posts...')).toBeNull();
    expect(screen.getByText("ID: 1")).toBeInTheDocument();
    expect(screen.getByText("Author: author")).toBeInTheDocument();
    expect(screen.getByText(`Published on ${new Date(2024, 4, 20, 15, 27).toLocaleDateString()}`)).toBeInTheDocument();
    expect(screen.getByText("the content")).toBeInTheDocument();
  });
});
