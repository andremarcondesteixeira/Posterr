import { strict as assert } from "node:assert";
import { suite, test } from "node:test";
import { Result } from "../Result";

suite("Result", () => {
  test("A Result object cannot be constructed directly using its constructor", () => {
    assert.throws(() => {
      new Result<string, Error>(true, "", null, null as any);
    });
  });

  test("Can create an Ok Result object", () => {
    const result = Result.Ok("test");
    assert.equal(result.isOk, true);
    assert.equal(result.isError, false);
    assert.equal(result.okValue, "test");
  });

  test("Can create an Error Result object", () => {
    const result = Result.Error(new Error("test"));
    assert.equal(result.isOk, false);
    assert.equal(result.isError, true);
    assert.equal(result.errorValue?.message, "test");
  });

  test("Given an Ok Result, when accessing errorValue, then throw error", () => {
    assert.throws(() => Result.Ok("test").errorValue);
  });

  test("Given an Error Result, when accessing okValue, then throw error", () => {
    assert.throws(() => Result.Error("test").okValue);
  });

  test("Given an Ok Result, when calling match, then the matcher's ok function should be called", () => {
    let okCalls = 0;
    let errorCalls = 0;
    Result.Ok("test").match({
      ok: () => okCalls++,
      error: () => errorCalls++,
    });
    assert.equal(okCalls, 1);
    assert.equal(errorCalls, 0);
  });

  test("Given an Error Result, when calling match, then the matcher's error function should be called", () => {
    let okCalls = 0;
    let errorCalls = 0;
    Result.Error("test").match({
      ok: () => okCalls++,
      error: () => errorCalls++,
    });
    assert.equal(okCalls, 0);
    assert.equal(errorCalls, 1);
  });

  test("Given an Ok Result, when calling okOrDefault, then the return the ok value", () => {
    const result = Result.Ok("test");
    assert.equal(result.okOrDefault("default"), "test");
  });

  test("Given an Error Result, when calling okOrDefault, then the return the default value", () => {
    const result = Result.Error("test");
    assert.equal(result.okOrDefault("default"), "default");
  });

  test("Given an Ok Result, when calling okOrThrow, then the return the ok value", () => {
    const result = Result.Ok("test");
    assert.equal(result.okOrThrow("default"), "test");
  });

  test("Given an Error Result, when calling okOrThrow, then throw error", () => {
    assert.throws(() => Result.Error("test").okOrThrow("default"));
  });
});
