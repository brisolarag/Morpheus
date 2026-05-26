export interface Job {
  id: string;
  title: string;
  company: string;
  companyLogo: string;
  location: string;
  seniorityLevel: string;
  contractType: string;
  externalPlatform: string;
  applyUrl: string;
  publishedAt: string;
  relevanceScore: number;
}

export interface JobSearchResponse {
  searchQuery: string;
  resultsCount: number;
  results: Job[];
}
