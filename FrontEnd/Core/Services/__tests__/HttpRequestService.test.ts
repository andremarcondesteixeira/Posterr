import { strict as assert } from "node:assert";
import { suite, test } from "node:test";
import { makeRequest } from "../HttpRequestService";

// https://github.com/nodejs/node/issues/52015
fetch;

suite("HttpRequestService", () => {
  suite("makeRequest", () => {
    test("Given an 200 OK HTTP status code when doing a request, then return an Ok Result object", async (context) => {
      context.mock.method(global, "fetch", () => Promise.resolve({
        json: () => Promise.resolve({ foo: "bar" }),
        status: 200,
        ok: true,
      }));
      const response = await makeRequest<{ foo: string }>("/");
      assert.deepEqual(response.okValue, { foo: "bar" });
      context.mock.reset();
    });

    test("Given the HTTP status code is not in the ok range (200 - 299) when doing a request, then return an Error Result object", async (context) => {
      context.mock.method(global, "fetch", () => Promise.resolve({ status: 404, ok: false }));
      const response = await makeRequest<never>("/");
      assert.equal(response.errorValue?.message, "Response HTTP status code was 404 when fetching /");
      assert.deepEqual(response.errorValue?.cause, { status: 404, ok: false });
      context.mock.reset();
    });

    test("Given an Error object is thrown when doing an HTTP request, then return an Error Result object", async (context) => {
      context.mock.method(global, "fetch", () => Promise.reject(new Error("something bad happened")));
      const response = await makeRequest<{ foo: "bar" }>("/");
      assert.equal(response.errorValue?.message, "something bad happened");
      context.mock.reset();
    });
  });
});
