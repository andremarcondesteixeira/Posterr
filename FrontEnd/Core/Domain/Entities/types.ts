export type Publication = {
  isRepost: boolean;
  id: number;
  authorUsername: string;
  publicationDate: string;
  content: string;
  originalPostAuthorUsername?: string;
  originalPostPublicationDate?: string;
  originalPostContent?: string;
};

export type Author = {
  id: number;
  username: string;
};
