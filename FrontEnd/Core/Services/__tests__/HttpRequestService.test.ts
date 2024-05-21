import { strict as assert } from "node:assert";
import { suite, test } from "node:test";
import { makeRequest } from "../HttpRequestService";

// https://github.com/nodejs/node/issues/52015
fetch;

suite("HttpRequestService", () => {
  suite("makeRequest", () => {
    test("It should return the response", async (context) => {
      context.mock.method(global, "fetch", async () => {
        return {
          json: () => Promise.resolve({ foo: "bar" }),
          status: 200,
        };
      });

      const response = await makeRequest<{ foo: "bar" }>("/");

      assert.deepEqual(response.okOrThrow(), { foo: "bar" });

      context.mock.reset();
    });
  });
});
