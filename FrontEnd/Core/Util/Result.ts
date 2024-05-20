const PRIVATE_KEY = Symbol();

export type ResultMatcher<OK_TYPE, ERROR_TYPE> = {
  ok: (value: OK_TYPE) => void;
  error: (value: ERROR_TYPE) => void;
}

export class Result<OK_TYPE, ERROR_TYPE extends Error = Error> {
  #okValue: OK_TYPE | null;
  #errorValue: ERROR_TYPE | null;

  constructor(okValue: OK_TYPE | null, errorValue: ERROR_TYPE | null, key: typeof PRIVATE_KEY) {
    if (key !== PRIVATE_KEY) {
      throw new Error("You must use either Result.Ok or Result.Error to create a new Result object");
    }

    this.#okValue = okValue;
    this.#errorValue = errorValue;
  }

  static Ok<T>(okValue: T) {
    return new Result(okValue, null, PRIVATE_KEY);
  }

  static Error<T extends Error = Error>(errorValue: T) {
    return new Result(null, errorValue, PRIVATE_KEY);
  }

  get isOk() {
    return this.#okValue !== null;
  }

  get isError() {
    return this.#errorValue !== null;
  }

  match(matcher: ResultMatcher<OK_TYPE, ERROR_TYPE>): void {
    if (this.isOk) {
      return matcher.ok(this.#okValue as OK_TYPE);
    }

    return matcher.error(this.#errorValue as ERROR_TYPE);
  }
}
