const PRIVATE_KEY = Symbol();

export type ResultMatcher<OK, ERROR> = {
  ok: (value: OK) => void;
  error: (value: ERROR) => void;
}

export class Result<OK, ERROR> {
  #isOk: boolean;
  #okValue: OK | null;
  #errorValue: ERROR | null;

  constructor(isOk: boolean, okValue: OK | null, errorValue: ERROR | null, key: typeof PRIVATE_KEY) {
    if (key !== PRIVATE_KEY) {
      throw new Error("You must use either Result.Ok or Result.Error to create a new Result object");
    }

    this.#isOk = isOk;
    this.#okValue = okValue;
    this.#errorValue = errorValue;
  }

  static Ok<OK, ERROR = never>(okValue: OK) {
    return new Result<OK, ERROR>(true, okValue, null, PRIVATE_KEY);
  }

  static Error<OK, ERROR>(errorValue: ERROR) {
    return new Result<OK, ERROR>(false, null, errorValue, PRIVATE_KEY);
  }

  get okValue() {
    if (this.isError) {
      throw new Error("Tried to access okValue of Result object when result is error", {
        cause: this,
      });
    }
    return this.#okValue;
  }

  get errorValue() {
    if (this.isOk) {
      throw new Error("Tried to access errorValue of Result object when result is ok", {
        cause: this,
      });
    }
    return this.#errorValue;
  }

  get isOk() {
    return this.#isOk;
  }

  get isError() {
    return !this.isOk;
  }

  match(matcher: ResultMatcher<OK, ERROR>) {
    if (this.isOk) {
      matcher.ok(this.#okValue as OK);
    } else {
      matcher.error(this.#errorValue as ERROR);
    }
  }

  okOrDefault(value: OK): OK {
    if (this.isOk) {
      return this.#okValue as OK;
    }

    return value;
  }

  okOrThrow(message: string): OK {
    if (this.isOk) {
      return this.#okValue as OK;
    }

    throw new Error(message, {
      cause: this,
    });
  }
}
