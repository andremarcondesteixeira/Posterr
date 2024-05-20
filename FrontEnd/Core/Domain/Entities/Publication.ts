export type Publication = {
  postId: number;
  authorUsername: string;
  content: string;
  publicationDate: Date;
  repostId?: number;
  reposterUsername?: string;
  repostDate?: Date;
}
