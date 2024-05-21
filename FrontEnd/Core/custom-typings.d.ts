declare module "node:test" {
  let suite: (description: string, block: () => void) => void;
}
