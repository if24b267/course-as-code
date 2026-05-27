export type ActivityType = 'Forum' | 'Page' | 'Quiz' | 'Assignment' | 'Url' | 'File' | 'Label' | 'Folder' | 'Wiki';

const PREFIXES: Record<ActivityType, string> = {
  Forum: 'Forum_',
  Page: 'Page_',
  Quiz: 'Quiz_',
  Assignment: 'Assignment_',
  Url: 'URL_',
  File: 'FILE_',
  Label: 'TMA_',
  Folder: 'FLD_',
  Wiki: 'WKI_',
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

const FORUM_TEMPLATE = [
  'Type: FORUM',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'ForumType: General  # General | Blog | Single | EachUser | QAndA',
  '# Intro: ""',
  '# SubscriptionMode: Optional  # Optional | Forced | Auto | Disabled',
  '# TrackingType: Optional  # Off | Optional | Forced',
  '# MaxAttachments: 9',
  '# MaxBytes: 0',
  '# DueDate: 0  # Unix timestamp',
  '# CutoffDate: 0  # Unix timestamp',
  '# CompletionDiscussions: 0',
  '# CompletionReplies: 0',
  '# CompletionPosts: 0',
  '# GradeForum: false',
  '# DisplayWordCount: false',
  '# LockDiscussionAfter: 0  # seconds; 0 = never',
  '',
].join('\n');

const PAGE_TEMPLATE = [
  'Type: PAGE',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# IntroFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'Content: |',
  '  <p>Page content goes here.</p>',
  '# ContentFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '',
].join('\n');

const QUIZ_TEMPLATE = [
  'Type: QUIZ',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# OpeningTime: "01-01-2025 00:00"  # dd-MM-yyyy HH:mm',
  '# ClosingTime: "31-12-2025 23:59"  # dd-MM-yyyy HH:mm',
  '# TimeLimit: 0  # seconds; 0 = no limit',
  '# TimeExpiryHandling: AutoSubmit  # AutoSubmit | GracePeriod | DoNotCount',
  '# AllowedAttempts: 0  # 0 = unlimited',
  '# AllowRedosDuringAttempt: false',
  '# BuildOnPreviousAttempts: false',
  '# GradingMethod: HighestGrade  # HighestGrade | AverageGrade | FirstAttempt | LastAttempt',
  '# GradeToPass: 0',
  '# QuestionBehaviour: Deferred  # Adaptive | AdaptiveNoPenalties | Deferred | DeferredCBM | Immediate | ImmediateCBM | InteractiveMultipleTries',
  '# ShuffleQuestions: false',
  '# Password: ""',
  'Questions:',
  '  - Ref: ""',
  '',
].join('\n');

const ASSIGNMENT_TEMPLATE = [
  'Type: ASSIGNMENT',
  'Name: ""',
  'Intro: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# AssignmentTextFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# DueDate: null',
  '# CutoffDate: null',
  '# AllowSubmissionsFromDate: null',
  '# GradingDueDate: null',
  '# Grade: 100',
  '# AllowSubmissionDrafts: false',
  '# SendStudentNotifications: true',
  '# SendNotifications: false',
  '# SendLateNotifications: false',
  '# CompletionSubmit: false',
  '# RequireSubmissionStatement: false',
  '# AllowTeamSubmissions: false',
  '# RequireAllTeamMembersSubmit: false',
  '# BlindMarking: false',
  '# HideGrader: false',
  '# MarkingWorkflow: false',
  '# MarkingAllocation: false',
  '',
].join('\n');

const URL_TEMPLATE = [
  'Type: URL',
  'Name: ""',
  'URL: ""',
  '# Description: ""',
  '',
].join('\n');

const FILE_TEMPLATE = [
  'Type: FILE',
  'Name: ""',
  'FilePath: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '',
].join('\n');

const LABEL_TEMPLATE = [
  'Type: LABEL',
  'Name: ""',
  '# Description: ""',
  'Format: MARKDOWN  # AUTO | HTML | PLAIN | MARKDOWN',
  'Content:',
  '  - Type: MARKDOWN',
  '    ContentFilePath: ./content/',
  '',
].join('\n');

const FOLDER_TEMPLATE = [
  'Type: FOLDER',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'VisibleToStudents: true',
  'DisplayDescription: false',
  'YamlFilePaths: []',
  '',
].join('\n');

const WIKI_TEMPLATE = [
  'Type: WIKI',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# Intro: ""',
  '# IntroFormat: MoodleAutoFormat  # MoodleAutoFormat | HTML | PlainText | Markdown',
  'FirstPageTitle: Overview',
  'WikiMode: Collaborative  # Collaborative | Individual',
  '# DefaultFormat: Html  # Html | Creole | Nwiki',
  '# ForceFormat: false',
  '# EditBegin: 0  # Unix timestamp; 0 = always open',
  '# EditEnd: 0    # Unix timestamp; 0 = always open',
  '',
].join('\n');

const TEMPLATES: Record<ActivityType, string> = {
  Forum: FORUM_TEMPLATE,
  Page: PAGE_TEMPLATE,
  Quiz: QUIZ_TEMPLATE,
  Assignment: ASSIGNMENT_TEMPLATE,
  Url: URL_TEMPLATE,
  File: FILE_TEMPLATE,
  Label: LABEL_TEMPLATE,
  Folder: FOLDER_TEMPLATE,
  Wiki: WIKI_TEMPLATE,
};

export function getTemplate(type: ActivityType): string {
  return TEMPLATES[type];
}
