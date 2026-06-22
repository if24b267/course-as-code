export type ActivityType = 'Forum' | 'Page' | 'Quiz' | 'Assignment' | 'Url' | 'File' | 'Label' | 'Folder' | 'Wiki' | 'Choice' | 'Feedback';

const PREFIXES: Record<ActivityType, string> = {
  Forum: 'Forum_',
  Page: 'PGE_',
  Quiz: 'QUIZ_',
  Assignment: 'ASN_',
  Url: 'URL_',
  File: 'FILE_',
  Label: 'TMA_',
  Folder: 'FLD_',
  Wiki: 'WKI_',
  Choice: 'CHO_',
  Feedback: 'FBK_',
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

const CHOICE_TEMPLATE = [
  'Type: CHOICE',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# Publish: false',
  '# ShowResults: false',
  '# Display: 0  # 0 = horizontal, 1 = vertical',
  '# AllowUpdate: false',
  'AllowMultiple: false',
  '# ShowUnanswered: true',
  '# LimitAnswers: false',
  '# TimeOpen: "01-01-2026 00:00"   # dd-MM-yyyy HH:mm',
  '# TimeClose: "31-12-2026 23:59"  # dd-MM-yyyy HH:mm',
  '# CompletionSubmit: false',
  'Options:',
  '  - Text: "Option 1"',
  '    MaxAnswers: 0  # 0 = unlimited; only used when LimitAnswers is true',
  '  - Text: "Option 2"',
  '    MaxAnswers: 0',
  '',
].join('\n');

const FEEDBACK_TEMPLATE = [
  'Type: FEEDBACK',
  'Name: ""',
  '# Description: ""',
  '# DescriptionFormat: HTML  # MoodleAutoFormat | HTML | PlainText | Markdown',
  '# Anonymous: 2  # 1 = identified, 2 = anonymous',
  '# EmailNotification: 0',
  '# MultipleSubmit: 0',
  '# Autonumbering: 0',
  '# PublishStats: 1',
  '# CompletionSubmit: false',
  'Items:',
  '  - Template: 0',
  '    Name: "Your question here?"',
  '    Type: multichoice  # multichoice | textfield | textarea | numeric | label | info',
  '    Presentation: "r>>>>>1|2|3|4<<<<<1"  # Moodle item encoding',
  '    HasValue: 1',
  '    Position: 1',
  '    Required: 0',
  '    DependItem: 0',
  '    Options: h',
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
  Choice: CHOICE_TEMPLATE,
  Feedback: FEEDBACK_TEMPLATE,
};

export function getTemplate(type: ActivityType): string {
  return TEMPLATES[type];
}
