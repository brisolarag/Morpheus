export interface CVSuggestion {
  id: string;
  textRef: string;
  suggestion: string;
  isAccepted?: boolean;
}

export interface CV {
  id: string;
  name: string;
  updatedAt: string;
  content: string;
  suggestions: CVSuggestion[];
}
