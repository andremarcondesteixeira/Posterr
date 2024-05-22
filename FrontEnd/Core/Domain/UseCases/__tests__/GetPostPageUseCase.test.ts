import { strict as assert } from "node:assert";
import { suite, test } from "node:test";
import { ApiEndpoint } from "../../../Services/ApiEndpointsService";
import * as HttpRequestService from "../../../Services/HttpRequestService";
import { GetPostsPageUseCase } from "../GetPostsPageUseCase";

// https://github.com/nodejs/node/issues/52015
fetch;

suite("GetPostsPageUseCase", () => {
  test("Given the first page is requested, then ask for 15 items", async (context) => {
    context.mock.method(global, "fetch", () => Promise.resolve({
      json: () => Promise.resolve([]),
      status: 200,
      ok: true,
    }));
    context.mock.method(HttpRequestService, "makeRequest");
    await GetPostsPageUseCase(1);
    assert.deepStrictEqual((HttpRequestService.makeRequest as any).mock.calls[0].arguments, [`${ApiEndpoint.posts}?page=1&pageSize=15`]);
    context.mock.reset();
  });

  test("Given the second page is requested, then ask for 20 items", async (context) => {
    context.mock.method(global, "fetch", () => Promise.resolve({
      json: () => Promise.resolve([]),
      status: 200,
      ok: true,
    }));
    context.mock.method(HttpRequestService, "makeRequest");
    await GetPostsPageUseCase(2);
    assert.deepStrictEqual((HttpRequestService.makeRequest as any).mock.calls[0].arguments, [`${ApiEndpoint.posts}?page=2&pageSize=20`]);
    context.mock.reset();
  });

  test("Given the requested page number is not an integer bigger than zero, then throw error", () => {
    assert.throws(() => GetPostsPageUseCase(0), "value was 0");
    assert.throws(() => GetPostsPageUseCase(-1), "value was -1");
    assert.throws(() => GetPostsPageUseCase(null as any), "value was null");
    assert.throws(() => GetPostsPageUseCase(undefined as any), "value was undefined");
    assert.throws(() => GetPostsPageUseCase("string" as any), "value was 'string'");
  });
});
