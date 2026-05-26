export type ActivityType = 'Forum' | 'Page' | 'Quiz' | 'Assignment';

const PREFIXES: Record<ActivityType, string> = {
  Forum: 'Forum_',
  Page: 'Page_',
  Quiz: 'Quiz_',
  Assignment: 'Assignment_',
};

export function sanitizeName(name: string): string {
  return name
    .toLowerCase()
    .replace(/\s+/g, '-')
    .replace(/[^a-z0-9_-]/g, '');
}

export function buildFileName(type: ActivityType, name: string): string {
  return `${PREFIXES[type]}${sanitizeName(name)}.yaml`;
}

const FORUM_TEMPLATE = `Type: FORUM\nName: ""\n`;

const PAGE_TEMPLATE = `Type: PAGE\nName: ""\nContent: |\n  <p>Page content goes here.</p>\n`;

const QUIZ_TEMPLATE = `Type: QUIZ\nName: ""\nQuestions: []\n`;

const ASSIGNMENT_TEMPLATE = `Type: ASSIGNMENT\nName: ""\n`;

const TEMPLATES: Record<ActivityType, string> = {
  Forum: FORUM_TEMPLATE,
  Page: PAGE_TEMPLATE,
  Quiz: QUIZ_TEMPLATE,
  Assignment: ASSIGNMENT_TEMPLATE,
};

export function getTemplate(type: ActivityType): string {
  return TEMPLATES[type];
}
