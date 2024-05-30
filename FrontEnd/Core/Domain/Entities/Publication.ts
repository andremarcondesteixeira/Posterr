export type Publication = {
  isRepost: boolean;
  postId: number;
  postAuthorUsername: string;
  postPublicationDate: string;
  postContent: string;
  repostAuthorUsername?: string | null;
  repostPublicationDate?: string | null;
}
