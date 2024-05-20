import * as usePostsPaginationHook from "@/hooks/usePostsPagination";
import { cleanup, render, screen } from '@testing-library/react';
import { afterAll, afterEach, describe, expect, test, vi } from 'vitest';
import Page from '../page';

describe("Home", () => {
  const usePostsPaginationSpy = vi.spyOn(usePostsPaginationHook, "usePostsPagination");

  afterEach(() => {
    usePostsPaginationSpy.mockReset();
    cleanup();
  });

  afterAll(() => {
    usePostsPaginationSpy.mockRestore();
  });

  test('It should render', () => {
    usePostsPaginationSpy.mockReturnValueOnce({
      errorLoadingPosts: undefined,
      isLoadingPosts: true,
      posts: undefined,
    });
    render(<Page />);
    expect(screen.getByRole('heading', { level: 1, name: 'Posterr' })).toBeDefined();
  });

  test("It should display a loading state", () => {
    usePostsPaginationSpy.mockReturnValueOnce({
      errorLoadingPosts: undefined,
      isLoadingPosts: true,
      posts: undefined,
    });
    render(<Page />);
    expect(screen.getByText('Loading posts...')).toBeInTheDocument();
  });
});
